
namespace CFR.Dudo;

public class Game
{
    public int NumSides;
    public int NumActions;
    public int Dudo;

    public int[] claimNum;
    public int[] claimRank;

    public readonly Random random;

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
    }

    public double CFR(bool[] isClaimed, double p1, double p2)
    {
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
            // player 'player' is dudo

            // simulate dice rolls
            int[] rollCounts = new int[NumSides];
            rollCounts[random.Next(NumSides)]++;
            rollCounts[random.Next(NumSides)]++;

            // Verify the claims
            int multiplier = player == 0 ? 1 : -1;

            // If claim is true, we lose
            return VerifyClaim(isClaimed, rollCounts) ? -multiplier : multiplier;
        }





        return 0.0;
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