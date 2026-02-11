#nullable disable warnings
using System;
using System.Collections.Generic;
using Godot;
using Skitter.Entities;
using Skitter.Entities.Nodes;
using Skitter.Nodes;
using Skitter.Utilities;
using Skitter.Utilities.Singletons;

namespace Skitter.Managers
{
    /// <summary> A manager singleton for nodes within the Godot game world. </summary>
    public partial class WorldManager : SingletonNode2D<WorldManager>
    {
        [ExportGroup("Nodes")]
        [Export] private GameCamera _mainCamera;

        /// <summary> The parent in Godot for actor nodes. </summary>
        [Export] private Node2D _entityNodeParent;


        /// <summary> How many entity the actor pool should contain. </summary>
        [ExportGroup("Settings")]
        [Export] private Int32 _poolSize = 100;

        /// <summary> The cell distance around the camera to render. </summary>
        [Export] private Int32 _viewDistance = 320; // TODO - Fix this, it works on pixels instead of cells? WTF.


        /// <summary> The prefab used to represent an entity within the game world. </summary>
        [ExportGroup("Resources")]
        [Export] private PackedScene _entityPrefab;


        /// <summary> A reference to the entity manager singleton. </summary>
        private EntityManager _entityManager;

        /// <summary> The pool used to manage entity nodes within Godot space. </summary>
        private ObjectPool<EntityNode> _entityPool;

        /// <summary> A helper map of entities that currently have a node representing them in the game world. </summary>
        private Dictionary<Entity, EntityNode> _entityMap = new Dictionary<Entity, EntityNode>();


        /// <inheritdoc/>
        public override void _Ready()
        {
            _entityManager = EntityManager.Instance;

            // Initialise the nodes.
            _entityPool = new ObjectPool<EntityNode>(_entityNodeParent, _entityPrefab, _poolSize);

            // TODO - Move to a dynamic culling system.
            EntityNode playerNode = _entityPool.GetAvailableObject();
            playerNode.Initialise(_entityManager.Player);
        }


        /// <inheritdoc/>
        public override void _Process(Double delta)
        {
            // Ensure camera follows the player.
            // TODO - Instead have it look at the 'look node'.
            if (_entityManager.Player != null)
            {
                Vector3 rawPosition = _entityManager.CalculateRenderPosition(_entityManager.Player.GetPosition());
                _mainCamera.GlobalPosition = new Vector2(rawPosition.X, rawPosition.Y);
            }

            // Cull / spawn nodes around the camera.
            Vector3I cameraCellPosition = _entityManager.CalculateGridPosition(new Vector3(_mainCamera.GlobalPosition.X, _mainCamera.GlobalPosition.Y, 0));  // TODO - Z pulls from current layer level. Do on Vector2 overload for CalculateGridPosition?

            // First check if an entity is not in range of the view.
            foreach (KeyValuePair<Entity, EntityNode> item in _entityMap)
            {
                Int32 distance = item.Key.GetPosition().DistanceSquaredTo(cameraCellPosition);
                if (distance > _viewDistance)
                {
                    item.Value.CleanUp();
                    _entityPool.FreeObject(item.Value);
                    _entityMap.Remove(item.Key);
                }
            }

            // Add new items if they are in range, checking first that they do not have a node already.
            foreach (Entity entity in _entityManager.GetEntities<Entity>())
            {
                Int32 distance = entity.GetPosition().DistanceSquaredTo(cameraCellPosition);
                if (distance <= _viewDistance && !_entityMap.ContainsKey(entity))
                {
                    EntityNode node = _entityPool.GetAvailableObject();
                    node.Initialise(entity);
                    _entityMap.Add(entity, node);
                }
            }
        }
    }
}
