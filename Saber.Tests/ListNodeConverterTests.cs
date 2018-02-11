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
        public void From_ListNodeWithData_HaveCorrectResult()
        {
            var listNode = new ListNode
            {
                Data = "Hello world"
            };
            var idMappings = GetMappings(new List<ListNode> { listNode });
            var converter = GetConverter(idMappings);

            var str = converter.ToString(listNode);

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("id: 0");
            stringBuilder.AppendLine("prev: ");
            stringBuilder.AppendLine("next: ");
            stringBuilder.AppendLine("rand: ");
            stringBuilder.AppendLine("data: \"Hello world\"");
            stringBuilder.AppendLine("}");
            var expected = stringBuilder.ToString();
            Assert.That(str == expected);
        }

        [Test]
        public void From_ListNodeWithAllPossibleRefs_HaveCorrectResult()
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
            var idMappings = GetMappings(new List<ListNode> { listNode, randNode, nextNode, prevNode });
            var converter = GetConverter(idMappings);

            var str = converter.ToString(listNode);

            var expected = Expected();
            Assert.That(Regex.IsMatch(str, expected));
        }

        private string Expected()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine(@"id: \d+");
            stringBuilder.AppendLine(@"prev: \d+");
            stringBuilder.AppendLine(@"next: \d+");
            stringBuilder.AppendLine(@"rand: \d+");
            stringBuilder.AppendLine("data: \"List node\"");
            stringBuilder.AppendLine("}");

            return stringBuilder.ToString();
        }

        //[Test]
        //public void Convert_FromStringToListNode_GetOneNodeWithoutData()
        //{
        //    var converter = new Deserializer();
        //    var lines = StringTemplates.NodeWithData.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            
        //    var node = converter.Deserialize(lines, new Dictionary<int, ListNode>());

        //    Assert.That(
        //        node != null &&
        //        node.Data == "List node");
        //}
    }
}
