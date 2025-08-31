
using System.Net;
using System.Net.Sockets;

namespace AssetManagement.Server
{
    public static class IpExtensions
    {
        public static bool IsInCidr(this IPAddress address, string cidr)
        {
            var parts = cidr.Split('/');
            var baseAddress = IPAddress.Parse(parts[0]);
            var prefixLength = int.Parse(parts[1]);

            var addressBytes = address.GetAddressBytes();
            var baseBytes = baseAddress.GetAddressBytes();

            if (addressBytes.Length != baseBytes.Length)
            {
                if (address.AddressFamily == AddressFamily.InterNetworkV6 &&
                    address.MapToIPv4().GetAddressBytes().Length == baseBytes.Length)
                {
                    addressBytes = address.MapToIPv4().GetAddressBytes();
                }
                else
                {
                    return false;
                }
            }

            int fullBytes = prefixLength / 8;
            int remainingBits = prefixLength % 8;

            for (int i = 0; i < fullBytes; i++)
                if (addressBytes[i] != baseBytes[i]) return false;

            if (remainingBits == 0) return true;

            int mask = (byte)~(0xFF >> remainingBits);
            return (addressBytes[fullBytes] & mask) == (baseBytes[fullBytes] & mask);
        }
    }
}
