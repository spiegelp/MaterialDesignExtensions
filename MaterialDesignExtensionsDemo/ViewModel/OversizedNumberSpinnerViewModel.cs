namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class OversizedNumberSpinnerViewModel : ViewModel
    {
        private int m_value;
        private int m_min;
        private int m_max;

        public override string DocumentationUrl
        {
            get
            {
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/oversizednumberspinner";
            }
        }

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
