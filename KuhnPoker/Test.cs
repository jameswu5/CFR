
namespace CFR.KuhnPoker;

public static class Test
{
    public static void Playground()
    {
        TrainKuhnPoker();
    }

    public static void TrainKuhnPoker()
    {
        int iterations = 1000000;
        Game game = new();
        game.Train(iterations);
    }
}