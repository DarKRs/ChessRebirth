using ChessRebirth.Models;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.MoveGenerators
{
    internal class KingMoveGenerator : IPieceMoveGenerator
    {
        public override List<Move> GetValidMoves(Piece king, bool checkForCheck, int? toX = null, int? toY = null)
        {
            List<Move> validMoves = new List<Move>();

            int[] directionsX = { -1, 0, 1, -1, 1, -1, 0, 1 };
            int[] directionsY = { -1, -1, -1, 0, 0, 1, 1, 1 };

            for (int i = 0; i < directionsX.Length; i++)
            {
                int newX = king.PositionX + directionsX[i];
                int newY = king.PositionY + directionsY[i];

                if (IsInBounds(newX, newY))
                {
                    Piece targetPiece = king.Board.GetPiece(newX, newY);
                    if (targetPiece == null || targetPiece.Color != king.Color)
                    {
                        validMoves.Add(new Move { FromX = king.PositionX, FromY = king.PositionY, ToX = newX, ToY = newY, TargetPiece = targetPiece });
                    }
                }
            }

            if (!king.HasMoved && !IsKingInCheck(king))
            {
                TryToAddCastlingMove(king, validMoves, 1, 3);
                TryToAddCastlingMove(king, validMoves, -1, 4);
            }

            return validMoves;
        }

        //Рокировка
        private void TryToAddCastlingMove(Piece king, List<Move> validMoves, int direction, int rookDistance)
        {
            bool canCastle = true;

            for (int i = 1; i <= rookDistance; i++)
            {
                int x = king.PositionX + direction * i;
                Piece piece = king.Board.GetPiece(x, king.PositionY);

                if (i < rookDistance && piece != null)
                {
                    canCastle = false;
                    break;
                }

                if (i == rookDistance && (piece == null || piece.Type != PieceType.Rook || piece.Color != king.Color || piece.HasMoved))
                {
                    canCastle = false;
                    break;
                }

                // Проверяем шах только для позиций, в которых король окажется после рокировки
                if ((i == 1 || i == 2) && IsKingInCheckAfterMove(king, x, king.PositionY))
                {
                    canCastle = false;
                    break;
                }
            }

            if (canCastle)
            {
                int toX = king.PositionX + 2 * direction;
                validMoves.Add(new Move { FromX = king.PositionX, FromY = king.PositionY, ToX = toX, ToY = king.PositionY, TargetPiece = null });
            }
        }

    }
}
