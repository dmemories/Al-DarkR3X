using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using WindowsInput;
using System.Threading;

namespace Al_DarkR3X
{
    public partial class Form1 : Form
    {

        [DllImport("User32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(Keys ArrowKeys);

        InputSimulator inputSimulator = new InputSimulator();
        bool enableProcess = false;
        bool wantCursorUpPosition = false;
        bool isCasting = false;
        bool isWatingAsura = false;

        const string JUMP_HIDDEN_KEY = "VK_6";
        const string HIDDEN_ASURA_KEY = "VK_5";
        const string DUEL_KEY = "VK_4";
        const string ABSORB_ZEN_HIDDEN_KEY = "VK_3";

        const int DELAY_LEFT_CLICK = 20;
        const string ASURA_KEY = "VK_F1";
        const string FINGER_OFFENSIVE_KEY = "VK_F";
        const string INVESTIGATE_KEY = "VK_H";

        public Form1()
        {
            InitializeComponent();
            Hook.CreateHook(KeyReaderr);
            SetEnableProcess(true);
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Hook.DestroyHook();
            Console.WriteLine("Hook Destroyed.");
        }

        private void RunClickKey(string key)
        {
            VirtualKeyCode vkKey = VirtualKeyCode.CANCEL;
            byte keybdEventKeyNum = 0;
            Keys asyncKeyState = Keys.None;
            switch (key)
            {
                case ASURA_KEY:
                    vkKey = VirtualKeyCode.F1;
                    keybdEventKeyNum = 112;
                    asyncKeyState = Keys.F1;
                    break;

                case FINGER_OFFENSIVE_KEY:
                    vkKey = VirtualKeyCode.VK_F;
                    keybdEventKeyNum = 70;
                    asyncKeyState = Keys.F;
                    break;

                case INVESTIGATE_KEY:
                    vkKey = VirtualKeyCode.VK_H;
                    keybdEventKeyNum = 72;
                    asyncKeyState = Keys.H;
                    break;
            }
            if (
                vkKey == VirtualKeyCode.CANCEL ||
                keybdEventKeyNum == 0 ||
                asyncKeyState == Keys.None
            ) { return; }


            isCasting = true;
            while (true)
            {
                inputSimulator.Keyboard.KeyPress(vkKey);
                Helper.LeftClick(inputSimulator);
                keybd_event(keybdEventKeyNum, 0x45, 0x0001 | 0, 0);
                Thread.Sleep(DELAY_LEFT_CLICK);
                byte[] result = BitConverter.GetBytes(GetAsyncKeyState(asyncKeyState));
                if (result[1] < 1) break;
            }
            Helper.LeftClick(inputSimulator);
            keybd_event(keybdEventKeyNum, 0x45, 0x0001 | 0x0002, 0);
            isCasting = false;
         }

        private void RunCastKey(string key)
        {
            isCasting = true;
            int j = 0;
            switch (key)
            {
                case ABSORB_ZEN_HIDDEN_KEY:
                    wantCursorUpPosition = Helper.AbsorbZen(wantCursorUpPosition, inputSimulator);
                    Thread.Sleep(70);
                    Helper.Hidden(inputSimulator);
                    break;

                case DUEL_KEY:
                    wantCursorUpPosition = Helper.AbsorbZen(wantCursorUpPosition, inputSimulator);
                    isWatingAsura = true;

                    for (int i = 0; i < 26; i++)
                    {
                        if (!isWatingAsura) break;
                        Thread.Sleep(1);
                        if (j == 16 || j == 19)
                        {
                            wantCursorUpPosition = LowLevelMouse.moveMouse(wantCursorUpPosition);
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F1);
                            Helper.LeftClick(inputSimulator);
                        }
                        j++;
                    }
                    isWatingAsura = false;
                    break;

                case HIDDEN_ASURA_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Thread.Sleep(18);
                    wantCursorUpPosition = LowLevelMouse.moveMouse(wantCursorUpPosition);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F1);
                    Helper.LeftClick(inputSimulator);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Thread.Sleep(16);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
                    break;

                case JUMP_HIDDEN_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(1);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                    Thread.Sleep(80);
                    Helper.Hidden(inputSimulator);
                    break;

            }
            isCasting = false;
        }

        private void KeyReaderr(IntPtr wParam, IntPtr lParam)
        {
            int key = Marshal.ReadInt32(lParam);
            string keyString = ((Hook.VK)key).ToString();

            if (keyString == "VK_PRIOR")
            {
                SetEnableProcess(!enableProcess);
            }
            string activeTitle = ActiveWindow.GetActiveWindowTitle().ToLower();
            if (
                !enableProcess ||
                (!activeTitle.Contains("ro") && !activeTitle.Contains("pvp"))
            )
            { return; }

            if (keyString == DUEL_KEY)
            {
                if (isCasting)
                {
                    if (!isWatingAsura) return;
                    isWatingAsura = false;
                    Thread.Sleep(17);
                }
                if (!isCasting) Task.Run(() => RunCastKey(keyString));
            }
            if (isCasting) return;

            switch (keyString) {
                case ASURA_KEY:
                case ABSORB_ZEN_HIDDEN_KEY:
                case HIDDEN_ASURA_KEY:
                case JUMP_HIDDEN_KEY:
                case FINGER_OFFENSIVE_KEY:
                case INVESTIGATE_KEY:
                    Task.Run(() => RunClickKey(keyString));
                    break;

                default: break;
            }
        }

        // =======================================================================
        private void SetEnableProcess(bool isEnable)
        {
            enableProcess = isEnable;
            enableCheckBox.Checked = isEnable;
        }

        private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetEnableProcess(enableCheckBox.Checked);
        }
    }


}
