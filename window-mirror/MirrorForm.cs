using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using window_mirror.lib;

namespace window_mirror
{
    public partial class MirrorForm : Form
    {
        public DisplayMode Mode;
        public DesktopWindow MirrorWindow;
        private int Interval;

        public MirrorForm(DesktopWindow window, DisplayMode mode, int refreshRate)
        {
            InitializeComponent();
            this.MirrorWindow = window;
            this.Mode = mode;
            this.Interval = 1000 / refreshRate;
            this.Text = "Mirror (" + refreshRate + " fps)";
        }

        public Image MirrorImage
        {
            get
            {
                return mirrorBox.Image;
            }
            set
            {
                mirrorBox.Image = value;
                this.updateSize();
            }
        }

        public void updateSize()
        {
            Size targetSize = this.Size;
            Point targetLocation = this.Location;
            bool topMost = false;

            switch (this.Mode)
            {
                case DisplayMode.Window:
                    targetSize = mirrorBox.Image.Size;
                    topMost = false;
                    break;

                case DisplayMode.Fullscreen:
                    Rectangle bound = Screen.FromControl(this).Bounds;
                    targetLocation = bound.Location;
                    targetSize = bound.Size;
                    topMost = true;
                    break;
            }

            if (this.Location != targetLocation)
                this.Location = targetLocation;
            if (this.Size != targetSize)
                this.Size = targetSize;
            if (this.TopMost != topMost)
                this.TopMost = topMost;
        }

        private void MirrorForm_Shown(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(MirrorThread)) { IsBackground = true }.Start();
        }

        private void MirrorThread()
        {
            try
            {
                while (this.MirrorWindow.Exists && this.InvokeEx(f => f.Visible))
                {
                    Bitmap test = this.MirrorWindow.Capture();
                    this.InvokeEx(f => f.MirrorImage = test);
                    Thread.Sleep(this.Interval);
                }
                this.InvokeEx(f => f.Close());
            }
            catch { }
        }

        public enum DisplayMode
        {
            Window,
            Fullscreen
        }

        private void MirrorForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Mode == DisplayMode.Window && e.Button == MouseButtons.Left)
            {
                WinAPI.ReleaseCapture();
                WinAPI.SendMessage(Handle, WinAPI.WM_NCLBUTTONDOWN, WinAPI.HT_CAPTION, 0);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fullscreenToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (fullscreenToolStripMenuItem.Checked)
                this.Mode = DisplayMode.Fullscreen;
            else
                this.Mode = DisplayMode.Window;
        }
    }
}
