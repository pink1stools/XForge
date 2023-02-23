//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

namespace WPFHexaEditor.Core
{
    /// <summary>
    /// ByteAction used for ByteModified class
    /// </summary>
    public enum ByteAction
    {
        Nothing,
        Added,
        Deleted,
        Modified,

        /// <summary>
        /// Used in ByteProvider for get list
        /// </summary>
        All
    }

    /// <summary>
    /// Used for coloring mode of selection
    /// </summary>
    public enum FirstColor
    {
        HexByteData,
        StringByteData
    }

    /// <summary>
    /// Mode of Copy/Paste
    /// </summary>
    public enum CopyPasteMode
    {
        Byte,
        HexaString,
        ASCIIString,
        TBLString,
        CSharpCode,
        VBNetCode,
        JavaCode,
        CCode,
        FSharp
    }

    /// <summary>
    /// Used with Copy to code fonction for language are similar to C.
    /// </summary>
    internal enum CodeLanguage
    {
        C,
        CSharp,
        Java,
        FSharp,
        VBNET
    }

    /// <summary>
    /// Used for check label are selected et next label to select...
    /// </summary>
    public enum KeyDownLabel
    {
        FirstChar,
        SecondChar,
        NextPosition
    }

    public enum ByteToString
    {
        /// <summary>
        /// Build-in convertion mode. (recommended)
        /// </summary>
        ByteToCharProcess,

        /// <summary>
        /// System.Text.Encoding.ASCII string encoder
        /// </summary>
        ASCIIEncoding
    }

    /// <summary>
    /// Scrollbar marker
    /// </summary>
    public enum ScrollMarker
    {
        Nothing,
        SearchHighLight,
        Bookmark,
        SelectionStart,
        ByteModified,
        ByteDeleted,
        TBLBookmark
    }

    /// <summary>
    /// Type are opened in byteprovider
    /// </summary>
    public enum ByteProviderStreamType
    {
        File,
        MemoryStream,
        Nothing
    }

    /// <summary>
    /// Type of character are used
    /// </summary>
    public enum CharacterTableType
    {
        ASCII,
        TBLFile
    }

    /// <summary>
    /// Used for control the speed of mouse wheel
    /// </summary>
    public enum MouseWheelSpeed : int
    {
        VerySlow = 1,
        Slow = 3,
        Normal = 5,
        Fast = 7,
        VeryFast = 9
    }

    /// <summary>
    /// Used with ByteHeaderAnalyser for give the file format
    /// </summary>
    internal enum FileFormat
    {
        EXE
        //JPG
        //PNG
        //... add more 
    }
}