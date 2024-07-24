
namespace CFR.Dudo;

public class Game
{
    public int NumSides;
    public int NumActions;
    public int Dudo;

    public int[] claimNum;
    public int[] claimRank;

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
                claimRank[i * NumSides + j] = (j + 1) % NumSides + 1;
            }
        }
    }
}