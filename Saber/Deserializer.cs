using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.TestTask
{
    public class Deserializer
    {
        public IFileManager FileManager { get; }

        public Deserializer(IFileManager fileManager)
        {
            FileManager = fileManager;
        }

        public ListRand Deserialize()
        {
            var contentInBytes = new List<byte>();
            var bytes = FileManager.Read();
            while (bytes.Length > 0)
            {
                contentInBytes.AddRange(bytes);
                bytes = FileManager.Read();
            }

            var content = Encoding.UTF8.GetString(contentInBytes.ToArray());

            var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var idMappings = new Dictionary<int, ListNode>();
            var commandsStream = new CommandsStream(lines);
            var list = Deserialize(commandsStream, idMappings);

            return list;
        }

        public ListRand Deserialize(CommandsStream commands, Dictionary<int, ListNode> idMappings)
        {
            var converter = new ListRandConverter();
            var list = converter.ToList(commands, idMappings);
            return list;
        }

        private ListNode ToLink(string value, Dictionary<int, ListNode> idMappings)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            
            var id = int.Parse(value);
            if (idMappings.TryGetValue(id, out var node))
            {
                return node;
            }

            node = new ListNode();
            idMappings[id] = node;

            return node;
        }
    }

    public class CommandsStream : IDisposable
    {
        private readonly IEnumerator<string> _commandsEnumerator;
        private IEnumerator<char> _charEnumerator;

        public IEnumerable<string> Commands { get; private set; }

        public CommandsStream(IEnumerable<string> lines)
        {
            Commands = lines.ToList();

            var line = Commands.First();

            var enumerator = Commands.GetEnumerator();
            var charEnumerator = line.GetEnumerator();
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
            return PeekElement();
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
            //if (!_commandsEnumerator.MoveNext())
            //{
            //    return null;
            //}

            //return _commandsEnumerator.Current;

            return NextElement();
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

        public void Dispose()
        {
            if (_commandsEnumerator != null)
            {
                _commandsEnumerator.Dispose();
            }
        }
    }
}
