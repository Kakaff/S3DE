using S3DE;
using S3DECore.Input;
using S3DECore.Math;
using S3DECore.Graphics;
using System;

namespace SampleGame
{
    /// <summary>
    /// A test game mainly used for debugging and developing new features.
    /// </summary>
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
            Mouse.Cursor.SetCursorMode(CursorMode.Hidden);

            Renderer.VSyncEnabled = true;
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
