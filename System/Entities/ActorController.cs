using System;
using Godot;
using Skitter.Entities.Nodes;

namespace Skitter.Entities
{
    /// <summary> A controller / brain for an actor entity. </summary>
    public class ActorController : IEquatable<ActorController>
    {
        /// <summary> The actor entity this controller manipulates. </summary>
        public ActorEntity Entity { get; private set; }

        /// <summary> The representative node for the actor within the game world. </summary>
        public ActorNode? Node { get; private set; } = null;


        /// <summary> A controller / brain for an actor entity. </summary>
        /// <param name="entity"> The actor entity this controller manipulates. </param>
        public ActorController(ActorEntity entity)
        {
            Entity = entity;
        }


        /// <summary> Set the node representing the actor within the game world. </summary>
        /// <param name="node"> The actor's new node, or a null if one is being removed. </param>
        public void SetActorNode(ActorNode? node)
        {
            // Check we have a node needing clean up.
            if (Node != null)
            {
                Node.CleanUp();
            }

            // Check if we need to setup the new node.
            Node = node;
            if (Node != null)
            {
                Node.Initialise(Entity);
            }
        }


        /// <summary> Attempt to move the actor. </summary>
        /// <param name="direction"> The direction to move. </param>
        /// <param name="collidingEntity"> If there was a collision stopping movement, this is the colliding entity. A null indicates that there wasn't one. </param>
        /// <returns> Whether the movement was successful. </returns>
        public Boolean TryMove(Vector2 direction, out IEntityNode? collidingEntity)
        {
            Boolean isSuccessful = false;
            collidingEntity = null;

            if (Node != null)   // TODO - A way to move when the node isn't loaded? How to handle position without node? Save data needs it.
            {
                isSuccessful = true;
                Node.Velocity = direction * 100f;
                Node.MoveAndSlide();
            }

            return isSuccessful;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Entity);


        /// <inheritdoc/>
        public Boolean Equals(ActorController? other) => other != null ? Entity == other.Entity : false;
    }
}
