using System;
using System.Threading.Tasks;
using Godot;
using Skitter.Managers;

namespace Skitter.Entities
{
    /// <summary> A base entity. All things within the game world will be derived from this. </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        /// <summary> A base entity. All things within the game world will be derived from this. </summary>
        public Entity()
        {
            GameManager.Instance.TurnStart.Subscribe(OnTurnStartAsync);
            GameManager.Instance.TurnEnd.Subscribe(OnTurnEndAsync);
        }


        /// <summary> Get the entity's unique identifier. </summary>
        /// <returns> A string representing the entity's unique identifier. </returns>
        public abstract String GetUId();


        /// <summary> Called when a new turn begins. </summary>
        public virtual async Task OnTurnStartAsync() { GD.Print($"Start: {GetUId()}"); }


        /// <summary> Called when the current turn concludes. </summary>
        public virtual async Task OnTurnEndAsync() { GD.Print($"End: {GetUId()}"); }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(GetUId());


        /// <inheritdoc/>
        public Boolean Equals(Entity? other) => other != null ? GetUId() == other.GetUId() : false;
    }
}
