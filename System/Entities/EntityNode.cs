#nullable disable warnings
using System;
using Godot;
using Skitter.Entities.Actors;
using Skitter.Nodes;

namespace Skitter.Entities.Nodes
{
    /// <summary> A node representing an actor entity within the game world. </summary>
    public partial class EntityNode : RogueNode<Entity>
    {
        /// <summary> A label to display debug information. </summary>
        [ExportCategory("Entity")]
        [ExportGroup("Nodes")]
        [Export] private RichTextLabel _debugLabel;  // TODO - A better solution. Just for debug purposes.


        /// <inheritdoc/>
        public override void _Process(Double delta)
        {
            base._Process(delta);

            if (_data != null)
            {
                if (_data is ActorEntity actor)
                {
                    // Debug Shit.
                    _debugLabel.Clear();
                    _debugLabel.AppendText($"Cost: {actor.QueuedAction?.GetCost()}\n");
                    _debugLabel.AppendText($"Position: {actor.GetPosition()}\n");
                }
            }
        }
    }
}
