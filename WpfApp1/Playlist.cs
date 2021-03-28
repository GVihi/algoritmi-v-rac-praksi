using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApp1
{
    public class Playlist
    {
        public ObservableCollection<Media> pMedia { get; set; } = new ObservableCollection<Media>();
    }
}
