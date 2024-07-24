
namespace CFR.ColonelBlotto;

public class Agent
{
    public int NumActions;

    public Agent(int battlefields, int soldiers)
    {
        NumActions = Utility.Choose(soldiers + battlefields - 1, battlefields - 1);
        
    }
}