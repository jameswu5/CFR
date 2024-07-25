using System.Text;

namespace CFR.Dudo;

public class Game
{
    public int NumSides;
    public int NumActions;
    public int Dudo;

    public int[] claimNum;
    public int[] claimRank;

    public readonly Random random;

    public Dictionary<int, Node> nodeMap;

    public Game(int sides = 6)
    {
        NumSides = sides;
        NumActions = 2 * NumSides + 1;
        Dudo = NumActions - 1;

        claimNum = new int[NumActions - 1];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < NumSides; j++)
            {
                claimNum[i * NumSides + j] = i + 1;
            }
        }
        claimRank = new int[NumActions - 1];
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < NumSides; j++)
            {
                claimRank[i * NumSides + j] = (j + 1) % NumSides; // for now dice are from 0 to 5
            }
        }

        random = new();
        nodeMap = new();
    }

    public double CFR(int[] rolls, List<int> history, double p0, double p1)
    {
        // Console.WriteLine($"Rolls: {rolls[0]} {rolls[1]} | Reach prob: {p0} {p1}");

        // rolls: [player 1's roll, player 2's roll]

        int player = history.Count % 2;

        // Check if history is terminal
        if (history.Count > 0 && history[^1] == Dudo)
        {
            // opponent made the claim
            int[] rollCounts = new int[NumSides];
            foreach (int roll in rolls) rollCounts[roll]++;

            // If claim is true, we win since the opponent made the doubt
            int multiplier = player == 0 ? 1 : -1;
            return VerifyClaim(history, rollCounts) ? -multiplier : multiplier;
        }

        int infoID = InfoSetToInteger(rolls[player], history);
        int actionsLeft = history.Count > 0 ? NumActions - history[^1] - 1 : NumActions;

        // Get information set node or create it if nonexistent
        if (!nodeMap.ContainsKey(infoID))
        {
            Node newNode = new(NumActions, actionsLeft, infoID, InfoIntToString(infoID));
            nodeMap[infoID] = newNode;
        }
        Node node = nodeMap[infoID];

        // Recursively call cfr with additional history and probability
        double[] strategy = node.GetStrategy(player == 0 ? p0 : p1);
        double[] util = new double[actionsLeft];
        double nodeUtil = 0;
        for (int a = 0; a < actionsLeft; a++)
        {
            int choice = a + NumActions - actionsLeft;
            history.Add(choice);
            util[a] = player == 0
                ? -CFR(rolls, history, p0 * strategy[a], p1)
                : -CFR(rolls, history, p0, p1 * strategy[a]);
            nodeUtil += strategy[a] * util[a];
            history.RemoveAt(history.Count - 1);
        }

        // Compute and accumulate CFR
        for (int a = 0; a < actionsLeft; a++)
        {
            double regret = util[a] - nodeUtil;
            node.regretSum[a] += (player == 0 ? p1 : p0) * regret;
        }

        return nodeUtil;
    }

    public void Train(int iterations)
    {
        double util = 0;
        for (int i = 0; i < iterations; i++)
        {
            int[] rolls = { random.Next(NumSides), random.Next(NumSides) };
            util += CFR(rolls, new List<int>(), 1, 1);

            if (i % 100 == 0)
            {
                Console.WriteLine($"Iteration {i}");
            }
        }

        Console.WriteLine($"Average game value: {util / iterations}\n");
        foreach (Node n in nodeMap.Values)
        {
            // Don't print every value
            // if (random.Next(100) == 0) Console.WriteLine(n);
            Console.WriteLine(n);
        }
    }

    private int InfoSetToInteger(int roll, List<int> history)
    {
        // ___hhhhrrr: last 3 bits encode the roll
        int res = history.Count > 0 ? history[^1] : NumActions + 1;
        return res * 8 + roll;
    }

    private string InfoIntToString(int infoInt)
    {
        int roll = infoInt & 0b111;
        int claim = infoInt >> 3;
        if (claim == NumActions + 1) return $"{roll+1} | None";
        return $"{roll+1} |  {claimNum[claim]}x{claimRank[claim]+1}";
    }

    private bool VerifyClaim(List<int> history, int[] rollCounts)
    {
        if (history.Count == 1) return true;

        // Rolling bottom number counts everywhere
        for (int i = 1; i < rollCounts.Length; i++) rollCounts[i] += rollCounts[0];

        int claimNum = this.claimNum[history[^2]];
        int claimRank = this.claimRank[history[^2]];
        if (rollCounts[claimRank] < claimNum) return false;

        return true;
    }
}