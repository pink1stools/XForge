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
    public partial class Warning : Form
    {
        public Warning()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Properties.Settings.Default.DontShow = this.checkBox1.Checked;
            //Properties.Settings.Default.Save();
        }

        
    }
}
