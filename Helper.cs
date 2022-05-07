using System;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace Al_DarkR3X
{
    class Helper
    {
        // https://blog.krybot.com/a?ID=00100-e197e40c-67bb-442f-a7ad-4c8030f5baef

        public static void LeftClick(InputSimulator inputSimulator)
        {
            inputSimulator.Mouse.LeftButtonDown();
            inputSimulator.Mouse.LeftButtonUp();
        }

        public static void Delay(int milliSec)
        {
            Task.Run(async delegate { await Task.Delay(milliSec); }).Wait();
        }

        public static void Hidden(InputSimulator inputSimulator)
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
            Thread.Sleep(20);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
        }

        public static bool AbsorbZen(bool wantCursorUpPosition, InputSimulator inputSimulator)
        {
            bool result = LowLevelMouse.moveMouse(wantCursorUpPosition);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F4);
            Helper.LeftClick(inputSimulator);
            Thread.Sleep(115);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F2);
            return result;
        }

        public static bool isActiveWindow()
        {
            string activeTitle = ActiveWindow.GetActiveWindowTitle().ToLower();
            return activeTitle.Contains("ro-") || activeTitle.Contains("pvp");
        }

    }
}
