using System.Net.Sockets;
using System.Net;
using TeamViewerLogReader.Domain;
using System.Net.NetworkInformation;

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

        public static string GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Checks if the network interface is active
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    // Displays MAC address only
                    return nic.GetPhysicalAddress().ToString();
                    // Stops after finding the first active interface
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