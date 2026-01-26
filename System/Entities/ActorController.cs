using System;
using Godot;
using Skitter.Entities.Data;

namespace Skitter.Entities
{
    /// <summary> A controller / brain for an actor entity. </summary>
    public class ActorController : IEquatable<ActorController>
    {
        /// <summary> The actor's data this controller manipulates. </summary>
        public ActorData Data { get; private set; }

        /// <summary> The representative node for the actor within the game world. </summary>
        public ActorNode? ActorNode { get; private set; } = null;


        /// <summary> A controller / brain for an actor entity. </summary>
        /// <param name="data"> The actor's data this controller manipulates.  </param>
        public ActorController(ActorData data)
        {
            Data = data;
        }


        /// <summary> Set the node representing the actor within the game world. </summary>
        /// <param name="node"> The actor's new node, or a null if one is being removed. </param>
        public void SetActorNode(ActorNode? node)
        {
            // Check we have a node needing clean up.
            if (ActorNode != null)
            {
                ActorNode.CleanUp();
            }

            // Check if we need to setup the new node.
            ActorNode = node;
            if (ActorNode != null)
            {
                ActorNode.Initialise(Data);
            }
        }


        /// <summary> Attempt to move the actor. </summary>
        /// <param name="direction"> The direction to move. </param>
        /// <param name="collidingEntity"> If there was a collision stopping movement, this is the colliding entity. A null indicates that there wasn't one. </param>
        /// <returns> Whether the movement was successful. </returns>
        public Boolean TryMove(Vector2 direction, out IEntity? collidingEntity)
        {
            Boolean isSuccessful = false;
            collidingEntity = null;

            if (ActorNode != null)   // TODO - A way to move when the node isn't loaded? How to handle position without node? Save data needs it.
            {
                isSuccessful = true;
                ActorNode.Velocity = direction * 100f;
                ActorNode.MoveAndSlide();
            }

            return isSuccessful;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Data);


        /// <inheritdoc/>
        public Boolean Equals(ActorController? other) => other != null ? Data == other.Data : false;
    }
}
