using ChessRebirth.Models;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.Services
{
    public class MoveService
    {
        public class MoveHistoryEntry
        {
            public Move Move { get; set; }
            public Piece CapturedPiece { get; set; }
            public PieceColor? CheckStatus { get; set; }
            public PieceColor? CheckmateStatus { get; set; }
        }

        private BoardService _boardService;
        private List<MoveHistoryEntry> _moveHistory;

        public MoveService(BoardService boardService)
        {
            _boardService = boardService;
            _moveHistory = new List<MoveHistoryEntry>();
        }

        public bool MakeMove(Move move)
        {
            // Проверьте, является ли ход допустимым
            var validMoves = _boardService.GetValidMoves(move.TargetPiece);
            if (!validMoves.Any(m => m.ToX == move.ToX && m.ToY == move.ToY))
            {
                return false;
            }

            // Сохраняем состояние доски перед ходом
            Piece capturedPiece = _boardService.Board.Pieces.FirstOrDefault(p => p.PositionX == move.ToX && p.PositionY == move.ToY);
            _moveHistory.Add(new MoveHistoryEntry
            {
                Move = move,
                CapturedPiece = capturedPiece,
                CheckStatus = _boardService.CheckStatus,
                CheckmateStatus = _boardService.CheckmateStatus
            });

            // Удаляем фигуру с позиции назначения, если она существует
            _boardService.Board.RemovePiece(move.ToX, move.ToY);

            // Перемещаем фигуру
            move.TargetPiece.Move(move.ToX, move.ToY);

            // Если ход был с превращением пешки, меняем тип фигуры
            if (move.PromotionPieceType.HasValue)
            {
                move.TargetPiece.Type = move.PromotionPieceType.Value;
            }

            // Обновляем состояние шаха и мат для противоположного цвета
            // Проверяем, является ли capturedPiece равным null перед получением цвета противника
            PieceColor opponentColor = capturedPiece == null ? (move.TargetPiece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White) : (capturedPiece.Color == PieceColor.White ? PieceColor.Black : PieceColor.White);
            move.TargetPiece.HasMoved = true;
            _boardService.UpdateCheckAndMateStatus(opponentColor);

            return true;
        }

        //Отмена хода 
        public void UndoMove(Move move)
        {
            if (_moveHistory.Count == 0)
            {
                throw new InvalidOperationException("There are no moves to undo.");
            }

            MoveHistoryEntry lastMoveEntry = _moveHistory.Last();
            Move lastMove = lastMoveEntry.Move;

            // Возвращаем фигуру на предыдущую позицию
            lastMove.TargetPiece.Move(lastMove.FromX, lastMove.FromY);

            // Восстанавливаем съеденную фигуру, если она была
            if (lastMoveEntry.CapturedPiece != null)
            {
                _boardService.Board.Pieces.Add(lastMoveEntry.CapturedPiece);
            }

            // Восстанавливаем состояние шаха и мат
            _boardService.SetCheckAndMateStatus(lastMoveEntry);

            // Удаляем последний ход из истории
            _moveHistory.RemoveAt(_moveHistory.Count - 1);
        }

        public bool IsValidMove(Move move)
        {
            Piece piece = _boardService.Board.GetPiece(move.FromX, move.FromY);
            if (piece == null) return false;

            List<Move> validMoves = _boardService.GetValidMoves(piece);
            return validMoves.Any(m => m.FromX == move.FromX && m.FromY == move.FromY && m.ToX == move.ToX && m.ToY == move.ToY);
        }

        public bool IsCastlingMove(Move move)
        {
            Piece movedPiece = _boardService.Board.GetPiece(move.FromX, move.FromY);
            Piece targetPiece = _boardService.Board.GetPiece(move.ToX, move.ToY);

            if (movedPiece.Type == PieceType.King && !movedPiece.HasMoved && targetPiece?.Type == PieceType.Rook && !targetPiece.HasMoved)
            {
                int xDifference = Math.Abs(move.FromX - move.ToX);
                int yDifference = Math.Abs(move.FromY - move.ToY);

                if (xDifference == 2 && yDifference == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEnPassantMove(Move move)
        {
            Piece movingPiece = _boardService.Board.GetPiece(move.FromX, move.FromY);
            if (movingPiece.Type != PieceType.Pawn)
            {
                return false;
            }

            int directionY = movingPiece.Color == PieceColor.White ? 1 : -1;
            if (move.ToY != move.FromY + directionY)
            {
                return false;
            }

            int deltaX = Math.Abs(move.ToX - move.FromX);
            if (deltaX != 1)
            {
                return false;
            }

            Piece targetPiece = _boardService.Board.GetPiece(move.ToX, move.ToY);
            if (targetPiece != null)
            {
                return false;
            }

            Piece enemyPawn = _boardService.Board.GetPiece(move.ToX, move.FromY);
            if (enemyPawn == null || enemyPawn.Type != PieceType.Pawn || enemyPawn.Color == movingPiece.Color)
            {
                return false;
            }

            Move lastMove = _moveHistory.Count > 0 ? _moveHistory[_moveHistory.Count - 1].Move : null;
            if (lastMove == null || lastMove.FromY != move.ToY || lastMove.ToY != move.FromY || Math.Abs(lastMove.FromY - lastMove.ToY) != 2)
            {
                return false;
            }

            return true;
        }
    }
}
