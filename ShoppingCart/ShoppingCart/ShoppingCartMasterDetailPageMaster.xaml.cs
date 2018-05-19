using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShoppingCart
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingCartMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public ShoppingCartMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new ShoppingCartMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class ShoppingCartMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<ShoppingCartMasterDetailPageMenuItem> MenuItems { get; set; }

            public ShoppingCartMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<ShoppingCartMasterDetailPageMenuItem>(new[]
                {
                    new ShoppingCartMasterDetailPageMenuItem { Id = 0, Title = "Page 1" },
                    new ShoppingCartMasterDetailPageMenuItem { Id = 1, Title = "Page 2" },
                    new ShoppingCartMasterDetailPageMenuItem { Id = 2, Title = "Page 3" },
                    new ShoppingCartMasterDetailPageMenuItem { Id = 3, Title = "Page 4" },
                    new ShoppingCartMasterDetailPageMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}