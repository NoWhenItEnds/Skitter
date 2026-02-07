using System;
using System.Threading.Tasks;
using Godot;
using Skitter.Managers;

namespace Skitter.Entities
{
    /// <summary> A base entity. All things within the game world will be derived from this. </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        /// <summary> The position of the entity in cell-space. </summary>
        public Vector3I Position { get; set; } = Vector3I.Zero;


        /// <summary> A reference to the entity manager singleton. </summary>
        protected readonly EntityManager ENTITY_MANAGER = EntityManager.Instance;


        /// <summary> A base entity. All things within the game world will be derived from this. </summary>
        public Entity()
        {
            GameManager.Instance.TurnStart.Subscribe(OnTurnStartAsync);
            GameManager.Instance.TurnEnd.Subscribe(OnTurnEndAsync);
        }


        /// <summary> Attempt to move an entity to a new cell in world space. </summary>
        /// <param name="newPosition"> The new position. </param>
        /// <returns> Whether the entity was successfully moved. </returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public Boolean TrySetPosition(Vector3I newPosition)
        {
            Boolean isSuccessful = false;

            if (ENTITY_MANAGER.TryGetCell(Position, out Cell? oldCell) && oldCell != null)
            {
                if (ENTITY_MANAGER.TryGetCell(newPosition, out Cell? newCell) && newCell != null)
                {
                    if (oldCell.CanRemoveEntity(this) && newCell.CanAddEntity(this))
                    {
                        oldCell.TryRemoveEntity(this);
                        newCell.TryAddEntity(this);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(newPosition), $"The new position, '{newPosition}', isn't within the world grid.");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Position), $"The old position, '{Position}', isn't within the world grid.");
            }

            return isSuccessful;
        }


        /// <summary> Called when a new turn begins. </summary>
        public virtual async Task OnTurnStartAsync() { }


        /// <summary> Called when the current turn concludes. </summary>
        public virtual async Task OnTurnEndAsync() { }


        /// <summary> Get the entity's unique identifier. </summary>
        /// <returns> A string representing the entity's unique identifier. </returns>
        public abstract String GetUId();


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(GetUId());


        /// <inheritdoc/>
        public Boolean Equals(Entity? other) => other != null ? GetUId() == other.GetUId() : false;
    }
}
