using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WindowsInput.Native;
using WindowsInput;
using System.Threading;
using GmaHook = Gma.System.MouseKeyHook;

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

        const int MIN_DELAY = 1;
        const string JUMP_HIDDEN_KEY = "VK_6";
        const string HIDDEN_ASURA_KEY = "VK_5";
        const string DUEL_KEY = "VK_4";
        const string ABSORB_ZEN_HIDDEN_KEY = "VK_3";
        const string ZEN_HIDDEN_KEY = "VK_2";
        const string ABSORB_HIDDEN_KEY = "VK_1";

        const int DELAY_LEFT_CLICK = 12;
        const string ASURA_KEY = "VK_F1";
        const string FINGER_OFFENSIVE_KEY = "VK_F";
        const string INVESTIGATE_KEY = "VK_H";

        byte keybdEventKeyNum = 0;
        bool skipKeyUpEvent = false;
        bool holdingKey = false;
        bool holdingAlt = false;

        public Form1()
        {
            InitializeComponent();
            Hook.CreateHook(KeyReaderr);
            SetEnableProcess(true);

            GmaHook.IKeyboardMouseEvents m_GlobalHook = GmaHook.Hook.GlobalEvents();
            m_GlobalHook.KeyDown += gmaKeyDownCallback;
            m_GlobalHook.KeyUp += gmaKeyUpCallback;
        }

        public void gmaKeyDownCallback(object sender, KeyEventArgs e)
        {
            if (!holdingAlt && e.KeyValue == 164) holdingAlt = true;
        }

        public void gmaKeyUpCallback(object sender, KeyEventArgs e)
        {
            if (holdingAlt && e.KeyValue == 164) holdingAlt = false;
            else if (
                holdingKey &&
                !skipKeyUpEvent &&
                e.KeyValue == keybdEventKeyNum
            ) holdingKey = false;
        }

        private void RunClickKey(string key)
        {
            VirtualKeyCode vkKey = VirtualKeyCode.CANCEL;
            keybdEventKeyNum = 0;
            switch (key)
            {
                case ASURA_KEY:
                    vkKey = VirtualKeyCode.F1;
                    keybdEventKeyNum = 112;
                    break;

                case FINGER_OFFENSIVE_KEY:
                    vkKey = VirtualKeyCode.VK_F;
                    keybdEventKeyNum = 70;
                    break;

                case INVESTIGATE_KEY:
                    vkKey = VirtualKeyCode.VK_H;
                    keybdEventKeyNum = 72;
                    break;
            }
            if (
                vkKey == VirtualKeyCode.CANCEL ||
                keybdEventKeyNum == 0
            ) { return; }

            isCasting = true;
            while (holdingKey && Helper.isActiveWindow())
            {
                skipKeyUpEvent = true;
                inputSimulator.Keyboard.KeyPress(vkKey);
                skipKeyUpEvent = false;
                Helper.LeftClick(inputSimulator);
                Thread.Sleep(DELAY_LEFT_CLICK);
            }
            Helper.LeftClick(inputSimulator);
            // keybd_event(keybdEventKeyNum, 0x45, 0x0001 | 0, 0);
            //keybd_event(keybdEventKeyNum, 0x45, 0x0001 | 0x0002, 0);
            isCasting = false;
         }

        private async void HiddenSwap()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
            await Task.Delay(20);
            wantCursorUpPosition = LowLevelMouse.moveMouse(wantCursorUpPosition);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
            await Task.Delay(40);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
        }

        private void RunCastKey(string key)
        {
            isCasting = true;
            int j = 0;
            switch (key)
            {
                case ABSORB_HIDDEN_KEY:
                    wantCursorUpPosition = LowLevelMouse.moveMouse(wantCursorUpPosition);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(80);
                    Helper.Hidden(inputSimulator);
                    break;

                case ZEN_HIDDEN_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                    Thread.Sleep(80);
                    Helper.Hidden(inputSimulator);
                    break;

                case ABSORB_ZEN_HIDDEN_KEY:
                    wantCursorUpPosition = Helper.AbsorbZen(wantCursorUpPosition, inputSimulator);
                    Thread.Sleep(80);
                    Helper.Hidden(inputSimulator);
                    break;

                case DUEL_KEY:
                    wantCursorUpPosition = Helper.AbsorbZen(wantCursorUpPosition, inputSimulator);
                    isWatingAsura = true;

                    for (int i = 0; i < 26; i++)
                    {
                        Thread.Sleep(MIN_DELAY);
                        if (!isWatingAsura) break;
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

                case JUMP_HIDDEN_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(MIN_DELAY);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                    Thread.Sleep(90);
                    Helper.Hidden(inputSimulator);
                    break;

            }
            isCasting = false;
        }

        private void KeyReaderr(IntPtr wParam, IntPtr lParam)
        {
            if (holdingAlt) return;
            int key = Marshal.ReadInt32(lParam);
            string keyString = ((Hook.VK)key).ToString();

            if (keyString == "VK_PRIOR") SetEnableProcess(!enableProcess);
            if (
                !enableProcess ||
                !Helper.isActiveWindow()
            ) { return; }

            if (holdingKey && keyString == HIDDEN_ASURA_KEY)
            {
                Task.Run(() => HiddenSwap());
                return;
            }

            if (keyString == DUEL_KEY)
            {
                if (isCasting)
                {
                    if (!isWatingAsura) return;
                    isWatingAsura = false;
                    Thread.Sleep(17);
                }
                if (!isCasting)
                {
                    Task.Run(() => RunCastKey(keyString));
                    return;
                }
            }
            if (isCasting) return;

            switch (keyString)
            {
                case ASURA_KEY:
                case FINGER_OFFENSIVE_KEY:
                case INVESTIGATE_KEY:
                    holdingKey = true;
                    Task.Run(() => RunClickKey(keyString));
                    return;

                default: break;
            }

            switch (keyString)
            {
                case ABSORB_HIDDEN_KEY:
                case ZEN_HIDDEN_KEY:
                case ABSORB_ZEN_HIDDEN_KEY:
                case JUMP_HIDDEN_KEY:
                    Task.Run(() => RunCastKey(keyString));
                    return;

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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Hook.DestroyHook();
            Console.WriteLine("Hook Destroyed.");
        }
    }


}
