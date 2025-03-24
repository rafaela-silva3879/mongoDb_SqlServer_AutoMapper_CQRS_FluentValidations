using AutoMapper;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Application.Notifications;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Services;
using MediatR;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeveloperStore.Application.RequestHandlers
{
    public class UserRequestHandler :
        IRequestHandler<UserCreateCommand, UserQuery>,
        IRequestHandler<UserDeleteCommand, UserQuery>,
        IRequestHandler<UserGetUserByEmailCommand, UserQuery>,
        IRequestHandler<UserGetUserByTokenCommand, UserQuery>,
        IRequestHandler<UserPreUpdatePasswordCommand, UserQuery>,
        IRequestHandler<UserUpdateCommand, UserQuery>,
        IRequestHandler<UserUpdatePasswordCommand, UserQuery>,
        IRequestHandler<UserValidateCredentialsCommand, UserQuery>,
        IRequestHandler<UserGetByIdCommand, UserQuery>
    {
        private readonly IUserDomainService _userDomainService;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UserRequestHandler(
            IUserDomainService userDomainService,
            IMapper mapper,
            IMediator mediator)
        {
            _userDomainService = userDomainService;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserQuery> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = request.Name,
                UserProfile = request.UserProfile,
                Email = request.Email,
                Password = request.Password,
            };

            await _userDomainService.AddAsync(user);

            await _mediator.Publish(new UserNotification(user, ActionNotification.AddAsync));

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }

        public async Task<UserQuery> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new Exception("User not registered.");
            }

            await _userDomainService.DeleteAsync(user);

            await _mediator.Publish(new UserNotification(user, ActionNotification.DeleteAsync));

            var userQuery = _mapper.Map<UserQuery>(user);

            return userQuery;
        }

        public async Task<UserQuery> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Name = request.Name;
            user.Password = request.Password;
            user.Email = request.Email;

            await _userDomainService.UpdateAsync(user);

            var notification = new UserNotification(
                user,
                ActionNotification.UpdateAsync
            );

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }

        public async Task<UserQuery> Handle(UserPreUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            user.PasswordResetToken = request.PasswordResetToken;
            user.PasswordResetExpiresIn = request.PasswordResetExpiresIn;

            await _userDomainService.UpdateAsync(user);

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }

        public async Task<UserQuery> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            user.Password = request.NewPassword; // Update password
            user.PasswordResetToken = null; // Invalida o token
            user.PasswordResetExpiresIn = DateTime.MinValue;

            await _userDomainService.UpdateAsync(user);

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }

        public async Task<UserQuery> Handle(UserGetUserByEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var notification = new UserNotification(
                user,
                ActionNotification.GetWithFilterAsync
            );

            await _mediator.Publish(notification);

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }

        public async Task<UserQuery> Handle(UserGetUserByTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetUserByTokenAsync(request.Token);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }

        public async Task<UserQuery> Handle(UserValidateCredentialsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.ValidateCredentialsAsync(request.Email, request.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }

        public async Task<UserQuery> Handle(UserGetByIdCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var query = _mapper.Map<UserQuery>(user);

            return query;
        }
    }
}