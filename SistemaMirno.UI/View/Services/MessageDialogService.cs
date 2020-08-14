using System.Windows;

namespace SistemaMirno.UI.View.Services
{
    public enum MessageDialogResult
    {
        OK,
        Cancel
    }

    public class MessageDialogService : IMessageDialogService
    {
        /// <summary>
        /// Shows a Message Box with Ok and Cancel buttons.
        /// </summary>
        /// <param name="text">The message string.</param>
        /// <param name="title">The message box title.</param>
        /// <returns>A <see cref="MessageDialogResult"/> that represents the button the user pressed.</returns>
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK
                ? MessageDialogResult.OK
                : MessageDialogResult.Cancel;
        }

        public MessageDialogResult ShowOkDialog(string text, string title)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK);
            return MessageDialogResult.OK;
        }
    }
}