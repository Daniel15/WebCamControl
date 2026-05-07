// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>
// https://d.sb/wcc

using Gio;
using Gtk;

namespace WebCamControl.Gtk.Widgets;

/// <summary>
/// Represents a submenu that shows a list of radio buttons.
/// </summary>
public class RadioButtonSubmenu<T> : IDisposable where T : IListItem
{
	private readonly ApplicationWindow _window;
	private readonly SimpleAction _action;
	private Dictionary<string, T> _items = new();
	
	public RadioButtonSubmenu(
		string actionName,
		ApplicationWindow window
		
	)
	{
		_window = window;
		_action = SimpleAction.NewStateful(
			actionName,
			GLib.VariantType.String,
			GLib.Variant.NewString("")
		);
		_action.OnActivate += OnChangeSelectedItem;
		_window.AddAction(_action);

		Menu = Menu.New();
	}

	/// <summary>
	/// Occurs when the selected item is changed.
	/// </summary>
	public event EventHandler<T?>? SelectionChanged;

	/// <summary>
	/// Gets or sets the list of items that are displayed in the menu.
	/// </summary>
	public IEnumerable<T> Items
	{
		get => _items.Values;
		set
		{
			Menu.RemoveAll();
			
			var actionName = _action.GetName();
			_items = [];
			foreach (var item in value)
			{
				var key = CalculateKey(item);
				var menuItem = MenuItem.New(item.Label, null);
				menuItem.SetActionAndTargetValue(
					$"win.{actionName}", 
					GLib.Variant.NewString(key)
				);
				_items.Add(key, item);
				Menu.AppendItem(menuItem);
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
			var key = _action.GetState()?.GetString(out _);
			return key is null ? default : _items[key];
		}
		set => _action.SetState(
			GLib.Variant.NewString(value is null ? string.Empty : CalculateKey(value))
		);
	}
	
	/// <summary>
	/// Gets the <see cref="Menu"/> that contains the radio buttons.
	/// </summary>
	public Menu Menu { get; }

	/// <summary>
	/// Handle when the selected item is changed.
	/// </summary>
	private void OnChangeSelectedItem(SimpleAction sender, SimpleAction.ActivateSignalArgs args)
	{
		var key = args.Parameter?.GetString(out _);
		var item = key is null ? default : _items[key];
		SelectionChanged?.Invoke(this, item);
	}

	/// <summary>
	/// Calculates a unique key for this item.
	/// </summary>
	private static string CalculateKey(T item) => $"{item.GetHashCode()}-{item.Label}";
	
	public void Dispose()
	{
		_window.RemoveAction(_action.GetName());
		_action.OnActivate -= OnChangeSelectedItem;
	}
}
