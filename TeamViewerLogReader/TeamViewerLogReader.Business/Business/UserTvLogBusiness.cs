﻿using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Repositories;
using System.Net.NetworkInformation;

namespace TeamViewerLogReader.Business
{
    public class UserTvLogBusiness : IUserTvLogBusiness
    {
        private readonly IUserTvLogRepository _repository;

        public UserTvLogBusiness(IUserTvLogRepository repository)
        {
            _repository = repository;
        }

        public UserTvLog Create(UserTvLog user)
        {
            var userDefault = CheckLastLogin();
            if (userDefault != null )
            {
                userDefault.Name = user.Name;
                userDefault.Surname = user.Surname;
                userDefault.Username = user.Username;
                userDefault.Position = user.Position;
                userDefault.Company = user.Company;
                userDefault.PhoneNumber = user.PhoneNumber;
                userDefault.Email = user.Email;
                userDefault.DateCreated = DateTime.Now;
                userDefault.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
                return _repository.Update(userDefault);
            }
            else
            {
                user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            }
            return _repository.Create(user);
        }

        public UserTvLog Login(UserTvLog userTvLog)
        {
            string storedHash = GetUserHashedPassword(userTvLog.Username);
            if (storedHash == null)
            {
                throw new InvalidOperationException("User not found or password not set");
            }

            var boolVerify = PasswordHasher.VerifyPassword(storedHash, userTvLog.PasswordHash);

            if (!boolVerify)
            {
                throw new InvalidOperationException("User not found or password not set");
            }
            userTvLog.PasswordHash = storedHash;
            var objUser = _repository.Login(userTvLog);

            _repository.RegisterLogin(new UserLoginHistory()
            {
                ComputerName = System.Environment.MachineName,
                IpAddress = HelperBusiness.GetLocalIPAddress(),
                LoginTimestamp = DateTime.Now,
                MacAddress = HelperBusiness.GetMacAddress(),
                UserTvLogId = objUser.Id
            });

            return objUser;
        }



        public UserTvLog Update(UserTvLog user)
        {
            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            return _repository.Update(user);
        }
        public string GetUserHashedPassword(string username)
        { 
            return _repository.GetUserHashedPassword(username);
        }

        public UserTvLog CheckLastLogin()
        {
            string computerName = System.Environment.MachineName;
            string ipAddress = HelperBusiness.GetLocalIPAddress();
            string macAddress = HelperBusiness.GetMacAddress();
            return _repository.CheckLastLogin(computerName, ipAddress, macAddress);
        }

        public UserTvLog? CreateDefaultUser()
        {
            var userDefault = new UserTvLog()
            {
                Name = "Default",
                Surname = "Surname Default",
                Email = "default@cf-partners.com",
                Username = "Default",
                PasswordHash = PasswordHasher.HashPassword("ChangeMe!")
            };

            _repository.Create(userDefault);

            string computerName = System.Environment.MachineName;
            string ipAddress = HelperBusiness.GetLocalIPAddress();
            string macAddress = HelperBusiness.GetMacAddress();

            var userLoginHistoryDefault = new UserLoginHistory()
            {
                UserTvLogId = userDefault.Id,
                ComputerName = computerName,
                IpAddress = ipAddress,
                LoginTimestamp = DateTime.Now,
                MacAddress = macAddress,
                UserTvLog = userDefault
            };

            _repository.RegisterLogin(userLoginHistoryDefault);
            return userDefault;
        }
    }
}
