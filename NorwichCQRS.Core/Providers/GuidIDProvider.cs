using NorwichCQRS.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.Providers
{
    public class GuidIDProvider : IIdProvider
    {
        public string GetNewID()
        {
            return Convert.ToString(Guid.NewGuid());
        }
    }
}
