/* This file is copyright © 2022 Dnj.Colab repository authors.

Dnj.Colab content is distributed as free software: you can redistribute it and/or modify it under the terms of the General Public License version 3 as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

Dnj.Colab content is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the General Public License version 3 for more details.

You should have received a copy of the General Public License version 3 along with this repository. If not, see <https://github.com/smaicas-org/Dnj.Colab/blob/dev/LICENSE>. */

using System.Drawing;

namespace BattleshipFieldValidator;

public static class BattlefieldValidator
{
    public static bool Validate(this int[,] field)
    {
        List<Point> checkedPoints = new();
        int shipsOfOne = 0;
        int shipsOfTwo = 0;
        int shipsOfThree = 0;
        int shipsOfFour = 0;

        // O(100)
        for (int i = 0; i < 10; i++)
        for (int j = 0; j < 10; j++)
        {
            if (field[i, j] != 1 || checkedPoints.Contains(new Point(i, j))) continue;

            checkedPoints.Add(new Point(i, j));

            if (CheckDiagonalsAndClose(i, j)) return false;

            if (NotOut(i + 1) && field[i + 1, j] == 1)
            {
                checkedPoints.Add(new Point(i + 1, j));
                if (CheckDiagonal(i + 1, j)) return false;
                if (NotOut(i + 2) && field[i + 2, j] == 1)
                {
                    checkedPoints.Add(new Point(i + 2, j));
                    if (CheckDiagonal(i + 2, j)) return false;
                    if (NotOut(i + 3) && field[i + 3, j] == 1)
                    {
                        checkedPoints.Add(new Point(i + 3, j));
                        if (CheckDiagonal(i + 3, j)) return false;
                        if (NotOut(i + 4))
                            if (field[i + 4, j] == 1)
                                // Ship of 5 -> Bad
                                return false;
                        // Horizontal Ship of 4
                        shipsOfFour++;
                    }
                    else
                    {
                        // Horizontal Ship of 3
                        shipsOfThree++;
                    }
                }
                else
                {
                    // Horizontal Ship of 2
                    shipsOfTwo++;
                }
            }
            else if (NotOut(j + 1) && field[i, j + 1] == 1)
            {
                checkedPoints.Add(new Point(i, j + 1));
                if (CheckDiagonal(i, j + 1)) return false;
                if (NotOut(j + 2) && field[i, j + 2] == 1)
                {
                    checkedPoints.Add(new Point(i, j + 2));
                    if (CheckDiagonal(i, j + 2)) return false;
                    if (NotOut(j + 3) && field[i, j + 3] == 1)
                    {
                        checkedPoints.Add(new Point(i, j + 3));
                        if (CheckDiagonal(i, j + 3)) return false;
                        if (NotOut(j + 4))
                            if (field[i, j + 4] == 1)
                                // Ship of 5 -> Bad
                                return false;
                        // Vertical Ship of 4
                        shipsOfFour++;
                    }
                    else
                    {
                        // Vertical Ship of 3
                        shipsOfThree++;
                    }
                }
                else
                {
                    // Vertical Ship of 2
                    shipsOfTwo++;
                }
            }
            else
            {
                // Ship of 1
                shipsOfOne++;
            }
        }

        return shipsOfFour == 1 && shipsOfThree == 2 && shipsOfTwo == 3 && shipsOfOne == 4;

        static bool NotOut(int i)
        {
            return i is < 10 and > -1;
        }

        // Has 1 in any lower diagonal
        bool CheckDiagonal(int i, int j)
        {
            if (NotOut(i + 1) && NotOut(j + 1) && field[i + 1, j + 1] == 1) return true;
            return NotOut(i - 1) && NotOut(j - 1) && field[i + 1, j - 1] == 1;
        }

        bool CheckDiagonalsAndClose(int i, int j)
        {
            if (CheckDiagonal(i, j)) return true;

            // Has one in more than 1 direction
            return field[i + 1, j] + field[i, j + 1] > 1;
        }
    }
}