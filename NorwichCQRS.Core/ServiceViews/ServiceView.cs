using NorwichCQRS.Infrastructure.ServicePresenters;
using NorwichCQRS.Infrastructure.ServiceViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.ServiceViews
{
    public class ServiceView : IServiceView
    {
        private List<IServicePresenter> _servicePresenters;
       
        public ServiceView()
        {
            _servicePresenters = new List<IServicePresenter>();
        }

        public void Start()
        {
            if (this.Started != null)
            {
                this.Started(this, new EventArgs());
            }
        }
        public event EventHandler<EventArgs> Started;

        public event EventHandler<EventArgs> Stopped;

        public event EventHandler<EventArgs> ShutDown;

        public event EventHandler<EventArgs> Paused;


        public void AddServicePresenter(IServicePresenter servicePresenter)
        {
            _servicePresenters.Add(servicePresenter);
            servicePresenter.StatusUpdated += servicePresenter_StatusUpdated;
        }

        void servicePresenter_StatusUpdated(object sender, StatusUpdatedArgs e)
        {
            
        }
    }
}
