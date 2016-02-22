using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spacemaster
{
    class fsusage
    {
        public const int COMMAND_NAME = 0;
        // ALLOCATION
        public const int CMD_ALLOC_VOLNAME = 1;
        public const int CMD_ALLOC_SIZE = 2;
        // DEALLOCATION
        public const int CMD_DEALLOC_VOLNAME = 1;
        // TRUNCATE
        public const int CMD_TRUNC_VOLNAME = 1;
        // DUMP
        public const int CMD_DUMP_VOLNAME = 1;
        // MOUNT
        public const int CMD_MOUNT_VOLNAME = 1;
        // UNMOUNT
        // NO ARGUMENTS FOR UNMOUNT

        static void Main(string[] args)
        {
            String Command = String.Empty;
            Program program;
            int exit = 0;
            String input = " ";
            program = new Program();

            input = Console.ReadLine();
            //Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetByteCount(args[0]));
            while (exit != 1)
            {
                

               /* if (args.Length == 0)
                {
                    Console.WriteLine("Invalid arguments");
                    return;
                }*/
                string[] inputCom = input.Split(' ');
                

                //
                // ALLOCATE <VOLUMENAME> <SIZE>
                //
                if (inputCom[0].Equals("ALLOCATE"))
                {
                  /*  if (args.Length != 3)
                    {
                        Console.WriteLine("Invalid number of arguments for the ALLOCATE command.");
                        return;
                    }*/

                    String VolumeName = inputCom[1];
                    String Size = inputCom[2];

                    Program.Allocate(VolumeName, int.Parse(Size));
                }
                else if (inputCom[0].Equals("DEALLOCATE"))
                {
                   /* if (args.Length != 2)
                    {
                        Console.WriteLine("Invalid number of arguments for the DEALLOCATE command.");
                        return;
                    }*/

                    String VolumeName = args[CMD_DEALLOC_VOLNAME];
                    Program.Deallocate(VolumeName);
                }
                else if (inputCom[0].Equals("TRUNCATE"))
                {
                   /* if (args.Length != 2)
                    {
                        Console.WriteLine("Invalid number of arguments for the TRUNCATE command.");
                        return;
                    }*/

                    String VolumeName = args[CMD_TRUNC_VOLNAME];
                    Program.Truncate(VolumeName);
                }
                else if (inputCom[0].Equals("DUMP"))
                {
                  /*  if (args.Length != 2)
                    {
                        Console.WriteLine("Invalid number of arguments for the DUMP command.");
                        return;
                    }*/

                    String VolumeName = args[CMD_DUMP_VOLNAME];
                    Program.Dump(VolumeName);
                }
                else if (inputCom[0].Equals("MOUNT"))
                {

                    
                    
                    if (inputCom.Length != 2)
                    {
                        Console.WriteLine("Invalid number of arguments for the MOUNT command.");
                        return;
                    }
                    
                    String VolumeName = inputCom[1];
                    
                    program.Mount(VolumeName);
                }
                else if (inputCom[0].Equals("UNMOUNT"))
                {
                   /* if (args.Length != 1)
                    {
                        Console.WriteLine("Invalid number of arguments for the UNMOUNT command.");
                        return;
                    }*/
                    program.UnMount();
                }
                else if (inputCom[0].Equals("VINFO"))
                {
                    program.Vinfo();
                }
                else if (inputCom[0].Equals("CREATE"))
                {
                    if(inputCom.Length != 2)
                    {
                        Console.WriteLine("Invalid arguments");
                        return;
                    }
                    program.createFile(inputCom[1]);
                }
                else if (inputCom[0].Equals("WRITE"))
                {
                    if (inputCom.Length != 3)
                    {
                        Console.WriteLine("Invalid arguments");
                        return;
                    }
                    program.fileWrite(inputCom[1], inputCom[2]);
                }
                else if (inputCom[0].Equals("DELETE"))
                {
                    if (inputCom.Length != 2)
                    {
                        Console.WriteLine("Invalid arguments");
                        return;
                    }
                    program.fileDelete(inputCom[1]);
                }
                else if (inputCom[0].Equals("FINFO"))
                {
                    if (inputCom.Length != 2)
                    {
                        Console.WriteLine("Invalid arguments");
                        return;
                    }
                    program.FileInfo(inputCom[1]);
                }
                else if (inputCom[0].Equals("READ"))
                {
                    if (inputCom.Length != 2)
                    {
                        Console.WriteLine("Invalid arguments");
                        return;
                    }
                    program.FileInfo(inputCom[1]);
                }
                else if (inputCom[0].Equals("FILEINFO"))
                {
                    if (inputCom.Length != 2)
                    {
                        Console.WriteLine("Invalid arguments");
                        return;
                    }
                    program.FileInfo(inputCom[1]);
                }
                else if (inputCom[0].Equals("EXIT"))
                {
                    System.Environment.Exit(1);
                    return;
                }
                input = Console.ReadLine();

            }
        }
    }

}