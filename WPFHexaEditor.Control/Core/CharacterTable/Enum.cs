//////////////////////////////////////////////
// Apache 2.0  - 2003-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

namespace WPFHexaEditor.Core.CharacterTable
{
    /// <summary>
    /// Type de DTE qui sera utilisé dans les classe de DTE
    /// </summary>
    public enum DTEType
    {
        Invalid = -1,
        ASCII = 0,
        Japonais,
        DualTitleEncoding,
        MultipleTitleEncoding,
        EndLine,
        EndBlock
    }

    public enum DefaultCharacterTableType
    {
        ASCII
        //ADD OTHERTYPE...
    }
}