using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Skitter.Grid;
using Skitter.Utilities.Singletons;
using Skitter.Utilities.Types;

namespace Skitter.Managers
{
    /// <summary> A singleton for the main game world. </summary>
    public partial class GameManager : SingletonNode<GameManager>
    {
        /// <summary> How many cells the world contains. </summary>
        [ExportGroup("Settings")]
        [Export] private Vector3I _worldSize = new Vector3I(1000, 1000, 10);

        /// <summary> The pixel size of each cell of the grid. </summary>
        [Export] private Int32 _cellSize = 32;


        /// <summary> An event that is called at the beginning of a new turn. </summary>
        public EventPublisher TurnStart { get; init; } = new EventPublisher();

        /// <summary> An event that signals to listeners its time to act within the game world. </summary>
        /// <remarks> Both the player should provide input and the AI use their controllers. </remarks>
        public EventPublisher TurnProcess { get; init; } = new EventPublisher();

        /// <summary> An event that is called at the end of a complete turn. </summary>
        /// <remarks> This should be used to do a final clean up or check after the actions of the entities. </remarks>
        public EventPublisher TurnEnd { get; init; } = new EventPublisher();


        /// <summary> The current state of the present turn. </summary>
        public TurnState State { get; private set; } = TurnState.NONE;

        /// <summary> The current time in the game world. </summary>
        public DateTime CurrentTime { get; private set; } = DateTime.Now;


        /// <summary> The world grid. Maps all the entities within the world to their cell position. </summary>
        private Dictionary<Vector3I, Cell> _grid = new Dictionary<Vector3I, Cell>();


        /// <inheritdoc/>
        public override void _Ready()
        {
            //  Initialise the grid.
            for (Int32 z = 0; z < _worldSize.Z; z++)
            {
                for (Int32 y = 0; y < _worldSize.Y; y++)
                {
                    for (Int32 x = 0; x < _worldSize.X; x++)
                    {
                        Vector3I position = new Vector3I(x, y, z);
                        _grid.Add(position, new Cell(position));
                    }
                }
            }
        }


        /// <summary> Progress the current turn by first invoking entities to move, then clean up after them. </summary>
        public void ProgressTurn()
        {
            State = TurnState.START;
            Task startTask = Task.Run(() => TurnStart.InvokeAsync());
            startTask.GetAwaiter().GetResult();

            State = TurnState.PROCESS;
            Task processTask = Task.Run(() => TurnProcess.InvokeAsync());
            processTask.GetAwaiter().GetResult();

            State = TurnState.END;
            Task endTask = Task.Run(() => TurnEnd.InvokeAsync());
            endTask.GetAwaiter().GetResult();

            CurrentTime = CurrentTime.AddSeconds(1f);
            GD.Print(CurrentTime.ToLongTimeString());
        }


        /// <summary> Attempt to get a cell within the the world grid. </summary>
        /// <param name="position"> The grid position to retrieve. </param>
        /// <param name="cell"> The returned cell. </param>
        /// <returns> Whether the cell position exists within the world grid. </returns>
        public Boolean TryGetCell(Vector3I position, out Cell? cell)
        {
            Boolean isFound = false;
            cell = null;

            if (_grid.ContainsKey(position))
            {
                isFound = true;
                cell = _grid[position];
            }

            return isFound;
        }


        /// <summary> Get the position of a cell in Godot space for rendering. </summary>
        /// <param name="position"> The cell position. </param>
        /// <returns> The calculated position within Godot-space. </returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Vector3 CalculateRenderPosition(Vector3I position)
        {
            if (TryGetCell(position, out Cell? _))
            {
                return new Vector3(position.X, position.Y, position.Z) * _cellSize;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(position), $"The position, '{position}', isn't within the world grid.");
            }
        }


        /// <summary> Get the cell position of a Godot-space location from the grid. </summary>
        /// <param name="position"> The Godot-space location. </param>
        /// <returns> The calculated position for the nearest cell within the grid. </returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Vector3I CalculateGridPosition(Vector3 position)
        {
            Vector3I cellPosition = new Vector3I((Int32)(position.X / _cellSize), (Int32)(position.Y / _cellSize), (Int32)(position.Z / _cellSize));
            if (TryGetCell(cellPosition, out Cell? _))
            {
                return cellPosition;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(position), $"The position, '{position}', resolves to a cell position, '{cellPosition}', that isn't within the world grid.");
            }
        }
    }


    /// <summary> The possible states of the current turn. </summary>
    public enum TurnState
    {
        NONE,
        START,
        PROCESS,
        END
    }
}
