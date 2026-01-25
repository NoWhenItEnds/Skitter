#nullable disable warnings
using System;
using Godot;

namespace Skitter.Entities.Data
{
    /// <summary> Data possessed by all actors within the game world. </summary>
    [GlobalClass]
    public partial class ActorData : EntityData
    {
        /// <summary> The actor's first name. </summary>
        [ExportGroup("ActorData")]
        [ExportSubgroup("General")]
        [Export] public String FirstName { get; set; }

        /// <summary> The actor's last name. </summary>
        [Export] public String LastName { get; set; }


        /// <summary> Data possessed by all actors within the game world. </summary>
        public ActorData() : base() { }


        /// <inheritdoc/>
        public override String GetUId() => $"actordata_{FirstName.ToLower()}{LastName.ToLower()}";
    }
}
