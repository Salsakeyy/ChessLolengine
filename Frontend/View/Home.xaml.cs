using System.IO;
using System.Reflection;
using System.Windows;
using Backend.IO;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.Game;
using Frontend.View.ModelView;
using Microsoft.Win32;

namespace Frontend.View
{
    public partial class Home
    {
        private readonly MainWindow _mainWindow;
        private readonly Container _container;

        public Home(MainWindow mainWindow, Container container)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _container = container;
        }

       
        private void UseSaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            ILoader loader = new BinaryLoader();

            const string directorySaveName = "Save";
            var fullSavePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" +
                               directorySaveName;

            if (!Directory.Exists(fullSavePath))
                Directory.CreateDirectory(fullSavePath);

            var openFileDialog = new OpenFileDialog
            {
                Filter = loader.Filter(),
                InitialDirectory = fullSavePath
            };

            if (openFileDialog.ShowDialog() != true) return;

            var container = loader.Load(openFileDialog.FileName);
            _mainWindow.MainControl.Content = new Home(_mainWindow, container);
    }

        private void Engine_Click(object sender, RoutedEventArgs e)
        {
            var gameFactory = new GameFactory();
            var boardView = new BoardView(new Container());
            var game = gameFactory.CreateGame(Mode.Engine, new Container(), boardView, Color.White, null);

            _mainWindow.MainControl.Content = new GameView(_mainWindow, game, boardView);
        }

        private void TileAiPlay_OnClick(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainControl.Content = new AiOptionSelection(_mainWindow, _container);
        }

        private void LocalGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            var gameFactory = new GameFactory();
            var boardView = new BoardView(_container);
            var game = gameFactory.CreateGame(Mode.Local, _container, boardView, Color.White, null);

            _mainWindow.MainControl.Content = new GameView(_mainWindow, game, boardView);
        }
    }
}