namespace DashboardApp.Services
{
    public enum ToastType { Success, Error, Warning, Info }

    public class ToastMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ToastType Type { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    public class ToastService
    {
        public event Action<ToastMessage>? OnShow;

        public void ShowSuccess(string message, string title = "Success") => Show(ToastType.Success, message, title);
        public void ShowError(string message, string title = "Error") => Show(ToastType.Error, message, title);
        public void ShowWarning(string message, string title = "Warning") => Show(ToastType.Warning, message, title);
        public void ShowInfo(string message, string title = "Info") => Show(ToastType.Info, message, title);

        private void Show(ToastType type, string message, string title)
        {
            OnShow?.Invoke(new ToastMessage { Type = type, Message = message, Title = title });
        }
    }
}
