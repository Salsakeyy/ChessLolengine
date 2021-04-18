using Backend.Model;
using Backend.Model.Pieces;
using Frontend.View.ModelView;

namespace Frontend.Game
{
    public abstract class GameCreator
    {
        public abstract Mode Mode { get; }

        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="boardView">The board view.</param>
        /// <param name="color">The color.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public abstract Backend.Core.Game CreateGame(Container container, BoardView boardView, Color color, GameCreatorParameters parameters);
    }

}