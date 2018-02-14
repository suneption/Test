using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Saber.TestTask
{
    public class ListNodeConverter : IListNodeConverter
    {
        private IReadOnlyDictionary<ListNode, int> _idMappings;

        public ListNodeConverter()
            : this(new Dictionary<ListNode, int>())
        { }

        public ListNodeConverter(IReadOnlyDictionary<ListNode, int> idMappings)
        {
            _idMappings = idMappings;
        }

        public string ToString(ListNode source)
        {
            var result = new StringBuilder();

            result.AppendLine(Constants.ObjectSymbols.Start);

            var id = _idMappings[source];
            var idStr = $"{Constants.ObjectSymbols.ListNode.Id}: {id}";
            result.AppendLine(idStr);

            result.AppendLine(ToStringFromLink($"{nameof(source.Prev)}:{{0}}", source.Prev, _idMappings));
            result.AppendLine(ToStringFromLink($"{nameof(source.Next)}:{{0}}", source.Next, _idMappings));
            result.AppendLine(ToStringFromLink($"{nameof(source.Rand)}:{{0}}", source.Rand, _idMappings));

            var escapedDataStr = source.Data.ToLiteral();
            result.AppendLine($"{nameof(source.Data)}:{escapedDataStr}");

            result.AppendLine(Constants.ObjectSymbols.End);

            return result.ToString();
        }

        public ListNode ToNode(FileLinesStream commandsStream, Dictionary<int, ListNode> idMappings)
        {
            //var c = new FromStringToObject<ListNode>();
            //var res = c.Foo(commandsStream, idMappings);

            var line = commandsStream.Peek();
            if (line != Constants.ObjectSymbols.Start)
            {
                return null;
            }

            var node = new ListNode();
            line = commandsStream.Next();
            while (line != null)
            {
                var (name, value) = Split(line);

                switch (name)
                {
                    case Constants.ObjectSymbols.ListNode.Id:
                        if (string.IsNullOrEmpty(value))
                        {
                            throw new FileHasIncorrectFormat();
                        }
                        node = ToLinkFromString(value, idMappings);
                        break;
                    case nameof(node.Prev):
                        node.Prev = ToLinkFromString(value, idMappings);
                        break;
                    case nameof(node.Next):
                        node.Next = ToLinkFromString(value, idMappings);
                        break;
                    case nameof(node.Rand):
                        node.Rand = ToLinkFromString(value, idMappings);
                        break;
                    case nameof(node.Data):
                        node.Data = value;
                        break;
                    case Constants.ObjectSymbols.End:
                        commandsStream.Next();
                        return node;
                    default:
                        throw new FileHasIncorrectFormat();
                }

                line = commandsStream.Next();
            }

            return node;
        }

        public ListNode ToLinkFromString(string value, Dictionary<int, ListNode> idMappings)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var id = int.Parse(value);
            var node = GetOrAddNode(idMappings, id);

            return node;
        }

        public string ToStringFromLink(string format, ListNode node, IReadOnlyDictionary<ListNode, int> idMappings)
        {
            if (node == null)
            {
                return string.Format(format, "");
            }

            var nodeId = idMappings[node];
            return string.Format(format, nodeId);
        }

        public ListNode GetOrAddNode(Dictionary<int, ListNode> idMappings, int id)
        {
            if (idMappings.TryGetValue(id, out var node))
            {
                return node;
            }

            node = new ListNode();
            idMappings[id] = node;
            return node;
        }

        public (string name, string value) Split(string line)
        {
            var nameRaw = line.TakeWhile(x => x != ':').ToArray();
            var name = new string(nameRaw).Trim();
            var valueRaw = line.Skip(nameRaw.Length + 1).ToArray();
            var value = new string(valueRaw).Trim();
            return (name, value);
        }
    }

    //public class FromStringToObject<TObject> where TObject : class, new()
    //{
    //    public TObject Foo(CommandsStream elementsStream, Dictionary<int, TObject> idMappings)
    //    {
    //        var line = elementsStream.Peek();
    //        if (line != Constants.ObjectSymbols.Start)
    //        {
    //            return null;
    //        }

    //        var type = typeof(TObject);
    //        var fields = type.GetFields().Where(f => f.IsPublic).ToList();
    //        var fieldNames = fields.Select(x => x.Name).ToList();

    //        var node = new TObject();
    //        line = elementsStream.Next();
    //        while (line != null)
    //        {
    //            var nameRaw = line.TakeWhile(x => x != ':').ToArray();
    //            var name = new string(nameRaw).Trim();
    //            var valueRaw = line.Skip(nameRaw.Length + 1).ToArray();
    //            var value = new string(valueRaw).Trim();

    //            if (name == Constants.ObjectSymbols.ListNode.Id)
    //            {
    //                if (string.IsNullOrEmpty(value))
    //                {
    //                    throw new FileHasIncorrectFormat();
    //                }
    //                node = ToLink(value, idMappings);
    //            }
    //            else if (fieldNames.Contains(name))
    //            {
    //                var field = fields.First(x => x.Name == name);
    //                var fieldType = field.FieldType;
    //                if (fieldType == typeof(string))
    //                {
    //                    field.SetValue(node, value);
    //                }
    //                else if (!fieldType.IsValueType)
    //                {
    //                    var link = ToLink(value, idMappings);
    //                    field.SetValue(node, link);
    //                }
    //            }
    //            else if (name == "}")
    //            {
    //                elementsStream.Next();
    //                return node;
    //            }
    //            else
    //            {
    //                throw new ArgumentException("Incorrect symbol");
    //            }

    //            line = elementsStream.Next();
    //        }

    //        return node;
    //    }

    //    private TObject ToLink(string value, Dictionary<int, TObject> idMappings)
    //    {
    //        if (string.IsNullOrEmpty(value))
    //        {
    //            return null;
    //        }

    //        var id = int.Parse(value);
    //        var node = GetOrAddNode(idMappings, id);

    //        return node;
    //    }

    //    private TObject GetOrAddNode(Dictionary<int, TObject> idMappings, int id)
    //    {
    //        if (idMappings.TryGetValue(id, out var node))
    //        {
    //            return node;
    //        }

    //        node = new TObject();
    //        idMappings[id] = node;
    //        return node;
    //    }
    //}
}
