using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Saber.TestTask
{
    public class ListRandConverter : IListRandConverter
    {
        private IListNodeConverter _listNodeConverter;
        private IReadOnlyDictionary<ListNode, int> _idMappings;

        public ListRandConverter()
            : this(new Dictionary<ListNode, int>())
        { }

        public ListRandConverter(IReadOnlyDictionary<ListNode, int> idMappings)
        {
            _idMappings = idMappings;
            _listNodeConverter = new ListNodeConverter();
        }

        public string ToString(ListRand source)
        {
            var result = new StringBuilder();

            result.AppendLine(Constants.ObjectSymbols.Start);
            var headStr = LinkToNodeToString($"{nameof(source.Head)}:{{0}}", source.Head);
            result.AppendLine(headStr);
            var tailStr = LinkToNodeToString($"{nameof(source.Tail)}:{{0}}", source.Tail);
            result.AppendLine(tailStr);
            result.AppendLine($"{nameof(source.Count)}:" + source.Count);
            result.AppendLine($"{Constants.ObjectSymbols.ListRand.Nodes}: {Constants.ArraySymbols.Start}");

            return result.ToString();
        }

        public string GetStringOfClosePart()
        {
            var closePartStr =
                Constants.ArraySymbols.End + Environment.NewLine +
                Constants.ObjectSymbols.End + Environment.NewLine;
            return closePartStr;
        }

        public ListRand ToList(CommandsStream commands, Dictionary<int, ListNode> idMappings)
        {
            var line = commands.Peek();
            if (line != Constants.ObjectSymbols.Start)
            {
                return null;
            }

            var list = new ListRand();
            line = commands.Next();
            while (line != null)
            {
                var nameRaw = line.TakeWhile(x => x != ':').ToArray();
                var name = new string(nameRaw).Trim();
                var valueRaw = line.Skip(nameRaw.Length + 1).ToArray();
                var value = new string(valueRaw).Trim();

                switch (name)
                {
                    case nameof(list.Head):
                        list.Head = ToLink(value, idMappings);
                        break;
                    case nameof(list.Tail):
                        list.Tail = ToLink(value, idMappings);
                        break;
                    case nameof(list.Count):
                        list.Count = int.Parse(value);
                        break;
                    case Constants.ObjectSymbols.ListRand.Nodes:
                        if (value != Constants.ArraySymbols.Start)
                        {
                            throw new FileHasIncorrectFormat();
                        }

                        commands.Next();
                        ListNode node = null;
                        do
                        {
                            var nodeConverter = new ListNodeConverter();
                            node = nodeConverter.ToNode(commands, idMappings);
                        } while (node != null);

                        var command = commands.Peek();
                        if (command != Constants.ArraySymbols.End)
                        {
                            throw new FileHasIncorrectFormat();
                        }
                        break;
                    case Constants.ObjectSymbols.End:
                        commands.Next();
                        return list;
                    default:
                        throw new FileHasIncorrectFormat();
                }

                line = commands.Next();
            }

            return list;
        }

        private string LinkToNodeToString(string format, ListNode node)
        {
            return _listNodeConverter.ToStringFromLink(format, node, _idMappings);
        }

        private ListNode ToLink(string value, Dictionary<int, ListNode> idMappings)
        {
            return _listNodeConverter.ToLinkFromString(value, idMappings);
        }
    }
}
