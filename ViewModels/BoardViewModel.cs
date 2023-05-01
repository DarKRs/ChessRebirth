using ChessRebirth.Models;
using ChessRebirth.Services;
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
        private readonly BoardService _boardService;
        public Board Board { get; set; } 
        public Piece SelectedPiece { get; set; }

        public BoardViewModel(BoardService boardService, Board board)
        {
            _boardService = boardService;
            Board = board;
            Cells = new ObservableCollection<CellViewModel>();
            InitializeCells();
        }


        //Добавление фигур на доску
        private void InitializeCells()
        {
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    SolidColorBrush cellColor = (i + j) % 2 == 0 ? Brushes.White : Brushes.Gray;
                    var cell = new CellViewModel(i, j, cellColor);

                    if (j == 0 || j == 7)
                    {
                        PieceType[] pieceTypes = { PieceType.Rook, PieceType.Knight, PieceType.Bishop, PieceType.Queen, PieceType.King, PieceType.Bishop, PieceType.Knight, PieceType.Rook };
                        PieceColor pieceColor = j == 0 ? PieceColor.Black : PieceColor.White;
                        Piece piece = CreatePiece(pieceTypes[i], pieceColor, i, j);
                        cell.Piece = piece;
                    }
                    else if (j == 1 || j == 6)
                    {
                        PieceColor pieceColor = j == 1 ? PieceColor.Black : PieceColor.White;
                        Piece piece = CreatePiece(PieceType.Pawn, pieceColor, i, j);
                        cell.Piece = piece;
                    }

                    Cells.Add(cell);
                }
            }
        }

        private Piece CreatePiece(PieceType type, PieceColor color, int x, int y)
        {
            Piece piece = new Piece { Type = type, Color = color, PositionX = x, PositionY = y };
            piece.Board = Board;
            Board.Pieces.Add(piece);
            return piece;
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

        public void UpdateBoard()
        {
            // Получить текущее состояние доски
            var boardPieces = _boardService.GetBoardState();

            // Обновить ObservableCollection<CellViewModel> в соответствии с текущим состоянием доски
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var cell = Cells.FirstOrDefault(c => c.X == i && c.Y == j);
                    if (cell != null)
                    {
                        // Найдите соответствующую фигуру на доске
                        var piece = boardPieces[i, j];

                        // Установите фигуру для текущей ячейки, если она существует, иначе установите null
                        cell.Piece = piece != null ? piece : null;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
