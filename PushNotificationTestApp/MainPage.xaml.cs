using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Management.Deployment;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PushNotificationTesterConnect;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PushNotificationTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Windows.Management.Deployment.PackageManager packageManager = new Windows.Management.Deployment.PackageManager();

            //var packageUsers =
            //    packageManager.FindUsers(Windows.ApplicationModel.Package.Current.Id.FullName).FirstOrDefault();

            //var sid = packageUsers.UserSecurityId;

            var channelOperation = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            Bridge.Register(channelOperation.Uri);//,sid);

        }
    }
}
