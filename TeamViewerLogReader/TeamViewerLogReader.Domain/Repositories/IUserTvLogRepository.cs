using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerLogReader.Domain.Repositories
{
    public interface IUserTvLogRepository
    {
        UserTvLog Login(UserTvLog userTvLog);
        UserTvLog Update(UserTvLog user);
        UserTvLog Create(UserTvLog user);

        string GetUserHashedPassword(string username);
    }
}
