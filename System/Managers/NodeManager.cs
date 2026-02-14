#nullable disable warnings
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Skitter.Entities;
using Skitter.Entities.Nodes;
using Skitter.Grid;
using Skitter.Nodes;
using Skitter.Utilities;
using Skitter.Utilities.Singletons;

namespace Skitter.Managers
{
    /// <summary> A manager singleton for nodes within the Godot game world. </summary>
    public partial class NodeManager : SingletonNode2D<NodeManager>
    {
        [ExportGroup("Nodes")]
        [Export] private GameCamera _mainCamera;

        /// <summary> The parent in Godot for cell nodes. </summary>
        [Export] private Node2D _cellNodeParent;

        /// <summary> The parent in Godot for entity nodes. </summary>
        [Export] private Node2D _entityNodeParent;


        /// <summary> How many entity the actor pool should contain. </summary>
        [ExportGroup("Settings")]
        [Export] private Int32 _poolSize = 100;

        /// <summary> The cell distance around the camera to render. </summary>
        [Export] private Int32 _viewDistance = 8;


        /// <summary> The prefab used to represent an cell within the game world. </summary>
        [ExportGroup("Resources")]
        [Export] private PackedScene _cellPrefab;

        /// <summary> The prefab used to represent an entity within the game world. </summary>
        [Export] private PackedScene _entityPrefab;


        /// <summary> A reference to the grid manager singleton. </summary>
        private GridManager _gridManager;

        /// <summary> A reference to the entity manager singleton. </summary>
        private EntityManager _entityManager;

        /// <summary> The pool used to manage cell nodes within Godot space. </summary>
        private ObjectPool<CellNode> _cellPool;

        /// <summary> The pool used to manage entity nodes within Godot space. </summary>
        private ObjectPool<EntityNode> _entityPool;

        /// <summary> A helper map of entities and their representative nodes in the game world. </summary>
        private Dictionary<Entity, EntityNode> _entityMap = new Dictionary<Entity, EntityNode>();


        /// <inheritdoc/>
        public override void _Ready()
        {
            _gridManager = GridManager.Instance;
            _entityManager = EntityManager.Instance;

            // Initialise the nodes.
            _cellPool = new ObjectPool<CellNode>(_cellNodeParent, _cellPrefab, _poolSize);
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
                Vector3 rawPosition = _gridManager.CalculateRenderPosition(_entityManager.Player.GetPosition());
                _mainCamera.GlobalPosition = new Vector2(rawPosition.X, rawPosition.Y);
            }

            //PerformCull();
        }


        private void PerformCull()
        {
            // Cull / spawn nodes around the camera.
            Vector3I cameraCellPosition = _gridManager.CalculateGridPosition(new Vector3(_mainCamera.GlobalPosition.X, _mainCamera.GlobalPosition.Y, 0));  // TODO - Z pulls from current layer level. Do on Vector2 overload for CalculateGridPosition?

            // Calculate sight range.
            Int32 x0 = Math.Clamp(cameraCellPosition.X - _viewDistance, 0, _gridManager.WorldSize.X);
            Int32 x1 = Math.Clamp(cameraCellPosition.X + _viewDistance, 0, _gridManager.WorldSize.X);
            Int32 y0 = Math.Clamp(cameraCellPosition.Y - _viewDistance, 0, _gridManager.WorldSize.Y);
            Int32 y1 = Math.Clamp(cameraCellPosition.Y + _viewDistance, 0, _gridManager.WorldSize.Y);

            // A list of all the entities we're currently showing, after checking the entities still in range, the values that remain here need to be culled.
            List<Entity> dirtyEntities = new List<Entity>(_entityMap.Keys);

            // Check each cell in range.
            for (Int32 y = y0; y <= y1; y++)
            {
                for (Int32 x = x0; x <= x1; x++)
                {
                    Vector3I position = new Vector3I(x, y, 0);  // TODO - Z pulls from current layer level. Do on Vector2 overload for CalculateGridPosition?
                    if (_gridManager.TryGetCell(position, out Cell? cell) && cell != null)
                    {
                        Entity? entity = cell.GetEntities<Entity>().FirstOrDefault() ?? null;   // TODO - We only want to show one entity, so we need sorting of some kind.
                        if (entity != null)
                        {
                            if (_entityMap.ContainsKey(entity))   // Are we already tracking the cell?
                            {
                                dirtyEntities.Remove(entity);
                            }
                            else                                    // If we're not, we want to by adding a new node.
                            {
                                EntityNode node = _entityPool.GetAvailableObject();
                                node.Initialise(entity);
                                _entityMap.Add(entity, node);
                            }
                        }
                    }
                }
            }


            foreach (Entity entity in dirtyEntities)
            {
                EntityNode node = _entityMap[entity];
                node.CleanUp();
                _entityPool.FreeObject(node);
                _entityMap.Remove(entity);
            }
        }
    }
}
