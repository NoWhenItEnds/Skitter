using Godot;
using System;
using System.Text.Json.Serialization;

namespace Skitter.Entities.Data
{
    /// <summary> Basic persistent data possessed by every entity within the game world. </summary>
    public abstract class EntityData : IEquatable<EntityData>
    {
        /// <summary> The entity's position within the game world. (X, Y) is the position on the floor, Z is the height level. </summary>
        [JsonPropertyName("position")]
        public Vector3 Position { get; set; }


        /// <summary> Get the string used as the data's unique identifier. </summary>
        /// <returns> A string representing the data's unique identifier. </returns>
        public abstract String GetUId();


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(GetUId());


        /// <inheritdoc/>
        public Boolean Equals(EntityData? other) => other != null ? GetUId() == other.GetUId() : false;
    }
}
