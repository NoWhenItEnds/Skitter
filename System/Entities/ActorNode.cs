#nullable disable warnings
using Godot;

namespace Skitter.Entities
{
    /// <summary> A node representing an actor entity within the game world. </summary>
    public partial class ActorNode : Node2D
    {
        /// <summary> The collision shape for the actor node. </summary>
        [ExportGroup("Nodes")]
        [Export] private CollisionShape2D _collisionShape;

        /// <summary> The actor's sprite. </summary>
        [Export] private AnimatedSprite2D _sprite;
    }
}
