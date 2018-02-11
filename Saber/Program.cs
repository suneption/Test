using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var headNode = new ListNode()
            {
                Data = "Head \r\n \"Hello world\" \r\nprev: 0\r\n node"
            };

            var node2 = new ListNode()
            {
                Data = "Second node"
            };

            headNode.Next = node2;
            node2.Prev = headNode;

            var tailNode = new ListNode()
            {
                Data = "Tail node"
            };

            node2.Next = tailNode;
            tailNode.Prev = node2;

            headNode.Rand = tailNode;
            tailNode.Rand = node2;
            
            var list = new ListRand()
            {
                Head = headNode,
                Tail = tailNode,
                Count = 3
            };
            using (var fs = File.Open("test.txt", FileMode.Open))
            {
                //list.Serialize(fs);
                //fs.Flush();
                list.Deserialize(fs);
            }
        }
    }

    public class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }
    public class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;
        public void Serialize(FileStream s)
        {
            var fileManager = new FileManager(s);
            var serializer = new Serializer(fileManager);
            serializer.Serialize(this);
        }
        public void Deserialize(FileStream s)
        {
            var fileManager = new FileManager(s);
            var deserializer = new Deserializer(fileManager);
            var listRand = deserializer.Deserialize();
        }
    }
}
