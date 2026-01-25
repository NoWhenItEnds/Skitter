#nullable disable warnings
using System;
using Administrator.Utilities.Singletons;
using Godot;

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
    }
}
