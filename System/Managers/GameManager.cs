using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Administrator.Utilities.Singletons;
using Godot;
using Skitter.Entities;
using Skitter.Utilities.Types;

namespace Skitter.Managers
{
    /// <summary> A singleton for the main game world. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        /// <summary> An event that is called at the beginning of a new turn. </summary>
        /// <remarks> This should be used to allow entities interact with the world. </remarks>
        public EventPublisher TurnStart { get; init; } = new EventPublisher();

        /// <summary> An event that is called at the end of a complete turn. </summary>
        /// <remarks> This should be used to do a final clean up or check after the actions of the entities. </remarks>
        public EventPublisher TurnEnd { get; init; } = new EventPublisher();


        /// <summary> The world grid. Maps all the entities within the world to their cell position. </summary>
        private Dictionary<Vector3I, List<Entity>> _grid = new Dictionary<Vector3I, List<Entity>>();


        /// <inheritdoc/>
        public override void _Ready()
        {
            //  Initialise the grid.
            for (Int32 z = 0; z < 10; z++)
            {
                for (Int32 y = 0; y < 1000; y++)
                {
                    for (Int32 x = 0; x < 1000; x++)
                    {
                        _grid.Add(new Vector3I(x, y, z), new List<Entity>());
                    }
                }
            }

            _grid[new Vector3I(0, 0, 0)].Add(new ActorEntity());
        }


        /// <summary> Progress the current turn by first invoking entities to move, then clean up after them. </summary>
        public void ProgressTurn()
        {
            Task startTask = Task.Run(() => TurnStart.InvokeAsync());
            startTask.GetAwaiter().GetResult();
            Task endTask = Task.Run(() => TurnEnd.InvokeAsync());
            endTask.GetAwaiter().GetResult();
        }
    }
}
