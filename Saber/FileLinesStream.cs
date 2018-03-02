using System;
using System.Collections.Generic;

namespace Saber.TestTask
{
    public class FileLinesStream : IDisposable
    {
        private readonly IEnumerator<string> _innerEnumerator;
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
            _innerEnumerator = enumerator;
        }
        
        public string Peek()
        {
            return _innerEnumerator.Current;
        }
        
        public string Next()
        {
            if (!_innerEnumerator.MoveNext())
            {
                return null;
            }

            return _innerEnumerator.Current;
        }

        public void Dispose()
        {
            if (_innerEnumerator != null)
            {
                _innerEnumerator.Dispose();
            }
        }
    }
}
