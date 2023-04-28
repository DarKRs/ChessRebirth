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
        public override List<Move> GetValidMoves(Piece king, Board board)
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

            // Рокировка
            if (!king.HasMoved)
            {
                // Короткая рокировка
                if (king.Board.GetPiece(king.PositionX + 1, king.PositionY) == null &&
                    king.Board.GetPiece(king.PositionX + 2, king.PositionY) == null)
                {
                    Piece rook = king.Board.GetPiece(king.PositionX + 3, king.PositionY);
                    if (rook != null && rook.Type == PieceType.Rook && rook.Color == king.Color && !rook.HasMoved)
                    {
                        // Проверяем, будет ли король под шахом после рокировки
                        if (!IsKingInCheckAfterMove(king, king.PositionX + 2, king.PositionY))
                        {
                            validMoves.Add(new Move { FromX = king.PositionX, FromY = king.PositionY, ToX = king.PositionX + 2, ToY = king.PositionY, TargetPiece = null });
                        }
                    }
                }

                // Длинная рокировка
                if (king.Board.GetPiece(king.PositionX - 1, king.PositionY) == null &&
                king.Board.GetPiece(king.PositionX - 2, king.PositionY) == null &&
                    king.Board.GetPiece(king.PositionX - 3, king.PositionY) == null)
                {
                    Piece rook = king.Board.GetPiece(king.PositionX - 4, king.PositionY);
                    // Проверяем, будет ли король под шахом после рокировки
                    if (!IsKingInCheckAfterMove(king, king.PositionX - 2, king.PositionY))
                    {
                        validMoves.Add(new Move { FromX = king.PositionX, FromY = king.PositionY, ToX = king.PositionX - 2, ToY = king.PositionY, TargetPiece = null });
                    }
                }
            }

            return validMoves;
        }
    }
}
