using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Controllers;

namespace MaterialDesignExtensions.Model
{
    /// <summary>
    /// A view model for an <see cref="IStep"/>.
    /// It is considered for internal use only.
    /// </summary>
    public class StepperStepViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private StepperController m_controller;

        private IStep m_step;

        private int m_number;
        private bool m_needsSpacer;
        private bool m_isActive;
        private bool m_isFirstStep;
        private bool m_isLastStep;

        public StepperController Controller
        {
            get
            {
                return m_controller;
            }

            set
            {
                m_controller = value;

                OnPropertyChanged(nameof(Controller));
            }
        }

        public bool IsActive
        {
            get
            {
                return m_isActive;
            }

            set
            {
                m_isActive = value;

                OnPropertyChanged(nameof(IsActive));
            }
        }

        public bool IsFirstStep
        {
            get
            {
                return m_isFirstStep;
            }

            set
            {
                m_isFirstStep = value;

                OnPropertyChanged(nameof(IsFirstStep));
            }
        }

        public bool IsLastStep
        {
            get
            {
                return m_isLastStep;
            }

            set
            {
                m_isLastStep = value;

                OnPropertyChanged(nameof(IsLastStep));
            }
        }

        public bool NeedsSpacer
        {
            get
            {
                return m_needsSpacer;
            }

            set
            {
                m_needsSpacer = value;

                OnPropertyChanged(nameof(NeedsSpacer));
            }
        }

        public int Number
        {
            get
            {
                return m_number;
            }

            set
            {
                m_number = value;

                OnPropertyChanged(nameof(Number));
            }
        }

        public IStep Step
        {
            get
            {
                return m_step;
            }

            set
            {
                m_step = value;

                OnPropertyChanged(nameof(Step));
            }
        }

        public StepperStepViewModel()
        {
            m_controller = null;

            m_step = null;

            m_number = 0;
            m_needsSpacer = false;
            m_isActive = false;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
