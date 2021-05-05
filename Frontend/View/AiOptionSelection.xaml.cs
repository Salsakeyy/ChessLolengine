using System.Windows;
using System.Windows.Controls;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.Game;
using Frontend.View.ModelView;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for AiOptionSelection.xaml
    /// </summary>
    public partial class AiOptionSelection : UserControl
    {
        private MainWindow _mainWindow;
        private Container _container;

        public AiOptionSelection(MainWindow mainWindow, Container container)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _container = container;
            for (var i = 1; i <= 20; i++)
            {
                ComboBoxLevel.Items.Add(new ComboBoxItem().Content = i);
            }
            for (var i = 0; i <= 42; i++)
            {
                ComboBoxValue.Items.Add(new ComboBoxItem().Content = i);
            }
            ComboBoxLevel.SelectedIndex = 19;
            ComboBoxValue.SelectedIndex = 10;
            ComboBoxSearchMode.SelectedIndex = 0;
        }
        
        private void ComboBoxSearchMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            var selectedIndex = ComboBoxSearchMode.SelectedIndex;

            if (selectedIndex == 0)
            {
                ComboBoxValue.Items.Clear();
                for (var i = 0; i <= 42; i++)
                {
                    ComboBoxValue.Items.Add(new ComboBoxItem().Content = i);
                }
                ComboBoxValue.SelectedIndex = 10;
            }
            else
            {
                ComboBoxValue.Items.Clear();
                for (var i = 500; i <= 5000; i+=500)
                {
                    ComboBoxValue.Items.Add(new ComboBoxItem().Content = i);
                }
                ComboBoxValue.SelectedIndex = 3;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO le controle pour l'antoine
            var gameFactory = new GameFactory();
            var boardView = new BoardView(_container);
            var skillLevel = ComboBoxLevel.SelectedValue as int? ?? 0;
            var searchValue = ComboBoxValue.SelectedValue as int? ?? 0;
            var game = gameFactory.CreateGame(Mode.Ai, _container, boardView, Color.White, new GameCreatorParameters()
            {
                AiSearchType = ComboBoxSearchMode.SelectedIndex == 0 ? "depth" : "movetime",
                AiSearchValue = searchValue,
                AiSkillLevel = skillLevel
            });
            _mainWindow.MainControl.Content = new GameView(_mainWindow, game, boardView);
        }
    }
}
