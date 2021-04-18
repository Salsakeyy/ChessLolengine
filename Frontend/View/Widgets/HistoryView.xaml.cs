using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Input;
using Backend.Command;
using Backend.Model;
using Frontend.View.ModelView;

namespace Frontend.View.Widgets
{
    public partial class HistoryView
    {
        //TODO the board should adapt to the loaded size
        private readonly Board _board = new Board();
        private readonly BoardView _boardView;
        private readonly HistoryViewConversation _conversation;
        private readonly Backend.Core.Game _game;
        private readonly GameView _gameView;
        private int _lastIndex = -1;
        private readonly ObservableCollection<ICompensableCommand> _moves = new ObservableCollection<ICompensableCommand>();
        private readonly BoardView _realBoardView;

        public HistoryView(GameView gameView)
        {
            InitializeComponent();
            _game = gameView.Game;

            _gameView = gameView;
            _realBoardView = gameView.UcBoardView.Content as BoardView;
            _conversation = new HistoryViewConversation();

            foreach (var command in _game.Container.Moves)
            {
                var momand = command.Copy(_board);
                _conversation.Execute(momand);
                _moves.Add(momand);
            }

            _game.Container.Moves.CollectionChanged += (sender, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            if (args.NewItems[args.NewItems.Count - 1] is ICompensableCommand command)
                            {
                                command = command.Copy(_board);
                                _conversation.Execute(command);
                                _moves.Add(command);
                            }

                            break;
                        }
                    case NotifyCollectionChangedAction.Remove:
                        _moves.RemoveAt(_moves.Count - 1);
                        _conversation.Undo();
                        break;
                }
            };

            _boardView = new BoardView(new Container(_board, _moves));
            ListViewHistory.ItemsSource = _moves;
        }

        private void ListViewHistory_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ReInit();

            _gameView.UcBoardView.Content = _realBoardView;
            _lastIndex = -1;
        }

        private void EventSetter_OnHandler(object sender, MouseEventArgs e)
        {
            var item = (sender as FrameworkElement)?.DataContext;
            var index = ListViewHistory.Items.IndexOf(item ?? 0);

            if (_lastIndex == -1)
            {
                for (var i = 1; i < _moves.Count - index; i++)
                {
                    _conversation.Undo();
                }
            }
            else if (index < _lastIndex)
            {
                for (var i = 0; i < _lastIndex - index; i++)
                {
                    _conversation.Undo();
                }
            }
            else if (index > _lastIndex)
            {
                for (var i = 0; i < index - _lastIndex; i++)
                {
                    _conversation.Redo();
                }
            }

            _lastIndex = index;
        }

        private void ListViewHistory_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _gameView.UcBoardView.Content = _boardView;
        }

        private void ItemListDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as FrameworkElement)?.DataContext;
            var index = ListViewHistory.Items.IndexOf(item ?? 0);

            ReInit();

            var count = _moves.Count;

            _game.Undo(count - index - 1);

            _lastIndex = -1;
        }

        private void ReInit()
        {
            if (_lastIndex == -1 || _lastIndex == _moves.Count - 1)
                return;

            for (var i = 1; i < _moves.Count - _lastIndex; i++)
                _conversation.Redo();
        }
    }
}