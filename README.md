# Counterfactual Regret Minimisation

This project aims to find optimal strategies to simple games numerically. By assigning every terminal game state a score, we can adjust our strategies for each possible information set, and iteratively converge to the **Nash Equilibrium** (a state where no players can gain by deviating from their strategy). For **imperfect information** games (those where some information is unknown to a player), we need to use the concept of **regret**, which is some score representing how much we 'regret' the move we made based on our current strategy, by looking at the other possible moves to see if they lead to more desirable results. Then solving these games becomes a minimisation problem where we try to minimise regret.

## Rock Paper Scissors
The rules of Rock Paper Scissors should be familiar, and it is a good example of an imperfect information game because we do not know what the opponent would play. By using **Regret Matching**, we can see that the optimal strategy would be to pick our three available choices uniformly randomly so that we cannot be exploited. Should our opponent use a weighted strategy, even a seemingly innocuous $(0.36, 0.32, 0.32)$, we can exploit that by playing $(0, 1, 0)$ where our tuples are in the form probability of playing (rock, paper, scissors).

## Colonel Blotto
This is a standard resource-allocation game, where we each have $S$ soldiers to allocate to $N$ battlefields. For example, in the case $N=3, S=5$, $(1, 1, 3)$ is a valid allocation. In this game, the two players allocate their soldiers and then their choices are compared. A player gains a point for every battlefield that contains more soldiers than the other. Whoever has more points by the time all battlefields are compared wins the game.

This game can still be solved using Regret Matching, although there are now ${N + S - 1}\choose {N-1}$ possible choices so it is more complex.

## Kuhn Poker
This is a very simple two-player variant of poker where there are only three cards: Jack, Queen and King. At the start of the game, each player wagers an ante of £1 before being dealt a card which is not shown to their respective opponent. Then the standard betting rules of Poker apply, except there is a fixed size bet of £1.

To start the betting phase, Player 1 has the choice to check or bet £1. If Player 1 checks, the Player 2 has the choice to check back or bet. If Player 1 bets, then Player 2 must call (showdown) or fold (forfeit). If Player 2 bets after Player 1 checks, now Player 1 must call (showdown) or fold (forfeit).

In the case of a showdown (either both players check or put in a further £1), the cards are compared. Whoever has the stronger card takes the pot.

For simplicity, if the player puts £0 we call it Pass, if they put £1 we call it Bet. Therefore checking and folding will be a Pass, and betting and calling will be a Bet.

By using **Counterfactual Regret Matching**, we can converge to a Nash Equilibrium. Here is an example of a strategy that our program converged to after 1 million iterations, which only took a few seconds to run:

```
Average game value: -0.05687986115746378

J     | [0.809  0.191]
J   p | [0.663  0.337]
J   b | [1.000  0.000]
J  pb | [1.000  0.000]
Q     | [1.000  0.000]
Q   p | [1.000  0.000]
Q   b | [0.666  0.334]
Q  pb | [0.471  0.529]
K     | [0.419  0.581]
K   p | [0.000  1.000]
K   b | [0.000  1.000]
K  pb | [0.000  1.000]
```

To interpret this, the first column is the card the player to play holds. Of course, we don't know what the opponent has. The second column shows the history: p for Pass, b for Bet. Thus if the history is of length 0 or 2, it is Player 1 to play. Otherwise (history is of length 1), it is Player 2 to play. The third column shows the probabilities in which you should pass, then bet.

In theory, Player 1 is expected to lose $\frac 1{18}$ pounds each round which agrees with our average game value. We can also observe some interesting elements in this (not necessarily unique) optimal strategy that is observed in more complex variants like the highly populer Texas Hold'em variant. Observe that if you hold a Jack (the weakest card), you should bet a fifth of the time, as a bluff. If you hold it as Player 2 and the opponent checks, you should bluff a third of the time. Otherwise, you would fold as you would expect as it is guaranteed you would lose the hand.

Interestingly, if you hold a King as Player 1, you should only bet three fifths of the time. This is perhaps to bait Player 2, who may hold a Jack, into bluffing in which we can gain an extra pound. If we had bet straight away, Player 2 would've folded holding a jack meaning that we miss out.

The optimal strategy can be found on [Wikipedia](https://en.wikipedia.org/wiki/Kuhn_poker#Optimal_strategy).

## Resources

[Kuhn Poker - Wikipedia](https://en.wikipedia.org/wiki/Kuhn_poker#Optimal_strategy)
[An Introduction to Counterfactual Regret Minimization (Neller, Lanctot)](https://www.ma.imperial.ac.uk/~dturaev/neller-lanctot.pdf)


