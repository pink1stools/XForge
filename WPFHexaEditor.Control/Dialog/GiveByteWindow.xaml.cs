using System.Windows;

namespace WPFHexaEditor.Control.Dialog
{
    /// <summary>
    /// This Window is used to give a hex value for fill the selection with.
    /// </summary>
    internal partial class GiveByteWindow : Window
    {
        public GiveByteWindow() => InitializeComponent();

        private void OKButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;

        /// <summary>
        /// Title of the window
        /// </summary>
        public new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.Title == "") base.Title = "Enter hexadecimal value.";
        }
    }
}
