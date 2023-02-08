using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;

namespace TicTacToe;

public sealed class TicTacToeBoard
{
    private const int Size = 3;
    private const char PlayerOneSymbol = 'O';
    private const char PlayerTwoSymbol = 'X';
    private Player _currentPlayer;
    private readonly Player[,] _field;

    public Player Winner { get; private set; }

    public TicTacToeBoard(Player startingPlayer = Player.One)
    {
        _currentPlayer = startingPlayer;
        _field = CreateField(Size);
    }

    public Player GetWinner()
    {
        bool diagonal0Won = true;
        bool diagonal1Won = true;
        for (int i = 0; i < Size; i++)
        {
            bool wonRow = true;
            bool wonColumn = true;
            for (int j = 0; j < Size; j++)
            {
                if (_field[i, j] != _currentPlayer)
                {
                    wonRow = false;
                }

                if (_field[j, i] != _currentPlayer)
                {
                    wonColumn = false;
                }
            }

            if (wonRow || wonColumn)
            {
                return _currentPlayer;
            }

            if (_field[i, i] != _currentPlayer)
            {
                diagonal0Won = false;
            }
            if (_field[Size - i - 1, Size - i - 1] != _currentPlayer)
            {
                diagonal1Won = false;
            }
        }

        if (diagonal0Won || diagonal1Won)
        {
            return _currentPlayer;
        }

        return Player.None;
    }

    public bool MakeMove(int row, int column)
    {
        if (Winner != Player.None
            || row is < 0 or >= Size 
            || column is < 0 or >= Size)
        {
            return false;
        }

        _field[row, column] = _currentPlayer;

        Winner = GetWinner();

        _currentPlayer = _currentPlayer == Player.One ? Player.Two : Player.One;

        return true;
    }

    public void PrintField()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                Console.Write($" {GetCharFromPlayer(_field[i, j])} ");
                if (j < Size - 1)
                {
                    Console.Write("|");
                }
            }
            Console.WriteLine();
        }
    }

    private Player[,] CreateField(int size)
    {
        Player[,] field = new Player[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                field[i, j] = Player.None;
            }
        }
        return field;
    }

    private char GetCharFromPlayer(Player player)
    {
        return player switch
        {
            Player.None => ' ',
            Player.One => PlayerOneSymbol,
            Player.Two => PlayerTwoSymbol,
            _ => ' '
        };
    }

    public enum Player
    {
        None,
        One,
        Two
    }
}