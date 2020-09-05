using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ChessBoardModel;

namespace ChessBoardConsoleApp
{
    /// <summary>
    /// Program asks user for a square on a chessboard (default 8x8) then displays all the legal squares 
    /// that piece can move.
    /// </summary>
    class Program
    {
        static Board myBoard = new Board(8);
        static void Main(string[] args)
        {
            // show the empty chess board
            PrintBoard(myBoard);

            // ask the user for a row and column where we will place the piece
            Cell currentCell = null;
            while (currentCell == null)
            {
                currentCell = SetCurrentCell();
            }
            currentCell.CurrentlyOccupied = true;

            // compute all legal moves for that piece
            myBoard.MarkNextLegalMoves(currentCell, GetValidPiece());

            // print the chess board. Use X for the occupied square, use + for legal move, and use . for an empty square.
            PrintBoard(myBoard);

            // wait for user to press enter and then close the program
            Console.Read();
        }

        /// <summary>
        /// Get and validate a legal piece name from the user.
        /// </summary>
        /// <returns>piece</returns>
        private static string GetValidPiece()
        {
            // 1. Prompt user for valid piece name string
            string piece = null;
            bool validPiece = false;
            while (!validPiece)
            {
                Console.WriteLine("Choose a piece from this list - [King, Queen, Bishop, Knight, Rook]?");
                piece = Console.ReadLine();
                validPiece = checkValidPiece(piece);
            }

            return piece;
        }

        /// <summary>
        /// verify piece name string is a legal piece name
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        private static bool checkValidPiece(string piece)
        {
            string[] validNames = {"King", "Queen", "Bishop", "Knight", "Rook"};
            if (validNames.Any(piece.Contains)) return true;
            return false;
        }

        /// <summary>
        /// get row and column numbers from user and return the Cell
        /// * added error checking
        /// </summary>
        /// <returns></returns>
        public static Cell SetCurrentCell()
        {
            var outOfBoundsEx = new Exception("Error: You have selected a square off the board");
            try
            {
                Console.WriteLine("Enter the currrent row number:");
                int currentRow = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                if (currentRow < 0 || currentRow > myBoard.Size)
                    throw outOfBoundsEx;

                Console.WriteLine("Enter the current column number:");
                int currentCol = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                if (currentCol < 0 || currentCol > myBoard.Size)
                    throw outOfBoundsEx;

                myBoard.TheGrid[currentRow, currentCol].CurrentlyOccupied = true;
                return myBoard.TheGrid[currentRow, currentCol];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Print the console chess board. Use X fot the piece, + for valid move, . for empty square
        /// </summary>
        /// <param name="myBoard"></param>
        private static void PrintBoard(Board myBoard)
        {
            for (var i = 0; i < myBoard.Size; i++)
            {
                for (var j = 0; j < myBoard.Size; j++)
                {
                    var c = myBoard.TheGrid[i, j];

                    if (c.CurrentlyOccupied == true)
                    {
                        Console.Write("X");
                    }
                    else if (c.LegalNextMove == true)
                    {
                        Console.Write("+");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("========/");
        }
    }
}
