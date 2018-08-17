using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MobileExample.Droid.Services
{
    [BroadcastReceiver]
    [IntentFilter(new[] { "baggyFilter" }, Categories = new[] { "android.intent.category.DEFAULT" })]
    public class SensorBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Console.WriteLine("Recibido el mensaje, se reinicia el servicio.");
            context.StartService(new Intent(context, typeof(SincronizacionService)));
        }
    }
}