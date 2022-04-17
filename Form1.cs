using System;
using System.Windows.Forms;
using System.Threading;
using WindowsInput;

namespace Al_DarkR3X
{
    public partial class Form1 : Form
    {
        private LowLevelKeyboardListener lowLevelKeyboardListener;
        private InputSimulator inputSimulator;
        private bool enableProcess;
        private bool skipKeypress;
        private bool wantCursorUpPosition;

        private int DELAY_AFTER_ZEN = 50; // 20, 
        private int DELAY_AFTER_ABSORB = 90; // 90, 120
        private int DELAY_BEFORE_CLICK = 1;

        public Form1()
        {
            InitializeComponent();
            inputSimulator = new InputSimulator();
            lowLevelKeyboardListener = new LowLevelKeyboardListener();
            lowLevelKeyboardListener.OnKeyPressed += _listener_OnKeyPressed;
            lowLevelKeyboardListener.HookKeyboard();
            enableProcess = true;
            skipKeypress = false;
            wantCursorUpPosition = true;
            SetEnableProcess(true);
        }

        void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            Console.WriteLine(e.KeyPressed.ToString());
            string activeTitle = ActiveWindow.GetActiveWindowTitle().ToLower();

            if (e.KeyPressed.ToString() == "PageUp")
            {
                if (enableProcess) SetEnableProcess(false);
                else SetEnableProcess(true);
                return;
            }

            if (
                !enableProcess ||
                skipKeypress ||
                (!activeTitle.Contains("ro") && !activeTitle.Contains("pvp"))
            )
            {
                return;
            }
            else
            {
                skipKeypress = true;
                switch (e.KeyPressed.ToString())
                {
                    case "D3":
                        ZenHidden();
                        break;

                    case "D4":
                        CastCombo();
                        break;

                    case "D5":
                        HiddenCombo();
                        break;

                    case "D6":
                        SendKeys.Send("a");
                        Thread.Sleep(80);
                        SendKeys.Send("z");
                        break;
                }
                Thread.Sleep(10);
                skipKeypress = false;
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            lowLevelKeyboardListener.UnHookKeyboard();
        }

        private void CastCombo()
        {
            if (wantCursorUpPosition)
            {
                wantCursorUpPosition = false;
                LowLevelMouse.SetCursorPos(Cursor.Position.X, Cursor.Position.Y + 10);
            }
            else
            {
                wantCursorUpPosition = true;
                LowLevelMouse.SetCursorPos(Cursor.Position.X, Cursor.Position.Y - 10);
            }
            SendKeys.Send("{F4}");
            Thread.Sleep(DELAY_BEFORE_CLICK);
            LeftClick();
            Thread.Sleep(DELAY_AFTER_ABSORB);
            SendKeys.Send("{F2}");
        }

        private void ZenHidden()
        {
            if (wantCursorUpPosition)
            {
                wantCursorUpPosition = false;
                LowLevelMouse.SetCursorPos(Cursor.Position.X, Cursor.Position.Y + 10);
            }
            else
            {
                wantCursorUpPosition = true;
                LowLevelMouse.SetCursorPos(Cursor.Position.X, Cursor.Position.Y - 10);
            }
            SendKeys.Send("w");
            Thread.Sleep(10);
            SendKeys.Send("{F2}");
            Thread.Sleep(10);
            SendKeys.Send("w");
            Thread.Sleep(16);
            SendKeys.Send("e");
        }

        private void HiddenCombo()
        {
            if (wantCursorUpPosition)
            {
                wantCursorUpPosition = false;
                LowLevelMouse.SetCursorPos(Cursor.Position.X, Cursor.Position.Y + 10);
            }
            else
            {
                wantCursorUpPosition = true;
                LowLevelMouse.SetCursorPos(Cursor.Position.X, Cursor.Position.Y - 10);
            }
            SendKeys.Send("w");
            SendKeys.Send("{F1}");
            Thread.Sleep(1);
            SendKeys.Send("w");
            SendKeys.Send("e");
        }

        private void LeftClick()
        {
            inputSimulator.Mouse.LeftButtonDown();
            Thread.Sleep(1);
            inputSimulator.Mouse.LeftButtonUp();
        }

        private void SetEnableProcess(bool isEnable)
        {
            Console.WriteLine(isEnable);
            enableProcess = isEnable;
            enableCheckBox.Checked = isEnable;
        }

        private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetEnableProcess(enableCheckBox.Checked);
        }
    }

}
