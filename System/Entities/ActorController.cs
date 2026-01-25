using System;

namespace Skitter.Entities
{
    /// <summary> A controller / brain for an actor entity. </summary>
    public class ActorController : IEquatable<ActorController>
    {
        /// <summary> The actor this controller manipulates. </summary>
        public Actor Actor { get; private set; }

        /// <summary> The representative node for the actor within the game world. </summary>
        public ActorNode? ActorNode { get; private set; } = null;


        /// <summary> A controller / brain for an actor entity. </summary>
        /// <param name="controlledActor"> The actor this controller manipulates. </param>
        public ActorController(Actor controlledActor)
        {
            Actor = controlledActor;
        }


        /// <summary> Set the node representing the actor within the game world. </summary>
        /// <param name="node"> The actor's new node, or a null if one is being removed. </param>
        public void SetActorNode(ActorNode? node)
        {
            ActorNode = node;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(Actor);


        /// <inheritdoc/>
        public Boolean Equals(ActorController? other) => other != null ? Actor == other.Actor : false;
    }
}
