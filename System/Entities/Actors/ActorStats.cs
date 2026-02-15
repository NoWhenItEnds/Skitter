using System.Collections.Generic;
using Skitter.Utilities;

namespace Skitter.Entities.Actors
{
    /// <summary> The attributes, skills, and derived stats the define an actor's abilities. </summary>
    public class ActorStats
    {
        /// <summary> An actor's raw, brute strength. </summary>
        public Stat Strength { get; init; } = new Stat("attribute_strength", 1, 0, 10);

        /// <summary> An actor's physical flexibility and finesse. </summary>
        public Stat Dexterity { get; init; } = new Stat("attribute_dexterity", 1, 0, 10);

        /// <summary> An actor's physical resilience to damage, wear, and injury. </summary>
        public Stat Vigor { get; init; } = new Stat("attribute_vigor", 1, 0, 10);

        /// <summary> An actor's raw intelligence and mental reasoning. </summary>
        public Stat Intellect { get; init; } = new Stat("attribute_intellect", 1, 0, 10);

        /// <summary> An actor's charisma and social influence. </summary>
        public Stat Presence { get; init; } = new Stat("attribute_presence", 1, 0, 10);

        /// <summary> A list of all the skills possessed by the actor. A skill not in this array is considered to be 'untrained'. </summary>
        public HashSet<Stat> Skills { get; init; } = new HashSet<Stat>();


        /// <summary> The actor's physical stamina. </summary>
        public DerivedStat StaminaStat { get; init; }

        /// <summary> How entertained / satisfied the actor is. </summary>
        public DerivedStat EntertainmentStat { get; init; }


        /// <summary> The actor the stats represent. </summary>
        private readonly ActorEntity ACTOR;


        /// <summary> The attributes, skills, and derived stats the define an actor's abilities. </summary>
        /// <param name="actor"> The actor the stats represent. </param>
        public ActorStats(ActorEntity actor)
        {
            ACTOR = actor;

            StaminaStat = new DerivedStat(() => 0, () => Vigor.CurrentValue + 3);
            EntertainmentStat = new DerivedStat(() => 0, () => 10);  // TODO - Start at max.
        }
    }
}
