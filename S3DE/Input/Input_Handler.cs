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
            } else if (Game.LostFocus)
            {
                Mouse.ClearMouseState();
                Keyboard.ClearKeyStates();
            }
        }
        
    }
}
