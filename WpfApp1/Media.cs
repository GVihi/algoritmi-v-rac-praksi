using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApp1
{
    public class Media : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        private string naslov = "";
        private string reziser = "";
        private string lokacija = "";
        private int leto = 9999;
        private int dolzina = 90;
        private bool predvajano = false;
        
        public string Title
        {
            get
            {
                return naslov;
            }

            set
            {
                naslov = value;
            }
        }
        
        public string Director
        {
            get
            {
                return reziser;
            }

            set
            {
                reziser = value;
            }
        }
        
        public string  DiskLocation
        {
            get
            {
                return lokacija;
            }

            set
            {
                lokacija = value;
            }
        }
        
        public int Year
        {
            get
            {
                return leto;
            }

            set
            {
                leto = value;
            }
        }
        
        public int Lenght
        {
            get
            {
                return dolzina;
            }

            set
            {
                dolzina = value;
            }
        }

        public bool Played
        {
            get
            {
                return predvajano;
            }

            set
            {
                predvajano = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Played"));
            }
        }
    }
}
