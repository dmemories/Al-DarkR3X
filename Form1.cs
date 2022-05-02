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
        InputSimulator inputSimulator = new InputSimulator();
        bool enableProcess = true;
        bool wantCursorUpPosition = false;
        bool isCasting = false;
        bool isWatingAsura = false;

        const string HIDDEN_ASURA_KEY = "VK_5";
        const string DUEL_KEY = "VK_4";
        const string ABSORB_ZEN_HIDDEN_KEY = "VK_3";
        
        public Form1()
        {
            InitializeComponent();
            Hook.CreateHook(KeyReaderr);
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Hook.DestroyHook();
            Console.WriteLine("Hook Destroyed.");
        }
      
        public void RunCastKey(string key)
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

            }
            isCasting = false;
        }

        private void KeyReaderr(IntPtr wParam, IntPtr lParam)
        {
            int key = Marshal.ReadInt32(lParam);
            string keyString = ((Hook.VK)key).ToString();

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
                case ABSORB_ZEN_HIDDEN_KEY:
                    Task.Run(() => RunCastKey(keyString));
                    break;

                case HIDDEN_ASURA_KEY:
                    Task.Run(() => RunCastKey(keyString));
                    break;
            }
        }

        // =======================================================================

        private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }


}
