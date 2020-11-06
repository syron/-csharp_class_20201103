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
        private App _app { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            _app = (App)App.Current;

            _app.People.CollectionChanged += People_CollectionChanged;
        }

        private void People_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // write to file, database, APP property
        }

        private void RemovePersonFromPeopleListButton_Click(object sender, RoutedEventArgs e)
        {
            Person person = (Person)PeopleListComboBox.SelectedItem;

            _app.People.Remove(person);
        }

        private void GoToAddPersonPageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddPersonPage));
        }

        private void PeopleListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Person person = (Person)PeopleListComboBox.SelectedItem;

            //Person person = (Person)e.AddedItems[0];

            if (person == null || person.Age < 0)
            {
                RemovePersonFromPeopleListButton.IsEnabled = false;
            }
            else
            {
                RemovePersonFromPeopleListButton.IsEnabled = true;
            }

        }
    }
}
