using Newtonsoft.Json;
using PeopleManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace PeopleManager
{
    /// <summary>
    /// This is an implementation of a car that will be driven by a third party such as a private person or business person.
    
    /// </summary>
    public class Car
    {
        public void Accelerate() { }
        
        public void BrakeWithBrakesOnFrontWheels() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(int x, int y)
        {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class P
    {
        /// <summary>
        /// 
        /// </summary>
        public void M() { }

        public 
    }


    public class DataPeopleHelper
    {
        private string _fileName { get; set; }

        public DataPeopleHelper(string fileName)
        {
            _fileName = fileName;
        }

        public async Task<T> ReadFromFile<T>()
        {
            // read from file
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile file;

            // check if the file exists.
            try
            {
                file = await storageFolder.GetFileAsync(_fileName);
            }
            catch (FileNotFoundException fnfe)
            {
                file = await storageFolder.CreateFileAsync(_fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var text = await Windows.Storage.FileIO.ReadTextAsync(file);
            T obj = JsonConvert.DeserializeObject<T>(text);



            return obj;
        }

        public async void WriteToFile<T>(T data)
        {
            // XML + JSON
            string jsonPeople;

            // jsonPeople = JsonConvert.SerializeObject(People, Formatting.Indented);
            jsonPeople = JsonConvert.SerializeObject(data, Formatting.None);

            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            Windows.Storage.StorageFile file;
            try
            {
                file = await storageFolder.GetFileAsync(_fileName);
            }
            catch (FileNotFoundException fnfe)
            {
                file = await storageFolder.CreateFileAsync(_fileName);
                System.Diagnostics.Trace.WriteLine(fnfe.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            await Windows.Storage.FileIO.WriteTextAsync(file, jsonPeople, Windows.Storage.Streams.UnicodeEncoding.Utf8);
        }
    }

    

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        const string FileName = "people.json";
        public ObservableCollection<Person> People { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

        }

        private async void People_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DataPeopleHelper helper = new DataPeopleHelper("people.json");
            helper.WriteToFile(People);
        }

        private async void Place_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            DataPeopleHelper placeHelper = new DataPeopleHelper("places.json");
            placeHelper.WriteToFile(new ObservableCollection<Place>() { new Place() { Name = "Karlstad" } });
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            DataPeopleHelper helper = new DataPeopleHelper("people.json");
            People = await helper.ReadFromFile<ObservableCollection<Person>>();

            DataPeopleHelper stringHelper = new DataPeopleHelper("places.json");
            ObservableCollection<Place> myPlaces = await stringHelper.ReadFromFile<ObservableCollection<Place>>();

            if (People == null)
            {
                People = new ObservableCollection<Person>()
                {
                    new Person() { Name="", Age=-1 },
                    new Person() { Name="Robert", Age=32 },
                    new Person() { Name="Winston", Age=30 },
                    new Person() { Name="Leonard", Age=40 }
                };
            }

            // or
            // do the following
            //People = new ObservableCollection<Person>()
            //{
            //    new Person() { Name="", Age=-1 },
            //    new Person() { Name="Robert", Age=32 },
            //    new Person() { Name="Winston", Age=30 },
            //    new Person() { Name="Leonard", Age=40 }
            //};
            People.CollectionChanged += People_CollectionChanged;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments); 
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
