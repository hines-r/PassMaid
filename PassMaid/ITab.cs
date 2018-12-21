using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassMaid
{
    public interface ITab
    {
        string Name { get; set; }
    }

    public abstract class Tab : ITab
    {
        public string Name { get; set; }
    }
}
