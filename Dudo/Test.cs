
namespace CFR.Dudo;

public static class Test
{
    public static void Playground()
    {
        TrainDudo();
    }

    public static void TrainDudo()
    {
        int iterations = 1000;
        Game game = new();
        game.Train(iterations);
    }
}