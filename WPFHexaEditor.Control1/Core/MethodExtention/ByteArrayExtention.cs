//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System.Collections.Generic;

namespace WPFHexaEditor.Core.MethodExtention
{
    /// <summary>
    /// Extention methodes for find match in byte[]
    /// </summary>
    public static class ByteArrayExtention
    {
        /// <summary>
        /// Finds all index of byte find
        /// </summary>
        public static IEnumerable<long> FindIndexOf(this byte[] self, byte[] candidate)
        {
            if (!IsEmptyLocate(self, candidate))
            {
                for (int i = 0; i < self.Length; i++)
                {
                    if (!IsMatch(self, i, candidate))
                        continue;

                    yield return i;
                }
            }
        }

        /// <summary>
        /// Check if match is finded
        /// </summary>
        private static bool IsMatch(byte[] array, long position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Check if can find
        /// </summary>
        private static bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }
    }
}