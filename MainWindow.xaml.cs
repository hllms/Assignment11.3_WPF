using Assignment11._3_WPF.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Assignment11._3_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OutdoorGear newOutdoorGear = new OutdoorGear();
        OutdoorGear? selectedGear = null;

        private readonly HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7206/")
        };

        private readonly ObservableCollection<OutdoorGear> _outdoorGears = new ObservableCollection<OutdoorGear>();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            OutdoorGearGrid.ItemsSource = _outdoorGears;
        }
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Test call – replace with actual binding later
                var items = await _http.GetFromJsonAsync<List<OutdoorGear>>("api/OutdoorGears");

                MessageBox.Show($"Received {items?.Count} items from API");

                // Example to set the DataGrid
                OutdoorGearGrid.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"API Error: {ex.Message}");
            }
        }

        private void UpdateGearforEdit(object sender, RoutedEventArgs e)
        {
            selectedGear = (sender as FrameworkElement).DataContext as OutdoorGear;
            UpdateGearGrid.DataContext = selectedGear;
        }

        private async void DeleteGear(object sender, RoutedEventArgs e)
        {
            try
            {
                // Correct cast
                var gear = (sender as FrameworkElement)?.DataContext as OutdoorGear;
                if (gear == null) return;

                // Optional confirmation
                if (MessageBox.Show($"Delete '{gear.Name}'?", "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;

                // Call API delete
                var response = await _http.DeleteAsync($"api/OutdoorGears/{gear.Id}");
                response.EnsureSuccessStatusCode();

                // Remove from the ObservableCollection
                _outdoorGears.Remove(gear);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting gear: {ex.Message}");
            }
        }

        private async void AddGear(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newOutdoorGear.Name))
                {
                    MessageBox.Show("Name is required.");
                    return;
                }

                newOutdoorGear.Id = Guid.Empty;

                var resp = await _http.PostAsJsonAsync("api/OutdoorGears", newOutdoorGear);
                resp.EnsureSuccessStatusCode();

                var created = await resp.Content.ReadFromJsonAsync<OutdoorGear>();
                if (created != null)
                {
                    _outdoorGears.Add(created);     // update UI
                    newOutdoorGear = new OutdoorGear();  // clear form
                    UpdateGearGrid.DataContext = newOutdoorGear;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add failed: {ex.Message}");
            }
        }

        private async void UpdateGear(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedGear == null)
                {
                    MessageBox.Show("No gear selected for update.");
                    return;
                }
                var resp = await _http.PutAsJsonAsync($"api/OutdoorGears/{selectedGear.Id}", selectedGear);
                resp.EnsureSuccessStatusCode();
                MessageBox.Show("Update successful.");
                newOutdoorGear = new OutdoorGear();
                UpdateGearGrid.DataContext = newOutdoorGear;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}");
            }
        }

        
    }
}