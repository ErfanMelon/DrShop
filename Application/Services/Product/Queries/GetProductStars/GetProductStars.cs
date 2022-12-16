using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Product.Queries.GetProductStars
{
    public class GetProductStars:IRequestHandler<RequestGetProductStars,int>
    {
        private readonly IDataBaseContext _context;

        public GetProductStars(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(RequestGetProductStars request, CancellationToken cancellationToken)
        {
            var comments =await _context.Comments
                .Include(e => e.Product)
                .Where(e => e.Product.ProductId == request.productId).ToListAsync();

            if (!comments.Any())
                return 0;

            return (int)comments.Average(e => e.Points);
        }
    }
    public record RequestGetProductStars(int productId) :IRequest<int>;
}
