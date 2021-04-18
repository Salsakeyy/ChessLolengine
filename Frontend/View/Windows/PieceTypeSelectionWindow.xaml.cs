using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Backend.Model.Pieces;
using Frontend.View.ModelView;

namespace Frontend.View.Windows
{
    public partial class PieceTypeSelectionWindow
    {
        private UserControl _selectedControl;

        public PieceTypeSelectionWindow(Color color)
        {
            InitializeComponent();
            _selectedControl = null;
            UserControlQueen.Content = new PieceView(new Queen(color));
            UserControlQueen.SetResourceReference(BorderBrushProperty, "MahApps.Brushes.Accent");

            UserControlRook.Content = new PieceView(new Rook(color));
            UserControlRook.SetResourceReference(BorderBrushProperty, "MahApps.Brushes.Accent");

            UserControlBishop.Content = new PieceView(new Bishop(color));
            UserControlBishop.SetResourceReference(BorderBrushProperty, "MahApps.Brushes.Accent");

            UserControlKnight.Content = new PieceView(new Knight(color));
            UserControlKnight.SetResourceReference(BorderBrushProperty, "MahApps.Brushes.Accent");
        }

        public Type ChosenType { get; set; }

        private void ChangeSelectedControl(UserControl uc)
        {
            if (_selectedControl != null)
                _selectedControl.BorderThickness = new Thickness(0);

            _selectedControl = uc;
            _selectedControl.BorderThickness = new Thickness(2);
        }

        private void UserControlQueen_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeSelectedControl(UserControlQueen);
        }

        private void UserControlBishop_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeSelectedControl(UserControlBishop);
        }

        private void UserControlRook_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeSelectedControl(UserControlRook);
        }

        private void UserControlKnight_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ChangeSelectedControl(UserControlKnight);
        }

        private void ButtonValidation_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(_selectedControl.Content is PieceView pv)) 
                return;

            ChosenType = pv.Piece.Type;
            DialogResult = true;
            Close();
        }

        protected override void OnClosing(CancelEventArgs e) => e.Cancel = DialogResult == null;
    }
}