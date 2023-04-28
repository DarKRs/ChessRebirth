using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.Models
{
    public class Move
    {
        public int Id { get; set; }
        public int FromX { get; set; }
        public int FromY { get; set; }
        public int ToX { get; set; }
        public int ToY { get; set; }
        public GameHistory GameHistory { get; set; }
        public int GameHistoryId { get; set; }
        public Piece TargetPiece { get; set; }
        public PieceType? PromotionPieceType { get; set; }
    }
}
