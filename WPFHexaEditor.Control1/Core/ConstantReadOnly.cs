//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

namespace WPFHexaEditor.Core
{
    public static class ConstantReadOnly
    {
        public static readonly string HexLineInfoStringFormat = "x8";
        public static readonly string HexStringFormat = "x";

        public const long LARGE_FILE_LENGTH = 52428800L;    //50 MB
        public const int COPY_BLOCK_SIZE = 131072;          //128 KB
        public const int FIND_BLOCK_SIZE = 1048576;         //1 MB
    }
}