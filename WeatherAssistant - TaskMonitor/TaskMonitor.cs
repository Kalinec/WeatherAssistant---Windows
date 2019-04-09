using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using MS.WindowsAPICodePack.Internal;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using WeatherAssistant___TaskMonitor.StormAPI;

namespace WeatherAssistant___TaskMonitor
{
    class TaskMonitor
    {
        private static double _latitude;
        private static double _longitude;
        private static int _radius;
        private static bool _frostChecked;
        private static bool _heatChecked;
        private static bool _windChecked;
        private static bool _rainChecked;
        private static bool _stormsChecked;
        private static bool _tornadoChecked;
        private static MyComplexTypeBurza _stormInfo;
        private static MyComplexTypeOstrzezenia _warningsInfo;

        static void Main(string[] args)
        {
            _latitude = double.Parse(args[0], System.Globalization.CultureInfo.InvariantCulture);
            _longitude = double.Parse(args[1], System.Globalization.CultureInfo.InvariantCulture);
            _radius = int.Parse(args[2]);
            _frostChecked = bool.Parse(args[3]);
            _heatChecked = bool.Parse(args[4]);
            _windChecked = bool.Parse(args[5]);
            _rainChecked = bool.Parse(args[6]);
            _stormsChecked = bool.Parse(args[7]);
            _tornadoChecked = bool.Parse(args[8]);
        
            ShortcutCreator.TryCreateShortcut("WeatherMonitoring.App", "WeatherMonitoring");
            getStormInfo();
            checkStormInfo();
            getWarningsInfo();
            checkWarningsInfo();
        }

        private static void getStormInfo()
        {
            _stormInfo = StormService.Instance.GetStormInfo(_radius, _latitude, _longitude);
        }

        private static void checkStormInfo()
        {
            if (_stormInfo.liczba != 0)
                displayStormNotification();
        }

        private static void getWarningsInfo()
        {
            _warningsInfo = StormService.Instance.GetWeatherWarnings(_latitude, _longitude);
        }

        private static void checkWarningsInfo()
        {
            if (_warningsInfo.burza.HasValue && _warningsInfo.burza != 0 && _stormsChecked)
                displayStormWarningNotification();
            if (_warningsInfo.mroz.HasValue && _warningsInfo.mroz != 0 && _frostChecked)
                displayFrostWarningNotification();
            if (_warningsInfo.opad.HasValue && _warningsInfo.opad != 0 && _rainChecked)
                displayRainWarningNotification();
            if (_warningsInfo.traba.HasValue && _warningsInfo.traba != 0 && _tornadoChecked)
                displayTornadoWarningNotification();
            if (_warningsInfo.upal.HasValue && _warningsInfo.upal != 0 && _heatChecked)
                displayHeatWarningNotification();
            if (_warningsInfo.wiatr.HasValue && _warningsInfo.wiatr != 0 && _windChecked)
                displayWindWarningNotification();
        }

        private static void displayStormNotification()
        {
            string title = "Wykryto wyładowanie atmosferyczne!";
            StringBuilder message = new StringBuilder();
            message.Append("Zarejestrowano ");
            message.Append(_stormInfo.liczba);
            message.Append(" wyładowań atmosferycznych w okolicy. Najbliższe ");
            message.Append(Math.Round(_stormInfo.odleglosc.Value, 2));
            message.Append(" km w kierunku ");
            message.Append(_stormInfo.kierunek);
            message.Append(" od podanej lokalizacji");

            ShowToast("WeatherMonitoring.App",
                title,
                message.ToString(),
                Path.GetFullPath("if_weather-frost.png"));
        }

        private static void displayStormWarningNotification()
        {
            string title = string.Format("Burze, {0} stopień zagrożenia", _warningsInfo.burza);
            StringBuilder message = new StringBuilder();
            message.Append(Environment.NewLine);
            message.Append("Komunikat obowiązuje:");
            message.Append(Environment.NewLine);
            message.Append(string.Format("Od: {0}", _warningsInfo.burza_od_dnia));
            message.Append(Environment.NewLine);
            message.Append(string.Format("Do: {0}", _warningsInfo.burza_do_dnia));

            ShowToast("WeatherMonitoring.App",
                title,
                message.ToString(),
                Path.GetFullPath("if_weather-storm.png"));
        }

        private static void displayFrostWarningNotification()
        {
            string title = string.Format("Duże mrozy, {0} stopień zagrożenia", _warningsInfo.mroz);
            StringBuilder message = new StringBuilder();
            message.Append(Environment.NewLine);
            message.Append("Komunikat obowiązuje:");
            message.Append(Environment.NewLine);
            message.Append(string.Format("Od: {0}", _warningsInfo.mroz_od_dnia));
            message.Append(Environment.NewLine);
            message.Append(string.Format("Do: {0}", _warningsInfo.mroz_do_dnia));

            ShowToast("WeatherMonitoring.App",
                title,
                message.ToString(),
                Path.GetFullPath("if_weather-frost.png"));
        }

        private static void displayRainWarningNotification()
        {
            string title = string.Format("Duże opady, {0} stopień zagrożenia", _warningsInfo.opad);
            StringBuilder message = new StringBuilder();
            message.Append(Environment.NewLine);
            message.Append("Komunikat obowiązuje:");
            message.Append(Environment.NewLine);
            message.Append(string.Format("Od: {0}", _warningsInfo.opad_od_dnia));
            message.Append(Environment.NewLine);
            message.Append(string.Format("Do: {0}", _warningsInfo.opad_do_dnia));

            ShowToast("WeatherMonitoring.App",
                title,
                message.ToString(),
                Path.GetFullPath("if_weather-rain.png"));
        }

        private static void displayTornadoWarningNotification()
        {
            string title = string.Format("Trąby powietrzne, {0} stopień zagrożenia", _warningsInfo.traba);
            StringBuilder message = new StringBuilder();
            message.Append(Environment.NewLine);
            message.Append("Komunikat obowiązuje:");
            message.Append(Environment.NewLine);
            message.Append(string.Format("Od: {0}", _warningsInfo.traba_od_dnia));
            message.Append(Environment.NewLine);
            message.Append(string.Format("Do: {0}", _warningsInfo.traba_do_dnia));

            ShowToast("WeatherMonitoring.App",
                title,
                message.ToString(),
                Path.GetFullPath("if_weather-tornado.png"));
        }

        private static void displayHeatWarningNotification()
        {
            string title = string.Format("Upały, {0} stopień zagrożenia", _warningsInfo.upal);
            StringBuilder message = new StringBuilder();
            message.Append(Environment.NewLine);
            message.Append("Komunikat obowiązuje:");
            message.Append(Environment.NewLine);
            message.Append(string.Format("Od: {0}", _warningsInfo.upal_od_dnia));
            message.Append(Environment.NewLine);
            message.Append(string.Format("Do: {0}", _warningsInfo.upal_do_dnia)); 

            ShowToast("WeatherMonitoring.App",
                title,
                message.ToString(),
                Path.GetFullPath("if_weather-heat.png"));
        }

        private static void displayWindWarningNotification()
        {
            string title = string.Format("Silny wiatr, {0} stopień zagrożenia", _warningsInfo.wiatr);
            StringBuilder message = new StringBuilder();
            message.Append(Environment.NewLine);
            message.Append("Komunikat obowiązuje:");
            message.Append(Environment.NewLine);
            message.Append(string.Format("Od: {0}", _warningsInfo.wiatr_od_dnia));
            message.Append(Environment.NewLine);
            message.Append(string.Format("Do: {0}", _warningsInfo.wiatr_do_dnia));

            ShowToast("WeatherMonitoring.App",
                title,
                message.ToString(),
                Path.GetFullPath("if_weather-wind.png"));
        }

        private static void ShowToast(string appId, string title, string message, string image)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);

            //Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");
            stringElements[0].AppendChild(toastXml.CreateTextNode(title));
            stringElements[1].AppendChild(toastXml.CreateTextNode(message));

            // Specify the absolute path to an image
            String imagePath = "file:///" + image;
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");
            imageElements[0].Attributes.GetNamedItem("src").NodeValue = imagePath;

            // Create the toast and attach event listeners
            ToastNotification toast = new ToastNotification(toastXml);

            ToastEvents events = new ToastEvents();

            toast.Activated += events.ToastActivated;
            toast.Dismissed += events.ToastDismissed;
            toast.Failed += events.ToastFailed;

            // Show the toast
            ToastNotificationManager.CreateToastNotifier(appId).Show(toast);
        }
    }
}
