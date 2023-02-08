﻿namespace TicTacToe;

public sealed class TicTacToeGame
{
    public const int Size = 3;
    private const char PlayerOneSymbol = 'O';
    private const char PlayerTwoSymbol = 'X';
    private readonly Player[,] _field;

    public Player CurrentPlayer { get; private set; }
    public Player Winner { get; private set; }

    public TicTacToeGame(Player startingPlayer = Player.One)
    {
        CurrentPlayer = startingPlayer;
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
                if (_field[i, j] != CurrentPlayer)
                {
                    wonRow = false;
                }

                if (_field[j, i] != CurrentPlayer)
                {
                    wonColumn = false;
                }
            }

            if (wonRow || wonColumn)
            {
                return CurrentPlayer;
            }

            if (_field[i, i] != CurrentPlayer)
            {
                diagonal0Won = false;
            }
            if (_field[i, Size - i - 1] != CurrentPlayer)
            {
                diagonal1Won = false;
            }
        }

        if (diagonal0Won || diagonal1Won)
        {
            return CurrentPlayer;
        }

        return Player.None;
    }

    public bool Move(int row, int column)
    {
        if (Winner != Player.None
            || _field[row, column] != Player.None
            || row is < 0 or >= Size 
            || column is < 0 or >= Size)
        {
            return false;
        }

        _field[row, column] = CurrentPlayer;

        Winner = GetWinner();

        CurrentPlayer = CurrentPlayer == Player.One ? Player.Two : Player.One;

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
}

public enum Player
{
    None,
    One,
    Two
}