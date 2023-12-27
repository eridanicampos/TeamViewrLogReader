using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Domain;

namespace TeamViewerLogReader.Business.Interfaces
{
    public interface IUserTvLogBusiness
    {
        UserTvLog Login(UserTvLog userTvLog);
        UserTvLog Update(UserTvLog user);
        UserTvLog Create(UserTvLog user);
        string GetUserHashedPassword(string username);
    }
}
