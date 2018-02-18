using NUnit.Framework;
using Saber.TestTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Saber.Tests
{
    [TestFixture]
    public class ListNodeConverterTests
    {
        public static class StringTemplates
        {
            public static (ListNode node, string nodeString) HelloWorldNode
            {
                get
                {
                    var node = new ListNode
                    {
                        Data = "Hello world"
                    };

                    var stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("{");
                    stringBuilder.AppendLine("Id:0");
                    stringBuilder.AppendLine("Prev:");
                    stringBuilder.AppendLine("Next:");
                    stringBuilder.AppendLine("Rand:");
                    stringBuilder.AppendLine("Data:Hello world");
                    stringBuilder.AppendLine("}");
                    var nodeStr = stringBuilder.ToString();
                    return (node, nodeStr);
                }
            }

            public static (ListNode node, string nodeString) NodeWithAllLinks
            {
                get
                {
                    ListNode Node()
                    {
                        var randNode = new ListNode
                        {
                            Data = "Random node"
                        };

                        var nextNode = new ListNode
                        {
                            Data = "Next node"
                        };

                        var prevNode = new ListNode
                        {
                            Data = "Prev node"
                        };

                        var listNode = new ListNode
                        {
                            Rand = randNode,
                            Next = nextNode,
                            Prev = prevNode,
                            Data = "List node"
                        };

                        return listNode;
                    }

                    string NodeAsString()
                    {
                        var stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("{");
                        stringBuilder.AppendLine(@"Id:\d+");
                        stringBuilder.AppendLine(@"Prev:\d+");
                        stringBuilder.AppendLine(@"Next:\d+");
                        stringBuilder.AppendLine(@"Rand:\d+");
                        stringBuilder.AppendLine("Data:List node");
                        stringBuilder.AppendLine("}");

                        return stringBuilder.ToString();
                    }

                    return (Node(), NodeAsString());
                }
            }

            public static string NodeWithData
            {
                get
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("{");
                    sb.AppendLine("id: 0");
                    sb.AppendLine("prev: ");
                    sb.AppendLine("next: ");
                    sb.AppendLine("rand: ");
                    sb.AppendLine("data: \"List node\"");
                    sb.AppendLine("}");
                    return sb.ToString();
                }
            }
        }

        public ListNodeConverter GetConverter(IReadOnlyDictionary<ListNode, int> idMappings)
        {
            return new ListNodeConverter(idMappings);
        }

        public IReadOnlyDictionary<ListNode, int> GetMappings(List<ListNode> listNodes)
        {
            var id = 0;
            var result = new Dictionary<ListNode, int>();
            foreach (var listNode in listNodes)
            {
                result.Add(listNode, id);
                id++;
            }
            return result;
        }

        [Test]
        public void Creation_WithoutParameters_Success()
        {
            var listNodeConverter = GetConverter(new Dictionary<ListNode, int>());

            Assert.That(listNodeConverter != null);
        }

        [Test]
        public void NodeToString_ListNodeWithData_HasCorrectResult()
        {
            var (listNode, expected) = StringTemplates.HelloWorldNode;
            var idMappings = GetMappings(new List<ListNode> { listNode });
            var converter = GetConverter(idMappings);

            var origin = converter.ToString(listNode);
            
            Assert.That(origin == expected);
        }

        [Test]
        public void NodeToString_ListNodeWithAllPossibleRefs_HaveCorrectResult()
        {
            var (listNode, expected) = StringTemplates.NodeWithAllLinks;
            var idMappings = GetMappings(new List<ListNode> { listNode, listNode.Rand, listNode.Next, listNode.Prev });
            var converter = GetConverter(idMappings);

            var str = converter.ToString(listNode);
            
            Assert.That(Regex.IsMatch(str, expected));
        }
    }
}
