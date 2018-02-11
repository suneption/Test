using Saber.TestTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saber.TestTask
{
    public interface IListNodeConverter : IConverter<ListNode>
    {
        ListNode ToLinkFromString(string value, Dictionary<int, ListNode> idMappings);
        string ToStringFromLink(string format, ListNode node, IReadOnlyDictionary<ListNode, int> idMappings);
    }
}
