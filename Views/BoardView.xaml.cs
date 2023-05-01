using ChessRebirth.Models;
using ChessRebirth.Services;
using ChessRebirth.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChessRebirth.Views
{
    public partial class BoardView : UserControl
    {
        public BoardView()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var grid = sender as Grid;
            var cellViewModel = grid.DataContext as CellViewModel;

            if (cellViewModel != null)
            {
                var mainWindow = Window.GetWindow(this) as MainWindow;

                if (mainWindow != null && mainWindow.DataContext is GameViewModel gameViewModel)
                {
                    if (cellViewModel.Piece != null && !cellViewModel.IsHighlighted)
                    {

                        gameViewModel.SelectPieceCommand.Execute(cellViewModel.Piece);
                    }
                    else if (cellViewModel.IsHighlighted)
                    {
                        gameViewModel.MovePieceCommand.Execute(new Point(cellViewModel.X, cellViewModel.Y));
                    }
                }
            }
        }
    }
}