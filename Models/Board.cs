using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.Models
{
    public class Board
    {
        public int Id { get; set; }
        public List<Piece> Pieces { get; set; }

        public Board()
        {
            Pieces = new List<Piece>();
        }

        public Piece GetPiece(int x, int y)
        {
            return Pieces.FirstOrDefault(piece => piece.PositionX == x && piece.PositionY == y);
        }

        public void RemovePiece(int x, int y)
        {
            Pieces.RemoveAll(p => p.PositionX == x && p.PositionY == y);
        }

        public Board Clone()
        {
            Board newBoard = new Board();
            newBoard.Pieces = new List<Piece>(Pieces.Count);
            foreach (Piece piece in Pieces)
            {
                newBoard.Pieces.Add(new Piece { Type = piece.Type,Color = piece.Color, PositionX = piece.PositionX, PositionY = piece.PositionY, HasMoved = piece.HasMoved });
            }

            return newBoard;
        }
    }
}
