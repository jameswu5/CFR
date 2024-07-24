
namespace CFR.KuhnPoker;

public class Game
{
    public enum Move { Pass, Bet };
    public enum Card { J, Q, K };
    public const int NumActions = 2;
    public readonly Random random;
    public Dictionary<string, Node> nodeMap;

    public Game()
    {
        random = new Random();
        nodeMap = new();
    }

    public double CFR(int[] cards, string history, double p0, double p1)
    {
        int plays = history.Length;
        int player = plays % 2;
        int opponent = 1 - player;

        // Return payoff for terminal states
        if (plays > 1)
        {
            bool terminalPass = history[^1] == 'p';
            bool doubleBet = history[^2..] == "bb";
            bool isPlayerCardHigher = cards[player] > cards[opponent];
            if (terminalPass)
            {
                if (history == "pp")
                {
                    return isPlayerCardHigher ? 1 : -1;
                }
                return 1;
            }
            else if (doubleBet)
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
                ? -CFR(cards, nextHistory, p0 * strategy[a], p1)
                : -CFR(cards, nextHistory, p0, p1 * strategy[a]);
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

    public void Train(int iterations)
    {
        int[] cards = { 0, 1, 2 };
        double util = 0;
        for (int i = 0; i < iterations; i++)
        {
            Utility.Shuffle(cards);
            util += CFR(cards, "", 1, 1);
        }
        Console.WriteLine($"Average game value: {util / iterations}\n");
        foreach (Node n in nodeMap.Values)
        {
            Console.WriteLine(n);
        }
    }
}