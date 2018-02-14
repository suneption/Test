using System;
using System.Collections.Generic;
using System.Linq;

namespace Saber.TestTask
{
    public class FileLinesStream : IDisposable
    {
        private readonly IEnumerator<string> _commandsEnumerator;
        private IEnumerator<char> _charEnumerator;

        public IEnumerable<string> Commands { get; private set; }

        public FileLinesStream(IEnumerable<string> lines)
        {
            //Commands = lines.ToList();

            //var line = Commands.First();
            Commands = lines;
            var enumerator = Commands.GetEnumerator();
            //var charEnumerator = line.GetEnumerator();
            var charEnumerator = "".GetEnumerator();
            try
            {
                enumerator.MoveNext();
                charEnumerator.MoveNext();
            }
            catch
            {
                enumerator.Dispose();
                charEnumerator.Dispose();
            }
            _commandsEnumerator = enumerator;
            _charEnumerator = charEnumerator;
        }

        public string PeekLine()
        {
            return _commandsEnumerator.Current;
        }

        public string Peek()
        {
            return PeekLine();
        }

        public string PeekElement()
        {
            return _charEnumerator.Current.ToString();
        }

        public string NextLine()
        {
            if (!_commandsEnumerator.MoveNext())
            {
                return null;
            }

            return _commandsEnumerator.Current;
        }

        public string Next()
        {
            return NextLine();
        }

        public string NextElement()
        {
            if (!_charEnumerator.MoveNext())
            {
                var nextLine = NextLine();
                if (nextLine == null)
                {
                    return null;
                }

                if (_charEnumerator != null)
                {
                    _charEnumerator.Dispose();
                }

                var enumerator = nextLine.GetEnumerator();
                try
                {
                    enumerator.MoveNext();
                }
                catch
                {
                    enumerator.Dispose();
                }
                _charEnumerator = enumerator;
            }

            return _charEnumerator.Current.ToString();
        }

        public IEnumerable<string> Elements()
        {
            do
            {
                yield return Peek();
            } while (Next() != null);
        }

        public void Dispose()
        {
            if (_commandsEnumerator != null)
            {
                _commandsEnumerator.Dispose();
            }
            if (_charEnumerator != null)
            {
                _charEnumerator.Dispose();
            }
        }
    }
}
