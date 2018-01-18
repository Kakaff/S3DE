using glfw3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine.Graphics
{
    internal sealed class Window
    {
        glfw3.Window GLFW_window;

        internal Window(int width, int height, string title) => CreateWindow(width, height, title);

        internal bool IsCloseRequested => GLFW_window.ShouldClose();

        internal void SwapBuffer() => GLFW_window.SwapBuffers();

        internal void MakeCurrentContext() => Glfw.MakeContextCurrent(GLFW_window);

        void CreateWindow(int width, int height, string title) => 
            GLFW_window = new glfw3.Window(width, height, title);

        public void PollEvents() => Glfw.PollEvents();
    }
}
