using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MobileExample.Droid.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "baggyFilter" })]
    public class SensorService : IntentService
    {
        public int contador = 0;
        public SensorService(Context contexto) : base("SensorService")
        {
            Console.WriteLine("Empezó el servicio.");
        }

        public SensorService()
        {

        }

        protected override void OnHandleIntent(Intent intent)
        {
            Console.WriteLine("me estoy creando");
            comenzarContador();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);
            //comenzarContador();
            return StartCommandResult.Sticky;
        }
        public override void OnCreate()
        {
            base.OnCreate();
            Console.WriteLine("me estoy creando");
        }
        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);
            Console.WriteLine("Servicio destruido.");
            Intent broadcastIntent = new Intent("baggyFilter");
            SendBroadcast(broadcastIntent);
            pararContador();
        }

        private Timer timer;

        long oldTime = 0;

        public void comenzarContador()
        {
            timer = new Timer(1000);
            comenzarTareaTimer();
        }

        public void comenzarTareaTimer()
        {
            timer.Elapsed += new ElapsedEventHandler(accionTimer);
            timer.Enabled = true;
        }

        private void accionTimer(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("El timer va por el numero: {contador}", contador++);
        }

        public void pararContador()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}