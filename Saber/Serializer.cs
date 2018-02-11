using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.TestTask
{
    public class Serializer
    {
        public IFileManager FileManager { get; }

        public Serializer(IFileManager fileManager)
        {
            FileManager = fileManager;
        }

        public byte[] EncodeToBytes(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return bytes;
        }
        
        public void Serialize(ListRand list)
        {
            var idMappings = GenerateIdMappings(list.Head, list.Tail);

            var converter = new ListRandConverter(idMappings);
            var serializedList = SerializeListAsStream(list, idMappings);

            foreach (var item in serializedList)
            {
                WriteToFile(item);
            }
        }

        public IEnumerable<string> SerializeListAsStream(ListRand list, IReadOnlyDictionary<ListNode, int> idMappings)
        {
            var listConverter = new ListRandConverter(idMappings);

            yield return listConverter.ToString(list);

            var serializedNodes = SerializeNodesAsStream(idMappings, list.Head);

            foreach (var serializedNode in serializedNodes)
            {
                yield return serializedNode;
            }

            yield return listConverter.GetStringOfClosePart();
        }

        public IEnumerable<string> SerializeNodesAsStream(IReadOnlyDictionary<ListNode, int> idMappings, ListNode headNode)
        {
            var currNode = headNode;
            while (currNode != null)
            {
                var nodeConverter = new ListNodeConverter(idMappings);
                var nodeString = nodeConverter.ToString(currNode);
                yield return nodeString;
                currNode = currNode.Next;
            }
        }

        private void WriteToFile(string input)
        {
            var dataBytes = EncodeToBytes(input);
            FileManager.Write(dataBytes);
        }

        private int GetOrAddIdMapping(Dictionary<ListNode, int> table, ListNode node, int newId)
        {
            if (table.TryGetValue(node, out var existingId))
            {
                return existingId;
            }

            table.Add(node, newId);
            newId++;
            return newId;
        }

        private IReadOnlyDictionary<ListNode, int> GenerateIdMappings(ListNode headNode, ListNode tailNode)
        {
            var startId = 0;
            var idMappings = new Dictionary<ListNode, int>();
            var currNode = headNode;
            while (currNode != null)
            {
                var refs = new List<ListNode> { headNode, headNode.Rand, headNode.Prev, headNode.Next }
                    .Where(x => x != null)
                    .ToList();
                startId = refs.Aggregate(startId, (currId, x) => GetOrAddIdMapping(idMappings, x, currId));

                currNode = currNode.Next;
            }

            return idMappings;
        }
    }
}
