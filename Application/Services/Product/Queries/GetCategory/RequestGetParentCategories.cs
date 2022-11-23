using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Services.Product.Queries.GetCategory
{
    public record RequestGetParentCategories():IRequest<SelectList>;
}
