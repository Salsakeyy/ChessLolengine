using Backend.Core;
using Backend.Engine;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.View.ModelView;

namespace Frontend.Game
{
    public class LocalGameCreator : GameCreator
    {
        public override Mode Mode => Mode.Local;

        public override Backend.Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            IEngine engine = new RealEngine(container);
            PlayerControler whitePlayerControler = new BoardViewPlayerController(boardView);
            PlayerControler blackPlayerControler = new BoardViewPlayerController(boardView);
            var whitePlayer = new Player(Color.White, whitePlayerControler);
            var blackPlayer = new Player(Color.Black, blackPlayerControler);

            var game = new Backend.Core.Game(engine, whitePlayer, blackPlayer, container, true);

            whitePlayer.Game = game;
            blackPlayer.Game = game;

            whitePlayerControler.Player = whitePlayer;
            blackPlayerControler.Player = blackPlayer;

            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController) whitePlayerControler);
            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController) blackPlayerControler);

            return game;
        }
    }
}