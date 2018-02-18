using Moq;
using NUnit.Framework;
using Saber.TestTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DeepCompare = NUnit.DeepObjectCompare.Is;

namespace Saber.Tests
{
    public static class ContainerOfListNodes
    {
        public static ListNode NodeWithoutLinks
        {
            get
            {
                return new ListNode
                {
                    Data = "Node without links"
                };
            }
        }

        public static ListNode HeadNode
        {
            get
            {
                return new ListNode
                {
                    Data = "Head node"
                };
            }
        }

        public static ListNode TailNode
        {
            get
            {
                return new ListNode
                {
                    Data = "Tail node"
                };
            }
        }

        public static ListNode RandomNode
        {
            get
            {
                return new ListNode
                {
                    Data = "Random node"
                };
            }
        }

        public static ListNode NextNode
        {
            get
            {
                return new ListNode
                {
                    Data = "Next node"
                };
            }
        }

        public static ListNode NodeWithLinkToRandomNode
        {
            get
            {
                var result = NodeWithoutLinks;
                result.Rand = RandomNode;
                return result;
            }
        }

        public static ListRand ListRandWithTwoListNodes
        {
            get
            {
                var result = new ListRand();
                var head = NodeWithoutLinks;
                var nextNode = NextNode;
                head.Next = nextNode;
                var tail = nextNode;
                result.Head = head;
                result.Tail = tail;
                result.Count = 2;
                return result;
            }
        }

        public static ListRand ListRandWithOneNode
        {
            get
            {
                var node = NodeWithoutLinks;
                var result = new ListRand();
                result.Head = node;
                result.Tail = node;
                result.Count = 1;
                return result;
            }
        }

        public static ListRand ListRandWithOneNodeThatHasRandNode
        {
            get
            {
                var headNode = HeadNode;
                var tailNode = TailNode;
                headNode.Next = tailNode;
                headNode.Rand = tailNode;
                var result = new ListRand();
                result.Head = headNode;
                result.Tail = tailNode;
                result.Count = 2;
                return result;
            }
        }
    }
    
    [TestFixture]
    public class SerializerTests
    {
        private Serializer GetSerializer()
        {
            var serializer = new Serializer();
            return serializer;
        }

        [Test]
        public void Serializer_CreationTest_Successfull()
        {
            var serializer = GetSerializer();

            Assert.That(serializer != null);
        }

        [Test]
        public void SerializeDeserialize_OneNodeWithoutLinks_Successful()
        {
            var serializer = GetSerializer();
            var origin = ContainerOfListNodes.ListRandWithOneNode;

            var serialized = serializer.Serialize(origin);
            var splitted = serialized.SelectMany(x => x.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)).ToList(); ;
            var deserialized = serializer.Deserialize(splitted);

            Assert.That(origin, DeepCompare.DeepEqualTo(deserialized));
        }

        [Test]
        public void SerializeDeserialize_ListWithTwoNodes_Success()
        {
            var serializer = GetSerializer();
            var origin = ContainerOfListNodes.ListRandWithOneNodeThatHasRandNode;

            var serialized = serializer.Serialize(origin);
            var splitted = serialized.SelectMany(x => x.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)).ToList(); ;
            var deserialized = serializer.Deserialize(splitted);

            Assert.That(origin, DeepCompare.DeepEqualTo(deserialized));
        }
    }
}
