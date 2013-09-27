using System;

namespace Alba.Interop
{
    internal partial class Native
    {
        /// <summary>The maximum character length of a path.</summary>
        internal const int MAX_PATH = 260;
        /// <summary>The maximum character length of a type name.</summary>
        internal const int MAX_TYPE = 80;

        /// <summary>Creates a LONG value by concatenating the specified values.</summary>
        public static Int32 MakeLong (Int16 wLow, Int16 wHigh)
        {
            return wLow | wHigh << 16;
        }

        /// <summary>Creates a LONG value by concatenating the specified values.</summary>
        public static UInt32 MakeLong (UInt16 wLow, UInt16 wHigh)
        {
            return (UInt32)(wLow | wHigh << 16);
        }
    }
}