﻿using System;
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
        bool wantCursorUpPosition = false;
        bool isCasting = false;
        bool isWatingAsura = false;

        const int MIN_DELAY = 1;
        const VirtualKeyCode ASURA_VK_KEY = VirtualKeyCode.F8;
        const string JUMP_HIDDEN_KEY = "VK_6";
        const string HIDDEN_ASURA_KEY = "VK_5";
        const string DUEL_KEY = "VK_4";
        const string ABSORB_ZEN_HIDDEN_KEY = "VK_3";
        const string ZEN_HIDDEN_KEY = "VK_TAB";
        const string ABSORB_HIDDEN_KEY = "192";
        const string DODGE_ASURA_KEY = "VK_2";

        const int DELAY_LEFT_CLICK = 12;
        const int DELAY_HIDDEN_ASURA = 50;
        bool skipKeyDownEvent = false;
        bool holdingKey = false;
        bool holdingAlt = false;
        bool wantHiddenFlash = false;
        Keys clickingKeys = Keys.None;

        // Mode
        const string MODE_CHAMPION = "Chamption";
        const string MODE_FARM = "Farm";

        public Form1()
        {
            InitializeComponent();
            Hook.CreateHook(KeyReaderr);
            SetEnableProcess(true);

            GmaHook.IKeyboardMouseEvents m_GlobalHook = GmaHook.Hook.GlobalEvents();
            m_GlobalHook.KeyDown += gmaKeyDownCallback;
            m_GlobalHook.KeyUp += gmaKeyUpCallback;
            modeComboBox.Items.Add(MODE_CHAMPION);
            modeComboBox.Items.Add(MODE_FARM);
            modeComboBox.SelectedItem = MODE_CHAMPION;
        }

        public void LoopClick(VirtualKeyCode vk)
        {
            while (holdingKey && Helper.isEnableProcess())
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
            if (modeComboBox.SelectedItem.ToString() != MODE_CHAMPION) return;
            if (
                Helper.isEnableProcess() &&
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
                    keybd_event(9, 0x45, 0x0001 | 0x0002, 0);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Thread.Sleep(10);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                    Thread.Sleep(76);
                    Helper.Hidden(inputSimulator);
                    break;

                case ABSORB_ZEN_HIDDEN_KEY:
                    wantCursorUpPosition = Helper.AbsorbZen(wantCursorUpPosition, inputSimulator);
                    Thread.Sleep(80);
                    Helper.Hidden(inputSimulator);
                    break;

                case DUEL_KEY:
                    bool skipAsura = false;
                    wantCursorUpPosition = Helper.AbsorbZen(wantCursorUpPosition, inputSimulator);
                    isWatingAsura = true;
                    for (int i = 0; i < 22; i++)
                    {
                        Thread.Sleep(MIN_DELAY);
                        if (!isWatingAsura || wantHiddenFlash) break;
                        if (j > 12)
                        {
                            if (skipAsura) skipAsura = false;
                            else
                            {
                                skipAsura = true;
                                wantCursorUpPosition = LowLevelMouse.moveMouse(wantCursorUpPosition);
                                inputSimulator.Keyboard.KeyPress(ASURA_VK_KEY);
                                Helper.LeftClick(inputSimulator);
                            }
                        }
                        j++;
                    }
                    wantHiddenFlash = false;
                    isWatingAsura = false;
                    break;

                case JUMP_HIDDEN_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(10);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_T);
                    Thread.Sleep(56);
                    Helper.Hidden(inputSimulator);
                    break;

                case HIDDEN_ASURA_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Thread.Sleep(DELAY_HIDDEN_ASURA);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_E);
                    break;

                case DODGE_ASURA_KEY:
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    Thread.Sleep(10);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                    Helper.LeftClick(inputSimulator);
                    Thread.Sleep(100);
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F2);
                    break;
            }
            isCasting = false;
        }

        private void KeyReaderr(IntPtr wParam, IntPtr lParam)
        {
            if (holdingAlt) return;
            int key = Marshal.ReadInt32(lParam);
            string keyString = ((Hook.VK)key).ToString();

            if (keyString == "VK_PRIOR") { SetEnableProcess(!Helper.enableProcess); }
            if (!Helper.isEnableProcess()) { return; }

            switch (modeComboBox.SelectedItem)
            {
                case MODE_CHAMPION:
                    KeyReaderChamption(keyString);
                    break;

                case MODE_FARM:
                    Task.Run(() => (new Farm()).KeyReaderFarm(keyString));
                    break;

                default: break;
            }
        }

        private void KeyReaderChamption(string keyString)
        {
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
                case DODGE_ASURA_KEY:
                    Task.Run(() => RunCastKey(keyString));
                    return;

                default: break;
            }
        }

        // =======================================================================
        private void SetEnableProcess(bool isEnable)
        {
            Helper.enableProcess = isEnable;
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

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (modeComboBox.SelectedItem.ToString())
            {
                case MODE_CHAMPION:
                    pictureBox.Image = Properties.Resources.MBlack;
                    break;

                case MODE_FARM:
                    pictureBox.Image = Properties.Resources.Blue;
                    break;

                default: break;
            }
        }
    }


}
