using NorwichCQRS.Infrastructure;
using NorwichCQRS.Infrastructure.ServicePresenters;
using NorwichCQRS.Infrastructure.ServiceViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NorwichCQRS.Core.ServicePresenters
{
    public class ServicePresenter : IServicePresenter
    {
        private IServiceView _serviceView;
        private IListener _listener;

        public event System.EventHandler<StatusUpdatedArgs> StatusUpdated;

        public ServicePresenter(IServiceView serviceView, IListener listener)
        {
            _serviceView = serviceView;
            _listener = listener;

            this.Initialize();
        }

        private void Initialize()
        {
            _serviceView.Started += _serviceView_Started;
            _serviceView.Paused += _serviceView_Paused;
            _serviceView.Stopped += _serviceView_Stopped;
            _serviceView.ShutDown += _serviceView_ShutDown;
        }        

        void _serviceView_ShutDown(object sender, EventArgs e)
        {
            _listener.Stop();

            this.RaiseStatusUpdate("ServicePresenter ShutDown...");
        }

        void _serviceView_Stopped(object sender, EventArgs e)
        {
            _listener.Stop();

            this.RaiseStatusUpdate("ServicePresenter Stopped...");
        }

        void _serviceView_Paused(object sender, EventArgs e)
        {
            this.RaiseStatusUpdate("ServicePresenter Paused...");
        }

        void _serviceView_Started(object sender, EventArgs e)
        {
            this.RaiseStatusUpdate("ServicePresenter Started...");

            ThreadStart threadStart = new ThreadStart(_listener.Start);
            Thread thread = new Thread(threadStart);
            thread.Start();
            //_commandListener.Start();            
        }

        private void RaiseStatusUpdate(string status)
        {
            if (this.StatusUpdated != null)
            {
                this.StatusUpdated(this, new StatusUpdatedArgs(status));
            }
        }
    }
}
