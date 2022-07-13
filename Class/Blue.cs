using System.Windows.Forms;
using System.Drawing;
using WindowsInput;
using System.Threading;
using WindowsInput.Native;
using Al_DarkR3X.Class;
using Al_DarkR3X.Libs;

namespace Al_DarkR3X
{
    public class Blue: Hero, IHeroClass
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
            Keyboard.KeyWithALT(VirtualKeyCode.VK_1);
            Thread.Sleep(3000);
        }

        private void StorageItem()
        {
            Keyboard.KeyWithALT(VirtualKeyCode.VK_9);
            Thread.Sleep(100);
            Mouse.SetCursor(initCursorPosition.X - 265, initCursorPosition.Y - 350);
            Keyboard.KeyWithALT(VirtualKeyCode.VK_E);

            Thread.Sleep(1000);
            Keyboard.KeyWithRightClick();
            Keyboard.KeyWithALT(VirtualKeyCode.VK_E);
            Thread.Sleep(100);
            LoadLocation();
        }

        private void HealAndStorage()
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F9);
            Thread.Sleep(100);
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_O);
            Mouse.SetCursor(initCursorPosition.X + 80, initCursorPosition.Y - 140);
            Mouse.LeftClick();
            Thread.Sleep(1000);
            if (STORAGE_ITEM) StorageItem();
        }

        private void WarpToFarmMap()
        {
            Mouse.SetCursor(initCursorPosition.X, initCursorPosition.Y - 140);
            Thread.Sleep(100);
            Mouse.LeftClick();
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Keyboard.KeyWithALT(VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Keyboard.KeyWithALT(VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Keyboard.KeyWithALT(VirtualKeyCode.RETURN);
            Thread.Sleep(DELAY_BETWEEN_ENTER);
            Keyboard.KeyWithALT(VirtualKeyCode.RETURN);
            Thread.Sleep(2000);
            Mouse.SetCursor(initCursorPosition.X, initCursorPosition.Y);
            Mouse.RightClick();
        }

        private void SkillAndWing()
        {
            bool alreadyUseSkill = false;
            if (INCLUDE_LORD)
            {
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F3);
                Mouse.LeftClick();
                alreadyUseSkill = true;
            }
            if (INCLUDE_METEOR_STORM)
            {
                if (alreadyUseSkill) Thread.Sleep(500);
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F5);
                Mouse.LeftClick();
                alreadyUseSkill = true;
            }
            if (INCLUDE_STORM_GUST)
            {
                if (alreadyUseSkill) Thread.Sleep(500);
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.F4);
                Mouse.LeftClick();
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

        public void HookAction(string key)
        {
            if (key != TRIGGER_KEY) return;
            initCursorPosition = Cursor.Position;

            Keyboard.KeyWithALT(VirtualKeyCode.VK_4);
            Keyboard.KeyWithALT(VirtualKeyCode.VK_0);
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
                if (!Helper.IsProcessEnabled()) break;

                Mouse.SetCursor(initCursorPosition.X, initCursorPosition.Y);
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


        public bool HasVirtualLoopKey(VirtualKeyCode vk) { return false; }
        public LoopClickCallback GetLoopClickCallback(VirtualKeyCode vk) { return null; }
    }
}
