#nullable disable warnings
using System;
using System.Collections.Generic;
using System.Linq;
using Administrator.Utilities.Singletons;
using Godot;
using Skitter.Entities;
using Skitter.Entities.Data;
using Skitter.Entities.Nodes;
using Skitter.Utilities;
using Skitter.Utilities.Extensions;

namespace Skitter.Managers
{
    /// <summary> The actor singleton for the game world. </summary>
    public partial class ActorManager : SingletonNode2D<ActorManager>
    {
        /// <summary> How many entity the actor pool should contain. </summary>
        [ExportGroup("Settings")]
        [Export] private Int32 _poolSize = 100;

        /// <summary> The prefab used to represent an actor within the game world. </summary>
        [ExportGroup("Resources")]
        [Export] private PackedScene _actorPrefab;


        /// <summary> The actor entity currently being controlled by the player. </summary>
        public ActorController PlayerController { get; private set; }


        /// <summary> The pool used to manage actor nodes. </summary>
        private ObjectPool<ActorNode> _actorPool;

        /// <summary> An exhaustive list of all the controllers for the actors within the game world. </summary>
        private HashSet<ActorController> _actorControllers;

        /// <summary> The loaded data of all the potential actors within the game world. </summary>
        /// <remarks> While this is initially loaded with templated 'prefabs', save data is then applied to modify them to correctly represent the current game state. </remarks>
        private HashSet<ActorData> _availableActorData;


        /// <inheritdoc/>
        public override void _Ready()
        {
            // Load actor data.
            ActorData[] initialData = JsonExtensions.LoadData<ActorData>("res://Data/EntityData/ActorData");
            _availableActorData = new HashSet<ActorData>(initialData);
            // TODO - Load save data and overwrite on modification.

            _actorControllers = new HashSet<ActorController>();
            _actorPool = new ObjectPool<ActorNode>(this, _actorPrefab, _poolSize);
            foreach (ActorData data in _availableActorData)
            {
                ActorEntity entity = new ActorEntity(data);
                ActorController controller = new ActorController(entity);
                _actorControllers.Add(controller);
                ActorNode node = _actorPool.GetAvailableObject();
                controller.SetActorNode(node);
            }

            // TODO - A better way to determine the player.
            PlayerController = _actorControllers.FirstOrDefault(x => x.Entity.Data.GetUId() == "actordata_janetestington") ?? throw new ArgumentNullException("Unable to find the player actor.");
        }
    }
}
