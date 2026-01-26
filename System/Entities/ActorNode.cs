#nullable disable warnings
using Godot;
using Skitter.Entities.Data;

namespace Skitter.Entities
{
    /// <summary> A node representing an actor entity within the game world. </summary>
    public partial class ActorNode : CharacterBody2D, IEntity
    {
        /// <summary> The collision shape for the actor node. </summary>
        [ExportGroup("Nodes")]
        [Export] private CollisionShape2D _collisionShape;

        /// <summary> The actor's sprite. </summary>
        [Export] private AnimatedSprite2D _sprite;

        /// <summary> A label to display the actor's name. </summary>
        [Export] private RichTextLabel _nameLabel;  // TODO - A better solution. Just for debug purposes.


        /// <summary> Initialise the node by passing it the actor data. </summary>
        /// <param name="data"> A reference to the actor's data the node will represent. </param>
        public void Initialise(ActorData data)
        {
            _nameLabel.Text = $"{data.FirstName} {data.LastName}";
        }


        /// <summary> Perform any clean up needed when unlinking the actor from it. </summary>
        public void CleanUp()
        {

        }
    }
}
