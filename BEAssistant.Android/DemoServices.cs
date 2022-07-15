using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BEAssistant.xamlThings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[assembly:Xamarin.Forms.Dependency(typeof(BEAssistant.Droid.DemoServices))]
namespace BEAssistant.Droid
{
    [Service(ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
    public class DemoServices : Service,IDemoServices
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent.Action == "START_SERVICES")
            {
                ComenzarRegistroDiario();
            }
            return StartCommandResult.NotSticky; 
        }

        private void ComenzarRegistroDiario()
        {
            NotificationChannel channel = new NotificationChannel("ServicioChannel", "Demo de servicio", NotificationImportance.Max);
            NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
            manager.CreateNotificationChannel(channel);
            Notification notification = new Notification.Builder(this, "ServicioChannel")
                .SetContentTitle("Servicios de B.E.A trabajando.")
                .SetSmallIcon(Resource.Drawable.abc_ic_star_black_16dp)
                .SetOngoing(true)
                .Build();
            StartForeground(100, notification);
        }

        public void Start()
        {
            Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(DemoServices));
            startService.SetAction("START_SERVICES");
            MainActivity.ActivityCurrent.StartService(startService);
        }
    }
}