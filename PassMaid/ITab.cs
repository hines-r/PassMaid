using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid
{
    public interface ITab
    {
        string TabName { get; set; }
    }

    public abstract class Tab : Screen, ITab
    {
        public string TabName { get; set; }
    }
}
