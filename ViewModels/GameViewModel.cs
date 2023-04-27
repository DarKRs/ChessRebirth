using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public GameViewModel()
        {
            BoardViewModel = new BoardViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
