using Backend.Core;
using Backend.Engine;
using Backend.IA;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.View.ModelView;

namespace Frontend.Game
{
    public class AiGameCreator : GameCreator
    {
        public override Mode Mode => Mode.Ai;

        public override Backend.Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            IEngine engine = new RealEngine(container);
            PlayerControler whitePlayerControler = new BoardViewPlayerController(boardView);
            PlayerControler blackPlayerControler = new UciProcessController(container, parameters.AiSearchType, parameters.AiSkillLevel, parameters.AiSearchValue);
            var whitePlayer = new Player(Color.White, whitePlayerControler);
            var blackPlayer = new Player(Color.Black, blackPlayerControler);

            var game = new Backend.Core.Game(engine, whitePlayer, blackPlayer, container, true);

            whitePlayer.Game = game;
            blackPlayer.Game = game;

            whitePlayerControler.Player = whitePlayer;
            blackPlayerControler.Player = blackPlayer;

            boardView.BoardViewPlayerControllers.Add((BoardViewPlayerController) whitePlayerControler);

            return game;
        }
    }
}