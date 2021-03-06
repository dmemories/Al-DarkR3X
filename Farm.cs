using System.Windows.Forms;
using System.Drawing;
using WindowsInput;
using System.Threading;
using WindowsInput.Native;

namespace Al_DarkR3X
{
    public class Farm
    {

        private const string TRIGGER_KEY = "VK_SPACE";
        private const int DELAY_BETWEEN_ENTER = 500;
        private const int DELAY_BETWEEN_SKILL = 390;
        private const int RESTORE_WHEN_SKILL_TIMES = 70;
        private const bool INCLUDE_METEOR_STORM = false;

        private Point initCursorPosition;
        private InputSimulator inputSimulator = new InputSimulator();

        private void LoadLocation()
        {
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_1);
            Thread.Sleep(3000);
        }

        private void StorageItem()
        {
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_4);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_9);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_0);
            Thread.Sleep(100);
            MouseEvent.setCursor(initCursorPosition.X - 265, initCursorPosition.Y - 350);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_E);

            Thread.Sleep(1000);
            Helper.keyWithRightClick(inputSimulator);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_E);
            Thread.Sleep(100);
            LoadLocation();
        }

        private void HealAndStorage()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F9);
            Thread.Sleep(100);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_O);
            MouseEvent.setCursor(initCursorPosition.X + 80, initCursorPosition.Y - 140);
            MouseEvent.LeftClick();
            Thread.Sleep(1000);
            StorageItem();
        }

        private void WarpToFarmMap()
        {
            MouseEvent.setCursor(initCursorPosition.X, initCursorPosition.Y - 140);
            Thread.Sleep(100);
            MouseEvent.LeftClick();
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(2000);
            MouseEvent.setCursor(initCursorPosition.X, initCursorPosition.Y);
        }

        private void SkillAndWing()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
            MouseEvent.LeftClick();
            Thread.Sleep(500);
            if (INCLUDE_METEOR_STORM)
            {
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F5);
                MouseEvent.LeftClick();
                Thread.Sleep(500);
            }
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F4);
            MouseEvent.LeftClick();
            Thread.Sleep(1);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);
            Thread.Sleep(100);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F1);
        }

        public void KeyReaderFarm(string keyString)
        {
            if (keyString != TRIGGER_KEY) return;
            initCursorPosition = Cursor.Position;

            LoadLocation();
            HealAndStorage();
            WarpToFarmMap();
            int i = 0;
            while (true)
            {
                if (!Helper.isEnableProcess()) break;

                MouseEvent.setCursor(initCursorPosition.X, initCursorPosition.Y);
                SkillAndWing();
                i++;
                if (i == RESTORE_WHEN_SKILL_TIMES)
                {
                    Thread.Sleep(3000);
                    LoadLocation();
                    HealAndStorage();
                    WarpToFarmMap();
                    i = 0;
                }
                Thread.Sleep(DELAY_BETWEEN_SKILL);
            }
        }
    }
}
