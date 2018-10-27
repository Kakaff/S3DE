using S3DE;
using S3DE.Input;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Mouse.SetCursor(CursorMode.LockedAndHidden);
            Time.EnableOversleepAdjustment(true);
            LoadStartScene<SampleScene>();
        }

        protected override S3DE_Vector2 LoadDisplayResolution()
        {
            return new S3DE_Vector2(1280, 720);
        }

        protected override S3DE_Vector2 LoadRenderResolution()
        {
            return new S3DE_Vector2(1280, 720);
        }
    }
}
