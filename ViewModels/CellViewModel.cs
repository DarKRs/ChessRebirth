﻿using ChessRebirth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChessRebirth.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        public int X { get; }
        public int Y { get; }
        public SolidColorBrush CellColor { get; }

        private Piece _piece;
        public Piece Piece
        {
            get => _piece;
            set
            {
                _piece = value;
                OnPropertyChanged(nameof(Piece));
            }
        }

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get => _isHighlighted;
            set
            {
                _isHighlighted = value;
                OnPropertyChanged(nameof(IsHighlighted));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CellViewModel(int x, int y, SolidColorBrush cellColor)
        {
            X = x;
            Y = y;
            CellColor = cellColor;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
