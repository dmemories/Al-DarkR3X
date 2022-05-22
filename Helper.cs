using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;
using System.Runtime.InteropServices;

namespace Al_DarkR3X
{
    class Helper
    {
        // https://blog.krybot.com/a?ID=00100-e197e40c-67bb-442f-a7ad-4c8030f5baef
        [DllImport("User32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        static public byte ALT_KEY = 164;
        public static bool enableProcess = false;

        public static bool isEnableProcess()
        {
            return (enableProcess && Helper.isActiveWindow());
        }

        public static void LeftClick(InputSimulator inputSimulator)
        {
            inputSimulator.Mouse.LeftButtonDown();
            inputSimulator.Mouse.LeftButtonUp();
        }

        public static void RightClick(InputSimulator inputSimulator)
        {
            inputSimulator.Mouse.LeftButtonDown();
            Thread.Sleep(100);
            inputSimulator.Mouse.LeftButtonUp();
        }

        public static void Delay(int milliSec)
        {
            Task.Run(async delegate { await Task.Delay(milliSec); }).Wait();
        }

        public static void Hidden(InputSimulator inputSimulator)
        {
            //inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
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

        public static void holdKey(byte keyCode)
        {
            keybd_event(keyCode, 0x45, 0x0001 | 0, 0);
        }

        public static void releaseKey(byte keyCode)
        {
            keybd_event(keyCode, 0x45, 0x0001 | 0x0002, 0);
        }

        public static void keyWithALT(InputSimulator inputSimulate, VirtualKeyCode vk)
        {
            Helper.holdKey(ALT_KEY);
            Thread.Sleep(20);
            inputSimulate.Keyboard.KeyPress(vk);
            Thread.Sleep(20);
            Helper.releaseKey(ALT_KEY);
        }

        public static void keyWithRightClick(InputSimulator inputSimulate)
        {
            Helper.holdKey(ALT_KEY);
            Thread.Sleep(100);
            inputSimulate.Mouse.RightButtonDown();
            Thread.Sleep(100);
            inputSimulate.Mouse.RightButtonUp();
            Helper.releaseKey(ALT_KEY);
        }


    }
}
