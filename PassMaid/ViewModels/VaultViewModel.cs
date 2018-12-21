using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid.ViewModels
{
    public class VaultViewModel : Tab
    {
        private const string _TabName = "Vault";

        public VaultViewModel()
        {
            TabName = _TabName;
        }
    }
}
