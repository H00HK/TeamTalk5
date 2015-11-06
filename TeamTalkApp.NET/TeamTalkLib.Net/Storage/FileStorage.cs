using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TeamTalkLib;

namespace TeamTalkLib.Storage
{
    public class FileStorage<T> : IStorage<List<T>>
    {
        private string fileName;

        public FileStorage(string fileName)
        {
            this.fileName = fileName;
        }

        public void Store(List<T> data)
        {
            int len = data.Count;
            string[] output = new string[len];
            for(int i =0; i < len; i++)
            {
                output[i] = data[i].ToString();
            }
            

            File.WriteAllLines(GetFileNameWithDate(), output);
        }

        private string GetFileNameWithDate()
        {
            return fileName + "_" + DateTime.Now.ToString("yyyy-mm-dd hh.mm.ss") + ".txt";
        }
    }

}

