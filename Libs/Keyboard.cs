using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace Al_DarkR3X.Libs
{
    internal class Keyboard
    {

        /* https://blog.krybot.com/a?ID=00100-e197e40c-67bb-442f-a7ad-4c8030f5baef */
        [DllImport("User32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public static InputSimulator inputSimulator = new InputSimulator();
        public static bool skipKeyDownEvent = false;
        static public byte ALT_KEY_BYTE = 164;

        public static void Press(VirtualKeyCode vk)
        {
            inputSimulator.Keyboard.KeyPress(vk);
        }

        public static void PressWithoutKeyEvent(VirtualKeyCode vk)
        {
            skipKeyDownEvent = true;
            inputSimulator.Keyboard.KeyPress(vk);
            skipKeyDownEvent = false;
        }

        public static void HoldKey(byte keyCode)
        {
            keybd_event(keyCode, 0x45, 0x0001 | 0, 0);
        }

        public static void ReleaseKey(byte keyCode)
        {
            keybd_event(keyCode, 0x45, 0x0001 | 0x0002, 0);
        }

        public static void KeyWithALT(VirtualKeyCode vk)
        {
            HoldKey(ALT_KEY_BYTE);
            Thread.Sleep(20);
            inputSimulator.Keyboard.KeyPress(vk);
            Thread.Sleep(20);
            ReleaseKey(ALT_KEY_BYTE);
        }

        public static void KeyWithRightClick()
        {
            HoldKey(ALT_KEY_BYTE);
            Thread.Sleep(100);
            inputSimulator.Mouse.RightButtonDown();
            Thread.Sleep(100);
            inputSimulator.Mouse.RightButtonUp();
            ReleaseKey(ALT_KEY_BYTE);
        }
    }
}
