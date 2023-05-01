using ChessRebirth.Models;
using ChessRebirth.Services;
using ChessRebirth.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChessRebirth.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private BoardViewModel _boardViewModel;
        public BoardViewModel BoardViewModel
        {
            get => _boardViewModel;
            set
            {
                _boardViewModel = value;
                OnPropertyChanged(nameof(BoardViewModel));
            }
        }

        private PieceColor _activePlayer;
        public PieceColor ActivePlayer
        {
            get => _activePlayer;
            set
            {
                _activePlayer = value;
                OnPropertyChanged(nameof(ActivePlayer));
            }
        }

        private BoardService _boardService;
        public BoardService BoardService => _boardService;

        private MoveService _moveService;


        public GameViewModel()
        {
            var board = new Board();
            _boardService = new BoardService(board);
            _moveService = new MoveService(_boardService);
            BoardViewModel = new BoardViewModel(_boardService,board);
            ActivePlayer = PieceColor.White;
        }

        public ICommand SelectPieceCommand => new RelayCommand<Piece>(SelectPiece);

        private void SelectPiece(Piece piece)
        {
            // Выбранная фигура принадлежит активному игроку
            if (piece.Color != ActivePlayer)
                return;

            BoardViewModel.ClearHighlights();
            var validMoves = _boardService.GetValidMoves(piece);

            // Подсветка доступных ходов
            BoardViewModel.HighlightValidMoves(validMoves);
            BoardViewModel.SelectedPiece = piece;
        }

        public ICommand MovePieceCommand => new RelayCommand<Point>(MovePiece);

        private void MovePiece(Point destination)
        {
            var selectedPiece = BoardViewModel.SelectedPiece;

            if (selectedPiece == null)
                return;

            var move = new Move
            {
                FromX = selectedPiece.PositionX,
                FromY = selectedPiece.PositionY,
                ToX = (int)destination.X,
                ToY = (int)destination.Y,
                TargetPiece = selectedPiece
            };

            if (_moveService.MakeMove(move))
            {
                // Обновите представление доски и очистите выделение
                BoardViewModel.UpdateBoard();
                BoardViewModel.ClearHighlights();

                // Переключите активного игрока
                ActivePlayer = ActivePlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
