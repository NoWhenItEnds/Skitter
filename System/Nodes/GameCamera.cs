#nullable disable warnings
using Godot;

namespace Skitter.Nodes
{
    /// <summary> A game world camera that the player uses to view the world. </summary>
    [GlobalClass]
    public partial class GameCamera : Node2D
    {
        /// <summary> The main camera node that actually provides camera functionality. </summary>
        /// <remarks> This is separate from the node to allow for occlusion and shaders. </remarks>
        private Camera2D _mainCamera;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _mainCamera = new Camera2D();
            AddChild(_mainCamera);
        }
    }
}
