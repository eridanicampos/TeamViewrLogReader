using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Service.DTOs;

namespace TeamViewerLogReader.Service.Interfaces
{
    public interface IUserTvLogService
    {
        UserTvLog Create(UserTvLog loginDto);
        UserTvLog Login(LoginDTO loginDto);
        UserTvLog Update(UserTvLog user);
        string GetUserHashedPassword(string username);
    }
}
