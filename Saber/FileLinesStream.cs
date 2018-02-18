using System;
using System.Collections.Generic;

namespace Saber.TestTask
{
    public class FileLinesStream : IDisposable
    {
        private readonly IEnumerator<string> _commandsEnumerator;
        public IEnumerable<string> OriginLines { get; }

        public FileLinesStream(IEnumerable<string> lines)
        {
            OriginLines = lines;
            var enumerator = OriginLines.GetEnumerator();
            try
            {
                enumerator.MoveNext();
            }
            catch
            {
                enumerator.Dispose();
                throw;
            }
            _commandsEnumerator = enumerator;
        }
        
        public string Peek()
        {
            return _commandsEnumerator.Current;
        }
        
        public string Next()
        {
            if (!_commandsEnumerator.MoveNext())
            {
                return null;
            }

            return _commandsEnumerator.Current;
        }

        public void Dispose()
        {
            if (_commandsEnumerator != null)
            {
                _commandsEnumerator.Dispose();
            }
        }
    }
}
