using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Skitter.Entities;

namespace Skitter.Grid
{
    /// <summary> A position within the game world. Holds entities. </summary>
    public class Cell
    {
        public event Action<Cell> CellUpdated = delegate { };

        /// <summary> What kind of ground surface this cell has. </summary>
        public CellKind Kind { get; private set; } = CellKind.NONE;


        /// <summary> The grid position within the global grid. </summary>
        private readonly Vector3I POSITION;


        /// <summary> The entities currently at this position. </summary>
        private HashSet<Entity> _entities = new HashSet<Entity>();


        /// <summary> A position within the game world. Holds entities. </summary>
        /// <param name="position"> The grid position within the global grid. </param>
        public Cell(Vector3I position)
        {
            POSITION = position;
        }


        /// <inheritdoc/>
        public Vector3I GetPosition() => POSITION;


        /// <summary> Get all the entities in a cell of the given type. </summary>
        /// <typeparam name="T"> The type of entity to retrieve. </typeparam>
        /// <returns> An immutable array of entities. </returns>
        public T[] GetEntities<T>() where T : Entity => _entities.OfType<T>().ToArray();


        /// <summary> Get the representative entity to render for the cell on the screen. </summary>
        /// <returns> The 'most important' entity representing the current state of the cell. A null indicates that there are no entities visible. </returns>
        public Entity? GetRenderEntity()
        {
            return _entities.FirstOrDefault() ?? null;
        }


        /// <summary> Attempt to add a new entity to the location. </summary>
        /// <param name="entity"> A reference to the incoming entity. </param>
        /// <returns> Whether the entity was accepted. </returns>
        public Boolean TryAddEntity(Entity entity)
        {
            Boolean canAdd = CanAddEntity(entity);
            if (canAdd)
            {
                _entities.Add(entity);
                CellUpdated.Invoke(this);
            }
            return canAdd;
        }


        /// <summary> Check that an entity CAN be added to this position. </summary>
        /// <param name="entity"> A reference to the incoming entity. </param>
        /// <returns> Whether the entity COULD be added. </returns>
        public Boolean CanAddEntity(Entity entity)
        {
            // TODO - Add cell-specific check based upon the entities that already occupy the location.
            return true;
        }


        /// <summary> Attempt to remove an entity from the location. </summary>
        /// <param name="entity"> A reference to the entity being removed. </param>
        /// <returns> Whether the operation was successful. </returns>
        public Boolean TryRemoveEntity(Entity entity)
        {
            Boolean canRemove = CanRemoveEntity(entity);
            if (canRemove)
            {
                _entities.Remove(entity);
                CellUpdated.Invoke(this);
            }
            return canRemove;
        }


        /// <summary> Check that an entity CAN be removed from this position. </summary>
        /// <param name="entity"> A reference to the outgoing entity. </param>
        /// <returns> Whether the entity COULD be removed. </returns>
        public Boolean CanRemoveEntity(Entity entity)
        {
            // TODO - Add cell-specific check based upon the entities that already occupy the location.
            return true;
        }
    }


    /// <summary> The kinds of surfaces within the game world. </summary>
    public enum CellKind
    {
        NONE,
        GROUND,
        STONE,
        WATER
    }
}
