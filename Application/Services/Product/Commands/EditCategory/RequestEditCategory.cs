using Common;
using MediatR;

namespace Application.Services.Product.Commands.EditCategory
{
    public record RequestEditCategory : IRequest<ResultDto>
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParentCategory { get; set; }
    }
}
