using Application.Interfaces;
using Common;
using Domain.Entities.Product;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Application.Services.Product.Commands.AddComment
{
    public class AddCommentService : IRequestHandler<RequestAddComment, ResultDto<int>>
    {
        private readonly IDataBaseContext _context;
        public AddCommentService(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<ResultDto<int>> Handle(RequestAddComment request, CancellationToken cancellationToken)
        {
            var validationRule = new RequestAddCommentValidation(_context);
            var validRequest = validationRule.Validate(request);
            if (!validRequest.IsValid)
            {
                return new ResultDto<int> { Message = validRequest.Errors[0].ErrorMessage };
            }
            Comment newComment = new Comment
            {
                Advantages = request.Adv.Select(e => new Comment_FeedBack { Point = e }).ToList(),
                Disadvantages = request.DisAdv.Select(e => new Comment_FeedBack { Point = e }).ToList(),
                Points = request.Point,
                Product = _context.Products.Find(request.ProductId),
                User = _context.Users.Find(request.UserId),
                CommentBody=request.Comment
            };
            _context.Comments.Add(newComment);

            await _context.SaveChangesAsync();
            return new ResultDto<int> { Data = newComment.CommentId, IsSuccess = true, Message = "نظر ثبت شد" };
        }
    }
    public class RequestAddComment : IRequest<ResultDto<int>>
    {
        public string Comment { get; set; }
        public int Point { get; set; }
        public List<string> Adv { get; set; }
        public List<string> DisAdv { get; set; }
        public int ProductId { get; set; }
        [BindNever]
        public int UserId { get; set; }
    }
    public class RequestAddCommentValidation : AbstractValidator<RequestAddComment>
    {
        private readonly IDataBaseContext context;
        public RequestAddCommentValidation(IDataBaseContext context)
        {
            this.context = context;

            RuleFor(e => e.Comment).NotEmpty().WithMessage("نظر را وارد کیند");
            RuleFor(e => e.Point).ExclusiveBetween(0, 6).WithMessage("امتیاز را به درستی وارد کنید");
            RuleFor(e => e.Adv).NotEmpty().WithMessage("نقاط قوت محصول را وارد کنید");
            RuleFor(e => e.DisAdv).NotEmpty().WithMessage("نقاط ضعف محصول را وارد کنید");
            RuleFor(e => e.ProductId).Must(ValidProduct).WithMessage("محصول پیدا نشد");
            RuleFor(e => e.UserId).Must(ValidUser).WithMessage("کاربر یافت نشد");
        }

        private bool ValidUser(int arg)
        {
            return context.Users.Any(e => e.UserId == arg);
        }

        private bool ValidProduct(int arg)
        {
            return context.Products.Any(e => e.ProductId == arg);
        }
    }
}
