namespace Fingo.Auth.ManagementApp.Alerts
{
    public class Alert
    {
        public Alert(AlertType alertType , string message)
        {
            AlertType = alertType;
            Message = message;
        }

        //public const string TempDataKey = "TempDataAlerts";
        public AlertType AlertType { get; set; }
        public string Message { get; set; }
    }

    public enum AlertType
    {
        Success ,
        Information ,
        Warning ,
        Danger
    }
}