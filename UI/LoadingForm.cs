using System.Windows.Forms;

namespace Polynano.UI
{
    public partial class LoadingForm : Form
    {
        /// <summary>
        /// Set the status bar progress value
        /// </summary>
        public int StatusBar
        {
            get => statusBar.Value;
            set => statusBar.Value = value;
        }

        /// <summary>
        /// Set the status text displayed
        /// </summary>
        public string StatusText
        {
            get => Text;
            set => Text = value;
        }

        public ProgressBarStyle Style
        {
            set
            {
                statusBar.Style = value;
                statusBar.MarqueeAnimationSpeed = 15;
            }
        }

        /// <summary>
        /// Set the statusBar text
        /// </summary>
        public string StatusBarText
        {
            get => statusBarText.Text;
            set
            {
                statusBarText.Text = value;
                statusBarText.Visible = true;
                statusBar.Visible = false;
            }
        }

        public LoadingForm()
        {
            InitializeComponent();
        }
    }
}