using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using weatherAssistant.Models;
using weatherAssistant.ViewModels.Interfaces;
using weatherAssistant.Commands;
using weatherAssistant.Helpers;

namespace weatherAssistant.ViewModels
{
    class StormyMapViewModel: BaseViewModel, IPageViewModel
    {

        #region private fields

        private bool _directionAndSpeedOfStormCloudsCheckBoxChecked;
        private bool _groupsAndNumberOfThunderboltsCheckBoxChecked;
        private int _selectedTabIndex; //binding selected tabcontrol index
        private bool _staticMapRadioButtonChecked;
        private bool _animatedMapRadioButtonChecked;
        private int _mapZoomComboBoxSelectedIndex;
        private List<string> _mapsOfPoland;
        private List<string> _mapsOfEurope;
        #endregion

        #region public properties

        public string Name
        {
            get { return Properties.Resources.Menu_StormyMap; }
        }

        public ObservableCollection<StormyMap> StormyMaps { get; set; }

        public ObservableCollection<string> SelectedMaps { get; set; }

        public bool DirectionAndSpeedOfStormCloudsCheckBoxChecked
        {
            get { return _directionAndSpeedOfStormCloudsCheckBoxChecked; }
            set
            {
                _directionAndSpeedOfStormCloudsCheckBoxChecked = value;
                if (value)
                    GroupsAndNumberOfThunderboltsCheckBoxChecked = false;
                ChangeMap();
                OnPropertyChanged();
            }
        }

        public bool GroupsAndNumberOfThunderboltsCheckBoxChecked
        {
            get { return _groupsAndNumberOfThunderboltsCheckBoxChecked; }
            set
            {
                _groupsAndNumberOfThunderboltsCheckBoxChecked = value;
                if (value)
                    DirectionAndSpeedOfStormCloudsCheckBoxChecked = false;
                ChangeMap();
                OnPropertyChanged();
            }
        }

        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                ChangeMap();
                if (value == 0)
                {
                    SelectedMaps.Clear();
                    foreach (var map in _mapsOfPoland)
                        SelectedMaps.Add(map);                  
                }
                else
                {
                    SelectedMaps.Clear();
                    foreach (var map in _mapsOfEurope)
                        SelectedMaps.Add(map);
                }
                MapZoomComboBoxSelectedIndex = 0;
                OnPropertyChanged();
            }
        }

        public bool StaticMapRadioButtonChecked
        {
            get { return _staticMapRadioButtonChecked; }
            set
            {
                _staticMapRadioButtonChecked = value;
                ChangeMap();
                OnPropertyChanged();
            }
        }

        public bool AnimatedMapRadioButtonChecked
        {
            get { return _animatedMapRadioButtonChecked; }
            set
            {
                _animatedMapRadioButtonChecked = value;
                ChangeMap();
                OnPropertyChanged();
            }
        }

        public int MapZoomComboBoxSelectedIndex
        {
            get { return _mapZoomComboBoxSelectedIndex; }
            set
            {
                _mapZoomComboBoxSelectedIndex = value;
                ChangeMap();
                OnPropertyChanged();
            }
        }


        #endregion

        #region Command properties

        #endregion

        public StormyMapViewModel()
        {
            _mapsOfPoland = new List<string>()
            {
                "brak",
                "północny-zachód",
                "północny-wschód",
                "centrum",
                "południowy-zachód",
                "południowy-wschód"
            };
            _mapsOfEurope = new List<string>()
            {
                "Europy",
                "Austrii",
                "Belgii, Holandii, Luksemburga",
                "Bośni i Hercegowiny, Chorwacji, Słowenii",
                "Czech",
                "Francji",
                "Grecji",
                "Hiszpanii, Portugalii",
                "Niemiec",
                "Słowacji, Węgier",
                "Szwajcarii, Liechtensteinu",
                "Włoch",
                "Wysp Brytyjskich"
            };

            StormyMaps = new ObservableCollection<StormyMap>()
            {
                new StormyMap(Properties.Resources.MapOfPoland, "https://burze.dzis.net/burze_europa.gif"),
                new StormyMap(Properties.Resources.MapOfEurope, "https://burze.dzis.net/burze.gif")
            };
            
            SelectedMaps = new ObservableCollection<string>(_mapsOfPoland);
            StaticMapRadioButtonChecked = true;
        }

        #region methods

        private void ChangeMap()
        {
            if (SelectedTabIndex == 0)
            {
                if (StaticMapRadioButtonChecked)
                {
                    if (DirectionAndSpeedOfStormCloudsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_wektory.gif";
                                break;
                            case 1:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_wektory_polska_nw.gif";
                                break;
                            case 2:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_wektory_polska_ne.gif";
                                break;
                            case 3:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_wektory_polska_c.gif";
                                break;
                            case 4:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_wektory_polska_sw.gif";
                                break;
                            case 5:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_wektory_polska_se.gif";
                                break;
                        }
                    }

                    else if (GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_grupy.gif";
                                break;
                            case 1:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_grupy_polska_nw.gif";
                                break;
                            case 2:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_grupy_polska_ne.gif";
                                break;
                            case 3:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_grupy_polska_c.gif";
                                break;
                            case 4:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_grupy_polska_sw.gif";
                                break;
                            case 5:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_grupy_polska_se.gif";
                                break;
                        }
                    }

                    else if (!DirectionAndSpeedOfStormCloudsCheckBoxChecked &&
                             !GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze.gif";
                                break;
                            case 1:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_polska_nw.gif";
                                break;
                            case 2:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_polska_ne.gif";
                                break;
                            case 3:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_polska_c.gif";
                                break;
                            case 4:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_polska_sw.gif";
                                break;
                            case 5:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_polska_se.gif";
                                break;
                        }
                    }
                    
                }

                else
                {
                    if (DirectionAndSpeedOfStormCloudsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_wektory.gif";
                                break;
                            case 1:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_wektory_polska_nw.gif";
                                break;
                            case 2:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_wektory_polska_ne.gif";
                                break;
                            case 3:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_wektory_polska_c.gif";
                                break;
                            case 4:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_wektory_polska_sw.gif";
                                break;
                            case 5:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_wektory_polska_se.gif";
                                break;
                        }
                    }

                    else if (GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_grupy.gif";
                                break;
                            case 1:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_grupy_polska_nw.gif";
                                break;
                            case 2:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_grupy_polska_ne.gif";
                                break;
                            case 3:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_grupy_polska_c.gif";
                                break;
                            case 4:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_grupy_polska_sw.gif";
                                break;
                            case 5:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_grupy_polska_se.gif";
                                break;
                        }
                    }

                    else if (!DirectionAndSpeedOfStormCloudsCheckBoxChecked &&
                             !GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim.gif";
                                break;
                            case 1:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_polska_nw.gif";
                                break;
                            case 2:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_polska_ne.gif";
                                break;
                            case 3:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_polska_c.gif";
                                break;
                            case 4:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_polska_sw.gif";
                                break;
                            case 5:
                                StormyMaps[0].Url = "https://burze.dzis.net/burze_anim_polska_se.gif";
                                break;
                        }
                    }

                }
            }

            else
            {
                if (StaticMapRadioButtonChecked)
                {
                    if (DirectionAndSpeedOfStormCloudsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[1].Url = "https://burze.dzis.net/burze_europa_wektory.gif";
                                break;
                            case 1:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_austria_direction.gif";
                                break;
                            case 2:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_belgium_netherlands_luxembourg_direction.gif";
                                break;
                            case 3:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_bosnia_and_herzegovina_croatia_slovenia_direction.gif";
                                break;
                            case 4:
                                StormyMaps[1].Url = "https://burze.dzis.net/stormy_mapa_ceska_republika_vektory.gif";
                                break;
                            case 5:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_france_direction.gif";
                                break;
                            case 6:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_greece_direction.gif";
                                break;
                            case 7:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_spain_and_portugal_direction.gif";
                                break;
                            case 8:
                                StormyMaps[1].Url = "https://burze.dzis.net/sturm_deutschland_vektoren.gif";
                                break;
                            case 9:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_slovenia_hungary_direction.gif";
                                break;
                            case 10:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_switzerland_and_liechtenstein_direction.gif";
                                break;
                            case 11:
                                StormyMaps[1].Url = "https://burze.dzis.net/italia_fulmine_vettori.gif";
                                break;
                            case 12:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_british_isles_direction.gif";
                                break;
                        }
                    }

                    else if (GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[1].Url = "https://burze.dzis.net/burze_europa_grupy.gif";
                                break;
                            case 1:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_austria_grup.gif";
                                break;
                            case 2:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_belgium_netherlands_luxembourg_grup.gif";
                                break;
                            case 3:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_bosnia_and_herzegovina_croatia_slovenia_grup.gif";
                                break;
                            case 4:
                                StormyMaps[1].Url = "https://burze.dzis.net/stormy_mapa_ceska_republika_skupiny.gif";
                                break;
                            case 5:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_france_grup.gif";
                                break;
                            case 6:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_greece_grup.gif";
                                break;
                            case 7:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_spain_and_portugal_grup.gif";
                                break;
                            case 8:
                                StormyMaps[1].Url = "https://burze.dzis.net/sturm_deutschland_gruppen.gif";
                                break;
                            case 9:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_slovenia_hungary_grup.gif";
                                break;
                            case 10:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_switzerland_and_liechtenstein_grup.gif";
                                break;
                            case 11:
                                StormyMaps[1].Url = "https://burze.dzis.net/italia_fulmine_gruppi.gif";
                                break;
                            case 12:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_british_isles_grup.gif";
                                break;
                        }
                    }

                    else if (!DirectionAndSpeedOfStormCloudsCheckBoxChecked &&
                             !GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[1].Url = "https://burze.dzis.net/burze_europa.gif";
                                break;
                            case 1:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_austria.gif";
                                break;
                            case 2:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_belgium_netherlands_luxembourg.gif";
                                break;
                            case 3:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_bosnia_and_herzegovina_croatia_slovenia.gif";
                                break;
                            case 4:
                                StormyMaps[1].Url = "https://burze.dzis.net/stormy_mapa_ceska_republika.gif";
                                break;
                            case 5:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_france.gif";
                                break;
                            case 6:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_greece.gif";
                                break;
                            case 7:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_spain_and_portugal.gif";
                                break;
                            case 8:
                                StormyMaps[1].Url = "https://burze.dzis.net/sturm_deutschland.gif";
                                break;
                            case 9:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_slovenia_hungary.gif";
                                break;
                            case 10:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_switzerland_and_liechtenstein.gif";
                                break;
                            case 11:
                                StormyMaps[1].Url = "https://burze.dzis.net/italia_fulmine.gif";
                                break;
                            case 12:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_british_isles.gif";
                                break;
                        }
                    }
                    
                }

                else
                {
                    if (DirectionAndSpeedOfStormCloudsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[1].Url = "https://burze.dzis.net/burze_europa_anim_wektory.gif";
                                break;
                            case 1:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_austria_animation_direction.gif";
                                break;
                            case 2:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_belgium_netherlands_luxembourg_animation_direction.gif";
                                break;
                            case 3:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_bosnia_and_herzegovina_croatia_slovenia_animation_direction.gif";
                                break;
                            case 4:
                                StormyMaps[1].Url = "https://burze.dzis.net/stormy_mapa_ceska_republika_animace_vektory.gif";
                                break;
                            case 5:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_france_animation_direction.gif";
                                break;
                            case 6:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_greece_animation_direction.gif";
                                break;
                            case 7:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_spain_and_portugal_animation_direction.gif";
                                break;
                            case 8:
                                StormyMaps[1].Url = "https://burze.dzis.net/sturm_deutschland_lebhaft_vektoren.gif";
                                break;
                            case 9:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_slovenia_hungary_animation_direction.gif";
                                break;
                            case 10:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_switzerland_and_liechtenstein_animation_direction.gif";
                                break;
                            case 11:
                                StormyMaps[1].Url = "https://burze.dzis.net/italia_fulmine_lebhaft_vettori.gif";
                                break;
                            case 12:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_british_isles_animation_direction.gif";
                                break;
                        }
                    }

                    else if (GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[1].Url = "https://burze.dzis.net/burze_europa_anim_grupy.gif";
                                break;
                            case 1:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_austria_animation_grup.gif";
                                break;
                            case 2:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_belgium_netherlands_luxembourg_animation_grup.gif";
                                break;
                            case 3:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_bosnia_and_herzegovina_croatia_slovenia_animation_grup.gif";
                                break;
                            case 4:
                                StormyMaps[1].Url = "https://burze.dzis.net/stormy_mapa_ceska_republika_animace_skupiny.gif";
                                break;
                            case 5:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_france_animation_grup.gif";
                                break;
                            case 6:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_greece_animation_grup.gif";
                                break;
                            case 7:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_spain_and_portugal_animation_grup.gif";
                                break;
                            case 8:
                                StormyMaps[1].Url = "https://burze.dzis.net/sturm_deutschland_lebhaft_gruppen.gif";
                                break;
                            case 9:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_slovenia_hungary_animation_grup.gif";
                                break;
                            case 10:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_switzerland_and_liechtenstein_animation_grup.gif";
                                break;
                            case 11:
                                StormyMaps[1].Url = "https://burze.dzis.net/italia_fulmine_lebhaft_gruppi.gif";
                                break;
                            case 12:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_british_isles_animation_grup.gif";
                                break;
                        }
                    }

                    else if (!DirectionAndSpeedOfStormCloudsCheckBoxChecked &&
                             !GroupsAndNumberOfThunderboltsCheckBoxChecked)
                    {
                        switch (MapZoomComboBoxSelectedIndex)
                        {
                            case 0:
                                StormyMaps[1].Url = "https://burze.dzis.net/burze_europa_anim.gif";
                                break;
                            case 1:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_austria_animation.gif";
                                break;
                            case 2:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_belgium_netherlands_luxembourg_animation.gif";
                                break;
                            case 3:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_bosnia_and_herzegovina_croatia_slovenia_animation.gif";
                                break;
                            case 4:
                                StormyMaps[1].Url = "https://burze.dzis.net/stormy_mapa_ceska_republika_animace.gif";
                                break;
                            case 5:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_france_animation.gif";
                                break;
                            case 6:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_greece_animation.gif";
                                break;
                            case 7:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_spain_and_portugal_animation.gif";
                                break;
                            case 8:
                                StormyMaps[1].Url = "https://burze.dzis.net/sturm_deutschland_lebhaft.gif";
                                break;
                            case 9:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_slovenia_hungary_animation.gif";
                                break;
                            case 10:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_switzerland_and_liechtenstein_animation.gif";
                                break;
                            case 11:
                                StormyMaps[1].Url = "https://burze.dzis.net/italia_fulmine_lebhaft.gif";
                                break;
                            case 12:
                                StormyMaps[1].Url = "https://burze.dzis.net/storm_british_isles_animation.gif";
                                break;


                        }
                    }
                    
                }
            }
        }
        #endregion
    }
}