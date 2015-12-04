using NorwichCQRS.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.Providers
{
    public class DilatingDateTimeProvider:IDateTimeProvider
    {
        private DateTime StartTime { get; set; }
        private TimeSpan DilatePer { get;set;}
        private TimeSpan DilateAmount { get; set; }

        public DilatingDateTimeProvider(DateTime startTime, TimeSpan dilatePer, TimeSpan dilateAmount)
        {
            throw new NotImplementedException();

            //this.StartTime = startTime;
            //this.DilatePer = dilatePer;
            //this.DilateAmount = dilateAmount;            
        }

        public DateTime CurrentDateTime
        {
            get
            {
                throw new NotImplementedException();

                //long differenceTicks = DateTime.UtcNow.Ticks - this.StartTime.Ticks;

                //var units = differenceTicks / DilatePer.Ticks;

                //long newTicks = units * DilateAmount.Ticks;

                //DateTime dateTime = new DateTime(newTicks, DateTimeKind.Utc);

                //return dateTime;
            }
        }
    }
}
