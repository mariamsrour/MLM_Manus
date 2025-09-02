using Newtonsoft.Json;

namespace Nop.API.Models.JsonLD;

public record JsonLdAggregateRatingModel : JsonLdModel
{
    #region Properties

    [JsonProperty("@type")]
    public static string Type => "AggregateRating";

    [JsonProperty("ratingValue")]
    public string RatingValue { get; set; }

    [JsonProperty("reviewCount")]
    public int ReviewCount { get; set; }

    #endregion
}