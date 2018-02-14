using System;
using System.Collections.Generic;
using System.IO;
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
            var lines = FileManager.ReadLines();

            var idMappings = new Dictionary<int, ListNode>();
            var commandsStream = new FileLinesStream(lines);
            var list = Deserialize(commandsStream, idMappings);

            return list;
        }

        public ListRand Deserialize(FileLinesStream commands, Dictionary<int, ListNode> idMappings)
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
}
