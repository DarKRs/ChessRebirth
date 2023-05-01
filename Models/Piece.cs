using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessRebirth.Utils;

namespace ChessRebirth.Models
{
    //Шахматная фигура
    public class Piece
    {
        public int Id { get; set; }
        public PieceType Type { get; set; }
        public PieceColor Color { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Board Board { get; set; }
        public int BoardId { get; set; }
        public bool HasMoved { get; set; }
        public bool EnPassantAvailable { get; set; }

        public void Move(int newX, int newY)
        {
            PositionX = newX;
            PositionY = newY;
        }
    }
}
