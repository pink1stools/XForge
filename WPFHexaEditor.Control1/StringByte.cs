//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
// Contributor: Janus Tida
//////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using WPFHexaEditor.Core;
using WPFHexaEditor.Core.Bytes;
using WPFHexaEditor.Core.CharacterTable;
using WPFHexaEditor.Core.Interface;

namespace WPFHexaEditor.Control
{
    internal partial class StringByte : TextBlock, IByteControl
    {
        //Global variable
        private HexaEditor _parent;
        private TBLStream _TBLCharacterTable = null;

        //event
        public event EventHandler Click;
        public event EventHandler RightClick;
        public event EventHandler MouseSelection;
        public event EventHandler ByteModified;
        public event EventHandler MoveNext;
        public event EventHandler MovePrevious;
        public event EventHandler MoveRight;
        public event EventHandler MoveLeft;
        public event EventHandler MoveUp;
        public event EventHandler MoveDown;
        public event EventHandler MovePageDown;
        public event EventHandler MovePageUp;
        public event EventHandler ByteDeleted;
        public event EventHandler EscapeKey;
        public event EventHandler CTRLZKey;
        public event EventHandler CTRLVKey;
        public event EventHandler CTRLCKey;
        public event EventHandler CTRLAKey;

        /// <summary>
        /// Load ressources dictionnary
        /// </summary>
        /// <param name="url"></param>
        private void LoadDictionary(string url)
        {
            var ttRes = new ResourceDictionary() { Source = new Uri(url, UriKind.Relative) };
            Resources.MergedDictionaries.Add(ttRes);
        }

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="parent"></param>
        public StringByte(HexaEditor parent)
        {
            LoadDictionary("/WPFHexaEditor;component/Resources/Dictionary/ToolTipDictionary.xaml");

            //Default properties
            Width = 12;
            Height = 22;
            Focusable = true;
            DataContext = this;
            Padding = new Thickness(0);
            TextAlignment = TextAlignment.Center;

            //Binding
            var txtBinding = new Binding()
            {
                Source = FindResource("ByteToolTip"),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWay
            };

            SetBinding(TextBlock.ToolTipProperty, txtBinding);

            //Event
            MouseEnter += UserControl_MouseEnter;
            MouseLeave += UserControl_MouseLeave;
            KeyDown += UserControl_KeyDown;
            MouseDown += StringByteLabel_MouseDown;

            //Parent hexeditor
            _parent = parent;
        }

        #region DependencyProperty

        /// <summary>
        /// Position in file
        /// </summary>
        public long BytePositionInFile
        {
            get { return (long)GetValue(BytePositionInFileProperty); }
            set { SetValue(BytePositionInFileProperty, value); }
        }

        public static readonly DependencyProperty BytePositionInFileProperty =
            DependencyProperty.Register("BytePositionInFile", typeof(long), typeof(StringByte), new PropertyMetadata(-1L));

        /// <summary>
        /// Used for selection coloring
        /// </summary>
        public bool FirstSelected
        {
            get { return (bool)GetValue(FirstSelectedProperty); }
            set { SetValue(FirstSelectedProperty, value); }
        }

        public static readonly DependencyProperty FirstSelectedProperty =
            DependencyProperty.Register("FirstSelected", typeof(bool), typeof(StringByte), new PropertyMetadata(true));

        /// <summary>
        /// Byte used for this instance
        /// </summary>
        public byte? Byte
        {
            get { return (byte?)GetValue(ByteProperty); }
            set { SetValue(ByteProperty, value); }
        }

        public static readonly DependencyProperty ByteProperty =
            DependencyProperty.Register("Byte", typeof(byte?), typeof(StringByte),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Byte_PropertyChanged)));

        private static void Byte_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                if (e.NewValue != null)
                {
                    if (e.NewValue != e.OldValue)
                    {
                        if (ctrl.Action != ByteAction.Nothing && ctrl.InternalChange == false)
                            ctrl.ByteModified?.Invoke(ctrl, new EventArgs());

                        ctrl.UpdateLabelFromByte();
                        ctrl.UpdateVisual();
                    }
                }
                else
                    ctrl.UpdateLabelFromByte();
        }

        /// <summary>
        /// Next Byte of this instance (used for TBL/MTE decoding)
        /// </summary>
        public byte? ByteNext
        {
            get { return (byte?)GetValue(ByteNextProperty); }
            set { SetValue(ByteNextProperty, value); }
        }

        public static readonly DependencyProperty ByteNextProperty =
            DependencyProperty.Register("ByteNext", typeof(byte?), typeof(StringByte),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ByteNext_PropertyChanged)));

        private static void ByteNext_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                if (e.NewValue != e.OldValue)
                {
                    ctrl.UpdateLabelFromByte();
                    ctrl.UpdateVisual();
                }
        }

        /// <summary>
        /// The Focused Property;
        /// </summary>
        public bool IsFocus
        {
            get { return (bool)GetValue(IsFocusProperty); }
            set { SetValue(IsFocusProperty, value); }
        }

        public static readonly DependencyProperty IsFocusProperty =
            DependencyProperty.Register("IsFocus", typeof(bool), typeof(StringByte),
                new FrameworkPropertyMetadata(false, IsFocus_PropertyChanged));

        private static void IsFocus_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                if (e.NewValue != e.OldValue)
                    ctrl.UpdateVisual();        
        }

        /// <summary>
        /// Get or set if control as selected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(StringByte),
                new FrameworkPropertyMetadata(false, new PropertyChangedCallback(IsSelected_PropertyChangedCallBack)));

        private static void IsSelected_PropertyChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                if (e.NewValue != e.OldValue)
                    ctrl.UpdateVisual();
        }

        /// <summary>
        /// Get of Set if control as marked as highlighted
        /// </summary>
        public bool IsHighLight
        {
            get { return (bool)GetValue(IsHighLightProperty); }
            set { SetValue(IsHighLightProperty, value); }
        }

        public static readonly DependencyProperty IsHighLightProperty =
            DependencyProperty.Register("IsHighLight", typeof(bool), typeof(StringByte),
                new FrameworkPropertyMetadata(false,
                    new PropertyChangedCallback(IsHighLight_PropertyChanged)));

        private static void IsHighLight_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                if (e.NewValue != e.OldValue)
                    ctrl.UpdateVisual();
        }

        /// <summary>
        /// Used to prevent StringByteModified event occurc when we dont want!
        /// </summary>
        public bool InternalChange
        {
            get { return (bool)GetValue(InternalChangeProperty); }
            set { SetValue(InternalChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InternalChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InternalChangeProperty =
            DependencyProperty.Register("InternalChange", typeof(bool), typeof(StringByte), new PropertyMetadata(false));

        /// <summary>
        /// Action with this byte
        /// </summary>
        public ByteAction Action
        {
            get { return (ByteAction)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action", typeof(ByteAction), typeof(StringByte),
                new FrameworkPropertyMetadata(ByteAction.Nothing,
                    new PropertyChangedCallback(Action_ValueChanged),
                    new CoerceValueCallback(Action_CoerceValue)));

        private static object Action_CoerceValue(DependencyObject d, object baseValue)
        {
            ByteAction value = (ByteAction)baseValue;

            if (value != ByteAction.All)
                return baseValue;
            else
                return ByteAction.Nothing;
        }

        private static void Action_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                if (e.NewValue != e.OldValue)
                    ctrl.UpdateVisual();
        }

        #endregion DependencyProperty

        #region Characters tables

        /// <summary>
        /// Show or not Multi Title Enconding (MTE) are loaded in TBL file
        /// </summary>
        public bool TBL_ShowMTE
        {
            get { return (bool)GetValue(TBL_ShowMTEProperty); }
            set { SetValue(TBL_ShowMTEProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TBL_ShowMTE.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TBL_ShowMTEProperty =
            DependencyProperty.Register("TBL_ShowMTE", typeof(bool), typeof(StringByte), 
                new FrameworkPropertyMetadata(true, 
                    new PropertyChangedCallback(TBL_ShowMTE_PropetyChanged)));

        private static void TBL_ShowMTE_PropetyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                ctrl.UpdateLabelFromByte();
        }

        /// <summary>
        /// Type of caracter table are used un hexacontrol.
        /// For now, somes character table can be readonly but will change in future
        /// </summary>
        public CharacterTableType TypeOfCharacterTable
        {
            get { return (CharacterTableType)GetValue(TypeOfCharacterTableProperty); }
            set { SetValue(TypeOfCharacterTableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeOfCharacterTable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeOfCharacterTableProperty =
            DependencyProperty.Register("TypeOfCharacterTable", typeof(CharacterTableType), typeof(StringByte),
                new FrameworkPropertyMetadata(CharacterTableType.ASCII,
                    new PropertyChangedCallback(TypeOfCharacterTable_PropertyChanged)));

        private static void TypeOfCharacterTable_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StringByte ctrl)
                ctrl.UpdateLabelFromByte();
        }

        public TBLStream TBLCharacterTable
        {
            get
            {
                return _TBLCharacterTable;
            }
            set
            {
                _TBLCharacterTable = value;
            }
        }

        #endregion Characters tables

        /// <summary>
        /// Update control label from byte property
        /// </summary>
        private void UpdateLabelFromByte()
        {
            if (Byte != null)
            {
                switch (TypeOfCharacterTable)
                {
                    case CharacterTableType.ASCII:
                        Text = ByteConverters.ByteToChar(Byte.Value).ToString();
                        Width = 12;
                        break;

                    case CharacterTableType.TBLFile:
                        ReadOnlyMode = !_TBLCharacterTable.AllowEdit;

                        if (_TBLCharacterTable != null)
                        {
                            string content = "#";

                            if (TBL_ShowMTE)
                                if (ByteNext.HasValue)
                                {
                                    string MTE = (ByteConverters.ByteToHex(Byte.Value) + ByteConverters.ByteToHex(ByteNext.Value));
                                    content = _TBLCharacterTable.FindMatch(MTE, true);
                                }

                            if (content == "#")
                                content = _TBLCharacterTable.FindMatch(ByteConverters.ByteToHex(Byte.Value), true);

                            Text = content;

                            //TODO: CHECK FOR AUTO ADAPT TO CONTENT AND FONTSIZE
                            switch (DTE.TypeDTE(content))
                            {
                                case DTEType.DualTitleEncoding:
                                    Width = 12 + content.Length * 2.2D;
                                    break;
                                case DTEType.MultipleTitleEncoding:
                                    Width = 12 + content.Length * 4.2D + (FontSize / 2);
                                    break;
                                case DTEType.EndLine:
                                    Width = 24;
                                    break;
                                case DTEType.EndBlock:
                                    Width = 34;
                                    break;
                                default:
                                    Width = 12;
                                    break;
                            }
                        }
                        else
                            goto case CharacterTableType.ASCII;
                        break;
                }
            }
            else
                Text = "";
        }

        /// <summary>
        /// Update Background,foreground and font property
        /// </summary>
        public void UpdateVisual()
        {
            FontFamily = _parent.FontFamily;
            SolidColorBrush Foreground2 = new SolidColorBrush(_parent.Foreground);
            if (IsFocus)
            {
                Foreground = Foreground2;
                Background = Brushes.Gainsboro;
            }
            else if (IsSelected)
            {
                SolidColorBrush ForegroundContrast2 = new SolidColorBrush(_parent.ForegroundContrast);
                FontWeight = _parent.FontWeight;
                Foreground = ForegroundContrast2;

                if (FirstSelected)
                    Background = _parent.SelectionFirstColor;
                else
                    Background = _parent.SelectionSecondColor;
            }
            else if (IsHighLight)
            {
                FontWeight = _parent.FontWeight;
                Foreground = Foreground2;

                Background = _parent.HighLightColor;
            }
            else if (Action != ByteAction.Nothing)
            {
                switch (Action)
                {
                    case ByteAction.Modified:
                        FontWeight = FontWeights.Bold;
                        Background = _parent.ByteModifiedColor;
                        Foreground = Foreground2;
                        break;

                    case ByteAction.Deleted:
                        FontWeight = FontWeights.Bold;
                        Background = _parent.ByteDeletedColor;
                        Foreground = Foreground2;
                        break;
                }
            }
            else //TBL COLORING
            {
                FontWeight = _parent.FontWeight;
                Background = Brushes.Transparent;
                Foreground = Foreground2;

                if (TypeOfCharacterTable == CharacterTableType.TBLFile)
                    switch (DTE.TypeDTE(Text))
                    {
                        case DTEType.DualTitleEncoding:
                            Foreground = _parent.TBL_DTEColor;
                            break;
                        case DTEType.MultipleTitleEncoding:
                            Foreground = _parent.TBL_MTEColor;
                            break;
                        case DTEType.EndLine:
                            Foreground = _parent.TBL_EndLineColor;
                            break;
                        case DTEType.EndBlock:
                            Foreground = _parent.TBL_EndBlockColor;
                            break;
                        default:
                            Foreground = _parent.TBL_DefaultColor;
                            break;
                    }
            }

            UpdateAutoHighLiteSelectionByteVisual();
        }

        private void UpdateAutoHighLiteSelectionByteVisual()
        {
            //Auto highlite selectionbyte
            if (_parent.AllowAutoHightLighSelectionByte && _parent.SelectionByte != null)
                if (Byte == _parent.SelectionByte && !IsSelected)
                    Background = _parent.AutoHighLiteSelectionByteBrush;
        }

        /// <summary>
        /// Get or set if control as in read only mode
        /// </summary>
        public bool ReadOnlyMode { get; set; } = false;

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyValidator.IsIgnoredKey(e.Key))
            {
                e.Handled = true;
                return;
            }
            else if (KeyValidator.IsUpKey(e.Key))
            {
                e.Handled = true;
                MoveUp?.Invoke(this, new EventArgs());

                return;
            }
            else if (KeyValidator.IsDownKey(e.Key))
            {
                e.Handled = true;
                MoveDown?.Invoke(this, new EventArgs());

                return;
            }
            else if (KeyValidator.IsLeftKey(e.Key))
            {
                e.Handled = true;
                MoveLeft?.Invoke(this, new EventArgs());

                return;
            }
            else if (KeyValidator.IsRightKey(e.Key))
            {
                e.Handled = true;
                MoveRight?.Invoke(this, new EventArgs());

                return;
            }
            else if (KeyValidator.IsPageDownKey(e.Key))
            {
                e.Handled = true;
                MovePageDown?.Invoke(this, new EventArgs());

                return;
            }
            else if (KeyValidator.IsPageUpKey(e.Key))
            {
                e.Handled = true;
                MovePageUp?.Invoke(this, new EventArgs());

                return;
            }
            else if (KeyValidator.IsDeleteKey(e.Key))
            {
                if (!ReadOnlyMode)
                {
                    e.Handled = true;
                    ByteDeleted?.Invoke(this, new EventArgs());

                    return;
                }
            }
            else if (KeyValidator.IsBackspaceKey(e.Key))
            {
                if (!ReadOnlyMode)
                {
                    e.Handled = true;
                    ByteDeleted?.Invoke(this, new EventArgs());

                    if (BytePositionInFile > 0)
                        MovePrevious?.Invoke(this, new EventArgs());

                    return;
                }
            }
            else if (KeyValidator.IsEscapeKey(e.Key))
            {
                e.Handled = true;
                EscapeKey?.Invoke(this, new EventArgs());
                return;
            }
            else if (KeyValidator.IsCtrlZKey(e.Key))
            {
                e.Handled = true;
                CTRLZKey?.Invoke(this, new EventArgs());
                return;
            }
            else if (KeyValidator.IsCtrlVKey(e.Key))
            {
                e.Handled = true;
                CTRLVKey?.Invoke(this, new EventArgs());
                return;
            }
            else if (KeyValidator.IsCtrlCKey(e.Key))
            {
                e.Handled = true;
                CTRLCKey?.Invoke(this, new EventArgs());
                return;
            }
            else if (KeyValidator.IsCtrlAKey(e.Key))
            {
                e.Handled = true;
                CTRLAKey?.Invoke(this, new EventArgs());
                return;
            }

            //MODIFY ASCII...
            if (!ReadOnlyMode)
            {
                bool isok = false;

                if (Keyboard.GetKeyStates(Key.CapsLock) == KeyStates.Toggled)
                {
                    if (Keyboard.Modifiers != ModifierKeys.Shift && e.Key != Key.RightShift && e.Key != Key.LeftShift)
                    {
                        Text = KeyValidator.GetCharFromKey(e.Key).ToString();
                        isok = true;
                    }
                    else if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key != Key.RightShift && e.Key != Key.LeftShift)
                    {
                        isok = true;
                        Text = KeyValidator.GetCharFromKey(e.Key).ToString().ToLower(); 
                    }
                }
                else
                {
                    if (Keyboard.Modifiers != ModifierKeys.Shift && e.Key != Key.RightShift && e.Key != Key.LeftShift)
                    {
                        Text = KeyValidator.GetCharFromKey(e.Key).ToString().ToLower(); 
                        isok = true;
                    }
                    else if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key != Key.RightShift && e.Key != Key.LeftShift)
                    {
                        isok = true;
                        Text = KeyValidator.GetCharFromKey(e.Key).ToString();
                    }
                }

                //Move focus event
                if (isok)
                    if (MoveNext != null)
                    {
                        Action = ByteAction.Modified;
                        Byte = ByteConverters.CharToByte(Text.ToString()[0]);

                        MoveNext(this, new EventArgs());
                    }
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Byte != null)
                if (Action != ByteAction.Modified &&
                    Action != ByteAction.Deleted &&
                    Action != ByteAction.Added &&
                    !IsSelected && !IsHighLight && !IsFocus)
                    Background = _parent.MouseOverColor;

            UpdateAutoHighLiteSelectionByteVisual();

            if (e.LeftButton == MouseButtonState.Pressed)
                MouseSelection?.Invoke(this, e);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Byte != null)
                if (Action != ByteAction.Modified &&
                    Action != ByteAction.Deleted &&
                    Action != ByteAction.Added &&
                    !IsSelected && !IsHighLight && !IsFocus)
                    Background = Brushes.Transparent;

            UpdateAutoHighLiteSelectionByteVisual();
        }

        private void StringByteLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Focus();

                Click?.Invoke(this, e);
            }

            if (e.RightButton == MouseButtonState.Pressed)            
                RightClick?.Invoke(this, e);            
        }
    }
}
