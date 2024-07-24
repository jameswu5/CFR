
namespace CFR.ColonelBlotto;

public class Game
{
    public int battlefields;
    public int soldiers;
    public int NumActions;
    public List<int[]> actions;

    public int[,] UtilityTable;

    public Agent agent1;
    public Agent agent2;

    public Game(int battlefields, int soldiers)
    {
        this.battlefields = battlefields;
        this.soldiers = soldiers;
        NumActions = Utility.Choose(soldiers + battlefields - 1, battlefields - 1);
        actions = new List<int[]>();
        GenerateActions(soldiers, battlefields, 0, new int[battlefields], actions);
        UtilityTable = GenerateUtilityTable(actions);

        agent1 = new(this);
        agent2 = new(this);
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

    public void TrainOneAgent(int iterations, double[] opponentStrategy)
    {
        double[] actionUtility = new double[NumActions];
        for (int i = 0; i < iterations; i++)
        {
            int action1 = agent1.GetAction(agent1.GetStrategy());
            int action2 = agent2.GetAction(opponentStrategy);

            // Compute action utilities
            for (int a = 0; a < NumActions; a++)
            {
                actionUtility[a] = UtilityTable[a, action2];
            }

            // Accumulate action regrets
            for (int a = 0; a < NumActions; a++)
            {
                agent1.regretSum[a] += actionUtility[a] - actionUtility[action1];
            }
        }

        agent2.strategySum = opponentStrategy;
    }

    public void TrainTwoAgents(int iterations)
    {
        double[] actionUtility1 = new double[NumActions];
        double[] actionUtility2 = new double[NumActions];
        for (int i = 0; i < iterations; i++)
        {
            int action1 = agent1.GetAction(agent1.GetStrategy());
            int action2 = agent2.GetAction(agent2.GetStrategy());

            // Compute action utilities
            for (int a = 0; a < NumActions; a++)
            {
                actionUtility1[a] = UtilityTable[a, action2];
                actionUtility2[a] = UtilityTable[a, action1];
            }

            // Accumulate action regrets
            for (int a = 0; a < NumActions; a++)
            {
                agent1.regretSum[a] += actionUtility1[a] - actionUtility1[action1];
                agent2.regretSum[a] += actionUtility2[a] - actionUtility2[action2];
            }
        }
    }

    public void DisplayStrategies()
    {
        Console.WriteLine("Player 1:");
        Utility.DisplayArray(agent1.GetAverageStrategy());
        Console.WriteLine("Player 2:");
        Utility.DisplayArray(agent2.GetAverageStrategy());
    }

    public void DisplayActions()
    {
        Console.WriteLine("Actions:");
        foreach (int[] action in actions)
        {
            Utility.DisplayArray(action);
        }
    }
}