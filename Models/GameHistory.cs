using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.Models
{
    public class GameHistory
    {
        public int Id { get; set; }
        public Player Player1 { get; set; }
        public int Player1Id { get; set; }
        public Player Player2 { get; set; }
        public int Player2Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<Move> Moves { get; set; }

        public GameHistory()
        {
            Moves = new List<Move>();
        }
    }

}
