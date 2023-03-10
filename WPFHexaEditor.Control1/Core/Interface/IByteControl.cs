//////////////////////////////////////////////
// Apache 2.0  - 2017
// Author       : Janus Tida
// Contributor  : Derek Tremblay
//////////////////////////////////////////////

using System;

namespace WPFHexaEditor.Core.Interface
{
    /// <summary>
    /// All byte control inherit from this interface.
    /// This interface is used to reduce the code when manipulate byte control
    /// </summary>
    internal interface IByteControl
    {
        //Properties
        long BytePositionInFile { get; set; }
        ByteAction Action { get; set; }
        byte? Byte { get; set; }
        bool IsFocus { get; set; }
        bool IsHighLight { get; set; }
        bool IsSelected { get; set; }
        bool FirstSelected { get; set; }
        bool ReadOnlyMode { get; set; }
        bool InternalChange { get; set; }

        //Methods
        void UpdateVisual();

        //Events
        event EventHandler ByteModified;
        event EventHandler MouseSelection;
        event EventHandler Click;
        event EventHandler RightClick;
        event EventHandler MoveNext;
        event EventHandler MovePrevious;
        event EventHandler MoveRight;
        event EventHandler MoveLeft;
        event EventHandler MoveUp;
        event EventHandler MoveDown;
        event EventHandler MovePageDown;
        event EventHandler MovePageUp;
        event EventHandler ByteDeleted;
        event EventHandler EscapeKey;
        event EventHandler CTRLZKey;
        event EventHandler CTRLVKey;
        event EventHandler CTRLCKey;
        event EventHandler CTRLAKey;
    }
}
