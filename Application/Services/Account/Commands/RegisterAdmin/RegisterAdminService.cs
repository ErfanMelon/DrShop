using Application.Interfaces;
using Application.Services.Account.Commands.RegisterUser;
using Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Account.Commands.RegisterAdmin
{
    public class RegisterAdminService : IRequestHandler<RequestRegisterAdmin>
    {
        private readonly IMediator _mediator;
        private readonly IDataBaseContext context;

        public RegisterAdminService(IMediator mediator, IDataBaseContext context)
        {
            _mediator = mediator;
            this.context = context;
        }

        public async Task<Unit> Handle(RequestRegisterAdmin request, CancellationToken cancellationToken)
        {
            bool HasAdmin = await context.Users.AnyAsync(e => e.RoleId == (int)BaseRole.Admin);
            if (HasAdmin == false)
            {
                await _mediator.Send(new RequestRegisterUser
                {
                    Email = request.email,
                    Password = request.password,
                    Username = request.email,
                    Role = BaseRole.Admin
                });
            }
            return Unit.Value;
        }
    }
    public record RequestRegisterAdmin(string email, string password) : IRequest;
}
