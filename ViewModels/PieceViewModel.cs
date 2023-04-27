using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRebirth.ViewModels
{
    public class PieceViewModel
    {
        public PieceType Type { get; }
        public PieceColor Color { get; }

        public PieceViewModel(PieceType type, PieceColor color)
        {
            Type = type;
            Color = color;
        }
    }
}
