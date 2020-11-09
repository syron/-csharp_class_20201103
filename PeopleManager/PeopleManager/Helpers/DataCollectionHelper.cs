using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PeopleManager.Helpers
{
    public class DataCollectionHelper<T>
    {
        private ObservableCollection<T> _data;
        private string _fileName { get; set; }

        public DataCollectionHelper(string fileName)
        {
            _fileName = fileName;
        }

        public async void ReadFromFileAsync()
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
                System.Diagnostics.Trace.WriteLine(fnfe.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var text = await Windows.Storage.FileIO.ReadTextAsync(file);
            ObservableCollection<T> obj = JsonConvert.DeserializeObject<ObservableCollection<T>>(text);

            if (obj == null)
            {
                obj = new ObservableCollection<T>();
            }

            obj.CollectionChanged += Obj_CollectionChanged;

            _data = obj;
        }

        private void Obj_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            WriteToFileAsync();
        }

        public void Add(T item)
        {
            _data.Add(item);
        }

        public void Remove(T item)
        {
            _data.Remove(item);
        }

        public ObservableCollection<T> Get()
        {
            return _data;
        }

        async void WriteToFileAsync()
        {
            // XML + JSON
            string jsonPeople;

            jsonPeople = JsonConvert.SerializeObject(_data, Formatting.None);

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
}
