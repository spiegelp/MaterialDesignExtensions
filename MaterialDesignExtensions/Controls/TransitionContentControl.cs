using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MaterialDesignExtensions.Controls
{
    /// <summary>
    /// A basic content control which uses an animation for switching its content.
    /// </summary>
    public class TransitionContentControl : Control
    {
        private const string ContentControl1Name = "ContentControl1";
        private const string ContentControl2Name = "ContentControl2";

        private object m_lockObject = new object();

        private bool AnimationIsRunning
        {
            get
            {
                lock (m_lockObject)
                {
                    return m_animationIsRunning;
                }
            }

            set
            {
                lock (m_lockObject)
                {
                    m_animationIsRunning = value;
                }
            }
        }

        /// <summary>
        /// The content of the control.
        /// </summary>
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            nameof(Content), typeof(object), typeof(TransitionContentControl), new PropertyMetadata(null, ContentPropertyChangedCallback));

        /// <summary>
        /// The content of the control.
        /// </summary>
        public object Content
        {
            get
            {
                return GetValue(ContentProperty);
            }

            set
            {
                SetValue(ContentProperty, value);
            }
        }

        /// <summary>
        /// The transition animation for switching the content.
        /// </summary>
        public static readonly DependencyProperty TransitionTypeProperty = DependencyProperty.Register(
            nameof(TransitionType), typeof(TransitionContentControlTransitionType), typeof(TransitionContentControl), new PropertyMetadata(TransitionContentControlTransitionType.FadeInAndGrow, null));

        /// <summary>
        /// The transition animation for switching the content.
        /// </summary>
        public TransitionContentControlTransitionType TransitionType
        {
            get
            {
                return (TransitionContentControlTransitionType)GetValue(TransitionTypeProperty);
            }

            set
            {
                SetValue(TransitionTypeProperty, value);
            }
        }

        private static void ContentPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is TransitionContentControl control)
            {
                control.SwitchContent(args.NewValue);
            }
        }

        private ContentControl[] m_contentControls;
        private IDictionary<TransitionContentControlTransitionType, Storyboard> m_storyboards;

        private int m_foregroundIndex;
        private bool m_animationIsRunning;

        static TransitionContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TransitionContentControl), new FrameworkPropertyMetadata(typeof(TransitionContentControl)));
        }

        /// <summary>
        /// Creates a new <see cref="TransitionContentControl" />.
        /// </summary>
        public TransitionContentControl()
            : base()
        {
            m_contentControls = null;
            m_storyboards = null;

            m_foregroundIndex = 0;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_contentControls = new ContentControl[]
            {
                GetTemplateChild(ContentControl1Name) as ContentControl,
                GetTemplateChild(ContentControl2Name) as ContentControl
            };

            m_storyboards = new Dictionary<TransitionContentControlTransitionType, Storyboard>();

            foreach (TransitionContentControlTransitionType transitionType in Enum.GetValues(typeof(TransitionContentControlTransitionType)))
            {
                m_storyboards[transitionType] = Template.Resources[GetStoryboardNameForTransitionType(transitionType)] as Storyboard;
            }
        }

        private async void SwitchContent(object content)
        {
            if (m_contentControls != null)
            {
                if (!AnimationIsRunning)
                {
                    try
                    {
                        AnimationIsRunning = true;

                        // set new content and push it into foreground
                        int currentIndex = m_foregroundIndex;
                        m_foregroundIndex++;

                        if (m_foregroundIndex >= m_contentControls.Length)
                        {
                            m_foregroundIndex = 0;
                        }

                        m_contentControls[m_foregroundIndex].Content = content;

                        // delay the animation until the rendering of the new content is finished
                        //     by running the code via the Dispatcher with a priority lower than the rendering
                        await Dispatcher.Invoke(async () =>
                        {
                            // move the new content into foreground
                            Panel.SetZIndex(m_contentControls[m_foregroundIndex], Panel.GetZIndex(m_contentControls[currentIndex]) + 1);
                            Panel.SetZIndex(m_contentControls[currentIndex], 0);

                            // start the animation
                            Storyboard storyboard = m_storyboards[TransitionType];
                            storyboard.Begin(m_contentControls[m_foregroundIndex]);

                            // wait until the animation completes and finally remove the hidden content to free memory
                            //     add a short delay to the animation duration
                            int animationDuration = (int)storyboard.Children.Max(x => x.Duration.TimeSpan.TotalMilliseconds);

                            await Task.Delay(animationDuration + 50);

                            m_contentControls[currentIndex].Content = null;
                        }, System.Windows.Threading.DispatcherPriority.Loaded);
                    }
                    finally
                    {
                        AnimationIsRunning = false;
                    }
                }
                else
                {
                    m_contentControls[m_foregroundIndex].Content = content;
                }
            }
        }

        private string GetStoryboardNameForTransitionType(TransitionContentControlTransitionType transitionType)
        {
            return $"{Enum.GetName(typeof(TransitionContentControlTransitionType), transitionType)}Storyboard";
        }
    }

    /// <summary>
    /// The available animations for <see cref="TransitionContentControl" />.
    /// </summary>
    public enum TransitionContentControlTransitionType : byte
    {
        FadeIn,
        Grow,
        FadeInAndGrow
    }
}
