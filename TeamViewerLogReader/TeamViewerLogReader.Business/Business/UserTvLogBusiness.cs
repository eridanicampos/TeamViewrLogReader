using System.Text.RegularExpressions;
using TeamViewerLogReader.Business.Interfaces;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Domain.Repositories;

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
            return _repository.Create(user);
        }

        public UserTvLog Login(UserTvLog userTvLog)
        {
            return _repository.Login(userTvLog);
        }

        public UserTvLog Update(UserTvLog user)
        {
            return _repository.Update(user);
        }
        public string GetUserHashedPassword(string username)
        { 
            return _repository.GetUserHashedPassword(username);
        }
    }
}
