// SPDX-License-Identifier: MIT
// SPDX-FileCopyrightText: 2026 Daniel Lo Nigro <d@d.sb>

using Gtk;

namespace WebCamControl.Gtk;

internal abstract class CustomComboRow
{
	protected sealed class ComboRowItem(string label, object value)
	{
		public string Label { get; } = label;

		public object Value { get; } = value;
	}
}

internal sealed class CustomComboRow<T> : CustomComboRow where T : notnull
{
	private readonly Adw.ComboRow _comboRow;
	private ComboRowItem[] _items = [];

	public CustomComboRow(Adw.ComboRow comboRow)
	{
		_comboRow = comboRow;
		_comboRow.OnNotify += (_, _) => OnSelectionChanged?.Invoke(this, EventArgs.Empty);
	}

	public event EventHandler? OnSelectionChanged;

	public Func<T, string> LabelCallback { get; init; } = item => item?.ToString() ?? string.Empty;

	public IEnumerable<T> Items
	{
		set
		{
			_items = value
				.Select(item => new ComboRowItem(LabelCallback(item), item))
				.ToArray();
			var store = StringList.New(null);
			foreach (var item in _items)
			{
				store.Append(item.Label);
			}

			_comboRow.Model = store;
		}
	}

	public T? SelectedItem
	{
		get
		{
			var selected = _comboRow.Selected;
			return selected < _items.Length && _items[selected].Value is T value ? value : default;
		}
		set
		{
			var index = Array.FindIndex(_items, item => EqualityComparer<T>.Default.Equals((T)item.Value, value));
			if (index >= 0)
			{
				_comboRow.SetSelected((uint)index);
			}
		}
	}
}
