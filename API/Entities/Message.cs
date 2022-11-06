using System;
using System.ComponentModel.DataAnnotations.Schema;
using API.Extensions;

namespace API.Entities
{
    [Table(name: "Messages")]
    public class Message
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public string SenderUserName { get; set; }

        public AppUser Sender { get; set; }

        public int RecipientId { get; set; }

        public string RecipientUserName { get; set; }

        public AppUser Recipient { get; set; }

        public string Content { get; set; }

        public DateTime? DateRead { get; set; }

        public DateTime MessageSent { get; set; } = DateTime.UtcNow;

        public bool SenderDeleted { get; set; }

        public bool RecipientDeleted { get; set; }


    }
}