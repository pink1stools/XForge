using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace QRC_Editoror
{
    public partial class Base : Form
    {
        Main_QRC main_qrc = new Main_QRC();
        private string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        int QRCTapNumber = 0;
        public string AppPath
        { 
            get{ return appPath; }
            set { appPath = value; }
        }
        
        public Base()
        {
            AppDomain.CurrentDomain.AppendPrivatePath("lib");
            InitializeComponent();
            
        }

        public Base(string fi)
        {
            AppDomain.CurrentDomain.AppendPrivatePath("lib");
            InitializeComponent();
            OpenQRC(fi);
        }

        void MDIParent1_MdiChildActivate(object sender, EventArgs e)
        {
            Form childForm2 = new Form();
            int i = this.MdiChildren.Count();
            if (i > 0)
            {
                
                foreach (Form childForm in this.MdiChildren)
                {
                    if (childForm == this.ActiveMdiChild)
                    {
                        childForm2 = childForm;
                    }
                    //childForm2 = childForm;
                }

               
                   
            }
            if(childForm2 != null)
            {
              childForm2.Close();
            }
            
        }

        public void Show_First()
        {
            PSTaskDialog.cTaskDialog.ForceEmulationMode = true;
            try { PSTaskDialog.cTaskDialog.EmulatedFormWidth = Convert.ToInt32("450"); }
            catch (Exception) { PSTaskDialog.cTaskDialog.EmulatedFormWidth = 450; }
            PSTaskDialog.cTaskDialog.ShowTaskDialogBox(this,
                                                   "Warning",
                                                   "Warning",
                                                   "This will rebuild your QRC it is recommended to make a backup before proceeding.",
                                                   "",
                                                   "",
                                                   "Don't show me this message again",
                                                   "",
                                                   "",
                                                   PSTaskDialog.eTaskDialogButtons.OKCancel,
                                                   PSTaskDialog.eSysIcons.Information,
                                                   PSTaskDialog.eSysIcons.Information);
            
            // UpdateResult(res);
        }
        
        public void Activate(object sender, EventArgs e)
        {
            if (this.MdiChildren.Count() > 1)
            {
                foreach (Main_QRC childForm in this.MdiChildren)
                {
                    if (childForm == this.ActiveMdiChild)
                    {
                        main_qrc = childForm;
                    }
                }
            }


        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            this.MDIParent1_MdiChildActivate(sender,  e);
            int i = this.MdiChildren.Count();
            if (i == 0)
            {
                WindowToolStripMenuItem.Visible = false;
                closeToolStripMenuItem.Enabled = false;
            }
                
        }

        private void CloseQRC(Main_QRC main_QRC)
        {
            main_QRC.Close();
            QRCTapNumber = 0;
            foreach (ToolStripItem m in WindowToolStripMenuItem.DropDown.Items)
            {
                QRCTapNumber++;
                m.Tag = QRCTapNumber;
                
            }
            QRCTapNumber = 0;
            foreach (Main_QRC main_QRC2 in this.MdiChildren)
            {
                QRCTapNumber++;
                main_QRC2.Tag = QRCTapNumber;

            }

            if (this.MdiChildren.Length == 0)
            {
                WindowToolStripMenuItem.Visible = false;
                // this.ActiveMdiChild.Close();
                QRCTapNumber = 0;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = AppPath;
            openFileDialog1.Filter = "QRC files (*.qrc; *.qrcf)|*.qrc; *.qrcf|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenQRC(openFileDialog1.FileName);
            }
        }

        private void OpenQRC(string fi)
        {
            closeToolStripMenuItem.Enabled = true;
            QRCTapNumber++;
            int dr1 = fi.LastIndexOf('\\');
            string dr = fi.Remove(0, dr1 + 1);
            WindowToolStripMenuItem.Visible = true;
            Main_QRC newMDIChild = new Main_QRC();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            System.Drawing.Point point = new System.Drawing.Point(0, 0);
            newMDIChild.Location = point;
            newMDIChild.Text = dr;
            newMDIChild.Auto_Load(fi);
            
            newMDIChild.Show();
           
        }

        private void saveQRCFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate(sender, e);
            main_qrc.SaveQRCF();
        }

        private void compressAndSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate(sender, e);
            main_qrc.SaveQRC();
        }

        private void extractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate(sender, e);
            main_qrc.Extract();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Options OptionsChild = new Options();
            OptionsChild.ShowDialog(this);
        }

        private void buildNewQRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK )
            {
                string[] files = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.xml");
                
                if(files[0] != null)
                {
                    string QRCF_Folder = folderBrowserDialog1.SelectedPath;
                    saveFileDialog1.AddExtension = true;
                    saveFileDialog1.Filter = "QRCF files (*.qrcf)|*.qrcf|All files (*.*)|*.*";
                    saveFileDialog1.DefaultExt = "QRCF files (*.qrcf)|*.qrcf";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.InitialDirectory = QRCF_Folder;
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string New_QRCF_File = saveFileDialog1.FileName;
                        BuildNewQRC(files[0], QRCF_Folder, New_QRCF_File);
                    }

                }
                
            }
           
        }

        public void BuildNewQRC(string file, string QRCF_Folder, string New_QRCF_File)
        {
            List<string> sn1 = new List<string>();
            List<string> sn2 = new List<string>();
            List<byte[]> file_bytes = new List<byte[]>();
            List<int> file_sizes = new List<int>();
            List<byte[]> name_bytes = new List<byte[]>();

            byte[] head = new byte[] { 0x51, 0x52, 0x43, 0x46, 0x00, 0x00, 0x01, 0x10 };
            byte[] TOCSize = new byte[4];
            byte[] TOCOffset = { 0x00, 0x00, 0x00, 0x40 };
            byte[] STableSize = new byte[4];
            byte[] STableOffset = new byte[4];
            byte[] TAgstableSize = { 0x00, 0x00, 0x00, 0x1B };
            byte[] TagstableOffset = new byte[4];
            byte[] INTSize = { 0x00, 0x00, 0x00, 0x00 };
            byte[] INTOffset = new byte[4];
            byte[] FLOATCSize = { 0x00, 0x00, 0x00, 0x00 };
            byte[] FLOATOffset = new byte[4];
            byte[] FTSize = new byte[4];
            byte[] FTOffset = new byte[4];
            byte[] Pad1 = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            byte[] qrc = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x1C };

            byte[] ft = new byte[] { 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x38 };

            byte[] last_child_elemen = new byte[4];

            byte[] TAGSTable = { 0x71, 0x72, 0x63, 0x00, 0x66, 0x69, 0x6C, 0x65, 0x2D, 0x74, 0x61, 0x62,
                0x6C, 0x65, 0x00, 0x66, 0x69, 0x6C, 0x65, 0x00, 0x73, 0x72, 0x63, 0x00,
                0x69, 0x64, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            if (File.Exists(file))
            {
                
                string line;
                //FileStream f1 = File.Open(QRCF_Folder, FileMode.Open);
                System.IO.StreamReader f1 =
    new System.IO.StreamReader(file);
                while ((line = f1.ReadLine()) != null)
                {
                    System.Console.WriteLine(line);
                    if(line.Contains("<file src="))
                    {
                        line = line.Replace("\t\t<file src=\"", "");
                        line = line.Replace("\" id=\"", "=");
                        line = line.Replace("\" />", "");
                        line = line.Replace("\"", "");
                        string[] l2 = line.Split('=');
                        if (l2.Length == 2)
                        {
                            sn1.Add(l2[0]);
                            sn2.Add(l2[1]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                }
                f1.Close();
                int y = 0x38;
                int i = 0;
                foreach (string s in sn1)
                {
                    if (File.Exists(QRCF_Folder + "/" + s))
                    {
                        byte[] fb = File.ReadAllBytes(QRCF_Folder + "/" + s);
                        
                        file_sizes.Add(fb.Length);

                        PadToMultipleOf(ref fb, 16);
                        file_bytes.Add(fb);
                        byte[] pad = BitConverter.GetBytes(y);
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(pad);
                        }
                        byte[] bytes = Encoding.ASCII.GetBytes(sn2[i]);
                        byte[] string_bytes = new byte[5 + bytes.Length];
                        Array.Copy(pad, 0, string_bytes, 0, pad.Length);
                        Array.Copy(bytes, 0, string_bytes, 4, bytes.Length);
                        name_bytes.Add(string_bytes);
                        y = y + 0x3C;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                    
                }
                
                i = 0;
                
                byte[] STable = name_bytes.SelectMany(s => s)
                          .ToArray();
                
                STableSize = BitConverter.GetBytes(STable.Length);
               
                PadToMultipleOf(ref STable, 16);

                int ITocS = 60 * sn1.Count;
                TOCSize = BitConverter.GetBytes(ITocS + 56);
                byte[] TOC = new byte[ITocS + 56];
                last_child_elemen = BitConverter.GetBytes(TOC.Length - 60);

                PadToMultipleOf(ref TOC, 16);
                Array.Copy(qrc, TOC, qrc.Length);
                Array.Copy(ft, 0, TOC, qrc.Length, ft.Length);

                STableOffset = BitConverter.GetBytes(TOC.Length+0x40);
                
                TagstableOffset = BitConverter.GetBytes(TOC.Length + STable.Length + 0x40);
                byte[] FTable = file_bytes.SelectMany(s => s)
                          .ToArray();
                FTOffset = BitConverter.GetBytes(TOC.Length + STable.Length + TAGSTable.Length + 0x40);
                FTSize = BitConverter.GetBytes(FTable.Length);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(FTOffset);
                    Array.Reverse(last_child_elemen);
                    Array.Reverse(STableOffset);
                    Array.Reverse(STableSize);
                    Array.Reverse(TOCSize);
                    Array.Reverse(TagstableOffset);
                    Array.Reverse(FTSize);
                }
                INTOffset = FTOffset;
                FLOATOffset = FTOffset;

                MemoryStream ms = new MemoryStream();
                
                ms.Write(head, 0x0, head.Length);
                
                ms.Write(TOCOffset, 0x0, TOCOffset.Length);
                ms.Write(TOCSize, 0x0, TOCSize.Length);
                
                ms.Write(STableOffset, 0x0, STableOffset.Length);
                ms.Write(STableSize, 0x0, STableSize.Length);
                
                ms.Write(TagstableOffset, 0x0, TagstableOffset.Length);
                ms.Write(TAgstableSize, 0x0, TAgstableSize.Length);
                
                ms.Write(INTOffset, 0x0, INTOffset.Length);
                ms.Write(INTSize, 0x0, INTSize.Length);
               
                ms.Write(FLOATOffset, 0x0, FLOATOffset.Length);
                ms.Write(FLOATCSize, 0x0, FLOATCSize.Length);
                
                ms.Write(FTOffset, 0x0, FTOffset.Length);
                ms.Write(FTSize, 0x0, FTSize.Length);
                ms.Write(Pad1, 0x0, Pad1.Length);
                ms.Write(qrc, 0x0, qrc.Length);
                ms.Write(ft, 0x0, ft.Length);
                ms.Write(last_child_elemen, 0x0, last_child_elemen.Length);
                byte[] Tables = ms.ToArray();
                ms.Close();
                FileStream q = File.Open(New_QRCF_File, FileMode.Create);
               
                q.Write(Tables, 0, Tables.Length);

                byte[] previous_brother = {0xFF, 0xFF, 0xFF, 0xFF };
                int next_brother = 0x74;
                byte[] nb = new byte[4] ;
                
                int offset = TOC.Length;
                i = 0;
                int foff = 0;
                int fnoff = 0;
                foreach (byte[] b in name_bytes)
                {
                    nb = BitConverter.GetBytes(next_brother);
                    if (i == sn1.Count - 1)
                    {

                        nb = new byte[]{ 0xFF, 0xFF, 0xFF, 0xFF };
                    }
                   
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(previous_brother);
                        Array.Reverse(nb);
                        

                    }

                    byte[] tocstart = { 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x1C };
                    q.Write(tocstart, 0, tocstart.Length);
                    q.Write(previous_brother, 0, previous_brother.Length);
                    q.Write(nb, 0, nb.Length);
                    byte[] tocmid = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x06 };
                    q.Write(tocmid, 0, tocmid.Length);

                    byte[] foffset = BitConverter.GetBytes(foff);
                    byte[] fsize = BitConverter.GetBytes(file_sizes[i]);
                  
                    byte[] fnoffset = BitConverter.GetBytes(fnoff);
                    byte[] fnsize = BitConverter.GetBytes(b.Length);
                    byte[] v2 = { 0x00, 0x00, 0x00, 0x00 };
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(foffset);
                        Array.Reverse(fsize);
                        Array.Reverse(fnoffset);
                        Array.Reverse(fnsize);
                    }
                    q.Write(foffset, 0, foffset.Length);
                    q.Write(fsize, 0, fsize.Length);
                    byte[] att = { 0x00, 0x00, 0x00, 0x18, 0x00, 0x00, 0x00, 0x07 };
                    q.Write(att, 0, att.Length);
                    q.Write(fnoffset, 0, fnoffset.Length);
                    q.Write(v2, 0, v2.Length);

                    foff = foff + file_sizes[i];

                    foff = (foff + 16 - 1) / 16 * 16;

                    fnoff = fnoff + b.Length;

                    previous_brother = BitConverter.GetBytes(next_brother - 0x3C);
                    next_brother = next_brother + 0x3C;
                   
                    i++;
                }
                q.Write(STable, 0, STable.Length);
                q.Write(TAGSTable, 0, TAGSTable.Length);
                q.Write(FTable, 0, FTable.Length);
                q.Close();
            }

            if(File.Exists(New_QRCF_File))
            {
                OpenQRC(New_QRCF_File);
            }

        }

        private static void PadToMultipleOf(ref byte[] src, int pad)
        {
            int len = (src.Length + pad - 1) / pad * pad;
            Array.Resize(ref src, len);
        }

        private void multiExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Activate(sender, e);
            main_qrc.Multi_extract();

        }
    }
}
