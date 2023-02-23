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

        public const long LARGEFILELENGTH = 52_428_800L;    //50 MB
        public const int COPYBLOCKSIZE = 131_072;          //128 KB
        public const int FINDBLOCKSIZE = 1_048_576;         //1 MB
    }
}