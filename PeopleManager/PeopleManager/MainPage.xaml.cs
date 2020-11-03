using PeopleManager.Models;
using PeopleManager.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PeopleManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<Person> People { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            People = new ObservableCollection<Person>()
            {
                new Person() { Name="Robert", Age=32 },
                new Person() { Name="Winston", Age=30 },
                new Person() { Name="Leonard", Age=40 }
            };

        }

        private void RemovePersonFromPeopleListButton_Click(object sender, RoutedEventArgs e)
        {
            Person person = (Person)PeopleListComboBox.SelectedItem;

            People.Remove(person);
        }

        private void GoToAddPersonPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddPersonPage));
        }
    }
}
