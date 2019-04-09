using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using Newtonsoft.Json;
using weatherAssistant.Helpers.Interfaces;
using weatherAssistant.Models;

namespace weatherAssistant.Helpers
{
    public class JsonDownloader : IJsonDownloader
    {
        public T Download<T>(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    var sourceData = webClient.DownloadData(url);
                    T result;
                    try
                    {
                       result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(sourceData));
                    }
                    catch (Newtonsoft.Json.JsonReaderException e)
                    {
                        return default(T);
                    }
                    return result; 
                }
            }
            catch (ArgumentNullException e)
            {
                MessageBox.Show("Wprowadzono niepoprawny adres");
                return default(T);
            }

            catch (WebException e)
            {
                MessageBox.Show("Brak dostępu do internetu");
                return default(T);
            }

            catch (NotSupportedException e)
            {
                return default(T);
            }

        }
    }

}
