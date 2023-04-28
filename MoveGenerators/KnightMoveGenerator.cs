using ChessRebirth.Models;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.MoveGenerators
{
    internal class KnightMoveGenerator : IPieceMoveGenerator
    {
        public override List<Move> GetValidMoves(Piece knight, bool checkForCheck, int? toX = null, int? toY = null)
        {
            List<Move> validMoves = new List<Move>();

            int[] offsetsX = { 2, 1, -1, -2, -2, -1, 1, 2 };
            int[] offsetsY = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int i = 0; i < offsetsX.Length; i++)
            {
                int newX = knight.PositionX + offsetsX[i];
                int newY = knight.PositionY + offsetsY[i];

                if (!IsInBounds(newX, newY))
                {
                    continue;
                }

                Move move = new Move { FromX = knight.PositionX, FromY = knight.PositionY, ToX = newX, ToY = newY, TargetPiece = knight.Board.GetPiece(newX, newY) };

                if (move.TargetPiece == null || move.TargetPiece.Color != knight.Color)
                {
                    if (!checkForCheck || !IsKingInCheckAfterMove(knight, newX, newY))
                    {
                        validMoves.Add(move);
                    }
                }

                if (toX.HasValue && toY.HasValue && newX == toX.Value && newY == toY.Value)
                {
                    break;
                }
            }

            return validMoves;
        }
    }
}
