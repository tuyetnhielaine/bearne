using System.Diagnostics;  // Thêm namespace này để dùng Debug.WriteLine
using System.Net;

namespace VlAutoUpdateClient;

public class VlHttpClientHandler : HttpClientHandler
{
    public VlHttpClientHandler()
    {
        //Set here whatever you need to get configured
        AutomaticDecompression = DecompressionMethods.All;
        MaxConnectionsPerServer = 1000;
        /* by Tuyết Nhi */
        AllowAutoRedirect = false;  //false tránh client và server xử lý không đồng nhất về đường dẫn.
        Debug.WriteLine("VlHttpClientHandler created nè:");
        Debug.WriteLine($" - AutomaticDecompression = {AutomaticDecompression}");
        Debug.WriteLine($" - MaxConnectionsPerServer = {MaxConnectionsPerServer}");
        Debug.WriteLine($" - AllowAutoRedirect = {AllowAutoRedirect}");
    }
}
