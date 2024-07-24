
namespace CFR;

public static class Program
{
    public static void Main()
    {
        TrainColonelBlotto(2, 3, 5);
        TrainRockPaperScissors(2);
    }

    public static void TrainRockPaperScissors(int players, double[]? strategy = null)
    {
        RegretMatching.RockPaperScissors trainer = new();
        int iterations = 1000000;

        switch (players)
        {
            case 1:
                if (strategy == null) throw new ArgumentNullException(nameof(strategy));
                trainer.TrainOneAgent(iterations, strategy);
                break;
            case 2:
                trainer.TrainTwoAgents(iterations);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(players));
        }

        trainer.DisplayStrategies();
    }

    public static void TrainColonelBlotto(int players, int battlefields, int soldiers, double[]? strategy = null)
    {
        RegretMatching.ColonelBlotto trainer = new(battlefields, soldiers);
        int iterations = 1000000;

        switch (players)
        {
            case 1:
                if (strategy == null) throw new ArgumentNullException(nameof(strategy));
                trainer.TrainOneAgent(iterations, strategy);
                break;
            case 2:
                trainer.TrainTwoAgents(iterations);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(players));
        }

        trainer.DisplayStrategies();
    }
}