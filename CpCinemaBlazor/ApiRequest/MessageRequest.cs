namespace CpCinemaBlazor.ApiRequest
{
    public class MessageRequest
    {
        public int Message_Id { get; set; } 
        public string Message { get; set; }
        public string? ImageURL { get; set; }
        public int? Film_Id { get; set; }
        public int? Recipient_Id { get; set; }
        public int? User_Id { get; set; }
        public DateTime dateTimeSent { get; set; } 
        public string SenderName { get; set; } 
        public int Sender_Id { get; set; } 
        public string RecipientName { get; set; } 
    }
}
