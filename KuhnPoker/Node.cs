
namespace CFR.KuhnPoker;

public class Node
{
    public const int NumActions = 2;

    public double[] regretSum;
    public double[] strategy;
    public double[] strategySum;

    public Node()
    {
        regretSum   = new double[NumActions];
        strategy    = new double[NumActions];
        strategySum = new double[NumActions];
    }

    private double GetStrategy(double realisationWeight)
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

            strategySum[a] += realisationWeight * strategy[a];
        }

        return strategy;
    }

    public double[] GetAverageStrategy()
    {
        double[] avgStrategy = new double[NumActions];
        double normalizingSum = 0;
        for (int a = 0; a < NumActions; a++)
        {
            normalizingSum += strategySum[a];
        }

        for (int a = 0; a < NumActions; a++)
        {
            if (normalizingSum > 0)
            {
                avgStrategy[a] = strategySum[a] / normalizingSum;
            }
            else
            {
                avgStrategy[a] = 1.0 / NumActions;
            }
        }

        return avgStrategy;
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}