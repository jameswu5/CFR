
namespace CFR.RockPaperScissors;

public class Game
{
    public enum Move { Rock, Paper, Scissors };
    public const int NumActions = 3;
    
    public Agent agent1;
    public Agent agent2;

    public Game()
    {
        agent1 = new();
        agent2 = new();
    }

    public void TrainTwoAgents(int iterations)
    {
        double[] actionUtility1 = new double[NumActions];
        double[] actionUtility2 = new double[NumActions];
        for (int i = 0; i < iterations; i++)
        {
            Move action1 = Agent.GetAction(agent1.GetStrategy());
            Move action2 = Agent.GetAction(agent2.GetStrategy());

            // Compute action utilities
            actionUtility1[(int) action2] = 0;
            actionUtility1[(int) action2 == NumActions - 1 ? 0 : (int) action2 + 1] = 1;
            actionUtility1[(int) action2 == 0 ? NumActions - 1 : (int) action2 - 1] = -1;

            actionUtility2[(int) action1] = 0;
            actionUtility2[(int) action1 == NumActions - 1 ? 0 : (int) action1 + 1] = 1;
            actionUtility2[(int) action1 == 0 ? NumActions - 1 : (int) action1 - 1] = -1;

            // Accumulate action regrets
            for (int a = 0; a < NumActions; a++)
            {
                agent1.regretSum[a] += actionUtility1[a] - actionUtility1[(int) action1];
                agent2.regretSum[a] += actionUtility2[a] - actionUtility2[(int) action2];
            }
        }
    }

    public void TrainOneAgent(int iterations, double[] opponentStrategy)
    {
        double[] actionUtility = new double[NumActions];
        for (int i = 0; i < iterations; i++)
        {
            Move action1 = Agent.GetAction(agent1.GetStrategy());
            Move action2 = Agent.GetAction(opponentStrategy);

            // Compute action utilities
            actionUtility[(int) action2] = 0;
            actionUtility[(int) action2 == NumActions - 1 ? 0 : (int) action2 + 1] = 1;
            actionUtility[(int) action2 == 0 ? NumActions - 1 : (int) action2 - 1] = -1;

            // Accumulate action regrets
            for (int a = 0; a < NumActions; a++)
            {
                agent1.regretSum[a] += actionUtility[a] - actionUtility[(int) action1];
            }
        }
        agent2.strategySum = opponentStrategy; // As the final strategy will be the fixed strategy
    }

    public void DisplayStrategies()
    {
        Console.WriteLine("Player 1:");
        foreach (double strategy in agent1.GetAverageStrategy())
        {
            Console.Write(strategy);
            Console.Write("\t");
        }
        Console.WriteLine();
        Console.WriteLine("Player 2:");
        foreach (double strategy in agent2.GetAverageStrategy())
        {
            Console.Write(strategy);
            Console.Write("\t");
        }
        Console.WriteLine();
    }
}