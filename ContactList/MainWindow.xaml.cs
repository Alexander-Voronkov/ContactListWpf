using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ContactList
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsAllValidated { get { return (
                    (new NameValidationRule().Validate(name_textbox.Text, new CultureInfo(1)) == new ValidationResult(true, null)) &&
                    (new MobilePhoneValidationRule().Validate(mobile_textbox.Text, new CultureInfo(1)) == new ValidationResult(true, null)) &&
                    (new HomePhoneValidationRule().Validate(home_textbox.Text, new CultureInfo(1)) == new ValidationResult(true, null) &&
                    (new PostalCodeValidationRule().Validate(postal_textbox.Text, new CultureInfo(1)) == new ValidationResult(true, null) &&
                    (new NameValidationRule().Validate(address_textbox.Text, new CultureInfo(1)) == new ValidationResult(true, null))))); } }
        public MainWindow()
        {
            InitializeComponent();
            OnStart();
        }

        private void OnStart()
        {
            try
            {
                using (var str = new FileStream("contacts.xml", FileMode.Open))
                {
                    var ser = new XmlSerializer(typeof(ObservableCollection<Contact>));
                    Contacts.ItemsSource = (ObservableCollection<Contact>)ser.Deserialize(str);
                }
            }
            catch (FileNotFoundException) { }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if(IsAllValidated)
            (Contacts.ItemsSource as ObservableCollection<Contact>).Add(new Contact(name_textbox.Text,mobile_textbox.Text,home_textbox.Text,postal_textbox.Text,address_textbox.Text));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            (Contacts.ItemsSource as ObservableCollection<Contact>).Remove(Contacts.SelectedItem as Contact);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (var str = new FileStream("contacts.xml", FileMode.Create))
            {
                var ser = new XmlSerializer(typeof(ObservableCollection<Contact>));
                ser.Serialize(str, Contacts.ItemsSource); 
            }            
        }
    }

    [Serializable()]
    public class Contact
    {
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }
        public Contact() { }
        public Contact(string name, string mpn, string hpn, string pc, string address)
        {
            Name = name;
            MobilePhone = mpn;
            HomePhone = hpn;
            PostalCode = pc;
            Address = address;
        }
    }

    public class Contactss
    {
        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
        public ObservableCollection<Contact> Get() => contacts;
    }

    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(string.IsNullOrEmpty((string)value)||string.IsNullOrWhiteSpace((string)value))
            {
                return new ValidationResult(false,"Can't be empty.");
            }

            return new ValidationResult(true,null);
        }
    }
    public class MobilePhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
            {
                return new ValidationResult(false, "Can't be empty.");
            }
            else if(!(new Regex(@"^[0-9]+$").IsMatch(value.ToString())))
            {
                return new ValidationResult(false, "Digits only.");
            }
            return new ValidationResult(true, null);
        }
    }

    public class HomePhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
            {
                return new ValidationResult(false, "Can't be empty.");
            }
            else if (!(new Regex(@"^[0-9]+$").IsMatch(value.ToString())))
            {
                return new ValidationResult(false, "Digits only.");
            }
            return new ValidationResult(true, null);
        }
    }

    public class PostalCodeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
            {
                return new ValidationResult(false, "Can't be empty.");
            }
            else if (!(new Regex(@"^[0-9]+$").IsMatch(value.ToString())))
            {
                return new ValidationResult(false, "Digits only.");
            }
            return new ValidationResult(true, null);
        }
    }

    public class AddressValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((string)value) || string.IsNullOrWhiteSpace((string)value))
            {
                return new ValidationResult(false, "Can't be empty.");
            }
            return new ValidationResult(true, null);
        }
    }

}
