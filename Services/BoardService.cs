using ChessRebirth.Models;
using ChessRebirth.MoveGenerators;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.Services
{
    public class BoardService
    {
        public Board Board { get; private set; }
        private Dictionary<PieceType, IPieceMoveGenerator> _moveGenerators;

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

        //Проверку наличия шаха для цвета фигур color
        public bool IsCheck(PieceColor color)
        {

        }

        public bool IsCheckmate(PieceColor color)
        {
            // Реализуйте проверку наличия мата для цвета фигур color
        }

        public bool IsStalemate(PieceColor color)
        {
            // Реализуйте проверку наличия пата для цвета фигур color
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

    }
}
