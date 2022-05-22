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
        private const int DELAY_BETWEEN_SKILL = 380;
        private const int RESTORE_WHEN_SKILL_TIMES = 70;

        private Point initCursorPosition;
        private InputSimulator inputSimulator = new InputSimulator();

        private void LoadLocation() {
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_1);
            Thread.Sleep(1000);
        }

        private void StorageItem()
        {
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_4);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_9);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_0);
            Thread.Sleep(100);
            LowLevelMouse.setCursor(initCursorPosition.X - 265, initCursorPosition.Y - 350);
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
            LowLevelMouse.setCursor(initCursorPosition.X + 80, initCursorPosition.Y - 140);
            Helper.LeftClick(inputSimulator);
            Thread.Sleep(1000);
            StorageItem();
        }

        private void WarpToFarmMap()
        {
            LowLevelMouse.setCursor(initCursorPosition.X, initCursorPosition.Y - 140);
            Thread.Sleep(100);
            Helper.LeftClick(inputSimulator);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.RETURN);
            Thread.Sleep(2000);
            LowLevelMouse.setCursor(initCursorPosition.X, initCursorPosition.Y);
            Helper.RightClick(inputSimulator);
        }

        private void SkillAndWing()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
            Helper.LeftClick(inputSimulator);
            Thread.Sleep(500);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F4);
            Helper.LeftClick(inputSimulator);
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

                LowLevelMouse.setCursor(initCursorPosition.X, initCursorPosition.Y);
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
