
namespace CFR.KuhnPoker;

public static class Test
{
    public static void Playground()
    {
        // TrainKuhnPoker3();
        TrainKuhn3Players();
    }

    public static void TrainKuhnPoker3()
    {
        int iterations = 1000000;
        Game game = new Kuhn2Players();
        game.Train(iterations);
    }

    public static void TrainKuhnPoker4()
    {
        int iterations = 5000000;
        Game game = new Kuhn2Players(4);
        game.Train(iterations);
    }

    public static void TrainKuhn3Players()
    {
        int iterations = 1000000;
        Game game = new Kuhn3Players();
        game.Train(iterations);
    }
}