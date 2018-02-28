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
                    new Step() { Header = new StepTitleHeader() { FirstLevelTitle = "Validation" }, Content = new StepperTutorialFourViewModel() }
                };
            }
        }

        public StepperViewModel()
            : base()
        {
            m_layout = StepperLayout.Horizontal;
            m_isLinear = false;
        }
    }

    public class StepperTutorialOneViewModel
    {
        public StepperTutorialOneViewModel() { }
    }

    public class StepperTutorialTwoViewModel
    {
        public StepperTutorialTwoViewModel() { }
    }

    public class StepperTutorialThreeViewModel
    {
        public StepperTutorialThreeViewModel() { }
    }

    public class StepperTutorialFourViewModel
    {
        public StepperTutorialFourViewModel() { }
    }
}
