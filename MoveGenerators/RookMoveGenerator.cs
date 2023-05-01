using ChessRebirth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.MoveGenerators
{
    internal class RookMoveGenerator : IPieceMoveGenerator
    {
        public override List<Move> GetValidMoves(Piece rook, bool checkForCheck, int? toX = null, int? toY = null)
        {
            List<Move> validMoves = new List<Move>();

            int[][] directions = new int[][]
            {
        new int[] { 1, 0 },
        new int[] { -1, 0 },
        new int[] { 0, 1 },
        new int[] { 0, -1 }
            };

            foreach (int[] direction in directions)
            {
                int stepX = direction[0];
                int stepY = direction[1];
                int newX = rook.PositionX;
                int newY = rook.PositionY;

                while (true)
                {
                    newX += stepX;
                    newY += stepY;

                    if (!IsInBounds(newX, newY))
                    {
                        break;
                    }

                    Move move = new Move { FromX = rook.PositionX, FromY = rook.PositionY, ToX = newX, ToY = newY, TargetPiece = rook.Board.GetPiece(newX, newY) };

                    if (move.TargetPiece == null || move.TargetPiece.Color != rook.Color)
                    {
                        if (!checkForCheck || !IsKingInCheckAfterMove(rook, newX, newY, checkForCheck))
                        {
                            validMoves.Add(move);
                        }
                    }

                    if (move.TargetPiece != null)
                    {
                        break;
                    }

                    if (toX.HasValue && toY.HasValue && newX == toX.Value && newY == toY.Value)
                    {
                        break;
                    }
                }
            }

            return validMoves;
        }
    }
}
