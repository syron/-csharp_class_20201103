using Newtonsoft.Json;
using PeopleManager.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PeopleManager
{
    /// <summary>
    /// Class that handles reading from and writing to files, no matter what 
    /// </summary>
    public class DataPeopleHelper
    {
        /// <summary>
        /// The file from which the program will read and write to.
        /// </summary>
        private string _fileName { get; set; }

        /// <summary>
        /// Constructor that is fired instantly when an object is being initiated.
        /// </summary>
        /// <param name="fileName">The filename to read from and write to.</param>
        public DataPeopleHelper(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Reads from a JSON file by filename and returns the result T.
        /// </summary>
        /// <typeparam name="T">The type of object that is being stored in the file</typeparam>
        /// <returns>The object T</returns>
        public async Task<T> ReadFromFile<T>()
        {
            // Locate the folder which this UWP application can read from and write to.
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Initiate a file variable.
            Windows.Storage.StorageFile file;
            try
            {
                // if the file exists, the file variable will be set to that.
                file = await storageFolder.GetFileAsync(_fileName);
            }
            catch (FileNotFoundException fnfe)
            {
                // if the file does not exist, an exception will be thrown, but we will make sure the file will be created.
                file = await storageFolder.CreateFileAsync(_fileName);
            }
            catch (Exception ex)
            {
                // if this exception occurs, this means we do not have handled the an exception and we want the program to crash.
                throw ex;
            }

            var text = await Windows.Storage.FileIO.ReadTextAsync(file);
            T obj = JsonConvert.DeserializeObject<T>(text);

            return obj;
        }

        /// <summary>
        /// Writes T to a file by the initial filename as JSON.
        /// </summary>
        /// <typeparam name="T">The object type to store</typeparam>
        /// <param name="data">The actual object to store in the file</param>
        public async void WriteToFile<T>(T data)
        {
            // Convert data to JSON object (https://www.w3schools.com/whatis/whatis_json.asp)
            string jsonContent = JsonConvert.SerializeObject(data, Formatting.None);

            // Locate the folder which this UWP application can read from and write to.
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Initiate a file variable.
            Windows.Storage.StorageFile file;
            try
            {
                // if the file exists, the file variable will be set to that.
                file = await storageFolder.GetFileAsync(_fileName);
            }
            catch (FileNotFoundException fnfe)
            {
                // if the file does not exist, an exception will be thrown, but we will make sure the file will be created.
                file = await storageFolder.CreateFileAsync(_fileName);
            }
            catch (Exception ex)
            {
                // if this exception occurs, this means we do not have handled the an exception and we want the program to crash.
                throw ex;
            }

            // Now, write the JSON object to the actual file.
            await Windows.Storage.FileIO.WriteTextAsync(file, jsonContent, Windows.Storage.Streams.UnicodeEncoding.Utf8);
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
