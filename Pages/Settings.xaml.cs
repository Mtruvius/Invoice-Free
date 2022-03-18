using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Invoice_Free
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {

        public int CharAvailable { get; set; }
        
        private StorageFile _chosenImage;
        private string _chosenEmail;
        private string _chosenContact;
        private string _chosenAddress;
        private string _chosenRegNo;
        private string _chosenTax;

        private StorageFile _newImage;
        private string _newEmail;
        private string _newContact;
        private string _newAddress;
        private string _newRegNo;
        private string _newTax;
        

        List<string[]> CompanyChangesList;
        private DispatcherTimer dispatcherTimer;

        public Settings()
        {
            this.InitializeComponent();
            App.ImageSelected += ImageSelected;

            GetCompanySettings();

            CharAvailable = 165;
            Available.Text = "Characters available: " + CharAvailable.ToString();
        }

        private void GetCompanySettings()
        {
            TaxToggle.IsOn = App.companyActive.AddTax;
            TaxPercentageBox.Value = App.companyActive.TaxRate;
        }

        private void EditCompanyBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowCompanyDetails();                 
        }

        private void ShowCompanyDetails()
        {
            CompanyName_TextBlock.Text = App.companyActive.CompanyName;
            if (_chosenImage != null)
            {
                BitmapImage logo = new BitmapImage(new Uri(_chosenImage.Path));
                if (logo.ToString() != App.companyActive.CompanyLogo.ToString())
                {
                    Company_logo.Source = App.companyActive.CompanyLogo;
                }
                else
                {
                    Company_logo.Source = logo;
                }
            }
            else 
            {
                Company_logo.Source = App.companyActive.CompanyLogo;
            }
            
            if (string.IsNullOrEmpty(_newEmail) || _newEmail != _chosenEmail)
            {
                CompanyEmail_TextBox.Text = App.companyActive.Email;
            }
            else
            {
                CompanyEmail_TextBox.Text = _newEmail;
            }
            if (string.IsNullOrEmpty(_newContact) || _newContact != _chosenContact)
            {
                CompanyContact_TextBox.Text = App.companyActive.Contact;
                Debug.WriteLine("_newContact: " + _newContact); 
                Debug.WriteLine("_chosenContact: " + _chosenContact); 
                Debug.WriteLine(" App.companyActive.Contact: " + App.companyActive.Contact); 
            }
            else
            {
                CompanyContact_TextBox.Text = _newContact;
                Debug.WriteLine("_newContact ELSE: " + _newContact);
                Debug.WriteLine("_chosenContact ELSE: " + _chosenContact);
                Debug.WriteLine(" App.companyActive.Contact ELSE: " + App.companyActive.Contact);
            }
            if (string.IsNullOrEmpty(_chosenAddress) || _chosenAddress == App.companyActive.Address)
            {
                CompanyAddress_TextBox.Text = App.companyActive.Address;
            }
            else
            {
                CompanyAddress_TextBox.Text = _chosenAddress;
            }
            if (string.IsNullOrEmpty(_chosenRegNo) || _chosenRegNo == App.companyActive.RegNo)
            {
                CompanyRegNo_TextBox.Text = App.companyActive.RegNo;
            }
            else
            {
                CompanyRegNo_TextBox.Text = _chosenRegNo;
            }
            if (string.IsNullOrEmpty(_chosenTax) || _chosenTax != App.companyActive.Tax)
            {
                CompanyTax_TextBox.Text = App.companyActive.Tax;
            }
            else
            {
                CompanyTax_TextBox.Text = _chosenTax;
            }
            
            CompanyEditing_Grid.Visibility = Visibility.Visible;
        }

        private void ChangeCompanyBtn_Click(object sender, RoutedEventArgs e)
        {
            App.ChangePageTo("Intro",null, App.AnimatePage("start"));
        }
         
        private void TaxSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggle = sender as ToggleSwitch;
            if (toggle.IsOn)
            {
                Debug.WriteLine("Toggle is ON");
                TaxRate.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("Toggle is OFF");
                TaxRate.Visibility = Visibility.Collapsed;
            }
            
        }
        
        private void SelectingImage()
        {
            App.SelectImage();
            ImageBorder.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void ImageSelected(BitmapImage image, StorageFile file)
        {
            Company_logo.Source = image;
            _chosenImage = file;
            AddToChangeMadeList("Company logo", "New Image");
        }

        private async void CompanyEditing_SaveClick(object sender, RoutedEventArgs e)
        {
            CompanyChangesList = new List<string[]>();

            foreach (var item in CompanyEditing_Stack.Children.OfType<Grid>())
            {
                if (item.Children[1].GetValue(TagProperty) != null && item.Children[1].GetValue(TagProperty).ToString() == "CompanyInput")
                {
                    TextBox input = item.Children[1] as TextBox;
                    TextBox_ChangedCheck(input);                   
                }                
            }

            if (CompanyChangesList.Count > 0)
            {
                string changes = "";
                ///var deferral = args.GetDeferral();

                foreach (var str in CompanyChangesList)
                {
                    changes += "- " + str[0] + " to " + str[1] + "\n";
                }


                MikesContentDialog dialog = new();
                dialog.DialogContentMaxWidth = 600;

                dialog.Title = "Accept Changes";
                dialog.TitleFontSize = 25;
                dialog.TitleFontWeight = FontWeights.Bold;
                dialog.TitleHorizontalAlignment = HorizontalAlignment.Center;

                dialog.ContentHeaderText = "The following changes were made...";

                TextBlock content = new()
                {
                    Text = changes,
                    FontStyle = Windows.UI.Text.FontStyle.Italic
                };
                dialog.Content = content;

                dialog.FooterHeaderText = "Would you like to save these changes?";

                dialog.CloseButtonText = "Cancel";

                dialog.PrimaryButtonText = "Save";
                dialog.PrimaryButtonClick += AcceptedChanges_AcceptbuttonClick;
                dialog.CloseButtonClick += AcceptedChanges_CancelbuttonClick;

                dialog.XamlRoot = this.Content.XamlRoot;
                await dialog.ShowAsync();
            }
            else
            {
                CompanyEditing_Grid.Visibility = Visibility.Collapsed;
            }
                
        }

        private void TextBox_ChangedCheck(TextBox input)
        {
            switch (input.Name)
            {
                case "CompanyEmail_TextBox":
                    if (input.Text != App.companyActive.Email)
                    {
                        _chosenEmail = input.Text;
                        AddToChangeMadeList("Email: " + App.companyActive.Email, _chosenEmail);
                    }
                    break;
                case "CompanyContact_TextBox":
                    if (input.Text != App.companyActive.Contact)
                    {
                        _chosenContact = input.Text;
                        AddToChangeMadeList("Contact: " + App.companyActive.Contact, _chosenContact);
                    }
                    break;
                case "CompanyAddress_TextBox":
                    if (input.Text != App.companyActive.Address)
                    {
                        _chosenAddress = input.Text;
                        AddToChangeMadeList("Address: " + App.companyActive.Address, _chosenAddress);
                    }
                    break;
                case "CompanyRegNo_TextBox":
                    if (input.Text != App.companyActive.RegNo)
                    {
                        _chosenRegNo = input.Text;
                        AddToChangeMadeList("RegNo: " + App.companyActive.RegNo, _chosenRegNo);
                    }
                    break;
                case "CompanyTax_TextBox":
                    if (input.Text != App.companyActive.Tax)
                    {
                        _chosenTax = input.Text;
                        AddToChangeMadeList("Tax: " + App.companyActive.Tax, _chosenTax);
                    }
                    break;
            }

        }

        private void AddToChangeMadeList(string a, string b)
        {
            string[] changeMade = new string[]
                {
                    a,b
                };
            CompanyChangesList.Add(changeMade);
        }
        
        private void AcceptedChanges_AcceptbuttonClick(ContentDialog dialog, RoutedEventArgs args)
        {
            _newImage = _chosenImage;
            _newEmail = _chosenEmail;
            _newContact = _chosenContact;
            _newAddress = _chosenAddress;
            _newRegNo = _chosenRegNo;
            _newTax = _chosenTax;
            dialog.Hide();
            CompanyEditing_Grid.Visibility = Visibility.Collapsed;
        }
        private void AcceptedChanges_CancelbuttonClick(ContentDialog sender, RoutedEventArgs args)
        {
            ShowCompanyDetails();            
            sender.Hide();
        }

        private async void SaveSettingsBtn_Click(object sender, RoutedEventArgs e)
        {

            string PATH_toCompany = App.PathToCompanies + App.companyActive.CompanyName;
            if (_newImage != null)
            {
                await _newImage.CopyAsync(await StorageFolder.GetFolderFromPathAsync(PATH_toCompany), "logo.png", NameCollisionOption.ReplaceExisting);
            }

            if (_newEmail != null && _newEmail != App.companyActive.Email)
            {
                App.companyActive.Email = _newEmail;
            }

            if (_newContact != null && _newContact != App.companyActive.Contact)
            {
                App.companyActive.Contact = _newContact;
            }

            if (_newAddress != null && _newAddress != App.companyActive.Address)
            {
                App.companyActive.Address = _newAddress;
            }

            if (_newRegNo != null && _newRegNo != App.companyActive.RegNo)
            {
                App.companyActive.RegNo = _newRegNo;
            }

            if (_newTax != null && _newTax != App.companyActive.Tax)
            {
                App.companyActive.Tax = _newTax;
            }

            if (TaxToggle.IsOn)
            {
                App.companyActive.AddTax = true;
            }
            else
            {
                App.companyActive.AddTax = false;
            }
            
            if (TaxPercentageBox.Value != App.companyActive.TaxRate)
            {
                Debug.WriteLine("TaxPercentageBox.Value: " + TaxPercentageBox.Value);
                App.companyActive.TaxRate = TaxPercentageBox.Value;
            }
            App.companyActive.InvoiceFooterMsg = InvFootMsg.Text;
            
            SaveManager.SaveCompanyEdits();
            CreateTimer();
            SaveSuccessNotification.Visibility = Visibility.Visible;
            MainSettingsContent.Visibility = Visibility.Collapsed;
            
        }

        private void CancelSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("CancelSettingsBtn_Click");
            MainPage.Popup_Panel.Visibility = Visibility.Collapsed;
            MainPage.Popup_Content.Children.Clear();
            Debug.WriteLine(MainPage.Instance.currentActivePage);
            MainPage.Instance.NavigateToPage(MainPage.Instance.currentActivePage, null);
        }

        public void CreateTimer()
        {
            // get a timer to close the ContentDialog after 2s
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimer_Tick; ;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            RoutedEventArgs args = new();

            SaveSuccessNotification.Visibility = Visibility.Collapsed;
            dispatcherTimer.Stop();
            dispatcherTimer = null;            
            CancelSettingsBtn_Click(sender, args);
        }

        private void InvFootMsg_TextChanged(object sender, TextChangedEventArgs e)
        {
            CharAvailable = 165;
            int MaxLength = 165;
            int textLength = 94;            
            TextBox box = (TextBox)sender;
            
            string[] lines = box.Text.Split('\r');
            Debug.WriteLine("Lines: " + lines.Length);
            Debug.WriteLine("box.Text.Length: " + box.Text.Length);
            if (lines.Length > 1 || box.Text.Length == textLength)
            {
                
                InvFootMsg.MaxLength = MaxLength - ((textLength +1) - lines[0].Length);
                InvFootMsg.AcceptsReturn = false;
                Debug.WriteLine("This was called");
                Debug.WriteLine("InvFootMsg.MaxLength: " + InvFootMsg.MaxLength);
                CharAvailable -= (textLength+1) - lines[0].Length;
            }
            else
            {
                InvFootMsg.AcceptsReturn = true;
            }
            
            CharAvailable -= box.Text.Length;
            Available.Text = "Characters available: " + CharAvailable.ToString();
            Debug.WriteLine("CharAvailable: " + CharAvailable);
        }
    }
}
