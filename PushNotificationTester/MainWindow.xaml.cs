using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using NotificationsExtensions.TileContent;
using NotificationsExtensions.ToastContent;

namespace PushNotificationTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            TB_ChannelURL.Text= "https://hk2.notify.windows.com/?token=AwYAAADyNUIHJN0kkeTDmWoi1Zuvu9ioddAz6qClFyJQUvt0kN1tRJ4o3SeqpIxLVXMycm2HfhBDFgEUJy8ZTz4Hs%2fH8C2wIjA5bhSYs%2b0vpNoPaQUSN5ANW70RJJvcZWI3juqc%3d";

            TB_ClientSecret.Text= "U2b/EnStbqU0ukVVze3xxjYVTz2b053U";
            TB_PackageSID.Text= "ms-app://s-1-15-2-470714665-1652791542-30469515-4199281006-3823060278-3325982249-1326930535";

#endif


        }

        private async void SendClick(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToElementState(Content as Grid, "Sending", true);
            //await Task.Yield();
            await Task.Delay(500);
            var uri = TB_ChannelURL.Text;

            var secret = TB_ClientSecret.Text;
            var sid = TB_PackageSID.Text;

            var result = await Task.Run(() =>
            {
                try
                {
                    var notificationType = "wns/toast";
                    var contentType = "text/xml";

                    var accessToken = GetAccessToken(secret, sid);

                    var request = HttpWebRequest.Create(uri) as HttpWebRequest;
                    request.Method = "POST";

                    request.Headers.Add("X-WNS-Type", notificationType);
                    request.ContentType = contentType;
                    request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.AccessToken));



                    var tileContent = ToastContentFactory.CreateToastText01();

                    tileContent.TextBodyWrap.Text = "123 Test!";

                    byte[] contentInBytes = Encoding.UTF8.GetBytes(tileContent.ToString());

                    using (Stream requestStream = request.GetRequestStream())
                        requestStream.Write(contentInBytes, 0, contentInBytes.Length);

                    using (HttpWebResponse webResponse = (HttpWebResponse) request.GetResponse())
                    {
                        var txt = webResponse.StatusCode.ToString();
                        return txt;
                    }

                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            });

            MessageBox.Show($"Response '{result}", "Sending...");


            VisualStateManager.GoToElementState(Content as Grid, "NotSending", true);
        }


        [DataContract]
        public class OAuthToken
        {
            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }
            [DataMember(Name = "token_type")]
            public string TokenType { get; set; }
        }

        private OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        protected OAuthToken GetAccessToken(string secret, string sid)
        {
            var urlEncodedSecret = HttpUtility.UrlEncode(secret);
            var urlEncodedSid = HttpUtility.UrlEncode(sid);

            var body =
              String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com", urlEncodedSid, urlEncodedSecret);

            string response;
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                response = client.UploadString("https://login.live.com/accesstoken.srf", body);
            }
            return GetOAuthTokenFromJson(response);
        }

        private void OpenDevCenterClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://account.live.com/Developers/Applications");
        }
    }
}
