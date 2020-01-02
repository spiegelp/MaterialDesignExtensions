﻿using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class LanguageViewModel : ViewModel
    {
        public ICommand SelectLanguageCommand { get; private set; }

        public LanguageViewModel()
            : base()
        {
            SelectLanguageCommand = new DelegateCommand(ExecuteSelectLanguage, null);
        }

        private void ExecuteSelectLanguage(object parameter)
        {
            if (parameter is string languageName)
            {
                CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(languageName);

                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }
        }
    }
}
