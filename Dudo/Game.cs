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

    public double CFR(int[] rolls, bool[] isClaimed, double p0, double p1)
    {
        // Console.WriteLine($"Rolls: {rolls[0]} {rolls[1]} | Reach prob: {p0} {p1}");

        // rolls: [player 1's roll, player 2's roll]

        int plays = 0;
        foreach (bool claim in isClaimed)
        {
            if (claim) plays++;
        }

        int player = plays % 2;
        int opponent = 1 - player;

        // Check if history is terminal
        if (isClaimed[^1])
        {
            int[] rollCounts = new int[NumSides];
            foreach (int roll in rolls) rollCounts[roll]++;

            // Verify the claims
            int multiplier = player == 0 ? 1 : -1;

            // If claim is true, we lose since we are the one who made the doubt
            return VerifyClaim(isClaimed, rollCounts) ? -multiplier : multiplier;
        }

        int infoID = InfoSetToInteger(rolls[player], isClaimed);

        int actionsLeft = 0;
        for (int a = isClaimed.Length - 1; a >= 0; a--)
        {
            if (isClaimed[a]) break;
            actionsLeft++;
        }

        // Get information set node or create it if nonexistent
        if (!nodeMap.ContainsKey(infoID))
        {
            Node newNode = new(NumActions, infoID, ClaimHistoryToString(isClaimed));
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
            isClaimed[choice] = true;
            util[a] = player == 0
                ? -CFR(rolls, isClaimed, p0 * strategy[a], p1)
                : -CFR(rolls, isClaimed, p0, p1 * strategy[a]);
            nodeUtil += strategy[a] * util[a];
            isClaimed[choice] = false;
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
            util += CFR(rolls, new bool[NumActions], 1, 1);

            if (i % 100 == 0)
            {
                Console.WriteLine($"Iteration {i}");
            }

        }
        Console.WriteLine($"Average game value: {util / iterations}\n");
        foreach (Node n in nodeMap.Values)
        {
            Console.WriteLine(n);
        }
    }

    private int InfoSetToInteger(int roll, bool[] isClaimed)
    {
        int res = roll;
        for (int a = NumActions - 2; a >= 0; a--)
        {
            res = res * 2 + (isClaimed[a] ? 1 : 0);
        }

        return res;
    }

    private string ClaimHistoryToString(bool[] isClaimed)
    {
        StringBuilder sb = new();
        for (int a = 0; a < NumActions - 1; a++)
        {
            if (!isClaimed[a]) continue;
            if (sb.Length > 0) sb.Append(", ");
            sb.Append($"{claimNum[a]}x{claimRank[a]}");
        }
        return sb.ToString();
    }

    private bool VerifyClaim(bool[] isClaimed, int[] rollCounts)
    {
        for (int a = 0; a < NumActions - 1; a++)
        {
            if (!isClaimed[a]) continue;
            if (rollCounts[claimRank[a]] < claimNum[a]) return false;
        }
        return true;
    }

}