
namespace CFR;

public static class RockPaperScissors
{
    public static void TrainAgent()
    {
        Trainer trainer = new();
        trainer.Train(1000000);
        foreach (double strategy in trainer.GetAverageStrategy())
        {
            Console.Write(strategy);
            Console.Write("\t");
        }
        Console.WriteLine();
    }
}