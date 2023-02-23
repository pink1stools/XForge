using Microsoft.Win32;
using QRC_Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace XForge
{
    public partial class Form1 : Form
    {
        public DataTable Fileinfo = new DataTable();
        string AppPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        byte[] Filebytes;
        byte[] FTO;
        byte[] FTS;
        byte[] New_Filebytes;
        string QRC_File_Name = "";
        string dwtype = "";

        string New_xml = "";
        string New_folder = "";
        string New_qrcf = "";

        List<string> sn = new List<string>();

        public Form1(string fn)
        {
            AppDomain.CurrentDomain.AppendPrivatePath("lib");
            if (File.Exists("ext/Icon.ico"))
            {
                using (var stream = File.OpenRead("ext/Icon.ico"))
                {
                   // this.Icon = new Icon(stream);
                }
            }
            /* if (File.Exists("ext/splash.png"))
             {
                 using (Images splashScreen = new Images())
                 {
                     splashScreen.BackgroundImage = Image.FromFile("ext/splash.png");
                     splashScreen.ShowDialog();
                 }
             }*/
            Images splashScreen = new Images();
            splashScreen.ShowDialog(); 
            InitializeComponent();
           // byteviewer = new ByteViewer();
            //LoadHTML();
            StartUp();
            Auto_Load(fn);
            string mainDirectory5 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Options.LoadSettings();
            //FileAssociations.RemoveAssociationsSet();
            // FileAssociations.RemoveAssociations2Set();
            //FileAssociations.EnsureAssociationsSet();
            //FileAssociations.EnsureAssociations2Set();
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
           // this.DragDrop += new DragEventHandler(Form1_DragDrop);
        }
        

        public Form1()
        {
            AppDomain.CurrentDomain.AppendPrivatePath("lib");
            if (File.Exists("ext/Icon.ico"))
            {
                using (var stream = File.OpenRead("ext/Icon.ico"))
                {
                    //this.Icon = new Icon(stream);
                    //Images splashScreen = new Images();
                    //splashScreen.BackgroundImage = Image.FromFile("ext/Icon.ico");
                    //splashScreen.ShowDialog();
                }
            }
            /* if (File.Exists("ext/splash.png"))
             {
                 using (Images splashScreen = new Images())
                 {
                     splashScreen.BackgroundImage = Image.FromFile("ext/splash.png");
                     splashScreen.ShowDialog();
                 }
             }*/
            Images splashScreen = new Images();
            splashScreen.ShowDialog();
            InitializeComponent();
            StartUp();
            Options.LoadSettings();

            // FileAssociations.RemoveAssociationsSet();
            // FileAssociations.RemoveAssociations2Set();
            // FileAssociations.EnsureAssociationsSet();
            // FileAssociations.EnsureAssociations2Set();

            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            //this.DragDrop += new DragEventHandler(Form1_DragDrop);
        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string file = files[0];
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && (Path.GetExtension(file) == ".qrc" || Path.GetExtension(file) == ".qrcf" || Path.GetExtension(file) == ".xml")) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            label31.Dock = DockStyle.Fill;
            label31.Text = "Loading...";
            label31.Visible = true;
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files[0] != null)
            {
                string file = files[0];
                string ext = Path.GetExtension(file);
                label31.Dock = DockStyle.Fill;
                label31.Text = "Loading...";
                label31.Visible = true;
                listBox1.Visible = false;
                tableLayoutPanel1.Visible = false;
                if (Path.GetExtension(file) == ".qrc" || Path.GetExtension(file) == ".qrcf")
                {

                    
                    Auto_Load(file);
                    this.Text = file;
                    UpdateTextPosition();
                    toolStrip1.Enabled = true;
                    label31.Visible = false;
                    listBox1.Visible = true;
                    tableLayoutPanel1.Visible = true;
                }
                if (Path.GetExtension(file) == ".xml")
                {
                    label31.Text = "Building New QRC...";
                    label31.Visible = true;
                    //string[] files = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.xml");
                    dwtype = "new";
                    toolStrip1.Enabled = false;
                    tableLayoutPanel1.Enabled = false;
                    listBox1.Visible = false;
                    BNew(file);
                    this.Text = file;
                    UpdateTextPosition();
                    toolStrip1.Enabled = true;
                    label31.Visible = false;
                    listBox1.Visible = true;
                    tableLayoutPanel1.Visible = true;
            label31.Text = "";
            label31.Visible = false;
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
                    if (line.Contains("<file src="))
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

                STableOffset = BitConverter.GetBytes(TOC.Length + 0x40);

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

                byte[] previous_brother = { 0xFF, 0xFF, 0xFF, 0xFF };
                int next_brother = 0x74;
                byte[] nb = new byte[4];

                int offset = TOC.Length;
                i = 0;
                int foff = 0;
                int fnoff = 0;
                foreach (byte[] b in name_bytes)
                {
                    nb = BitConverter.GetBytes(next_brother);
                    if (i == sn1.Count - 1)
                    {

                        nb = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
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

            

        }

        public void Open_File()
        {
            
            if (sn != null)
            {
                sn.Clear();
            }

            byte[] Headsize = new byte[4];
            Array.Copy(Filebytes, 8, Headsize, 0, 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(Headsize);
            }
            int hs = BitConverter.ToInt32(Headsize, 0);


            byte[] Headbytes = new byte[hs];
            Array.Copy(Filebytes, 0, Headbytes, 0, Headbytes.Length);
            QRC u = new QRC(Headbytes);

            byte[] TreeSize = u.Tree_size;
            byte[] TreeOffset = u.Tree_offset;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(TreeSize);
                Array.Reverse(TreeOffset);
            }

            int x = BitConverter.ToInt32(TreeSize, 0);
            int y = BitConverter.ToInt32(TreeOffset, 0);

            byte[] ToCbytes = new byte[x];
            Array.Copy(Filebytes, y, ToCbytes, 0, x);

            Toc t = new Toc(ToCbytes);

            byte[] StringSize = u.Id_table_size;
            byte[] StringOffset = u.Id_table_offset;

            FTS = u.File_table_size;
            FTO = u.File_table_offset;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(StringSize);
                Array.Reverse(StringOffset);
                Array.Reverse(FTO);
            }

            x = BitConverter.ToInt32(StringSize, 0);
            y = BitConverter.ToInt32(StringOffset, 0);
            byte[] STbytes = new byte[x];
            Array.Copy(Filebytes, y, STbytes, 0, x);



            int o = 0;
            listBox1.BeginUpdate();
            foreach (QRC_File w in t.files)
            {

                if (BitConverter.ToInt32(w.attributes, 0) == 2)
                {
                    int Temp = o + 1;
                    byte[] F = t.files[o].attribute2_variable_1;
                    byte[] N;
                    if (o < t.files.Count - 1)
                    {
                        N = t.files[Temp].attribute2_variable_1;
                    }
                    else
                    {
                        N = BitConverter.GetBytes(STbytes.Length);

                    }
                    if (BitConverter.IsLittleEndian)
                    {
                        if (o < t.files.Count - 1)
                        {
                            Array.Reverse(N);
                        }
                    }

                    int f = BitConverter.ToInt32(F, 0);
                    int n = BitConverter.ToInt32(N, 0);

                    byte[] SIZE = BitConverter.GetBytes(n - f);

                    double dValue = BitConverter.ToInt32(SIZE, 0);

                    MemoryStream memStream = new MemoryStream();
                    BinaryFormatter binForm = new BinaryFormatter();
                    memStream.Write(STbytes, 0, STbytes.Length);
                    memStream.Seek(f, SeekOrigin.Begin);
                    int iSize = (int)dValue;
                    byte[] File_Name = new byte[iSize];
                    memStream.Read(File_Name, 0, iSize);
                    byte[] File_Name2 = new byte[iSize - 5];
                    Array.Copy(File_Name, 4, File_Name2, 0, File_Name2.Length);
                    byte[] TOC_Offset = new byte[4];
                    Array.Copy(File_Name, 0, TOC_Offset, 0, TOC_Offset.Length);
                    memStream.Close();
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(w.attributes);
                        Array.Reverse(w.attribute2_variable_1);
                        Array.Reverse(w.attribute2_variable_2);
                    }
                    string utfString = Encoding.UTF8.GetString(File_Name2, 0, File_Name2.Length);
                    sn.Add(utfString);
                    DataRow dr = Fileinfo.NewRow();

                    dr["name"] = utfString;
                    dr["TOC_Offset"] = TOC_Offset;
                    dr["Row"] = o;
                    dr["name_offset"] = BitConverter.ToString(w.name_offset, 0);
                    dr["attributes"] = BitConverter.ToString(w.attributes, 0);
                    dr["parent_offset"] = BitConverter.ToString(w.parent_offset, 0);
                    dr["previous_brother_offset"] = BitConverter.ToString(w.previous_brother_offset, 0);
                    dr["next_brother_offset"] = BitConverter.ToString(w.next_brother_offset, 0);
                    dr["first_child_offset"] = BitConverter.ToString(w.first_child_offset, 0);
                    dr["last_child_offset"] = BitConverter.ToString(w.last_child_offset, 0);
                    dr["attribute1_name_offset"] = BitConverter.ToString(w.attribute1_name_offset, 0);
                    dr["attribute1_type"] = BitConverter.ToString(w.attribute1_type, 0);
                    dr["attribute1_variable_1"] = w.attribute1_variable_1;
                    dr["attribute1_variable_2"] = w.attribute1_variable_2;
                    dr["attribute2_name_offset"] = BitConverter.ToString(w.attribute2_name_offset, 0);
                    dr["attribute2_type"] = BitConverter.ToString(w.attribute2_type, 0);
                    dr["attribute2_variable_1"] = BitConverter.ToString(w.attribute2_variable_1, 0);
                    dr["attribute2_variable_2"] = BitConverter.ToString(w.attribute2_variable_2, 0);

                    Fileinfo.Rows.Add(dr);



                }
                o++;

            }
            listBox1.EndUpdate();

            Enable();
            listBox1.SelectedIndex = 1;
            listBox1.SelectedIndex = 0;

        }

        public void WriteXML(string path, string xname)
        {
            using (System.IO.StreamWriter nfile =

            new System.IO.StreamWriter(path + xname +".xml", false))
            {

                nfile.WriteLine("<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "UTF-8" + '"' + "?> \n <qrc>\n	<file-table>");

                foreach (string s in sn)
                {

                    nfile.WriteLine("		<file src=" + '"' + s + '"' + " id=" + '"' + s + "\" />");
                }

                nfile.WriteLine("	</file-table>\n<qrc>");

            }

        }

        private static void PadToMultipleOf(ref byte[] src, int pad)
        {
            int len = (src.Length + pad - 1) / pad * pad;
            Array.Resize(ref src, len);
        }

        void Auto_Load(string fname)
        {

            if (File.Exists(fname))
            {
                Disable();
                QRC_File_Name = fname;
                
                FileStream q = File.Open(QRC_File_Name, FileMode.Open);

                byte[] Filebytes1 = new byte[q.Length];
                q.Read(Filebytes1, 0, Filebytes1.Length);

                byte[] testmagic = new byte[4];
                Array.Copy(Filebytes1, 0, testmagic, 0, 4);
                byte[] qrcmagic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
                byte[] qrcfmagic = new byte[] { 0x51, 0x52, 0x43, 0x46 };


                if (Check_Magic(testmagic, qrcmagic) == true)
                {
                    MemoryStream m = new MemoryStream();
                    for (long i = 8; i < q.Length; i++)
                        m.WriteByte(Filebytes1[i]);
                    //Filebytes = DecompressZlib(m.ToArray());
                    Open_File();
                }
                else if (Check_Magic(testmagic, qrcfmagic) == true)
                {

                    Filebytes = new byte[Filebytes1.Length];
                    Array.Copy(Filebytes1, 0, Filebytes, 0, Filebytes1.Length);
                    Open_File();
                }
                else
                {
                    MessageBox.Show("Error", "Not a Valid QRC File");
                }
                q.Close();
            }

        }

        bool Check_Magic(byte[] testmagic, byte[] magic)
        {
            bool isqrcf = testmagic.SequenceEqual(magic);
            return isqrcf;
        }

        void Disable()
        {
            
            
            button1.Enabled = false;
            button2.Enabled = false;
            extractAllToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            compressQRCToolStripMenuItem.Enabled = false;
            saveQRCFToolStripMenuItem1.Enabled = false;
            Fileinfo.Clear();
            if (Filebytes != null)
            {
                Array.Clear(Filebytes, 0, Filebytes.Length);
            }

            label2.Text = "";
            label4.Text = "";
            label6.Text = "";
            label8.Text = "";
            label10.Text = "";
            label12.Text = "";
            label14.Text = "";
            label16.Text = "";
            label18.Text = "";
            label20.Text = "";
            label22.Text = "";
            label24.Text = "";
            label26.Text = "";
            label28.Text = "";
            label30.Text = "";
        }

        void Enable()
        {

            button1.Enabled = true;
            button2.Enabled = true;
            extractAllToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            compressQRCToolStripMenuItem.Enabled = true;
            saveQRCFToolStripMenuItem1.Enabled = true;
        }

        void Extract_File(string QRCF_File, Object[] item)
        {
            byte[] OR = item[12] as byte[];
            byte[] ORsize = item[13] as byte[];

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(OR);
                Array.Reverse(ORsize);
            }

            int fto = BitConverter.ToInt32(FTO, 0);
            int or = BitConverter.ToInt32(OR, 0);
            int orsize = BitConverter.ToInt32(ORsize, 0);
            int nor = or + fto;
            MemoryStream memStream2 = new MemoryStream();
            BinaryFormatter binForm2 = new BinaryFormatter();
            memStream2.Write(Filebytes, 0, Filebytes.Length);
            memStream2.Seek(nor, SeekOrigin.Begin);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(OR);
                Array.Reverse(ORsize);
            }

            byte[] F1 = new byte[orsize];
            memStream2.Read(F1, 0, orsize);
            File.WriteAllBytes(QRCF_File, F1);

        }

        void Replace_File(string NEW_File, Object[] item)
        {
            if (File.Exists(NEW_File))
            {

                FileStream q = File.Open(NEW_File, FileMode.Open);

                byte[] Filebytes1 = new byte[q.Length];
                q.Read(Filebytes1, 0, Filebytes1.Length);
                q.Close();
                byte[] New_add = new byte[4];
                New_add = BitConverter.GetBytes(Filebytes1.Length);
                PadToMultipleOf(ref Filebytes1, 16);
                byte[] OR = item[12] as byte[];
                byte[] ORsize = item[13] as byte[];
                int ORrow = (int)item[2];

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(OR);
                    Array.Reverse(ORsize);
                    Array.Reverse(New_add);
                    Array.Reverse(FTS);
                }

                int fto = BitConverter.ToInt32(FTO, 0);
                int fts = BitConverter.ToInt32(FTS, 0);
                int or = BitConverter.ToInt32(OR, 0);
                int orsize = BitConverter.ToInt32(ORsize, 0);
                int orrow = ORrow;
                while (orsize % 16 != 0)
                {
                    orsize++;
                }
                int nor = or + fto;
                int New_Size = Filebytes.Length - orsize + Filebytes1.Length;
                New_Filebytes = new byte[New_Size];

                Array.Copy(Filebytes, 0, New_Filebytes, 0, nor);
                Array.Copy(Filebytes1, 0, New_Filebytes, nor, Filebytes1.Length);
                Array.Copy(Filebytes, nor + orsize, New_Filebytes, nor + Filebytes1.Length, Filebytes.Length - nor - orsize);


                int diff = 0;
                string add = "";
                if (Filebytes1.Length > orsize)
                {
                    diff = Math.Abs(Filebytes1.Length - orsize);
                    fts += diff;
                    add = "add";
                }
                if (Filebytes1.Length < orsize)
                {
                    diff = Math.Abs(orsize - Filebytes1.Length);
                    fts -= diff;
                    add = "sub";
                }
                FTS = BitConverter.GetBytes(fts);
                foreach (DataRow item1 in Fileinfo.Rows)
                {
                    Object[] items2 = item1.ItemArray;



                    if (items2[4].ToString() == "00-00-00-02")
                    {
                        byte[] tocadd = items2[1] as byte[];
                        byte[] Old_add = items2[12] as byte[];
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(tocadd);
                            Array.Reverse(Old_add);
                        }
                        int oldfileadd = BitConverter.ToInt32(Old_add, 0);
                        int fileadd = BitConverter.ToInt32(tocadd, 0) + 0x28 + 0x40;
                        int NEWrow = (int)items2[2];
                        //byte[] New_add = new byte[4];
                        byte[] New_l = new byte[4];

                        if (add == "add")
                        {
                            oldfileadd += diff;
                        }
                        else if (add == "sub")
                        {
                            oldfileadd -= diff;
                        }


                        New_l = BitConverter.GetBytes(oldfileadd);
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(New_l);
                        }
                        if (NEWrow == orrow)
                        {
                            Array.Copy(New_add, 0, New_Filebytes, fileadd, New_add.Length);
                        }

                        if (NEWrow > orrow)
                        {
                            Array.Copy(New_l, 0, New_Filebytes, fileadd - 4, New_l.Length);
                        }
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(tocadd);
                            Array.Reverse(Old_add);
                        }
                    }
                }

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(OR);
                    Array.Reverse(ORsize);
                    Array.Reverse(FTS);
                }
                Array.Copy(FTS, 0, New_Filebytes, 0x34, FTS.Length);
                Array.Resize(ref Filebytes, New_Filebytes.Length);

                Disable();
                Array.Copy(New_Filebytes, 0, Filebytes, 0, New_Filebytes.Length);

                Open_File();

            }
        }

        void StartUp()
        {
            label31.Text = "";
            label31.Dock = DockStyle.Fill;
            if (File.Exists("jpegtran.exe"))
            {
                // ImagesButton.Visible = true;

            }
            toolTip1.SetToolTip(button1, "Extract Selected File");
            toolTip1.SetToolTip(button2, "Replace Selected File");
            Disable();

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

            listBox1.DataSource = Fileinfo;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "Row";



        }
        
        public string Get_MD5(byte[] bytes)
        {
            // Step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            //byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

       

        #region<<buttons>>

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AboutBox1 aboutBox = new AboutBox1();

            //aboutBox.ShowDialog(this);
        }

        private void BuildNewQRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "XMLC files (*.xml)|*.xml|All files (*.*)|*.*";
            
            label31.Text = "Building New QRC...";
            label31.Visible = true;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog2.FileName;
                //string[] files = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.xml");
                dwtype = "new";
                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Visible = false;
                BNew(file);
                //dw();
                /*
                if (files[0] != null)
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
                */

            }
            else
            {
                label31.Text = "";
                label31.Visible = false;
            }


        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem is DataRowView item1)
            {
                int dr1 = item1.Row.ItemArray[0].ToString().LastIndexOf('/');
                string dr = item1.Row.ItemArray[0].ToString().Remove(0, dr1 + 1);
                saveFileDialog1.FileName = dr;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Object[] items2 = item1.Row.ItemArray;
                    Extract_File(saveFileDialog1.FileName, items2);
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem is DataRowView item1)
            {
                int dr1 = item1.Row.ItemArray[0].ToString().LastIndexOf('/');
                string dr = item1.Row.ItemArray[0].ToString().Remove(0, dr1 + 1);
                openFileDialog1.Filter = "All files (*.*)|*.*";
                openFileDialog1.FileName = dr;
                openFileDialog1.FilterIndex = 2;
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Object[] items2 = item1.Row.ItemArray;
                    Replace_File(openFileDialog1.FileName, items2);
                }
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "";
            UpdateTextPosition();
            label31.Text = "";
            label31.Visible = true;
            label31.Dock = DockStyle.Fill;
            Disable();
        }

        private void CompressQRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "QRC files (*.qrc)|*.qrc|All files (*.*)|*.*";
            saveFileDialog1.DefaultExt = "QRC files (*.qrc)|*.qrc";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            label31.Text = "Saving QRC...";
            label31.Visible = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dwtype = "qrc";
                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Visible = false;
                dw();


                /*
                string QRCF_File = saveFileDialog1.FileName;
                FileStream q = File.Open(QRCF_File, FileMode.Create);
                byte[] temp = CompressZlib(Filebytes);
                byte[] New_Magic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
                byte[] FSize = new byte[4];
                FSize = BitConverter.GetBytes(Filebytes.Length);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(FSize);
                }
                q.Write(New_Magic, 0, New_Magic.Length);
                q.Write(FSize, 0, FSize.Length);
                q.Write(temp, 0, temp.Length);
                q.Close();
                */
            }
            else
            {
                label31.Text = "";
                label31.Visible = false;
            }
        }

        private void ExtractAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string t = QRC_File_Name;
            int dr1 = t.LastIndexOf('\\');
            string dr = t.Remove(dr1, t.Length - dr1);
            folderBrowserDialog1.SelectedPath = t;

            label31.Text = "Extracting All Files...";
            label31.Visible = true;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                dwtype = "e_all";

                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Visible = false;
                dw();

                /*
                string folder = folderBrowserDialog1.SelectedPath;

                foreach (DataRow item1 in Fileinfo.Rows)
                {
                    Object[] items2 = item1.ItemArray;
                    string name = items2[0].ToString();


                    string ext = name.Remove(0, name.LastIndexOf('.'));
                    foreach (string[] EXT1 in Options.EXTS)
                    {
                        string EXT2 = EXT1[0];
                        if (ext == EXT2 && EXT1[0] != "")
                        {
                            name.Replace(ext, ext + EXT1[0]);
                        }


                        dr1 = name.LastIndexOf('/');
                        dr = name.Remove(dr1, name.Length - dr1);
                        if (dr.Length != name.Length)
                        {
                            //Directory.CreateDirectory(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + dr);
                            Directory.CreateDirectory(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\" + "\\" + dr);
                        }
                    }


                    //Extract_File(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + name, items2);
                    Extract_File(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\" + name, items2);

                }

                WriteXML(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\");
                */
            }
            else
            {
                label31.Text = "";
                label31.Visible = false;
            }
        }

        private void ImagesButton_Click(object sender, EventArgs e)
        {
            //Images OptionsChild = new Images();
            //Base one = new Base();

            //newMDIChild.MdiParent = this;
            // one.Show();


            //OptionsChild.ShowDialog(this);
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem is DataRowView item1)
            {

                Object[] items2 = item1.Row.ItemArray;
                label2.Text = BitConverter.ToString(items2[1] as byte[]);
                //label2.Text = items2[1].ToString();
                label4.Text = items2[4].ToString();
                label6.Text = items2[5].ToString();
                label8.Text = items2[6].ToString();
                label10.Text = items2[7].ToString();
                label12.Text = items2[8].ToString();
                label14.Text = items2[9].ToString();
                label16.Text = items2[10].ToString();
                label18.Text = items2[11].ToString();
                //label20.Text = items2[12].ToString();
                //int x = BitConverter.ToInt32(items2[12] as byte[], 0);
                //int x = BitConverter.ToInt32(items2[12] as byte[], 0);
                label20.Text = BitConverter.ToString(items2[12] as byte[]);
                label22.Text = BitConverter.ToString(items2[13] as byte[]);

                //label22.Text = items2[13].ToString();
                label24.Text = items2[14].ToString();
                label26.Text = items2[15].ToString();
                label28.Text = items2[16].ToString();
                label30.Text = items2[17].ToString();

            }
        }

        private void multiExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Multi_Extract multi_Extract = new Multi_Extract(Fileinfo);
            //multi_Extract.ShowDialog(this);
        }

        private void OpenQRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = AppPath;
            openFileDialog1.Filter = "QRC files (*.qrc; *qrcf )|*.qrc; *.qrcf |All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            //openFileDialog1.DefaultExt = "QRC files (*.qrc)|*.qrc";
            openFileDialog1.RestoreDirectory = true;

            label31.Text = "Loading...";
            label31.Visible = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                toolStrip1.Enabled = false;
                tableLayoutPanel1.Visible = false;
                listBox1.Visible = false;
                Auto_Load(openFileDialog1.FileName);
                this.Text = openFileDialog1.FileName;
                UpdateTextPosition();
                toolStrip1.Enabled = true;
                label31.Visible = false;
                listBox1.Visible = true;
                tableLayoutPanel1.Visible = true;
                /* Disable();
                 QRC_File_Name = openFileDialog1.FileName;

                 FileStream q = File.Open(QRC_File_Name, FileMode.Open);

                 byte[] Filebytes1 = new byte[q.Length];
                 q.Read(Filebytes1, 0, Filebytes1.Length);

                 byte[] testmagic = new byte[4];
                 Array.Copy(Filebytes1, 0, testmagic, 0, 4);
                 byte[] qrcmagic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
                 byte[] qrcfmagic = new byte[] { 0x51, 0x52, 0x43, 0x46 };


                 if (Check_Magic(testmagic, qrcmagic) == true)
                 {
                     MemoryStream m = new MemoryStream();
                     for (long i = 8; i < q.Length; i++)
                         m.WriteByte(Filebytes1[i]);
                     Filebytes = DecompressZlib(m.ToArray());
                     Open_File();
                 }
                 else if (Check_Magic(testmagic, qrcfmagic) == true)
                 {
                     Filebytes = new byte[Filebytes1.Length];
                     Array.Copy(Filebytes1, 0, Filebytes, 0, Filebytes1.Length);
                     Open_File();
                 }
                 else
                 {
                     MessageBox.Show("Error", "Not a Valid QRC File");
                 }
                 q.Close();*/
            }
            else
            {
                label31.Text = "";
                label31.Visible = true;
            }
            //label31.Text = "";
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Options OptionsChild = new Options();

            //OptionsChild.ShowDialog(this);
            Form2 OptionsChild = new Form2();

            OptionsChild.Show();

        }

        private void SaveQRCFToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "QRCF files (*.qrcf)|*.qrcf|All files (*.*)|*.*";
            saveFileDialog1.DefaultExt = "QRCF files (*.qrcf)|*.qrcf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            label31.Text = "Saving QRCF...";
            label31.Visible = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dwtype = "qrcf";
                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Visible = false;
                dw();
                /*
                string QRCF_File = saveFileDialog1.FileName;
                FileStream q = File.Open(QRCF_File, FileMode.Create);
                q.Write(Filebytes, 0, Filebytes.Length);
                q.Close();
                */
            }
            else
            {
                label31.Text = "";
                label31.Visible = false;
            }
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        #endregion<<>>

        public void ExtractAll()
        {
            string xname = "";
            string folder = folderBrowserDialog1.SelectedPath;
            foreach (DataRow item1 in Fileinfo.Rows)
            {
                Object[] items2 = item1.ItemArray;
                string name = items2[0].ToString();


                string ext = name.Remove(0, name.LastIndexOf('.'));
                foreach (string[] EXT1 in Options.EXTS)
                {
                    string EXT2 = EXT1[0];
                    if (ext == EXT2 && EXT1[0] != "")
                    {
                        name.Replace(ext, ext + EXT1[0]);
                    }


                    int dr1 = name.LastIndexOf('/');
                    string dr = name.Remove(dr1, name.Length - dr1);
                    if (dr.Length != name.Length)
                    {
                        //Directory.CreateDirectory(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + dr);
                        Directory.CreateDirectory(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\" + "\\" + dr);
                    }
                }


                //Extract_File(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + name, items2);
                Extract_File(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\" + name, items2);

            }

            WriteXML(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\", xname);

        }

        public void Save_QRCF()
        {
            string QRCF_File = saveFileDialog1.FileName;
            FileStream q = File.Open(QRCF_File, FileMode.Create);
            q.Write(Filebytes, 0, Filebytes.Length);
            q.Close();
        }

        public void Save_QRC()
        {
            string QRCF_File = saveFileDialog1.FileName;
            FileStream q = File.Open(QRCF_File, FileMode.Create);
            //byte[] temp = CompressZlib(Filebytes);
            byte[] New_Magic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
            byte[] FSize = new byte[4];
            FSize = BitConverter.GetBytes(Filebytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(FSize);
            }
            q.Write(New_Magic, 0, New_Magic.Length);
            q.Write(FSize, 0, FSize.Length);
           // q.Write(temp, 0, temp.Length);
            q.Close();
        }

        public void BNew(string file )
        {

            //string[] files = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.xml");

            if (file != null)
            {
                string QRC_Folder = Path.GetDirectoryName(file);//folderBrowserDialog1.SelectedPath;
                int dr1 = QRC_Folder.LastIndexOf('\\');
                string dr = QRC_Folder.Remove(dr1, QRC_Folder.Length - dr1);
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.Filter = "QRC files (*.qrc)|*.qrc|All files (*.*)|*.*";
                saveFileDialog1.DefaultExt = "QRC files (*.qrc)|*.qrc";
                saveFileDialog1.FilterIndex = 1;
                saveFileDialog1.InitialDirectory = dr;
                saveFileDialog1.RestoreDirectory = true;
                //saveFileDialog1.ShowDialog();
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    New_qrcf = saveFileDialog1.FileName;
                    New_folder = QRC_Folder;
                    New_xml = file;
                    dw2();
                    //BuildNewQRC(files[0], QRCF_Folder, New_QRCF_File);


                }
                else
                {
                    label31.Text = "";
                    label31.Visible = false;
                }

            }
        }

        private void dw()
        {


            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += worker_DoWork;

            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(10000);


        }

        private void dw2()
        {


            BackgroundWorker worker2 = new BackgroundWorker();
            worker2.WorkerReportsProgress = true;

            worker2.DoWork += worker_DoWork2;

            worker2.RunWorkerCompleted += worker_RunWorkerCompleted2;
            worker2.RunWorkerAsync(10000);


        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (dwtype != "")
            {

                if (dwtype == "e_all")
                {
                    ExtractAll();

                }
                else if (dwtype == "qrcf")
                {
                    Save_QRCF();
                }
                else if (dwtype == "qrc")
                {
                    Save_QRC();
                }
                else if (dwtype == "new")
                {
                    //BNew();
                }
            }

        }

        void worker_DoWork2(object sender, DoWorkEventArgs e)
        {
            BuildNewQRC(New_xml, New_folder, New_qrcf);

        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dwtype = "";
            label31.Text = "";
            label31.Visible = false;
            toolStrip1.Enabled = true;
            label31.Visible = false;
            listBox1.Visible = true;
            tableLayoutPanel1.Enabled = true;


        }

        void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            dwtype = "";
            label31.Text = "";
            label31.Visible = false;
            toolStrip1.Enabled = true;
            label31.Visible = false;
            listBox1.Visible = true;
            tableLayoutPanel1.Enabled = true;
            if (File.Exists(New_qrcf))
            {
                Auto_Load(New_qrcf);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void UpdateTextPosition()
        {
            this.Text = this.Text.Remove(0, this.Text.LastIndexOf("\\") + 1);
            Graphics g = this.CreateGraphics();
            Double startingPoint = ((this.Width / 2)-100) - (g.MeasureString(this.Text.Trim(), this.Font).Width / 2);
           
            Double widthOfASpace = g.MeasureString(" ", this.Font).Width;
            String tmp = "QRC Editor";
            Double tmpWidth = 0;

            while ((tmpWidth + widthOfASpace) < startingPoint)
            {
                tmp += " ";
                tmpWidth += widthOfASpace;
            }
            if(tmp.Length < 20)
            {
                tmp = tmp.Remove(10, 10);
            }
            
            this.Text = tmp + this.Text.Trim();

        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (e.Button == MouseButtons.Right)
            {
                //select the item under the mouse pointer
                listBox1.SelectedIndex = listBox1.IndexFromPoint(e.Location);
                if (listBox1.SelectedIndex != -1)
                {
                    //listboxContextMenu.Show();
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem is DataRowView item1)
            {
                int dr0 = AppPath.LastIndexOf('\\');
                string dr = AppPath.Remove(dr0, AppPath.Length - dr0) + "\\tmp\\";
                int dr1 = item1.Row.ItemArray[0].ToString().LastIndexOf('/');
                dr += item1.Row.ItemArray[0].ToString().Remove(0, dr1 + 1);
                ;
                if (!Directory.Exists("tmp"))
                {
                    Directory.CreateDirectory("tmp");
                }
               
                    Object[] items2 = item1.Row.ItemArray;
                    Extract_File(dr, items2);
                System.Diagnostics.Process.Start(dr);
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists("tmp"))
            {
                Directory.Delete("tmp", true);
            }
        }

        private void compressQRCToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {

        }

       
    }


    /*public class QRC
    {
        public byte[] File_table_offset = new byte[4];
        public byte[] File_table_size = new byte[4];
        public byte[] Float_array_table_offset = new byte[4];
        public byte[] Float_array_table_size = new byte[4];
        public byte[] Id_table_offset = new byte[4];
        public byte[] Id_table_size = new byte[4];
        public byte[] Int_array_table_offset = new byte[4];
        public byte[] Int_array_table_size = new byte[4];
        public byte[] Magic = new byte[4];
        public byte[] String_table_offset = new byte[4];
        public byte[] String_table_size = new byte[4];
        public byte[] Tree_offset = new byte[4];
        public byte[] Tree_size = new byte[4];
        public byte[] Version = new byte[4];

        public QRC(byte[] Header)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(Header, 0, Header.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            memStream.Read(Magic, 0x0, 0x4);

            memStream.Read(Version, 0x0, 0x4);

            memStream.Read(Tree_offset, 0x0, 0x4);

            memStream.Read(Tree_size, 0x0, 0x4);

            memStream.Read(Id_table_offset, 0x0, 0x4);

            memStream.Read(Id_table_size, 0x0, 0x4);

            memStream.Read(String_table_offset, 0x0, 0x4);

            memStream.Read(String_table_size, 0x0, 0x4);
            memStream.Read(Int_array_table_offset, 0x0, 0x4);

            memStream.Read(Int_array_table_size, 0x0, 0x4);

            memStream.Read(Float_array_table_offset, 0x0, 0x4);

            memStream.Read(Float_array_table_size, 0x0, 0x4);

            memStream.Read(File_table_offset, 0x0, 0x4);

            memStream.Read(File_table_size, 0x0, 0x4);

        }

    }

    public class QRC_File
    {

        public byte[] attribute1_name_offset = new byte[4];
        public byte[] attribute1_type = new byte[4];
        public byte[] attribute1_variable_1 = new byte[4];
        public byte[] attribute1_variable_2 = new byte[4];
        public byte[] attribute2_name_offset = new byte[4];
        public byte[] attribute2_type = new byte[4];
        public byte[] attribute2_variable_1 = new byte[4];
        public byte[] attribute2_variable_2 = new byte[4];
        public byte[] attribute3_name_offset = new byte[4];
        public byte[] attribute3_type = new byte[4];
        public byte[] attribute3_variable_1 = new byte[4];
        public byte[] attribute3_variable_2 = new byte[4];
        public byte[] attributes = new byte[4];
        public byte[] first_child_offset = new byte[4];
        public byte[] last_child_offset = new byte[4];
        public byte[] name_offset = new byte[4];
        public byte[] next_brother_offset = new byte[4];
        public byte[] parent_offset = new byte[4];
        public byte[] previous_brother_offset = new byte[4];
    }

    public class Toc
    {
        public List<QRC_File> files = new List<QRC_File>();

        public Toc(byte[] Header)
        {


            int fc = Header.Length;

            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(Header, 0, Header.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            while (memStream.Position < fc)
            {


                while (memStream.Position < Header.Length)
                {
                    QRC_File n = new QRC_File();

                    memStream.Read(n.name_offset, 0x0, 0x4);
                    memStream.Read(n.attributes, 0x0, 0x4);
                    memStream.Read(n.parent_offset, 0x0, 0x4);
                    memStream.Read(n.previous_brother_offset, 0x0, 0x4);
                    memStream.Read(n.next_brother_offset, 0x0, 0x4);
                    memStream.Read(n.first_child_offset, 0x0, 0x4);
                    memStream.Read(n.last_child_offset, 0x0, 0x4);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(n.attributes);

                    }

                    int x = BitConverter.ToInt32(n.attributes, 0);

                    int i = 0;
                    while (i < x)
                    {
                        if (i == 0)
                        {
                            memStream.Read(n.attribute1_name_offset, 0x0, 0x4);
                            memStream.Read(n.attribute1_type, 0x0, 0x4);
                            memStream.Read(n.attribute1_variable_1, 0x0, 0x4);
                            memStream.Read(n.attribute1_variable_2, 0x0, 0x4);
                        }
                        if (i == 0)
                        {
                            memStream.Read(n.attribute2_name_offset, 0x0, 0x4);
                            memStream.Read(n.attribute2_type, 0x0, 0x4);
                            memStream.Read(n.attribute2_variable_1, 0x0, 0x4);
                            memStream.Read(n.attribute2_variable_2, 0x0, 0x4);
                        }
                        if (i == 0 && x == 3)
                        {
                            memStream.Read(n.attribute3_name_offset, 0x0, 0x4);
                            memStream.Read(n.attribute3_type, 0x0, 0x4);
                            memStream.Read(n.attribute3_variable_1, 0x0, 0x4);
                            memStream.Read(n.attribute3_variable_2, 0x0, 0x4);
                        }
                        i++;
                    }
                    files.Add(n);
                }


            }



        }
    }

    public class Attributes
    {
        public byte[] attribute_type = new byte[4];
        public byte[] attribute_variable_1 = new byte[4];
        public byte[] attribute_variable_2 = new byte[4];

    }
   */
}