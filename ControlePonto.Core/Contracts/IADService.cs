using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlePonto.Core.Contracts
{
    public interface IADService
    {
        Usuario GetByEmail(string email);
    }
}
