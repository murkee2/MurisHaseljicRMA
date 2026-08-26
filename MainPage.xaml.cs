using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace MurisHaseljic472;

public partial class MainPage : ContentPage
{
    private readonly HttpClient _httpClient = new();
    public ObservableCollection<ArtworkData> Artworks { get; set; } = new();

    public MainPage()
    {
        InitializeComponent();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        BindingContext = this;
        UpdateEmptyViewVisibility();
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(PageNumberEntry.Text, out int pageNumber) || pageNumber <= 0)
        {
            await DisplayAlert("Error", "Page number has to be a valid integer", "OK");
            return;
        }

        string pageSize = PageSizePicker.SelectedItem?.ToString() ?? "10";

        try
        {
            string url = $"https://api.artic.edu/api/v1/artworks?page={pageNumber}&limit={pageSize}";
            var response = await _httpClient.GetFromJsonAsync<ArtworkResponse>(url);

            Artworks.Clear();
            if (response?.Data != null)
            {
                foreach (var item in response.Data)
                {
                    Artworks.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            UpdateEmptyViewVisibility();
        }
    }

    private async void OnPreviewClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ArtworkData artwork && !string.IsNullOrEmpty(artwork.ImageSource))
        {
            await Launcher.OpenAsync(artwork.ImageSource);
        }
    }

    private void UpdateEmptyViewVisibility()
    {
        EmptyView.IsVisible = Artworks.Count == 0;
        ArtworksContainer.IsVisible = Artworks.Count > 0;
    }
}