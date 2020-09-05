using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ChessBoardModel
{
    /// <summary>
    /// 
    /// </summary>
    public class Board
    {
        // the size of the board ususally 8x8
        public int Size { get; set; }

        // 2D array of type Cell
        public Cell[,] TheGrid { get; set; }

        // constructor
        public Board(int s)
        {
            // the original board size defined by the perameter s
            Size = s;
            // create a 2D array of type Cell
            TheGrid = new Cell[Size, Size];
            // fill the 2D array with new Cell objects, each with unique row and column coordinates
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    TheGrid[i, j] = new Cell(i, j);
                }
            }
        }

        /// <summary>
        /// mark all the legal moves for a given piece
        /// </summary>
        /// <param name="currentCell"></param>
        /// <param name="chessPiece"></param>
        public void MarkNextLegalMoves(Cell currentCell, string chessPiece)
        {
            // step 1. clear all previous legal moves
            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    TheGrid[i, j].LegalNextMove = false;
                    TheGrid[i, j].CurrentlyOccupied = false;
                }
            }

            // step 2. find all legal moves and mark the Cells as "legal"
            int[] dr;
            int[] dc;
            switch (chessPiece)
            {
                case "Knight":
                    dr = new[] {2, 2, -2, -2, 1, 1, -1, -1};
                    dc = new[] {1, -1, 1, -1, 2, -2, 2, -2};
                    for (int i = 0; i < Size; i++)
                    {
                        if (IsInBounds(currentCell.RowNumber + dr[i], currentCell.ColumnNumber + dc[i]))
                            TheGrid[currentCell.RowNumber + dr[i], currentCell.ColumnNumber + dc[i]].LegalNextMove =
                                true;
                    }
                    break;
                case "King":
                    dr = new[] {1, 1, 1, -1, -1, -1, 0, 0};
                    dc = new[] {-1, 0, 1, -1, 0, 1, 1, -1};
                    for (int i = 0; i < Size; i++)
                    {
                        if (IsInBounds(currentCell.RowNumber + dr[i], currentCell.ColumnNumber + dc[i]))
                            TheGrid[currentCell.RowNumber + dr[i], currentCell.ColumnNumber + dc[i]].LegalNextMove =
                                true;
                    }

                    break;
                case "Rook":
                    MarkHorizontalandVerticalSquares(currentCell);
                    break;
                case "Bishop":
                    MarkDiagonalSquares(currentCell);
                    break;
                case "Queen":
                    MarkHorizontalandVerticalSquares(currentCell);
                    MarkDiagonalSquares(currentCell);
                    break;
                case "Pawn":
                    // Pawn is very tricky it can move forward 1, but if it is onthesecond rank it can move 2 forward,
                    // or if any opposing piece is in front and to the left or right of the pawn, it can move diagonally
                    // Moving "forward" is also ambiguous on our board 
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            TheGrid[currentCell.RowNumber, currentCell.ColumnNumber].CurrentlyOccupied = true;
        }

        private bool IsInBounds(int row, int col)
        {
            return row >= 0 && row <= Size && col >= 0 && col <= Size;
        }

        /// <summary>
        /// Mark horizontalsquares from the current Cell position
        /// </summary>
        /// <param name="currentCell"></param>
        private void MarkHorizontalandVerticalSquares(Cell currentCell)
        {
            for (int i = 0; i < Size; i++)
            {
                TheGrid[currentCell.RowNumber, i].LegalNextMove = true;
                TheGrid[i, currentCell.ColumnNumber].LegalNextMove = true;
            }
        }

        /// <summary>
        /// Mark diagonal squares from current Cell positions
        /// </summary>
        /// <param name="currentCell"></param>
        private void MarkDiagonalSquares(Cell currentCell)
        {
            // go up and to the right until off board
            var nxtRow = currentCell.RowNumber - 1;
            var nxtCol = currentCell.ColumnNumber + 1;

            var isValidMove = IsValidSquare(nxtRow, nxtCol);
            while (isValidMove)
            {
                {
                    TheGrid[nxtRow, nxtCol].LegalNextMove = true;
                }

                // go up and to the left
                nxtRow--;
                nxtCol++;
                isValidMove = IsValidSquare(nxtRow, nxtCol);
            }

            // go up and to the left until off board
            nxtRow = currentCell.RowNumber - 1;
            nxtCol = currentCell.ColumnNumber - 1;
            isValidMove = IsValidSquare(nxtRow, nxtCol);
            ;

            while (isValidMove)
            {
                {
                    TheGrid[nxtRow, nxtCol].LegalNextMove = true;
                }

                // go up and to the left
                nxtRow--;
                nxtCol--;
                isValidMove = IsValidSquare(nxtRow, nxtCol);
            }

            // go down and to the right until off board
            nxtRow = currentCell.RowNumber + 1;
            nxtCol = currentCell.ColumnNumber + 1;
            isValidMove = IsValidSquare(nxtRow, nxtCol);
            ;

            while (isValidMove)
            {
                {
                    TheGrid[nxtRow, nxtCol].LegalNextMove = true;
                }

                // go up and to the left
                nxtRow++;
                nxtCol++;
                isValidMove = IsValidSquare(nxtRow, nxtCol);
            }

            // go dowm and to the left until off board
            nxtRow = currentCell.RowNumber + 1;
            nxtCol = currentCell.ColumnNumber - 1;
            isValidMove = IsValidSquare(nxtRow, nxtCol);
            ;

            while (isValidMove)
            {
                {
                    TheGrid[nxtRow, nxtCol].LegalNextMove = true;
                }

                // go up and to the left
                nxtRow++;
                nxtCol--;
                isValidMove = IsValidSquare(nxtRow, nxtCol);
            }
        }

        /// <summary>
        /// make sure nxtRow and nxtCol is on the board
        /// </summary>
        /// <param name="nxtRow"></param>
        /// <param name="nxtCol"></param>
        /// <returns></returns>
        private bool IsValidSquare(int nxtRow, int nxtCol)
        {
            return nxtRow >= 0 && nxtRow < Size && nxtCol >= 0 && nxtCol < Size;
        }
    }
}