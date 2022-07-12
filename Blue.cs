using System.Windows.Forms;
using System.Drawing;
using WindowsInput;
using System.Threading;
using WindowsInput.Native;

namespace Al_DarkR3X
{
    public class Blue
    {

        private const string TRIGGER_KEY = "VK_SPACE";
        private const int DELAY_BETWEEN_ENTER = 500;
        private const int DELAY_BETWEEN_SKILL = 390;
        private const int RESTORE_WHEN_SKILL_TIMES = 140;
        private const bool STORAGE_ITEM = false;
        private const bool INCLUDE_LORD = true;
        private const bool INCLUDE_STORM_GUST = true;
        private const bool INCLUDE_METEOR_STORM = false;
        private const bool ONLY_CASTING = false;

        private Point initCursorPosition;
        private InputSimulator inputSimulator = new InputSimulator();
        private int rounds = 0;

        private void LoadLocation() {
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_1);
            Thread.Sleep(3000);
        }

        private void StorageItem()
        {
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_9);
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
            Thread.Sleep(100);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_O);
            LowLevelMouse.setCursor(initCursorPosition.X + 80, initCursorPosition.Y - 140);
            Helper.LeftClick(inputSimulator);
            Thread.Sleep(1000);
            if (STORAGE_ITEM) StorageItem();
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
            bool alreadyUseSkill = false;
            if (INCLUDE_LORD)
            {
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                Helper.LeftClick(inputSimulator);
                alreadyUseSkill = true;
            }
            if (INCLUDE_METEOR_STORM)
            {
                if (alreadyUseSkill) Thread.Sleep(500);
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F5);
                Helper.LeftClick(inputSimulator);
                alreadyUseSkill = true;
            }
            if (INCLUDE_STORM_GUST)
            {
                if (alreadyUseSkill) Thread.Sleep(500);
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                Helper.LeftClick(inputSimulator);
            }
            Thread.Sleep(1);
            System.Console.WriteLine(rounds);
            if ((rounds % 20) == 0)
            {
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                Thread.Sleep(500);
            }
            else inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_X);            
            Thread.Sleep(120);
            if (!ONLY_CASTING) inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F1);
        }

        public void KeyReaderFarm(string keyString)
        {
            if (keyString != TRIGGER_KEY) return;
            initCursorPosition = Cursor.Position;

            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_4);
            Helper.keyWithALT(inputSimulator, VirtualKeyCode.VK_0);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_A);
            Thread.Sleep(100);
            if (!ONLY_CASTING)
            {
                LoadLocation();
                HealAndStorage();
                WarpToFarmMap();
            }

            rounds = 0;
            while (true)
            {
                if (!Helper.isEnableProcess()) break;

                LowLevelMouse.setCursor(initCursorPosition.X, initCursorPosition.Y);
                SkillAndWing();
                if (!ONLY_CASTING)
                {
                    rounds++;
                    if (rounds == RESTORE_WHEN_SKILL_TIMES)
                    {
                        Thread.Sleep(3000);
                        LoadLocation();
                        HealAndStorage();
                        WarpToFarmMap();
                        rounds = 0;
                    }
                }
                Thread.Sleep(DELAY_BETWEEN_SKILL);
            }
        }
    }
}
