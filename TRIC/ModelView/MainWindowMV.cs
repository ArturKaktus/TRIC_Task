using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TRIC.Service;
using tricLib;
using static tricLib.Class1;

namespace TRIC.ModelView
{
    public class MainWindowMV : INotifyPropertyChanged
    {
        private string _CountList = string.Empty;
        private RelayCommand _addListCommand;
        private Class1 _Class1;
        private List<Class1.Service> _ServiceListMain;
        private List<Class1.Service> _ServiceList;
        private List<Class1.Service> _ServiceActiveList;

        private Class1.Service _SelectedService;
        private string _ServiceName;
        private string _ServiveStatus;
        private string _ServiceDescription;
        private string _ServiceSLA;
        private string _ServiceTime;

        private bool isActive = false;
        public MainWindowMV()
        {

        }
        public string CountList { 
            get => _CountList;
            set
            {
                if (value != _CountList)
                {
                    _CountList = value;
                    OnPropertyChanged(nameof(CountList));
                }
            }
        }

        public Class1.Service SelectedService
        {
            get => _SelectedService;
            set
            {
                if (value == _SelectedService || value == null) return;

                _SelectedService = value;
                ServiceName = _SelectedService.Name;
                ServiceStatus = _SelectedService.ServiceStatus.ToString();
                ServiceDescription = _SelectedService.Description;
                ServiceTime = _SelectedService.UpdateTime.ToString(CultureInfo.InvariantCulture); 

                ServiceSLA = Convert.ToString(_Class1.GetPercentSlaByName(ServiceName), CultureInfo.InvariantCulture);
            }
        }
        public string ServiceName
        {
            get => _ServiceName;
            set
            {
                if (value != _ServiceName)
                {
                    _ServiceName = value;
                    OnPropertyChanged(nameof(ServiceName));
                }
            }
        }

        public string ServiceStatus
        {
            get => _ServiveStatus;
            set
            {
                if (value != _ServiveStatus)
                {
                    _ServiveStatus = value;
                    OnPropertyChanged(nameof(ServiceStatus));
                }
            }
        }

        public string ServiceSLA
        {
            get => _ServiceSLA;
            set
            {
                if (_ServiceSLA != value)
                {
                    _ServiceSLA = value;
                    OnPropertyChanged(nameof(ServiceSLA));
                }
            }
        }

        public string ServiceDescription
        {
            get => _ServiceDescription;
            set
            {
                if (_ServiceDescription != value)
                {
                    _ServiceDescription = value;
                    OnPropertyChanged(nameof(ServiceDescription));
                }
            }
        }

        public string ServiceTime
        {
            get => _ServiceTime;
            set
            {
                if (_ServiceTime != value)
                {
                    _ServiceTime = value;
                    OnPropertyChanged(nameof(ServiceTime));
                }
            }
        }

        public List<Class1.Service> ServiceList
        {
            get => _ServiceList;
            set
            {
                if (_ServiceList != value)
                {
                    _ServiceList = value;
                    OnPropertyChanged(nameof(ServiceList));
                }
            }
        }

        public void AddList()
        {
            _Class1 = new Class1(string.IsNullOrEmpty(CountList) ? 1 : int.Parse(CountList));
            _ServiceListMain = _Class1.ServiceList;
            _ServiceActiveList = _Class1.GetActualStatus();
            ServiceList = _ServiceListMain;
        }

        public void SaveToJson()
        {
            SaveFileDialog saveJson = new SaveFileDialog()
            {
                Filter = "json files (*.json)|*.json"
            };
            bool result = saveJson.ShowDialog() ?? false;
            if (result)
            {
                var json = JsonSerializer.Serialize(SelectedService);
                File.WriteAllText(saveJson.FileName, json);
            }
        }

        public void ActiveList()
        {
            if (isActive)
            {
                ServiceList = _ServiceListMain;
                isActive = false;
            }
            else
            {
                ServiceList = _ServiceActiveList;
                isActive = true;
            }
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
