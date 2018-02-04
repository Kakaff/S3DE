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
        
        internal static glfw3.GLFWwindow window => instance.GLFW_window;

        glfw3.GLFWwindow GLFW_window;
        bool isFocused = false;
        bool regainedFocus = false;
        bool lostFocus = false;
        bool isFullScreen = false;
        float aspect;

        internal static bool IsFocused => instance.isFocused;
        internal static bool RegainedFocus => instance.regainedFocus;
        internal static bool LostFocus => instance.lostFocus;
        internal static bool IsFullScreen => instance.isFullScreen;

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
                instance.CreateGLFWWindow((int)Game.DisplayResolution.x,(int)Game.DisplayResolution.y, title);
                instance.aspect = Game.DisplayResolution.x / Game.DisplayResolution.y;
            }
        }

        void CreateGLFWWindow(int width, int height, string title)
        {
            GLFW_window = Glfw.CreateWindow(width, height, title, null, null);
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
            Game.DisplayResolution = new Vector2(width, height);
        }
        
        internal static void ResizeWindow()
        {
            Glfw.SetWindowSize(instance.GLFW_window, (int)Game.DisplayResolution.x, (int)Game.DisplayResolution.y);
            instance.aspect = Game.DisplayResolution.x / Game.DisplayResolution.y;
            Renderer.OnWindowResized_Internal();
        }

        internal static void SetFullScreen(bool value)
        {
            instance.isFullScreen = value;
            Glfw.SetWindowMonitor(instance.GLFW_window, value ? Glfw.GetPrimaryMonitor() : null, 0, 0, 
                (int)Renderer.DisplayResolution.x, (int)Renderer.DisplayResolution.y, Renderer.RefreshRate);
        }

        internal static void SetWindowed()
        {

        }
    }
}
