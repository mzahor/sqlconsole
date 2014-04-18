using System;
using System.Runtime.InteropServices;
using SQLConsole.BizLogic.TaskRunning;

namespace SqlConsole.BizLogic.TaskRunning
{
    public class PerformanceCounter : IPerformanceCounter
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long perfcount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long freq);

        /// <summary>
        /// Gets time in seconds which action executing takes
        /// </summary>
        /// <param name="action">Action to run</param>
        /// <returns>Time in seconds</returns>
        public TimeSpan GetExecutingTime(Action action)
        {
            long startCount;
            long stopCount;
            
            // start profiling
            QueryPerformanceCounter(out startCount);

            //execute action
            action();

            // stop profiling
            QueryPerformanceCounter(out stopCount);
            
            // calculate results
            long elapsedCount = stopCount - startCount;
            long frequency;
            QueryPerformanceFrequency(out frequency);
            double elapsedSeconds = (double)elapsedCount / frequency;
            
            return TimeSpan.FromSeconds(elapsedSeconds);
        }
    }
}
