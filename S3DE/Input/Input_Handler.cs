using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Input
{
    internal static partial class Input_Handler
    {
        internal static void PollInput()
        {
            if (Game.IsFocused)
            {
                //Mouse Input
                Mouse.Update();
                Keyboard.UpdateKeyStates();
            }
        }
        
    }
}
