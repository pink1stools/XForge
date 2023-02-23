using DarkUI.Forms;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace XForge
{
    public partial class Options : DarkDialog
    {
        //public static CheckBox checkBox1 = new CheckBox();
        private bool _preventMove = true;
        public DataTable Ext = new DataTable();
        public static bool showsplash;
        public static bool firstrun;
        
        public static string[][] EXTS = new string[14][] { new string[] {".elf", "" }, new string[2] { ".bin", ""  }, new string[2] { ".dump", ""  },
            new string[2] { ".fpo", ""  }, new string[2] { ".vpo", ""  }, new string[2] { ".mnu", ""  }, new string[2] { ".txt", ""  },
            new string[2] { ".ini", ""  }, new string[2] { ".path", ""  }, new string[2] { ".bmp", ""  }, new string[2] { ".jpg", ""  },
            new string[2] { ".tga", ""  }, new string[2] { ".dds", ""  }, new string[2] { ".gft", ""  } };
        public Options()
        {// 
         // checkBox1
         // 


            InitializeComponent();

            Set_Up();
        }

        void Set_Up()
        {
            LoadSettings();
            checkBox1.Checked = showsplash;
            //dataGridView1.Columns.Add(new DataGridViewImageColumn());
            Ext.Columns.Add("Icon", typeof(Image));
            Ext.Columns.Add("Original").ReadOnly = true;
            Ext.Columns.Add("Replacment").DataType = typeof(string);
            /*
            dataGridView1.RowHeadersVisible = false;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {.add
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (string[] s2 in EXTS)
            {
                DataRow dr = Ext.NewRow();
                
                dr["Original"] = s2[0];
                dr["Replacment"] = s2[1];
                Ext.Rows.Add(dr);
            }
            dataGridView1.DataSource = Ext;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            */


            darkDataGridView1.RowHeadersVisible = false;
            foreach (DataGridViewColumn column in darkDataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            int i = 0;
            foreach (string[] s2 in EXTS)
            {
                DataRow dr = Ext.NewRow();
                var bmp = new Bitmap(XForge.Properties.Resources.bin);
                string te = s2[0];
                switch (s2[0])
             {
                    case ".elf":
                        bmp = global::XForge.Properties.Resources.elf;
                        break;
                    case ".bin":
                        bmp = global::XForge.Properties.Resources.bin;
                        break;
                    case ".dump":
                        bmp = global::XForge.Properties.Resources.dump;
                        break;
                    case ".fpo":
                        bmp = global::XForge.Properties.Resources.fpo;
                        break;
                    case ".vpo":
                        bmp = global::XForge.Properties.Resources.vpo;
                        break;
                    case ".mnu":
                        bmp = global::XForge.Properties.Resources.mnu;
                        break;
                    case ".txt":
                        bmp = global::XForge.Properties.Resources.txt;
                        break;
                    case ".ini":
                        bmp = global::XForge.Properties.Resources.ini;
                        break;
                    case ".path":
                        bmp = global::XForge.Properties.Resources.path;
                        break;
                    case ".bmp":
                        bmp = global::XForge.Properties.Resources.bmp;
                        break;
                    case ".jpg":
                        bmp = global::XForge.Properties.Resources.jpg;
                        break;
                    case ".tga":
                        bmp = global::XForge.Properties.Resources.tga;
                        break;
                    case ".dds":
                        bmp = global::XForge.Properties.Resources.dds;
                        break;
                    case ".gft":
                        bmp = global::XForge.Properties.Resources.gtf;
                        break;
                }


                dr["icon"] = bmp;//imageList1.Images[i];
                dr["Original"] = s2[0];
                dr["Replacment"] = s2[1];
                Ext.Rows.Add(dr);
                i++;
            }
            darkDataGridView1.DataSource = Ext;
            
            foreach (DataGridViewColumn column in darkDataGridView1.Columns)
            {
                if(column.DisplayIndex == 0)
                {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                }
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

          //  darkDataGridView1.Columns[0].Width = 50;

        }


        #region<<settings>>
        public static void LoadSettings()
        {
            //Properties.Settings.Default.

            EXTS[0][1] = Properties.Settings.Default.bin;
            EXTS[1][1] = Properties.Settings.Default.bmp;
            EXTS[2][1] = Properties.Settings.Default.dds;
            EXTS[3][1] = Properties.Settings.Default.dump;
            EXTS[4][1] = Properties.Settings.Default.elf;
            EXTS[5][1] = Properties.Settings.Default.fpo;
            EXTS[6][1] = Properties.Settings.Default.gtf;
            EXTS[7][1] = Properties.Settings.Default.jpg;
            EXTS[8][1] = Properties.Settings.Default.mnu;
            EXTS[9][1] = Properties.Settings.Default.path;
            EXTS[10][1] = Properties.Settings.Default.tga;
            EXTS[11][1] = Properties.Settings.Default.txt;
            EXTS[12][1] = Properties.Settings.Default.vpo;
            showsplash = Properties.Settings.Default.ShowSplash;
            firstrun = Properties.Settings.Default.FirstRun;
        }

        public static void SaveSettings()
        {
            Properties.Settings.Default.bin = EXTS[0][1];
            Properties.Settings.Default.bmp = EXTS[1][1];
            Properties.Settings.Default.dds = EXTS[2][1];
            Properties.Settings.Default.dump = EXTS[3][1];
            Properties.Settings.Default.elf = EXTS[4][1];
            Properties.Settings.Default.fpo = EXTS[5][1];
            Properties.Settings.Default.gtf = EXTS[6][1];
            Properties.Settings.Default.jpg = EXTS[7][1];
            Properties.Settings.Default.mnu = EXTS[8][1];
            Properties.Settings.Default.path = EXTS[9][1];
            Properties.Settings.Default.tga = EXTS[10][1];
            Properties.Settings.Default.txt = EXTS[11][1];
            Properties.Settings.Default.vpo = EXTS[12][1];
            Properties.Settings.Default.ShowSplash = showsplash;
            Properties.Settings.Default.FirstRun = firstrun;
            Properties.Settings.Default.Save();
        }

        #endregion<<settings>>





        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            /*foreach (DataGridViewRow item1 in dataGridView1.Rows)
            {
                EXTS[i][1] = item1.Cells[1].Value.ToString();
                i++;
            }
            i = 0;*/
            foreach (DataGridViewRow item1 in darkDataGridView1.Rows)
            {
                EXTS[i][1] = item1.Cells[2].Value.ToString();
                i++;
            }
            showsplash = checkBox1.Checked;
            SaveSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;
            /* foreach (DataGridViewRow item1 in dataGridView1.Rows)
             {
                 item1.Cells[1].Value = null;
                 EXTS[i][1] = null;
                 i++;
             }
             i = 0;*/
            foreach (DataGridViewRow item1 in darkDataGridView1.Rows)
            {
                item1.Cells[2].Value = null;
                EXTS[i][1] = null;
                i++;
            }
            SaveSettings();
        }

        private void darkDataGridView1_Load(object sender, EventArgs e)
        {

        }
    }
}
