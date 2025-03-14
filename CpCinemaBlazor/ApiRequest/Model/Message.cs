using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CpCinemaBlazor.ApiRequest.Model
{
    public class MessageShort
    {
        public int Id_Message { get; set; }
        public string Message { get; set; }
        public DateTime dateTimeSent { get; set; }
        public string FilmName { get; set; } 
        public string UserName { get; set; } 
        public string RecipientName { get; set; } 
        public string ImageURL { get; set; }
    }
    public class Messages
    {
        [Key]
        public int Id_Message { get; set; }
        public string Message { get; set; }
        public DateTime dateTimeSent { get; set; }
        public int Film_Name { get; set; }
        public string FilmName { get; set; } 
        public int User_Name { get; set; }
        public string UserName { get; set; } 
        public int Recipient_Name { get; set; }
        public string RecipientName { get; set; } 
        public string ImageURL { get; set; }
    }
    public class MessageData
    {
        public bool Status { get; set; } 
        public MessageDataContainer Data { get; set; }
    }
    public class MessageDataContainer
    {
        public List<MessageShort> messages { get; set; }
    }
    public class AddMessage
    {
        public string Message { get; set; }
        public int Film_ { get; set; }
        public int User_Name { get; set; }
        public int Recipient_Name { get; set; }
        public string ImageURL { get; set; }
    }
    public class UpdateMessage
    {
        public int Id_Message { get; set; }
        public string Message { get; set; }
        public string ImageURL { get; set; }
    }
}
