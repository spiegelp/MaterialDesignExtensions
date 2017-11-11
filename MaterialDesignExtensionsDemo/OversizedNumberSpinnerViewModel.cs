using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignExtensionsDemo
{
    public class OversizedNumberSpinnerViewModel : ViewModel
    {
        private int m_value;
        private int m_min;
        private int m_max;

        public int Value
        {
            get
            {
                return m_value;
            }

            set
            {
                m_value = value;

                OnPropertyChanged(nameof(value));
            }
        }

        public int Min
        {
            get
            {
                return m_min;
            }

            set
            {
                m_min = value;

                OnPropertyChanged(nameof(Min));
            }
        }

        public int Max
        {
            get
            {
                return m_max;
            }

            set
            {
                m_max = value;

                OnPropertyChanged(nameof(Max));
            }
        }

        public OversizedNumberSpinnerViewModel()
            : base()
        {
            m_value = 2;
            m_min = 0;
            m_max = 4;
        }
    }
}
