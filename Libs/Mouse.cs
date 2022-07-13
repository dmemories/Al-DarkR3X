using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsInput;

namespace Al_DarkR3X.Libs
{
    internal class Mouse
    {
        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        public static InputSimulator inputSimulator = new InputSimulator();
        public static bool wantCursorUpPosition = false;

        public static void LeftClick()
        {
            inputSimulator.Mouse.LeftButtonDown();
            inputSimulator.Mouse.LeftButtonUp();
        }

        public static void RightClick()
        {
            inputSimulator.Mouse.RightButtonDown();
            inputSimulator.Mouse.RightButtonUp();
        }


        public static void MoveUp() { SetCursorPos(Cursor.Position.X, Cursor.Position.Y + 10); }
        public static void MoveDown() { SetCursorPos(Cursor.Position.X, Cursor.Position.Y - 10); }

        public static bool SwitchMove()
        {
            if (wantCursorUpPosition) MoveUp();
            else MoveDown();
            wantCursorUpPosition = !wantCursorUpPosition;
            return wantCursorUpPosition;
        }

        public static void SetCursor(int x, int y)
        {
            SetCursorPos(x, y);
        }

    }
}
