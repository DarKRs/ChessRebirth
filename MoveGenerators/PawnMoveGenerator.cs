using ChessRebirth.Models;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.MoveGenerators
{
    internal class PawnMoveGenerator : IPieceMoveGenerator
    {
        public override List<Move> GetValidMoves(Piece pawn, bool checkForCheck, int? toX = null, int? toY = null)
        {
            List<Move> validMoves = new List<Move>();

            int direction = pawn.Color == PieceColor.White ? -1 : 1;
            int startRank = pawn.Color == PieceColor.White ? 6 : 1;

            // Одиночный ход вперед
            int newX = pawn.PositionX;
            int newY = pawn.PositionY + direction;
            if (IsInBounds(newX, newY) && pawn.Board.GetPiece(newX, newY) == null)
            {
                TryToAddPawnMove(pawn, validMoves, newX, newY, checkForCheck);
            }

            // Двойной ход вперед с начальной позиции
            if (pawn.PositionY == startRank)
            {
                newY = pawn.PositionY + 2 * direction;
                if (IsInBounds(newX, newY) && pawn.Board.GetPiece(newX, newY) == null && pawn.Board.GetPiece(newX, newY - direction) == null)
                {
                    TryToAddPawnMove(pawn, validMoves, newX, newY, checkForCheck);
                }
            }

            // Взятие по диагонали
            for (int dx = -1; dx <= 1; dx += 2)
            {
                newX = pawn.PositionX + dx;
                newY = pawn.PositionY + direction;
                if (IsInBounds(newX, newY))
                {
                    Piece targetPiece = pawn.Board.GetPiece(newX, newY);
                    if (targetPiece != null && targetPiece.Color != pawn.Color)
                    {
                        TryToAddPawnMove(pawn, validMoves, newX, newY, checkForCheck);
                    }

                    // Взятие на проходе
                    Piece enPassantPawn = pawn.Board.GetPiece(newX, newY - direction);
                    if (enPassantPawn != null && enPassantPawn.Type == PieceType.Pawn && enPassantPawn.Color != pawn.Color && enPassantPawn.EnPassantAvailable)
                    {
                        TryToAddPawnMove(pawn, validMoves, newX, newY, checkForCheck);
                    }
                }
            }

            return validMoves;
        }

        private void TryToAddPawnMove(Piece pawn, List<Move> validMoves, int newX, int newY, bool checkForCheck)
        {
            if (!checkForCheck || !IsKingInCheckAfterMove(pawn, newX, newY,checkForCheck))
            {
                Move move = new Move { FromX = pawn.PositionX, FromY = pawn.PositionY, ToX = newX, ToY = newY, TargetPiece = pawn.Board.GetPiece(newX, newY) };

                // Проверка на превращение пешки
                if ((pawn.Color == PieceColor.White && newY == 7) || (pawn.Color == PieceColor.Black && newY == 0))
                {
                    move.PromotionPieceType = PieceType.Queen; // Используйте ферзя как промоцию по умолчанию, или предоставьте выбор игроку
                }

                validMoves.Add(move);
            }
        }
    }
}
