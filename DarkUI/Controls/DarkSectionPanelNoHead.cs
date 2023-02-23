using DarkUI.Collections;
using DarkUI.Config;
using DarkUI.Extensions;
using DarkUI.Forms;
using DarkUI.Icons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkSectionPanelNoHead : DarkScrollView
    {
        #region Event Region

        

        #endregion

        #region Field Region

        private bool _disposed;

        //private readonly int _expandAreaSize = 16;
        private int _itemHeight = 20;

        private int _indent = 20;



        #endregion

        #region Property Region





        [Category("Appearance")]
        [Description("Determines the height of tree nodes.")]
        [DefaultValue(20)]
        public int ItemHeight
        {
            get { return _itemHeight; }
            set
            {
                _itemHeight = value;
                MaxDragChange = _itemHeight;
                
            }
        }

        [Category("Appearance")]
        [Description("Determines the amount of horizontal space given by parent node.")]
        [DefaultValue(20)]
        public int Indent
        {
            get { return _indent; }
            set
            {
                _indent = value;
                
            }
        }

        #endregion

        #region Constructor Region

        public DarkSectionPanelNoHead()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            base.Padding = new Padding(1, 25, 1, 1);
        }

        #endregion

        #region Dispose Region

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Event Handler Region

        protected override void OnEnter(System.EventArgs e)
        {
            base.OnEnter(e);

            Invalidate();
        }

        protected override void OnLeave(System.EventArgs e)
        {
            base.OnLeave(e);

            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (Controls.Count > 0)
                Controls[0].Focus();
        }

        #endregion



        #region Paint Region

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            var rect = ClientRectangle;

            // Fill body
            using (var b = new SolidBrush(Colors.GreyBackground))
            {
                g.FillRectangle(b, rect);
            }

            // Draw header
            var bgColor = ContainsFocus ? Colors.BlueBackground : Colors.HeaderBackground;
            var darkColor = ContainsFocus ? Colors.DarkBlueBorder : Colors.DarkBorder;
            var lightColor = ContainsFocus ? Colors.LightBlueBorder : Colors.LightBorder;

            using (var b = new SolidBrush(bgColor))
            {
                var bgRect = new Rectangle(0, 0, rect.Width, 25);
                g.FillRectangle(b, bgRect);
            }

            using (var p = new Pen(darkColor))
            {
                g.DrawLine(p, rect.Left, 0, rect.Right, 0);
                g.DrawLine(p, rect.Left, 25 - 1, rect.Right, 25 - 1);
            }

            using (var p = new Pen(lightColor))
            {
                g.DrawLine(p, rect.Left, 1, rect.Right, 1);
            }

            var xOffset = 3;

            using (var b = new SolidBrush(Colors.LightText))
            {
                var textRect = new Rectangle(xOffset, 0, rect.Width - 4 - xOffset, 25);

                var format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    FormatFlags = StringFormatFlags.NoWrap,
                    Trimming = StringTrimming.EllipsisCharacter
                };

               
            }

            // Draw border
            using (var p = new Pen(Colors.DarkBorder, 1))
            {
                var modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);

                g.DrawRectangle(p, modRect);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Absorb event
        }

        protected override void PaintContent(Graphics g)
        {
            
        }
        
        #endregion
    }
}