#nullable disable warnings
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Skitter.Entities;
using Skitter.Entities.Actors;
using Skitter.Utilities.Singletons;

namespace Skitter.Managers
{
    /// <summary> A manager holding and coordinating all the information for entities within the game world. </summary>
    public partial class EntityManager : SingletonNode<EntityManager>
    {
        public ActorEntity Player { get; private set; }


        /// <summary> A collection of all the entities in the game world. </summary>
        /// <remarks> Having a separate set with references to the entities stops us from a more expensive search through all the cells in the grid. </remarks>
        private HashSet<Entity> _entities = new HashSet<Entity>();

        /// <summary> The AI controller used to provide non-player controlled actors with actions. </summary>
        private ActorController _actorController = new ActorController();


        /// <inheritdoc/>
        public override void _Ready()
        {
            // TODO - Test entities.
            Player = new ActorEntity();
            RandomNumberGenerator random = new RandomNumberGenerator();
            for (Int32 i = 0; i < 1000; i++)
            {
                ActorEntity entity = new ActorEntity();
                Vector3I position = new Vector3I(random.RandiRange(0, 100), random.RandiRange(0, 100), 0);
                entity.TrySetPosition(position);
                _entities.Add(entity);
            }
        }


        /// <summary> Get all the entities in the world of the given type. </summary>
        /// <typeparam name="T"> The type of entity to retrieve. </typeparam>
        /// <returns> An immutable array of entities. </returns>
        public T[] GetEntities<T>() where T : Entity => _entities.OfType<T>().ToArray();
    }
}
