
namespace CFR.KuhnPoker;

public class Node
{
    public const int NumActions = 2;

    public string infoSet;
    public double[] regretSum;
    public double[] strategy;
    public double[] strategySum;

    public Node(string infoSet)
    {
        this.infoSet = infoSet;
        regretSum    = new double[NumActions];
        strategy     = new double[NumActions];
        strategySum  = new double[NumActions];
    }

    public double[] GetStrategy(double realisationWeight)
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
        Game.Card card = (Game.Card)int.Parse(infoSet[0].ToString());
        return string.Format("{0} {1,3} | [{2}]", card, infoSet[1..], string.Join("  ", GetAverageStrategy().Select(x => x.ToString("F3"))));
    }
}