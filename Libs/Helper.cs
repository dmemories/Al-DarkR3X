using System.Collections.Generic;
using System.Windows.Forms;
using WindowsInput.Native;
using static Al_DarkR3X.Configuration;

namespace Al_DarkR3X
{
    class Helper
    {
        public static bool enableProcess = false;

        public static bool IsProcessEnabled()
        {
            return enableProcess && Helper.IsGameWindowActive();
        }

        public static bool IsGameWindowActive()
        {
            string activeTitle = ActiveWindow.GetActiveWindowTitle().ToLower();
            return activeTitle.Contains("ro-") || activeTitle.Contains("pvp") || activeTitle.Contains("ragnarok");
        }

        public static VirtualKeyCode GetVirtualKeyCodeFromKey(Keys keyCode)
        {
            IDictionary<Keys, VirtualKeyCode> mapVirtualKeyCode = new Dictionary<Keys, VirtualKeyCode>();
            mapVirtualKeyCode.Add(Keys.D1, ASURA_VK_KEY);
            mapVirtualKeyCode.Add(Keys.F, VirtualKeyCode.VK_F);
            mapVirtualKeyCode.Add(Keys.H, VirtualKeyCode.VK_H);
            mapVirtualKeyCode.Add(Keys.Space, VirtualKeyCode.SPACE);
            mapVirtualKeyCode.Add(Keys.D, VirtualKeyCode.VK_D);
            mapVirtualKeyCode.Add(Keys.F3, VirtualKeyCode.F3);

            return mapVirtualKeyCode.ContainsKey(keyCode) ? mapVirtualKeyCode[keyCode] : VirtualKeyCode.NONAME;
        }

    }
}
