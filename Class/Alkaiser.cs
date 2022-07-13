using Al_DarkR3X.Class;
using Al_DarkR3X.Libs;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput.Native;
using static Al_DarkR3X.Configuration;

namespace Al_DarkR3X
{
    public class Alkaiser: Hero, IHeroClass
    {

        [DllImport("User32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const short DELAY_FLASH_HIDDEN = 50;
        private const short DELAY_SKILL_BEFORE_ZEN = 115;
        private const short DELAY_SKILL_BEFORE_HIDDEN = 110;
        private const short DELAY_JUMP_BEFORE_HIDDEN = 80;
        private const short DELAY_ZEN_AFTER_RELO = 100;
        private const short DELAY_LOOP_BETWEEEN_FINGER = 106;

        private const string JUMP_HIDDEN_KEY = "VK_6";
        private const string FLASH_HIDDEN_KEY = "VK_5";
        private const string DUEL_KEY = "VK_4";
        private const string ABSORB_ZEN_HIDDEN_KEY = "VK_3";
        private const string ZEN_HIDDEN_KEY = "VK_TAB";
        private const string ABSORB_HIDDEN_KEY = "192";
        private const string RELO_ASURA_KEY = "VK_2";

        bool isWatingAsura = false;
        bool wantHiddenFlash = false;

        public void HookAction(string key)
        {
            if (
               isCasting &&
               key == ABSORB_ZEN_HIDDEN_KEY &&
               wantHiddenFlash == false
           )
            {
                wantHiddenFlash = true;
                RunCastKey(key);
                return;
            }
            if (key == DUEL_KEY)
            {
                if (isCasting)
                {
                    if (!isWatingAsura) return;
                    isWatingAsura = false;
                    Thread.Sleep(17);
                }
                if (!isCasting)
                {
                    RunCastKey(key);
                    return;
                }
            }
            if (isCasting) return;

            switch (key)
            {
                case ABSORB_HIDDEN_KEY:
                case ZEN_HIDDEN_KEY:
                case ABSORB_ZEN_HIDDEN_KEY:
                case JUMP_HIDDEN_KEY:
                case FLASH_HIDDEN_KEY:
                case RELO_ASURA_KEY:
                    RunCastKey(key);
                    return;

                default: break;
            }
        }

        private void RunCastKey(string key)
        {
            isCasting = true;
            int j = 0;
            switch (key)
            {
                case ABSORB_HIDDEN_KEY:
                    Mouse.SwitchMove();
                    Keyboard.Press(VirtualKeyCode.F4);
                    Mouse.LeftClick();
                    Thread.Sleep(DELAY_SKILL_BEFORE_HIDDEN);
                    Hidden();
                    break;

                case ZEN_HIDDEN_KEY:
                    keybd_event(9, 0x45, 0x0001 | 0x0002, 0);
                    Keyboard.Press(VirtualKeyCode.VK_W);
                    Thread.Sleep(DELAY_AFTER_SWITCH_ITEM);
                    Keyboard.Press(VirtualKeyCode.F2);
                    Thread.Sleep(DELAY_SKILL_BEFORE_HIDDEN);
                    Hidden();
                    break;

                case ABSORB_ZEN_HIDDEN_KEY:
                    AbsorbZen();
                    Thread.Sleep(DELAY_SKILL_BEFORE_HIDDEN);
                    Hidden();
                    break;

                case DUEL_KEY:
                    bool skipAsura = false;
                    AbsorbZen();
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
                                Mouse.SwitchMove();
                                Keyboard.Press(ASURA_VK_KEY);
                                Mouse.LeftClick();
                            }
                        }
                        j++;
                    }
                    wantHiddenFlash = false;
                    isWatingAsura = false;
                    break;

                case JUMP_HIDDEN_KEY:
                    Keyboard.Press(VirtualKeyCode.F3);
                    Mouse.LeftClick();
                    Thread.Sleep(10);
                    Keyboard.Press(VirtualKeyCode.VK_T);
                    Thread.Sleep(DELAY_JUMP_BEFORE_HIDDEN);
                    Hidden();
                    break;

                case FLASH_HIDDEN_KEY:
                    Keyboard.Press(VirtualKeyCode.VK_W);
                    Thread.Sleep(DELAY_FLASH_HIDDEN);
                    Keyboard.Press(VirtualKeyCode.VK_E);
                    break;

                case RELO_ASURA_KEY:
                    Keyboard.Press(VirtualKeyCode.VK_W);
                    Thread.Sleep(DELAY_AFTER_SWITCH_ITEM);
                    Keyboard.Press(VirtualKeyCode.F3);
                    Mouse.LeftClick();
                    Thread.Sleep(DELAY_ZEN_AFTER_RELO);
                    Keyboard.Press(VirtualKeyCode.F2);
                    break;
            }
            isCasting = false;
        }

        public static void Hidden()
        {
            Keyboard.Press(VirtualKeyCode.VK_E);
        }

        public static void AbsorbZen()
        {
            Mouse.SwitchMove();
            Keyboard.Press(VirtualKeyCode.F4);
            Mouse.LeftClick();
            Thread.Sleep(DELAY_SKILL_BEFORE_ZEN);
            Keyboard.Press(VirtualKeyCode.F2);
        }

        public bool HasVirtualLoopKey(VirtualKeyCode vk)
        {
            switch (vk)
            {
                case ASURA_VK_KEY:
                case VirtualKeyCode.VK_F:
                case VirtualKeyCode.VK_H:
                case VirtualKeyCode.SPACE:
                    return true;

                default: return false;
            }
        }

        public LoopClickCallback GetLoopClickCallback(VirtualKeyCode vk)
        {
            LoopClickCallback callback;
            switch (vk)
            {
                case VirtualKeyCode.SPACE:
                    callback = delegate () {
                        Keyboard.PressWithoutKeyEvent(VirtualKeyCode.F2);
                        Thread.Sleep(DELAY_LOOP_BETWEEEN_FINGER);
                        Keyboard.PressWithoutKeyEvent(VirtualKeyCode.VK_F);
                        Mouse.LeftClick();
                        Thread.Sleep(DELAY_LOOP_LEFT_CLICK);
                    };
                    break;

                default:
                    callback = null;
                    break;
            }
            return callback;
        }

    }
}
