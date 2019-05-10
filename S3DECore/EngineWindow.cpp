#include "EngineWindow.h"
#include <GLFW\glfw3.h>

using namespace S3DECore;

enum CursorMode:int {Normal = 0,Hidden = 1,LockedAndHidden = 2};

GLFWwindow* window;
bool Window::vsync = false;

DLL_Export void S3DECore::Extern_SetWindowHint(int hint, int value)
{
	Window::SetWindowHint(hint, value);
}

DLL_Export void Extern_SetWindowSize(int width, int height) {
	Window::SetResolution(width, height);
}

DLL_Export int Extern_GetAttribute(int attr) {
	return glfwGetWindowAttrib(window,attr);
}

DLL_Export int Extern_GetKey(int key) {
	return glfwGetKey(window, key);
}

DLL_Export void Extern_GetCursorPos(double &x, double &y) {
	glfwGetCursorPos(window, &x, &y);
}

DLL_Export void Extern_SetCursorPos(double x, double y) {
	glfwSetCursorPos(window, x, y);
}

DLL_Export void Extern_SetCursor(CursorMode mode) 
{
	switch (mode) {
	case Normal: glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_NORMAL); break;
	case Hidden: glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_HIDDEN); break;
	case LockedAndHidden: glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED); break;
	}
}

DLL_Export void Extern_SetSwapInterval(int v) 
{
	Window::SwapInterval(v);
	Window::SetVsync(v == 0 ? false : true);
}

void Window::SetWindowHint(int hint, int value) {
	glfwWindowHint(hint, value);
}

void Window::SetVsync(bool b) {
	vsync = b;
}

bool Window::VsyncEnabled() {
	return vsync;
}

bool Window::CreateWindow()
{
	window = glfwCreateWindow(1280, 720, "", NULL, NULL);
	if (window == NULL)
		return false;
	
	glfwMakeContextCurrent(window);
	glfwSwapInterval(0);
	return true;
}

void Window::DestroyWindow()
{
	glfwDestroyWindow(window);
}

void Window::SetResolution(int width, int height)
{
	glfwSetWindowSize(window, width, height);
}


void Window::SwapBuffers()
{
	glfwSwapBuffers(window);
}

void Window::SwapInterval(int v)
{
	glfwSwapInterval(v);
}

bool Window::IsCloseRequested() {
	return glfwWindowShouldClose(window);
}


