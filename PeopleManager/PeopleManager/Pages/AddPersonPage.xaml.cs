using PeopleManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PeopleManager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddPersonPage : Page
    {
        private App _app { get; set; }

        public AddPersonPage()
        {
            this.InitializeComponent();

            _app = (App)App.Current;
        }

        private void BackToMainPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void AddPersonButton_Click(object sender, RoutedEventArgs e)
        {
            string name = PersonNameTextBox.Text;
            string age = PersonAgeTextBox.Text;

            int ageInt = 0;

            if (int.TryParse(age, out ageInt))
            {
                _app.People.Add(new Person() { Name = name, Age = ageInt });

                PersonNameTextBox.Text = "";
                PersonAgeTextBox.Text = "";
            }else
            {
                ErrorMessage.Text = "Age is not a number... Fix it, NOW!";
            }

        }
    }
}
