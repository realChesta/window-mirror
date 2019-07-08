using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using window_mirror.lib;

namespace window_mirror
{
    public partial class ListForm : Form
    {
        private ListViewColumnSorter sorter;
        private Dictionary<int, ListViewItem> currentItems = new Dictionary<int, ListViewItem>();
        private List<int> garbageHandles = new List<int>();

        public ListForm()
        {
            InitializeComponent();
        }

        private void ListForm_Load(object sender, EventArgs e)
        {
            windowListView.EnableSearch = true;
            sorter = new ListViewColumnSorter();
            windowListView.ListViewItemSorter = sorter;
            sorter.Order = SortOrder.Descending;
            sorter.Column = 2;

            refreshTimer.Start();

            Thread refreshThread = new Thread(new ThreadStart(refreshThreadWork)) { Name = "refreshThread", IsBackground = true };
            refreshThread.Start();
        }

        private void windowListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            sorter.Column = e.Column;
            if (sorter.Order == SortOrder.Ascending)
            {
                sorter.Order = SortOrder.Descending;
            }
            else
            {
                sorter.Order = SortOrder.Ascending;
            }
            this.windowListView.Sort();
        }


        private void refreshThreadWork()
        {
            while (true)
            {
                refreshItems();
                Thread.Sleep(500);
            }
        }

        private void refreshItems()
        {
            DesktopWindow[] currentWindows = DesktopWindow.getCurrentWindows();
            List<int> currentHandles = currentWindows.Select(w => w.Handle.ToInt32()).ToList();

            foreach (DesktopWindow window in currentWindows)
            {
                ListViewItem item;
                int handle = window.Handle.ToInt32();

                if (hideWindowInList(window))
                    continue;

                if (currentItems.ContainsKey(handle))
                {
                    item = currentItems[handle];
                }
                else
                {
                    item = new ListViewItem(new string[3]);
                    currentItems.Add(handle, item);
                }

                this.InvokeEx(() =>
                {
                    item.SubItems[0].Text = window.Caption;
                    item.SubItems[1].Text = window.Style.ToString();
                    item.SubItems[2].Text = window.Visible.ToString();
                    item.Tag = handle;
                    item.ImageKey = window.Visible ? "window-visible" : "window-hidden";
                });
            }

            foreach (KeyValuePair<int, ListViewItem> item in currentItems)
            {
                if (!currentHandles.Contains(item.Key))
                {
                    garbageHandles.Add(item.Key);
                    this.InvokeEx(f => f.windowListView.Items.Remove(item.Value));
                    continue;
                }

                this.InvokeEx(f =>
                {
                    if (!f.windowListView.Items.Contains(item.Value))
                        f.windowListView.Items.Add(item.Value);
                });
            }
            garbageHandles.ForEach(gh => currentItems.Remove(gh));
            garbageHandles.Clear();
        }

        private bool hideWindowInList(DesktopWindow window)
        {
            return (String.IsNullOrWhiteSpace(window.Caption) || 
                this.InvokeEx(f => !f.hiddenCheckBox.Checked && !window.Visible));
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                switch (keyData)
                {
                    case (Keys.Control | Keys.F):
                        windowListView.SearchPopup.ToggleSearch();
                        break;
                }
            }
            catch { }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void hiddenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            hiddenCheckBox.Enabled = false;
        }

        private void windowListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && windowListView.SelectedItems.Count > 0)
            {
                this.mirrorSelectedItems();
            }
        }

        private void mirrorSelectedItems()
        {
            foreach (ListViewItem item in windowListView.SelectedItems)
            {
                try
                {
                    int handle = (int)item.Tag;
                    MirrorForm mirror = new MirrorForm(new DesktopWindow(handle), (int)fpsUpDown.Value, MirrorForm.DisplayMode.Window);
                    mirror.Show();
                }
                catch { }
            }
        }

        private void windowListView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && windowListView.SelectedItems.Count > 0)
            {
                listMenuStrip.Show(windowListView, e.X, e.Y);
            }
        }

        private void mirrorSelectedWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mirrorSelectedItems();
        }
    }
}
