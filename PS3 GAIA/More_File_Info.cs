using DarkUI.Forms;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace XForge
{
    public partial class More_File_Info : DarkDialog
    {

        Object[] Fileitems;
        DataRow Datarow;
        public More_File_Info(DataRow dataRow)
        {
            Datarow = dataRow;
            Fileitems = dataRow.ItemArray;
            InitializeComponent();
        }

        private void More_File_Info_Load(object sender, EventArgs e)
        {
            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            MenuItem menuItem = new MenuItem("Copy");
            menuItem.Click += new EventHandler(CopyAction);
            contextMenu.MenuItems.Add(menuItem);
            listView1.ContextMenu = contextMenu;

            listView1.Clear();
            listView1.Columns.Add("", 160);
            listView1.Columns.Add("", 110);
            int i = 1;
            while (i < Fileitems.Length)
            {
                string[] teco2 = new string[2];
                teco2[0] = Datarow.Table.Columns[i].ToString().Replace('_', ' ');
                teco2[1] = Fileitems[i].ToString();
                if (teco2[1] == "System.Byte[]")
                {
                    teco2[1] = BitConverter.ToString(Fileitems[i] as byte[]);

                }
                if (teco2[0] == "Attribute3 Name Offset")
                {
                    if (Fileitems[i].ToString() == "00-00-00-00" && Fileitems[i].ToString() == "00-00-00-00" && Fileitems[i].ToString() == "00-00-00-00")
                    {
                        break;
                    }

                }
                if (teco2[0] != "Row")
                {
                    var listViewItemt2 = new ListViewItem(teco2);
                    //listView1.Items.Add(new ListViewItem(""));
                    listView1.Items.Add(listViewItemt2);
                }

                i++;
            }
            //listView1.Columns[0].AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.Columns[1].AutoResize(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);

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

                    //int nw = w - oldtw;
                    //this.tableLayoutPanel1.Location = new System.Drawing.Point(215, 37);

                    //this.Width = oldfw;// this.Width - nw;
                    //this.Size = RestoreBounds.Size;//oldfw;
                    // this.tableLayoutPanel1.Size = oldtw;// new System.Drawing.Size(oldtw, 438);



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

        void CopyAction(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();

            int ni = 1;
            foreach (ListViewItem i in listView1.SelectedItems)
            {

                builder.Append(i.SubItems[0].Text).Append(" ");
                if (ni == listView1.SelectedItems.Count)
                {
                    builder.Append(i.SubItems[1].Text);
                }
                else
                {
                    builder.Append(i.SubItems[1].Text).Append(Environment.NewLine);
                }
                ni++;

            }
            System.Windows.Forms.Clipboard.SetText(builder.ToString());

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

        private void listView1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void listView1_ClientSizeChanged(object sender, EventArgs e)
        {

        }
    }
}
