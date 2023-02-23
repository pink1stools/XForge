using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace QRC_Editoror
{
    public partial class Main_QRC : Form
    {
        
        public Main_QRC()
        {
            InitializeComponent();
            //new_Tab1.Show();
        }
       
        public void Auto_Load(string fname)
        {
            new_Tab1.Auto_Load(fname);
        }

        public void SaveQRCF()
        {
            new_Tab1.SaveQRCFToolStripMenuItem1_Click();
        }

        public void SaveQRC()
        {
            new_Tab1.CompressQRCToolStripMenuItem_Click();
        }

        public void Extract()
        {
            new_Tab1.ExtractAllToolStripMenuItem_Click();
        }

        public void Multi_extract()
        {
            Multi_Extract multi_Extract = new Multi_Extract(new_Tab1.Fileinfo);
            multi_Extract.ShowDialog(this.ParentForm);
        }

    }
}
