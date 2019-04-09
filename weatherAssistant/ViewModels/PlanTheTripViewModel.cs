using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weatherAssistant.ViewModels.Interfaces;


namespace weatherAssistant.ViewModels
{
    class PlanTheTripViewModel: BaseViewModel, IPageViewModel
    {

        #region private fields

        private string _sourceCity;
        private string _sourceLatitude;
        private string _sourceLongitude;
        private string _destinationCity;
        private string _destinationLatitude;
        private string _destinationLongitude;
        #endregion

        #region public properties

        public string Name
        {
            get { return Properties.Resources.Menu_PlanTheTrip; }
        }
        #endregion

        public PlanTheTripViewModel()
        {
        }
    }
}
