using ChessRebirth.Models;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessRebirth.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CellViewModel> Cells { get; set; }

        public BoardViewModel()
        {
            Cells = new ObservableCollection<CellViewModel>();
            InitializeCells();
        }


        //Добавление фигур на доску
        private void InitializeCells()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SolidColorBrush cellColor = (i + j) % 2 == 0 ? Brushes.White : Brushes.Gray;
                    var cell = new CellViewModel(i, j, cellColor);

                    if (i == 0 || i == 7)
                    {
                        PieceType pieceType = PieceType.None;

                        switch (j)
                        {
                            case 0:
                            case 7:
                                pieceType = PieceType.Rook;
                                break;
                            case 1:
                            case 6:
                                pieceType = PieceType.Knight;
                                break;
                            case 2:
                            case 5:
                                pieceType = PieceType.Bishop;
                                break;
                            case 3:
                                pieceType = PieceType.Queen;
                                break;
                            case 4:
                                pieceType = PieceType.King;
                                break;
                        }

                        PieceColor pieceColor = i == 0 ? PieceColor.Black : PieceColor.White;
                        Piece piece = new Piece { Type = pieceType, Color = pieceColor, PositionX = i, PositionY = j };
                        cell.Piece = piece;
                    }
                    else if (i == 1)
                    {
                        Piece piece = new Piece { Type = PieceType.Pawn, Color = PieceColor.Black, PositionX = i, PositionY = j };
                        cell.Piece = piece;
                    }
                    else if (i == 6)
                    {
                        Piece piece = new Piece { Type = PieceType.Pawn, Color = PieceColor.White, PositionX = i, PositionY = j };
                        cell.Piece = piece;
                    }

                    Cells.Add(cell);
                }
            }
        }

        public void HighlightValidMoves(List<Move> validMoves)
        {
            ClearHighlights();

            foreach (var move in validMoves)
            {
                var cell = Cells.Single(c => c.X == move.ToX && c.Y == move.ToY);
                cell.IsHighlighted = true;
            }
        }

        public void ClearHighlights()
        {
            foreach (var cell in Cells)
            {
                cell.IsHighlighted = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
