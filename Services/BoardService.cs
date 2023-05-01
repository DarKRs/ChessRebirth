using ChessRebirth.Models;
using ChessRebirth.MoveGenerators;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static ChessRebirth.Services.MoveService;

namespace ChessRebirth.Services
{
    public class BoardService
    {
        public Board Board { get; private set; }
        private Dictionary<PieceType, IPieceMoveGenerator> _moveGenerators;

        public PieceColor? CheckStatus { get; private set; }
        public PieceColor? CheckmateStatus { get; private set; }

        public BoardService(Board board)
        {
            Board = board;
            _moveGenerators = new Dictionary<PieceType, IPieceMoveGenerator>
            {
                { PieceType.King, new KingMoveGenerator() },
                { PieceType.Pawn, new PawnMoveGenerator() },
                { PieceType.Bishop, new BishopMoveGenerator() },
                { PieceType.Queen, new QueenMoveGenerator() },
                { PieceType.Rook, new RookMoveGenerator() },
                { PieceType.Knight, new KnightMoveGenerator() },
            };
        }

        //Проверка наличия шаха для цвета фигур color
        public bool IsCheck(PieceColor color)
        {
            // Найдите короля данного цвета.
            Piece king = Board.Pieces.FirstOrDefault(p => p.Color == color && p.Type == PieceType.King);
            if (king == null)
            {
                throw new InvalidOperationException("King not found on the board");
            }

            // Проверьте, атакует ли какая-либо фигура противоположного цвета короля.
            PieceColor enemyColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
            foreach (Piece enemyPiece in Board.Pieces.Where(p => p.Color == enemyColor))
            {
                List<Move> enemyMoves = GetValidMoves(enemyPiece, false);
                if (enemyMoves.Any(move => move.ToX == king.PositionX && move.ToY == king.PositionY))
                {
                    return true;
                }
            }

            return false;
        }

        // Проверка наличия мата для цвета фигур color
        public bool IsCheckmate(PieceColor color)
        {
            if (!IsCheck(color))
            {
                return false;
            }

            foreach (Piece piece in Board.Pieces.Where(p => p.Color == color))
            {
                List<Move> validMoves = GetValidMoves(piece);
                if (validMoves.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        //Проверка наличия пата для цвета фигур color
        public bool IsStalemate(PieceColor color)
        {
            if (IsCheck(color))
            {
                return false;
            }

            foreach (Piece piece in Board.Pieces.Where(p => p.Color == color))
            {
                List<Move> validMoves = GetValidMoves(piece);
                if (validMoves.Count > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public List<Move> GetValidMoves(Piece piece, bool checkForCheck = true)
        {
            if (_moveGenerators.TryGetValue(piece.Type, out IPieceMoveGenerator moveGenerator))
            {
                return moveGenerator.GetValidMoves(piece, checkForCheck);
            }
            else
            {
                throw new InvalidOperationException($"Move generator for piece type {piece.Type} not found.");
            }
        }

        public Piece[,] GetBoardState()
        {
            var boardState = new Piece[8, 8];

            foreach (var piece in Board.Pieces)
            {
                boardState[piece.PositionX, piece.PositionY] = piece;
            }

            return boardState;
        }

        public void UpdateCheckAndMateStatus(PieceColor color)
        {
            // Проверяем, находится ли король под шахом
            if (IsCheck(color))
            {
                CheckStatus = color;
            }
            else
            {
                CheckStatus = null;
            }

            // Если король находится под шахом, проверяем мат
            if (CheckStatus.HasValue)
            {
                if (IsCheckmate(color))
                {
                    CheckmateStatus = color;
                }
                else
                {
                    CheckmateStatus = null;
                }
            }
            else
            {
                CheckmateStatus = null;
            }
        }

        public void SetCheckAndMateStatus(MoveHistoryEntry moveHistoryEntry)
        {
            CheckStatus = moveHistoryEntry.CheckStatus;
            CheckmateStatus = moveHistoryEntry.CheckmateStatus;
        }

    }
}
