using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MurisHaseljic472
{
    public class ArtworkResponse
    {
        [JsonPropertyName("data")]
        public List<ArtworkData> Data { get; set; } = new();
    }

    public class ArtworkData
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("date_display")]
        public string DateDisplay { get; set; } = string.Empty;

        [JsonPropertyName("place_of_origin")]
        public string PlaceOfOrigin { get; set; } = string.Empty;

        [JsonPropertyName("medium_display")]
        public string MediumDisplay { get; set; } = string.Empty;

        [JsonPropertyName("classification_title")]
        public string ClassificationTitle { get; set; } = string.Empty;

        [JsonPropertyName("material_titles")]
        public List<string> MaterialTitles { get; set; } = new();

        [JsonPropertyName("image_id")]
        public string? ImageId { get; set; }

        public string? ImageSource => !string.IsNullOrEmpty(ImageId)
            ? $"https://www.artic.edu/iiif/2/{ImageId}/full/843,/0/default.jpg"
            : null;

        public string FirstMaterial => (MaterialTitles != null && MaterialTitles.Count > 0) ? MaterialTitles[0] : "N/A";
    }

    public class ExhibitionResponse
    {
        [JsonPropertyName("data")]
        public List<ExhibitionData> Data { get; set; } = new();
    }

    public class ExhibitionData
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("aic_start_at")]
        public string? AicStartAt { get; set; }

        [JsonPropertyName("_score")]
        public double? Score { get; set; }

        [JsonPropertyName("api_link")]
        public string ApiLink { get; set; } = string.Empty;

        public string FormattedDate
        {
            get
            {
                if (!string.IsNullOrEmpty(AicStartAt) && DateTime.TryParse(AicStartAt, out DateTime dt))
                {
                    return dt.ToString("MMMM dd, yyyy");
                }
                return "N/A";
            }
        }

        public string ConfidenceScoreText => $"Confidence Score: {Score ?? 0}%";
    }
}