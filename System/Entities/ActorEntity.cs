using Skitter.Entities.Data;

namespace Skitter.Entities
{
    /// <summary> Represents a thinking, controllable entity within the game world. </summary>
    public class ActorEntity : Entity<ActorData>
    {
        /// <summary> Represents a thinking, controllable entity within the game world. </summary>
        /// <param name="data"> The persistent data object representing the actor's state. </param>
        public ActorEntity(ActorData data) : base(data) {}
    }
}
