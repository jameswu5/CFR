
namespace CFR.KuhnPoker;

public class Kuhn3Players : Game
{
    public Kuhn3Players(int cardsInPlay = 4) : base(cardsInPlay)
    {
        NumOfPlayers = 3;
    }

    public override double CFR(int[] cards, string history, double[] probs)
    {
        double p0 = probs[0];
        double p1 = probs[1];
        double p2 = probs[2];

        int plays = history.Length;
        int player = plays % 3;

        int terminalState = GetTerminal(history);
        if (terminalState != -1)
        {
            int winner = GetWinner(terminalState, cards);
            int pot = GetPot(terminalState);
            int staked = ((terminalState & (1 << (player + 3))) > 0) ? 2 : 1;

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

            util[a] = player switch
            {
                0 => -CFR(cards, nextHistory, new double[] { p0 * strategy[a], p1, p2 }),
                1 => -CFR(cards, nextHistory, new double[] { p0, p1 * strategy[a], p2 }),
                2 => -CFR(cards, nextHistory, new double[] { p0, p1, p2 * strategy[a] }),
                _ => throw new Exception($"Invalid player [{player}]"),
            };
            nodeUtil += strategy[a] * util[a];
        }

        for (int a = 0; a < NumActions; a++)
        {
            double regret = util[a] - nodeUtil;

            node.regretSum[a] += player switch
            {
                0 => p1 * p2 * regret,
                1 => p0 * p2 * regret,
                2 => p0 * p1 * regret,
                _ => throw new Exception($"Invalid player [{player}]"),
            };
        }

        return nodeUtil;
    }

    private static int GetTerminal(string history)
    {
        // code: sssaaa
        // s is a flag for whether the player has staked an extra chip
        // a is a flag for whether a player is still active

        // simulate the game
        bool[] players = new bool[] { true, true, true };
        bool[] staked = new bool[] { false, false, false};
        
        int active = 3;

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
                turn = (turn + 1) % 3;

                if (turn == target)
                {
                    // terminal game state
                    int res = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        if (staked[i])
                        {
                            res |= 1 << (i + 3);
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

    private static int GetWinner(int terminalState, int[] cards)
    {
        int winner = -1;
        int bestCard = -1;
        for (int i = 0; i < 3; i++)
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

    private static int GetPot(int terminalState)
    {
        int pot = 3;
        for (int i = 3; i < 6; i++)
        {
            if ((terminalState & (1 << i)) != 0)
            {
                pot++;
            }
        }
        return pot;
    }
}