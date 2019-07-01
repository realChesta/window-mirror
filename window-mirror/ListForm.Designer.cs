namespace window_mirror
{
    partial class ListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListForm));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.hiddenCheckBox = new System.Windows.Forms.CheckBox();
            this.listMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fpsUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mirrorSelectedWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowListView = new window_mirror.lib.ListViewEx();
            this.titleHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.stateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.visibleHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "window-hidden");
            this.imageList1.Images.SetKeyName(1, "window-visible");
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(371, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Please select a window to mirror:";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 500;
            // 
            // hiddenCheckBox
            // 
            this.hiddenCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hiddenCheckBox.Location = new System.Drawing.Point(12, 353);
            this.hiddenCheckBox.Name = "hiddenCheckBox";
            this.hiddenCheckBox.Size = new System.Drawing.Size(257, 17);
            this.hiddenCheckBox.TabIndex = 4;
            this.hiddenCheckBox.Text = "Show non-visible windows";
            this.toolTip1.SetToolTip(this.hiddenCheckBox, "Check this to display hidden windows too.");
            this.hiddenCheckBox.UseVisualStyleBackColor = true;
            this.hiddenCheckBox.CheckedChanged += new System.EventHandler(this.hiddenCheckBox_CheckedChanged);
            // 
            // listMenuStrip
            // 
            this.listMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mirrorSelectedWindowsToolStripMenuItem});
            this.listMenuStrip.Name = "contextMenuStrip1";
            this.listMenuStrip.Size = new System.Drawing.Size(212, 26);
            // 
            // fpsUpDown
            // 
            this.fpsUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fpsUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fpsUpDown.Location = new System.Drawing.Point(341, 352);
            this.fpsUpDown.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.fpsUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.fpsUpDown.Name = "fpsUpDown";
            this.fpsUpDown.Size = new System.Drawing.Size(43, 20);
            this.fpsUpDown.TabIndex = 5;
            this.toolTip1.SetToolTip(this.fpsUpDown, "Sets how many FPS should be used while mirroring.\r\nHigher values will result in h" +
        "igher CPU usage.");
            this.fpsUpDown.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 355);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "FPS:";
            this.toolTip1.SetToolTip(this.label2, "Sets how many FPS should be used while mirroring.\r\nHigher values will result in h" +
        "igher CPU usage.");
            // 
            // mirrorSelectedWindowsToolStripMenuItem
            // 
            this.mirrorSelectedWindowsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mirrorSelectedWindowsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("mirrorSelectedWindowsToolStripMenuItem.Image")));
            this.mirrorSelectedWindowsToolStripMenuItem.Name = "mirrorSelectedWindowsToolStripMenuItem";
            this.mirrorSelectedWindowsToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.mirrorSelectedWindowsToolStripMenuItem.Text = "Mirror selected window(s)";
            this.mirrorSelectedWindowsToolStripMenuItem.Click += new System.EventHandler(this.mirrorSelectedWindowsToolStripMenuItem_Click);
            // 
            // windowListView
            // 
            this.windowListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windowListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleHeader,
            this.stateHeader,
            this.visibleHeader});
            this.windowListView.EnableSearch = false;
            this.windowListView.FullRowSelect = true;
            this.windowListView.HideSelection = false;
            this.windowListView.Location = new System.Drawing.Point(12, 33);
            this.windowListView.Name = "windowListView";
            this.windowListView.Size = new System.Drawing.Size(372, 313);
            this.windowListView.SmallImageList = this.imageList1;
            this.windowListView.TabIndex = 0;
            this.windowListView.UseCompatibleStateImageBehavior = false;
            this.windowListView.View = System.Windows.Forms.View.Details;
            this.windowListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.windowListView_ColumnClick);
            this.windowListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.windowListView_MouseDoubleClick);
            this.windowListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.windowListView_MouseUp);
            // 
            // titleHeader
            // 
            this.titleHeader.Text = "Title";
            this.titleHeader.Width = 219;
            // 
            // stateHeader
            // 
            this.stateHeader.Text = "State";
            this.stateHeader.Width = 71;
            // 
            // visibleHeader
            // 
            this.visibleHeader.Text = "Visible";
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 381);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fpsUpDown);
            this.Controls.Add(this.hiddenCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.windowListView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(312, 320);
            this.Name = "ListForm";
            this.Text = "Window Mirror";
            this.Load += new System.EventHandler(this.ListForm_Load);
            this.listMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private lib.ListViewEx windowListView;
        private System.Windows.Forms.ColumnHeader titleHeader;
        private System.Windows.Forms.ColumnHeader stateHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader visibleHeader;
        private System.Windows.Forms.CheckBox hiddenCheckBox;
        private System.Windows.Forms.ContextMenuStrip listMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mirrorSelectedWindowsToolStripMenuItem;
        private System.Windows.Forms.NumericUpDown fpsUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}