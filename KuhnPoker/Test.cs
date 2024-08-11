
namespace CFR.KuhnPoker;

public static class Test
{
    public static void Playground()
    {
        TrainKuhn2Players(3);
        // TrainKuhnNPlayers(2, 3);
    }

    public static void TrainKuhn2Players(int cards)
    {
        int iterations = 1000000;
        Game game = new Kuhn2Players(cards);
        game.Train(iterations);
    }

    public static void TrainKuhnNPlayers(int players, int cards)
    {
        int iterations = 1000000;
        Game game = new KuhnNPlayers(players, cards);
        game.Train(iterations);
    }
}