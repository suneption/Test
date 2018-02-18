using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.TestTask
{
    public class FileManager : IFileManager
    {
        private readonly FileStream _innerFileStream;
        private readonly StreamReader _innerStreamReader;
        private readonly StreamWriter _innerStreamWriter;

        public FileManager(FileStream fileStream)
        {
            _innerFileStream = fileStream;
            _innerStreamReader = new StreamReader(_innerFileStream, Encoding.UTF8);
            _innerStreamWriter = new StreamWriter(_innerFileStream, Encoding.UTF8);
        }

        public void Write(string input)
        {
            _innerStreamWriter.Write(input);
        }

        public IEnumerable<string> ReadLines()
        {
            string line = null;
            do
            {
                line = _innerStreamReader.ReadLine();
                yield return line;
            }
            while (line != null);
        }

        public void Flush()
        {
            _innerStreamWriter.Flush();
        }
    }
}
