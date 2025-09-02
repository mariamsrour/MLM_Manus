using Newtonsoft.Json;

namespace Nop.API.Models.JsonLD;

public record JsonLdPersonModel : JsonLdModel
{
    #region Properties

    [JsonProperty("@type")]
    public static string Type => "Person";

    [JsonProperty("name")]
    public string Name { get; set; }

    #endregion
}