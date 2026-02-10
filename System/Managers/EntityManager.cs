#nullable disable warnings
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Skitter.Entities;
using Skitter.Entities.Actors;
using Skitter.Utilities.Singletons;

namespace Skitter.Managers
{
    /// <summary> A manager holding and coordinating all the information for entities within the game world. </summary>
    public partial class EntityManager : SingletonNode<EntityManager>
    {
        /// <summary> How many cells the world contains. </summary>
        [ExportGroup("Settings")]
        [Export] private Vector3I _worldSize = new Vector3I(1000, 1000, 10);

        /// <summary> The pixel size of each cell of the grid. </summary>
        [Export] private Int32 _cellSize = 32;


        public ActorEntity Player { get; private set; }


        /// <summary> The world grid. Maps all the entities within the world to their cell position. </summary>
        private Dictionary<Vector3I, Cell> _grid = new Dictionary<Vector3I, Cell>();

        /// <summary> A collection of all the entities in the game world. </summary>
        /// <remarks> Having a separate set with references to the entities stops us from a more expensive search through all the cells in the grid. </remarks>
        private HashSet<Entity> _entities = new HashSet<Entity>();


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

            // TODO - Test entities.
            Player = new ActorEntity();
            RandomNumberGenerator random = new RandomNumberGenerator();
            for (Int32 i = 0; i < 10; i++)
            {
                ActorEntity entity = new ActorEntity();
                Vector3I position = new Vector3I(random.RandiRange(0, 10), random.RandiRange(0, 10), 0);
                entity.TrySetPosition(position);
                _entities.Add(entity);
            }
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


        /// <summary> Get all the entities in the world of the given type. </summary>
        /// <typeparam name="T"> The type of entity to retrieve. </typeparam>
        /// <returns> An immutable array of entities. </returns>
        public T[] GetEntities<T>() where T : Entity => _entities.OfType<T>().ToArray();
    }
}
