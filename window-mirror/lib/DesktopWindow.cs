using System;
using System.Drawing;

namespace window_mirror.lib
{
    public class DesktopWindow
    {
        public IntPtr Handle { get; private set; }
        public string Caption
        {
            get
            { return WinAPI.GetWindowText(this.Handle); }
            set
            { WinAPI.SetWindowText(this.Handle, value); }
        }
        public bool Visible
        {
            get
            { return WinAPI.IsWindowVisible(this.Handle); }
        }
        public WindowShowStyle Style
        {
            get
            { return WinAPI.GetWindowStyle(this.Handle);  }
            set
            { WinAPI.ShowWindow(this.Handle, value); }
        }
        public bool Exists
        {
            get
            {
                return WinAPI.IsWindow(this.Handle);
            }
        }


        public DesktopWindow(int hwnd)
        {
            this.Handle = new IntPtr(hwnd);
        }
        public DesktopWindow(IntPtr hwnd)
        {
            this.Handle = hwnd;
        }

        public bool IsForegroundWindow()
        {
            return WinAPI.GetForegroundWindow().Equals(this.Handle);
        }

        public bool SetAsForegroundWindow()
        {
            return WinAPI.SetForegroundWindow(this.Handle);
        }

        public Bitmap Capture()
        {
            return WinAPI.CaptureWindow(this.Handle);
        }

        public void Close()
        {
            WinAPI.CloseWindow(this.Handle);
        }

        public static DesktopWindow[] getCurrentWindows()
        {
            return WinAPI.GetDesktopWindows();
        }


        public enum WindowShowStyle : uint
        {
            Hide = 0,
            Normal = 1,
            Minimized = 2,
            Maximized = 3,
            Maximize = 3,
            ShowNormalNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActivate = 7,
            ShowNoActivate = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimized = 11
        }

        public enum FlashMode
        {
            UntilForeground,
            UntilClosed
        }
    }
}
