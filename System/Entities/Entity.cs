using System;
using System.Threading.Tasks;
using Godot;
using Skitter.Grid;
using Skitter.Interfaces;
using Skitter.Managers;

namespace Skitter.Entities
{
    /// <summary> A base entity. All things within the game world will be derived from this. </summary>
    public abstract class Entity : IGraphical, IEquatable<Entity>, IDisposable
    {
        /// <summary> A reference to the game manager singleton. </summary>
        protected readonly GameManager GAME_MANAGER = GameManager.Instance;

        /// <summary> A reference to the entity manager singleton. </summary>
        protected readonly EntityManager ENTITY_MANAGER = EntityManager.Instance;


        /// <summary> The grid position within the global grid. </summary>
        protected Vector3I _position;


        /// <summary> A base entity. All things within the game world will be derived from this. </summary>
        public Entity()
        {
            GAME_MANAGER.TurnStart.Subscribe(OnTurnStartAsync);
            GAME_MANAGER.TurnStart.Subscribe(OnTurnProcessAsync);
            GAME_MANAGER.TurnEnd.Subscribe(OnTurnEndAsync);
        }


        /// <summary> Called when a new turn begins. </summary>
        protected virtual async Task OnTurnStartAsync() { }


        /// <summary> Called when its time to act within the game world. </summary>
        protected virtual async Task OnTurnProcessAsync() { }


        /// <summary> Called when the current turn concludes. </summary>
        protected virtual async Task OnTurnEndAsync() { }


        /// <inheritdoc/>
        public Vector3I GetPosition() => _position;


        /// <summary> Attempt to move an entity to a new cell in world space. </summary>
        /// <param name="newPosition"> The new position. </param>
        /// <returns> Whether entity's position was successfully set. </returns>
        public Boolean TrySetPosition(Vector3I newPosition)
        {
            Boolean isSuccessful = false;

            if (ENTITY_MANAGER.TryGetCell(_position, out Cell? oldCell) && oldCell != null)
            {
                if (ENTITY_MANAGER.TryGetCell(newPosition, out Cell? newCell) && newCell != null)
                {
                    if (oldCell.CanRemoveEntity(this) && newCell.CanAddEntity(this))
                    {
                        oldCell.TryRemoveEntity(this);
                        newCell.TryAddEntity(this);
                        _position = newPosition;
                        isSuccessful = true;
                    }
                }
                else
                {
                    GD.PrintErr($"The new position, '{newPosition}', isn't within the world grid.");
                }
            }
            else
            {
                GD.PrintErr($"The old position, '{_position}', isn't within the world grid.");
            }

            return isSuccessful;
        }


        /// <summary> Get the entity's unique identifier. </summary>
        /// <returns> A string representing the entity's unique identifier. </returns>
        public abstract String GetUId();


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(GetUId());


        /// <inheritdoc/>
        public Boolean Equals(Entity? other) => other != null ? GetUId() == other.GetUId() : false;


        /// <inheritdoc/>
        public void Dispose()
        {
            GAME_MANAGER.TurnStart.Unsubscribe(OnTurnStartAsync);
            GAME_MANAGER.TurnStart.Unsubscribe(OnTurnProcessAsync);
            GAME_MANAGER.TurnEnd.Unsubscribe(OnTurnEndAsync);
        }
    }
}
