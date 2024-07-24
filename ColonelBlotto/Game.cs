
namespace CFR.ColonelBlotto;

public class Game
{
    public int battlefields;
    public int soldiers;
    public int NumActions;
    public List<int[]> actions;

    public int[,] UtilityTable;

    public Game(int battlefields, int soldiers)
    {
        this.battlefields = battlefields;
        this.soldiers = soldiers;
        NumActions = Utility.Choose(soldiers + battlefields - 1, battlefields - 1);
        actions = new List<int[]>();
        GenerateActions(soldiers, battlefields, 0, new int[battlefields], actions);
        UtilityTable = GenerateUtilityTable(actions);
        foreach(int[] action in actions)
        {
            Utility.DisplayArray(action);
        }
        Console.WriteLine("--------------------");
        Utility.DisplayMatrix(UtilityTable);
    }

    private void GenerateActions(int soldiers, int battlefields, int index, int[] current, List<int[]> result)
    {
        if (index == battlefields - 1)
        {
            current[index] = soldiers;
            result.Add(current.ToArray());
            return;
        }

        for (int i = 0; i <= soldiers; i++)
        {
            current[index] = i;
            GenerateActions(soldiers - i, battlefields, index + 1, current, result);
        }
    }

    private int[,] GenerateUtilityTable(List<int[]> actions)
    {
        int[,] table = new int[actions.Count, actions.Count];

        for (int i = 0; i < actions.Count; i++)
        {
            for (int j = 0; j < actions.Count; j++)
            {
                int land = 0;
                for (int b = 0; b < battlefields; b++)
                {
                    if (actions[i][b] > actions[j][b])       land++;
                    else if (actions[i][b] < actions[j][b])  land--;
                }

                if      (land > 0)  table[i, j] = 1;
                else if (land < 0)  table[i, j] = -1;
            }
        }

        return table;
    }
}