
namespace CFR;

public abstract class Trainer
{
    public int NumActions;
    public int[,] UtilityTable;
    public Agent agent1;
    public Agent agent2;

    public void TrainOneAgent(int iterations, double[] opponentStrategy)
    {
        if (opponentStrategy.Length != NumActions)
        {
            throw new ArgumentException("Strategy length must match number of actions");
        }

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
        DisplayActions();
        Console.WriteLine("\nStrategies:");
        Console.WriteLine("Player 1:");
        Utility.DisplayArray(agent1.GetAverageStrategy());
        Console.WriteLine("Player 2:");
        Utility.DisplayArray(agent2.GetAverageStrategy());
    }

    public abstract void DisplayActions();
}