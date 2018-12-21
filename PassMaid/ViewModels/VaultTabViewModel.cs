using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.ViewModels
{
    public class VaultTabViewModel : Tab
    {
        private const string _TabName = "Vault";

        public VaultTabViewModel()
        {
            TabName = _TabName;
        }
    }
}
