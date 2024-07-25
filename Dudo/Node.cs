
using System.Text;

namespace CFR.Dudo;

// This is basically the same as in Kuhn Poker so look to merge the two after this
public class Node
{
    public readonly int NumActions;
    public int actionsLeft;
    public int infoID;
    private string history;

    public double[] regretSum;
    public double[] strategy;
    public double[] strategySum;

    public Node(int NumActions, int actionsLeft, int infoID, string history)
    {
        this.NumActions = NumActions;
        this.actionsLeft = actionsLeft;
        this.infoID = infoID;
        this.history = history;

        regretSum   = new double[actionsLeft];
        strategy    = new double[actionsLeft];
        strategySum = new double[actionsLeft];
    }

    public double[] GetStrategy(double realisationWeight)
    {
        double normalizingSum = 0;
        for (int a = 0; a < actionsLeft; a++)
        {
            strategy[a] = regretSum[a] > 0 ? regretSum[a] : 0;
            normalizingSum += strategy[a];
        }

        for (int a = 0; a < actionsLeft; a++)
        {
            if (normalizingSum > 0)
            {
                strategy[a] /= normalizingSum;
            }
            else
            {
                strategy[a] = 1.0 / actionsLeft;
            }

            strategySum[a] += realisationWeight * strategy[a];
        }

        return strategy;
    }

    public double[] GetAverageStrategy()
    {
        double[] avgStrategy = new double[actionsLeft];
        double normalizingSum = 0;
        for (int a = 0; a < actionsLeft; a++)
        {
            normalizingSum += strategySum[a];
        }

        for (int a = 0; a < actionsLeft; a++)
        {
            if (normalizingSum > 0)
            {
                avgStrategy[a] = strategySum[a] / normalizingSum;
            }
            else
            {
                avgStrategy[a] = 1.0 / actionsLeft;
            }
        }

        return avgStrategy;
    }

    public override string ToString()
    {
        double[] strat = new double[NumActions];
        double[] avgStrategy = GetAverageStrategy();
        int roll = infoID >> (NumActions - 1);

        for (int i = 0; i < actionsLeft; i++)
        {
            strat[NumActions - actionsLeft + i] = Math.Round(avgStrategy[i], 3);
        }

        return $"{roll + 1} | {history} | {string.Join(", ", strat)}";
    }
}