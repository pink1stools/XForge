using System.Windows;

namespace WPFHexaEditor.Control.Dialog
{
    /// <summary>
    /// This Window is used to give tow hex value for deal with.
    /// </summary>
    internal partial class ReplaceByteWindow : Window
    {
        public ReplaceByteWindow() => InitializeComponent();

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
            if (base.Title == "") base.Title = "Enter hexadecimal value to replace.";
        }
    }
}
