using Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Product
{
    public class Comment
    {
        public int CommentId { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public DateTime InsertTime { get; set; }
        public int Points { get; set; } // 0 - 5
        public string CommentBody { get; set; }
        public ICollection<Comment_FeedBack> Advantages { get; set; }
        public ICollection<Comment_FeedBack> Disadvantages { get; set; }
    }
    public class Comment_FeedBack
    {
        public string Point { get; set; }
    }
}
