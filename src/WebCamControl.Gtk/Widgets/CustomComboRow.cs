// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Adw;
using Gtk;
using Object = GObject.Object;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Wrapper around <see cref="ComboRow"/> that allows C# types to be used as items, and items to be
/// disabled.
/// </summary>
/// <typeparam name="T">Type of item</typeparam>
public class CustomComboRow<T> : IDisposable where T : notnull
{
	private readonly ComboRow _comboRow;
	private T[] _items = [];
	private Dictionary<string, T> _labelToItem = new();
	private bool _shouldFireSelectionChanged = true;

	public CustomComboRow(ComboRow comboRow)
	{
		_comboRow = comboRow;
		_comboRow.OnNotify += OnComboRowNotify;
	}

	public event EventHandler? OnSelectionChanged;
	
	/// <summary>
	/// Gets or sets a callback to get the label for the specified item.
	/// </summary>
	public Func<T, string> LabelCallback { get; set; } = item => item.ToString() ?? string.Empty;

	/// <summary>
	/// Gets or sets the list of items to show 
	/// </summary>
	/// <exception cref="ArgumentException">Thrown if multiple items have the same label</exception>
	public IEnumerable<T> Items
	{
		set
		{
			// Don't call OnSelectionChanged while changing items.
			_shouldFireSelectionChanged = false;
			try
			{
				_items = value.ToArray();
				var itemCount = _items.Length;
				var labelToItem = new Dictionary<string, T>(itemCount);
				var labels = new string[itemCount];

				for (var i = 0; i < itemCount; i++)
				{
					var item = _items[i];
					var label = LabelCallback(item);
					labels[i] = label;
					if (!labelToItem.TryAdd(label, item))
					{
						throw new ArgumentException(
							$"Two items have the same label '{label}'. All items must have a unique label"
						);
					}
				}

				_comboRow.Model = StringList.New(labels);
				_labelToItem = labelToItem;
			}
			finally
			{
				GLib.Functions.IdleAdd(0, () =>
				{
					_shouldFireSelectionChanged = true;
					return false;
				});
			}
		}
	}

	/// <summary>
	/// Gets or sets the currently selected item.
	/// </summary>
	public T? SelectedItem
	{
		get
		{
			var label = (StringObject?)_comboRow.SelectedItem;
			return label?.String == null ? default : _labelToItem[label.String];
		}
		set
		{
			try
			{
				// Don't fire SelectionChanged event if we're setting it programatically.
				_shouldFireSelectionChanged = false;
				var index = Array.FindIndex(_items, item => Equals(item, value));
				if (index == -1)
				{
					throw new Exception($"Could not find {value?.ToString() ?? "<NULL>"} in list");
				}

				_comboRow.Selected = (uint)index;
			}
			finally
			{
				GLib.Functions.IdleAdd(0, () =>
				{
					_shouldFireSelectionChanged = true;
					return false;
				});
			}
		}
	}
	
	/// <summary>
	/// Handle Notify signals on the combo box
	/// </summary>
	private void OnComboRowNotify(Object _, Object.NotifySignalArgs args)
	{
		if (args.Pspec.GetName() == "selected" && _shouldFireSelectionChanged)
		{
			OnSelectionChanged?.Invoke(this, EventArgs.Empty);
		}
	}

	public void Dispose()
	{
		_comboRow.OnNotify -= OnComboRowNotify;
	}
}
