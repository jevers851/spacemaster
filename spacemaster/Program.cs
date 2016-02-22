using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacemaster
{
    class Program
    {
        public const int VNAME_SIZE = 8;
        public const int spaceToFiles = 2500;
        private FileStream m_file;

        public static Boolean Allocate(String VolumeName, long Size)
        {
            Boolean allocated = true;
            int totalFiles = 0;

            FileStream writer = new FileStream(VolumeName, FileMode.Create);

            byte[] bytesData = new byte[500];

            if ((System.Text.ASCIIEncoding.ASCII.GetByteCount(VolumeName) < VNAME_SIZE))
            {


                bytesData.Initialize();

                //WRITE FILE NAME TO START OF DISK

                bytesData = Encoding.ASCII.GetBytes(VolumeName);

                if (bytesData.Length <= 8)
                {
                    writer.Write(bytesData, 0, bytesData.Length);
                    writer.Seek((8 - (bytesData.Length)), SeekOrigin.Current);
                }

                //WRITE SIZE OF FILE TO START OF DISK


                bytesData = Encoding.ASCII.GetBytes((Size.ToString()));

                if (bytesData.Length <= 8)
                {
                    writer.Write(bytesData, 0, bytesData.Length);
                    writer.Seek((8 - (bytesData.Length)), SeekOrigin.Current);
                }

                //WRITE TOTAL FILES ON DISK


                bytesData = Encoding.ASCII.GetBytes(totalFiles.ToString());
                writer.Write(bytesData, 0, bytesData.Length);


                for (int i = 21; i < Size; i++)
                {
                    writer.WriteByte((byte)'\0');
                }

            }
            else
            {
                Console.WriteLine("Volume name to long");
            }


            //BinaryReader reader = new BinaryReader(File.Open(VolumeName, FileMode.Open));

            //Console.WriteLine(reader.ReadString());

            //reader.Close();

            writer.Close();

            return allocated;
        }
        public static Boolean Deallocate(String VolumeName)
        {
            Boolean deallocated = true;

            if (File.Exists(VolumeName))
                File.Delete(VolumeName);

            return deallocated;
        }
        public static Boolean Truncate(String VolumeName)
        {
            Boolean truncated = true;

            if (File.Exists(VolumeName))
            {
                FileInfo fInfo = new FileInfo(VolumeName);
                FileStream file = new FileStream(VolumeName, FileMode.Open);

                for (long i = 21; i < fInfo.Length; i++)
                {
                    file.WriteByte((byte)'\0');

                    file.Seek(-10, SeekOrigin.Current);

                }

                file.Close();
            }
            else
                truncated = false;

            return truncated;
        }
        public static void Dump(String VolumeName)
        {
            if (File.Exists(VolumeName))
            {
                FileStream file = new FileStream(VolumeName, FileMode.Open);

                for (int i = 0; i < file.Length; i++)
                {
                    Console.Write("{0}", file.ReadByte());
                }

                file.Close();
            }
        }
        public Boolean Mount(String VolumeName)
        {
            Boolean mounted = true;

            if (File.Exists(VolumeName))
            {
                m_file = new FileStream(VolumeName, FileMode.Open);
                Console.WriteLine("Mounted");
            }
            else
            {
                mounted = false;
                Console.WriteLine("File not Mounted");
            }

            return mounted;
        }
        public void UnMount()
        {
            if (m_file != null)
                m_file.Close();
        }
        public void Vinfo()
        {

            byte[] bytesData = new byte[500];
            bytesData.Initialize();
            if (m_file != null)
            {

                m_file.Seek(0, SeekOrigin.Begin);
                m_file.Read(bytesData, 0, 8);
                Console.Write("Volume Name: ");
                for (int i = 0; i < 8; i++)
                    Console.Write((char)bytesData[i]);
                Console.WriteLine("");

                m_file.Read(bytesData, 0, 8);
                Console.Write("Total Size of Volume:");
                for (int i = 0; i < 8; i++)
                    Console.Write((char)bytesData[i]);
                Console.WriteLine("");


                m_file.Read(bytesData, 0, 4);
                Console.Write("Total Files: ");
                for (int i = 0; i < 4; i++)
                    Console.Write((char)bytesData[i]);
            }


        }
        public void FileInfo(string FileName)
        {
            byte[] bytesData = new byte[500];
            bytesData.Initialize();
            if (m_file != null)
            {
                if(search(FileName))
                {
                    m_file.Seek(21, SeekOrigin.Begin);

                    m_file.Read(bytesData, 0, 16);
                    int fCount = totalFiles();
                    while (fCount != 0)
                    {
                        String name = "";
                        for (int i = 0; i < 16; i++)
                        {
                            
                            name += (char)bytesData[i];
                            if (name.Equals(FileName))
                            {
                                Console.WriteLine(name);

                                m_file.Read(bytesData, 0, 4);
                                Console.Write("Total Size of File: ");
                                for (int g = 0; g < 4; g++)
                                    Console.Write((char)bytesData[g]);
                                Console.WriteLine("");

                                m_file.Read(bytesData, 0, 1);
                                Console.Write("Read Only Value: ");
                                for (int g = 0; g < 1; g++)
                                    Console.Write((char)bytesData[g]);
                                Console.WriteLine("");

                                m_file.Read(bytesData, 0, 8);
                                Console.Write("Date Created: ");
                                for (int g = 0; g < 8; g++)
                                    Console.Write((char)bytesData[g]);
                                Console.WriteLine("");

                                m_file.Read(bytesData, 0, 4);
                                Console.Write("TimeCreated: ");
                                for (int g = 0; g < 4; g++)
                                    Console.Write((char)bytesData[g]);
                                Console.WriteLine("");

                                m_file.Read(bytesData, 0, 8);
                                Console.Write("Date Last Modified: ");
                                for (int g = 0; g < 8; g++)
                                    Console.Write((char)bytesData[g]);
                                Console.WriteLine("");

                                m_file.Read(bytesData, 0, 4);
                                Console.Write("Time Last Modified: ");
                                for (int g = 0; g < 4; g++)
                                    Console.Write((char)bytesData[g]);
                                Console.WriteLine("");
                            }


                        }
                        m_file.Seek(30, SeekOrigin.Current);
                        m_file.Read(bytesData, 0, 16);
                        fCount -= 1;
                    }
                }
                
            }


        }
        public void createFile(string FileName)
        {
            byte[] bytesData = new byte[500];
            int count = 0;
            if (m_file != null)
            {
                if(!search(FileName))
                { 
                count = totalFiles();
                count = count * 45;
                count += 21;

                m_file.Seek(count, SeekOrigin.Begin);
        //Write FileName to Disk
                bytesData = Encoding.ASCII.GetBytes(FileName);

                if (bytesData.Length <= 16)
                {
                    m_file.Write(bytesData, 0, bytesData.Length);
                    for (int i = 0; i < (16 - bytesData.Length); i++)
                    {
                        m_file.WriteByte((byte)'\0');
                    }
                }
        //Write Size to Disk
                for (int i = 0; i < 4; i++)
                {
                    m_file.WriteByte((byte)'0');
                }
        //Write ReadOnly Bool to Disk
                for (int i = 0; i < 1; i++)
                {
                    m_file.WriteByte((byte)'0');
                }
        //Write DateCreated to Disk
                DateTime dt = DateTime.Now;
            //Day**********************
                bytesData = Encoding.ASCII.GetBytes(dt.Day.ToString());
                if (bytesData.Length < 2)
                {
                    m_file.WriteByte((byte)'0');
                }
                    m_file.Write(bytesData, 0, bytesData.Length);

            //Month*********************
                bytesData = Encoding.ASCII.GetBytes(dt.Month.ToString());
                if (bytesData.Length < 2)
                {
                    m_file.WriteByte((byte)'0');
                }
                    m_file.Write(bytesData, 0, bytesData.Length);


            //Year*********************
                bytesData = Encoding.ASCII.GetBytes(dt.Year.ToString());

                    m_file.Write(bytesData, 0, bytesData.Length);

        //Write TimeCreate to Disk

            //Hour*********************
                bytesData = Encoding.ASCII.GetBytes(dt.Hour.ToString());
                if (bytesData.Length < 2)
                {
                    m_file.WriteByte((byte)'0');
                }
                m_file.Write(bytesData, 0, bytesData.Length);

            //Minute*******************
                bytesData = Encoding.ASCII.GetBytes(dt.Minute.ToString());
                if (bytesData.Length < 2)
                {
                    m_file.WriteByte((byte)'0');
                }
                m_file.Write(bytesData, 0, bytesData.Length);


                for (int i = 0; i < 12; i++)
                {
                    m_file.WriteByte((byte)'\0');
                }

                int files = totalFiles() + 1;

                bytesData = Encoding.ASCII.GetBytes(files.ToString());
                m_file.Seek(16, SeekOrigin.Begin);
                m_file.Write(bytesData, 0, bytesData.Length);


                }


            }
        }
        public long volSize()
        {
            byte[] bytesData = new byte[500];
            String result = "";
            long end;
            if (m_file != null)
            {
                m_file.Seek(8, SeekOrigin.Begin);
                m_file.Read(bytesData, 0, 8);
                for (int i = 0; i < 8; i++)
                    result += (char)bytesData[i];
                end = Convert.ToInt64(result);
                return end;

            }
            return -1;
        }
        public int totalFiles()
        {
            byte[] bytesData = new byte[500];
            String result = "";
            int end;
            if (m_file != null)
            {
                m_file.Seek(16, SeekOrigin.Begin);
                m_file.Read(bytesData, 0, 8);
                for (int i = 0; i < 4; i++)
                    result += (char)bytesData[i];
                end = Convert.ToInt32(result);
                return end;

            }
            return -1;
        }
        public bool search(string FileName)
        {

            byte[] bytesData = new byte[500];
            if (m_file != null)
            {
                m_file.Seek(21, SeekOrigin.Begin);

                m_file.Read(bytesData, 0, 16);
                int fCount = totalFiles();
                while(fCount != 0)
                {
                    String name = "";
                    for (int i = 0; i < 16; i++)
                    {
                        
                        name += (char)bytesData[i];
                        if (name.Equals(FileName))
                        {
                            return true;
                        }


                    }
                    m_file.Seek(30, SeekOrigin.Current);
                    m_file.Read(bytesData, 0, 16);
                    fCount -= 1;
                }
                return false;


            }
            Console.WriteLine("No volume mounted");
            return false;

        }
        public int fileSize(string FileName)
        {

            byte[] bytesData = new byte[500];
            if (m_file != null)
            {
                m_file.Seek(21, SeekOrigin.Begin);

                m_file.Read(bytesData, 0, 16);
                int fCount = totalFiles();


                while (fCount != 0)
                {
                    String name = "";
                    for (int i = 0; i < 16; i++)
                    {

                        name += (char)bytesData[i];

                        if (name.Equals(FileName))
                        {
                            if (bytesData[0] != '\0')
                            {
                                name = "";
                                for (int g = 0; g < 4; g++)
                                {
                                    m_file.Seek(16, SeekOrigin.Current);
                                    m_file.Read(bytesData, 0, 4);

                                    name += (char)bytesData[g];
                                    
                                }
                                return Convert.ToInt32(name);
                            }
                            else
                            {
                                return 0;
                            }
                        }

                    }
                    


                    m_file.Seek(30, SeekOrigin.Current);
                    m_file.Read(bytesData, 0, 16);
                    fCount -= 1;
                }
                }
                Console.WriteLine("No file found");
                return -1;
            }
        public int fStart(string FileName)
        {
            byte[] bytesData = new byte[500];
            int count = totalFiles();
            string name = "";
            string sname = "";
            int start = spaceToFiles;

            if(search(FileName))
            {
                m_file.Seek(21, SeekOrigin.Begin);
                m_file.Read(bytesData, 0, 16);
                for (int i = 0; i < 16; i++)
                {
                    sname += (char)bytesData[i];
                }
                while(!name.Equals(FileName))
                { 
                if (bytesData[0] != '\0')
                {
                    m_file.Read(bytesData, 0, 4);
                    for (int i = 0; i < 4; i++)
                    {
                        name += (char)bytesData[i];
                        
                    }
                    start += Convert.ToInt32(name);
                    m_file.Seek(37, SeekOrigin.Current);
                    m_file.Read(bytesData, 0, 16);
                    sname = "";
                    for (int i = 0; i < 16; i++)
                    {
                        sname += (char)bytesData[i];
                    }
                }
                else
                {
                    return start;
                }
                }
            }

            return 0;

        }
        public long spaceRemain()
        {

            byte[] bytesData = new byte[500];
            if (m_file != null)
            {
                m_file.Seek(37, SeekOrigin.Begin);
                long space = 0;
                m_file.Read(bytesData, 0, 4);
                int fCount = totalFiles();
                while (fCount != 0)
                {
                    String name = "";
                    for (int i = 0; i < 4; i++)
                    {
                        name += (char)bytesData[i];
                    }
                    fCount--;
                    m_file.Seek(41, SeekOrigin.Current);
                    m_file.Read(bytesData, 0, 4);
                    space += Convert.ToInt64(name);
                    name = "";
                }
                return space;
            }
            Console.WriteLine("No volume mounted");
            return 0;
        }
        public void fileWrite(string FileName, string data)
        {
            byte[] bytesData = new byte[500];

            if (m_file != null)
            {
                if (search(FileName))
                {
                    if (fileSize(FileName) == 0)
                    {
                        if (spaceRemain() - data.Length < 0)
                        {

                            int right = filesRight(FileName);

                            long start = fStart(FileName);
                            if (right == 0)
                            {


                     //File WRITTEN***************************


                                m_file.Seek(start, SeekOrigin.Begin);
                                bytesData = Encoding.ASCII.GetBytes(data);
                                m_file.Write(bytesData, 0, bytesData.Length);





                    //UPDATING FILE INFO**************************

                                byte[] byteData = new byte[500];
                                m_file.Seek(21, SeekOrigin.Begin);

                                m_file.Read(byteData, 0, 16);
                                int fCount = totalFiles();
                                while (fCount != 0)
                                {
                                    String name = "";
                                    for (int i = 0; i < 16; i++)
                                    {

                                        name += (char)byteData[i];
                                        if (name.Equals(FileName))
                                        {
                                            
                                            //Write DateMod to Disk
                                            DateTime dt = DateTime.Now;
                                            //Day**********************
                                            bytesData = Encoding.ASCII.GetBytes(dt.Day.ToString());
                                            if (bytesData.Length < 2)
                                            {
                                                m_file.WriteByte((byte)'0');
                                            }
                                            m_file.Write(bytesData, 0, bytesData.Length);
                                            //Month*********************
                                            bytesData = Encoding.ASCII.GetBytes(dt.Month.ToString());
                                            if (bytesData.Length < 2)
                                            {
                                                m_file.WriteByte((byte)'0');
                                            }
                                            m_file.Write(bytesData, 0, bytesData.Length);


                                            //Year*********************
                                            bytesData = Encoding.ASCII.GetBytes(dt.Year.ToString());

                                            m_file.Write(bytesData, 0, bytesData.Length);

                                            //Write TimeMod to Disk

                                            //Hour*********************
                                            bytesData = Encoding.ASCII.GetBytes(dt.Hour.ToString());
                                            if (bytesData.Length < 2)
                                            {
                                                m_file.WriteByte((byte)'0');
                                            }
                                            m_file.Write(bytesData, 0, bytesData.Length);

                                            //Minute*******************
                                            bytesData = Encoding.ASCII.GetBytes(dt.Minute.ToString());
                                            if (bytesData.Length < 2)
                                            {
                                                m_file.WriteByte((byte)'0');
                                            }
                                            m_file.Write(bytesData, 0, bytesData.Length);

                                        }


                                    }
                                    m_file.Seek(30, SeekOrigin.Current);
                                    m_file.Read(byteData, 0, 16);
                                    fCount -= 1;
                                }

                            }
                            else
                            {
                                filemigrate(FileName, 1);
                                bytesData = Encoding.ASCII.GetBytes(data);

                                m_file.Seek(start, SeekOrigin.Begin);
                                m_file.Write(bytesData, 0, bytesData.Length);



                        //Write DateMod to Disk
                                DateTime dt = DateTime.Now;
                            //Day**********************
                                bytesData = Encoding.ASCII.GetBytes(dt.Day.ToString());
                                if (bytesData.Length < 2)
                                {
                                    m_file.WriteByte((byte)'0');
                                }
                                m_file.Write(bytesData, 0, bytesData.Length);
                            //Month*********************
                                bytesData = Encoding.ASCII.GetBytes(dt.Month.ToString());
                                if (bytesData.Length < 2)
                                {
                                    m_file.WriteByte((byte)'0');
                                }
                                m_file.Write(bytesData, 0, bytesData.Length);


                            //Year*********************
                                bytesData = Encoding.ASCII.GetBytes(dt.Year.ToString());

                                m_file.Write(bytesData, 0, bytesData.Length);

                        //Write TimeMod to Disk

                            //Hour*********************
                                bytesData = Encoding.ASCII.GetBytes(dt.Hour.ToString());
                                if (bytesData.Length < 2)
                                {
                                    m_file.WriteByte((byte)'0');
                                }
                                m_file.Write(bytesData, 0, bytesData.Length);

                            //Minute*******************
                                bytesData = Encoding.ASCII.GetBytes(dt.Minute.ToString());
                                if (bytesData.Length < 2)
                                {
                                    m_file.WriteByte((byte)'0');
                                }
                                m_file.Write(bytesData, 0, bytesData.Length);

                                filemigrate(FileName, 0);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Space for file");


                        }
                        
                    }


                }
                else
                {
                    Console.WriteLine("File Not Found");
                }

            }
        }
        public void fileDelete(string FileName)
        {


            byte[] bytesData = new byte[500];

            filemigrate(FileName, 2);

            int start = fileLoc(FileName);
            m_file.Seek(start, SeekOrigin.Begin);

            long offset = spaceToFiles;

            start = -45;
            offset = offset - start;
            int get = 45;
            while (offset != 0)
            {
                m_file.Seek(get, SeekOrigin.Current);
                m_file.Read(bytesData, 0, 1);

                m_file.Seek((-start), SeekOrigin.Current);
                m_file.Write(bytesData, 0, 1);
                get++;
                offset--;
            }

        }
        public int fileLoc(string FileName)
        {
            byte[] bytesData = new byte[500];
            if (m_file != null)
            {
                m_file.Seek(21, SeekOrigin.Begin);

                m_file.Read(bytesData, 0, 16);
                int fCount = totalFiles();
                int offsetCount = 21;
                while(fCount != 0)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        String name = "";
                        name += (char)bytesData[i];
                        if (name.Equals(FileName))
                        {
                            return offsetCount;
                        }
                       

                    }
                    m_file.Seek(30, SeekOrigin.Current);
                    m_file.Read(bytesData, 0, 16);
                    fCount -= 1;
                    offsetCount += 45;
                }
                return 0;


            }
            Console.WriteLine("No volume mounted");
            return -1;

        }
        public int filesRight(string FileName)
        {
            byte[] bytesData = new byte[500];

            int filesr = 0;

            if (m_file != null)
            {
                m_file.Seek(21, SeekOrigin.Begin);

                m_file.Read(bytesData, 0, 16);
                int fCount = totalFiles();
                while(fCount != 0)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        String name = "";
                        name += (char)bytesData[i];
                        if (name.Equals(FileName))
                        {
                            int end = 0;
                            while(end != 1)
                            {
                            m_file.Seek(30, SeekOrigin.Current);
                            m_file.Read(bytesData, 0, 16);

                                for (int j = 0; j < 16; j++)
                                {
                                    name += (char)bytesData[i];
                                }
                                if(name.Equals("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0"))
                                {
                                    return 0;
                                }
                                else
                                {
                                    filesr++;
                                }
                            }
                         }


                    }
                    m_file.Seek(30, SeekOrigin.Current);
                    m_file.Read(bytesData, 0, 16);
                    fCount -= 1;
                }
                return 0;


            }
            Console.WriteLine("No volume mounted");
            return -1;

        }
        public void filemigrate(string FileName, int end)
        {
            int right = filesRight(FileName);
            byte[] bytesData = new byte[500];
            int total = totalFiles();
            string sname = "";
            if(end == 1)
            {
                int read = (total * 45) + 21;
                int fromEnd = 0;
                while(right != 0)
                { 
                m_file.Seek(read, SeekOrigin.Begin);
                m_file.Read(bytesData, 0, 16);
                for (int i = 0; i < 16; i++)
                {
                    sname += (char)bytesData[i];
                }
                if(search(sname))
                { 
                int start = fStart(sname);


                m_file.Seek(start, SeekOrigin.Begin);
                m_file.Read(bytesData, 0, fileSize(sname));
                fromEnd -= fileSize(sname);

                m_file.Seek(fromEnd, SeekOrigin.End);
                m_file.Write(bytesData, 0, bytesData.Length);
                }
                right--;
                read -= 45;
                }
                Console.WriteLine("Files moved to end");
            }
            if(end == 0)
            {
                long offset = spaceRemain();
                long start = fStart(FileName);

                offset = start - offset;
                start = start + fileSize(FileName);

                while(offset != 0)
                {
                    m_file.Seek(offset, SeekOrigin.End);
                    m_file.Read(bytesData, 0, 1);

                    m_file.Seek(start, SeekOrigin.Begin);
                    m_file.Write(bytesData, 0, 1);
                    start++;
                    offset++;
                }

            }
            if(end == 2)
            {
                long offset = spaceRemain();
                long start = fStart(FileName);

                long read = start + fileSize(FileName);
                offset = offset - read;
                while (offset != 0)
                {
                    m_file.Seek(read, SeekOrigin.Begin);
                    m_file.Read(bytesData, 0, 1);

                    m_file.Seek(start, SeekOrigin.Begin);
                    m_file.Write(bytesData, 0, 1);
                    read++;
                    start++;
                    offset++;
                }



            }
        }
        public void fileRead(string FileName)
        {
            if (m_file != null)
            {
                if (search(FileName))
                {
                    int start = fStart(FileName);
                    int size = fileSize(FileName);

                    byte[] bytesData = new byte[500];

                    m_file.Seek(start, SeekOrigin.Begin);
                    m_file.Read(bytesData, 0, size);

                    Console.Write("Data of: " + FileName);
                    for (int g = 0; g < size; g++)
                        Console.Write((char)bytesData[g]);
                    Console.WriteLine("");
                }
            }


        }
    }
}