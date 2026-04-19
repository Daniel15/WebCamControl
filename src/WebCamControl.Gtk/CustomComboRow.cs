// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2025 Daniel Lo Nigro <d@d.sb>

using Adw;
using Gtk;

namespace WebCamControl.Gtk;

/// <summary>
/// Wrapper around <see cref="ComboRow"/> that allows C# types to be used as items.
/// </summary>
/// <typeparam name="T">Type of item</typeparam>
public class CustomComboRow<T>
{
	private readonly ComboRow _comboRow;
	private T[] _items = [];
	private Dictionary<string, T> _labelToItem = new();

	public CustomComboRow(ComboRow comboRow)
	{
		_comboRow = comboRow;
		GObject.Object.NotifySignal.Connect(
			_comboRow,
			(_, _) => OnSelectedItemChanged?.Invoke(this, EventArgs.Empty),
			detail: ComboRow.SelectedPropertyDefinition.UnmanagedName
		);
	}

	/// <summary>
	/// Raised when the selected item changes.
	/// </summary>
	public event EventHandler? OnSelectedItemChanged;

	/// <summary>
	/// Gets or sets a callback to get the label for the specified item.
	/// </summary>
	public Func<T, string> LabelCallback { get; set; } = item => item.ToString();

	/// <summary>
	/// Gets or sets the list of items to show
	/// </summary>
	/// <exception cref="ArgumentException">Thrown if multiple items have the same label</exception>
	public IEnumerable<T> Items
	{
		set
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
	}

	/// <summary>
	/// Gets the currently selected item.
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
			var index = (uint)Array.FindIndex(_items, item => Equals(item, value));
			_comboRow.SetSelected(index);
		}
	}
}
