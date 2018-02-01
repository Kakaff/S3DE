using glfw3;
using S3DE.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    internal sealed class Window
    {
        static Window instance;
        
        internal static glfw3.Window window => instance.GLFW_window;

        glfw3.Window GLFW_window;
        bool isFocused = false;
        bool regainedFocus = false;
        bool lostFocus = false;
        float aspect;

        internal static bool IsFocused => instance.isFocused;
        internal static bool RegainedFocus => instance.regainedFocus;
        internal static bool LostFocus => instance.lostFocus;
        private Window() { }

        internal static bool IsCloseRequested => instance.GLFW_window.ShouldClose();

        internal static void SwapBuffer() => instance.GLFW_window.SwapBuffers();

        internal static void MakeCurrentContext() => Glfw.MakeContextCurrent(instance.GLFW_window);

        internal static void CreateWindow(string title)
        {
            if (instance == null)
            {
                Console.WriteLine("Creating new window");
                instance = new Window();
                instance.CreateGLFWWindow((int)Game.Resolution.x,(int)Game.Resolution.y, title);
                instance.aspect = Game.Resolution.x / Game.Resolution.y;
            }
        }

        void CreateGLFWWindow(int width, int height, string title)
        {
            GLFW_window = new glfw3.Window(width, height, title);
        }

        internal static void PollEvents()
        {
            Glfw.PollEvents();
            int f = Glfw.GetWindowAttrib(S3DE.Engine.Graphics.Window.window, (int)glfw3.State.Focused);

            instance.regainedFocus = false;
            instance.lostFocus = false;
            if (f == 1)
            {
                if (!instance.isFocused)
                    instance.regainedFocus = true;
                instance.isFocused = true;
            }
            else
            {
                if (instance.isFocused)
                    instance.lostFocus = true;
                instance.isFocused = false;
            }
                
        }

        public static float AspectRatio => instance.aspect;
        
        
        internal static void SetResolution(int width, int height)
        {
            Game.Resolution = new Vector2(width, height);
        }
        
        internal static void ResizeWindow()
        {
            Glfw.SetWindowSize(instance.GLFW_window, (int)Game.Resolution.x, (int)Game.Resolution.y);
            instance.aspect = Game.Resolution.x / Game.Resolution.y;
            Renderer.OnWindowResized_Internal();
        }

        internal static void SetFullScreen()
        {

        }

        internal static void SetWindowed()
        {

        }
    }
}
