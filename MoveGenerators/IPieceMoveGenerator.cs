using ChessRebirth.Models;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.MoveGenerators
{
    public abstract class IPieceMoveGenerator
    {
        public abstract List<Move> GetValidMoves(Piece piece, bool checkForCheck, int? toX = null, int? toY = null);

        protected bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        protected bool IsKingInCheckAfterMove(Piece piece, int toX, int toY)
        {
            // Ищем короля этой стороны.
            Piece king = piece.Board.Pieces.FirstOrDefault(p => p.Color == piece.Color && p.Type == PieceType.King);
            if (king == null)
            {
                throw new InvalidOperationException("King not found on the board");
            }

            // Проверяем, находится ли король под атакой.
            PieceColor enemyColor = king.Color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            bool isInCheck = false;

            foreach (Piece enemyPiece in piece.Board.Pieces)
            {
                if (enemyPiece.Color == enemyColor)
                {
                    List<Move> enemyMoves;
                    if (enemyPiece == piece)
                    {
                        enemyMoves = GetValidMoves(enemyPiece, false, toX, toY);
                    }
                    else
                    {
                        enemyMoves = GetValidMoves(enemyPiece, false);
                    }

                    foreach (Move move in enemyMoves)
                    {
                        if (move.ToX == king.PositionX && move.ToY == king.PositionY)
                        {
                            isInCheck = true;
                            break;
                        }
                    }
                }

                if (isInCheck)
                {
                    break;
                }
            }

            return isInCheck;
        }
    }
}
