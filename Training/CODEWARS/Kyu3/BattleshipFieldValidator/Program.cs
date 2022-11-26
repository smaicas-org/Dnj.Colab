using BattleshipFieldValidator;

//  Write a method that takes a field for well - known
//  board game "Battleship" as an argument and returns true if it has a valid disposition of ships,
//  false otherwise.Argument is guaranteed to be 10 * 10 two - dimension array.Elements in the array are numbers,
//  0 if the cell is free and 1 if occupied by ship.
//  https://www.codewars.com/kata/52bb6539a4cf1b12d90005b7

int[,] buenField = new int[10, 10]
{
    {1, 0, 0, 0, 0, 1, 1, 0, 0, 0},
    {1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
    {1, 0, 1, 0, 1, 1, 1, 0, 1, 0},
    {1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
    {0, 0, 0, 0, 1, 1, 1, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
    {0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
};

int[,] malField = new int[10, 10]
{
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 1, 1, 1, 0, 0, 0, 0, 0},
    {0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0},

};

Console.WriteLine($"Campo válido {malField.Validate()}");

