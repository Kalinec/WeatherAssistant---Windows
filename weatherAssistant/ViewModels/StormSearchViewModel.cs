using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using weatherAssistant.Commands;
using weatherAssistant.Services;
using weatherAssistant.StormApi;
using weatherAssistant.ViewModels.Interfaces;

namespace weatherAssistant.ViewModels
{
    class StormSearchViewModel : BaseViewModel, IPageViewModel
    {
        #region private fields

        private MyComplexTypeBurza _stormInfo;
        private MyComplexTypeMiejscowosc _locationInfo;

        private string _city;
        private double _longitude;
        private double _latitude;
        private int _radius;
        private int _indexRadius;
        private int? _numberOfThunderbolts;
        private float? _nearestThunderbolt;
        private string _directionToTheNearestThunderbolt;
        private int? _time;

        //warnings fields
        private int? _stormDegree;
        private string _storm_from_day;
        private string _storm_to_day;
        private int? _frostDegree;
        private string _frost_from_day;
        private string _frost_to_day;
        private int? _rainDegree;
        private string _rain_from_day;
        private string _rain_to_day;
        private int? _tornadoDegree;
        private string _tornado_from_day;
        private string _tornado_to_day;
        private int? _heatDegree;
        private string _heat_from_day;
        private string _heat_to_day;
        private int? _windDegree;
        private string _wind_from_day;
        private string _wind_to_day;

        private string _stormDescription;
        private string _frostDescription;
        private string _rainDescription;
        private string _tornadoDescription;
        private string _heatDescription;
        private string _windDescription;

        //warnings visibility
        private bool _displayStorm;
        private bool _displayFrost;
        private bool _displayRain;
        private bool _displayTornado;
        private bool _displayHeat;
        private bool _displayWind;


        #endregion

        #region public properties

        public string Name
        {
            get { return Properties.Resources.Menu_StormSearch; }
        }

        public int? NumberOfThunderbolts
        {
            get { return _numberOfThunderbolts; }
            set
            {
                _numberOfThunderbolts = value;
                OnPropertyChanged();
            }
        }

        public float? NearestThunderbolt
        {
            get { return _nearestThunderbolt;}
            set
            {
                _nearestThunderbolt = value;
                OnPropertyChanged();
            }
        }

        public string DirectionToTheNearestThunderbolt
        {
            get { return _directionToTheNearestThunderbolt;}
            set
            {
                _directionToTheNearestThunderbolt = value;
                OnPropertyChanged();
            }
        }

        public int? Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public double Longitude
        {
            get { return _longitude; }
            set
            {
                _longitude = value;
                OnPropertyChanged();
            }
        }

        public double Latitude
        {
            get { return _latitude; }
            set
            {
                _latitude = value;
                OnPropertyChanged();
            }
        }

        public int Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged();
            }
        }

        public int IndexRadius
        {
            get { return _indexRadius; }
            set
            {
                _indexRadius = value;
                OnPropertyChanged();
            }
        }

        //warning properties
        public int? StormDegree
        {
            get { return _stormDegree; }
            set
            {
                _stormDegree = value;
                OnPropertyChanged();

            }
        }

        public string StormFromDay
        {
            get { return _storm_from_day; }
            set
            {
                _storm_from_day = value;
                OnPropertyChanged();
            }
        }

        public string StormToDay
        {
            get { return _storm_to_day; }
            set
            {
                _storm_to_day = value;
                OnPropertyChanged();
            }
        }

        public int? FrostDegree
        {
            get { return _frostDegree; }
            set
            {
                _frostDegree = value;
                OnPropertyChanged();
            }
        }

        public string FrostFromDay
        {
            get { return _frost_from_day; }
            set
            {
                _frost_from_day = value;
                OnPropertyChanged();
            }
        }

        public string FrostToDay
        {
            get { return _frost_to_day; }
            set
            {
                _frost_to_day = value;
                OnPropertyChanged();
            }
        }

        public int? RainDegree
        {
            get { return _rainDegree; }
            set
            {
                _rainDegree = value;
                OnPropertyChanged();
            }
        }

        public string RainFromDay
        {
            get { return _rain_from_day; }
            set
            {
                _rain_from_day = value;
                OnPropertyChanged();
            }
        }

        public string RainToDay
        {
            get { return _rain_to_day; }
            set
            {
                _rain_to_day = value;
                OnPropertyChanged();
            }
        }

        public int? TornadoDegree
        {
            get { return _tornadoDegree; }
            set
            {
                _tornadoDegree = value;
                OnPropertyChanged();
            }
        }

        public string TornadoFromDay
        {
            get { return _tornado_from_day; }
            set
            {
                _tornado_from_day = value;
                OnPropertyChanged();
            }
        }

        public string TornadoToDay
        {
            get { return _tornado_to_day; }
            set
            {
                _tornado_to_day = value;
                OnPropertyChanged();
            }
        }

        public int? HeatDegree
        {
            get { return _heatDegree; }
            set
            {
                _heatDegree = value;
                OnPropertyChanged();
            }
        }

        public string HeatFromDay
        {
            get { return _heat_from_day; }
            set
            {
                _heat_from_day = value;
                OnPropertyChanged();
            }
        }

        public string HeatToDay
        {
            get { return _heat_to_day; }
            set
            {
                _heat_to_day = value;
                OnPropertyChanged();
            }
        }

        public int? WindDegree
        {
            get { return _windDegree; }
            set
            {
                _windDegree = value;
                OnPropertyChanged();
            }
        }

        public string WindFromDay
        {
            get { return _wind_from_day; }
            set
            {
                _wind_from_day = value;
                OnPropertyChanged();
            }
        }

        public string WindToDay
        {
            get { return _wind_to_day; }
            set
            {
                _wind_to_day = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayStorm
        {
            get { return _displayStorm; }
            set
            {
                _displayStorm = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayFrost
        {
            get { return _displayFrost; }
            set
            {
                _displayFrost = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayRain
        {
            get { return _displayRain; }
            set
            {
                _displayRain = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayTornado
        {
            get { return _displayTornado; }
            set
            {
                _displayTornado = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayHeat
        {
            get { return _displayHeat; }
            set
            {
                _displayHeat = value;
                OnPropertyChanged();
            }
        }

        public bool DisplayWind
        {
            get { return _displayWind; }
            set
            {
                _displayWind = value;
                OnPropertyChanged();
            }
        }

        public string StormDescription
        {
            get { return _stormDescription; }
            set
            {
                _stormDescription = value;
                OnPropertyChanged();
            }
        }

        public string FrostDescription
        {
            get { return _frostDescription; }
            set
            {
                _frostDescription = value;
                OnPropertyChanged();
            }
        }

        public string RainDescription
        {
            get { return _rainDescription; }
            set
            {
                _rainDescription = value;
                OnPropertyChanged();
            }
        }

        public string TornadoDescription
        {
            get { return _tornadoDescription; }
            set
            {
                _tornadoDescription = value;
                OnPropertyChanged();
            }
        }

        public string HeatDescription
        {
            get { return _heatDescription; }
            set
            {
                _heatDescription = value;
                OnPropertyChanged();
            }
        }

        public string WindDescription
        {
            get { return _windDescription; }
            set
            {
                _windDescription = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> RadiusItems { get; set; }
        public MyComplexTypeOstrzezenia WarningInfo = new MyComplexTypeOstrzezenia();
        #endregion

        public StormSearchViewModel()
        {
            _stormInfo = new MyComplexTypeBurza();
            _locationInfo = new MyComplexTypeMiejscowosc();

            RadiusItems = new ObservableCollection<int>();
            initializeItems(); 
            
            DisplayStorm = false;
            DisplayFrost = false;
            DisplayRain = false;
            DisplayTornado = false;
            DisplayHeat = false;
            DisplayWind = false; 
        }

        #region command properties

        public ICommand CheckThunderboltCommand
        {
            get { return new BaseCommand(CheckThunderbolt); }
        }

        #endregion

        #region methods

        private void initializeItems()
        {
            //Add items to RadiusItems
            for (int i = 5; i < 100; i += 5)
                RadiusItems.Add(i);
            for (int i = 100; i <= 300; i += 50)
                RadiusItems.Add(i);

            //Set default ComboBox value
            IndexRadius = 4;

            //Initialize warnings display
            DisplayStorm = true;
            DisplayFrost = true;
            DisplayRain = true;
            DisplayTornado = true;
            DisplayHeat = true;
            DisplayWind = true;

        }

        private void CheckThunderbolt()
        {
            if (City == null)
            {
                MessageBox.Show("City jest nullem");
                return;
            }
            _locationInfo = StormService.Instance.GetLocationByCityName(City);

            if (Longitude != 0 && Latitude != 0)
            {
                _locationInfo.x = (float)Longitude;
                _locationInfo.y = (float)Latitude;
            }

            _stormInfo = StormService.Instance.GetStormInfo(Radius, _locationInfo);
            CheckWarning();
            UpdateData();
        }

        private void CheckWarning()
        {
            WarningInfo = StormService.Instance.GetWeatherWarnings(_locationInfo);
        }

        private void UpdateData()
        {
            
            Latitude = Double.Parse(_locationInfo.y.ToString());
            Longitude = Double.Parse(_locationInfo.x.ToString());
            Time = _stormInfo.okres;
            NearestThunderbolt = _stormInfo.odleglosc;
            DirectionToTheNearestThunderbolt = _stormInfo.kierunek;
            NumberOfThunderbolts = _stormInfo.liczba;

            WindFromDay = WarningInfo.wiatr_od_dnia;

            //warnings
            StormDegree = WarningInfo.burza;
            StormFromDay = WarningInfo.burza_od_dnia;
            StormToDay = WarningInfo.burza_do_dnia;
            FrostDegree = WarningInfo.mroz;
            FrostFromDay = WarningInfo.mroz_od_dnia;
            FrostToDay = WarningInfo.mroz_do_dnia;
            RainDegree = WarningInfo.opad;
            RainFromDay = WarningInfo.opad_od_dnia;
            RainToDay = WarningInfo.opad_do_dnia;
            TornadoDegree = WarningInfo.traba;
            TornadoFromDay = WarningInfo.traba_od_dnia;
            TornadoToDay = WarningInfo.traba_do_dnia;
            HeatDegree = WarningInfo.upal;
            HeatFromDay = WarningInfo.upal_od_dnia;
            HeatToDay = WarningInfo.upal_do_dnia;
            WindDegree = WarningInfo.wiatr;
            WindFromDay = WarningInfo.wiatr_od_dnia;
            WindToDay = WarningInfo.wiatr_do_dnia;

            SetWarningsVisibility();
            SetWarningsDescriptions();

            if(City.Equals("test"))
                FakeSetWarningDescriptions();

        }

        private void SetWarningsVisibility()
        {
            if (StormDegree != 0)
                DisplayStorm = true;
            else
                DisplayStorm = false;

            if (FrostDegree != 0)
                DisplayFrost = true;
            else
                DisplayFrost = false;

            if (RainDegree != 0)
                DisplayRain = true;
            else
                DisplayRain = false;

            if (TornadoDegree != 0)
                DisplayTornado = true;
            else
                DisplayTornado = false;

            if (HeatDegree != 0)
                DisplayHeat = true;
            else
                DisplayHeat = false;

            if (WindDegree != 0)
                DisplayWind = true;
            else
                DisplayWind = false;
        }

        private void SetWarningsDescriptions()
        {
            if (StormDegree != 0)
            {
                if (StormDegree == 1)
                    StormDescription = Properties.Resources.Warning_Storms1;
                else if (StormDegree == 2)
                    StormDescription = Properties.Resources.Warning_Storms2;
                else if (StormDegree == 3)
                    StormDescription = Properties.Resources.Warning_Storms3;


            }

            if (FrostDegree != 0)
            {
                if (FrostDegree == 1)
                    FrostDescription = Properties.Resources.Warning_Frost1;
                else if (FrostDegree == 2)
                    FrostDescription = Properties.Resources.Warning_Frost2;
                else if (StormDegree == 3)
                    FrostDescription = Properties.Resources.Warning_Frost3;
            }

            if (RainDegree != 0)
            {
                if (RainDegree == 1)
                    RainDescription = Properties.Resources.Warning_Rain1;
                else if (RainDegree == 2)
                    RainDescription = Properties.Resources.Warning_Rain2;
                else if (StormDegree == 3)
                    RainDescription = Properties.Resources.Warning_Rain3;
            }

            if (TornadoDegree != 0)
            {
                if (TornadoDegree == 1)
                    TornadoDescription = Properties.Resources.Warning_Tornado1;
                else if (TornadoDegree == 2)
                    TornadoDescription = Properties.Resources.Warning_Tornado2;
                else if (TornadoDegree == 3)
                    TornadoDescription = Properties.Resources.Warning_Tornado3;
            }

            if (HeatDegree != 0)
            {
                if (HeatDegree == 1)
                    HeatDescription = Properties.Resources.Warning_Heat1;
                else if (HeatDegree == 2)
                    HeatDescription = Properties.Resources.Warning_Heat2;
                else if (HeatDegree == 3)
                    HeatDescription = Properties.Resources.Warning_Heat3;
            }

            if (WindDegree != 0)
            {
                if (WindDegree == 1)
                    WindDescription = Properties.Resources.Warning_Wind1;
                else if (WindDegree == 2)
                    WindDescription = Properties.Resources.Warning_Wind2;
                else if (WindDegree == 3)
                    WindDescription = Properties.Resources.Warning_Wind3;
            }
        }

        private void FakeSetWarningDescriptions()
        {       
                    StormDescription = Properties.Resources.Warning_Storms1;
                    FrostDescription = Properties.Resources.Warning_Frost3;
                    RainDescription = Properties.Resources.Warning_Rain3;   
                    TornadoDescription = Properties.Resources.Warning_Tornado3;
                    HeatDescription = Properties.Resources.Warning_Heat3;
                    WindDescription = Properties.Resources.Warning_Wind3;  
        }
        #endregion

    }
}
