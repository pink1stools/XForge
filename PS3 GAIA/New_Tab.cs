using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace QRC_Editor
{ 
    public partial class New_Tab : UserControl
    {
        byte[] Filebytes;
        byte[] New_Filebytes;
        string QRC_File_Name = "";
        byte[] FTO;
        string AppPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public DataTable Fileinfo = new DataTable();
        byte[] FTS;
        List<string> sn = new List<string>();

        public New_Tab()
        {
            
            InitializeComponent();
            StartUp();
            
           
        }


        #region<<buttons>>

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
        
        public void SaveQRCFToolStripMenuItem1_Click()
        {
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.Filter = "QRCF files (*.qrcf)|*.qrcf|All files (*.*)|*.*";
            saveFileDialog1.DefaultExt = "QRCF files (*.qrcf)|*.qrcf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string QRCF_File = saveFileDialog1.FileName;
                FileStream q = File.Open(QRCF_File, FileMode.Create);
                q.Write(Filebytes, 0, Filebytes.Length);
                q.Close();
            }
        }

        public void CompressQRCToolStripMenuItem_Click()
        {
            saveFileDialog2.AddExtension = true;
            
            saveFileDialog2.Filter = "QRC files (*.qrc)|*.qrc|All files (*.*)|*.*";
            saveFileDialog2.DefaultExt = "QRC files (*.qrc)|*.qrc";
            saveFileDialog2.FilterIndex = 1;
            saveFileDialog2.RestoreDirectory = true;
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string QRCF_File = saveFileDialog2.FileName;
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
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Disable();
        }

        public void BuildNewQRCToolStripMenuItem_Click()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK && File.Exists(folderBrowserDialog1.SelectedPath + "\\*xml"))
            {
               // Disable();
                string QRCF_Folder = folderBrowserDialog1.SelectedPath;
                
            }
        }

        public void ExtractAllToolStripMenuItem_Click()
        {
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
                        Directory.CreateDirectory(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + dr);
                    }
                }


                Extract_File(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + name, items2);
            }

            WriteXML();

            /*byte[] OR = w.attribute1_variable_1;
                    byte[] ORsize = w.attribute1_variable_2;

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(OR);
                        Array.Reverse(ORsize);
                        //Array.Reverse(FTO);
                    }
                    int fto = BitConverter.ToInt32(FTO, 0);
                    int or = BitConverter.ToInt32(OR, 0);
                    int orsize = BitConverter.ToInt32(ORsize, 0);
                    int nor = or + fto;
                    MemoryStream memStream2 = new MemoryStream();
                    BinaryFormatter binForm2 = new BinaryFormatter();
                    memStream2.Write(Filebytes, 0, Filebytes.Length);
                    memStream2.Seek(nor, SeekOrigin.Begin);

                    Console.WriteLine("#" + o + " " + utfString);
                    sn.Add(utfString);
                    int dr1 = utfString.LastIndexOf('/');
                    string dr = utfString.Remove(dr1, utfString.Length - dr1);
                    if (dr.Length != utfString.Length)
                    {
                        Directory.CreateDirectory("extracted/" + dr);
                    }
                    //File.Create(utfString);
                    byte[] F1 = new byte[orsize];
                    memStream2.Read(F1, 0, orsize);
                    File.WriteAllBytes("extracted/" + utfString, F1);

                */
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            Options OptionsChild = new Options();

            OptionsChild.ShowDialog(this);
        }

        public void Button1_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem is DataRowView item1)
            {
                int dr1 = item1.Row.ItemArray[0].ToString().LastIndexOf('/');
                string dr = item1.Row.ItemArray[0].ToString().Remove(0, dr1 + 1);
                saveFileDialog3.FileName = dr;

                if (saveFileDialog3.ShowDialog() == DialogResult.OK)
                {
                    Object[] items2 = item1.Row.ItemArray;
                    Extract_File(saveFileDialog3.FileName, items2);
                }
            }
        }

        public void Button2_Click(object sender, EventArgs e)
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

        private void ImagesButton_Click(object sender, EventArgs e)
        {
            //Images OptionsChild = new Images();
            Base one = new Base();

            //newMDIChild.MdiParent = this;
            one.Show();


            //OptionsChild.ShowDialog(this);
        }

        #endregion<<>>

        #region<<Zlib>>

        public static byte[] DecompressZlib(byte[] input)
        {
            MemoryStream source = new MemoryStream(input);
            byte[] result = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                using (InflaterInputStream inf = new InflaterInputStream(source))
                {
                    inf.CopyTo(outStream);
                }
                result = outStream.ToArray();
            }
            return result;
        }

        public static byte[] CompressZlib(byte[] input)
        {
            MemoryStream m = new MemoryStream();
            DeflaterOutputStream zipStream = new DeflaterOutputStream(m, new ICSharpCode.SharpZipLib.Zip.Compression.Deflater(9));
            zipStream.Write(input, 0, input.Length);
            zipStream.Finish();
            return m.ToArray();
        }

        #endregion<<>>

        public void Auto_Load(string fname)
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
                q.Close();
            }

        }

        void Disable()
        {
            button1.Enabled = false;
            button2.Enabled = false;
           // extractAllToolStripMenuItem.Enabled = false;
           // closeToolStripMenuItem.Enabled = false;
           // compressQRCToolStripMenuItem.Enabled = false;
           // saveQRCFToolStripMenuItem1.Enabled = false;
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
           // extractAllToolStripMenuItem.Enabled = true;
           // closeToolStripMenuItem.Enabled = true;
           // compressQRCToolStripMenuItem.Enabled = true;
           // saveQRCFToolStripMenuItem1.Enabled = true;
        }

        void StartUp()
        {
            
           // toolTip1.SetToolTip(button1, "Extract Selected File");
           // toolTip1.SetToolTip(button2, "Replace Selected File");
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

        bool Check_Magic(byte[] testmagic, byte[] magic)
        {
            bool isqrcf = testmagic.SequenceEqual(magic);
            return isqrcf;
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

        void Extract_File(string QRCF_File, Object[] item)
        {


            byte[] OR = item[12] as byte[];
            byte[] ORsize = item[13] as byte[];

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(OR);
                Array.Reverse(ORsize);
                //Array.Reverse(FTO);
            }


            int fto = BitConverter.ToInt32(FTO, 0);
            int or = BitConverter.ToInt32(OR, 0);
            int orsize = BitConverter.ToInt32(ORsize, 0);
            int nor = or + fto;
            MemoryStream memStream2 = new MemoryStream();
            BinaryFormatter binForm2 = new BinaryFormatter();
            memStream2.Write(Filebytes, 0, Filebytes.Length);
            memStream2.Seek(nor, SeekOrigin.Begin);


            byte[] F1 = new byte[orsize];
            memStream2.Read(F1, 0, orsize);
            File.WriteAllBytes(QRCF_File, F1);



            //FileStream q = File.Open(QRCF_File, FileMode.Create);
            //File.WriteAllBytes("extracted/" + utfString, F1);
            //q.Close();
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

        public void WriteXML()
        {
            using (System.IO.StreamWriter nfile =

            new System.IO.StreamWriter(Path.GetFileNameWithoutExtension(QRC_File_Name + "/QRC.xml"), false))
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

        public void Multi_Extract()
        {
            foreach(DataRowView item1 in this.listBox1.Items)
            {

            }
            //this.listBox1.SelectedItem is 
            Multi_Extract multi_Extract = new Multi_Extract(Fileinfo);
            multi_Extract.ShowDialog(this.ParentForm.ParentForm);
        }

    }
}
