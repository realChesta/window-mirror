using System;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace window_mirror.lib
{
    /// <summary>
    /// Zusammenfassung für ListViewEx.
    /// </summary>
    public class ListViewEx : ListViewNF
    {
        public new ObservableListViewItemCollection Items;
        public SearchPopup SearchPopup;
        public bool EnableSearch
        {
            get
            {
                return (SearchPopup != null);
            }
            set
            {
                if (value)
                {
                    if (SearchPopup == null)
                    {
                        this.InvokeEx(() =>
                        {
                            SearchPopup = new SearchPopup(this, SearchPopup.SearchMode.RemoveNonMatchingItems);
                            try
                            {
                                Items.CountChanged += Items_CountChanged;
                            }
                            catch { }
                        });
                    }
                }
                else
                {
                    this.InvokeEx(() =>
                    {
                        try
                        {
                            if (SearchPopup != null)
                            {
                                SearchPopup.CloseSearch();
                                SearchPopup = null;
                                Items.CountChanged -= Items_CountChanged;
                            }
                        }
                        catch
                        { }
                    });
                }
            }
        }

        #region Interop-Defines
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wPar, IntPtr lPar);

        // ListView messages
        private const int LVM_FIRST = 0x1000;
        private const int LVM_GETCOLUMNORDERARRAY = (LVM_FIRST + 59);

        // Windows Messages
        private const int WM_PAINT = 0x000F;
        #endregion

        /// <summary>
        /// Structure to hold an embedded control's info
        /// </summary>
        private struct EmbeddedControl
        {
            public Control Control;
            public int Column;
            public int Row;
            public DockStyle Dock;
            public ListViewItem Item;
        }

        private ArrayList _embeddedControls = new ArrayList();

        public ListViewEx() : this(false) { }
        public ListViewEx(bool enableSearch = false)
        {
            Items = new ObservableListViewItemCollection(this);

            this.EnableSearch = enableSearch;
        }

        private void Items_CountChanged(ListViewEx sender, int count, ObservableListViewItemCollection.Action action)
        {
            SearchPopup.UpdateOrigLVIs(Items.OfType<ListViewItem>());
        }

        /// <summary>
        /// Retrieve the order in which columns appear
        /// </summary>
        /// <returns>Current display order of column indices</returns>
        protected int[] GetColumnOrder()
        {
            IntPtr lPar = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * Columns.Count);

            IntPtr res = SendMessage(Handle, LVM_GETCOLUMNORDERARRAY, new IntPtr(Columns.Count), lPar);
            if (res.ToInt32() == 0) // Something went wrong
            {
                Marshal.FreeHGlobal(lPar);
                return null;
            }

            int[] order = new int[Columns.Count];
            Marshal.Copy(lPar, order, 0, Columns.Count);

            Marshal.FreeHGlobal(lPar);

            return order;
        }

        /// <summary>
        /// Retrieve the bounds of a ListViewSubItem
        /// </summary>
        /// <param name="Item">The Item containing the SubItem</param>
        /// <param name="SubItem">Index of the SubItem</param>
        /// <returns>Subitem's bounds</returns>
        protected Rectangle GetSubItemBounds(ListViewItem Item, int SubItem)
        {
            Rectangle subItemRect = Rectangle.Empty;

            if (Item == null)
                throw new ArgumentNullException("Item");

            int[] order = GetColumnOrder();
            if (order == null) // No Columns
                return subItemRect;

            if (SubItem >= order.Length)
                throw new IndexOutOfRangeException("SubItem " + SubItem + " out of range");

            // Retrieve the bounds of the entire ListViewItem (all subitems)
            Rectangle lviBounds = Item.GetBounds(ItemBoundsPortion.Entire);
            int subItemX = lviBounds.Left;

            // Calculate the X position of the SubItem.
            // Because the columns can be reordered we have to use Columns[order[i]] instead of Columns[i] !
            ColumnHeader col;
            int i;
            for (i = 0; i < order.Length; i++)
            {
                col = this.Columns[order[i]];
                if (col.Index == SubItem)
                    break;
                subItemX += col.Width;
            }

            subItemRect = new Rectangle(subItemX, lviBounds.Top, this.Columns[order[i]].Width, lviBounds.Height);

            return subItemRect;
        }

        /// <summary>
        /// Add a control to the ListView
        /// </summary>
        /// <param name="c">Control to be added</param>
        /// <param name="col">Index of column</param>
        /// <param name="row">Index of row</param>
        public void AddEmbeddedControl(Control c, int col, int row)
        {
            AddEmbeddedControl(c, col, row, DockStyle.Fill);
        }
        /// <summary>
        /// Add a control to the ListView
        /// </summary>
        /// <param name="c">Control to be added</param>
        /// <param name="col">Index of column</param>
        /// <param name="row">Index of row</param>
        /// <param name="dock">Location and resize behavior of embedded control</param>
        public void AddEmbeddedControl(Control c, int col, int row, DockStyle dock)
        {
            if (c == null)
                throw new ArgumentNullException();
            if (col >= Columns.Count || row >= Items.Count)
                throw new ArgumentOutOfRangeException();

            EmbeddedControl ec;
            ec.Control = c;
            ec.Column = col;
            ec.Row = row;
            ec.Dock = dock;
            ec.Item = Items[row];

            _embeddedControls.Add(ec);

            // Add a Click event handler to select the ListView row when an embedded control is clicked
            c.Click += new EventHandler(_embeddedControl_Click);

            this.Controls.Add(c);
        }

        /// <summary>
        /// Remove a control from the ListView
        /// </summary>
        /// <param name="c">Control to be removed</param>
        public void RemoveEmbeddedControl(Control c)
        {
            if (c == null)
                throw new ArgumentNullException();

            for (int i = 0; i < _embeddedControls.Count; i++)
            {
                EmbeddedControl ec = (EmbeddedControl)_embeddedControls[i];
                if (ec.Control == c)
                {
                    c.Click -= new EventHandler(_embeddedControl_Click);
                    this.Controls.Remove(c);
                    _embeddedControls.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Control not found!");
        }

        /// <summary>
        /// Retrieve the control embedded at a given location
        /// </summary>
        /// <param name="col">Index of Column</param>
        /// <param name="row">Index of Row</param>
        /// <returns>Control found at given location or null if none assigned.</returns>
        public Control GetEmbeddedControl(int col, int row)
        {
            foreach (EmbeddedControl ec in _embeddedControls)
                if (ec.Row == row && ec.Column == col)
                    return ec.Control;

            return null;
        }

        [DefaultValue(View.LargeIcon)]
        public new View View
        {
            get
            {
                return base.View;
            }
            set
            {
                // Embedded controls are rendered only when we're in Details mode
                foreach (EmbeddedControl ec in _embeddedControls)
                    ec.Control.Visible = (value == View.Details);

                base.View = value;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_PAINT:
                    if (View != View.Details)
                        break;

                    // Calculate the position of all embedded controls
                    foreach (EmbeddedControl ec in _embeddedControls)
                    {
                        Rectangle rc = this.GetSubItemBounds(ec.Item, ec.Column);

                        if ((this.HeaderStyle != ColumnHeaderStyle.None) &&
                            (rc.Top < this.Font.Height)) // Control overlaps ColumnHeader
                        {
                            ec.Control.Visible = false;
                            continue;
                        }
                        else
                        {
                            ec.Control.Visible = true;
                        }

                        switch (ec.Dock)
                        {
                            case DockStyle.Fill:
                                break;
                            case DockStyle.Top:
                                rc.Height = ec.Control.Height;
                                break;
                            case DockStyle.Left:
                                rc.Width = ec.Control.Width;
                                break;
                            case DockStyle.Bottom:
                                rc.Offset(0, rc.Height - ec.Control.Height);
                                rc.Height = ec.Control.Height;
                                break;
                            case DockStyle.Right:
                                rc.Offset(rc.Width - ec.Control.Width, 0);
                                rc.Width = ec.Control.Width;
                                break;
                            case DockStyle.None:
                                rc.Size = ec.Control.Size;
                                break;
                        }

                        // Set embedded control's bounds
                        ec.Control.Bounds = rc;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void _embeddedControl_Click(object sender, EventArgs e)
        {
            // When a control is clicked the ListViewItem holding it is selected
            foreach (EmbeddedControl ec in _embeddedControls)
            {
                if (ec.Control == (Control)sender)
                {
                    this.SelectedItems.Clear();
                    ec.Item.Selected = true;
                }
            }
        }
    }

    public class ListViewNF : System.Windows.Forms.ListView
    {
        public ListViewNF()
        {
            //Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }


        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }

    public class ObservableListViewItemCollection : ListView.ListViewItemCollection
    {
        public delegate void CountChangedHandler(ListViewEx sender, int count, Action action);
        public event CountChangedHandler CountChanged;
        private ListViewEx Owner;

        new public int Count
        {
            get
            {
                return base.Count;
            }
        }

        public ObservableListViewItemCollection(ListViewEx owner) : base(owner) { this.Owner = owner; }

        public override ListViewItem Add(ListViewItem value)
        {
            var item = base.Add(value);
            OnChanged(Action.Add);
            return item;
        }

        public ListViewItem SilentAdd(ListViewItem value)
        {
            return base.Add(value);
        }

        public override ListViewItem Add(string key, string text, int imageIndex)
        {
            var item = base.Add(key, text, imageIndex);
            OnChanged(Action.Add);
            return item;
        }

        public override ListViewItem Add(string key, string text, string imageKey)
        {
            var item = base.Add(key, text, imageKey);
            OnChanged(Action.Add);
            return item;
        }

        public override ListViewItem Add(string text)
        {
            var item = base.Add(text);
            OnChanged(Action.Add);
            return item;
        }

        public override ListViewItem Add(string text, int imageIndex)
        {
            var item = base.Add(text, imageIndex);
            OnChanged(Action.Add);
            return item;
        }

        public override ListViewItem Add(string text, string imageKey)
        {
            var item = base.Add(text, imageKey);
            OnChanged(Action.Add);
            return item;
        }

        public new ListViewItem Insert(int index, ListViewItem item)
        {
            var tr = base.Insert(index, item);
            OnChanged(Action.Add);
            return tr;
        }

        public override ListViewItem Insert(int index, string key, string text, int imageIndex)
        {
            var item = base.Insert(index, key, text, imageIndex);
            OnChanged(Action.Add);
            return item;
        }

        public override ListViewItem Insert(int index, string key, string text, string imageKey)
        {
            var item = base.Insert(index, key, text, imageKey);
            OnChanged(Action.Add);
            return item;
        }

        public override void Remove(ListViewItem item)
        {
            base.Remove(item);
            OnChanged(Action.Remove);
        }

        public override void RemoveAt(int index)
        {
            base.RemoveAt(index);
            OnChanged(Action.Remove);
        }

        public override void RemoveByKey(string key)
        {
            base.RemoveByKey(key);
            OnChanged(Action.Remove);
        }

        public void SilentRemove(ListViewItem item)
        {
            base.Remove(item);
        }

        public override void Clear()
        {
            base.Clear();
            OnChanged(Action.Clear);
        }

        public void SilentClear()
        {
            base.Clear();
        }

        new public void AddRange(ListViewItem[] items)
        {
            base.AddRange(items);
            OnChanged(Action.AddRange);
        }

        public void SilentAddRange(ListViewItem[] items)
        {
            base.AddRange(items);
        }

        public override bool ContainsKey(string key)
        {
            return base.ContainsKey(key);
        }

        public new bool Contains(ListViewItem item)
        {
            return base.Contains(item);
        }

        public override int IndexOfKey(string key)
        {
            return base.IndexOfKey(key);
        }

        public override ListViewItem this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
            }
        }

        public override ListViewItem this[string key]
        {
            get
            {
                return base[key];
            }
        }

        private void OnChanged(Action a)
        {
            if (this.CountChanged != null)
            {
                this.CountChanged(this.Owner, this.Count, a);
            }
        }

        public enum Action
        {
            Add,
            AddRange,
            Remove,
            Clear
        }
    }
}
