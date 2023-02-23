using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace XForge
{
    public class QRC
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

}
