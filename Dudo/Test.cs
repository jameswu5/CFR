
namespace CFR.Dudo;

public static class Test
{
    public static void Playground()
    {
        TrainDudo(2000);
    }

    public static void TrainDudo(int iterations)
    {
        Game game = new();
        game.Train(iterations);
    }
}