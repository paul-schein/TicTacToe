using TicTacToe;

Console.Write("Do you want to play a local or online game? (l/o): ");

switch (Console.ReadLine())
{
    case "o":
    {
        PrintMessage(OnlineTicTacToe.Run());
        break;
    }
    default:
    {
        PrintMessage(LocalTicTacToe.Run());
        break;
    }
}

Console.ReadKey();

void PrintMessage(Player winner)
{
    Console.WriteLine(winner == Player.None 
        ? "No one won!" 
        : $"Player {winner} won!");
}