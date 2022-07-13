using WindowsInput.Native;
using System.Collections.Generic;
using System.Drawing;

namespace Al_DarkR3X
{
    internal class Configuration
    {
        // General
        public const int MIN_DELAY = 1;
        public const VirtualKeyCode ASURA_VK_KEY = VirtualKeyCode.F8;
        public const string ENABLE_PROCESS_KEY = "VK_PRIOR";

        public const short DELAY_AFTER_SWITCH_ITEM = 10;
        public const short DELAY_LOOP_LEFT_CLICK = 12;
        public const short DELAY_AFTER_RELEASE_KEY = 20;

        // Modes
        public enum MODE
        {
            ALKAISER, BLUE, JOKER
        }

        public static Dictionary<MODE, KeyValuePair<string, Image>> modeDict = new Dictionary<MODE, KeyValuePair<string, Image>>(){
            {MODE.ALKAISER, new KeyValuePair<string, Image>("Alkaiser", Properties.Resources.MBlack)},
            {MODE.BLUE, new KeyValuePair<string, Image>("Blue", Properties.Resources.Blue)},
            {MODE.JOKER, new KeyValuePair<string, Image>("Joker", Properties.Resources.Joker)},
        };

    }
}
