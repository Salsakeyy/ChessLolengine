using Backend.Core;
using Backend.Engine;
using Backend.Lolengine;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.View.ModelView;

namespace Frontend.Game
{
    public class EngineCreator : GameCreator
    {
        public override Mode Mode => Mode.Engine;

        public override Backend.Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            IEngine engine = new RealEngine(container);
            PlayerControler whitePlayerControler = new BoardViewPlayerController(boardView);
            PlayerControler lolengineController = new LolengineController(container);
            var whitePlayer = new Player(Color.White, whitePlayerControler);
            var blackLolengine = new Player(Color.Black, lolengineController);

            var game = new Backend.Core.Game(engine, whitePlayer, blackLolengine, container, true);

            whitePlayer.Game = game;
            blackLolengine.Game = game;

            whitePlayerControler.Player = whitePlayer;
            lolengineController.Player = blackLolengine;

            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController)whitePlayerControler);

            return game;
        }
    }
}