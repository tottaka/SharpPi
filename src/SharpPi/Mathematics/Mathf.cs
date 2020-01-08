using System;

using OpenTK;
using OpenTK.Graphics;

namespace SharpPi
{
    public static class Mathf
    {
        /// <summary>
        /// 3.141593
        /// </summary>
        public const float PI = 3.141593f;

        /// <summary>
        /// 6.283186
        /// </summary>
        public const float TwoPI = 6.283186f;

        /// <summary>
        /// Positive infinity.
        /// </summary>
        public const float Infinity = float.PositiveInfinity;

        /// <summary>
        /// Negative infinity.
        /// </summary>
        public const float NegativeInfinity = float.NegativeInfinity;

        /// <summary>
        /// 0.01745329
        /// </summary>
        public const float Deg2Rad = 0.01745329f;

        /// <summary>
        /// 57.29578
        /// </summary>
        public const float Rad2Deg = 57.29578f;

        public static float Lerp(float from, float to, float time) => (1.0f - time) * from + time * to;
        public static double Lerp(double from, double to, double time) => (1.0d - time) * from + time * to;
        public static float Clamp(float value, float min, float max) => value > max ? max : value < min ? min : value;
        public static int Clamp(int value, int min, int max) => value > max ? max : value < min ? min : value;

        /// <summary>
        /// Computes the angle (in degrees) from one <see cref="Vector2"/> to another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="a">Starting point</param>
        /// <param name="b">Ending point</param>
        /// <returns></returns>
        public static float Bearing(Vector2 a, Vector2 b)
        {
            // if (a1 = b1 and a2 = b2) throw an error 
            float theta = (float)Math.Atan2((double)(b.X - a.X), (double)(a.Y - b.Y));
            if (theta < 0.0)
                theta += TwoPI;
            return Rad2Deg * theta;
        }

        /// <summary>
        /// Combines two Int32 values into an Int64(long).
        /// Example usage: Size attribute.
        /// </summary>
        /// <param name="left">The first value.</param>
        /// <param name="right">The second value.</param>
        public static long Combine(int left, int right) => (long)left << 32 | (uint)right;

        /// <summary>
        /// Split an Int64(long) into two Int32 values.
        /// Example usage: Size attribute.
        /// </summary>
        /// <param name="value">The Int64 to split</param>
        /// <returns>A tuple containing the two Int32 values.</returns>
        public static Tuple<int, int> Split(long value)
        {
            byte[] valueData = BitConverter.GetBytes(value);
            return new Tuple<int, int>(BitConverter.ToInt32(valueData, 0), BitConverter.ToInt32(valueData, 4));
        }

        public static Color4 Offset(this Color4 color, float r, float g, float b, float a) => new Color4(Clamp(color.R + r, 0.0f, 1.0f), Clamp(color.G + g, 0.0f, 1.0f), Clamp(color.B + b, 0.0f, 1.0f), Clamp(color.A + a, 0.0f, 1.0f));
    }
}
