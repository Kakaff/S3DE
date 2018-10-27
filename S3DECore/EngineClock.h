#pragma once

extern"C" void WaitForNextFrame(void);
__declspec(dllexport) void InitClock(void);

__declspec(dllexport) long long int GetTime();