using S3DE;
using S3DE.Input;
using S3DE.Maths;
using System;

namespace SampleGame
{
    public class MyGame : Game
    {
        public static void Main(String[] args)
        {
            try
            {
                new MyGame().Run();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Press Any Key To Exit...");
                Console.ReadKey();
            }
        }

        public override string GameName => "Sample Game";

        protected override void Initialize()
        {
            
        }

        protected override void Start()
        {
            Enable_Powersaving(true);
            Mouse.SetCursor(CursorMode.LockedAndHidden);
            LoadStartScene<SampleScene>();
        }

        protected override Vector2 LoadDisplayResolution()
        {
            return new Vector2(1280, 720);
        }

        protected override Vector2 LoadRenderResolution()
        {
            return new Vector2(1280, 720);
        }
    }
}
