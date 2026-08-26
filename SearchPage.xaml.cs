using System;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace MurisHaseljic472;

public partial class SearchPage : ContentPage
{
    public ObservableCollection<ExhibitionData> Datums { get; set; } = new();
    private readonly HttpClient _httpClient = new();

    public SearchPage()
    {
        InitializeComponent();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        BindingContext = this;
        UpdateEmptyViewVisibility();
    }

    private async void OnSearchExhibitionsClicked(object sender, EventArgs e)
    {
        string query = ExhibitionNameEntry.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(query)) return;

        try
        {
            string url = $"https://api.artic.edu/api/v1/exhibitions/search?q={Uri.EscapeDataString(query)}";
            var response = await _httpClient.GetFromJsonAsync<ExhibitionResponse>(url);

            Datums.Clear();
            if (response?.Data != null)
            {
                foreach (var item in response.Data)
                {
                    Datums.Add(item);
                }

                await DisplayAlert("Message", $"Found {response.Data.Count} entries", "OK");
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

    private async void OnViewMoreClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is ExhibitionData exhibition && !string.IsNullOrEmpty(exhibition.ApiLink))
        {
            await Launcher.OpenAsync(exhibition.ApiLink);
        }
    }

    private void UpdateEmptyViewVisibility()
    {
        EmptyExhibitionsView.IsVisible = Datums.Count == 0;
        ExhibitionsContainer.IsVisible = Datums.Count > 0;
    }
}