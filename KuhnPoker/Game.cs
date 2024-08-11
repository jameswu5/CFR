
using System.Reflection.Metadata.Ecma335;

namespace CFR.KuhnPoker;

public abstract class Game
{
    public enum Move { Pass, Bet };
    public enum Card { J, Q, K, A };
    public const int NumActions = 2;
    public readonly Random random;
    public Dictionary<string, Node> nodeMap;
    public readonly NodeComparer nodeComparer;
    public int cardsInPlay;

    public int NumOfPlayers;

    public Game(int cardsInPlay)
    {
        random = new Random();
        nodeMap = new();
        nodeComparer = new();
        this.cardsInPlay = cardsInPlay;
    }

    public abstract double CFR(int[] cards, string history, double[] probs);

    public void Train(int iterations)
    {
        int[] cards = Enumerable.Range(0, cardsInPlay).ToArray();
        double util = 0;

        double[] ones = Enumerable.Repeat(1.0, NumOfPlayers).ToArray();

        for (int i = 0; i < iterations; i++)
        {
            Utility.Shuffle(cards);
            util += CFR(cards, "", ones);
        }
        Console.WriteLine($"Average game value: {util / iterations}\n");
        
        List<Node> nodes = nodeMap.Values.ToList();
        nodes.Sort(nodeComparer);
        
        foreach (Node n in nodes)
        {
            Console.WriteLine(n);
        }
    }
}