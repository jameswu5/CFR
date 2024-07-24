
namespace CFR.RegretMatching;

public class RockPaperScissors : Trainer
{
    public enum Move { Rock, Paper, Scissors };

    public RockPaperScissors() : base()
    {
        NumActions = 3;
        UtilityTable = new int[,] { { 0, -1, 1 }, { 1, 0, -1 }, { -1, 1, 0 } };

        agent1 = new(NumActions);
        agent2 = new(NumActions);
    }

    public override void DisplayActions()
    {
        Console.WriteLine("Actions:");
        for (int i = 0; i < NumActions; i++)
        {
            Console.WriteLine($"{i}: {(Move)i}");
        }
    }
}