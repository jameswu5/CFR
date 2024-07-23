
namespace CFR;

public class Trainer
{
    public enum Move { Rock, Paper, Scissors };
    public const int NumActions = 3;
    public double[] regretSum;
    public double[] strategy;
    public double[] strategySum;

    public double[] opponentStrategy;

    public Trainer()
    {
        regretSum = new double[NumActions];
        strategy = new double[NumActions];
        strategySum = new double[NumActions];

        opponentStrategy = new double[] {0.4, 0.3, 0.3};
    }

    private double[] GetStrategy()
    {
        double normalizingSum = 0;
        for (int a = 0; a < NumActions; a++)
        {
            strategy[a] = regretSum[a] > 0 ? regretSum[a] : 0;
            normalizingSum += strategy[a];
        }
        for (int a = 0; a < NumActions; a++)
        {
            if (normalizingSum > 0)
            {
                strategy[a] /= normalizingSum;
            }
            else
            {
                strategy[a] = 1.0 / NumActions;
            }
            strategySum[a] += strategy[a];
        }
        return strategy;
    }

    private static Move GetAction(double[] strategy)
    {
        double r = new Random().NextDouble();
        double cumulativeProbability = 0;
        int a;
        for (a = 0; a < NumActions - 1; a++)
        {
            cumulativeProbability += strategy[a];
            if (r < cumulativeProbability)
            {
                return (Move) a;
            }
        }
        return (Move) a;
    }

    public void Train(int iterations)
    {
        double[] actionUtility = new double[NumActions];
        for (int i = 0; i < iterations; i++)
        {
            // Get regret-matched mixed-strategy actions
            double[] strategy = GetStrategy();
            Move myAction = GetAction(strategy);
            Move otherAction = GetAction(opponentStrategy);

            // Compute action utilities
            actionUtility[(int) otherAction] = 0;
            actionUtility[(int) otherAction == NumActions - 1 ? 0 : (int) otherAction + 1] = 1;
            actionUtility[(int) otherAction == 0 ? NumActions - 1 : (int) otherAction - 1] = -1;

            // Accumulate action regrets
            for (int a = 0; a < NumActions; a++)
            {
                regretSum[a] += actionUtility[a] - actionUtility[(int) myAction];
            }
        }
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