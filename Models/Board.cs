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
    }
}
