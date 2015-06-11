using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Infrastructure.Providers
{
    public interface IIdProvider
    {
        string GetNewID();
    }
}
