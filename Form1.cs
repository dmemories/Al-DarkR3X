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
        private bool skipKeypress;
        private bool wantCursorUpPosition;

        private int DELAY_AFTER_ZEN = 50; // 20, 
        private int DELAY_AFTER_ABSORB = 120; // 90, 120

        public Form1()
        {
            InitializeComponent();
            inputSimulator = new InputSimulator();
            lowLevelKeyboardListener = new LowLevelKeyboardListener();
            lowLevelKeyboardListener.OnKeyPressed += _listener_OnKeyPressed;
            lowLevelKeyboardListener.HookKeyboard();
            skipKeypress = false;
            wantCursorUpPosition = true;
        }

        void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            //this.textBox_DisplayKeyboardInput.Text += e.KeyPressed.ToString();
            Console.WriteLine(e.KeyPressed);
            //SendKeys.Send("d");
            if (skipKeypress)
            {
                return;
            }
            else
            {
                if (e.KeyPressed.ToString() == "D1")
                {
                    skipKeypress = true;
                    CastCombo();
                    Thread.Sleep(DELAY_AFTER_ZEN);
                    CastCombo();
                    Thread.Sleep(DELAY_AFTER_ZEN);
                    CastCombo();

                    Thread.Sleep(10);
                    skipKeypress = false;
                }
                if (e.KeyPressed.ToString() == "D2")
                {
                    skipKeypress = true;
                    SendKeys.Send("a");
                    Thread.Sleep(4);
                    SendKeys.Send("Z");
                    Thread.Sleep(10);
                    skipKeypress = false;
                }
                if (e.KeyPressed.ToString() == "D3")
                {
                    skipKeypress = true;
                    SendKeys.Send("a");
                    Thread.Sleep(4);
                    SendKeys.Send("Z");
                    Thread.Sleep(10);
                    skipKeypress = false;
                }
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
            Thread.Sleep(10);
            LeftClick();
            Thread.Sleep(DELAY_AFTER_ABSORB);
            SendKeys.Send("{F2}");
        }

        private void LeftClick()
        {
            inputSimulator.Mouse.LeftButtonDown();
            Thread.Sleep(1);
            inputSimulator.Mouse.LeftButtonUp();
        }

    }

}
