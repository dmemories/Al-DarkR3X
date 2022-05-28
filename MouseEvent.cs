using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Al_DarkR3X
{
    class MouseEvent
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);

        static uint DOWN = 0x0002;
        static uint UP = 0x0004;
        static uint MOUSEEVENTF_ABSOLUTE = 0x8000;
        static uint MOUSEEVENTF_MOVE = 0x0001;

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

        public static void LeftClick()
        {
            Thread.Sleep(1);
            mouse_event(DOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            Thread.Sleep(1);
            mouse_event(UP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
        }

        public static bool moveMouse(bool wantCursorUpPosition, int newPos = 0)
        {
            if (newPos == 0) newPos = 1;
            if (wantCursorUpPosition)
            {
                MouseEvent.SetCursorPos(Cursor.Position.X, Cursor.Position.Y + newPos);
                return false;
            }
            else
            {
                MouseEvent.SetCursorPos(Cursor.Position.X, Cursor.Position.Y - newPos);
                return true;
            }
        }

        public static void setCursor(int x, int y)
        {
            MouseEvent.SetCursorPos(x, y);
        }

    }
}
