
namespace CFR.KuhnPoker;

public class Kuhn2Players : Game
{
    public Kuhn2Players(int cardsInPlay = 3) : base(cardsInPlay)
    {
        NumOfPlayers = 2;
    }

    public override double CFR(int[] cards, string history, double[] probs)
    {
        double p0 = probs[0];
        double p1 = probs[1];

        int plays = history.Length;
        int player = plays % 2;
        int opponent = 1 - player;

        // Return payoff for terminal states
        if (plays > 1)
        {
            bool isPlayerCardHigher = cards[player] > cards[opponent];
            if (history[^1] == 'p')
            {
                if (history == "pp")
                {
                    return isPlayerCardHigher ? 1 : -1;
                }
                return 1;
            }
            else if (history[^2..] == "bb")
            {
                return isPlayerCardHigher ? 2 : -2;
            }
        }

        string infoSet = $"{cards[player]}{history}";

        // Get information set node or create it if nonexistent
        if (!nodeMap.ContainsKey(infoSet))
        {
            Node newNode = new(infoSet);
            nodeMap[infoSet] = newNode;
        }

        Node node = nodeMap[infoSet];

        // Recursively call cfr with additional history and probability
        double[] strategy = node.GetStrategy(player == 0 ? p0 : p1);
        double[] util = new double[NumActions];
        double nodeUtil = 0;
        for (int a = 0; a < NumActions; a++)
        {
            string nextHistory = history + (a == 0 ? "p" : "b");
            util[a] = player == 0
                ? -CFR(cards, nextHistory, new double[] {p0 * strategy[a], p1})
                : -CFR(cards, nextHistory, new double[] {p0, p1 * strategy[a]});
            nodeUtil += strategy[a] * util[a];
        }

        // Compute and accumulate CFR
        for (int a = 0; a < NumActions; a++)
        {
            double regret = util[a] - nodeUtil;
            node.regretSum[a] += (player == 0 ? p1 : p0) * regret;
        }

        return nodeUtil;
    }


}