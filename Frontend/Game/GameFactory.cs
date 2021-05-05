using System.Collections.Generic;
using System.Linq;
using Backend.Model;
using Backend.Model.Pieces;
using Frontend.View.ModelView;

namespace Frontend.Game
{
    public class GameFactory
    {
        public List<GameCreator> GameCreators = new List<GameCreator>();

        public GameFactory()
        {
            GameCreators.Add(new LocalGameCreator());
            GameCreators.Add(new AiGameCreator());
            GameCreators.Add(new EngineCreator());
        }

        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="container">The container.</param>
        /// <param name="boardView">The board view.</param>
        /// <param name="color">The color.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The game.</returns>
        public Backend.Core.Game CreateGame(Mode mode, Container container, BoardView boardView, Color color, GameCreatorParameters parameters)
        {
            return GameCreators.FindAll(x => x.Mode == mode).First().CreateGame(container, boardView, color, parameters);
        }
    }


    /// <summary>
    /// Défini les différents mode de jeu possibles
    /// </summary>
    public enum Mode
    {
        Local,
        Ai,
        Engine
    }
}