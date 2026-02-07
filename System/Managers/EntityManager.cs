using System;
using System.Collections.Generic;
using Godot;
using Skitter.Entities;
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
    }
}
