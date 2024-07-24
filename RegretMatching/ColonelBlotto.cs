
namespace CFR.RegretMatching;

public class ColonelBlotto : Trainer
{
    private readonly int battlefields;
    private readonly List<int[]> actions;

    public ColonelBlotto(int battlefields, int soldiers) : base()
    {
        this.battlefields = battlefields;

        NumActions = Utility.Choose(soldiers + battlefields - 1, battlefields - 1);
        actions = new List<int[]>();
        GenerateActions(soldiers, battlefields, 0, new int[battlefields], actions);
        UtilityTable = GenerateUtilityTable();

        agent1 = new(NumActions);
        agent2 = new(NumActions);
    }

    private int[,] GenerateUtilityTable()
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

    public override void DisplayActions()
    {
        Console.WriteLine("Actions:");
        for (int i = 0; i < NumActions; i++)
        {
            Console.Write($"{i}:\t( ");
            foreach (int action in actions[i])
            {
                Console.Write($"{action} ");
            }
            Console.WriteLine(")");
        }
    }
}