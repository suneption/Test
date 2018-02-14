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

        public FileManager(FileStream fileStream)
        {
            _innerFileStream = fileStream;
            _innerStreamReader = new StreamReader(_innerFileStream, Encoding.UTF8);
        }

        public void Write(byte[] bytes)
        {
            _innerFileStream.Write(bytes, 0, bytes.Length);
        }

        public byte[] Read()
        {
            var bytes = new byte[4 * 1024];
            var count = _innerFileStream.Read(bytes, 0, bytes.Length);

            if (count == 0)
            {
                return new byte[] { };
            }

            if (count == bytes.Length)
            {
                return bytes;
            }

            var result = bytes.Take(count).ToArray();
            return result;
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

        public void WriteLine(string input)
        {

        }
    }
}
