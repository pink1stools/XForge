//////////////////////////////////////////////
// Apache 2.0  - 2016-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
// Contributor: Janus Tida
//////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPFHexaEditor.Core;
using WPFHexaEditor.Core.Bytes;
using WPFHexaEditor.Core.Interfaces;

namespace WPFHexaEditor.Control
{
    internal class HexByte : TextBlock, IByteControl
    {
        public HexByte(HexaEditor parent)
        {
            //Default properties
            DataContext = this;
            Focusable = true;
            Width = 20;
            TextAlignment = TextAlignment.Left;
            Padding = new Thickness(2, 0, 0, 0);

            //Event
            KeyDown += UserControl_KeyDown;
            MouseDown += HexChar_MouseDown;
            MouseEnter += UserControl_MouseEnter;
            MouseLeave += UserControl_MouseLeave;

            //Parent hexeditor
            _parent = parent;
        }

        private KeyDownLabel _keyDownLabel = KeyDownLabel.FirstChar;
        private HexaEditor _parent;

        public event EventHandler ByteModified;
        public event EventHandler MouseSelection;
        public event EventHandler Click;
        public event EventHandler RightClick;
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


        #region DependencyProperty

        /// <summary>
        /// Position in file
        /// </summary>
        public long BytePositionInFile
        {
            get => (long)GetValue(BytePositionInFileProperty);
            set => SetValue(BytePositionInFileProperty, value);
        }

        public static readonly DependencyProperty BytePositionInFileProperty =
            DependencyProperty.Register("BytePositionInFile", typeof(long), typeof(HexByte), new PropertyMetadata(-1L));

        /// <summary>
        /// Action with this byte
        /// </summary>
        public ByteAction Action
        {
            get => (ByteAction)GetValue(ActionProperty);
            set => SetValue(ActionProperty, value);
        }

        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action", typeof(ByteAction), typeof(HexByte),
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
            if (d is HexByte ctrl)
                if (e.NewValue != e.OldValue)
                    ctrl.UpdateVisual();
        }

        /// <summary>
        /// Used for selection coloring
        /// </summary>
        public bool FirstSelected
        {
            get => (bool)GetValue(FirstSelectedProperty);
            set => SetValue(FirstSelectedProperty, value);
        }

        public static readonly DependencyProperty FirstSelectedProperty =
            DependencyProperty.Register("FirstSelected", typeof(bool), typeof(HexByte), new PropertyMetadata(true));

        /// <summary>
        /// Byte used for this instance
        /// </summary>
        public byte? Byte
        {
            get => (byte?)GetValue(ByteProperty);
            set => SetValue(ByteProperty, value);
        }

        public static readonly DependencyProperty ByteProperty =
            DependencyProperty.Register("Byte", typeof(byte?), typeof(HexByte),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(Byte_PropertyChanged)));

        private static void Byte_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HexByte ctrl)
                if (e.NewValue != null)
                {
                    if (e.NewValue != e.OldValue)
                    {
                        if (ctrl.Action != ByteAction.Nothing && ctrl.InternalChange == false)
                            ctrl.ByteModified?.Invoke(ctrl, new EventArgs());

                        ctrl.UpdateLabelFromByte();
                    }
                }
                else
                    ctrl.UpdateLabelFromByte();
        }

        /// <summary>
        /// Used to prevent ByteModified event occurc when we dont want! 
        /// </summary>
        public bool InternalChange
        {
            get => (bool)GetValue(InternalChangeProperty);
            set => SetValue(InternalChangeProperty, value);
        }

        // Using a DependencyProperty as the backing store for InternalChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InternalChangeProperty =
            DependencyProperty.Register("InternalChange", typeof(bool), typeof(HexByte), new PropertyMetadata(false));

        #endregion

        /// <summary>
        /// Get or set if control as in read only mode
        /// </summary>
        public bool ReadOnlyMode { get; set; } = false;

        /// <summary>
        /// Get or Set if control as selected
        /// </summary>
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(HexByte),
                new FrameworkPropertyMetadata(false,
                    new PropertyChangedCallback(IsSelected_PropertyChange)));


        private static void IsSelected_PropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HexByte ctrl)
                if (e.NewValue != e.OldValue)
                {
                    ctrl._keyDownLabel = KeyDownLabel.FirstChar;
                    ctrl.UpdateVisual();
                }
        }

        /// <summary>
        /// Get of Set if control as marked as highlighted
        /// </summary>                        
        public bool IsHighLight
        {
            get => (bool)GetValue(IsHighLightProperty);
            set => SetValue(IsHighLightProperty, value);
        }

        public static readonly DependencyProperty IsHighLightProperty =
            DependencyProperty.Register("IsHighLight", typeof(bool), typeof(HexByte),
                new FrameworkPropertyMetadata(false,
                    new PropertyChangedCallback(IsHighLight_PropertyChanged)));

        private static void IsHighLight_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HexByte ctrl)
                if (e.NewValue != e.OldValue)
                {
                    ctrl._keyDownLabel = KeyDownLabel.FirstChar;
                    ctrl.UpdateVisual();
                }
        }

        public bool IsFocus
        {
            get => (bool)GetValue(IsFocusProperty);
            set => SetValue(IsFocusProperty, value);
        }

        public static readonly DependencyProperty IsFocusProperty =
            DependencyProperty.Register("IsFocus", typeof(bool), typeof(HexByte),
                new FrameworkPropertyMetadata(false, IsFocus_PropertyChanged));

        private static void IsFocus_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HexByte ctrl)
                if (e.NewValue != e.OldValue)
                {
                    ctrl._keyDownLabel = KeyDownLabel.FirstChar;
                    ctrl.UpdateVisual();
                }
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
                Foreground = Brushes.Gainsboro;
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
                Foreground = Foreground;
                Background = _parent.HighLightColor;
            }
            else if (Action != ByteAction.Nothing)
            {
                FontWeight = FontWeights.Bold;
                Foreground = Foreground2;

                switch (Action)
                {
                    case ByteAction.Modified:
                        Background = _parent.ByteModifiedColor;
                        break;
                    case ByteAction.Deleted:
                        Background = _parent.ByteDeletedColor;
                        break;
                }
            }
            else
            {
                FontWeight = _parent.FontWeight;
                Background = Brushes.Transparent;
                Foreground = Foreground2;
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


        private void UpdateLabelFromByte()
        {
            if (Byte != null)
            {
                var chArr = ByteConverters.ByteToHexCharArray(Byte.Value);
                Text = new string(chArr);
            }
            else            
                Text = string.Empty;            
        }

        private void HexChar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Focus();
                Click?.Invoke(this, e);
            }

            if (e.RightButton == MouseButtonState.Pressed)            
                RightClick?.Invoke(this, e);            
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyValidator.IsUpKey(e.Key))
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
                e.Handled = true;
                ByteDeleted?.Invoke(this, new EventArgs());

                MovePrevious?.Invoke(this, new EventArgs());

                return;
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

            //MODIFY BYTE
            if (!ReadOnlyMode)
                if (KeyValidator.IsHexKey(e.Key))
                {
                    string key;
                    if (KeyValidator.IsNumericKey(e.Key))
                        key = KeyValidator.GetDigitFromKey(e.Key).ToString();
                    else
                        key = e.Key.ToString().ToLower();

                    //Update byte
                    char[] ByteValueCharArray = ByteConverters.ByteToHexCharArray(Byte.Value);
                    switch (_keyDownLabel)
                    {
                        case KeyDownLabel.FirstChar:
                            ByteValueCharArray[0] = key.ToCharArray()[0];
                            _keyDownLabel = KeyDownLabel.SecondChar;
                            Action = ByteAction.Modified;
                            Byte = ByteConverters.HexToByte(ByteValueCharArray[0].ToString() + ByteValueCharArray[1].ToString())[0];
                            break;
                        case KeyDownLabel.SecondChar:
                            ByteValueCharArray[1] = key.ToCharArray()[0];
                            _keyDownLabel = KeyDownLabel.NextPosition;

                            Action = ByteAction.Modified;
                            Byte = ByteConverters.HexToByte(ByteValueCharArray[0].ToString() + ByteValueCharArray[1].ToString())[0];

                            //Move focus event
                            MoveNext?.Invoke(this, new EventArgs());
                            break;
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
    }
}
