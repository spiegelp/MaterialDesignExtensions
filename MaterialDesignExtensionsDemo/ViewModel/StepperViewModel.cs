using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MaterialDesignExtensions.Controls;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsDemo.ViewModel
{
    public class StepperViewModel : ViewModel
    {
        private StepperLayout m_layout;
        private bool m_isLinear;
        private bool m_blockNavigationOnValidationErrors;
        private bool m_contentAnimationsEnabled;

        public bool BlockNavigationOnValidationErrors
        {
            get
            {
                return m_blockNavigationOnValidationErrors;
            }

            set
            {
                m_blockNavigationOnValidationErrors = value;

                OnPropertyChanged(nameof(BlockNavigationOnValidationErrors));
            }
        }

        public bool ContentAnimationsEnabled
        {
            get
            {
                return m_contentAnimationsEnabled;
            }

            set
            {
                m_contentAnimationsEnabled = value;

                OnPropertyChanged(nameof(ContentAnimationsEnabled));
            }
        }

        public override string DocumentationUrl
        {
            get
            {
                return "https://spiegelp.github.io/MaterialDesignExtensions/#documentation/stepper";
            }
        }

        public bool IsLinear
        {
            get
            {
                return m_isLinear;
            }

            set
            {
                m_isLinear = value;

                OnPropertyChanged(nameof(IsLinear));
            }
        }

        public StepperLayout Layout
        {
            get
            {
                return m_layout;
            }

            set
            {
                m_layout = value;

                OnPropertyChanged(nameof(Layout));
            }
        }

        public IEnumerable<StepperLayout> Layouts
        {
            get
            {
                return new List<StepperLayout>()
                {
                    StepperLayout.Horizontal,
                    StepperLayout.Vertical
                };
            }
        }

        public IEnumerable<IStep> Steps
        {
            get
            {
                return new List<IStep>()
                {
                    new Step() { Header = new StepTitleHeader() { FirstLevelTitle = "What is a Stepper?" }, Content = new StepperTutorialOneViewModel() },
                    new Step() { Header = new StepTitleHeader() { FirstLevelTitle = "Layout and navigation" }, Content = new StepperTutorialTwoViewModel() },
                    new Step() { Header = new StepTitleHeader() { FirstLevelTitle = "Steps", SecondLevelTitle = "Header and content" }, Content = new StepperTutorialThreeViewModel() },
                    new ValidationStep()
                };
            }
        }

        public StepperViewModel()
            : base()
        {
            m_layout = StepperLayout.Horizontal;
            m_isLinear = false;
            m_blockNavigationOnValidationErrors = false;
            m_contentAnimationsEnabled = true;
        }
    }

    public class ValidationStep : Step
    {
        public ValidationStep()
            : base()
        {
            Header = new StepTitleHeader() { FirstLevelTitle = "Validation" };
            Content = new StepperTutorialFourViewModel();
        }

        public override void Validate()
        {
            // example: the user must agree to the license in order to proceed to the next step
            HasValidationErrors = !((StepperTutorialFourViewModel)Content).AgreedToLicense;
        }
    }

    public class StepperTutorialOneViewModel : ViewModel
    {
        public StepperTutorialOneViewModel() { }
    }

    public class StepperTutorialTwoViewModel : ViewModel
    {
        public StepperTutorialTwoViewModel() { }
    }

    public class StepperTutorialThreeViewModel : ViewModel
    {
        private bool m_showFileDialogs;

        public bool ShowFileDialogs
        {
            get
            {
                return m_showFileDialogs;
            }

            set
            {
                m_showFileDialogs = value;

                OnPropertyChanged(nameof(ShowFileDialogs));
            }
        }

        public StepperTutorialThreeViewModel()
        {
            m_showFileDialogs = false;
        }
    }

    public class StepperTutorialFourViewModel : ViewModel
    {
        private bool m_agreedToLicense;

        public bool AgreedToLicense
        {
            get
            {
                return m_agreedToLicense;
            }

            set
            {
                m_agreedToLicense = value;

                OnPropertyChanged(nameof(AgreedToLicense));
            }
        }

        public StepperTutorialFourViewModel()
        {
            m_agreedToLicense = false;
        }
    }
}
