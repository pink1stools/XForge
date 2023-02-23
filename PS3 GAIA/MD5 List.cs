using DarkUI.Forms;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace XForge
{
    public partial class MD5_List : DarkForm
    {
        DataTable table = new DataTable();
        public DataTable qrcinfo = new DataTable();
        byte[] Filebytes;
        public MD5_List(DataTable QRCinfo, byte[] bytes)
        {
            Filebytes = bytes;
            qrcinfo = QRCinfo;
            InitializeComponent();
            dataGridView1.DataSource = table;
            loadlist();
        }

        public void loadlist()
        {
            
            //add in tables
            table.Columns.Add("File Name", typeof(string));
            table.Columns.Add("MD5", typeof(string));
            
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
                DataRow dr = table.NewRow();
                dr["File Name"] = name;
                dr["MD5"] = md5;
                table.Rows.Add(dr);
            }
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }



    }
}
