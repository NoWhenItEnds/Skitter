using System;
using Skitter.Entities.Data;

namespace Skitter.Entities
{
    /// <summary> A base object within the game world. </summary>
    public abstract class Entity<T> : IEquatable<Entity<T>> where T : EntityData
    {
        /// <summary> The unique data used to define the entity. </summary>
        public T EntityData { get; protected set; }


        /// <summary> A base object within the game world. </summary>
        /// <param name="entityData"> The unique data used to define the entity. </param>
        public Entity(T entityData)
        {
            EntityData = entityData;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(EntityData.GetUId());


        /// <inheritdoc/>
        public Boolean Equals(Entity<T>? other) => other != null ? EntityData.GetUId() == other.EntityData.GetUId() : false;
    }
}
