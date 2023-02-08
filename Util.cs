namespace TicTacToe;

public static class Util
{
    public static int GetInt(string message, string errorMessage, int min, int max)
    {
        int input;
        bool inputValid;
        do
        {
            Console.Write(message);
            inputValid = int.TryParse(Console.ReadLine(), out input)
                         && input >= min && input <= max;
            if (!inputValid)
            {
                Console.WriteLine(errorMessage);
            }
        } while (!inputValid);
        return input;
    }
}