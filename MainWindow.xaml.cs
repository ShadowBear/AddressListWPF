using System.Net.Http;
using System.Net.Http.Json;
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
using AddressListWPF.Models;
using AddressListWPF.Services;

namespace AddressListWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAddressService _addressService;
        private readonly IContactInfoService _contactInfoService;
        private List<Address> addresses;
        private List<ContactInfo> contacts;

        public MainWindow()
        {
            InitializeComponent();

            HttpClient httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7018/api/") };
            _addressService = new AddressService(httpClient);
            _contactInfoService = new ContactInfoService(httpClient);

            //Can be used to load from small database and then filter in cached data
            //LoadAllData();
        }

        private async void LoadAllData()
        {
            try
            {
                addresses = await _addressService.GetAddressesAsync();
                contacts = await _contactInfoService.GetContactInfosAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}");
            }
        }

        public async Task RefreshAddressesAsync()
        {
            addresses = await _addressService.GetAddressesAsync();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text;

            if (string.IsNullOrEmpty(query)) return;

            try
            {
                var results = await _addressService.SearchAddressesAsync(query);
                ResultsGrid.ItemsSource = results;

                if (results.Any())
                {
                    int fileId = results.First().FileID;
                    var contacts = await _contactInfoService.GetContactInfosByFileIdAsync(fileId);
                    ContactResultsGrid.ItemsSource = contacts;
                }

                /********* For small Database use cached data instead of API calls
                  
                var searchAddressList = addresses.FindAll(item => item.Aktenzeichen.Equals(query));
                if (searchAddressList.Count <= 0) return;

                var sortedAddressList = searchAddressList.OrderByDescending(item => item.AktuelleAnschrift).ToList();
                int fileId = searchAddressList[0].FileID;
                var searchContactInfoList = contacts.FindAll(item => item.FileID == fileId);
                
                ResultsGrid.ItemsSource = sortedAddressList;
                ContactResultsGrid.ItemsSource = searchContactInfoList;
                */


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}");
            }
        }

        private void OpenAddAddress_Click(object sender, RoutedEventArgs e)
        {
            var addAddressWindow = new AddAddress(_addressService);
            addAddressWindow.ShowDialog();
        }
    }
}