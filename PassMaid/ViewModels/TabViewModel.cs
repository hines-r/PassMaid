using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.ViewModels
{
    public class TabViewModel : Screen
    {
        private int _selectedIndex;

        public TabViewModel()
        {
            Init();
        }

        private void Init()
        {
            Tabs = new BindableCollection<ITab>
            {
                new VaultTabViewModel(),
                new GeneratorTabViewModel()
            };

            SelectedIndex = 0; // Sets the first tab to be selected
        }

        public BindableCollection<ITab> Tabs { get; private set; }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyOfPropertyChange(() => SelectedIndex);
            }
        }
    }
}
