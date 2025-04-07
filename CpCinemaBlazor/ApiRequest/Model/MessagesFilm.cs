namespace CpCinemaBlazor.ApiRequest.Model
{
    public class MessagesFilm
    {
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
