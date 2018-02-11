using Moq;
using NUnit.Framework;
using Saber.TestTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static ListNode NodeWithNextNodeThatIsRandNodeAlso
        {
            get
            {
                var result = NodeWithoutLinks;
                result.Rand = RandomNode;
                result.Next = RandomNode;
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
                return result;
            }
        }

        public static ListRand ListRandWithOneNodeThatHasRandNode
        {
            get
            {
                var node = NodeWithLinkToRandomNode;
                var result = new ListRand();
                result.Head = node;
                result.Tail = node;
                return result;
            }
        }
    }
    
    [TestFixture]
    public class SerializerTests
    {
        private Mock<IListNodeConverter> GetListNodeConvert()
        {
            var converter = new Mock<IListNodeConverter>();
            return converter;
        }

        private Mock<IFileManager> GetFileManager()
        {
            var fileManagerMock = new Mock<IFileManager>();
            return fileManagerMock;
        }

        private Serializer GetSerializer(IFileManager fileManager)
        {
            var serializer = new Serializer(fileManager);
            return serializer;
        }

        [Test]
        public void Serializer_CreationTest_Successfull()
        {
            var fileManagerMock = new Mock<IFileManager>();
            var converter = GetListNodeConvert();
            var serializer = GetSerializer(fileManagerMock.Object);

            Assert.That(serializer != null);
        }

        [Test]
        public void Serializer_SerializeOneNodeWithoutLinks_Successful()
        {
            var fileManagerMock = GetFileManager();
            fileManagerMock.Setup(x => x.Write(It.IsAny<byte[]>()));
            var serializer = GetSerializer(fileManagerMock.Object);
            var list = ContainerOfListNodes.ListRandWithOneNode;

            serializer.Serialize(list);

            fileManagerMock.Verify(x => x.Write(It.IsAny<byte[]>()), Times.Exactly(3));
        }

        [Test]
        public void Serialize_ListWithTwoNodes_Success()
        {
            var fileManagerMock = GetFileManager();
            fileManagerMock.Setup(x => x.Write(It.IsAny<byte[]>()));
            var serializer = GetSerializer(fileManagerMock.Object);
            var list = ContainerOfListNodes.ListRandWithOneNodeThatHasRandNode;

            serializer.Serialize(list);

            fileManagerMock.Verify(x => x.Write(It.IsAny<byte[]>()), Times.Exactly(3));
        }
    }
}
