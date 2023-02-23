//////////////////////////////////////////////
// Apache 2.0  - 2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using WPFHexaEditor.Core.Bytes;

namespace WPFHexaEditor.Core.Interfaces
{
    public interface IByteProvider
    {
        //Properties
        bool CanRead { get; }
        bool CanSeek { get; }
        bool CanUndo { get; }
        bool CanWrite { get; }
        bool EOF { get; }
        string FileName { get; set; }
        bool IsEmpty { get; }
        bool IsOnLongProcess { get; }
        bool IsOpen { get; }
        bool IsUndoEnabled { get; set; }
        long Length { get; }
        double LongProcessProgress { get; }
        long Position { get; set; }
        bool ReadOnlyMode { get; set; }
        MemoryStream Stream { get; set; }
        ByteProviderStreamType StreamType { get; }
        int UndoCount { get; }
        Stack<ByteModified> UndoStack { get; }

        //Events
        event EventHandler ChangesSubmited;
        event EventHandler Closed;
        event EventHandler FillWithByteCompleted;
        event EventHandler LongProcessProgressCanceled;
        event EventHandler LongProcessProgressChanged;
        event EventHandler LongProcessProgressCompleted;
        event EventHandler LongProcessProgressStarted;
        event EventHandler PositionChanged;
        event EventHandler ReadOnlyChanged;
        event EventHandler ReplaceByteCompleted;
        event EventHandler StreamOpened;
        event EventHandler Undone;

        //Methods
        bool CanCopy(long selectionStart, long selectionStop);
        void Close();
        void FillWithByte(long startPosition, long length, byte b);
        IEnumerable<long> FindIndexOf(byte[] bytesTofind, long startPosition = 0);
        IEnumerable<long> FindIndexOf(string stringToFind, long startPosition = 0);
        byte? GetByte(long position, bool copyChange = true);
        long[] GetByteCount();
        long GetSelectionLenght(long selectionStart, long selectionStop);
        void OpenFile();
        int Read(byte[] destination, int offset, int count);
        byte[] Read(int count);
        int ReadByte();
        void ReplaceByte(long startPosition, long length, byte original, byte replace);
        void SubmitChanges();
        bool SubmitChanges(string newFilename, bool overwrite = false);
        void Undo();
    }
}