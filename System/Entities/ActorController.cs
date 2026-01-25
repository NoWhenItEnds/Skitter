using System;

namespace Skitter.Entities
{
    /// <summary> A controller / brain for an actor entity. </summary>
    public class ActorController : IEquatable<ActorController>
    {
        /// <summary> The actor this controller manipulates. </summary>
        public Actor ControlledActor { get; private set; }


        /// <summary> A controller / brain for an actor entity. </summary>
        /// <param name="controlledActor"> The actor this controller manipulates. </param>
        public ActorController(Actor controlledActor)
        {
            ControlledActor = controlledActor;
        }


        /// <inheritdoc/>
        public override Int32 GetHashCode() => HashCode.Combine(ControlledActor);


        /// <inheritdoc/>
        public Boolean Equals(ActorController? other) => other != null ? ControlledActor == other.ControlledActor : false;
    }
}
