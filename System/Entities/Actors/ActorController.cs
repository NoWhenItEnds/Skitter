using System;
using Skitter.Entities.Nodes;

namespace Skitter.Entities.Actors
{
    /// <summary> A controller / brain for an actor entity. </summary>
    public class ActorController : IEquatable<ActorController>
    {
        /// <summary> The actor entity this controller manipulates. </summary>
        public ActorEntity Entity { get; private set; }

        /// <summary> The representative node for the actor within the game world. </summary>
        public EntityNode? Node { get; private set; } = null;


        /// <summary> A controller / brain for an actor entity. </summary>
        /// <param name="entity"> The actor entity this controller manipulates. </param>
        public ActorController(ActorEntity entity)
        {
            Entity = entity;
        }


        /// <summary> Set the node representing the actor within the game world. </summary>
        /// <param name="node"> The actor's new node, or a null if one is being removed. </param>
        public void SetActorNode(EntityNode? node)
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


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Entity);


        /// <inheritdoc/>
        public Boolean Equals(ActorController? other) => other != null ? Entity == other.Entity : false;
    }
}
