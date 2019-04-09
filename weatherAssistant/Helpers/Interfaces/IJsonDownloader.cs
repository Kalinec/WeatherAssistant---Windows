using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherAssistant.Models;

namespace weatherAssistant.Helpers.Interfaces
{
    public interface IJsonDownloader
    {
        T Download<T>(string url);
    }
}
