#nullable disable warnings
using System;
using Godot;
using Skitter.Entities.Actors;
using Skitter.Managers;

namespace Skitter.Entities.Nodes
{
    /// <summary> A node representing an actor entity within the game world. </summary>
    public partial class EntityNode : Node2D
    {
        /// <summary> The actor's sprite. </summary>
        [ExportGroup("Nodes")]
        [Export] private AnimatedSprite2D _sprite;

        /// <summary> A label to display debug information. </summary>
        [Export] private RichTextLabel _debugLabel;  // TODO - A better solution. Just for debug purposes.


        /// <summary> The data entity this node represents in the game world. </summary>
        private Entity? _entity = null;


        /// <summary> A reference to the entity manager singleton. </summary>
        private EntityManager _entityManager;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _entityManager = EntityManager.Instance;
        }



        /// <summary> Initialise the node. </summary>
        /// <param name="entity"> The data entity this node represents in the game world. </param>
        public void Initialise(Entity entity)
        {
            _entity = entity;
        }


        /// <inheritdoc/>
        public override void _Process(Double delta)
        {
            if (_entity != null)
            {
                Vector3 rawPosition = _entityManager.CalculateRenderPosition(_entity.Position);
                GlobalPosition = new Vector2(rawPosition.X, rawPosition.Y);

                if (_entity is ActorEntity actor)
                {
                    // Debug Shit.
                    _debugLabel.Clear();
                    _debugLabel.AppendText($"Cost: {actor.QueuedAction?.GetCost()}\n");
                    _debugLabel.AppendText($"Position: {actor.Position}\n");
                }
            }
        }


        /// <summary> Perform any clean up needed when unlinking the actor from it. </summary>
        public void CleanUp()
        {
            _entity = null;
        }
    }
}
