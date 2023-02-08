namespace TicTacToe;

public static class LocalTicTacToe
{
    public static Player Run()
    {
        TicTacToeGame game = new TicTacToeGame(TicTacToeGame.GetRandomPlayer());

        do
        {
            Console.Clear();
            game.PrintField();

            var moveWorked = false;
            do
            {
                moveWorked = game.Move(
                    Util.GetInt($"Player {game.CurrentPlayer} enter the row: ", "ERROR", 1, TicTacToeGame.Size) - 1,
                    Util.GetInt($"Player {game.CurrentPlayer} enter the column: ", "ERROR", 1, TicTacToeGame.Size) - 1);
                if (!moveWorked)
                {
                    Console.WriteLine("ERROR");
                }
            } while (!moveWorked);
        } while (game.Winner == Player.None);

        Console.Clear();
        game.PrintField();

        return game.Winner;
    }
}