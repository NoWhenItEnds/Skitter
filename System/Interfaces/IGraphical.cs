using Godot;

namespace Skitter.Interfaces
{
    /// <summary> Represents that a data object requires a representation within the Godot game world. </summary>
    public interface IGraphical
    {
        /// <summary> Get the data object's position in cell-space. </summary>
        /// <returns> The current position of the data object in cell space. </returns>
        public Vector3I GetPosition();
    }
}
