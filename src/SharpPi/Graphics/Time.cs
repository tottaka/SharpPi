using System.Threading;

namespace SharpPi.Graphics
{
    public static class Time
    {
        /// <summary>
        /// The time since the last frame. (in seconds)
        /// </summary>
        public static float deltaTime { get; internal set; }

        /// <summary>
        /// The time since the start of the application. (in seconds)
        /// </summary>
        public static float time { get; internal set; }

        /// <summary>
        /// The current average framerate. (average frames per second)
        /// </summary>
        public static double averageFramerate = 0.0;

        /// <summary>
        /// Sleep the calling thread for some time.
        /// </summary>
        public static void Delay(int milliseconds) => Thread.Sleep(milliseconds);
    }
}
