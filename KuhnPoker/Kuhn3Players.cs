
namespace CFR.KuhnPoker;

public class Kuhn3Players : Game
{
    public Kuhn3Players(int cardsInPlay = 4) : base(cardsInPlay)
    {
        NumOfPlayers = 3;
    }

    public override double CFR(int[] cards, string history, double[] probs)
    {
        double p0 = probs[0];
        double p1 = probs[1];
        double p2 = probs[2];

        int plays = history.Length;
        int player = plays % 3;
        int opponent1 = (player + 1) % 3;
        int opponent2 = (player + 2) % 3;

        // TODO: terminal states !!


        string infoSet = $"{cards[player]}{history}";
        if (!nodeMap.ContainsKey(infoSet))
        {
            Node newNode = new(infoSet);
            nodeMap[infoSet] = newNode;
        }

        Node node = nodeMap[infoSet];

        double[] strategy = node.GetStrategy(probs[player]);
        double[] util = new double[NumActions];
        double nodeUtil = 0;
        for (int a = 0; a < NumActions; a++)
        {
            string nextHistory = history + (a == 0 ? "p" : "b");

            util[a] = player switch
            {
                0 => -CFR(cards, nextHistory, new double[] { p0 * strategy[a], p1, p2 }),
                1 => -CFR(cards, nextHistory, new double[] { p0, p1 * strategy[a], p2 }),
                2 => -CFR(cards, nextHistory, new double[] { p0, p1, p2 * strategy[a] }),
                _ => throw new Exception($"Invalid player [{player}]"),
            };
            nodeUtil += strategy[a] * util[a];
        }

        for (int a = 0; a < NumActions; a++)
        {
            double regret = util[a] - nodeUtil;

            node.regretSum[a] += player switch
            {
                0 => p1 * p2 * regret,
                1 => p0 * p2 * regret,
                2 => p0 * p1 * regret,
                _ => throw new Exception($"Invalid player [{player}]"),
            };
        }

        return nodeUtil;
    }
}