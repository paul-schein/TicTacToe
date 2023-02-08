using System;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TicTacToe;

public static class OnlineTicTacToe
{
    public const int Port = 42069;

    public static Player Run()
    {
        Console.Write("Are you the host (y/n): ");
        switch (Console.ReadLine())
        {
            case "y":
            {
                return Host();
            }
            default:
            {
                return Client();
            }
        }
    }

    private static Player Client()
    {
        TicTacToeGame game = new TicTacToeGame();
        var client = new TcpClient();

        Console.Write("Enter host IP: ");
        client.Connect(Console.ReadLine() ?? string.Empty, 42069);

        NetworkStream stream = client.GetStream();
        Byte[] bytes = new Byte[2];

        do
        {
            Console.Clear();
            game.PrintField();

            if (game.CurrentPlayer == Player.Two)
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

    private static Player Host()
    {
        TicTacToeGame game = new TicTacToeGame();
        var server = new TcpListener(IPAddress.Parse("127.0.0.1"), Port);
        server.Start();

        Console.WriteLine("Waiting for connection...");
        TcpClient client = server.AcceptTcpClient();
        NetworkStream stream = client.GetStream();
        Byte[] bytes = new Byte[2];

        do
        {
            Console.Clear();
            game.PrintField();

            if (game.CurrentPlayer == Player.One)
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