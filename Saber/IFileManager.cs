using System.Collections.Generic;

namespace Saber.TestTask
{
    public interface IFileManager
    {
        void Write(string input);
        IEnumerable<string> ReadLines();
        void Flush();
    }
}