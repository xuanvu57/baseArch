using Application.Repositories.Interfaces;
using Application.User.Dtos.Requests;
using Application.User.Validators.Interfaces;
using BaseArch.Application.Repositories.Interfaces;
using BaseArch.Domain.DependencyInjection;
using BaseArch.Domain.ErrorHandling;
using Domain.Constants;
using Domain.MultilingualProviders.Interfaces;
using FluentValidation;

namespace Application.User.Validators
{
    [DIService(DIServiceLifetime.Scoped)]
    public class CreateUserValidator : AbstractValidator<CreateUserRequest>, ICreateUserValidator
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISampleMultilingualProvider _multilingualProvider;

        public CreateUserValidator(IUnitOfWork unitOfWork, ISampleMultilingualProvider multilingualProvider)
        {
            RuleFor(u => u.FirstName).NotEmpty().WithMessage(u => multilingualProvider.GetString(ResourceConst.MSG0001, nameof(u.FirstName)).Result);
            RuleFor(u => u.LastName).NotEmpty().WithMessage(u => multilingualProvider.GetString(ResourceConst.MSG0001, nameof(u.LastName)).Result);

            _unitOfWork = unitOfWork;
            _multilingualProvider = multilingualProvider;
        }

        public async Task ValidateWithDatabaseAsync(CreateUserRequest request)
        {
            var firstUserInDatabase = await _unitOfWork.GetRepository<IUserRepository>().GetFirstOrDefault();
            if (firstUserInDatabase.FirstName == request.FirstName)
            {
                var message = await _multilingualProvider.GetString(ResourceConst.MSG0002, nameof(request.FirstName), request.FirstName);
                throw new BaseArchValidationException(nameof(request.FirstName), [message]);
            }
        }
    }
}
