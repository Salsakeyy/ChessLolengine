using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Backend.Engine;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.View.FlyoutContent;
using Frontend.View.ModelView;
using Frontend.View.Widgets;
using MahApps.Metro.Controls.Dialogs;

namespace Frontend.View
{
    public partial class GameView
    {
        public GameView(MainWindow mainWindow, Backend.Core.Game game, BoardView boardView)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            Game = game;

            game.StateChanged += boardView.GameStateChanged;
            
            game.StateChanged += state =>
            {
                switch (state)
                {
                    case BoardState.BlackCheckMate:
                        _mainWindow.ShowMessageAsync("Game finished", "White wins! Black is check mate.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                    case BoardState.WhiteCheckMate:
                        _mainWindow.ShowMessageAsync("Game finished", "Black wins! White is check mate.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                    case BoardState.BlackPat:
                        _mainWindow.ShowMessageAsync("Pat", "The player black is pat.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                    case BoardState.WhitePat:
                        _mainWindow.ShowMessageAsync("Pat", "The player white is pat.",
                            MessageDialogStyle.AffirmativeAndNegative);
                        break;
                }
            };

            //Création et ajout du contenu du PLS pour cette vue
            var gameViewFlyout = new GameViewFlyout(this);
            _mainWindow.Flyout.Content = gameViewFlyout.Content;
            UcBoardView.Content = boardView;

            game.Container.MoveDone += move =>
            {
                LabelPlayerTurn.Content = move.PieceColor == Color.Black ? Color.White.ToString() : Color.Black.ToString();
            };

            game.Container.MoveUndone += move =>
            {
                LabelPlayerTurn.Content = move.PieceColor == Color.Black ? Color.White.ToString() : Color.Black.ToString();
            };

            HistoryView.Content = new HistoryView(this);
        }

        private readonly MainWindow _mainWindow;
        public Backend.Core.Game Game { get; set; }


        private void ButtonUndo_OnClick(object sender, RoutedEventArgs e) => Game.Undo();
        private void ButtonRedo_OnClick(object sender, RoutedEventArgs e) => Game.Redo();

        #region Flyout

        private void Grid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_mainWindow.Flyout.IsOpen) 
                return;
            _mainWindow.Flyout.IsOpen = false;
        }

        private void ButtonMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (_mainWindow.Flyout.IsOpen) return;
            _mainWindow.Flyout.IsOpen = true;
        }

        public async Task Quit()
        {
            _mainWindow.Flyout.IsOpen = false;

            var result =
                await
                    _mainWindow.ShowMessageAsync("Quit the game",
                        "Are you sure you want to quit? The game will be lost and you wont be able to continue this party of chess.",
                        MessageDialogStyle.AffirmativeAndNegative);

            if (result == MessageDialogResult.Affirmative)
            {
                _mainWindow.Flyout.Content = null;
                _mainWindow.MainControl.Content = new Home(_mainWindow, new Container());
            }
        }

        #endregion
    }
}