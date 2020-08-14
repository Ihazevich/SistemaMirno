namespace SistemaMirno.UI.View.Services
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowOkCancelDialog(string text, string title);
        MessageDialogResult ShowOkDialog(string text, string title);
    }
}