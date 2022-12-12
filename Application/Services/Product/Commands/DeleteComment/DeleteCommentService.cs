using Application.Interfaces;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Commands.DeleteComment
{
    public class DeleteCommentService : IRequestHandler<RequestDeleteComment, ResultDto>
    {
        private readonly IDataBaseContext context;

        public DeleteCommentService(IDataBaseContext context)
        {
            this.context = context;
        }

        public async Task<ResultDto> Handle(RequestDeleteComment request, CancellationToken cancellationToken)
        {
            var comment = context.Comments
                .Include(e => e.Disadvantages)
                .Include(e => e.Advantages)
                .FirstOrDefault(e => e.CommentId == request.CommentId);
            if (comment == null)
                new ThrowThisException(new ArgumentNullException($"the comment with id {request.CommentId} not found"), "نظر پیدا نشد");
            context.comment_FeedBacks.RemoveRange(comment.Advantages.Concat(comment.Disadvantages));
            context.Comments.Remove(comment);
            await context.SaveChangesAsync();
            return new ResultDto { IsSuccess = true, Message = "نظر حذف شد" };
        }
    }
    public record RequestDeleteComment(int CommentId) : IRequest<ResultDto>;
}
