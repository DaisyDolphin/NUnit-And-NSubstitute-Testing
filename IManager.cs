using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuilding
{
    public interface IManager // would be <<abstract>> class,
                              // however atm it's an interface so does nothing
    {
        string GetStatus();
    }
}
