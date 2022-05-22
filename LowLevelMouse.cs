using System;
using System.Runtime.InteropServices;

namespace Al_DarkR3X
{
    class LowLevelMouse
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int X, int Y)
            {
                x = X;
                y = Y;
            }

        }

        public static bool moveMouse(bool wantCursorUpPosition)
        {
            if (wantCursorUpPosition)
            {
                LowLevelMouse.SetCursorPos(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y + 10);
                return false;
            }
            else
            {
                LowLevelMouse.SetCursorPos(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y - 10);
                return true;
            }
        }

        public static void setCursor(int x, int y)
        {
            LowLevelMouse.SetCursorPos(x, y);
        }

    }
}
