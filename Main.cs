using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput.Native;
using GmaHook = Gma.System.MouseKeyHook;
using Al_DarkR3X.Class;
using Al_DarkR3X.Libs;
using static Al_DarkR3X.Configuration;

namespace Al_DarkR3X
{
    public partial class Main : Form
    {


        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(Keys ArrowKeys);

        public static IHeroClass Alkaiser = new Alkaiser();
        public static IHeroClass Blue = new Blue();
        public static IHeroClass Joker = new Joker();

        bool holdingKey = false;
        bool holdingAlt = false;
        Keys clickingKeys = Keys.None;

        public Main()
        {
            InitializeComponent();
            Hook.CreateHook(KeyHookAction);
            SetEnableProcess(true);

            GmaHook.IKeyboardMouseEvents m_GlobalHook = GmaHook.Hook.GlobalEvents();
            m_GlobalHook.KeyDown += GmaKeyDownCallback;
            m_GlobalHook.KeyUp += GmaKeyUpCallback;
            foreach (KeyValuePair<MODE, KeyValuePair<string, Image>> mode in modeDict)
            {
                modeComboBox.Items.Add(mode.Value.Key);
            }
            modeComboBox.SelectedItem = modeDict[MODE.ALKAISER].Key;
        }

        public void LoopClick(VirtualKeyCode vk, LoopClickCallback heroCallback = null)
        {
            while (holdingKey && Helper.IsProcessEnabled())
            {
                if (heroCallback != null) heroCallback();
                else {
                    Keyboard.PressWithoutKeyEvent(vk);
                    Mouse.LeftClick();
                    Thread.Sleep(DELAY_LOOP_LEFT_CLICK);
                }
            }
            Mouse.LeftClick();
        }

        private IHeroClass GetHeroClassFactory(string name)
        {
            if (name == modeDict[MODE.ALKAISER].Key) return Alkaiser;
            else if (name == modeDict[MODE.BLUE].Key) return Blue;
            else if (name == modeDict[MODE.JOKER].Key) return Joker;

            return null;
        }

        public void GmaKeyDownCallback(object sender, KeyEventArgs e)
        {
            IHeroClass hero = GetHeroClassFactory(modeComboBox.SelectedItem.ToString());
            if (hero == null) return;
            if (
                Helper.IsProcessEnabled() &&
                !Keyboard.skipKeyDownEvent &&
                !holdingKey
            )
            {
                VirtualKeyCode vk = Helper.GetVirtualKeyCodeFromKey(e.KeyCode);
                if (vk != VirtualKeyCode.NONAME && hero.HasVirtualLoopKey(vk))
                {
                    holdingKey = true;
                    clickingKeys = e.KeyCode;
                    LoopClickCallback callback = hero.GetLoopClickCallback(vk);
                    Task.Run(() => LoopClick(vk, callback));
                }
            }
            if (!holdingAlt && (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)) holdingAlt = true;
        }

        public void GmaKeyUpCallback(object sender, KeyEventArgs e)
        {
            if (holdingAlt && (e.KeyCode == Keys.LMenu || e.KeyCode == Keys.RMenu)) holdingAlt = false;
            else if (holdingKey && !Keyboard.skipKeyDownEvent) {
                if (
                    (e.KeyCode != Keys.D5 && e.KeyCode != Keys.W && e.KeyCode != Keys.E) ||
                    e.KeyCode == clickingKeys
                ) holdingKey = false;
            }
        }

        private void KeyHookAction(IntPtr wParam, IntPtr lParam)
        {
            if (holdingAlt) return;
            int key = Marshal.ReadInt32(lParam);
            string keyString = ((Hook.VK)key).ToString();

            if (keyString == ENABLE_PROCESS_KEY) { SetEnableProcess(!Helper.enableProcess); }
            if (!Helper.IsProcessEnabled()) { return; }

            IHeroClass heroClass = GetHeroClassFactory(modeComboBox.SelectedItem.ToString());
            if (heroClass != null) { Task.Run(() => heroClass.HookAction(keyString)); }
        }

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
            Console.WriteLine("End");
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox.Image = modeDict[(MODE) modeComboBox.SelectedIndex].Value;
        }
    }


}
