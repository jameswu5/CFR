
namespace CFR.RegretMatching;

public static class Test
{
    public static void Playground()
    {
        TrainColonelBlotto(2, 3, 5);
        TrainColonelBlotto(1, 3, 3, new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 1});
        TrainRockPaperScissors(2);
        TrainRockPaperScissors(1, new double[] {0.4, 0.3, 0.3});
    }

    public static void TrainRockPaperScissors(int players, double[]? strategy = null)
    {
        RockPaperScissors trainer = new();
        TrainRegretMatching(trainer, players, strategy);
    }

    public static void TrainColonelBlotto(int players, int battlefields, int soldiers, double[]? strategy = null)
    {
        ColonelBlotto trainer = new(battlefields, soldiers);
        TrainRegretMatching(trainer, players, strategy);
    }

    private static void TrainRegretMatching(Trainer trainer, int players, double[]? strategy = null)
    {
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