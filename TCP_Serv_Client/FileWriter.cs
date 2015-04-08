using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TCP_Serv_Client
{
    public class FileWriter
    {
        public string filepath;
        public FileStream fs;
        public FileWriter() { }
        public FileWriter(string file)
        {
            filepath = file;
        }
        public FileWriter(string file, string data)
        {
            filepath = file;
            WriteLineToFile(data);
        }
        public bool Exists()
        {
            if (File.Exists(filepath)) return true;
            else return false;
        }
        public bool CreateFile()
        {
            if (Exists()) return true;
            else
            {
                try
                {
                    string path = Path.GetDirectoryName(filepath);
                    Directory.CreateDirectory(path);
                    File.Create(filepath).Close(); // Make sure to close the path when done otherwise it is in use 
                }
                catch (Exception ex)
                {
                    // uh oh
                }
                if (Exists()) return true;
                else return false;
            }
        }
        public bool WriteLineToFile(string str)
        {
            CreateFile();
            try
            {
                fs = new FileStream(filepath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                // oops not logged
            }
            
            return true;
        }
    }
}
