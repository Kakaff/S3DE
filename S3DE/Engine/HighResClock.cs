﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace S3DE.Engine
{
    public class HighResClock : EngineClock
    {
        [DllImport("Kernel32.dll",CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

        bool IsSupported;

        protected override long GetTick()
        {
            if (!IsSupported)
                throw new InvalidOperationException("High resolution Clock is not supported on this system!");

            long t;
            GetSystemTimePreciseAsFileTime(out t);

            return t;
        }

        protected override void OnCreation()
        {
            try
            {
                long fileTime;
                GetSystemTimePreciseAsFileTime(out fileTime);
                IsSupported = true;
            } catch (EntryPointNotFoundException ex)
            {
                Console.WriteLine("High resolution Clock is not supported on this system!");
                IsSupported = false;
            }
        }

        protected override void StartClock()
        {
        }

        protected override void StopClock()
        {
        }
    }
}