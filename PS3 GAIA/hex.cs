using DarkUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XForge
{
    public partial class Hex : DarkForm
    {
        byte[] FTO;
        Object[] item1;
        byte[] Filebytes;

        public Hex(Object[] oitem, byte[] oFilebytes, byte[] oFTO)
        {
            FTO = oFTO;
            item1 = oitem;
            Filebytes = oFilebytes;

            InitializeComponent();
            loadHex();
        }

        public void loadHex()
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

        private void darkButton1_Click(object sender, EventArgs e)
        {
            //hexaEditor1.
            //Form2.Replace_File("", item1);
        }



        /*void Replace_File(string NEW_File, Object[] item)
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

        void Replace_File2(string NEW_File, Object[] item)
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
        */


    }
}
