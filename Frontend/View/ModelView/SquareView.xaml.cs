using System.ComponentModel;
using System.Windows.Controls;
using Backend.Model;

namespace Frontend.View.ModelView
{
    public partial class SquareView
    {
        public SquareView(Square square)
        {
            InitializeComponent();
            Square = square;
            DataContext = this;
            Square.PropertyChanged += SquarePropertyChangeHandler;

            if (square.Piece != null)
                PieceView = new PieceView(square.Piece);

            SetResourceReference(BackgroundProperty,
                (square.X + square.Y)%2 == 0 ? "MahApps.Brushes.Accent" : "MahApps.Brushes.Accent4");

            Grid.SetColumn(this, square.X);
            Grid.SetRow(this, square.Y);
        }

        public PieceView PieceView
        {
            get => UcPieceView.Content as PieceView;
            set => UcPieceView.Content = value;
        }

        public Square Square { get; set; }

        private void SquarePropertyChangeHandler(object sender, PropertyChangedEventArgs e)
        {
            PieceView = Square.Piece != null ? new PieceView(Square.Piece) : null;
            UcPieceView.Content = PieceView;
        }

        public override string ToString() => Square.ToString();
    }
}