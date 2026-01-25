using Skitter.Entities.Data;

namespace Skitter.Entities
{
    /// <summary> A thinking entity that moves around and interacts with the game world. </summary>
    public class Actor : Entity<ActorData>
    {
        /// <summary> A thinking entity that moves around and interacts with the game world. </summary>
        /// <param name="actorData"> The unique data used to define the actor. </param>
        public Actor(ActorData actorData) : base(actorData)
        {
        }
    }
}
