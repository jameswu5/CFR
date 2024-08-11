
namespace CFR.KuhnPoker;

public class NodeComparer : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        // compare card values first
        if (x.card != y.card)
        {
            return x.card.CompareTo(y.card);
        }

        // compare infoSet values
        return x.infoSet.CompareTo(y.infoSet);
    }
}