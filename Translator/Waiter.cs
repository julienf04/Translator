using System;
using System.Diagnostics;
using System.Threading;

namespace Translations
{
    /// <summary>
    /// Wait until an event occurs or until a timeout occurs
    /// </summary>
    internal static class Waiter
    {
        /// <summary>
        /// Wait until Action is completed or until a timeout occurs
        /// </summary>
        /// <param name="toExecute">Action to be completed</param>
        public static void WaitForNoExceptionAndSleep(Action toExecute)
        {
            bool successful = false;
            Stopwatch st = new Stopwatch();
            st.Start();

            int i = 0;
            while (!successful)
            {
                try
                {
                    toExecute();
                    successful = true;
                    st.Stop();
                }
                catch (Exception)
                {
                    if (st.ElapsedMilliseconds > MilisecondsToWait)
                        throw;
                    Thread.Sleep(MilisecondsToSleep + i);
                    i += 10;
                }
            }
            Thread.Sleep(MilisecondsToSleep + i);
        }

        public static int MilisecondsToWait { get; set; } = 10000;
        public static int MilisecondsToSleep { get; set; } = 0;
    }
}
