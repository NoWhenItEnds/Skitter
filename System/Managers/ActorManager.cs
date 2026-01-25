#nullable disable warnings
using System;
using System.Collections.Generic;
using Administrator.Utilities.Singletons;
using Godot;
using Skitter.Entities;
using Skitter.Utilities;

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


        /// <summary> The pool used to manage actor nodes. </summary>
        private ObjectPool<ActorNode> _actorPool;

        /// <summary> An exhaustive list of all the controllers for the actors within the game world. </summary>
        private HashSet<ActorController> _actorControllers;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _actorPool = new ObjectPool<ActorNode>(this, _actorPrefab, _poolSize);
            _actorControllers = new HashSet<ActorController>();
        }
    }
}
