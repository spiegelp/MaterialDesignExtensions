using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class BusyOverlayViewModel : ViewModel
    {
        private bool m_isBusy;
        private int m_progress;

        public bool IsBusy
        {
            get
            {
                return m_isBusy;
            }

            set
            {
                m_isBusy = value;

                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public int Progress
        {
            get
            {
                return m_progress;
            }

            set
            {
                m_progress = value;

                OnPropertyChanged(nameof(Progress));
            }
        }

        public BusyOverlayViewModel()
            : base()
        {
            m_isBusy = false;
            m_progress = 0;
        }

        public async Task BeBusyAsync()
        {
            try
            {
                Progress = 0;
                IsBusy = true;

                await Task.Run(async () =>
                {
                    int progress = 0;

                    while (progress < 100)
                    {
                        await Task.Delay(250);

                        progress += 10;
                        Progress = progress;
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
