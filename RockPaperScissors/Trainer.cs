
namespace CFR.RockPaperScissors;

public class Agent
{
    public const int NumActions = 3;
    public double[] regretSum;
    public double[] strategy;
    public double[] strategySum;

    public Agent()
    {
        regretSum = new double[NumActions];
        strategy = new double[NumActions];
        strategySum = new double[NumActions];
    }

    public double[] GetStrategy()
    {
        double normalizingSum = 0;
        for (int a = 0; a < NumActions; a++)
        {
            // If it's negative then set to 0
            strategy[a] = regretSum[a] > 0 ? regretSum[a] : 0;

            // Sum all strategy values so we can normalise the sum to be 1
            normalizingSum += strategy[a];
        }
        for (int a = 0; a < NumActions; a++)
        {
            if (normalizingSum > 0) // normalise the strategy
            {
                strategy[a] /= normalizingSum;
            }
            else // if the sum is 0 then choose uniformly
            {
                strategy[a] = 1.0 / NumActions;
            }
            strategySum[a] += strategy[a];
        }
        return strategy;
    }

    public static Game.Move GetAction(double[] strategy)
    {
        double r = new Random().NextDouble();
        double cumulativeProbability = 0;
        int a;
        for (a = 0; a < NumActions - 1; a++)
        {
            cumulativeProbability += strategy[a];
            if (r < cumulativeProbability)
            {
                return (Game.Move) a;
            }
        }
        return (Game.Move) a;
    }

    public double[] GetAverageStrategy()
    {
        double[] avgStrategy = new double[NumActions];
        double normalizingSum = 0;
        for (int i = 0; i < NumActions; i++)
        {
            normalizingSum += strategySum[i];
        }
        for (int i = 0; i < NumActions; i++)
        {
            if (normalizingSum > 0)
            {
                avgStrategy[i] = strategySum[i] / normalizingSum;
            }
            else
            {
                avgStrategy[i] = 1.0 / NumActions;
            }
        }
        return avgStrategy;
    }
}