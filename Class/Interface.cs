using WindowsInput.Native;

namespace Al_DarkR3X.Class
{
    public interface IHeroClass
    {
        void HookAction(string key);
        bool HasVirtualLoopKey(VirtualKeyCode vk);
        LoopClickCallback GetLoopClickCallback(VirtualKeyCode vk);
    }
}
