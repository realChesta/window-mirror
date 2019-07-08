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
        RotateFlipType Rotation;
        public DesktopWindow MirrorWindow;
        private int Interval;

        public MirrorForm(DesktopWindow window, int refreshRate, DisplayMode mode = DisplayMode.Window)
        {
            InitializeComponent();
            this.MirrorWindow = window;
            this.Mode = mode;
            this.Interval = 1000 / refreshRate;
            this.Text = "Mirror (" + refreshRate + " fps)";
            this.Rotation = RotateFlipType.RotateNoneFlipNone;
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

                    if (this.Rotation == RotateFlipType.Rotate90FlipNone || this.Rotation == RotateFlipType.Rotate270FlipNone)
                        targetSize = new Size(targetSize.Height, targetSize.Width);

                    break;

                case DisplayMode.Fullscreen:
                    Rectangle bound = Screen.FromControl(this).Bounds;
                    targetLocation = bound.Location;
                    targetSize = bound.Size;
                    topMost = true;
                    break;
            }

            if (this.Rotation != RotateFlipType.RotateNoneFlipNone)
                mirrorBox.Image.RotateFlip(this.Rotation);

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
                    Bitmap bmp = this.MirrorWindow.Capture();
                    this.InvokeEx(f => f.MirrorImage = bmp);
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

        private void rotateBy90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Rotation = NextRotation();
        }

        private RotateFlipType NextRotation()
        {
            switch (this.Rotation)
            {
                case RotateFlipType.RotateNoneFlipNone:
                    return RotateFlipType.Rotate90FlipNone;

                case RotateFlipType.Rotate90FlipNone:
                    return RotateFlipType.Rotate180FlipNone;

                case RotateFlipType.Rotate180FlipNone:
                    return RotateFlipType.Rotate270FlipNone;

                case RotateFlipType.Rotate270FlipNone:
                    return RotateFlipType.RotateNoneFlipNone;

                default:
                    return RotateFlipType.RotateNoneFlipNone;
            }
        }
    }
}
