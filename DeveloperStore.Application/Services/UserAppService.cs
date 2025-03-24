using AutoMapper;
using DeveloperStore.Application.Interfaces;
using DeveloperStore.Application.Models.Commands;
using DeveloperStore.Application.Models.Queries;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeveloperStore.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserCache? _userCache;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserAppService(IUserCache userCache,
                                  IMediator mediator,
                                  IMapper mapper)
        {
            _userCache = userCache;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<UserQuery> CreateAsync(UserCreateCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserQuery> UpdatePrePasswordAsync(UserPreUpdatePasswordCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserQuery> UpdatePasswordAsync(UserUpdatePasswordCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserQuery?> ValidateCredentialsAsync(UserValidateCredentialsCommand command)
        {
            try
            {
                var filter = Builders<User>.Filter.And(
        Builders<User>.Filter.Eq(c => c.Email, command.Email),
        Builders<User>.Filter.Eq(c => c.Password, command.Password)
    );

                var user = await _userCache.GetWithFilterAsync(filter);
                return _mapper.Map<UserQuery?>(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao validar credenciais.", ex);
            }
        }


        public async Task<UserQuery> GetUserByEmailAsync(UserGetUserByEmailCommand command)
        {
            try
            {
                var filter = Builders<User>.Filter.And(
                                Builders<User>.Filter.Eq(c => c.Email, command.Email));

                var user = await _userCache.GetWithFilterAsync(filter);
                return _mapper.Map<UserQuery?>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserQuery> GetUserByTokenAsync(UserGetUserByTokenCommand command)
        {
            try
            {
                var filter = Builders<User>.Filter.And(
                                Builders<User>.Filter.Eq(c => c.PasswordResetToken, command.Token));

                var user = await _userCache.GetWithFilterAsync(filter);
                return _mapper.Map<UserQuery?>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserQuery>> GetAllAsync()
        {
            try
            {
                var users = await _userCache.GetAllAsync();
                return _mapper.Map<List<UserQuery>>(users);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserQuery> GetByIdAsync(string id)
        {
            try
            {
                var user = await _userCache.GetByIdAsync(id);
                return _mapper.Map<UserQuery>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserQuery> UpdateAsync(UserUpdateCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserQuery> DeleteAsync(UserDeleteCommand command)
        {
            try
            {
                return await _mediator.Send(command);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
