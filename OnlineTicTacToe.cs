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
        TcpClient client = new TcpClient();

        Console.Write("Are you the host? (y/n): ");
        switch (Console.ReadLine())
        {
            case "y":
            {
                player = Player.One;
                var server = new TcpListener(IPAddress.Parse("127.0.0.1"), Port);
                server.Start();

                Console.WriteLine("Waiting for connection...");
                client = server.AcceptTcpClient();
                break;
            }
            default:
            {
                player = Player.Two;
                Console.Write("Enter host IP: ");
                client.Connect(Console.ReadLine() ?? string.Empty, 42069);
                break;
            }
        }
        NetworkStream stream = client.GetStream();

        return Play(stream, player);
    }

    private static Player Play(NetworkStream stream, Player player)
    {
        TicTacToeGame game = new TicTacToeGame(Player.One);
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
                Console.WriteLine("Waiting for opponents move...");
                stream.Read(bytes, 0, bytes.Length);
                game.Move(bytes[0], bytes[1]);
            }
        } while (game.Winner == Player.None);

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