using DarkUI.Forms;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace XForge
{

    public partial class MD5s : DarkDialog
    {
        Boolean isR = false;
        Boolean isL = false;

        private bool _preventMove = true;
        DataTable table = new DataTable();
        public DataTable qrcinfo = new DataTable();
        byte[] Filebytes;
        string name = "";
        string md5 = "";
        public MD5s(DataTable QRCinfo, byte[] bytes, string fn, string fm)
        {
            Filebytes = bytes;
            qrcinfo = QRCinfo;
            name = fn;
            md5 = fm;
            InitializeComponent();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 500;
            // dataGridView1.DataSource = table;

            loadlist();
            ContextMenu lcontextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem lmenuItem = new MenuItem("Copy Name");
            lmenuItem.Click += new EventHandler(CopynameAction);
            lcontextMenu.MenuItems.Add(lmenuItem);
            lmenuItem = new MenuItem("Copy MD5");
            lmenuItem.Click += new EventHandler(CopysizeAction);
            lcontextMenu.MenuItems.Add(lmenuItem);
            lmenuItem = new MenuItem("Save MD5 List");
            lmenuItem.Click += new EventHandler(listAction);
            lcontextMenu.MenuItems.Add(lmenuItem);
            //menuItem = new MenuItem("Paste");
            //menuItem.Click += new EventHandler(PasteAction);
            //contextMenu.MenuItems.Add(menuItem);

            listView1.ContextMenu = lcontextMenu;
        }


        public void loadlist()
        {

            //add in tables
            table.Columns.Add("File Name", typeof(string));
            table.Columns.Add("MD5", typeof(string));
            listView1.Clear();
            listView1.Columns.Add("", 307);
            listView1.Columns.Add("", 230);
            //add in rows
            foreach (DataRow r in qrcinfo.Rows)
            {
                Object[] item = r.ItemArray;
                byte[] OR = item[12] as byte[];
                byte[] ORsize = item[13] as byte[];
                string a3 = item[20].ToString();
                string name = item[0].ToString();
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(OR);
                    Array.Reverse(ORsize);
                }

                int fto = BitConverter.ToInt32(Form2.FTO, 0);
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
                string md5 = Form2.Get_MD5(F1);

                string[] teco2 = new string[2];
                teco2[0] = name;
                teco2[1] = md5;

                DataRow dr = table.NewRow();
                dr["File Name"] = name;
                dr["MD5"] = md5;
                table.Rows.Add(dr);

                var listViewItemt2 = new ListViewItem(teco2);
                //listView1.Items.Add(new ListViewItem(""));
                listView1.Items.Add(listViewItemt2);
                //ListViewItem items = new ListViewItem(name);
                //items.SubItems.Add(md5);

                //listView1.Items.Add(items);
                //darkListView1.Items.Add(name, md5);
            }

            listView1.Columns[0].AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
            /**/
            listView1.Columns[1].AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);

            if (listView1.Items.Count > 0 && this.listView1.Items[0].Bounds.Height != 0 && listView1.ClientSize.Height != 0)
            {

                if (this.listView1.ClientSize.Width > this.listView1.Items[0].Bounds.Width)
                {
                    listView1.BeginUpdate();

                    int t = this.listView1.ClientSize.Width - this.listView1.Items[0].Bounds.Width;
                    //listView1.Columns[0].Width += t;
                    this.Width = this.Width - t;
                    listView1.EndUpdate();
                    // listView1.Width = listView1.Width - t;
                }

                else if (this.listView1.ClientSize.Width < this.listView1.Items[0].Bounds.Width + 21)
                {

                    int t = this.listView1.Items[0].Bounds.Width - this.listView1.ClientSize.Width;

                    listView1.Width = listView1.Width + t;

                }

            }



            listView1.Items[0].Focused = true;
            listView1.Items[0].Selected = true;
            listView1.Select();
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

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Stop();
            isR = false;
            isL = false;
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
                //int n = listView1.SelectedIndices[0] - 10;
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

        void listAction(object sender, EventArgs e)
        {
            using (System.IO.StreamWriter nfile =

               new System.IO.StreamWriter("ext/" + Path.GetFileNameWithoutExtension(name) + "_md5.txt", false))
            {
                nfile.WriteLine(Path.GetFileName(name) + ", " + md5);
                foreach (ListViewItem I in listView1.Items)
                {
                    string n = I.SubItems[0].ToString();
                    n = n.Remove(0, 18);
                    n = n.Remove(n.Length - 1, 1);

                    string m = I.SubItems[1].ToString();
                    m = m.Remove(0, 18);
                    m = m.Remove(m.Length - 1, 1);
                    if (n != "" && m != "")
                    {
                        nfile.WriteLine(n + ", " + m);
                    }

                }

            }
            //Clipboard.Clear();
        }

        private void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {

            e.DrawDefault = true;
            if ((e.ItemIndex % 2) == 1)
            {
                e.Item.BackColor = System.Drawing.Color.FromArgb(50, 53, 55);
                e.Item.UseItemStyleForSubItems = true;
            }
        }

        private void listView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;

        }

    }
}
