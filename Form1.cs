using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using WindowsInput;

namespace Al_DarkR3X
{
    public partial class Form1 : Form
    {
        // https://blog.krybot.com/a?ID=00100-e197e40c-67bb-442f-a7ad-4c8030f5baef
        [DllImport("User32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(Keys ArrowKeys);
        public Form1()
        {
            InitializeComponent();
            Hook.CreateHook(KeyReaderr);
        }

        const int DELAY_LEFT_CLICK = 14;
        InputSimulator inputSimulator = new InputSimulator();

        bool wantCursorUpPosition = false;
        bool isClicking = false;
        bool isCasting = false;
        bool wantHiddenCasting = false;

        bool isCastingCombo = false;

        static void OnProcessExit(object sender, EventArgs e)
        {
            Hook.DestroyHook();
            Console.WriteLine("Hook Destroyed.");
        }

        public void RunClickKey(string key)
        {
            Delay(1);
            switch (key)
            {
                case "VK_F122":
                    while (true)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F1);
                        LeftClick();
                        keybd_event(112, 0x45, 0x0001 | 0, 0);
                        Delay(DELAY_LEFT_CLICK);
                        byte[] result = BitConverter.GetBytes(GetAsyncKeyState(Keys.F1));
                        if (result[1] < 1) break;
                    }
                    LeftClick();
                    keybd_event(112, 0x45, 0x0001 | 0x0002, 0);
                    break;

                case "VK_F":
                    while (true)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_F);
                        LeftClick();
                        keybd_event(70, 0x45, 0x0001 | 0, 0);
                        Delay(DELAY_LEFT_CLICK);
                        byte[] result = BitConverter.GetBytes(GetAsyncKeyState(Keys.F));
                        if (result[1] < 1) break;
                    }
                    LeftClick();
                    keybd_event(70, 0x45, 0x0001 | 0x0002, 0);
                    break;

                case "VK_H":
                    while (true)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_H);
                        LeftClick();
                        keybd_event(72, 0x45, 0x0001 | 0, 0);
                        Delay(DELAY_LEFT_CLICK);
                        byte[] result = BitConverter.GetBytes(GetAsyncKeyState(Keys.H));
                        if (result[1] < 1) break;
                    }
                    LeftClick();
                    keybd_event(72, 0x45, 0x0001 | 0x0002, 0);
                    break;

                case "VK_4":
                    isCastingCombo = true;
                    while (true)
                    {
                        wantCursorUpPosition = LowLevelMouse.moveMouse(wantCursorUpPosition);
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                        LeftClick();
                        //keybd_event(115, 0x45, 0x0001 | 0, 0);
                        Delay(100);
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                        Delay(50);
                        if (wantHiddenCasting)
                        {
                            Delay(20);
                            wantHiddenCasting = false;
                            Hidden();
                            break;
                        }
                        if (BitConverter.GetBytes(GetAsyncKeyState(Keys.D4))[1] < 1) break;
                        Delay(50);
                        if (BitConverter.GetBytes(GetAsyncKeyState(Keys.D4))[1] < 1) break;
                    }
                    //keybd_event(115, 0x45, 0x0001 | 0x0002, 0);
                    isCastingCombo = false;
                    break;
            }
            isClicking = false;
        }


        public void RunCastKey(string key)
        {
            switch (key)
            {
                case "VK_3":
                    if (isCastingCombo) wantHiddenCasting = true;
                    break;

                case "VK_5":
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Delay(35);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Delay(40);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
                    break;

                case "VK_6":
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    LeftClick();
                    Delay(1);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                    Delay(80);
                    Hidden();
                    break;
            }
            isCasting = false;
        }

        private void KeyReaderr(IntPtr wParam, IntPtr lParam)
        {
            int key = Marshal.ReadInt32(lParam);
            Hook.VK vk = (Hook.VK)key;
            string keyString = vk.ToString();

            if (!isClicking && (keyString == "VK_F1" || keyString == "VK_4" || keyString == "VK_F" || keyString == "VK_H"))
            {
                isClicking = true;
                Task.Run(() => RunClickKey(keyString));
            }
            if (!isCasting && (keyString == "VK_6" || keyString == "VK_3" || keyString == "VK_5"))
            {
                isCasting = true;
                Task.Run(() => RunCastKey(keyString));
            }
        }

        private void Hidden()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
            Delay(20);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
        }

        private void LeftClick()
        {
            inputSimulator.Mouse.LeftButtonDown();
            inputSimulator.Mouse.LeftButtonUp();
        }

        public static void Delay(int milliSec)
        {
            Task.Run(async delegate
            {
                await Task.Delay(milliSec);
            }).Wait();
        }

        private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }


}
