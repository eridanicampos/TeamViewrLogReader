using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Service.Interfaces;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.Service.DTOs;
using TeamViewerLogReader.Business;

namespace TeamViewerLogReader.Service
{
    public class UserTvLogService : IUserTvLogService
    {
        private readonly IUserTvLogBusiness _business;

        public UserTvLogService(IUserTvLogBusiness business)
        {
            _business = business;
        }

        public UserTvLog Create(UserTvLog user)
        {
            string storedHash = _business.GetUserHashedPassword(user.Username);
            if (storedHash != null)
            {
                throw new InvalidOperationException("The username already exists! Please choose another username.");
            }
            return _business.Create(user);
        }

        public UserTvLog Login(LoginDTO loginDto)
        {
            return _business.Login(new UserTvLog() { Username = loginDto.Login, PasswordHash = loginDto.Password });
        }

        public UserTvLog Update(UserTvLog user)
        {
            return _business.Update(user);
        }

        public string GetUserHashedPassword(string username)
        {
            return _business.GetUserHashedPassword(username);
        }

    }
}
