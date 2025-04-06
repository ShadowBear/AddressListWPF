using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AddressList.Shared.Models;
using AddressList.Shared.Services;
using AddressListWPF.Services;

namespace AddressListWPF
{
    /// <summary>
    /// Interaktionslogik für AddAddress.xaml
    /// </summary>
    public partial class AddAddress : Window
    {
        private readonly IAddressService _addressService;

        public AddAddress(IAddressService addressService)
        {
            InitializeComponent();
            _addressService = addressService;
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            // Ensure required fields are not empty
            if (string.IsNullOrWhiteSpace(FileIDBox.Text) ||
                string.IsNullOrWhiteSpace(AktenzeichenBox.Text) ||
                string.IsNullOrWhiteSpace(VornameBox.Text) ||
                string.IsNullOrWhiteSpace(NachnameBox.Text) ||
                string.IsNullOrWhiteSpace(AddressStatusBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Parse FileID (since it's an integer)
            if (!int.TryParse(FileIDBox.Text, out int fileId))
            {
                MessageBox.Show("Invalid File ID. Please enter a number.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var address = new Address
            {
                FileID = fileId,
                Aktenzeichen = AktenzeichenBox.Text,
                Rechtsform = RechtsformBox.Text,
                Anrede = AnredeBox.Text,
                Titel = TitelBox.Text,
                Vorname = VornameBox.Text,
                Nachname = NachnameBox.Text,
                Straße = StraßeBox.Text,
                Hausnummer = HausnummerBox.Text,
                PLZ = PLZBox.Text,
                Ort = OrtBox.Text,
                Land = LandBox.Text,
                Datenquelle = DatenquelleBox.Text,
                AktuelleAnschrift = AktuelleAnschriftBox.IsChecked ?? false,
                AddressStatus = AddressStatusBox.Text
            };

            bool success = await _addressService.SaveAddress(address);
            if (success)
            {
                //Update cached data here to be in sync with the global DB 
                MessageBox.Show("Address added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to add address.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
