using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace PushNotificationTesterConnect
{
    public static class Bridge
    {
        public static async void Register(string channelUri)//, string packageSid)
        {
            try
            {
                

                var packageSid = Uri.EscapeUriString(Windows.ApplicationModel.Package.Current.Id.Name).Replace(".","_");
                var http = new HttpClient();
                var baseUri =
                    $"http://pushnotificationbridge.azurewebsites.net/api/bridge/id={packageSid}";
                var content = new StringContent(channelUri, Encoding.UTF8);
                await http.PostAsync(baseUri, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
