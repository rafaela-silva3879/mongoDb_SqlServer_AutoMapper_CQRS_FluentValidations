using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DeveloperStore.Application.Notifications;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Domain.Entities.Enums;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Domain.Interfaces.Cache; 

namespace DeveloperStore.Application.Handlers
{
    public class UserRequestHandler :
         IRequestHandler<UserCreateCommand, UserQuery>,
         IRequestHandler<UserDeleteCommand, UserQuery>,
         IRequestHandler<UserPreUpdatePasswordCommand, UserQuery>,
         IRequestHandler<UserUpdateCommand, UserQuery>,
         IRequestHandler<UserUpdatePasswordCommand, UserQuery>
    {
        private readonly IUserDomainService? _userDomainService;
        private readonly IUserCache _userCache;
        private readonly IMapper _mapper;
        private readonly IMediator? _mediator;

        public UserRequestHandler(
            IUserDomainService? userDomainService,
            IUserCache userCache,
            IMapper mapper,
            IMediator? mediator)
        {
            _userDomainService = userDomainService;
            _userCache = userCache;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<UserQuery> Handle(UserCreateCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password,
                UserProfile = (int)UserProfile.User
            };

            await _userDomainService.AddAsync(user);
            await _userCache.AddAsync(user); // Adiciona ao cache

            var notification = new UserNotification(
                user,
                ActionNotification.AddAsync
            );

            await _mediator.Publish(notification);

            return _mapper.Map<UserQuery>(user);
        }

        public async Task<UserQuery> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user != null)
            {
                await _userDomainService.DeleteAsync(user);
                await _userCache.DeleteAsync(user);
            }
            else
            {
                throw new Exception("User not found.");
            }

            var notification = new UserNotification(
                    user,
                    ActionNotification.DeleteAsync
                );

            await _mediator.Publish(notification);

            return _mapper.Map<UserQuery>(user);
        }

        public async Task<UserQuery> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Email = request.Email;
            user.Name = request.Name;
            user.Password = request.Password;
            user.UserProfile = request.UserProfile;

            await _userDomainService.UpdateAsync(user);
            await _userCache.UpdateAsync(user);

            var notification = new UserNotification(
                user,
                ActionNotification.UpdateAsync
            );

            await _mediator.Publish(notification);

            return _mapper.Map<UserQuery>(user);
        }

        public async Task<UserQuery> Handle(UserPreUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.PasswordResetToken = request.PasswordResetToken;
            user.PasswordResetExpiresIn = request.PasswordResetExpiresIn;

            await _userDomainService.UpdateAsync(user);
            await _userCache.UpdateAsync(user);

            var notification = new UserNotification(
                user,
                ActionNotification.UpdateAsync
            );

            await _mediator.Publish(notification);

            return _mapper.Map<UserQuery>(user);
        }

        public async Task<UserQuery> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userDomainService.GetByIdAsync(request.Id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.Password = request.NewPassword;
            user.PasswordResetToken = null;
            user.PasswordResetExpiresIn = DateTime.MinValue;

            await _userDomainService.UpdateAsync(user);
            await _userCache.UpdateAsync(user);

            var notification = new UserNotification(
                user,
                ActionNotification.UpdateAsync
            );

            await _mediator.Publish(notification);

            return _mapper.Map<UserQuery>(user);
        }


    }
}
