using TicTacToe;

Console.Write("Do you want to play a local or online game? (l/o): ");

switch (Console.ReadLine())
{
    case "o":
    {
        Console.WriteLine($"Player {OnlineTicTacToe.Run()} won!");
        break;
    }
    default:
    {
        Console.WriteLine($"Player {LocalTicTacToe.Run()} won!");
        break;
    }
}

Console.ReadKey();