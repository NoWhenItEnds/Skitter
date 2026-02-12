#nullable disable warnings
using System;
using Godot;
using Skitter.Interfaces;
using Skitter.Managers;

namespace Skitter.Nodes
{
    /// <summary> A visual indication of data within the game world. Maps to a piece of data on the game world grid. </summary>
    public abstract partial class RogueNode<T> : Node2D where T : class, IGraphical
    {
        /// <summary> The node's sprite. </summary>
        [ExportCategory("General")]
        [ExportGroup("Nodes")]
        [Export] protected AnimatedSprite2D _sprite;


        /// <summary> The data entity this node represents in the game world. </summary>
        protected T? _data = null;


        /// <summary> A reference to the game manager singleton. </summary>
        protected GameManager _gameManager;


        /// <inheritdoc/>
        public override void _Ready()
        {
            _gameManager = GameManager.Instance;
        }



        /// <summary> Initialise the node. </summary>
        /// <param name="entity"> The data entity this node represents in the game world. </param>
        public void Initialise(T entity)
        {
            _data = entity;
        }


        /// <inheritdoc/>
        public override void _Process(Double delta)
        {
            if (_data != null)
            {
                Vector3 rawPosition = _gameManager.CalculateRenderPosition(_data.GetPosition());
                GlobalPosition = new Vector2(rawPosition.X, rawPosition.Y);
            }
        }


        /// <summary> Perform any clean up needed when unlinking the actor from it. </summary>
        public void CleanUp()
        {
            _data = null;
        }
    }
}
