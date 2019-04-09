using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using weatherAssistant.Commands;
using weatherAssistant.ViewModels.Interfaces;

namespace weatherAssistant.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, IPageViewModel
    {
        #region private Fields

        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private string _sources;

        #endregion

        public MainWindowViewModel()
        {
            //Add available pages
            PageViewModels.Add(new WeatherForecastViewModel());
            PageViewModels.Add(new StormyMapViewModel());
            PageViewModels.Add(new StormSearchViewModel());
            PageViewModels.Add(new PlanTheTripViewModel());
            PageViewModels.Add(new NotificationsViewModel());

            //Set starting page
            CurrentPageViewModel = PageViewModels[0];
            Properties.Settings.Default.Language = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            changeSourcesInfo();
        }

        #region Command properties

        public string Name
        {
            get { return "Main Window"; }
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new BaseCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);

                }

                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Sources
        {
            get { return _sources; }
            set
            {
                _sources = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
            changeSourcesInfo();
        }

        private void changeSourcesInfo()
        {
            if (CurrentPageViewModel.ToString().Contains("WeatherForecast"))
                Sources = "OpenWeatherMap";
            else if (CurrentPageViewModel.ToString().Contains("StormyMap"))
                Sources = "burze.dzis.net";
            else if (CurrentPageViewModel.ToString().Contains("StormSearch"))
                Sources = "burze.dzis.net";
            else if (CurrentPageViewModel.ToString().Contains("PlanTheTrip"))
                Sources = "Mapbox, burze.dzis.net";
            else if (CurrentPageViewModel.ToString().Contains("Notifications"))
                Sources = "burze.dzis.net";
        }
        #endregion
    }
}
