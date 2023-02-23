using CG.Web.MegaApiClient;
using DarkUI.Forms;
using DevIL;
using FastColoredTextBoxNS;
using Joveler.ZLibWrapper;
using Microsoft.Win32;
using QRC_Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace XForge
{

    public partial class Form2 : DarkForm
    {
        INode Newnode;
        string version = "";
        Size oldtw;
        Size oldfw;
        //private SyntaxHighlighter syntaxHighlighter;
        private MRUManager mruManager;
        public ImageImporter m_importer;
        public ImageExporter m_exporter;
        Boolean isR = false;
        Boolean isL = false;
        public DevIL.Image m_activeImage;
        public bool Show_Splash;
        public bool FirstRun;
        Size pbsz;
        int loop = 0;
        public DataTable Fileinfo = new DataTable();
        string AppPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        public byte[] Filebytes;
        public static byte[] FTO;
        byte[] FTS;
        byte[] New_Filebytes;
        public string QRC_File_Name = "";
        string dwtype = "";
        public string md5 = "";
        string New_xml = "";
        string New_folder = "";
        string New_qrcf = "";
        byte[] loadedBtes;
        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        List<string> sn = new List<string>();
        byte[] Loaded_file;
        string Loaded_ext = "";
        string ltype = "QRC";
        bool QRCcompression = false;
        int srow = 0;
        bool changed = false;

        public Form2(string fn)
        {


            try
            {
                if (!Directory.Exists("ext"))
                {
                    Directory.CreateDirectory("ext");
                }
                if (!Directory.Exists("ext/x86"))
                {
                    Directory.CreateDirectory("ext/x86");
                }

                if (!Directory.Exists("ext/x64"))
                {
                    Directory.CreateDirectory("ext/x64");
                }
                if (!Directory.Exists("ext/temp"))
                {
                    Directory.CreateDirectory("ext/temp");
                }

                File.WriteAllBytes("ext/x86/zlibwapi.dll", Properties.Resources.zlibwapi1);

                File.WriteAllBytes("ext/x64/zlibwapi.dll", Properties.Resources.zlibwapi);
                File.WriteAllBytes("ext/ILU.dll", Properties.Resources.ILU);

                File.WriteAllBytes("ext/DevIL.dll", Properties.Resources.DevIL);
            }
            catch
            {

            }

            Options.LoadSettings();

            Show_Splash = Options.showsplash;
            FirstRun = Options.firstrun;
            if (IntPtr.Size == 8) // This app is running on 64bit .Net Framework

                //ZLibNative.AssemblyInit(x64);
                ZLibNative.AssemblyInit(Path.Combine("ext", "x64", "zlibwapi.dll"));
            else // This app is running on 32bit .Net Framework
                ZLibNative.AssemblyInit(Path.Combine("ext", "x86", "zlibwapi.dll"));


            InitializeComponent();
            InitDevIL();
            StartUp();
            Options.LoadSettings();
            this.DragEnter += new DragEventHandler(Form1_DragEnter);

            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";

            Loaded_ext = "";
            //this.label4.Visible = false;
            //this.Text = "";
            //UpdateTextPosition();
            label31.Text = "";
            label31.Visible = true;
            label31.Dock = DockStyle.Fill;
            Disable();
            Refresh();
            Auto_Load(fn);
            listBox1.Update();
            this.listView1.Focus();
            //this.Text = openFileDialog1.FileName;
            //UpdateTextPosition();
            toolStripStatusLabel1.Text = fn;
            toolStripStatusLabel1.Text = toolStripStatusLabel1.Text.Remove(0, toolStripStatusLabel1.Text.LastIndexOf("\\") + 1);
            toolStrip1.Enabled = true;
            label31.Visible = false;
            //listBox1.Visible = true;
            tableLayoutPanel1.Visible = true;
            this.listView1.Items[0].Focused = true;
            this.listView1.Items[0].Selected = true;
            listBox1.SelectedIndex = 1;
            listBox1.SelectedIndex = 0;
            this.listView1.Items[0].Focused = true;
            this.listView1.Items[0].Selected = true;
            this.listView1.Focus();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 500;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 20;
            /**/
            // Application.EnableVisualStyles();
            progressBar1.Visible = false;
            if (Show_Splash || FirstRun)
            {
                if (FirstRun)
                {
                    Options.firstrun = false;
                    Options.SaveSettings();
                }
                Images splashScreen = new Images();
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                splashScreen.ShowDialog();
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
            }
            //var syntaxHighlighter = new WinFormsSyntaxHighlighter.SyntaxHighlighter(richTextBox11);
        }

        public Form2()
        {

            try
            {
                if (!Directory.Exists("ext"))
                {
                    Directory.CreateDirectory("ext");
                }
                if (!Directory.Exists("ext/x86"))
                {
                    Directory.CreateDirectory("ext/x86");
                }

                if (!Directory.Exists("ext/x64"))
                {
                    Directory.CreateDirectory("ext/x64");
                }
                if (!Directory.Exists("ext/temp"))
                {
                    Directory.CreateDirectory("ext/temp");
                }
                if (!File.Exists("ext/x86/zlibwapi.dll"))
                {
                    File.WriteAllBytes("ext/x86/zlibwapi.dll", Properties.Resources.zlibwapi1);
                }
                if (!File.Exists("ext/x64/zlibwapi.dll"))
                {
                    File.WriteAllBytes("ext/x64/zlibwapi.dll", Properties.Resources.zlibwapi);
                }
                if (!File.Exists("ext/ILU.dll"))
                {
                    File.WriteAllBytes("ext/ILU.dll", Properties.Resources.ILU);
                }
                if (!File.Exists("ext/DevIL.dll"))
                {
                    File.WriteAllBytes("ext/DevIL.dll", Properties.Resources.DevIL);
                }
            }
            catch
            {

            }

            Options.LoadSettings();

            Show_Splash = Options.showsplash;
            FirstRun = Options.firstrun;
            if (IntPtr.Size == 8) // This app is running on 64bit .Net Framework

                //ZLibNative.AssemblyInit(x64);
                ZLibNative.AssemblyInit(Path.Combine("ext", "x64", "zlibwapi.dll"));
            else // This app is running on 32bit .Net Framework
                ZLibNative.AssemblyInit(Path.Combine("ext", "x86", "zlibwapi.dll"));



            InitializeComponent();
            InitDevIL();
            StartUp();
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            Refresh();
            Options.LoadSettings();
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 500;
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 20;
            /**/
            // Application.EnableVisualStyles();
            progressBar1.Visible = false;
            if (Show_Splash || FirstRun)
            {
                if (FirstRun)
                {
                    Options.firstrun = false;
                    Options.SaveSettings();
                }
                Images splashScreen = new Images();
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                splashScreen.ShowDialog();
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
            }
        }

        private void InitDevIL()
        {


            m_importer = new ImageImporter();
            m_exporter = new ImageExporter();
            new ImageState
            {
                AbsoluteFormat = DataFormat.BGRA,
                AbsoluteDataType = DataType.UnsignedByte,
                AbsoluteOrigin = OriginLocation.UpperLeft
            }.Apply();
            new CompressionState
            {
                KeepDxtcData = true
            }.Apply();
            new SaveState
            {
                OverwriteExistingFile = true
            }.Apply();
        }

        private bool convertDDS(MemoryStream input, string output, ImageType T)
        {


            bool result;
            try
            {
                this.m_activeImage = this.m_importer.LoadImageFromStream(T, input);
                this.m_exporter.SaveImage(this.m_activeImage, output);
                result = true;
            }
            catch
            {
                //MessageBox.Show("Failed to convert \"" + input + "\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                result = false;
            }
            return result;
        }/**/

        private bool convertDDS(FileStream input, string output, ImageType T)
        {


            bool result;
            try
            {
                this.m_activeImage = this.m_importer.LoadImageFromStream(T, input);
                this.m_exporter.SaveImage(this.m_activeImage, output);
                result = true;
            }
            catch
            {
                //MessageBox.Show("Failed to convert \"" + input + "\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                result = false;
            }
            return result;
        }/**/

        #region<<Zlib>>

        public static byte[] CompressZlib(byte[] input)
        {
            byte[] decompBytes = ZLibCompressor.Compress(input, CompressionLevel.Level9);
            return decompBytes;
        }

        public static byte[] DecompressZlib(byte[] input)
        {
            byte[] decompBytes = ZLibCompressor.Decompress(input);
            return decompBytes;
        }

        public static byte[] CompressGZip(byte[] input)
        {
            byte[] decompBytes = GZipCompressor.Compress(input, CompressionLevel.Level9);
            return decompBytes;
        }

        public static byte[] DecompressGZip(byte[] input)
        {
            byte[] decompBytes = GZipCompressor.Decompress(input);
            return decompBytes;
        }

        #endregion<<>>

        #region<<buttons>>

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AboutBox1 aboutBox = new AboutBox1();

            //aboutBox.ShowDialog(this);
        }

        private void BuildNewQRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog2.Filter = "XMLC files (*.xml)|*.xml|All files (*.*)|*.*";

            label31.Text = "Working...";
            label31.Visible = true;

            label31.Visible = true;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string file = openFileDialog2.FileName;
                dwtype = "new";
                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Visible = false;
                tableLayoutPanel1.Visible = false;
                BNew(file);

            }
            else
            {
                this.label31.Image = null;
                label31.Text = "";
                label31.Visible = false;
            }


        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {


                //hexaEditor1.FileName = "";
                int si = listView1.SelectedIndices[0];
                if (Fileinfo.Rows[si] is DataRow item1)
                //if (this.listBox1.SelectedItem is DataRowView item1)
                {
                    int dr1 = item1.ItemArray[0].ToString().LastIndexOf('/');
                    string dr = item1.ItemArray[0].ToString().Remove(0, dr1 + 1);
                    string g = Path.GetExtension(dr);

                    if (g == "")
                    {
                        g = ".dds";
                    }
                    saveFileDialog1.Filter = g.ToUpper() + " files (*" + g + ")|*" + g + "|All files (*.*)|*.*";
                    saveFileDialog1.DefaultExt = g.ToUpper() + " files (*" + g + ")|*" + g + "";
                    //saveFileDialog1.DefaultExt = g;
                    saveFileDialog1.FileName = dr;

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        Object[] items2 = item1.ItemArray;
                        Extract_File(saveFileDialog1.FileName, items2);
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {


                //hexaEditor1.FileName = "";
                int si = listView1.SelectedIndices[0];
                if (Fileinfo.Rows[si] is DataRow item1)
                //if (this.listBox1.SelectedItem is DataRowView item1)
                {
                    int dr1 = item1.ItemArray[0].ToString().LastIndexOf('/');
                    string dr = item1.ItemArray[0].ToString().Remove(0, dr1 + 1);
                    openFileDialog1.Filter = "All files (*.*)|*.*";
                    openFileDialog1.FileName = dr;
                    openFileDialog1.FilterIndex = 2;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //waitForm.Show(this);
                        Object[] items2 = item1.ItemArray;
                        Replace_File(openFileDialog1.FileName, items2);
                        //waitForm.Close();
                    }
                }
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (changed)
            {
                if (MessageBox.Show("Close QRC without saving changes?", "Unsaved changes!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    changed = false;
                }
                /*var window = MessageBox.Show(
              "Close QRC without saving changes?",
              "Unsaved changes",
               MessageBoxButtons.YesNo);

                if (window == DialogResult.Yes)
                //(DialogResult.ToString() == "Yes")
                {
                    changed = false;
                }*/
            }

            if (!changed)
            {
                ltype = "";
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel2.Text = "";
                Loaded_ext = "";
                //this.label4.Visible = false;
                label31.Text = "";
                label31.Visible = true;
                label31.Dock = DockStyle.Fill;
                Disable();
            }
        }

        private void CompressQRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "QRC files (*.qrc)|*.qrc|All files (*.*)|*.*";
            saveFileDialog1.DefaultExt = "QRC files (*.qrc)|*.qrc";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = Path.GetFileName(QRC_File_Name);
            //label31.Text = "Saving QRC...";
            //label31.Visible = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //waitForm.Show(this);
                dwtype = "qrc";
                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Visible = false;
                tableLayoutPanel1.Visible = false;
                darkLabel1.Text = "Saving QRC";
                start_progress();
                dw();
                //stop_progress();
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
            folderBrowserDialog1.SelectedPath = dr;

            label31.Text = "Extracting All Files...";
            label31.Visible = true;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {

                dwtype = "e_all";

                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Enabled = false;
                darkLabel1.Text = "Extracting QRC";
                start_progress();
                dw();
                //stop_progress();
            }
            else
            {
                label31.Text = "";
                label31.Visible = false;
            }
        }

        private void ImagesButton_Click(object sender, EventArgs e)
        {
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            loop++;
            if (listView1.SelectedIndices.Count > 0)
            {
                loop = 1;
                if (loop == 1)
                {

                    int si = listView1.SelectedIndices[0];
                    ListViewItem itm = listView1.Items[si] as ListViewItem;

                    string itmim = itm.ImageKey;
                    //itm.ImageKey = "dds.png";
                    //hexaEditor1.FileName = "";

                    if (Fileinfo.Rows[si] is DataRow item1)
                    //if (this.listBox1.SelectedItem is DataRowView item1)
                    {
                        richTextBox11.Dispose();
                        richTextBox1.Dispose();
                        richTextBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
                        //this.richTextBox11 = new System.Windows.Forms.RichTextBox();
                        
                        label6.Text = "";
                        string h1 = sn[listBox1.SelectedIndex].ToString();


                        Loaded_ext = "";
                        this.richTextBox11.Visible = false;
                        this.elementHost1.Visible = false;
                        this.pictureBox1.Visible = false;
                        Object[] items2 = item1.ItemArray;
                        byte[] Size = items2[13] as byte[];
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                        Array.Reverse(items2[12] as byte[]);
                        Array.Reverse(Size);
                        string sz = SizeSuffix(BitConverter.ToInt32(Size, 0), 0);
                        string tpe = "";
                        int a = items2[0].ToString().LastIndexOf('.');
                        if (a > 1)
                        {
                            tpe = items2[0].ToString().Remove(0, items2[0].ToString().LastIndexOf('.') + 1);
                            Loaded_ext = "." + tpe;
                            //label2.Text = "Type  " + items2[0].ToString().Remove(0, items2[0].ToString().LastIndexOf('.') + 1);
                        }
                        else
                        {
                            //label2.Text = "Type  " + items2[23].ToString();
                        }
                        SetRTB(tpe);
                        //label1.Text = "Size  " + sz;
                        Array.Reverse(Size);
                        //label3.Text = "Offset  " + BitConverter.ToInt32(items2[12] as byte[], 0);
                        //label4.Text = "More file Info";
                        Array.Reverse(items2[12] as byte[]);



                        if (tpe == "jpg" || tpe == "bmp" || tpe == "dds" || tpe == "tga")
                        {
                            byte[] OR = item1[12] as byte[];
                            byte[] ORsize = item1[13] as byte[];

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
                            Loaded_file = new byte[F1.Length];
                            Loaded_file = F1;

#pragma warning restore UnhandledExceptions // Unhandled exception(s)
                            if (tpe == "jpg" || tpe == "bmp")
                            {
                                Bitmap image;
                                using (MemoryStream stream = new MemoryStream(F1))
                                {
                                    image = new Bitmap(stream);
                                }
                                if (tpe == "jpg")
                                {
                                    string szs = image.Size.ToString().Trim('{', '}');
                                    //string szs = sz2.ToString().Trim('{', '}');
                                    szs = szs.Replace("Width=", "");
                                    szs = szs.Replace("Height=", "");
                                    szs = szs.Replace(", ", "x") + "px";
                                    szs = szs.Replace(" ", "");
                                    label6.Text = szs;
                                    toolTip1.SetToolTip(this.pictureBox1, szs);
                                    //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    pictureBox1.Image = image;
                                }
                                this.pictureBox1.Visible = true;
                                //this.label4.Visible = true;
                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                if (tpe == "bmp")
                                {

                                    Bitmap bmp = image;

                                    Size sz2 = bmp.Size;
                                    Size max = new System.Drawing.Size(1024, 1024);
                                    string szs = sz2.ToString().Trim('{', '}');
                                    szs = szs.Replace("Width=", "");
                                    szs = szs.Replace("Height=", "");
                                    szs = szs.Replace(", ", "x") + "px";
                                    szs = szs.Replace(" ", "");
                                    label6.Text = szs;
                                    toolTip1.SetToolTip(this.pictureBox1, szs);
                                    if (max.Height > sz2.Height && max.Width > sz2.Width)
                                    {
                                        float zoom = (float)(500 / 4f + 1);
                                        int w = (int)sz2.Width * (int)zoom;
                                        int h = (int)sz2.Height * (int)zoom;
                                        Bitmap zoomed = ResizeImage(bmp, w, h);

                                        //if (zoomed != null) zoomed.Dispose();


                                        //zoomed = new Bitmap((int)(1600), (int)(1200));



                                        /*   using (Graphics g = Graphics.FromImage(zoomed))
                                          {
                                              g.InterpolationMode = InterpolationMode.NearestNeighbor;
                                              g.DrawImage(bmp, new Rectangle(Point.Empty, zoomed.Size));
                                          }*/
                                        pictureBox1.Image = zoomed;
                                        //File.WriteAllBytes("temp.bmp", imageToByteArray(zoomed));
                                    }
                                    else
                                    {
                                        pictureBox1.Image = image;
                                    }

                                    //pictureBox1.Image = Newimage;
                                    this.pictureBox1.Visible = true;
                                    //this.label4.Visible = true;

                                    //pictureBox1.Dock = DockStyle.Fill;
                                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                }
                            }

                            if (tpe == "dds" || tpe == "tga")
                            {
                                using (MemoryStream stream = new MemoryStream(F1))
                                {
                                    try
                                    {


                                        if (tpe == "tga")
                                        {
                                            convertDDS(stream, "ext/temp/temp.png", ImageType.Tga);

                                        }
                                        else if (tpe == "dds")
                                        {
                                            convertDDS(stream, "ext/temp/temp.png", ImageType.Dds);

                                        }
                                        if (File.Exists("ext/temp/temp.png"))
                                        {

                                            Bitmap image;
                                            using (FileStream stream2 = new FileStream("ext/temp/temp.png", FileMode.Open))
                                            {
                                                if (items2[0].ToString() == "/datacom/border_donuts-nnha.dds" || items2[0].ToString() == "/datacom/border_sphere-nnha.dds" || items2[0].ToString() == "/datacom/arrow_A-nnha.dds" || items2[0].ToString() == "/datacom/arrow_B-nnha.dds")
                                                {
                                                    Bitmap Obmp = new Bitmap(stream2);
                                                    //Obmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                                    pictureBox1.Image = Obmp;
                                                    Bitmap bmp1 = new Bitmap(pictureBox1.Image);
                                                    Bitmap bmp2 = new Bitmap(pictureBox1.Image);
                                                    Bitmap bmp3 = new Bitmap(pictureBox1.Image);
                                                    int x, y;
                                                    for (x = 0; x < bmp1.Width; x++)

                                                    {

                                                        for (y = 0; y < bmp1.Height; y++)
                                                        {
                                                            System.Drawing.Color oldPixelColor = bmp1.GetPixel(x, y);
                                                            if (oldPixelColor != System.Drawing.Color.FromArgb(0, 0, 0, 0))
                                                            {

                                                                System.Drawing.Color newPixelColor = System.Drawing.Color.FromArgb(oldPixelColor.A / 2, oldPixelColor.B, oldPixelColor.B, oldPixelColor.B);
                                                                //Color newPixelColor = Color.FromArgb(oldPixelColor.A, oldPixelColor.B, oldPixelColor.B, oldPixelColor.B);
                                                                //Color newPixelColor1 = Color.FromArgb(oldPixelColor.A, oldPixelColor.G, oldPixelColor.G, oldPixelColor.G);
                                                                //Color newPixelColor2 = Color.FromArgb(oldPixelColor.A, oldPixelColor.R, 0, 0);


                                                                bmp1.SetPixel(x, y, newPixelColor);


                                                            }
                                                        }

                                                    }
                                                    image = bmp1;

                                                }
                                                else
                                                    image = new Bitmap(stream2);
                                            }
                                            Size max = new System.Drawing.Size(1024, 1024);
                                            Bitmap bmp = image;
                                            Size sz2 = bmp.Size;
                                            string szs = sz2.ToString().Trim('{', '}');
                                            szs = szs.Replace("Width=", "");
                                            szs = szs.Replace("Height=", "");
                                            szs = szs.Replace(", ", "x") + "px";
                                            szs = szs.Replace(" ", "");
                                            label6.Text = szs;
                                            toolTip1.SetToolTip(this.pictureBox1, szs);
                                            if (max.Height > sz2.Height && max.Width > sz2.Width)
                                            {
                                                /* float zoom = (float)(500 / 4f + 1);
                                                int w = (int)sz2.Width * (int)zoom;
                                                int h = (int)sz2.Height * (int)zoom;
                                                Bitmap zoomed = ResizeImage(bmp, w, h);*/
                                                Bitmap zoomed = (Bitmap)pictureBox1.Image;
                                                if (zoomed != null) zoomed.Dispose();

                                                float zoom = (float)(100 / 4f + 1);
                                                zoomed = new Bitmap((int)(sz2.Width * zoom), (int)(sz2.Height * zoom));

                                                using (Graphics g = Graphics.FromImage(zoomed))
                                                {
                                                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                                                    g.DrawImage(bmp, new Rectangle(Point.Empty, zoomed.Size));
                                                }

                                                pictureBox1.Image = zoomed;
                                            }

                                            else
                                            {
                                                pictureBox1.Image = image;
                                            }



                                            //pictureBox1.Image = Newimage;
                                            this.pictureBox1.Visible = true;
                                            //this.label4.Visible = true;

                                            //pictureBox1.Dock = DockStyle.Fill;
                                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                        }
                                    }




                                    catch
                                    {

                                    }
                                }
                            }

                            pbsz = pictureBox1.Size;
                        }
                        else if (tpe == "" && (bool)items2[22])
                        {
                            byte[] OR = item1[12] as byte[];
                            byte[] ORsize = item1[13] as byte[];

                            if (BitConverter.IsLittleEndian)
                            {
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
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
                            Loaded_file = new byte[F1.Length];
                            Loaded_file = F1;
                            byte[] F2 = DecompressZlib(F1);
                            using (MemoryStream stream = new MemoryStream(F2))
                            {
                                try
                                {

                                    convertDDS(stream, "ext/temp/temp.png", ImageType.Dds);
                                    if (File.Exists("ext/temp/temp.png"))
                                    {
                                        Bitmap image;
                                        using (FileStream stream2 = new FileStream("ext/temp/temp.png", FileMode.Open))
                                        {
                                            Bitmap Obmp = new Bitmap(stream2);
                                            //Obmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                            pictureBox1.Image = Obmp;
                                            Bitmap bmp1 = new Bitmap(pictureBox1.Image);
                                            Bitmap bmp2 = new Bitmap(pictureBox1.Image);
                                            Bitmap bmp3 = new Bitmap(pictureBox1.Image);
                                            int x, y;
                                            for (x = 0; x < bmp1.Width; x++)

                                            {

                                                for (y = 0; y < bmp1.Height; y++)
                                                {
                                                    System.Drawing.Color oldPixelColor = bmp1.GetPixel(x, y);
                                                    if (oldPixelColor != System.Drawing.Color.FromArgb(0, 0, 0, 0))
                                                    {

                                                        System.Drawing.Color newPixelColor = System.Drawing.Color.FromArgb(oldPixelColor.A, oldPixelColor.B, oldPixelColor.B, oldPixelColor.B); //(oldPixelColor.A / 2, oldPixelColor.B, oldPixelColor.B, oldPixelColor.B);
                                                                                                                                                                                                //Color newPixelColor = Color.FromArgb(oldPixelColor.A, oldPixelColor.B, oldPixelColor.B, oldPixelColor.B);
                                                                                                                                                                                                //Color newPixelColor1 = Color.FromArgb(oldPixelColor.A, oldPixelColor.G, oldPixelColor.G, oldPixelColor.G);
                                                                                                                                                                                                //Color newPixelColor2 = Color.FromArgb(oldPixelColor.A, oldPixelColor.R, 0, 0);


                                                        bmp1.SetPixel(x, y, newPixelColor);


                                                    }
                                                }

                                            }
                                            image = bmp1;
                                            //image = new Bitmap(stream2);
                                        }
                                        Size max = new System.Drawing.Size(1024, 1024);
                                        Bitmap bmp = image;
                                        Size sz2 = bmp.Size;
                                        string szs = image.Size.ToString().Trim('{', '}');
                                        //string szs = sz2.ToString().Trim('{', '}');
                                        szs = szs.Replace("Width=", "");
                                        szs = szs.Replace("Height=", "");
                                        szs = szs.Replace(", ", "x") + "px";
                                        szs = szs.Replace(" ", "");
                                        label6.Text = szs;
                                        toolTip1.SetToolTip(this.pictureBox1, szs);
                                        if (max.Height > sz2.Height && max.Width > sz2.Width)
                                        {
                                            /*float zoom = (float)(500 / 4f + 1);
                                            int w = (int)sz2.Width * (int)zoom;
                                            int h = (int)sz2.Height * (int)zoom;
                                            Bitmap zoomed = ResizeImage(bmp, w, h);*/
                                            Bitmap zoomed = (Bitmap)pictureBox1.Image;
                                            if (zoomed != null) zoomed.Dispose();

                                            float zoom = (float)(100 / 4f + 1);
                                            zoomed = new Bitmap((int)(sz2.Width * zoom), (int)(sz2.Height * zoom));

                                            using (Graphics g = Graphics.FromImage(zoomed))
                                            {
                                                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                                                g.DrawImage(bmp, new Rectangle(Point.Empty, zoomed.Size));
                                                float s = (bmp.Width * (g.DpiX / bmp.HorizontalResolution));
                                            }

                                            //Clone it to another bitmap
                                            Bitmap zoomed2 = (Bitmap)zoomed.Clone();
                                            //Mirroring
                                            zoomed2.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                            pictureBox1.Image = zoomed2;
                                            this.pictureBox1.Visible = true;
                                            //this.label4.Visible = true;
                                            //newdds();
                                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                        }

                                        else
                                        {
                                            pictureBox1.Image = image;
                                        }

                                        //pictureBox1.Image = Newimage;
                                        this.pictureBox1.Visible = true;
                                        //this.label4.Visible = true;

                                        //pictureBox1.Dock = DockStyle.Fill;
                                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                        pbsz = pictureBox1.Size;
                                    }
                                    else
                                    {

                                    }
                                }
                                catch
                                {

                                }



                            }
                        }
                        else if (tpe == "mnu" || tpe == "path" || tpe == "ini" || tpe == "txt")
                        {
                            byte[] OR = item1[12] as byte[];
                            byte[] ORsize = item1[13] as byte[];

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

                            Loaded_file = new byte[F1.Length];
                            Loaded_file = F1;

                            string str = System.Text.Encoding.Default.GetString(F1);
                            if (tpe == "mnu  ")
                            {
                                str = str.Replace("\r\n", "\n");
                                //    Parse(str);
                            }
                            //else
                            //richTextBox1.ForeColor = System.Drawing.Color.Gainsboro;
                            richTextBox11.SuspendLayout();
                            this.richTextBox11.Text = str;
                            // this.richTextBox11.Visible = true;
                            richTextBox11.ResumeLayout();
                            richTextBox1.SuspendLayout();
                            this.richTextBox1.Text = str;
                            this.richTextBox1.Visible = true;
                            richTextBox1.ResumeLayout();
                        }
                        else if (tpe == "vpo" && File.Exists("ext/sce-cgcdisasm.exe") || tpe == "fpo" && File.Exists("ext/sce-cgcdisasm.exe"))
                        {
                            byte[] OR = item1[12] as byte[];
                            byte[] ORsize = item1[13] as byte[];

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
                            memStream2.Close();

                            try
                            {
                                string fileName = "ext/temp." + tpe;

                                using (FileStream
                                fileStream = new FileStream(fileName, FileMode.Create))
                                {
                                    // Write the data to the file, byte by byte.
                                    for (int i = 0; i < F1.Length; i++)
                                    {
                                        fileStream.WriteByte(F1[i]);
                                    }


                                }
                                Process p = new Process();
                                // Redirect the output stream of the child process.
                                p.StartInfo.UseShellExecute = false;
                                p.StartInfo.CreateNoWindow = true;
                                p.StartInfo.RedirectStandardOutput = true;
                                p.StartInfo.FileName = "ext/sce-cgcdisasm.exe";
                                p.StartInfo.Arguments = fileName;
                                p.StartInfo.RedirectStandardError = true;
                                p.Start();
                                // Do not wait for the child process to exit before
                                // reading to the end of its redirected stream.
                                // p.WaitForExit();
                                // Read the output stream first and then wait.
                                string output = p.StandardOutput.ReadToEnd();
                                //this.richTextBox1.SelectedText = p.StandardOutput.ReadToEnd();
                                string errors = p.StandardError.ReadToEnd();
                                p.WaitForExit();


                                // Loaded_file = new byte[F1.Length];
                                //Loaded_file = F1;
                                this.richTextBox11.Visible = false;
                                this.richTextBox1.Visible = false;
                                //string str = System.Text.Encoding.Default.GetString(F1);
                                //this.richTextBox1.Text = "";
                                //Parse(output);
                                //richTextBox1.SelectionColor = System.Drawing.Color.Gainsboro;
                                /*if (tpe == "vpo  " || tpe == "fpo  ")
                                {
                                    //str = str.Replace("\r\n", "\n");
                                    Parse(output);
                                }
                                else*/
                                //richTextBox1.ForeColor = System.Drawing.Color.Gainsboro;
                                richTextBox11.SuspendLayout();
                                this.richTextBox11.Text = output;

                                //this.richTextBox11.Visible = true;
                                richTextBox11.ResumeLayout();
                                richTextBox1.SuspendLayout();
                                this.richTextBox1.Text = output;

                                this.richTextBox1.Visible = true;
                                richTextBox1.ResumeLayout();
                            }
                            catch (Exception ex)
                            {
                                string e1 = ex.ToString();
                            }
                        }
                        else if (tpe == "gtf" && File.Exists("ext/gtf2dds.exe"))
                        {
                            byte[] OR = item1[12] as byte[];
                            byte[] ORsize = item1[13] as byte[];

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
                            memStream2.Close();
                            try
                            {
                                string fileName = "ext/temp/temp.dds";

                                using (FileStream
                                fileStream = new FileStream(fileName, FileMode.Create))
                                {
                                    // Write the data to the file, byte by byte.
                                    for (int i = 0; i < F1.Length; i++)
                                    {
                                        fileStream.WriteByte(F1[i]);
                                    }

                                }
                                Process p = new Process();
                                // Redirect the output stream of the child process.
                                p.StartInfo.UseShellExecute = false;
                                p.StartInfo.CreateNoWindow = true;
                                p.StartInfo.RedirectStandardOutput = true;
                                p.StartInfo.FileName = "ext/gtf2dds.exe";
                                p.StartInfo.Arguments = fileName;
                                p.Start();
                                // Do not wait for the child process to exit before
                                // reading to the end of its redirected stream.
                                // p.WaitForExit();
                                // Read the output stream first and then wait.
                                string output = p.StandardOutput.ReadToEnd();
                                p.WaitForExit();
                                if (File.Exists("ext/temp/temp.dds"))
                                {
                                    using (FileStream stream = new FileStream("ext/temp/temp.dds", FileMode.Open))
                                    {
                                        convertDDS(stream, "ext/temp/temp.png", ImageType.Dds);
                                    }

                                    if (File.Exists("ext/temp/temp.png"))
                                    {

                                        Bitmap image;
                                        using (FileStream stream2 = new FileStream("ext/temp/temp.png", FileMode.Open))
                                        {
                                            image = new Bitmap(stream2);
                                        }
                                        Size max = new System.Drawing.Size(1024, 1024);
                                        Bitmap bmp = image;
                                        Size sz2 = bmp.Size;
                                        string szs = sz2.ToString().Trim('{', '}');
                                        szs = szs.Replace("Width=", "");
                                        szs = szs.Replace("Height=", "");
                                        szs = szs.Replace(", ", "x") + "px";
                                        szs = szs.Replace(" ", "");
                                        label6.Text = szs;
                                        toolTip1.SetToolTip(this.pictureBox1, szs);
                                        if (max.Height > sz2.Height && max.Width > sz2.Width)
                                        {
                                            /* float zoom = (float)(500 / 4f + 1);
                                            int w = (int)sz2.Width * (int)zoom;
                                            int h = (int)sz2.Height * (int)zoom;
                                            Bitmap zoomed = ResizeImage(bmp, w, h);*/
                                            Bitmap zoomed = (Bitmap)pictureBox1.Image;
                                            if (zoomed != null) zoomed.Dispose();

                                            float zoom = (float)(100 / 4f + 1);
                                            zoomed = new Bitmap((int)(sz2.Width * zoom), (int)(sz2.Height * zoom));

                                            using (Graphics g = Graphics.FromImage(zoomed))
                                            {
                                                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                                                g.DrawImage(bmp, new Rectangle(Point.Empty, zoomed.Size));
                                            }

                                            pictureBox1.Image = zoomed;
                                        }

                                        else
                                        {
                                            pictureBox1.Image = image;
                                        }

                                        this.pictureBox1.Visible = true;
                                        //this.label4.Visible = true;

                                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string e1 = ex.ToString();
                            }
                        }
                        else
                        {

                            byte[] OR = item1[12] as byte[];
                            byte[] ORsize = item1[13] as byte[];

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
                            //BinaryFormatter binForm2 = new BinaryFormatter();
                            memStream2.Write(Filebytes, nor, orsize);
                            memStream2.Seek(0, SeekOrigin.Begin);
                            if (BitConverter.IsLittleEndian)
                            {
                                Array.Reverse(OR);
                                Array.Reverse(ORsize);
                            }

                            byte[] F1 = new byte[orsize];
                            byte[] loadedBtes = new byte[orsize];
                            using (MemoryStream ms = new MemoryStream())
                            {
                                int read;
                                while ((read = memStream2.Read(F1, 0, F1.Length)) > 0)
                                {
                                    ms.Write(F1, 0, read);
                                }

                            }
                            Array.Copy(F1, loadedBtes, F1.Length);
                            if (F1.Length > 0)
                            {
                                //hexaEditor1.DataContext = loadedBtes;
                                //hexaEditor1.FileName = "";
                                //File.WriteAllBytes("ext/temp.bin", F1);
                                //hexaEditor1.FileName = "ext/temp.bin";
                                hexaEditor1.byteName = F1;
                                //hexaEditor1.Stream = memStream2;
                                // memStream2.Read(F1, 0, orsize);
                                //memStream2.Seek(nor, SeekOrigin.Begin);
                                //Loaded_file = new byte[F1.Length];
                                //Loaded_file = F1;
                                //hexBox1.Dispose();
                                //this.hexBox1 = 
                                //elementHost1.Refresh();
                                //panel2.Controls.Add(elementHost1);
                                //this.elementHost1.Dock = DockStyle.Fill;
                                this.elementHost1.Visible = true;

                            }

                        }

                    }
                }
            }

            label6.Text = "";
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
            imageIn.Save(ms, ImageFormat.Gif);
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
            return ms.ToArray();
        }

        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
#pragma warning restore UnhandledExceptions // Unhandled exception(s)

            return destImage;
        }

        private void multiExtractToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void OpenQRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = AppPath;
            openFileDialog1.Filter = "QRC files (*.qrc; *.qrcf;*.xml)|*.qrc;*.qrcf;*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                string openedFile = openFileDialog1.FileName;

                //Now give it to the MRUManager
                this.mruManager.AddRecentFile(openedFile);

                label31.Text = "Loading...";
                label31.Visible = true;
                toolStrip1.Enabled = false;

                tableLayoutPanel1.Visible = false;
                listBox1.Visible = false;
                tableLayoutPanel1.Visible = false;
                CloseToolStripMenuItem_Click(openQRCToolStripMenuItem, EventArgs.Empty);
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel2.Text = "";

                Loaded_ext = "";
                //this.label4.Visible = false;
                //this.Text = "";
                //UpdateTextPosition();
                label31.Text = "";
                label31.Visible = true;
                label31.Dock = DockStyle.Fill;
                Disable();
                Refresh();

                if (openFileDialog1.FileName.Remove(0, openFileDialog1.FileName.LastIndexOf('.') + 1) == "xml")
                {
                    string file = openFileDialog1.FileName;
                    dwtype = "new";
                    toolStrip1.Enabled = false;
                    tableLayoutPanel1.Enabled = false;
                    listBox1.Visible = false;
                    tableLayoutPanel1.Visible = false;

                    BNew(file);

                }
                else
                {
                    Auto_Load(openFileDialog1.FileName);

                }
                listBox1.Update();
                //this.Text = openFileDialog1.FileName;
                //UpdateTextPosition();
                toolStripStatusLabel1.Text = openFileDialog1.FileName;
                toolStripStatusLabel1.Text = toolStripStatusLabel1.Text.Remove(0, toolStripStatusLabel1.Text.LastIndexOf("\\") + 1);
                toolStrip1.Enabled = true;
                label31.Visible = false;
                //listBox1.Visible = true;
                tableLayoutPanel1.Visible = true;
                /*this.listView1.Items[1].Focused = true;
                this.listView1.Items[1].Selected = true;
                listBox1.SelectedIndex = 1;
                this.listView1.Items[0].Focused = true;
                this.listView1.Items[0].Selected = true;
                listBox1.SelectedIndex = 0;
                listView1.Focus();*/
                setlist();
            }
            else
            {
                /*CloseToolStripMenuItem_Click(openQRCToolStripMenuItem, EventArgs.Empty);
                this.label31.Image = null;
                label31.Text = "";
                label31.Visible = true;
                toolStrip1.Enabled = true;*/
            }

        }

        void setlist()
        {
            if (this.listView1.Items.Count > 0)
            {
                //listBox1.Update();
                //this.Text = openFileDialog1.FileName;
                //UpdateTextPosition();
                toolStripStatusLabel1.Text = openFileDialog1.FileName;
                toolStripStatusLabel1.Text = toolStripStatusLabel1.Text.Remove(0, toolStripStatusLabel1.Text.LastIndexOf("\\") + 1);
                toolStrip1.Enabled = true;
                label31.Visible = false;
                //listBox1.Visible = true;
                tableLayoutPanel1.Visible = true;



                //this.listView1.Items[1].Focused = true;
                //this.listView1.Items[1].Selected = true;
                //listBox1.SelectedIndex = 1;
                this.listView1.Items[0].Focused = true;
                this.listView1.Items[0].Selected = true;
                //listBox1.SelectedIndex = 0;
                listView1.Focus();/**/
            }
        }

        private void myOwnRecentFileGotClicked_handler(object obj, EventArgs evt)
        {
            string fName = (obj as ToolStripItem).Text;
            if (!File.Exists(fName))
            {
                // if (MessageBox.Show(string.Format("{0} doesn't exist. Remove from recent workspaces?", fName), "File not found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //   this.mruManager.RemoveRecentFile(fName);
                return;
            }
            else if (File.Exists(fName))
            {
                openFileDialog1.FileName = fName;
                string openedFile = openFileDialog1.FileName;

                //Now give it to the MRUManager
                this.mruManager.AddRecentFile(openedFile);

                label31.Text = "Loading...";
                label31.Visible = true;
                toolStrip1.Enabled = false;

                tableLayoutPanel1.Visible = false;
                listBox1.Visible = false;
                tableLayoutPanel1.Visible = false;
                CloseToolStripMenuItem_Click(openQRCToolStripMenuItem, EventArgs.Empty);
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel2.Text = "";

                Loaded_ext = "";
                //this.label4.Visible = false;
                //this.Text = "";
                //UpdateTextPosition();
                label31.Text = "";
                label31.Visible = true;
                label31.Dock = DockStyle.Fill;
                Disable();
                Refresh();

                if (openFileDialog1.FileName.Remove(0, openFileDialog1.FileName.LastIndexOf('.') + 1) == "xml")
                {
                    string file = openFileDialog1.FileName;
                    dwtype = "new";
                    toolStrip1.Enabled = false;
                    tableLayoutPanel1.Enabled = false;
                    listBox1.Visible = false;
                    tableLayoutPanel1.Visible = false;

                    BNew(file);

                }
                else
                {
                    Auto_Load(openFileDialog1.FileName);

                }
                listBox1.Update();
                //this.Text = openFileDialog1.FileName;
                //UpdateTextPosition();
                /*toolStripStatusLabel1.Text = openFileDialog1.FileName;
                toolStripStatusLabel1.Text = toolStripStatusLabel1.Text.Remove(0, toolStripStatusLabel1.Text.LastIndexOf("\\") + 1);
                toolStrip1.Enabled = true;
                label31.Visible = false;
                //listBox1.Visible = true;
                tableLayoutPanel1.Visible = true;
                this.listView1.Items[1].Focused = true;
                this.listView1.Items[1].Selected = true;
                listBox1.SelectedIndex = 1;
                this.listView1.Items[0].Focused = true;
                this.listView1.Items[0].Selected = true;
                listBox1.SelectedIndex = 0;
                listView1.Focus();*//**/
                setlist();
            }

            //OpenQRCToolStripMenuItem_Click(obj, evt);
            //do something with the file here
            //MessageBox.Show(string.Format("Through the 'Recent Files' menu item, you opened: {0}", fName));
        }

        private void myOwnRecentFilesGotCleared_handler(object obj, EventArgs evt)
        {
            //prior to this function getting called, all recent files in the registry and 
            //in the program's 'Recent Files' menu are cleared.

            //perhaps you want to do something here after all this happens
            //MessageBox.Show("You just cleared all recent files.");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options OptionsChild = new Options();

#pragma warning disable UnhandledExceptions // Unhandled exception(s)
            OptionsChild.ShowDialog(this);
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
        }

        private void SaveQRCFToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "QRCF files (*.qrcf)|*.qrcf|All files (*.*)|*.*";
            saveFileDialog1.DefaultExt = "QRCF files (*.qrcf)|*.qrcf";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            //label31.Text = "Saving QRCF...";
            //label31.Visible = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //waitForm.Show(this);
                dwtype = "qrcf";
                toolStrip1.Enabled = false;
                tableLayoutPanel1.Enabled = false;
                listBox1.Visible = false;
                tableLayoutPanel1.Visible = false;
                darkLabel1.Text = "Saving QRC";
                start_progress();
                dw();
                //stop_progress();
                //waitForm.Close();
                /*
                string QRCF_File = saveFileDialog1.FileName;
                FileStream q = File.Open(QRCF_File, FileMode.Create);
                q.Write(Filebytes, 0, Filebytes.Length);
                q.Close();
                */
            }
            else
            {
                // waitForm.Close();
                //label31.Text = "";
                //label31.Visible = false;
            }
        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem is DataRowView item1)
            {

                DataRow items2 = item1.Row;
                More_File_Info mfi = new More_File_Info(items2);

#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                mfi.ShowDialog(this);
#pragma warning restore UnhandledExceptions // Unhandled exception(s)

            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void qRCToQRCFToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "QRC files (*.qrc)|*.qrc|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.DefaultExt = "QRC files (*.qrc)|*.qrc";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //waitForm.Show(this);
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                FileStream q = File.Open(openFileDialog1.FileName, FileMode.Open);
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
                    byte[] temp = DecompressZlib(m.ToArray());
                    saveFileDialog2.Filter = "QRCF files (*.qrcf)|*.qrcf|All files (*.*)|*.*";
                    if (saveFileDialog2.ShowDialog() == DialogResult.OK)
                    {
                        FileStream q1 = File.Open(saveFileDialog2.FileName, FileMode.Create);
                        q1.Write(temp, 0, temp.Length);
                        q1.Close();
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
                    }
                    //waitForm.Close();
                }
                else if (Check_Magic(testmagic, qrcfmagic) == true)
                {
                    // waitForm.Close();
                    MessageBox.Show("Error", "Not a Valid QRC File");
                }
                else
                {
                    // waitForm.Close();
                    MessageBox.Show("Error", "Not a Valid QRC File");
                }
                q.Close();
            }
        }

        private void qRCFToQRCToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "QRCF files (*.qrcf)|*.qrcf|All files (*.*)|*.*";
            openFileDialog1.DefaultExt = "QRC files (*.qrcf)|*.qrcf";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //waitForm.Show(this);
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                FileStream q = File.Open(openFileDialog1.FileName, FileMode.Open);

                Filebytes = new byte[q.Length];
                q.Read(Filebytes, 0, Filebytes.Length);

                byte[] testmagic = new byte[4];
                Array.Copy(Filebytes, 0, testmagic, 0, 4);
                byte[] qrcmagic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
                byte[] qrcfmagic = new byte[] { 0x51, 0x52, 0x43, 0x46 };
#pragma warning restore UnhandledExceptions // Unhandled exception(s)

                if (Check_Magic(testmagic, qrcmagic) == true)
                {
                    //waitForm.Close();
                    MessageBox.Show("Error", "Not a Valid QRCF File");
                }
                else if (Check_Magic(testmagic, qrcfmagic) == true)
                {
                    //byte[] bytes1 = new byte[q.Length];
                    // q.Read(bytes1, 0, bytes1.Length);



                    saveFileDialog1.Filter = "QRC files (*.qrc)|*.qrc|All files (*.*)|*.*";
                    saveFileDialog1.DefaultExt = "QRC files (*.qrc)|*.qrc";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;
                    //label31.Text = "Saving QRC...";
                    //label31.Visible = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //waitForm.Show(this);
                        dwtype = "qrc";
                        toolStrip1.Enabled = false;
                        tableLayoutPanel1.Enabled = false;
                        listBox1.Visible = false;
                        tableLayoutPanel1.Visible = false;
                        darkLabel1.Text = "Opening QRC";
                        start_progress();
                        dw();
                        //stop_progress();
                        //waitForm.Close();

                        //string QRCF_File = saveFileDialog2.FileName;
                        /* FileStream q1 = File.Open(saveFileDialog2.FileName, FileMode.Create);
                         byte[] temp = CompressZlib(bytes1);
                         byte[] New_Magic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
                         byte[] FSize = new byte[4];
                         FSize = BitConverter.GetBytes(bytes1.Length);
                         if (BitConverter.IsLittleEndian)
                         {
                             Array.Reverse(FSize);
                         }
                         q1.Write(New_Magic, 0, New_Magic.Length);
                         q1.Write(FSize, 0, FSize.Length);
                         q1.Write(temp, 0, temp.Length);
                         q1.Close();*/
                    }
                    //waitForm.Close();

                }
                else
                {
                    //waitForm.Close();
                    MessageBox.Show("Error", "Not a Valid QRCF File");
                }
                q.Close();
            }
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
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                    Directory.CreateDirectory("tmp");
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
                }

                Object[] items2 = item1.Row.ItemArray;
                Extract_File(dr, items2);
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                Process.Start(dr);
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
            }

        }

        #endregion<<>>

        #region<<helpers>>

        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value); }

            int c = 0;
            decimal dValue = (decimal)value;
            /*while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                c++;
            }*/

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[c]);
        }

        void StartUp()
        {
            

            var versionInfo = FileVersionInfo.GetVersionInfo(AppPath);
            version = versionInfo.FileVersion;
            clean();
            if (richTextBox1.WordWrap == true)
            {
                wordWrapToolStripMenuItem.Checked = true;
            }
            //click event
            //MessageBox.Show("you got it!");
            /*ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem = new MenuItem("Cut");
            //menuItem.Click += new EventHandler(CutAction);
            //contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem("Copy");
            menuItem.Click += new EventHandler(CopyAction);
            contextMenu.MenuItems.Add(menuItem);
            menuItem = new MenuItem("Word Wrap");
            //menuItem.Checked
            menuItem.Click += new EventHandler(WordWrapAction);
            contextMenu.MenuItems.Add(menuItem);
            //menuItem = new MenuItem("Paste");
            //menuItem.Click += new EventHandler(PasteAction);
            //contextMenu.MenuItems.Add(menuItem);

            richTextBox1.ContextMenu = contextMenu;*/

            ContextMenu lcontextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem lmenuItem = new MenuItem("Copy Name");
            lmenuItem.Click += new EventHandler(CopynameAction);
            lcontextMenu.MenuItems.Add(lmenuItem);
            lmenuItem = new MenuItem("Copy Size");
            lmenuItem.Click += new EventHandler(CopysizeAction);
            lcontextMenu.MenuItems.Add(lmenuItem);
            //lmenuItem = new MenuItem("Delete Item");
            //lmenuItem.Click += new EventHandler(DeleteItemAction);
            //lcontextMenu.MenuItems.Add(lmenuItem);
            //menuItem = new MenuItem("Paste");
            //menuItem.Click += new EventHandler(PasteAction);
            //contextMenu.MenuItems.Add(menuItem);

            listView1.ContextMenu = lcontextMenu;

            ContextMenu pcontextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem pmenuItem = new MenuItem("Background Color");
            //menuItem.Click += new EventHandler(CutAction);
            //contextMenu.MenuItems.Add(menuItem);

            pmenuItem.Click += new EventHandler(BackgroundColorAction);
            pcontextMenu.MenuItems.Add(pmenuItem);
            //menuItem = new MenuItem("Paste");
            //menuItem.Click += new EventHandler(PasteAction);
            //contextMenu.MenuItems.Add(menuItem);

            pictureBox1.ContextMenu = pcontextMenu;
            string d = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            //d = d.Replace("", "");
            d = d.Substring(d.LastIndexOf('\\') + 1);
            d = d.Replace(".exe", "");
            this.mruManager = new MRUManager(this.recentFilesToolStripMenuItem, d, this.myOwnRecentFileGotClicked_handler,
                this.myOwnRecentFilesGotCleared_handler);
            Set_Size();
            hexaEditor1.ReadOnlyMode = true;
            // hexBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            elementHost1.Refresh();
            panel2.Controls.Add(elementHost1);
            byte[] fill = { 0x00, 0x00, 0x00, 0x00 };
            hexaEditor1.byteName = fill;
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel2.Text = "";
            listBox1.Visible = false;
            tableLayoutPanel1.Visible = true;
            this.elementHost1.Visible = true;
            // tableLayoutPanel1.Visible = false;
            listView1.Visible = false;
            this.label31.Image = null;
            //this.label5.Text = " ";
            //this.label4.Visible = false;
            this.richTextBox11.Visible = false;
            this.richTextBox1.Visible = false;
            //this.richTextBox1.Dock = DockStyle.Fill;
            this.elementHost1.Dock = DockStyle.Fill;
            //this.elementHost1.Visible = false;

            this.pictureBox1.Visible = false;
            this.pictureBox1.Dock = DockStyle.Fill;
            label31.Text = "";
            //label31.Dock = DockStyle.Fill;
            // hexaEditor1.FileName = "test.bin";
            toolTip1.SetToolTip(darkButton1, "Extract Selected File");
            toolTip1.SetToolTip(darkButton2, "Replace Selected File");
            //Disable();


            if (Directory.Exists("PS3_Gaia_Visualization_Helper"))
            {
                string[] files = System.IO.Directory.GetFiles("PS3_Gaia_Visualization_Helper", "*.bat");

                foreach (string f in files)
                {
                    if (f.StartsWith("PS3_Gaia_Visualization_Helper"))
                    {

                        deViL303sToolBoxToolStripMenuItem.Visible = true;
                        break;
                    }
                    else
                    {
                        deViL303sToolBoxToolStripMenuItem.Visible = false;
                    }
                }
            }
            else
            {
                deViL303sToolBoxToolStripMenuItem.Visible = false;
            }

            Fileinfo.Columns.Add("name");
            Fileinfo.Columns.Add("TOC_Offset", typeof(byte[]));
            Fileinfo.Columns.Add("Row", typeof(Int32));
            Fileinfo.Columns.Add("Name_Offset");
            Fileinfo.Columns.Add("Attributes");
            Fileinfo.Columns.Add("Parent_Offset");
            Fileinfo.Columns.Add("Previous_Brother_Offset");
            Fileinfo.Columns.Add("Next_Brother_Offset");
            Fileinfo.Columns.Add("First_Child_Offset");
            Fileinfo.Columns.Add("Last_Child_Offset");
            Fileinfo.Columns.Add("Attribute1_Name_Offset");
            Fileinfo.Columns.Add("Attribute1_Type");
            Fileinfo.Columns.Add("Attribute1_Variable_1", typeof(byte[]));
            Fileinfo.Columns.Add("Attribute1_Variable_2", typeof(byte[]));
            Fileinfo.Columns.Add("Attribute2_Name_Offset");
            Fileinfo.Columns.Add("Attribute2_Type");
            Fileinfo.Columns.Add("Attribute2_Variable_1");
            Fileinfo.Columns.Add("Attribute2_Variable_2");
            //Fileinfo.Columns.Add("attribute1_variable_1_add");
            Fileinfo.Columns.Add("Attribute3_Name_Offset");
            Fileinfo.Columns.Add("Attribute3_Type");
            Fileinfo.Columns.Add("Attribute3_Variable_1");
            Fileinfo.Columns.Add("Attribute3_Variable_2");
            Fileinfo.Columns.Add("File_Compressed", typeof(bool));
            Fileinfo.Columns.Add("File_Extension", typeof(string));
            //listBox1.DataSource = Fileinfo;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "Row";
            if (Show_Splash == true)
            {
                this.toolStripMenuItem1.Checked = true;
                toolStripMenuItem1.Text = "Splash Screen [On]";
            }
            else if (Show_Splash == false)
            {
                this.toolStripMenuItem1.Checked = false;
                toolStripMenuItem1.Text = "Splash Screen [Off]";
            }
            //checkicon();
            //SetText();


            if (!IsAdministrator())
            {
                customFiletypeIconsToolStripMenuItem.ToolTipText = "Run as Administrator to set File Associations";
                customFiletypeIconsToolStripMenuItem.Enabled = false;
                qRCToolStripMenuItem.Enabled = false;
                qRCFToolStripMenuItem.Enabled = false;
            }
            else
                checkicon();

            Disable();
        }

        void Set_Size()
        {
            try
            {


                this.darkStatusStrip1.Font = new Font("Segoe UI", (float)9, FontStyle.Regular);
                this.toolStrip1.Font = new Font("Segoe UI", (float)9, FontStyle.Regular);
                listView1.Font = new Font("Segoe UI", (float)8.25, FontStyle.Regular);
                /*columnHeader1.Width = 270;
                columnHeader2.Width = 80;
                this.listView1.Width = 371;
                columnHeader1.AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
                columnHeader2.AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);*/
                //this.listView1.Font = new Font("Microsoft Sans Serif", (float)8.25, FontStyle.Regular);
                //this.richTextBox1.Font = new Font("Microsoft Sans Serif", (float)8.25, FontStyle.Regular);
                if (listView1.Items.Count > 0 && this.listView1.Items[0].Bounds.Height != 0 && listView1.ClientSize.Height != 0)
                {
                    /* if (this.listView1.ClientSize.Width > this.listView1.Items[0].Bounds.Width)
                     {
                         listView1.BeginUpdate();
                         int t = this.listView1.ClientSize.Width - this.listView1.Items[0].Bounds.Width;
                         columnHeader1.Width += t;
                         int i = columnHeader1.Width + columnHeader2.Width;
                         listView1.EndUpdate();
                         // listView1.Width = listView1.Width - t;
                     }

                     else if (this.listView1.ClientSize.Width < this.listView1.Items[0].Bounds.Width + 21)
                     {

                         int t = this.listView1.Items[0].Bounds.Width - this.listView1.ClientSize.Width;
                         int i = columnHeader1.Width + columnHeader2.Width;
                         listView1.Width = listView1.Width + t;
                     }

                     if (this.listView1.ClientSize.Height > (this.listView1.Items.Count + 1) * this.listView1.Items[0].Bounds.Height)
                     {

                         columnHeader2.Width = columnHeader2.Width;
                     }*/

                }

            }
            catch
            {

            }
        }

        void Set_list()
        {
            try
            {


                this.darkStatusStrip1.Font = new Font("Segoe UI", (float)9, FontStyle.Regular);
                this.toolStrip1.Font = new Font("Segoe UI", (float)9, FontStyle.Regular);
                listView1.Font = new Font("Segoe UI", (float)8.25, FontStyle.Regular);
                //columnHeader1.Width = 100;
                //columnHeader2.Width = 80;
                this.listView1.Width = 200;
                //this.tableLayoutPanel1.Location = new System.Drawing.Point(386, 37);
                //this.tableLayoutPanel1.Size = oldtw;
                columnHeader1.AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
                columnHeader2.AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
                //this.listView1.Font = new Font("Microsoft Sans Serif", (float)8.25, FontStyle.Regular);
                //this.richTextBox1.Font = new Font("Microsoft Sans Serif", (float)8.25, FontStyle.Regular);
                if (listView1.Items.Count > 0 && this.listView1.Items[0].Bounds.Height != 0 && listView1.ClientSize.Height != 0)
                {
                    /*if (oldtw.Width == 0)
                    {
                        oldtw = this.tableLayoutPanel1.Size;
                    }
                    if (oldfw.Width == 0)
                    {
                        oldfw = this.Size;
                    }
                    this.tableLayoutPanel1.Location = new System.Drawing.Point(215, 37);
                    this.Size = oldfw;
                    this.tableLayoutPanel1.Size = oldtw;*/
                    if (this.listView1.ClientSize.Width > this.listView1.Items[0].Bounds.Width)
                    {
                        listView1.BeginUpdate();
                        int w = this.tableLayoutPanel1.Size.Width;
                        if (oldtw.Width == 0)
                        {
                            oldtw = this.tableLayoutPanel1.Size;
                        }
                        if (oldfw.Width == 0)
                        {
                            oldfw = this.Size;
                        }
                        //int nw = w - oldtw;
                        //this.tableLayoutPanel1.Location = new System.Drawing.Point(215, 37);

                        //this.Width = oldfw;// this.Width - nw;
                        //this.Size = RestoreBounds.Size;//oldfw;
                        // this.tableLayoutPanel1.Size = oldtw;// new System.Drawing.Size(oldtw, 438);



                        int t = this.listView1.ClientSize.Width - this.listView1.Items[0].Bounds.Width;
                        columnHeader1.Width += t;

                        listView1.EndUpdate();
                        // listView1.Width = listView1.Width - t;
                    }

                    else if (this.listView1.ClientSize.Width < this.listView1.Items[0].Bounds.Width + 21)
                    {

                        int t = this.listView1.Items[0].Bounds.Width - this.listView1.ClientSize.Width;

                        listView1.Width = listView1.Width + t;


                        int w = this.tableLayoutPanel1.Size.Width;
                        oldfw = this.Size;
                        oldtw = this.tableLayoutPanel1.Size;
                        //this.tableLayoutPanel1.Size = new System.Drawing.Size(w - t, 438);
                        int n = tableLayoutPanel1.Location.X + t;

                        if (WindowState != FormWindowState.Maximized)
                        {
                            //this.tableLayoutPanel1.Location = new System.Drawing.Point(n, 37);
                            // this.Width = this.Width + t;
                            //this.tableLayoutPanel1.Size = new System.Drawing.Size(w, this.tableLayoutPanel1.Size.Height);

                        }
                        else
                        {
                            //this.tableLayoutPanel1.Location = new System.Drawing.Point(n, 37);
                            // this.Width = this.Width + t;
                            //this.tableLayoutPanel1.Size = new System.Drawing.Size(w - t, this.tableLayoutPanel1.Size.Height);
                        }


                    }

                    /*if (this.listView1.ClientSize.Height > (this.listView1.Items.Count + 1) * this.listView1.Items[0].Bounds.Height)
                    {

                        columnHeader2.Width = columnHeader2.Width;
                    }*/

                }

            }
            catch
            {

            }
        }

        void SetRTB(string typ)
        {
            //SetText();


            /*
                        richTextBox11.ForeColor = System.Drawing.Color.Gainsboro;
                        this.richTextBox11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
                        this.richTextBox11.BorderStyle = System.Windows.Forms.BorderStyle.None;
                        this.richTextBox11.ContextMenuStrip = this.contextMenuStrip1;
                        this.richTextBox11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        //this.richTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
                        this.richTextBox11.Location = new System.Drawing.Point(3, 30);
                        this.richTextBox11.Name = "richTextBox1";
                        this.richTextBox11.ReadOnly = true;
                        this.richTextBox11.Size = new System.Drawing.Size(183, 288);
                        this.richTextBox11.TabIndex = 16;
                        this.richTextBox11.Text = "";
                        this.richTextBox11.WordWrap = false;
                        this.richTextBox11.MouseUp += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseUp);
                        panel2.Controls.Add(this.richTextBox11);

                        if (richTextBox11.WordWrap == true)
                        {
                            wordWrapToolStripMenuItem.Checked = true;
                        }
                        this.richTextBox11.Visible = false;
                        this.richTextBox11.Dock = DockStyle.Fill;
                        */


            this.richTextBox1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.richTextBox1.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\n^\\s*(case|default)\\s*[^:]*(" +
    "?<range>:)\\s*(?<range>[^;]+);";
            this.richTextBox1.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.richTextBox1.BackBrush = null;
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.richTextBox1.CharHeight = 14;
            this.richTextBox1.CharWidth = 8;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.richTextBox1.IsReplaceMode = false;

            this.richTextBox1.Paddings = new System.Windows.Forms.Padding(0);
            this.richTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            //this.richTextBox1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(ComponentResourceManager..resources.GetObject("richTextBox1.ServiceColors")));
            this.richTextBox1.Size = new System.Drawing.Size(150, 150);
            this.richTextBox1.TabIndex = 19;
            //this.richTextBox1.Text = "fastColoredTextBox1";
            this.richTextBox1.Zoom = 100;
            richTextBox1.ForeColor = System.Drawing.Color.Gainsboro;
            this.richTextBox1.IndentBackColor = System.Drawing.Color.DimGray;

            this.richTextBox1.LineNumberColor = System.Drawing.Color.Gainsboro;
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.richTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.richTextBox1.Location = new System.Drawing.Point(3, 30);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(183, 288);
            this.richTextBox1.TabIndex = 16;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseUp);
            panel2.Controls.Add(this.richTextBox1);
            richTextBox1.Dock = DockStyle.Fill;

            if(typ == "vpo" || typ == "fpo")
            {
                this.richTextBox1.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.richTextBox1_TextChanged);
            }
            else
            {
                this.richTextBox1.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.richTextBox1_TextChangedMNU);
            }


            this.richTextBox1.TextChanging += new System.EventHandler<FastColoredTextBoxNS.TextChangingEventArgs>(this.richTextBox1_TextChanging);



        }

        void Form1_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                var temp = e.Data.GetData(DataFormats.FileDrop);
                string t2 = temp.GetType().ToString();
                if (t2 == "System.String[]")
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files != null)
                    {
                        string file = files[0];
                        if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
                        //if (e.Data.GetDataPresent(DataFormats.FileDrop) && (Path.GetExtension(file) == ".qrc" || Path.GetExtension(file) == ".qrcf" || Path.GetExtension(file) == ".xml")) e.Effect = DragDropEffects.Copy;
                    }
                }
            }
            catch
            {

            }
        }

        void Form1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var temp = e.Data.GetData(DataFormats.FileDrop);
                string t2 = temp.GetType().ToString();
                if (t2 == "System.String[]")
                {
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files[0] != null)
                    {
                        string file = files[0];
                        string n = System.IO.Path.GetDirectoryName(AppPath) + "\\ext\\temp";
                        string f = Path.GetDirectoryName(file);
                        if (f != n)
                        {


                            if (Path.GetExtension(file) == ".qrc" || Path.GetExtension(file) == ".qrcf" || Path.GetExtension(file) == ".xml")
                            {

                                string ext = Path.GetExtension(file);



                                listBox1.Visible = false;
                                tableLayoutPanel1.Visible = false;
                                tableLayoutPanel1.Visible = false;
                                if (Path.GetExtension(file) == ".qrc" || Path.GetExtension(file) == ".qrcf")
                                {


                                    Auto_Load(file);

                                    toolStripStatusLabel1.Text = file;
                                    toolStripStatusLabel1.Text = toolStripStatusLabel1.Text.Remove(0, toolStripStatusLabel1.Text.LastIndexOf("\\") + 1);
                                    //this.Text = file;
                                    //UpdateTextPosition();

                                    toolStrip1.Enabled = true;
                                    label31.Visible = false;
                                    //listBox1.Visible = true;
                                    tableLayoutPanel1.Visible = true;
                                }
                                else if (Path.GetExtension(file) == ".xml")
                                {
                                    label31.Text = "Building New QRC...";
                                    label31.Visible = true;
                                    //string[] files = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.xml");
                                    dwtype = "new";
                                    toolStrip1.Enabled = false;
                                    tableLayoutPanel1.Enabled = false;
                                    listBox1.Visible = false;
                                    tableLayoutPanel1.Visible = false;
                                    BNew(file);

                                    toolStripStatusLabel1.Text = file;
                                    toolStripStatusLabel1.Text = toolStripStatusLabel1.Text.Remove(0, toolStripStatusLabel1.Text.LastIndexOf("\\") + 1);
                                    //this.Text = file;
                                    //UpdateTextPosition();
                                    toolStrip1.Enabled = true;
                                    label31.Visible = false;
                                    //listBox1.Visible = true;
                                    tableLayoutPanel1.Visible = true;
                                    label31.Text = "";
                                    label31.Visible = false;
                                }


                            }
                            else if (Path.GetExtension(file) == Loaded_ext)
                            {
                                if (File.Exists(file))
                                {
                                    if (listView1.SelectedIndices.Count > 0)
                                    {

                                        //hexaEditor1.FileName = "";
                                        int si = listView1.SelectedIndices[0];
                                        DataRow item1 = Fileinfo.Rows[si] as DataRow;
                                        //DataRowView item1 = this.listBox1.SelectedItem as DataRowView;
                                        string i = item1[0].ToString();
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                                        //DialogResult dialogResult = MessageBox.Show("Do you want to overwrite \"" + i + " with " + file + "\"? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (MessageBox.Show("Do you want to overwrite \"" + i + " with " + file + "\"? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                        {
                                            srow = (int)item1[2];
                                            Object[] items2 = item1.ItemArray;
                                            Replace_File(file, items2);
                                            //FileStream q = File.Open(file, FileMode.Open);
                                            //New_Loaded_file = new byte[q.Length];
                                            // q.Read(New_Loaded_file, 0, New_Loaded_file.Length);
                                            //q.Close();

                                            MessageBox.Show(this, i + " replaced with " + file, "File replaced", MessageBoxButtons.OK);
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public void BuildNewQRC(string file, string QRCF_Folder, string New_QRCF_File)
        {
            //waitForm.Show(this);
            List<string> sn1 = new List<string>();
            List<string> sn2 = new List<string>();
            List<string> sn3 = new List<string>();
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
                string attType = "";
                //FileStream f1 = File.Open(QRCF_Folder, FileMode.Open);
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                StreamReader f1 =
    new System.IO.StreamReader(file);
                while ((line = f1.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    if (line.Contains("<qrc "))
                    {
                        line = line.Replace("<qrc compression=\"", "");
                        line = line.Replace("\">", "");
                        line = line.Replace("\" attributes=\"", "-");
                        string[] l2 = line.Split('-');
                        if (l2.Length == 2)
                        {
                            if (l2[0] == " False" && l2[1] == "3")
                            {
                                attType = "icontex";
                            }
                            if (l2[0] == " False" && l2[1] == "2")
                            {
                                attType = "QRCF";
                            }
                            if (l2[0] == " True" && l2[1] == "2")
                            {
                                attType = "QRC";
                            }

                        }
                        else
                        {
                            break;
                        }

                    }
                    if (line.Contains("<file src="))
                    {
                        line = line.Replace("\t\t<file src=\"", "");
                        line = line.Replace("\" id=\"", "=");
                        line = line.Replace("\" compression=\"", "=");
                        line = line.Replace("\" />", "");
                        line = line.Replace("\"", "");
                        string[] l2 = line.Split('=');
                        if (l2.Length == 3)
                        {
                            sn1.Add(l2[0]);
                            sn2.Add(l2[1]);
                            sn3.Add(l2[2]);
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                if (attType == "icontex")
                {
                    TAgstableSize = new byte[] { 0x00, 0x00, 0x00, 0x20 };
                    TAGSTable = new byte[]{ 0x71, 0x72, 0x63, 0x00, 0x66, 0x69, 0x6C, 0x65, 0x2D, 0x74, 0x61, 0x62,
                        0x6C, 0x65, 0x00, 0x66, 0x69, 0x6C, 0x65, 0x00, 0x73, 0x72, 0x63, 0x00, 0x69, 0x64, 0x00, 0x73, 0x69, 0x7A, 0x65, 0x00 };

                }
                f1.Close();
                int y = 0x38;
                int i = 0;
                int sn = sn1.Count;
                byte[][] a3n3 = new byte[sn][];
                List<string> rMsn1 = new List<string>();
                foreach (string s in sn1)
                {
                    if (File.Exists(QRCF_Folder + "/" + s))
                    {

                        byte[] fb = File.ReadAllBytes(QRCF_Folder + "/" + s);
                        //bool enc = false;
                        a3n3[i] = new byte[4];
                        byte[] testmagic = new byte[2];
                        Array.Copy(fb, 0, testmagic, 0, 2);
                        byte[] zlibmagic = new byte[] { 0x78, 0xDA };
                        if (attType == "icontex" && sn3[i] == "True" && Check_Magic(testmagic, zlibmagic) == false)
                        {
                            a3n3[i] = BitConverter.GetBytes(fb.Length);
                            byte[] fb1 = CompressZlib(fb);
                            Array.Resize(ref fb, fb.Length);
                            Array.Copy(fb1, fb, fb1.Length);
                            //enc = true;
                        }
                        else if (attType == "icontex" && sn3[i] == "True" && Check_Magic(testmagic, zlibmagic) == true)
                        {
                            byte[] fb1 = DecompressZlib(fb);
                            a3n3[i] = BitConverter.GetBytes(fb1.Length);
                            //enc = true;
                        }
                        file_sizes.Add(fb.Length);
                        Array.Reverse(a3n3[i]);
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
                        if (attType == "icontex")
                        {
                            y = y + 0x4C;
                        }
                        else
                        {
                            y = y + 0x3C;
                        }

                        i++;
                    }
                    else
                    {
                        rMsn1.Add(s);
                        i++;
                        //break;
                    }

                }
                bool con = true;
                if (rMsn1.Count > 0)
                {
                    if (MessageBox.Show(QRCF_Folder + " is missing files!\nContinue?", "Missing files!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        foreach (string s in rMsn1)
                        {
                            try
                            {
                                sn1.Remove(s);
                            }
                            catch
                            { }

                        }
                    }
                    else
                    {
                        con = false;
                        //DarkMessageBox.ShowInformation("Please Check Again Later.", "Please Check Again Later.", DarkDialogButton.Ok);
                    }
                }
                i = 0;
                if (con == true)
                {

                    byte[] STable = name_bytes.SelectMany(s => s)
                              .ToArray();

                    STableSize = BitConverter.GetBytes(STable.Length);

                    PadToMultipleOf(ref STable, 16);

                    int ITocS = 60 * sn1.Count;
                    TOCSize = BitConverter.GetBytes(ITocS + 56);
                    byte[] TOC = new byte[ITocS + 56];
                    last_child_elemen = BitConverter.GetBytes(TOC.Length - 60);
                    if (attType == "icontex")
                    {
                        ITocS = 76 * sn1.Count;
                        TOCSize = BitConverter.GetBytes(ITocS + 56);
                        TOC = new byte[ITocS + 56];
                        last_child_elemen = BitConverter.GetBytes(TOC.Length - 76);
                    }
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
                    MemoryStream q = new MemoryStream();

                    q.Write(Tables, 0, Tables.Length);

                    byte[] previous_brother = { 0xFF, 0xFF, 0xFF, 0xFF };
                    int next_brother = 0x74;
                    byte[] nb = new byte[4];

                    int offset = TOC.Length;
                    i = 0;
                    int foff = 0;
                    int fnoff = 0;
                    if (attType == "icontex")
                    {
                        next_brother = 0x84;
                    }
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
                        if (attType == "icontex")
                        {
                            tocstart = new byte[] { 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x1C };

                        }
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

                        if (attType == "icontex")
                        {

                            byte[] att3 = { 0x00, 0x00, 0x00, 0x1B, 0x00, 0x00, 0x00, 0x01 };

                            byte[] att32 = { 0x00, 0x01, 0x55, 0xD4 };
                            byte[] att33 = { 0x00, 0x00, 0x00, 0x00 };
                            q.Write(att3, 0, att3.Length);
                            q.Write(a3n3[i], 0, a3n3[i].Length);
                            q.Write(att33, 0, att33.Length);
                            previous_brother = BitConverter.GetBytes(next_brother - 0x4C);
                            next_brother = next_brother + 0x4C;
                        }
                        else
                        {
                            previous_brother = BitConverter.GetBytes(next_brother - 0x3C);
                            next_brother = next_brother + 0x3C;
                        }
                        i++;
                    }
                    byte[] p = new byte[] { 0x00 };
                    while (q.Length % 16 != 0)
                    {
                        q.Write(p, 0, p.Length);
                    }
                    q.Write(STable, 0, STable.Length);
                    q.Write(TAGSTable, 0, TAGSTable.Length);
                    q.Write(FTable, 0, FTable.Length);


                    FileStream q1 = File.Open(New_QRCF_File, FileMode.Create);
                    byte[] Filebytes1 = q.ToArray();

                    q.Close();
                    byte[] temp;

                    if (attType == "icontex")
                    {
                        temp = new byte[Filebytes1.Length];
                        Array.Copy(Filebytes1, temp, Filebytes1.Length);
                        q1.Write(temp, 0, temp.Length);
                        q1.Close();

                    }


                    else if (attType == "QRCF" || attType == "QRC")
                    {
                        temp = CompressZlib(Filebytes1);
                        byte[] New_Magic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
                        byte[] FSize = new byte[4];
                        FSize = BitConverter.GetBytes(Filebytes1.Length);
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(FSize);
                        }
                        q1.Write(New_Magic, 0, New_Magic.Length);
                        q1.Write(FSize, 0, FSize.Length);
                        q1.Write(temp, 0, temp.Length);
                        q1.Close();
                    }
                    //Auto_Load(New_QRCF_File);
                }
            }
#pragma warning restore UnhandledExceptions // Unhandled exception(s)

        }

        public DataTable Open_File()
        {
            listBox1.DataSource = null;

            if (sn != null)
            {
                sn.Clear();
            }

            byte[] Headsize = new byte[4];
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
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


            listBox1.BeginUpdate();
            int o = 0;
            foreach (QRC_File w in t.files)
            {

                if (BitConverter.ToInt32(w.attributes, 0) == 2 || BitConverter.ToInt32(w.attributes, 0) == 3)
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
                    if (BitConverter.ToInt32(w.attributes, 0) == 3)
                    {

                    }

                    memStream.Close();
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(w.attributes);
                        Array.Reverse(w.attribute2_variable_1);
                        Array.Reverse(w.attribute2_variable_2);
                    }
                    string try2 = BitConverter.ToString(w.attribute3_type, 0);
                    bool Compressed = false;
                    if (BitConverter.ToString(w.attribute3_type, 0) == "00-00-00-01")
                    {
                        Compressed = true;
                    }
                    string e = "";
                    string utfString = Encoding.UTF8.GetString(File_Name2, 0, File_Name2.Length);
                    if (Compressed && utfString.LastIndexOf('.') < 0)
                    {
                        byte[] OR = w.attribute1_variable_1;
                        byte[] ORsize = w.attribute1_variable_2;

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


                        byte[] F2 = DecompressZlib(F1);
                        byte[] testmagic = new byte[4];
                        Array.Copy(F2, 0, testmagic, 0, 4);
                        byte[] ddsmagic = new byte[] { 0x44, 0x44, 0x53, 0x20 };
                        if (Check_Magic(testmagic, ddsmagic) == true)
                        {

                            e = ".dds";
                        }
                    }

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

                    dr["attribute3_name_offset"] = BitConverter.ToString(w.attribute3_name_offset, 0);
                    dr["attribute3_type"] = BitConverter.ToString(w.attribute3_type, 0);
                    dr["attribute3_variable_1"] = BitConverter.ToString(w.attribute3_variable_1, 0);
                    dr["attribute3_variable_2"] = BitConverter.ToString(w.attribute3_variable_2, 0);
                    dr["File_Compressed"] = Compressed;
                    dr["File_Extension"] = e;
                    Fileinfo.Rows.Add(dr);


                }


                o++;

            }
            listBox1.EndUpdate();
            listBox1.DataSource = Fileinfo;
            //Enable();
            //listBox1.SelectedIndex = 1;
            //listBox1.SelectedIndex = 0;
            return Fileinfo;
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
        }

        void Auto_Load(string fname)
        {
            label31.Visible = true;
            //label31.BackColor = System.Drawing.Color.Black;
            Fileinfo.Clear();
            if (File.Exists(fname))
            {
                ltype = "QRC";


                changed = false;
                QRC_File_Name = fname;

#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                FileStream q = File.Open(QRC_File_Name, FileMode.Open);

                byte[] Filebytes1 = new byte[q.Length];
                q.Read(Filebytes1, 0, Filebytes1.Length);
                //this.label5.Text = " ";
                byte[] testmagic = new byte[4];
                Array.Copy(Filebytes1, 0, testmagic, 0, 4);
                byte[] qrcmagic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
                byte[] qrcfmagic = new byte[] { 0x51, 0x52, 0x43, 0x46 };
                md5 = Get_MD5(Filebytes1);
                //string zipPath = "";

                label6.Text = Check_MD5(md5, "ext/MD5Data");

                split_string(md5).Replace('\0', ' ');
                //label5.Text = "MD5 " + md5;

                string[] md5s = { "97342FFE4E3B427E9C2133C1BB6A3BB4", "90146D3F374EC92EBED34535E90CD298", "261CD31A2516EF85A6D3B584C9BD02C5", "99F008FE3C2BACB937BFC925004E6E99", "1FCC497E937FDC6CDAD6A1707C9CD187", "053DA0D0ED6B336E12665D41F0A18387", "BEDFE56680ED40807C53E61603C76E1D", "5E551FE46DCA11258A29031128E8D998" };
                if (md5s.Contains(md5))
                {
                    switch (md5)
                    {
                        case "97342FFE4E3B427E9C2133C1BB6A3BB4":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.10~4.85 icons.qrc)";
                            break;
                        case "90146D3F374EC92EBED34535E90CD298":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.00~4.85 icontex.qrc)";
                            break;
                        case "261CD31A2516EF85A6D3B584C9BD02C5":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.60~4.85 lines.qrc)";
                            break;
                        case "99F008FE3C2BACB937BFC925004E6E99":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.00~4.85 rhm.qrc)";
                            break;
                        case "1FCC497E937FDC6CDAD6A1707C9CD187":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.10~4.85 raf.qrc)";
                            break;
                        case "053DA0D0ED6B336E12665D41F0A18387":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.40~4.85 canyon.qrc)";
                            break;
                        case "BEDFE56680ED40807C53E61603C76E1D":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.40~4.85 earth.qrc)";
                            break;
                        case "5E551FE46DCA11258A29031128E8D998":
                            toolStripStatusLabel2.Text = "MD5 " + md5 + "     (PS3 OFW 4.40~4.85 store.qrc)";
                            break;


                    }
                }
                else
                    toolStripStatusLabel2.Text = "MD5 " + md5;

                // Filebytes1.


                if (Check_Magic(testmagic, qrcmagic) == true)
                {
                    QRCcompression = true;
                    byte[] nb = new byte[Filebytes1.Length - 8];
                    Array.Copy(Filebytes1, 8, nb, 0, nb.Length);
                    Filebytes = DecompressZlib(nb);

                    Open_File();

                    listView1.Items.Clear();
                    //darkListView1.Columns.Add(header);

                    foreach (DataRow row in Fileinfo.Rows)
                    {
                        byte[] Size = row[13] as byte[];
                        Array.Reverse(Size);
                        string sz = SizeSuffix(BitConverter.ToInt32(Size, 0), 0);
                        Array.Reverse(Size);
                        ListViewItem item = new ListViewItem(row[0].ToString());
                        item.SubItems.Add(sz);
                        item.ImageKey = getIcon(row[0].ToString());
                        item.ToolTipText = "";

                        listView1.Items.Add(item);

                    }
                    //Dispatcher v = new Dispatcher;

                    Enable();
                    listView1.BeginUpdate();

                    Set_Size();
                    Set_list();
                    listView1.EndUpdate();
                }
                else if (Check_Magic(testmagic, qrcfmagic) == true)
                {
                    QRCcompression = false;
                    Filebytes = new byte[Filebytes1.Length];
                    Array.Copy(Filebytes1, 0, Filebytes, 0, Filebytes1.Length);

                    Open_File();

                    listView1.Items.Clear();
                    //darkListView1.Columns.Add(header);

                    foreach (DataRow row in Fileinfo.Rows)
                    {
                        byte[] Size = row[13] as byte[];
                        Array.Reverse(Size);
                        string sz = SizeSuffix(BitConverter.ToInt32(Size, 0), 0);
                        Array.Reverse(Size);
                        ListViewItem item = new ListViewItem(row[0].ToString());
                        item.SubItems.Add(sz);
                        string ik = getIcon(row[0].ToString());

                        item.ImageKey = ik;


                        listView1.Items.Add(item);
                    }
                    Enable();
                    listView1.BeginUpdate();
                    Set_Size();
                    Set_list();
                    listView1.EndUpdate();

                }
                else
                {
                    MessageBox.Show("Error", "Not a Valid QRC File");
                }
                q.Close();
                label31.Visible = false;
#pragma warning restore UnhandledExceptions // Unhandled exception(s)

            }

        }

        string getIcon(string name)
        {
            var images = imageList1.Images;
            int t = name.LastIndexOf('.') + 1;
            string ext = name.Substring(name.LastIndexOf('.') + 1);
            string file_icon = "";
            if (t == 0)
            {
                ext = "dds";

            }

            if (ext == "jpg")
            {
                file_icon = "jpg.png";
            }
            else if (ext == "mnu")
            {
                file_icon = "mnu.png";
            }
            else if (ext == "vpo")
            {
                file_icon = "vpo.png";
            }
            else if (ext == "bmp")
            {
                file_icon = "bmp.png";
            }
            else if (ext == "path")
            {
                file_icon = "path.png";
            }
            else if (ext == "fpo")
            {
                file_icon = "fpo.png";
            }
            else if (ext == "dds")
            {
                file_icon = "dds.png";
            }
            else if (ext == "ini")
            {
                file_icon = "ini.png";
            }
            else if (ext == "gtf")
            {
                file_icon = "gtf.png";
            }
            else if (ext == "tga")
            {
                file_icon = "tga.png";
            }
            else if (ext == "txt")
            {
                file_icon = "txt.png";
            }
            else if (ext == "")
            {
                file_icon = "dds.png";
            }
            else if (ext == "elf")
            {
                file_icon = "elf.png";
            }
            else if (ext == "bin")
            {
                file_icon = "bin.png";
            }
            else if (ext == "dump")
            {
                file_icon = "dump.png";
            }
            else
            {
                file_icon = "";
            }
            return file_icon;
        }

        bool Check_Magic(byte[] testmagic, byte[] magic)
        {
            bool isqrcf = testmagic.SequenceEqual(magic);
            return isqrcf;
        }

        void Disable()
        {
            try
            {
                listView1.Items.Clear();
                //tableLayoutPanel1.Visible = false;
                listBox1.Visible = false;
                listView1.Visible = false;
                toolStripStatusLabel1.Text = "";
                toolStripStatusLabel2.Text = "";
                md5ToolStripMenuItem.Enabled = false;
                label6.Text = "";
                //this.label5.Text = " ";
                this.listBox1.SelectedIndexChanged += null;
                //toolStripDropDownButton2.Enabled = false;
                //toolStripDropDownButton6.Enabled = false;
                //this.label4.Visible = false;
                this.richTextBox11.Visible = false;
                this.richTextBox11.Dock = DockStyle.Fill;
                this.richTextBox1.Visible = false;
                this.richTextBox1.Dock = DockStyle.Fill;
                this.elementHost1.Dock = DockStyle.Fill;
                //this.elementHost1.Visible = false;
                this.pictureBox1.Visible = false;
                this.pictureBox1.Dock = DockStyle.Fill;
                //label31.Text = "";
                //label31.Dock = DockStyle.Fill;
                darkButton1.Enabled = false;
                darkButton2.Enabled = false;
                extractAllToolStripMenuItem.Enabled = false;
                closeToolStripMenuItem.Enabled = false;
                compressQRCToolStripMenuItem.Enabled = false;
                //saveQRCFToolStripMenuItem1.Enabled = false;
                extractFileToolStripMenuItem1.Enabled = false;
                replaceFileToolStripMenuItem1.Enabled = false;
                compressQRCToolStripMenuItem.Text = "Save File";
                darkButton3.Text = "Save File";
                Fileinfo.Clear();
                if (Filebytes != null)
                {
                    Array.Clear(Filebytes, 0, Filebytes.Length);
                }
                tableLayoutPanel1.Visible = false;
                //columnHeader2.Width = 80;
            }
            catch { }
        }

        void Enable()
        {
            try
            {
                //listBox1.Visible = true;
                listView1.Visible = true;
                tableLayoutPanel1.Visible = true;
                md5ToolStripMenuItem.Enabled = true;
                this.listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
                //toolStripDropDownButton2.Enabled = true;
                //toolStripDropDownButton6.Enabled = true;
                darkButton1.Enabled = true;
                darkButton2.Enabled = true;
                extractAllToolStripMenuItem.Enabled = true;
                closeToolStripMenuItem.Enabled = true;
                compressQRCToolStripMenuItem.Enabled = true;
                compressQRCToolStripMenuItem.Text = "Save " + ltype + " File";
                darkButton3.Text = "Save " + ltype + " File";
                //saveQRCFToolStripMenuItem1.Enabled = true;
                extractFileToolStripMenuItem1.Enabled = true;
                replaceFileToolStripMenuItem1.Enabled = true;
            }
            catch { }
        }

        void Extract_File(string QRCF_File, Object[] item)
        {
            byte[] OR = item[12] as byte[];
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
            byte[] ORsize = item[13] as byte[];
            string a3 = item[22].ToString();
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

            if (a3 == "True")
            {
                F1 = DecompressZlib(F1.ToArray());
                byte[] testmagic = new byte[4];
                Array.Copy(F1, 0, testmagic, 0, 4);
                byte[] ddsmagic = new byte[] { 0x44, 0x44, 0x53, 0x20 };
                if (Check_Magic(testmagic, ddsmagic) == true)
                {

                    //QRCF_File;// += ".dds";
                }

            }

            File.WriteAllBytes(QRCF_File, F1);
#pragma warning restore UnhandledExceptions // Unhandled exception(s)

        }
#pragma warning disable UnhandledExceptions // Unhandled exception(s)

        public void Replace_File(string NEW_File, Object[] item)
        {
            if (File.Exists(NEW_File))
            {
                srow = (int)item[2];
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



                    if (items2[4].ToString() == "00-00-00-03")
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
                changed = true;
                //this.listBox1.SelectedIndex = srow - 2;
                //this.listView1.SelectedIndices[0] = srow - 2;
                listView1.Items.Clear();
                //darkListView1.Columns.Add(header);

                foreach (DataRow row in Fileinfo.Rows)
                {
                    byte[] Size = row[13] as byte[];
                    Array.Reverse(Size);
                    string sz = SizeSuffix(BitConverter.ToInt32(Size, 0), 0);
                    Array.Reverse(Size);
                    ListViewItem item2 = new ListViewItem(row[0].ToString());
                    item2.SubItems.Add(sz);
                    item2.ImageKey = getIcon(row[0].ToString());
                    item2.ToolTipText = "";
                    listView1.Items.Add(item2);

                }
                //Dispatcher v = new Dispatcher;

                Enable();
                listView1.BeginUpdate();

                Set_Size();
                Set_list();
                listView1.EndUpdate(); ;
                this.listView1.Items[srow - 2].Focused = true;
                this.listView1.Items[srow - 2].Selected = true;
                this.listView1.Focus();
            }
        }

        public void Replace_File2(string NEW_File, Object[] item)
        {
            if (File.Exists(NEW_File))
            {

                FileStream q = File.Open(NEW_File, FileMode.Open);
                //Array.Reverse(items2[12] as byte[]);
                byte[] Filebytes1 = new byte[q.Length];
                q.Read(Filebytes1, 0, Filebytes1.Length);
                q.Close();
                byte[] New_add = new byte[4];
                New_add = BitConverter.GetBytes(Filebytes1.Length);
                PadToMultipleOf(ref Filebytes1, 16);
                //Array.Reverse(item[12] as byte[]);
                byte[] OR = item[12] as byte[];
                byte[] ORsize = item[13] as byte[];
                int ORrow = (int)item[2];

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(OR);
                    Array.Reverse(ORsize);
                    Array.Reverse(New_add);
                    Array.Reverse(FTS);
                    //Array.Reverse(FTO);
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

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(OR);
                    Array.Reverse(ORsize);
                    Array.Reverse(New_add);
                    Array.Reverse(FTS);
                    //Array.Reverse(FTO);
                }
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
                if (Filebytes1.Length == orsize)
                {
                    diff = Math.Abs(orsize - Filebytes1.Length);
                    fts -= diff;
                    add = "none";
                }
                FTS = BitConverter.GetBytes(fts);
                foreach (DataRow item1 in Fileinfo.Rows)
                {
                    Object[] items2 = item1.ItemArray;



                    if (items2[4].ToString() == "00-00-00-02")
                    {
                        byte[] tocadd = items2[1] as byte[];
                        byte[] Old_add = items2[16] as byte[];
                        if (BitConverter.IsLittleEndian)
                        {
                            //Array.Reverse(FTS);
                            Array.Reverse(tocadd);
                            Array.Reverse(Old_add);
                        }
                        int oldfileadd = BitConverter.ToInt32(Old_add, 0);
                        int fileadd = BitConverter.ToInt32(tocadd, 0) + 0x28 + 0x40;
                        int NEWrow = (int)items2[2];
                        //byte[] New_add = new byte[4];
                        byte[] New_l = new byte[4];
                        if (oldfileadd > 0)
                        {
                            if (add == "add")
                            {
                                oldfileadd += diff;
                            }
                            else if (add == "sub")
                            {
                                oldfileadd -= diff;
                            }
                            else if (add == "none")
                            {
                                //oldfileadd -= diff;
                            }
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

                        if (NEWrow > orrow || NEWrow < orrow)
                        {
                            Array.Copy(New_l, 0, New_Filebytes, fileadd - 4, New_l.Length);
                        }
                        if (BitConverter.IsLittleEndian)
                        {
                            // Array.Reverse(New_l);
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
                listView1.Items.Clear();
                //darkListView1.Columns.Add(header);

                foreach (DataRow row in Fileinfo.Rows)
                {
                    byte[] Size = row[13] as byte[];
                    Array.Reverse(Size);
                    string sz = SizeSuffix(BitConverter.ToInt32(Size, 0), 0);
                    Array.Reverse(Size);
                    ListViewItem item2 = new ListViewItem(row[0].ToString());
                    item2.SubItems.Add(sz);
                    item2.ImageKey = getIcon(row[0].ToString());
                    item2.ToolTipText = "";
                    listView1.Items.Add(item2);

                }
                //Dispatcher v = new Dispatcher;

                Enable();
                listView1.BeginUpdate();

                Set_Size();
                Set_list();
                listView1.EndUpdate(); ;
                this.listView1.Items[srow - 2].Focused = true;
                this.listView1.Items[srow - 2].Selected = true;
                this.listView1.Focus();
            }
        }

        public void ExtractAll()
        {
            int na = 0;
            string xname = this.toolStripStatusLabel1.Text;
            xname = xname.Trim(' ');
            xname = xname.Remove(xname.LastIndexOf('.'), xname.Length - xname.LastIndexOf('.'));
            //int a = 0;
            //start_progress();
            string folder = folderBrowserDialog1.SelectedPath;
            foreach (DataRow item1 in Fileinfo.Rows)
            {
                Object[] items2 = item1.ItemArray;
                string name = items2[0].ToString();
                string at = items2[4].ToString();
                char[] a = at.ToCharArray();
                char[] a2 = new char[a.Length - 3];
                int acount = 0;
                int a2count = 0;
                while (a2count < a2.Length)
                {
                    Array.Copy(a, acount, a2, a2count, 2);
                    a2count += 2;
                    acount += 3;
                }
                string newatt = new string(a2);
                newatt = newatt.TrimStart('0');
                na = Int32.Parse(newatt);
                //a = BitConverter.ToInt32(at, 0);




                string ext = name;
                if (ext.Contains('.'))
                    ext = ext.Remove(0, name.LastIndexOf('.'));

                foreach (string[] EXT1 in Options.EXTS)
                {
                    string EXT2 = EXT1[0];
                    if (ext == EXT2 && EXT1[0] != "")
                    {
                        name.Replace(ext, ext + EXT1[0]);
                    }

                    string dr = "";
                    int dr1 = name.LastIndexOf('/');

                    if (dr1 > 0)
                    {
                        dr = name.Remove(dr1, name.Length - dr1);

                    }
                    if (dr.Length != name.Length)
                    {
                        //Directory.CreateDirectory(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + dr);
                        Directory.CreateDirectory(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\" + "\\" + dr);
                    }
                }


                //Extract_File(Path.GetFileNameWithoutExtension(QRC_File_Name) + "/" + name, items2);
                Extract_File(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\" + name, items2);

            }

            WriteXML(folder + "\\" + Path.GetFileNameWithoutExtension(QRC_File_Name) + "\\", xname, na);
            //stop_progress();
        }

        public void Save_QRCF()
        {

            string QRCF_File = saveFileDialog1.FileName;
            FileStream q = File.Open(QRCF_File, FileMode.Create);
            q.Write(Filebytes, 0, Filebytes.Length);
            q.Close();
            changed = false;
        }

        public void Save_QRC()
        {
            string QRCF_File = saveFileDialog1.FileName;
            FileStream q = File.Open(QRCF_File, FileMode.Create);
            //FileStream q2 = File.Open(QRCF_File + ".zip", FileMode.Create);
            byte[] temp = CompressZlib(Filebytes);
            //byte[] temp2 = CompressDeflate(Filebytes);
            byte[] New_Magic = new byte[] { 0x51, 0x52, 0x43, 0x43 };
            byte[] FSize = new byte[4];
            FSize = BitConverter.GetBytes(Filebytes.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(FSize);
            };
            // q2.Write(temp2, 0, temp2.Length);
            // q2.Close();
            q.Write(New_Magic, 0, New_Magic.Length);
            q.Write(FSize, 0, FSize.Length);
            q.Write(temp, 0, temp.Length);
            q.Close();
            changed = false;
        }

        public void BNew(string file)
        {
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
                    darkLabel1.Text = "Building QRC";
                    start_progress();
                    dw2();
                    //stop_progress();
                }
                else
                {

                }

            }
        }

        public void WriteXML(string path, string xname, int attributesN)
        {
            using (System.IO.StreamWriter nfile =

            new System.IO.StreamWriter(path + xname + ".xml", false))
            {
                string com = "True";
                if (attributesN == 3 && QRCcompression.ToString() == "False")
                {
                    com = "False";
                }
                nfile.WriteLine("<?xml version=" + '"' + "1.0" + '"' + " encoding=" + '"' + "UTF-8" + '"' + "?>");
                nfile.WriteLine(" <qrc compression=\"" + com + "\" attributes=\"" + attributesN + "\">");
                nfile.WriteLine("	<file-table>");

                foreach (DataRow r in Fileinfo.Rows)
                {
                    Object[] item = r.ItemArray;

                    string name = item[0].ToString();
                    string id = item[23].ToString();
                    bool fileC = (bool)item[22];


                   // nfile.WriteLine("		<file src=" + '"' + name + id + '"' + " id=" + '"' + name + "\" compression=\"" + fileC.ToString() + "\" />");

                    nfile.WriteLine("		<file src=" + '"' + name + '"' + " id=" + '"' + name + "\" compression=\"" + fileC.ToString() + "\" />");

                }

                nfile.WriteLine("	</file-table>\n</qrc>");

            }

        }

        private static void PadToMultipleOf(ref byte[] src, int pad)
        {
            int len = (src.Length + pad - 1) / pad * pad;
            Array.Resize(ref src, len);
        }

        public string Get_File_MD5(DataRowView item)
        {
            string r = "";
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
            memStream2.Close();
            r = Get_MD5(F1);

            return r;
        }

        public static string Get_MD5(byte[] bytes)
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

        public string Check_MD5(string md5, string file)
        {
            List<string[]> l1 = new List<string[]>(2);
            string out_name = "";
            if (File.Exists(file))
            {

                string line;
                //FileStream f1 = File.Open(QRCF_Folder, FileMode.Open);
                System.IO.StreamReader f1 =
    new System.IO.StreamReader(file);
                while ((line = f1.ReadLine()) != null)
                {
                    System.Console.WriteLine(line);
                    if (line.Contains("<md5 hash="))
                    {
                        line = line.Replace("\t\t<md5 hash=\"", "");
                        line = line.Replace("\" id=\"", "=");
                        line = line.Replace("\" />", "");
                        line = line.Replace("\"", "");
                        string[] l2 = line.Split('=');
                        if (l2.Length == 2)
                        {
                            l1.Add(l2);
                            if (l2[0] == md5)
                            {
                                out_name = l2[1];
                                break;
                            }
                        }

                    }

                }
                f1.Close();
                if (l1[0].Contains("dog"))
                {
                    Console.WriteLine("dog was found");
                }
                Console.WriteLine(l1[0].Contains("fish"));
            }
            label6.Text = out_name;
            return out_name;
        }

        public string split_string(string instring)
        {
            int i = 0;
            int r = 0;
            char[] outchar = new char[instring.Length + (instring.Length / 2)];
            while (i < instring.Length)
            {

                instring.CopyTo(i, outchar, r, 2);
                i += 2;
                r += 3;
            }
            StringBuilder outstring = new StringBuilder();
            foreach (char value in outchar)
            {
                outstring.Append(value);
            }
            return outstring.ToString().Replace('\0', ' ');
        }

        #endregion<<helpers>>

        #region<<Background worker>>

        private void dw()
        {



            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += worker_DoWork;

            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            //start_progress();
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
                else if (dwtype == "open")
                {
                    Open_File();
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
                    byte[] Filebytes1 = new byte[Filebytes.Length];
                    for (long i = 8; i < Filebytes1.Length; i++)
                        // m.WriteByte(Filebytes1[i]);
                        Filebytes = DecompressZlib(Filebytes1);
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
            label31.Image = null;
            toolStrip1.Enabled = true;
            label31.Visible = false;
            listBox1.Enabled = true;
            //listBox1.Visible = true;
            tableLayoutPanel1.Enabled = true;
            stop_progress();
            //waitForm.Close();


        }

        void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            dwtype = "";
            label31.Text = "";
            label31.Visible = false;
            label31.Image = null;
            toolStrip1.Enabled = true;
            label31.Visible = false;
            listBox1.Enabled = true;
            //listBox1.Visible = true;
            tableLayoutPanel1.Enabled = true;
            if (File.Exists(New_qrcf))
            {
                Auto_Load(New_qrcf);
            }
            stop_progress();
            setlist();

        }

        #endregion

        #region<<update>>

        private void clean()
        {

            if (File.Exists("XForge_OldVersion.exe"))
            {
                Thread.Sleep(1000);
                File.Delete("XForge_OldVersion.exe");
            }
            if (File.Exists("Update"))
            {
                File.Delete("Update");
            }


        }

        private void checkUpdate()
        {
            String[] cuSplit = version.Split('.');

            if (cuSplit.Length == 4)
            {
                //old_file_name = cu1;
                int[] iu = new int[4];
                int[] ic = new int[4];

                int i = 0;
                while (i < ic.Length)
                {
                    int t = Int32.Parse(cuSplit[i]);
                    ic[i] = t;
                    i++;
                }

                var client = new MegaApiClient();
                client.LoginAnonymous();

                Uri folderLink = new Uri("https://mega.nz/#F!8yISwKyL!kQwk5rHLk1LNShU1ErKHnA");
                IEnumerable<INode> nodes = client.GetNodesFromLink(folderLink);
                foreach (INode node in nodes.Where(x => x.Type == NodeType.File))
                {
                    if (node.Name.EndsWith(".upt"))
                    {
                        string up = node.Name.Substring(7, node.Name.Length - 11);
                        String[] upSplit = up.Split('.');
                        if (upSplit.Length == 4)
                        {
                            i = 0;
                            while (i < iu.Length)
                            {
                                iu[i] = Int32.Parse(upSplit[i]);
                                i++;
                            }
                            if (iu[0] > ic[0] || iu[1] > ic[1] || iu[2] > ic[2] || iu[3] > ic[3])
                            {
                                //DarkMessageBox.ShowInformation("Update " + up + " Available!\nUpdate Now?", "Update Available!", DarkDialogButton.YesNo);
                                // linkLabel1.Text = "Update " + up + " Available!";
                                Newnode = node;
                            }
                        }
                    }

                }

                client.Logout();
                if (Newnode != null)
                {
                    string up = Newnode.Name.Substring(7, Newnode.Name.Length - 11);
                    //DarkDialog..ShowInformation("Update " + up + " Available!\nUpdate Now?", "Update Available!", DarkDialogButton.YesNo);
                    if (MessageBox.Show("Update " + up + " Available!\nUpdate Now?", "Update Available!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        startUpdate(Newnode);
                    }
                    else
                    {
                        //DarkMessageBox.ShowInformation("Please Check Again Later.", "Please Check Again Later.", DarkDialogButton.Ok);
                    }
                }
                else
                {
                    var window = MessageBox.Show("No New Updates are Available.", "No Updates Available.", MessageBoxButtons.OK);
                    //DarkMessageBox.ShowInformation("No New Updates are Available.", "No Updates Available.", DarkDialogButton.Ok);

                }
            }
        }

        private void startUpdate(INode node)
        {
            string[] lines;
            if (Newnode != null)
            {
                try
                {
                    var client = new MegaApiClient();
                    client.LoginAnonymous();
                    client.DownloadFile(Newnode, Newnode.Name);
                    client.Logout();
                }
                catch
                {

                }
            }

            if (File.Exists(Newnode.Name))
            {

                var list = new List<string>();
                var fileStream = new FileStream(Newnode.Name, FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                lines = list.ToArray();

                if (lines.Length == 2)
                {
                    if (lines[0] == "Download")
                    {

                        var client = new MegaApiClient();
                        client.LoginAnonymous();

                        Uri fileLink = new Uri(lines[1]);
                        INodeInfo node1 = client.GetNodeFromLink(fileLink);

                        //Console.WriteLine($"Downloading {node.Name}");
                        client.DownloadFile(fileLink, "Update");

                        client.Logout();
                        update();
                    }
                    else if (lines[0] == "Link")
                    {

                    }
                }
                else
                {

                }

                File.Delete(Newnode.Name);
            }

        }

        private void update()
        {
            if (File.Exists("Update"))
            {
                string p = "";
                string destinationFile = System.Reflection.Assembly.GetExecutingAssembly().Location;

                // Assumes: using System.Reflection;
                Assembly currentAssembly = Assembly.GetEntryAssembly();
                if (currentAssembly == null)
                    currentAssembly = Assembly.GetCallingAssembly();
                if (currentAssembly.Location.ToUpper() == destinationFile.ToUpper())
                {
                    string appFolder = Path.GetDirectoryName(currentAssembly.Location);
                    string appName = Path.GetFileNameWithoutExtension(currentAssembly.Location);
                    string appExtension = Path.GetExtension(currentAssembly.Location);
                    string archivePath = Path.Combine(appFolder, appName + "_OldVersion" + appExtension);
                    p = appName + appExtension;
                    if (File.Exists(archivePath))
                        File.Delete(archivePath);

                    File.Move(destinationFile, archivePath);
                }

                File.Copy("Update", p, true);
                File.Delete("Update");
                Process.Start(p);
                Application.Exit();
            }
        }

        private void checkUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkUpdate();
        }

        #endregion<<update>>



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (changed)
                {
                    var window = MessageBox.Show(
                  "Exit without saving changes?",
                  "Unsaved changes",
                   MessageBoxButtons.YesNo);

                    e.Cancel = (window == DialogResult.No);
                    if (DialogResult.ToString() == "Yes")
                    {
                        changed = false;
                    }
                }

                if (!changed)
                {
                    hexaEditor1.CloseProvider();
                    elementHost1.Refresh();


                    if (WindowState == FormWindowState.Minimized)
                    {
                        WindowState = FormWindowState.Normal;
                    }


                    if (WindowState == FormWindowState.Maximized)
                    {
                        Properties.Settings.Default.Location = RestoreBounds.Location;
                        Properties.Settings.Default.Size = RestoreBounds.Size;
                        Properties.Settings.Default.Maximised = true;
                        Properties.Settings.Default.Minimised = false;
                    }
                    else if (WindowState == FormWindowState.Normal)
                    {
                        Properties.Settings.Default.Location = Location;
                        Properties.Settings.Default.Size = Size;
                        Properties.Settings.Default.Maximised = false;
                        Properties.Settings.Default.Minimised = false;
                    }
                    else
                    {
                        Properties.Settings.Default.Location = RestoreBounds.Location;
                        Properties.Settings.Default.Size = RestoreBounds.Size;
                        Properties.Settings.Default.Maximised = false;
                        Properties.Settings.Default.Minimised = false;
                    }
                    Properties.Settings.Default.Save();
                    ZLibNative.AssemblyCleanup();

                    hexaEditor1.Dispose();
                    elementHost1.Dispose();
                    this.m_importer.Dispose();
                    this.m_exporter.Dispose();
                    m_activeImage.Dispose();
                    var exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;

                    if (!exists)
                    {
                        if (Directory.Exists("tmp"))
                        {
                            Directory.Delete("tmp", true);
                        }
                        if (Directory.Exists("ext/temp"))
                        {
                            Directory.Delete("ext/temp", true);
                        }

                    }/**/
                }
            }
            catch { }
        }


        private void md5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MD5s t = new MD5s(Fileinfo, Filebytes, QRC_File_Name, md5);
            t.ShowDialog(this);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.Maximised)
            {
                WindowState = FormWindowState.Maximized;
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
            else if (Properties.Settings.Default.Minimised)
            {
                WindowState = FormWindowState.Normal;
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }
            else
            {
                Location = Properties.Settings.Default.Location;
                Size = Properties.Settings.Default.Size;
            }

            try
            {
                GetChInterop1.KbHit();
            }
            catch
            {
                try
                {
                    GetChInterop2.KbHit();
                }
                catch
                {
                    if (!Directory.Exists("ext"))
                    {
                        Directory.CreateDirectory("ext");
                    }
                    if (!File.Exists("ext/msvcr71.dll"))
                    {
                        File.WriteAllBytes("ext/msvcr71.dll", Properties.Resources.msvcr71);
                    }

                }
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItem1.Checked == true)
            {
                toolStripMenuItem1.Checked = false;
            }
            else
                toolStripMenuItem1.Checked = true;
        }

        private void qRCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            checkicon();
            if (File.Exists(Path.Combine(directory, "XForge", "qrc.ico")))
            {
                string d = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                string icon = Path.Combine(directory, "XForge", "qrc.ico");
                if (!FileAssociations.IsAssociated(".qrc"))
                {
                    FileAssociations.Associate(".qrc", "QRC_File", "qrc File", icon, d);
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                    checkicon();
                }
                else
                {
                    Registry.ClassesRoot.DeleteSubKey(".qrc");
                    //Registry.ClassesRoot.DeleteSubKey(".qrc");
                    //SetAssociation_User("qrc", d, "XForge", Path.Combine(directory, "XForge", "qrc.ico"));
                    //FileAssociations.Associate(".qrc", "", "", "", "");
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                    checkicon();
                }
            }
        }

        private void qRCFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkicon();
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (File.Exists(Path.Combine(directory, "XForge", "qrcf.ico")))
            {
                string d = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                string icon = Path.Combine(directory, "XForge", "qrcf.ico");

                //SetAssociation_User("qrcf", d, "XForge", Path.Combine(directory, "XForge", "qrcf.ico"));


                // bool g = FileAssociations.IsAssociated(".qrcf");
                if (!FileAssociations.IsAssociated(".qrcf"))
                {
                    FileAssociations.Associate(".qrcf", "QRCF_File", "qrcf File", icon, d);
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                    checkicon();
                }
                else
                {
                    Registry.ClassesRoot.DeleteSubKey(".qrcf");
                    //Registry.ClassesRoot.DeleteSubKey(".qrcf");
                    // FileAssociations.Associate(".qrcf", "", "", "", "");
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                    checkicon();
                }
            }
        }

        public bool IsAdministrator()
        {
            System.Security.Principal.WindowsIdentity identity
                = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal
                = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(
                System.Security.Principal.WindowsBuiltInRole.Administrator
            );
        }

        public void checkicon()
        {
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!File.Exists(Path.Combine(directory, "XForge", "qrc.ico")))
            {
                Directory.CreateDirectory(Path.Combine(directory, "XForge"));



                // write data   
                File.WriteAllBytes(Path.Combine(directory, "XForge", "qrc.ico"), Properties.Resources.qrc);

            }
            if (!File.Exists(Path.Combine(directory, "XForge", "qrcf.ico")))
            {
                Directory.CreateDirectory(Path.Combine(directory, "XForge"));

                // write data       
                File.WriteAllBytes(Path.Combine(directory, "XForge", "qrcf.ico"), Properties.Resources.qrcf);

            }
            // Initializes a new AF_FileAssociator to associate the .ABC file extension.


            // Gets each piece of association info individually, all as strings.
            string d = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            //AF_FileAssociator assoc = new AF_FileAssociator(".qrcf");

            //string icon1 = assoc1.DefaultIcon.IconPath;
            //string icon2 = assoc2.DefaultIcon.IconPath;

            // var f = Registry.ClassesRoot.OpenSubKey(".qrc", false);
            //var de = Registry.ClassesRoot
            //var f2 = Registry.ClassesRoot.OpenSubKey(".qrcf", false);
            //var de2 = Registry.ClassesRoot;
            if (FileAssociations.IsAssociated(".qrc"))
            {
                //Registry.ClassesRoot.OpenSubKey(".qrc", false);
                qRCToolStripMenuItem.Checked = true;

            }
            else
            {
                qRCToolStripMenuItem.Checked = false;
            }

            if (FileAssociations.IsAssociated(".qrcf"))
            {


                qRCFToolStripMenuItem.Checked = true;

            }
            else
            {
                qRCFToolStripMenuItem.Checked = false;
            }
        }

        private void toolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
            Options.LoadSettings();
            if (toolStripMenuItem1.Checked == true)
            {
                toolStripMenuItem1.Text = "Splash Screen [On]";
                Show_Splash = true;
                Options.showsplash = true;
                Options.SaveSettings();
            }
            else
            {
                toolStripMenuItem1.Text = "Splash Screen [Off]";
                Show_Splash = false;
                Options.showsplash = false;
                Options.SaveSettings();
            }


        }



        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        public static void SetAssociation_User(string Extension, string OpenWith, string ExecutableName, string icon)
        {
            try
            {

                using (RegistryKey User_Classes = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\", true))
                using (RegistryKey User_Ext = User_Classes.CreateSubKey("." + Extension))
                using (RegistryKey User_AutoFile = User_Classes.CreateSubKey(Extension + "_auto_file"))
                using (RegistryKey User_AutoFile_Command = User_AutoFile.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                using (RegistryKey ApplicationAssociationToasts = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts\\", true))
                using (RegistryKey User_Classes_Applications = User_Classes.CreateSubKey("Applications"))
                using (RegistryKey User_Classes_Applications_Exe = User_Classes_Applications.CreateSubKey(ExecutableName))
                using (RegistryKey User_Application_Command = User_Classes_Applications_Exe.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                using (RegistryKey User_Explorer = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\." + Extension))
                using (RegistryKey User_Choice = User_Explorer.OpenSubKey("UserChoice"))
                {
                    User_Ext.SetValue("", Extension + "_auto_file", RegistryValueKind.String);
                    User_Classes.SetValue("", Extension + "_auto_file", RegistryValueKind.String);
                    User_Classes.CreateSubKey(Extension + "_auto_file");
                    User_AutoFile_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                    if (ApplicationAssociationToasts != null)
                    {
                        ApplicationAssociationToasts.SetValue(Extension + "_auto_file_." + Extension, 0);
                        ApplicationAssociationToasts.SetValue(@"Applications\" + ExecutableName + "_." + Extension, 0);
                    }
                    User_Application_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                    User_Explorer.CreateSubKey("OpenWithList").SetValue("a", ExecutableName);
                    User_Explorer.CreateSubKey("OpenWithProgids").SetValue(Extension + "_auto_file", "0");
                    if (User_Choice != null) User_Explorer.DeleteSubKey("UserChoice");
                    User_Explorer.CreateSubKey("UserChoice").SetValue("ProgId", @"Applications\" + ExecutableName);

                    User_Explorer.CreateSubKey("DefaultIcon").SetValue("", icon);
                }
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception excpt)
            {
                string e1 = excpt.ToString();
            }
        }
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        private void darkListView1_SelectedIndicesChanged(object sender, EventArgs e)
        {
            try
            {
                if (listView1.SelectedIndices.Count > 0)
                {
                    loop = 0;
                    listBox1.SelectedIndex = listView1.SelectedIndices[0];
                }
            }
            catch { }
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            try
            {
                var richTextBox = (RichTextBox)sender;
                richTextBox.Width = e.NewRectangle.Width;
                richTextBox.Height = e.NewRectangle.Height;
            }
            catch { }
        }

        private void pS3GaiaVisualizationHelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("PS3_Gaia_Visualization_Helper"))
            {
                string[] files = System.IO.Directory.GetFiles("PS3_Gaia_Visualization_Helper", "*.bat");
                string path = Directory.GetCurrentDirectory();
                foreach (string f in files)
                {
                    string fn = f.Substring(f.LastIndexOf('\\') + 1);

                    if (f.StartsWith("PS3_Gaia_Visualization_Helper"))
                    {
                        if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string[] nameList = new string[] { "cubemap_clouds.png", "cubemap_ground.png", "cubemap_specular.png", "cubemap_clouds.jpg", "cubemap_ground.jpg", "cubemap_specular.jpg" };
                            string[] jimages = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.jpg");
                            string[] pimages = System.IO.Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.png");
                            int l = jimages.Length + pimages.Length;
                            string[] images = new string[l];
                            Array.Copy(jimages, images, jimages.Length);
                            Array.Copy(pimages, 0, images, jimages.Length, pimages.Length);
                            foreach (string i in images)
                            {
                                string fi = i.Substring(i.LastIndexOf('\\') + 1);
                                if (nameList.Contains(fi))
                                {
                                    CreateHardLink(path + "\\PS3_Gaia_Visualization_Helper\\" + fi, folderBrowserDialog1.SelectedPath + "\\" + fi, IntPtr.Zero);
                                }
                            }

                            Process p = new Process();

                            p.StartInfo.WorkingDirectory = path + "\\PS3_Gaia_Visualization_Helper";
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.CreateNoWindow = false;
                            p.StartInfo.RedirectStandardOutput = false;
                            p.StartInfo.FileName = "cmd.exe";
                            p.StartInfo.Arguments = "/c " + fn;
                            p.StartInfo.RedirectStandardError = false;
                            p.Start();

                            break;
                        }
                    }
                }
            }

        }

        private void darkStatusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            isR = false;
            isL = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isL)
            {
                DoL();
            }
            if (isR)
            {
                DoR();
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            if (e.KeyCode == Keys.Left)
            {
                isL = true;
                DoL();
            }
            if (e.KeyCode == Keys.Right)
            {
                isR = true;
                DoR();
            }
        }

        void DoL()
        {
            if (listView1.SelectedIndices[0] > 10)
            {
                int n = listView1.SelectedIndices[0] - 10;
                this.listView1.Items[n].Focused = true;
                this.listView1.Items[n].Selected = true;
                listView1.EnsureVisible(n);
            }
            else
            {
                this.listView1.Items[0].Focused = true;
                this.listView1.Items[0].Selected = true;
                listView1.EnsureVisible(0);
            }
        }

        void DoR()
        {
            int c = listView1.Items.Count;
            int oldc = listView1.SelectedIndices[0];
            int newc = oldc + 10;
            if (newc < c)
            {
                int n = listView1.SelectedIndices[0] - 10;
                this.listView1.Items[newc].Focused = true;
                this.listView1.Items[newc].Selected = true;
                listView1.EnsureVisible(newc);
            }
            else
            {
                this.listView1.Items[c - 1].Focused = true;
                this.listView1.Items[c - 1].Selected = true;
                listView1.EnsureVisible(c - 1);
            }
        }

        void start_progress()
        {
            listView1.Visible = false;
            tableLayoutPanel1.Visible = false;
            progressBar1.Visible = true;
            darkLabel1.Visible = true;
            timer2.Enabled = true;
            timer2.Start();
        }

        void stop_progress()
        {
            listView1.Visible = true;
            tableLayoutPanel1.Visible = true;
            progressBar1.Visible = false;
            darkLabel1.Visible = false;
            timer2.Enabled = false;
            timer2.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 100)
            {
                progressBar1.Value = 0;
            }
            else
                progressBar1.Value++;
        }


        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
           
        }
        void CutAction(object sender, EventArgs e)
        {
            richTextBox11.Cut();
        }

        void CopyAction(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                System.Windows.Forms.Clipboard.SetText(richTextBox1.SelectedText);
            }
            //Clipboard.Clear();
        }

        void PasteAction(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Rtf))
            {
                richTextBox11.SelectedRtf
                    = Clipboard.GetData(DataFormats.Rtf).ToString();
            }
        }
        void CopynameAction(object sender, EventArgs e)
        {
            string t = listView1.SelectedItems[0].SubItems[0].ToString();
            t = t.Remove(0, 18);
            t = t.Remove(t.Length - 1, 1);
            if (t != "")
            {
                System.Windows.Forms.Clipboard.SetText(t);
            }
            //Clipboard.Clear();
        }
        void CopysizeAction(object sender, EventArgs e)
        {
            string t = listView1.SelectedItems[0].SubItems[1].ToString();
            t = t.Remove(0, 18);
            t = t.Remove(t.Length - 1, 1);
            if (t != "")
            {
                System.Windows.Forms.Clipboard.SetText(t);
            }
            //Clipboard.Clear();
        }
        void WordWrapAction(object sender, EventArgs e)
        {
            if (richTextBox1.WordWrap == true)
            {
                wordWrapToolStripMenuItem.Checked = false;
                richTextBox1.WordWrap = false;
            }
            else if (richTextBox1.WordWrap != true)
            {
                wordWrapToolStripMenuItem.Checked = true;
                richTextBox1.WordWrap = true;
            }
            //Clipboard.Clear();
        }

        void BackgroundColorAction(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;

            }
            //Clipboard.Clear();
        }

        void DeleteItemAction(object sender, EventArgs e)
        {
            string t = listView1.SelectedItems[0].SubItems[0].ToString();
            t = t.Remove(0, 18);
            t = t.Remove(t.Length - 1, 1);
            if (t != "")
            {
                System.Windows.Forms.Clipboard.SetText(t);
            }
            //Clipboard.Clear();
        }




        System.Drawing.Color listViewSelectionColor = System.Drawing.Color.Orange;
        private void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            if ((e.ItemIndex % 2) == 1)
            {
                e.Item.BackColor = System.Drawing.Color.FromArgb(50, 53, 55);
                e.Item.UseItemStyleForSubItems = true;
            }

        }

        protected void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private TextFormatFlags GetTextAlignment(ListView lstView, int colIndex)
        {
            TextFormatFlags flags = (lstView.View == View.Tile)
                ? (colIndex == 0) ? TextFormatFlags.Default : TextFormatFlags.Bottom
                : TextFormatFlags.VerticalCenter;

            flags |= TextFormatFlags.LeftAndRightPadding | TextFormatFlags.NoPrefix;
            switch (lstView.Columns[colIndex].TextAlign)
            {
                case HorizontalAlignment.Left:
                    flags |= TextFormatFlags.Left;
                    break;
                case HorizontalAlignment.Right:
                    flags |= TextFormatFlags.Right;
                    break;
                case HorizontalAlignment.Center:
                    flags |= TextFormatFlags.HorizontalCenter;
                    break;
            }
            return flags;
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            Set_Size();
            listView1.EndUpdate();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int si = listView1.SelectedIndices[0];
                if (Fileinfo.Rows[si] is DataRow item1)
                {
                    Object[] items2 = item1.ItemArray;
                    string n = System.IO.Path.GetDirectoryName(AppPath) + "\\ext\\temp\\" + item1[0].ToString().Substring(item1[0].ToString().LastIndexOf('/') + 1);
                    Extract_File(n, items2);
                    //
                    // Proceed with the drag-and-drop, passing the selected items for 
                    string[] filepaths = { n };
                    System.Collections.Specialized.StringCollection files = new System.Collections.Specialized.StringCollection();
                    files.Add(n);
                    var data = new DataObject(DataFormats.FileDrop, filepaths);
                    // Perform dragdrop
                    listView1.DoDragDrop(data, DragDropEffects.Copy);
                    // Delete the temp file
                    File.Delete(n);
                }
            }

        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {


                int si = listView1.SelectedIndices[0];
                ListViewItem itm = listView1.Items[si] as ListViewItem;

                string itmim = itm.ImageKey;
                //itm.ImageKey = "dds.png";
                //hexaEditor1.FileName = "";

                if (Fileinfo.Rows[si] is DataRow item1)
                //if (this.listBox1.SelectedItem is DataRowView item1)
                {

                    int dr0 = AppPath.LastIndexOf('\\');
                    string dr = AppPath.Remove(dr0, AppPath.Length - dr0) + "\\tmp\\";
                    int dr1 = item1.ItemArray[0].ToString().LastIndexOf('/');
                    dr += item1.ItemArray[0].ToString().Remove(0, dr1 + 1);
                    ;
                    if (!Directory.Exists("tmp"))
                    {
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                        Directory.CreateDirectory("tmp");
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
                    }

                    Object[] items2 = item1.ItemArray;
                    Extract_File(dr, items2);
#pragma warning disable UnhandledExceptions // Unhandled exception(s)
                    Process.Start(dr);
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lineNumbersForRichText1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void richTextBox1_TextChanging(object sender, TextChangingEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextStyle indianRed = new TextStyle(Brushes.IndianRed, null, FontStyle.Regular);
            TextStyle lightSeaGreen = new TextStyle(Brushes.LightSeaGreen, null, FontStyle.Regular);
            TextStyle darkSeaGreen = new TextStyle(Brushes.DarkSeaGreen, null, FontStyle.Italic);
            TextStyle mediumSpringGreen = new TextStyle(Brushes.MediumSpringGreen, null, FontStyle.Italic);
            TextStyle green = new TextStyle(Brushes.Green, null, FontStyle.Italic);
            TextStyle limeGreen = new TextStyle(Brushes.LimeGreen, null, FontStyle.Regular);
            TextStyle bGainsboro = new TextStyle(Brushes.Gainsboro, null, FontStyle.Bold);
            TextStyle orngeStyle = new TextStyle(Brushes.Orange, null, FontStyle.Bold);

            //string[] pattern1 = { "for", "foreach", "int", "var", "float", ":" };
            string[] pattern1 = { " -[R,H,O]\\d+", " [R,H,O]\\d[.]\\w\\w\\w", " TEX\\d+", " [R,H,O]\\d" , " [R,H,O]\\d[.]\\w", " [R,H,O]\\d[.]\\w\\w",
                " [R,H,O]\\d[.]\\w\\w\\w", " [R,H,O]\\d[.]\\w\\w\\w\\w", " -[R,H,O]\\d[.]\\w", " -[R,H,O]\\d[.]\\w\\w", " -[R,H,O]\\d[.]\\w\\w\\w",
                " -[R,H,O]\\d[.]\\w\\w\\w\\w", "RC[.]\\w", "[[]", "[]]"};
            string[] pattern2 = {"ADD ", "END\r ", "TEXR ", "NRMH ", "ADDR ", "MADR ", "DP3H ", "DP3R ",
               "MOVH ", "MADH ", "MOVR ", "MULR ", "MULH ", "ADDH ", "FENC ", "MAXR ", "MAXH ", "MOV ", "SGTRC ", "MAD ",
               "DIVSQR ", "FENCTR ", "FENCBR ", "MAX ", "MIN ", "MUL ", "MOVR\\S*", "DPH ", "RCPH ", "DIVR ", "RCPR ", "MULXC ", "SLTR ",
               "SGER ", "COS ", "RSQ ", "SLTC ", "SGT ", "RCP ", "KILR ", "SGTXC ", "MINR ", "LIFR ", "SLTRC ", "SGERC ", "MOVXC ", "FLRR ",
               "RSQR ", "SINR ", "SGERC ", "SIN ", "DP4 ", "ADDR\\S*", "MULR\\S*", "DP3 ", "MADR\\S*", "MUL\\S*",
                "DIVR_sat", "DIVR_m2", "EX2R", "DP3R_d2", "DP3R_m2", "DP3R_sat", "RETH", "SGTR", "SEQX", "DDXR", "DDYR",
                "TXDR", "DP3RC", "DP2R", "DP3H_d2", "DP3H_sat", "MADH_sat", "SLT", "FRCR_m2","", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            string[] pattern3 = { ",", ";", "", "" };

            e.ChangedRange.SetStyle(indianRed, new Regex(@"#\d+\.#\d+|#\d+.*?$",
                    RegexOptions.Multiline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(lightSeaGreen, new Regex(@"index.*?$",
                    RegexOptions.Multiline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(lightSeaGreen, new Regex(@"#default.*?$",
                    RegexOptions.Multiline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(lightSeaGreen, new Regex(@"#const.*?$",
                    RegexOptions.Multiline | RegexOptions.Compiled));



            e.ChangedRange.SetStyle(mediumSpringGreen, new Regex(@"# .*?$",
                    RegexOptions.Multiline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(green, new Regex(@"# .*?$",
                    RegexOptions.Multiline | RegexOptions.Compiled));


            e.ChangedRange.SetStyle(bGainsboro, "#MNU_1.0");
            //e.ChangedRange.SetStyle(orngeStyle, @"\d+\.\d+|\d+");

            foreach (string s in pattern1)
            {
                e.ChangedRange.SetStyle(lightSeaGreen, new Regex(s, RegexOptions.Compiled));
            }
            foreach (string s in pattern2)
            {
                e.ChangedRange.SetStyle(limeGreen, new Regex(s, RegexOptions.Compiled));
            }

            foreach (string s in pattern3)
            {
                e.ChangedRange.SetStyle(darkSeaGreen, new Regex(s, RegexOptions.Compiled));
            }

        }


        private void richTextBox1_TextChangedMNU(object sender, TextChangedEventArgs e)
        {

          /*  MNU fileype is the easyest to highlight, we just needs 3 search patterns
                -lines starting with "#" = white bold
            - from the first ":" up to the last ":"(but not including them) = green(but i prefer light blue)
        - from the second ":" up to the EOL = orange

         * and for the werid MNU files that uses "[" and "]" instead of ":" pretty much the same*/
   


               TextStyle indianRed = new TextStyle(Brushes.IndianRed, null, FontStyle.Regular);
            TextStyle lightSeaGreen = new TextStyle(Brushes.LightSeaGreen, null, FontStyle.Regular);
            TextStyle darkSeaGreen = new TextStyle(Brushes.DarkSeaGreen, null, FontStyle.Italic);
            TextStyle pink = new TextStyle(Brushes.Plum, null, FontStyle.Italic);
            TextStyle green = new TextStyle(Brushes.Green, null, FontStyle.Italic);
            TextStyle limeGreen = new TextStyle(Brushes.LimeGreen, null, FontStyle.Regular);
            TextStyle bGainsboro = new TextStyle(Brushes.Gainsboro, null, FontStyle.Bold);
            TextStyle orngeStyle = new TextStyle(Brushes.Orange, null, FontStyle.Bold);
            TextStyle blue = new TextStyle(Brushes.SkyBlue, null, FontStyle.Regular);



            // e.ChangedRange.SetStyle(indianRed, new Regex(@"#\d+\.#\d+|#\d+.*?$",
            //         RegexOptions.Multiline | RegexOptions.Compiled));
            // e.ChangedRange.SetStyle(lightSeaGreen, new Regex(@"index.*?$",
            //         RegexOptions.Multiline | RegexOptions.Compiled));
            // e.ChangedRange.SetStyle(lightSeaGreen, new Regex(@"#default.*?$",
            //         RegexOptions.Multiline | RegexOptions.Compiled));
            // e.ChangedRange.SetStyle(lightSeaGreen, new Regex(@"/\*(.|[\r\n])*?\*/",
            //         RegexOptions.Multiline | RegexOptions.Compiled));
            // e.ChangedRange.SetStyle(mediumSpringGreen, new Regex(@"# .*?$",
            //         RegexOptions.Multiline | RegexOptions.Compiled));
            // e.ChangedRange.SetStyle(darkSeaGreen, new Regex(@"/\*(.|[\r\n])*?\*/",
            //         RegexOptions.Multiline | RegexOptions.Compiled));
            // e.ChangedRange.SetStyle(green, new Regex(@":.*?$",
            //     RegexOptions.RightToLeft | RegexOptions.Compiled));

           e.ChangedRange.SetStyle(bGainsboro, "#MNU_1.0", RegexOptions.Singleline | RegexOptions.Compiled);

            // e.ChangedRange.SetStyle(orngeStyle, @"\d+\.\d+|\d+");


            e.ChangedRange.SetStyle(green,new Regex(@"""""|''|"".*?[^\\]""|'.*?[^\\]'", RegexOptions.Singleline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(orngeStyle,new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?\b", RegexOptions.Singleline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(green,new Regex(@"--.*$", RegexOptions.Multiline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(green,new Regex(@"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(green,new Regex(@"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(pink, new Regex(@":",  RegexOptions.Compiled));
            e.ChangedRange.SetStyle(indianRed, new Regex(@"-", RegexOptions.Compiled));
            e.ChangedRange.SetStyle(orngeStyle, new Regex(@"@[a-zA-Z_\d]*\b", RegexOptions.Singleline | RegexOptions.Compiled));
            e.ChangedRange.SetStyle(blue, new Regex(
                   @"\b(BIGINT|NUMERIC|BIT|SMALLINT|DECIMAL|SMALLMONEY|INT|TINYINT|MONEY|FLOAT|REAL|DATE|DATETIMEOFFSET|DATETIME2|SMALLDATETIME|DATETIME|CHAR|VARCHAR|TEXT|NCHAR|NVARCHAR|NTEXT|BINARY|VARBINARY|IMAGE|TIMESTAMP|HIERARCHYID|TABLE|UNIQUEIDENTIFIER|SQL_VARIANT|XML)\b",
                   RegexOptions.IgnoreCase | RegexOptions.Compiled));

            //e.ChangedRange.SetStyle(blue);

        }

        private void hexEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0)
            {
                int si = listView1.SelectedIndices[0];
                if (Fileinfo.Rows[si] is DataRow item1)
                {
                    Object[] items2 = item1.ItemArray;

                    Hex h1 = new Hex(items2, Filebytes, FTO);
                    h1.ShowDialog();
                }
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void deViL303sToolBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


#pragma warning restore UnhandledExceptions // Unhandled exception(s)
    }

    public class GetChInterop2
    {
        [DllImport("ext/msvcr71.dll", EntryPoint = "_kbhit")]
        public static extern int KbHit();

    }
    public class GetChInterop1
    {
        [DllImport("msvcr71.dll", EntryPoint = "_kbhit")]
        public static extern int KbHit();

    }

    public class ProgressBarEx : ProgressBar
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private SolidBrush brush = null;
#pragma warning restore IDE0044 // Add readonly modifier

        public ProgressBarEx()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // None... Helps control the flicker.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            const int inset = 2; // A single inset value to control teh sizing of the inner rect.

#pragma warning disable UnhandledExceptions // Unhandled exception(s)
            using (System.Drawing.Image offscreenImage = new Bitmap(this.Width, this.Height))
            {
                using (Graphics offscreen = Graphics.FromImage(offscreenImage))
                {
                    Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

                    if (ProgressBarRenderer.IsSupported)
                        ProgressBarRenderer.DrawHorizontalBar(offscreen, rect);

                    rect.Inflate(new Size(-inset, -inset)); // Deflate inner rect.
                    rect.Width = (int)(rect.Width * ((double)this.Value / this.Maximum));
                    if (rect.Width == 0) rect.Width = 1; // Can't draw rec with width of 0.

                    LinearGradientBrush brush = new LinearGradientBrush(rect, this.BackColor, this.ForeColor, LinearGradientMode.Vertical);
                    offscreen.FillRectangle(brush, inset, inset, rect.Width, rect.Height);

                    e.Graphics.DrawImage(offscreenImage, 0, 0);
                    offscreenImage.Dispose();
                }
            }/**/
#pragma warning restore UnhandledExceptions // Unhandled exception(s)
        }
    }


}

