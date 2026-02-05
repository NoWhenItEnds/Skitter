using System;

namespace Skitter.Entities
{
    /// <summary> Represents a thinking, controllable entity within the game world. </summary>
    public class ActorEntity : Entity
    {
        /// <summary> Represents a thinking, controllable entity within the game world. </summary>
        public ActorEntity() : base() { }

        /// <inheritdoc/>
        public override String GetUId() => "Test";

    }
}
