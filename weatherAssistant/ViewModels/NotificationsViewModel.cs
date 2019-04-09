using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32.TaskScheduler;
using weatherAssistant.Commands;
using weatherAssistant.Services;
using weatherAssistant.StormApi;
using weatherAssistant.ViewModels.Interfaces;

namespace weatherAssistant.ViewModels
{
    class NotificationsViewModel : BaseViewModel, IPageViewModel
    {
        #region private fields
        private string _place;
        private bool _frostIsChecked;
        private bool _heatIsChecked;
        private bool _windIsChecked;
        private bool _rainIsChecked;
        private bool _stormsIsChecked;
        private bool _tornadoIsChecked;
        private int _selectedRadius;
        private int _indexRadius;
        private int _selectedFrequency;
        private int _indexUpdates;
        private MyComplexTypeMiejscowosc _locationInfo;
        #endregion

        #region public properties
        public string Name
        { get { return Properties.Resources.Menu_Notifications; } }

        public string Place
        {
            get { return _place; }
            set
            {
                _place = value;
                OnPropertyChanged();
            }
        }

        public bool FrostIsChecked
        {
            get { return _frostIsChecked; }
            set
            {
                _frostIsChecked = value;
                OnPropertyChanged();
            }
        }

        public bool HeatIsChecked
        {
            get { return _heatIsChecked; }
            set
            {
                _heatIsChecked = value;
                OnPropertyChanged();
            }
        }

        public bool WindIsChecked
        {
            get { return _windIsChecked; }
            set
            {
                _windIsChecked = value;
                OnPropertyChanged();
            }
        }

        public bool RainIsChecked
        {
            get { return _rainIsChecked; }
            set
            {
                _rainIsChecked = value;
                OnPropertyChanged();
            }
        }

        public bool StormsIsChecked
        {
            get { return _stormsIsChecked; }
            set
            {
                _stormsIsChecked = value;
                OnPropertyChanged();
            }
        }

        public bool TornadoIsChecked
        {
            get { return _tornadoIsChecked; }
            set
            {
                _tornadoIsChecked = value;
                OnPropertyChanged();
            }
        }

        public int SelectedRadius
        {
            get { return _selectedRadius; }
            set
            {
                _selectedRadius = value;
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

        public int SelectedFrequency
        {
            get { return _selectedFrequency; }
            set
            {
                _selectedFrequency = value;
                OnPropertyChanged();
            }
        }

        public int IndexUpdates
        {
            get { return _indexUpdates; }
            set
            {
                _indexUpdates = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> RadiusItems { get; set; }
        public ObservableCollection<int> UpdatesFrequencyItems { get; set; }

        #endregion

        #region Commands Properties
        public ICommand ConfirmCommand
        {
            get { return new BaseCommand(CreateMonitoringTask); }
        }

        public ICommand CancelCommand
        {
            get { return new BaseCommand(CancelMonitoringTask);}
        }
        #endregion

        public NotificationsViewModel()
        {
            RadiusItems = new ObservableCollection<int>();
            UpdatesFrequencyItems = new ObservableCollection<int>();
            initializeItems();
        }

        private void initializeItems()
        {
            //Add items to RadiusItems
            for (int i = 5; i < 100; i += 5)
                RadiusItems.Add(i);
            for(int i = 100; i <= 300; i += 50)
                RadiusItems.Add(i);

            //Default values of ComboBoxes
            IndexRadius = 4;
            IndexUpdates = 0;

            //Add items to UpdatesFrequencyItems
            UpdatesFrequencyItems.Add(1);
            UpdatesFrequencyItems.Add(15);
            UpdatesFrequencyItems.Add(30);
            UpdatesFrequencyItems.Add(60);
            UpdatesFrequencyItems.Add(90);
            UpdatesFrequencyItems.Add(120);
            UpdatesFrequencyItems.Add(180);
            UpdatesFrequencyItems.Add(240);
            UpdatesFrequencyItems.Add(360);

        }

        private void CreateMonitoringTask()
        {
           
            using (TaskService taskService = new TaskService())
            {
                TaskDefinition taskDefinition = taskService.NewTask();
                taskDefinition.RegistrationInfo.Description = "Background monitoring storms and weather warnings";

                TimeTrigger trigger = new TimeTrigger();
                trigger.StartBoundary = DateTime.Now;
                trigger.Repetition.Interval = TimeSpan.FromMinutes(SelectedFrequency);
                taskDefinition.Triggers.Add(trigger);
                string path = Path.GetFullPath("WeatherAssistant - TaskMonitor.exe");
                string parameters = getArguments();
                taskDefinition.Actions.Add(new ExecAction(@path, parameters, null));
                taskService.RootFolder.RegisterTaskDefinition("WeatherMonitoring", taskDefinition);
            }
        }

        private void CancelMonitoringTask()
        {
            using (TaskService taskService = new TaskService())
            {
                if(taskService.GetTask("WeatherMonitoring") != null)
                    taskService.RootFolder.DeleteTask("WeatherMonitoring");
            }
        }

        private string getArguments()
        {
            _locationInfo = StormService.Instance.GetLocationByCityName(Place);
            StringBuilder argumentsBuilder = new StringBuilder();
            argumentsBuilder.Append(_locationInfo.y);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(_locationInfo.x);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(SelectedRadius);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(FrostIsChecked);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(HeatIsChecked);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(WindIsChecked);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(RainIsChecked);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(StormsIsChecked);
            argumentsBuilder.Append(" ");
            argumentsBuilder.Append(TornadoIsChecked);

            return argumentsBuilder.ToString();
        }
    }
}
