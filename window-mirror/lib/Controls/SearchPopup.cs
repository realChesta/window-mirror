using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using window_mirror.Properties;

namespace window_mirror.lib
{
    public partial class SearchPopup : Form
    {
        public readonly ListViewEx SearchListView;
        public SearchMode Mode;
        private ListViewItem[] OrigLVIs;
        private ListViewGroup SearchGroup = new ListViewGroup("seach", "Search");
        private ListViewGroup UnclassedGroup = new ListViewGroup("uncl", "Unclassified");
        public delegate void OrigLVIsUpdatedHandler(SearchPopup sender, long count);
        public event OrigLVIsUpdatedHandler OrigLVIsUpdated;

        public SearchPopup(ListViewEx lv, SearchMode mode)
        {
            InitializeComponent();

            this.SearchListView = lv;
            this.Mode = mode;
            this.TopLevel = false;
            //this.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            this.SearchListView.Controls.Add(this);
            this.Height = GetCorrectHeight();
            this.Location = new Point(SearchListView.ClientRectangle.Width - this.Size.Width, SearchListView.ClientRectangle.Height - this.Size.Height);
            this.SearchListView.Resize += SearchListView_Resize;
            this.searchBox.ClientSizeChanged += SearchListView_Resize;
            this.Resize += SearchListView_Resize_test;
            this.OrigLVIs = SearchListView.Items.OfType<ListViewItem>().ToArray();

            filterListView.Items.Clear();
            foreach (ColumnHeader header in SearchListView.Columns)
            {
                filterListView.Items.Add(new ListViewItem(header.Text) { Tag = header.Index, Checked = true });
            }
        }

        private void SearchListView_Resize_test(object sender, EventArgs e)
        {
            SearchListView_Resize(sender, e);
        }

        #region Expand-logic
        private bool Expanded = false;

        private void SearchListView_Resize(object sender, EventArgs e)
        {
            this.Location = new Point(SearchListView.ClientRectangle.Width - this.Size.Width, SearchListView.ClientRectangle.Height - this.Size.Height);
        }

        private void expandButton_Click(object sender, EventArgs e)
        {
            Expanded = !Expanded;
            this.Height = GetCorrectHeight();
            expandButton.Image = Expanded ? Resources.arrow_down_medium : Resources.arrow_up_medium;
        }

        private int GetCorrectHeight()
        {
            if (Expanded)
            {
                return 117; //315; 117
            }
            else
            {
                return (searchBox.Location.Y * 2) + searchBox.Height;
            }
        }

        #endregion

        public enum SearchMode
        { 
            RemoveNonMatchingItems,
            GroupItems
        }

        public void FocusTextBox()
        { searchBox.Focus(); }

        public void CloseSearch()
        {
            this.searchBox.Text = "";
            this.Hide();
            SearchListView.Focus();
        }

        public void ShowSearch()
        {
            this.Show();
            searchBox.Focus();
            searchBox.SelectAll();
        }

        public bool ToggleSearch()
        {
            if (this.Visible && (this.Focused || searchBox.Focused))
            { this.CloseSearch(); }
            else
            { this.ShowSearch(); }

            return this.Visible;
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            string pattern = searchBox.Text.ToLower();
            int[] columns;
            List<int> tempClmns = new List<int>();
            foreach (ListViewItem item in filterListView.Items)
            {
                if (item.Checked || !this.Expanded)
                {
                    tempClmns.Add((int)item.Tag);
                }
            }
            columns = tempClmns.ToArray();

            SearchListView.BeginUpdate();

            switch (Mode)
            {
                case SearchMode.RemoveNonMatchingItems:
                    {
                        
                        if (string.IsNullOrEmpty(pattern))
                        {
                            SearchListView.Items.SilentClear();
                            SearchListView.Items.SilentAddRange(OrigLVIs);
                            break;
                        }
                        List<ListViewItem> toRemove = new List<ListViewItem>();
                        List<ListViewItem> toAdd = new List<ListViewItem>();
                        List<ListViewItem> alreadyThere = new List<ListViewItem>();
                        bool found = false;
                        for (int i = 0; i < OrigLVIs.Length; i++)
                        {
                            for (int j = 0; j < columns.Length; j++)
                            {
                                if ((OrigLVIs[i].SubItems.Count > columns[j]) && (OrigLVIs[i].SubItems[columns[j]] != null) && (OrigLVIs[i].SubItems[columns[j]].Text.ToLower().Contains(pattern)))
                                {
                                    if (!SearchListView.Items.Contains(OrigLVIs[i]))
                                    {
                                        //SearchListView.Items.Add(OrigLVIs[i]);
                                        toAdd.Add(OrigLVIs[i]);
                                    }
                                    else
                                    { alreadyThere.Add(OrigLVIs[i]); }
                                    found = true;
                                    break;
                                }
                            }
                            if (!found && SearchListView.Items.Contains(OrigLVIs[i]))
                            {
                                toRemove.Add(OrigLVIs[i]);
                                //SearchListView.Items.Remove(OrigLVIs[i]);
                            }
                            found = false;
                        }

                        if (toRemove.Count > (toAdd.Count + alreadyThere.Count))
                        {
                            SearchListView.Items.SilentClear();
                            List<ListViewItem> all = new List<ListViewItem>(alreadyThere);
                            all.AddRange(toAdd);
                            SearchListView.Items.SilentAddRange(all.ToArray());
                        }
                        else
                        {
                            for (int i = 0; i < toRemove.Count; i++)
                            {
                                SearchListView.Items.SilentRemove(toRemove[i]);
                            }
                            SearchListView.Items.SilentAddRange(toAdd.ToArray());
                        }

                        toRemove.Clear();
                        toAdd.Clear();
                        alreadyThere.Clear();

                    }
                    break;

                case SearchMode.GroupItems:
                    {
                        if (!SearchListView.Groups.Contains(SearchGroup))
                            SearchListView.Groups.Add(SearchGroup);
                        if (!SearchListView.Groups.Contains(UnclassedGroup))
                            SearchListView.Groups.Add(UnclassedGroup);

                        if (string.IsNullOrEmpty(pattern))
                        {
                            SearchListView.ShowGroups = false;
                            for (int i = 0; i < SearchListView.Items.Count; i++)
                            {
                                SearchListView.Items[i].Group = null;
                            }
                            break;
                        }
                        else
                        { SearchListView.ShowGroups = true; }

                        bool found = false;
                        for (int i = 0; i < SearchListView.Items.Count; i++)
                        {
                            for (int j = 0; j < columns.Length; j++)
                            {
                                if ((SearchListView.Items[i].SubItems.Count > j) && (SearchListView.Items[i].SubItems[j] != null) && (SearchListView.Items[i].SubItems[j].Text.ToLower().Contains(pattern)))
                                {
                                    SearchListView.Items[i].Group = SearchGroup;
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                            { found = false; }
                            else
                            { SearchListView.Items[i].Group = UnclassedGroup; }
                        }
                    }
                    break;
            }

            SearchListView.EndUpdate();
        }

        public void UpdateOrigLVIs(IEnumerable<ListViewItem> lvis, bool updateListView = true)
        {
            OrigLVIs = lvis.ToArray();

            OnOrigLVIsUpdate();

            if (updateListView)
            {
                searchBox_TextChanged(null, null);
            }
        }

        private void OnOrigLVIsUpdate()
        {
            if (this.OrigLVIsUpdated != null)
            {
                this.OrigLVIsUpdated(this, this.OrigLVIs.LongLength);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                this.ToggleSearch();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                this.CloseSearch();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
