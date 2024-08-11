
namespace CFR.KuhnPoker;

public class KuhnNPlayers : Game
{
    public KuhnNPlayers(int NumOfPlayers, int cardsInPlay) : base(cardsInPlay)
    {
        this.NumOfPlayers = NumOfPlayers;
    }

    public override double CFR(int[] cards, string history, double[] probs)
    {
        int plays = history.Length;
        int player = plays % NumOfPlayers;

        int terminalState = GetTerminal(history);
        if (terminalState != -1)
        {
            int winner = GetWinner(terminalState, cards);
            int pot = GetPot(terminalState);
            int staked = ((terminalState & (1 << (player + NumOfPlayers))) > 0) ? 2 : 1;

            return (winner == player) ? pot - staked : -staked;
        }

        string infoSet = $"{cards[player]}{history}";
        if (!nodeMap.ContainsKey(infoSet))
        {
            Node newNode = new(infoSet);
            nodeMap[infoSet] = newNode;
        }

        Node node = nodeMap[infoSet];

        double[] strategy = node.GetStrategy(probs[player]);
        double[] util = new double[NumActions];
        double nodeUtil = 0;
        for (int a = 0; a < NumActions; a++)
        {
            string nextHistory = history + (a == 0 ? "p" : "b");

            double[] nextProbs = new double[NumOfPlayers];
            Array.Copy(probs, nextProbs, NumOfPlayers);
            nextProbs[player] *= strategy[a];
            util[a] = -CFR(cards, nextHistory, nextProbs);

            nodeUtil += strategy[a] * util[a];
        }

        double probsProduct = 1;
        for (int i = 0; i < NumOfPlayers; i++)
        {
            probsProduct *= probs[i];
        }

        for (int a = 0; a < NumActions; a++)
        {
            double regret = util[a] - nodeUtil;

            node.regretSum[a] += probsProduct * regret / probs[player];
        }

        return nodeUtil;
    }

    public int GetTerminal(string history)
    {
        // code: s__sa__a
        // s is a flag for whether the player has staked an extra chip
        // a is a flag for whether a player is still active

        // simulate the game
        bool[] players = Enumerable.Repeat(true, NumOfPlayers).ToArray();
        bool[] staked = Enumerable.Repeat(false, NumOfPlayers).ToArray();
        
        int active = NumOfPlayers;

        int turn = 0;
        int target = 0;
        bool bet = false;
        foreach (char action in history)
        {
            if (action == 'p')
            {
                // fold
                if (bet)
                {
                    players[turn] = false;
                    active--;
                }

                // else check: do nothing but increment turn
            }
            if (action == 'b')
            {
                staked[turn] = true;

                if (!bet)
                {
                    // raise
                    bet = true;
                    target = turn;
                }

                // else call: do nothing but increment turn
            }

            // increment turn
            do {
                turn = (turn + 1) % NumOfPlayers;

                if (turn == target)
                {
                    // terminal game state
                    int res = 0;
                    for (int i = 0; i < NumOfPlayers; i++)
                    {
                        if (staked[i])
                        {
                            res |= 1 << (i + NumOfPlayers);
                        }

                        if (players[i])
                        {
                            res |= 1 << i;
                        }
                    }
                    return res;
                }

            } while (!players[turn]);
        }

        // not terminal
        return -1;
    }

    private int GetWinner(int terminalState, int[] cards)
    {
        int winner = -1;
        int bestCard = -1;
        for (int i = 0; i < NumOfPlayers; i++)
        {
            // check if still active
            if ((terminalState & (1 << i)) != 0)
            {
                if (cards[i] > bestCard)
                {
                    bestCard = cards[i];
                    winner = i;
                }
            }
        }
        return winner;
    }

    private int GetPot(int terminalState)
    {
        int pot = NumOfPlayers;
        for (int i = NumOfPlayers; i < NumOfPlayers * 2; i++)
        {
            if ((terminalState & (1 << i)) != 0)
            {
                pot++;
            }
        }
        return pot;
    }
}