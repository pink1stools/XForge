using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QRC_Editoror
{
    public partial class Multi_Extract : Form
    {
        DataTable Fileinfo = new DataTable();
        private bool _preventMove = true;

        public Multi_Extract(DataTable file)
        {
            InitializeComponent();
            
            Set_Up();
            Fileinfo.Merge(file);
        }

        void Set_Up()
        {

            Fileinfo.Columns.Add("name");
            Fileinfo.Columns.Add("TOC_Offset", typeof(byte[]));
            Fileinfo.Columns.Add("Row", typeof(Int32));
            Fileinfo.Columns.Add("name_offset");
            Fileinfo.Columns.Add("attributes");
            Fileinfo.Columns.Add("parent_offset");
            Fileinfo.Columns.Add("previous_brother_offset");
            Fileinfo.Columns.Add("next_brother_offset");
            Fileinfo.Columns.Add("first_child_offset");
            Fileinfo.Columns.Add("last_child_offset");
            Fileinfo.Columns.Add("attribute1_name_offset");
            Fileinfo.Columns.Add("attribute1_type");
            Fileinfo.Columns.Add("attribute1_variable_1", typeof(byte[]));
            Fileinfo.Columns.Add("attribute1_variable_2", typeof(byte[]));
            Fileinfo.Columns.Add("attribute2_name_offset");
            Fileinfo.Columns.Add("attribute2_type");
            Fileinfo.Columns.Add("attribute2_variable_1");
            Fileinfo.Columns.Add("attribute2_variable_2");
            Fileinfo.Columns.Add("attribute1_variable_1_add");

           checkedListBox1.DataSource = Fileinfo;
            checkedListBox1.DisplayMember = "name";
            checkedListBox1.ValueMember = "Row";

        }

        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 274;
            const int SC_MOVE = 0xF010;

            if (_preventMove)
            {
                switch (message.Msg)
                {
                    case WM_SYSCOMMAND:
                        int command = message.WParam.ToInt32() & 0xfff0;
                        if (command == SC_MOVE)
                            return;
                        break;
                }
            }

            base.WndProc(ref message);
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
