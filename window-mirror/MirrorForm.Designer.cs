namespace window_mirror
{
    partial class MirrorForm
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
            this.mirrorMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateBy90ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mirrorBox = new System.Windows.Forms.PictureBox();
            this.mirrorMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mirrorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mirrorMenuStrip
            // 
            this.mirrorMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullscreenToolStripMenuItem,
            this.rotateBy90ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.mirrorMenuStrip.Name = "mirrorMenuStrip";
            this.mirrorMenuStrip.Size = new System.Drawing.Size(162, 76);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(158, 6);
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.CheckOnClick = true;
            this.fullscreenToolStripMenuItem.Image = global::window_mirror.Properties.Resources.application_resize;
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.fullscreenToolStripMenuItem.Text = "Fullscreen";
            this.fullscreenToolStripMenuItem.CheckedChanged += new System.EventHandler(this.fullscreenToolStripMenuItem_CheckedChanged);
            // 
            // rotateBy90ToolStripMenuItem
            // 
            this.rotateBy90ToolStripMenuItem.Image = global::window_mirror.Properties.Resources.arrow_circle_315;
            this.rotateBy90ToolStripMenuItem.Name = "rotateBy90ToolStripMenuItem";
            this.rotateBy90ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.rotateBy90ToolStripMenuItem.Text = "Rotate by 90°";
            this.rotateBy90ToolStripMenuItem.Click += new System.EventHandler(this.rotateBy90ToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::window_mirror.Properties.Resources.cross;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.closeToolStripMenuItem.Text = "Close this mirror";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // mirrorBox
            // 
            this.mirrorBox.BackColor = System.Drawing.Color.Transparent;
            this.mirrorBox.ContextMenuStrip = this.mirrorMenuStrip;
            this.mirrorBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mirrorBox.Location = new System.Drawing.Point(0, 0);
            this.mirrorBox.Name = "mirrorBox";
            this.mirrorBox.Size = new System.Drawing.Size(256, 256);
            this.mirrorBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.mirrorBox.TabIndex = 0;
            this.mirrorBox.TabStop = false;
            this.mirrorBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MirrorForm_MouseDown);
            // 
            // MirrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(256, 256);
            this.ContextMenuStrip = this.mirrorMenuStrip;
            this.Controls.Add(this.mirrorBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MirrorForm";
            this.ShowIcon = false;
            this.Text = "Mirror";
            this.Shown += new System.EventHandler(this.MirrorForm_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MirrorForm_MouseDown);
            this.mirrorMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mirrorBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox mirrorBox;
        private System.Windows.Forms.ContextMenuStrip mirrorMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateBy90ToolStripMenuItem;
    }
}