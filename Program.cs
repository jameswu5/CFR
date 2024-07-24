
namespace CFR;

public static class Program
{
    public static void Main()
    {
        // TrainRockPaperScissors(2);
        TrainColonelBlotto(1, 3, 3, new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });
    }

    public static void TrainRockPaperScissors(int players, double[]? strategy = null)
    {
        RockPaperScissors.Game game = new();
        int iterations = 1000000;

        switch (players)
        {
            case 1:
                if (strategy == null) throw new ArgumentNullException(nameof(strategy));
                game.TrainOneAgent(iterations, strategy);
                break;
            case 2:
                game.TrainTwoAgents(iterations);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(players));
        }

        game.DisplayStrategies();
    }

    public static void TrainColonelBlotto(int players, int battlefields, int soldiers, double[]? strategy = null)
    {
        ColonelBlotto.Game game = new(battlefields, soldiers);
        int iterations = 1000000;

        switch (players)
        {
            case 1:
                if (strategy == null) throw new ArgumentNullException(nameof(strategy));
                game.TrainOneAgent(iterations, strategy);
                break;
            case 2:
                game.TrainTwoAgents(iterations);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(players));
        }

        game.DisplayActions();
        game.DisplayStrategies();
    }
}