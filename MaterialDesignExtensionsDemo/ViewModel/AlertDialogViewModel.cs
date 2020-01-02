namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class AlertDialogViewModel : ViewModel
    {
        private string m_message;

        public string Message
        {
            get
            {
                return m_message;
            }

            set
            {
                m_message = value;

                OnPropertyChanged(nameof(Message));
            }
        }

        public AlertDialogViewModel(string message)
            : base()
        {
            m_message = message;
        }
    }
}
