using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork.ViewModel
{
    class MainViewModel : ViewModel
    {
        public MainViewModel()
        {

        }

        private string _Id;

        public string Id 
        {
            get => _Id;
            set => SetOnPropertyChanged(ref _Id, value);
        }
    }
}