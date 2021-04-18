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
        /// Retourne une instance de partie dans le mode de jeu passé en paramètre
        /// </summary>
        /// <param name="mode">Mode de jeu souhaité</param>
        /// <returns>Une partie dans le mode de jeu passé en paramètre</returns>
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
        AI,
        Engine
    }
}