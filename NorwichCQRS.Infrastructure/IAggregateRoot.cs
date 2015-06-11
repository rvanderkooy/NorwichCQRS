using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Infrastructure
{
    public interface IAggregateRoot
    {
        int Version { get; set; }
        DateTime DateTime { get; set; }
    }
}
