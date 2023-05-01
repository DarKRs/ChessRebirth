using ChessRebirth.Models;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.MoveGenerators
{
    internal class QueenMoveGenerator : IPieceMoveGenerator
    {
        public override List<Move> GetValidMoves(Piece queen, bool checkForCheck, int? toX = null, int? toY = null)
        {
            List<Move> validMoves = new List<Move>();

            int[] directionsX = { 1, 1, -1, -1, 1, -1, 0, 0 };
            int[] directionsY = { 1, -1, 1, -1, 0, 0, 1, -1 };

            for (int i = 0; i < directionsX.Length; i++)
            {
                AddMovesInDirection(queen, checkForCheck, validMoves, directionsX[i], directionsY[i], toX, toY);
            }

            return validMoves;
        }

        private void AddMovesInDirection(Piece queen, bool checkForCheck, List<Move> validMoves, int directionX, int directionY, int? toX, int? toY)
        {
            for (int distance = 1; distance < 8; distance++)
            {
                int newX = queen.PositionX + directionX * distance;
                int newY = queen.PositionY + directionY * distance;

                if (!IsInBounds(newX, newY))
                {
                    break;
                }

                Move move = new Move { FromX = queen.PositionX, FromY = queen.PositionY, ToX = newX, ToY = newY, TargetPiece = queen.Board.GetPiece(newX, newY) };

                if (move.TargetPiece != null)
                {
                    if (move.TargetPiece.Color != queen.Color)
                    {
                        if (!checkForCheck || !IsKingInCheckAfterMove(queen, newX, newY, checkForCheck))
                        {
                            validMoves.Add(move);
                        }
                    }

                    break;
                }
                else
                {
                    if (!checkForCheck || !IsKingInCheckAfterMove(queen, newX, newY, checkForCheck))
                    {
                        validMoves.Add(move);
                    }
                }

                if (toX.HasValue && toY.HasValue && newX == toX.Value && newY == toY.Value)
                {
                    break;
                }
            }
        }


    }
}
