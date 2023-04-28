using ChessRebirth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.MoveGenerators
{
    internal class BishopMoveGenerator : IPieceMoveGenerator
    {
        public override List<Move> GetValidMoves(Piece bishop, bool checkForCheck, int? toX = null, int? toY = null)
        {
            List<Move> validMoves = new List<Move>();
            int[] directions = { -1, 1 };

            foreach (int dx in directions)
            {
                foreach (int dy in directions)
                {
                    AddMovesInDirection(bishop, validMoves, dx, dy, checkForCheck);
                }
            }

            if (toX.HasValue && toY.HasValue)
            {
                validMoves.RemoveAll(move => move.ToX != toX || move.ToY != toY);
            }

            return validMoves;
        }

        private void AddMovesInDirection(Piece bishop, List<Move> validMoves, int dx, int dy, bool checkForCheck)
        {
            for (int step = 1; step < 8 && IsInBounds(bishop.PositionX + step * dx, bishop.PositionY + step * dy); step++)
            {
                int newX = bishop.PositionX + step * dx;
                int newY = bishop.PositionY + step * dy;

                Piece targetPiece = bishop.Board.GetPiece(newX, newY);

                if (targetPiece != null)
                {
                    if (targetPiece.Color != bishop.Color)
                    {
                        Move move = new Move
                        {
                            FromX = bishop.PositionX,
                            FromY = bishop.PositionY,
                            ToX = newX,
                            ToY = newY,
                            TargetPiece = targetPiece
                        };

                        if (!checkForCheck || !IsKingInCheckAfterMove(bishop, newX, newY))
                        {
                            validMoves.Add(move);
                        }
                    }
                    break;
                }
                else
                {
                    Move move = new Move
                    {
                        FromX = bishop.PositionX,
                        FromY = bishop.PositionY,
                        ToX = newX,
                        ToY = newY,
                        TargetPiece = null
                    };

                    if (!checkForCheck || !IsKingInCheckAfterMove(bishop, newX, newY))
                    {
                        validMoves.Add(move);
                    }
                }
            }
        }

    }
}
