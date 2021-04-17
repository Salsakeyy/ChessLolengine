using WinEchek.Core;
using WinEchek.Engine;
using WinEchek.IA;
using WinEchek.Model;
using WinEchek.Model.Pieces;
using WinEchek.ModelView;

namespace WinEchek.Game
{
    public class EngineCreator : GameCreator
    {
        public override Mode Mode => Mode.Engine;

        public override Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            IEngine engine = new RealEngine(container);
            PlayerControler whitePlayerControler = new BoardViewPlayerController(boardView);
            PlayerControler lolengineController = new LolengineController(container);
            Player whitePlayer = new Player(Color.White, whitePlayerControler);
            Player blackLolengine = new Player(Color.Black, lolengineController);

            Core.Game game = new Core.Game(engine, whitePlayer, blackLolengine, container, true);

            whitePlayer.Game = game;
            blackLolengine.Game = game;

            whitePlayerControler.Player = whitePlayer;
            lolengineController.Player = blackLolengine;

            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController)whitePlayerControler);

            return game;
        }
    }
}