using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace TicTacToe;

public static class OnlineTicTacToe
{
    public const int Port = 42069;

    public static Player Run()
    {
        Player player;
        TcpClient client;

        Console.Write("Are you the host? (y/n): ");
        switch (Console.ReadLine())
        {
            case "y":
            {
                player = Player.One;
                var server = new TcpListener(IPAddress.Any, Port);
                server.Start();

                Console.WriteLine("Waiting for connection...");
                client = server.AcceptTcpClient();
                break;
            }
            default:
            {
                player = Player.Two;
                Console.Write("Enter host IP: ");
                client = new TcpClient((Console.ReadLine() ?? string.Empty), Port);
                break;
            }
        }
        NetworkStream stream = client.GetStream();

        Player startingPlayer;
        if (player == Player.One)
        {
            startingPlayer = TicTacToeGame.GetRandomPlayer();
            stream.Write(new byte[] { (byte)startingPlayer });
        }
        else
        {
            byte[] bytes = new byte[1];
            stream.Read(bytes, 0, bytes.Length);
            startingPlayer = (Player)bytes[0];
        }

        return Play(stream, player, startingPlayer);
    }

    private static Player Play(NetworkStream stream, Player player, Player startingPlayer)
    {
        TicTacToeGame game = new TicTacToeGame(startingPlayer);
        Byte[] bytes = new Byte[2];

        do
        {
            Console.Clear();
            game.PrintField();

            if (game.CurrentPlayer == player)
            {
                stream.Write(OwnMove(game));
            }
            else
            {
                Console.WriteLine("Waiting for the opponent's move...");
                stream.Read(bytes, 0, bytes.Length);
                game.Move(bytes[0], bytes[1]);
            }
        } while (!game.GameOver);

        Console.Clear();
        game.PrintField();

        return game.Winner;
    }

    private static byte[] OwnMove(TicTacToeGame game)
    {
        var moveWorked = false;
        int row;
        int column;
        do
        {
            row = Util.GetInt($"Player {game.CurrentPlayer} enter the row: ", "ERROR", 1, TicTacToeGame.Size) - 1;
            column = Util.GetInt($"Player {game.CurrentPlayer} enter the column: ", "ERROR", 1, TicTacToeGame.Size) - 1;
            moveWorked = game.Move(row, column);
            if (!moveWorked)
            {
                Console.WriteLine("ERROR");
            }
        } while (!moveWorked);

        return new byte[] { (byte)row, (byte)column };
    }
}