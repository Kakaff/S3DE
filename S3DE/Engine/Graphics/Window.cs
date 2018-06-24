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
        bool vsync = false;
        float aspect;

        internal static bool IsFocused => instance.isFocused;
        internal static bool RegainedFocus => instance.regainedFocus;
        internal static bool LostFocus => instance.lostFocus;
        internal static bool IsFullScreen => instance.isFullScreen;
        internal static bool VSync => instance.vsync;

        private Window() { }

        internal static bool IsCloseRequested => instance.GLFW_window.ShouldClose();

        internal static void SwapBuffer() => instance.GLFW_window.SwapBuffers();

        internal static void DestroyWindow() { Glfw.DestroyWindow(instance.GLFW_window); instance.Dispose(); instance = null; }

        internal static bool IsCurrentContext()
        {
            GLFWwindow w = Glfw.GetCurrentContext();

            return !(w == null);
        }
        internal static void MakeCurrentContext() {
            Console.WriteLine("Setting GLFW_Window as Current Context");
            Glfw.MakeContextCurrent(instance.GLFW_window);
            
        }

        internal static void CreateWindow(string title)
        {
            if (instance == null)
            {
                instance = new Window();
                instance.CreateGLFWWindow((int)Game.DisplayResolution.X,(int)Game.DisplayResolution.Y, title);
                instance.aspect = Game.DisplayResolution.X / Game.DisplayResolution.Y;
                
            }
        }

        void CreateGLFWWindow(int width, int height, string title)
        {
            GLFW_window = Glfw.CreateWindow(width, height, title, null, null);
        }

        void Dispose()
        {
            GLFW_window = null;
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
        
        internal static void SetTitle(string title)
        {
            Glfw.SetWindowTitle(window, title);
        }

        internal static void SetResolution(int width, int height)
        {
            Game.DisplayResolution = new S3DE_Vector2(width, height);
        }
        
        internal static void ResizeWindow()
        {
            Console.WriteLine($"Setting window size to {(int)Game.DisplayResolution.X}*{(int)Game.DisplayResolution.Y}");
            Glfw.SetWindowSize(instance.GLFW_window, (int)Game.DisplayResolution.X, (int)Game.DisplayResolution.Y);
            instance.aspect = Game.DisplayResolution.X / Game.DisplayResolution.Y;
            Renderer.OnWindowResized_Internal();
        }

        internal static void SetFullScreen(bool value)
        {
            instance.isFullScreen = value;
            Glfw.SetWindowMonitor(instance.GLFW_window, value ? Glfw.GetPrimaryMonitor() : null, 0, 0, 
                (int)Renderer.DisplayResolution.X, (int)Renderer.DisplayResolution.Y, Renderer.RefreshRate);
        }

        internal static void SetVSync(bool value)
        {
            if (value)
            {
                Glfw.SwapInterval(1);
                instance.vsync = true;
            }
            else if (!value)
            {
                Glfw.SwapInterval(0);
                instance.vsync = false;
            }
        } 
    }
}
