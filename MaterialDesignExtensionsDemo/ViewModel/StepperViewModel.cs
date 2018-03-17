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

        public override string DocumentationUrl
        {
            get
            {
                return "https://github.com/spiegelp/MaterialDesignExtensions/wiki/Stepper";
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
        public StepperTutorialThreeViewModel() { }
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
