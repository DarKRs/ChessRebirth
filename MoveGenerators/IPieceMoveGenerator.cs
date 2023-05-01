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

    }
}
