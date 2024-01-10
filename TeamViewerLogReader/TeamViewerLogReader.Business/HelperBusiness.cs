using System.Net.Sockets;
using System.Net;
using TeamViewerLogReader.Domain;

namespace TeamViewerLogReader.Business
{
    public static class HelperBusiness
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return null;
        }

        public static UserTvLog CreateUserDefault(string computerName, string ipAddress)
        {
            return new UserTvLog()
            {
                Name = "Default",
                Email = "default@cf-partners.com",
                PasswordHash = PasswordHasher.HashPassword("ChangeMe!"),
            };
        }
    }
}