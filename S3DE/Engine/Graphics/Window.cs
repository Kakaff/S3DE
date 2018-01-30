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

        glfw3.Window GLFW_window;
        float aspect;
        Vector2 res;

        private Window() { }

        internal static bool IsCloseRequested => instance.GLFW_window.ShouldClose();

        internal static void SwapBuffer() => instance.GLFW_window.SwapBuffers();

        internal static void MakeCurrentContext() => Glfw.MakeContextCurrent(instance.GLFW_window);

        internal static void CreateWindow(int width, int height, string title)
        {
            if (instance == null)
            {
                instance = new Window();
                SetResolution(width, height);
                instance.CreateGLFWWindow(width,height, title);
            }
        }

        void CreateGLFWWindow(int width, int height, string title)
        {
            GLFW_window = new glfw3.Window(width, height, title);
        }
        public static void PollEvents() => Glfw.PollEvents();

        public static float AspectRatio => instance.aspect;

        internal static void SetResolution(int width, int height)
        {
            instance.res = new Vector2(width, height);
            instance.aspect = (float)width / (float)height;
        }
    }
}
