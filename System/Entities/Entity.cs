using System;
using Skitter.Entities.Data;

namespace Skitter.Entities
{
    /// <summary> A base entity. All things within the game world will be derived from this. </summary>
    /// <typeparam name="T"> The persistent data object representing the entity's state. </typeparam>
    public abstract class Entity<T> : IEquatable<Entity<T>> where T : EntityData
    {
        /// <summary> The persistent data object representing the entity's state. </summary>
        public T Data { get; init; }


        /// <summary> A base entity. All things within the game world will be derived from this. </summary>
        /// <param name="data"> The persistent data object representing the entity's state. </param>
        public Entity(T data)
        {
            Data = data;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Data);


        /// <inheritdoc/>
        public Boolean Equals(Entity<T>? other) => other != null ? Data == other.Data : false;
    }
}
