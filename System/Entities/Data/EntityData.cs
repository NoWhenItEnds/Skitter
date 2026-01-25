using Godot;
using System;

namespace Skitter.Entities.Data
{
    /// <summary> Basic data possessed by every entity within the game world. </summary>
    public abstract partial class EntityData : Resource
    {
        /// <summary> Basic data possessed by every entity within the game world. </summary>
        public EntityData() { }


        /// <summary> Get the string used as the data's unique identifier. </summary>
        /// <returns> A string representing the data's unique identifier. </returns>
        public abstract String GetUId();
    }
}
