namespace SquoundApp_v1.ViewsCustom;

public partial class SearchView : ContentView
{
	public SearchView()
	{
		InitializeComponent();
    }

    private void OnSearchEntryChanged(object? sender, TextChangedEventArgs e)
    {
        // Handle the text changed event for the search entry
        // You can implement search functionality here
        // For example, filter a list based on the entered text
        string searchText = e.NewTextValue;
        // Perform search logic here
    }

    private void OnSearchEntryCompleted(object? sender, EventArgs e)
    {
        // Handle the event when the search entry is completed
        // This can be used to trigger a search when the user presses Enter
        string searchText = SearchEntry.Text;
        // Perform search logic here
    }
}