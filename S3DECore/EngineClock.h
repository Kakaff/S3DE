#pragma once

extern"C" void WaitForNextFrame(bool b);

__declspec(dllexport) void InitClock(void);

__declspec(dllexport) long long int GetTime();