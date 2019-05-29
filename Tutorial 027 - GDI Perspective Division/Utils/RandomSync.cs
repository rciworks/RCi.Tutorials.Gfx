using System.Runtime.CompilerServices;

namespace RCi.Tutorials.Gfx.Utils
{
    /// <summary>
    /// Synchronized <see cref="System.Random"/>.
    /// </summary>
    public static class RandomSync
    {
        #region // pure

        private static readonly object m_Lock = new object();
        private static readonly System.Random m_Random = new System.Random((int)System.DateTime.UtcNow.Ticks);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next()
        {
            lock (m_Lock)
            {
                return m_Random.Next();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next(int maxValue)
        {
            lock (m_Lock)
            {
                return m_Random.Next(maxValue);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next(int minValue, int maxValue)
        {
            lock (m_Lock)
            {
                return m_Random.Next(minValue, maxValue);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void NextBytes(byte[] buffer)
        {
            lock (m_Lock)
            {
                m_Random.NextBytes(buffer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble()
        {
            lock (m_Lock)
            {
                return m_Random.NextDouble();
            }
        }

        #endregion

        #region // extensions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble(double maxValue)
        {
            lock (m_Lock)
            {
                return NextDouble() * maxValue;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble(double minValue, double maxValue)
        {
            lock (m_Lock)
            {
                return minValue + NextDouble(maxValue - minValue);
            }
        }

        #endregion
    }
}
