using NorwichCQRS.Infrastructure.ServicePresenters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Infrastructure.ServiceViews
{
    public interface IServiceView
    {
        event System.EventHandler<EventArgs> Started;
        event System.EventHandler<EventArgs> Stopped;
        event System.EventHandler<EventArgs> ShutDown;
        event System.EventHandler<EventArgs> Paused;

        void AddServicePresenter(IServicePresenter servicePresenter);
        void Start();

    }
}
