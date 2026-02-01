#nullable disable warnings
using System;
using System.Text.Json.Serialization;

namespace Skitter.Entities.Data
{
    /// <summary> Data possessed by all actors within the game world. </summary>
    public class ActorData : EntityData
    {
        /// <summary> The actor's first name. </summary>
        [JsonPropertyName("first_name")]
        public String FirstName { get; set; }

        /// <summary> The actor's last name. </summary>
        [JsonPropertyName("last_name")]
        public String LastName { get; set; }


        /// <inheritdoc/>
        public override String GetUId() => $"actordata_{FirstName.ToLower()}{LastName.ToLower()}";
    }
}
