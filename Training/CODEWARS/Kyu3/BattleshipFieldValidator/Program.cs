/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using BattleshipFieldValidator;

//  Write a method that takes a field for well - known
//  board game "Battleship" as an argument and returns true if it has a valid disposition of ships,
//  false otherwise.Argument is guaranteed to be 10 * 10 two - dimension array.Elements in the array are numbers,
//  0 if the cell is free and 1 if occupied by ship.
//  https://www.codewars.com/kata/52bb6539a4cf1b12d90005b7

int[,] buenField = new int[10, 10]
{
    { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0 },
    { 1, 0, 1, 0, 0, 0, 0, 0, 1, 0 },
    { 1, 0, 1, 0, 1, 1, 1, 0, 1, 0 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
    { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
    { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
};

int[,] malField = new int[10, 10]
{
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 1, 1, 1, 0, 0, 0, 0, 0 },
    { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
};

Console.WriteLine($"Campo válido {malField.Validate()}");