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
        const VirtualKeyCode ASURA_VK_KEY = VirtualKeyCode.F8;
        const string JUMP_HIDDEN_KEY = "VK_6";
        const string HIDDEN_ASURA_KEY = "VK_5";
        const string DUEL_KEY = "VK_4";
        const string ABSORB_ZEN_HIDDEN_KEY = "VK_3";
        const string ZEN_HIDDEN_KEY = "VK_2";
        const string ABSORB_HIDDEN_KEY = "192";

        const int DELAY_LEFT_CLICK = 12;
        const int DELAY_HIDDEN_ASURA = 50;
        bool skipKeyDownEvent = false;
        bool holdingKey = false;
        bool holdingAlt = false;
        bool wantHiddenFlash = false;
        Keys clickingKeys = Keys.None;

        public Form1()
        {
            InitializeComponent();
            Hook.CreateHook(KeyReaderr);
            SetEnableProcess(true);

            GmaHook.IKeyboardMouseEvents m_GlobalHook = GmaHook.Hook.GlobalEvents();
            m_GlobalHook.KeyDown += gmaKeyDownCallback;
            m_GlobalHook.KeyUp += gmaKeyUpCallback;
        }

        private bool isEnableProcess()
        {
            return (enableProcess && Helper.isActiveWindow());
        }

        public void LoopClick(VirtualKeyCode vk)
        {
            while (holdingKey && isEnableProcess())
            {
                if (vk == VirtualKeyCode.SPACE)
                {
                    skipKeyDownEvent = true;
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                    skipKeyDownEvent = false;
                    Thread.Sleep(106);
                    skipKeyDownEvent = true;
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_F);
                    skipKeyDownEvent = false;
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(DELAY_LEFT_CLICK);
                }
                else
                {
                    skipKeyDownEvent = true;
                    inputSimulator.Keyboard.KeyPress(vk);
                    skipKeyDownEvent = false;
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(DELAY_LEFT_CLICK);
                }
            }
            Helper.LeftClick(inputSimulator);
        }

        private VirtualKeyCode getVirtualKeyCode(Keys keyCode)
        {
            VirtualKeyCode resultVk = VirtualKeyCode.NONAME;

            Keys[] keyList = { Keys.D1, Keys.F, Keys.H, Keys.Space };
            VirtualKeyCode[] vkList = {
                ASURA_VK_KEY,
                VirtualKeyCode.VK_F,
                VirtualKeyCode.VK_H,
                VirtualKeyCode.SPACE
            };
            for (int i = 0; i < keyList.Length; i++) {
                if (keyCode == keyList[i])
                {
                    resultVk = vkList[i];
                    break;
                }
            }
            return resultVk;
        }

        public void gmaKeyDownCallback(object sender, KeyEventArgs e)
        {
            if (
                isEnableProcess() &&
                !skipKeyDownEvent &&
                !holdingKey
            )
            {
                VirtualKeyCode vk = getVirtualKeyCode(e.KeyCode);
                if (vk != VirtualKeyCode.NONAME)
                {
                    holdingKey = true;
                    clickingKeys = e.KeyCode;
                    Task.Run(() => LoopClick(vk));
                }
            }
            if (
                !holdingAlt &&
                (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
            ) holdingAlt = true;
        }

        public void gmaKeyUpCallback(object sender, KeyEventArgs e)
        {
            if (
                holdingAlt &&
                (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)
            ) holdingAlt = false;
            else if (
                holdingKey &&
                !skipKeyDownEvent
            ) {
                if (
                    (e.KeyCode != Keys.D5 && e.KeyCode != Keys.W && e.KeyCode != Keys.E) ||
                    e.KeyCode == clickingKeys
                 ) holdingKey = false;
            }
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
                        if (!isWatingAsura || wantHiddenFlash) break;
                        if (j == 16 || j == 19)
                        {
                            wantCursorUpPosition = LowLevelMouse.moveMouse(wantCursorUpPosition);
                            skipKeyDownEvent = true;
                            inputSimulator.Keyboard.KeyPress(ASURA_VK_KEY);
                            skipKeyDownEvent = false;
                            Helper.LeftClick(inputSimulator);
                        }
                        j++;
                    }
                    wantHiddenFlash = false;
                    isWatingAsura = false;
                    break;

                case JUMP_HIDDEN_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(MIN_DELAY);
                    //inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                    //Thread.Sleep(90);
                    Thread.Sleep(70);
                    Helper.Hidden(inputSimulator);
                    break;

                case HIDDEN_ASURA_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Thread.Sleep(DELAY_HIDDEN_ASURA);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
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
            if (!isEnableProcess()) { return; }

            if (
                isCasting &&
                keyString == ABSORB_ZEN_HIDDEN_KEY &&
                wantHiddenFlash == false
            )
            {
                wantHiddenFlash = true;
                Task.Run(() => RunCastKey(keyString));
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
                case ABSORB_HIDDEN_KEY:
                case ZEN_HIDDEN_KEY:
                case ABSORB_ZEN_HIDDEN_KEY:
                case JUMP_HIDDEN_KEY:
                case HIDDEN_ASURA_KEY:
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
