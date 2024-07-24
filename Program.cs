
namespace CFR;

public static class Program
{
    public static void Main()
    {
        // TrainRockPaperScissors(2);
        ColonelBlotto.Game game = new(3, 3);
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
}