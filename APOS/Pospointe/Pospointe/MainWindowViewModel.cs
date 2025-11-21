using Pospointe.PosMenu;
using Pospointe.Trans_Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pospointe
{
    class MainWindowViewModel : INotifyPropertyChanged

    {
        public TransData.Root invoice { get; set; }
        FrmViewRecall ff = new FrmViewRecall();
        private bool _showDialog;
        public bool ShowDialog
        {
            get => _showDialog;
            set
            {
                _showDialog = value;
                OnPropertyChanged(nameof(ShowDialog));
            }
        }

        public object UserControlContent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            ff.SetData(invoice);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            // Set initial state
            ShowDialog = false;
           
           



            UserControlContent = ff;

        }
    }
}
