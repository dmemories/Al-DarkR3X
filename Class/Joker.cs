using Al_DarkR3X.Class;
using Al_DarkR3X.Libs;
using System.Threading;
using WindowsInput.Native;
using static Al_DarkR3X.Configuration;

namespace Al_DarkR3X
{
    public class Joker: Hero, IHeroClass
    {
        private const string BACKSLIDE_LEFT = "VK_1";
        private const string BACKSLIDE = "VK_2";
        private const string BACKSLIDE_RIGHT = "VK_3";
        private const VirtualKeyCode blackSlideVk = VirtualKeyCode.VK_K;

        private void BlackSlide(VirtualKeyCode vk)
        {
            Keyboard.ReleaseKey(Keyboard.ALT_KEY_BYTE);
            Thread.Sleep(DELAY_AFTER_RELEASE_KEY);
            Keyboard.KeyWithALT(vk);
            Thread.Sleep(DELAY_AFTER_RELEASE_KEY);
            Keyboard.ReleaseKey(Keyboard.ALT_KEY_BYTE);
        }

        public void HookAction(string key)
        {
            switch (key)
            {
                case BACKSLIDE_LEFT:
                    isCasting = true;
                    BlackSlide(VirtualKeyCode.VK_4);
                    break;

                case BACKSLIDE:
                    isCasting = true;
                    Keyboard.Press(blackSlideVk);
                    break;

                case BACKSLIDE_RIGHT:
                    isCasting = true;
                    BlackSlide(VirtualKeyCode.VK_5);
                    break;
            }
            isCasting = false;
        }
       
        public bool HasVirtualLoopKey(VirtualKeyCode vk)
        {
            switch (vk)
            {
                case VirtualKeyCode.F3:
                case VirtualKeyCode.VK_F:
                case VirtualKeyCode.VK_D:
                    return true;

                default: return false;
            }
        }

        public LoopClickCallback GetLoopClickCallback(VirtualKeyCode vk) { return null; }
    }
}
