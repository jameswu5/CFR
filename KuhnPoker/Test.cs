
namespace CFR.KuhnPoker;

public static class Test
{
    public static void Playground()
    {
        TrainKuhnPoker3();
    }

    public static void TrainKuhnPoker3()
    {
        int iterations = 1000000;
        Game game = new();
        game.Train(iterations);
    }

    public static void TrainKuhnPoker4()
    {
        int iterations = 5000000;
        Game game = new(4);
        game.Train(iterations);
    }
}