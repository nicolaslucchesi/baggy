using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileExample.Database;
using MobileExample.Tables;
using SQLite;
using Xamarin.Forms;

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
            comenzarContador();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);
            return StartCommandResult.Sticky;
        }
        public override void OnCreate()
        {
            base.OnCreate();
        }
        public override void OnTaskRemoved(Intent rootIntent)
        {
            base.OnTaskRemoved(rootIntent);
            Intent broadcastIntent = new Intent("baggyFilter");
            SendBroadcast(broadcastIntent);
            pararContador();
        }

        private Timer timer;
        /// <summary>
        /// Este método es el que se ejecuta cuando empieza el servicio.
        /// </summary>
        public void comenzarContador()
        {
            timer = new Timer(15000);
            timer.Elapsed += new ElapsedEventHandler(accionTimer);
            timer.Enabled = true;
        }
        /// <summary>
        /// Esta es la acción que se ejecuta cada vez que transcurre el lapso definido
        /// en el timer.
        /// Acá irían las actividades de sincronización y verificación de datos y demás.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accionTimer(object sender, ElapsedEventArgs e)
        {
            int cantidadMochilas = DatabaseHelper.db.Table<Mochila>().Count();
            int cantidadRecordatorios = DatabaseHelper.db.Table<Recordatorio>().Count();
            int cantidadElementos = DatabaseHelper.db.Table<Elemento>().Count();
            string textoNotificacion = "Hay " + cantidadMochilas + " mochilas, "
                                            + cantidadRecordatorios + " recordatorios y "
                                            + cantidadElementos + " elementos.";

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            Notification.Builder builder = new Notification.Builder(this)
                .SetContentTitle("Hola!")
                .SetContentText(textoNotificacion)
                // Esta linea es para que vibre y suene. Por ahora queda deshabilitada porque
                // es un dolor de pelotas que suene todo el tiempo.
                //.SetDefaults(NotificationDefaults.Vibrate | NotificationDefaults.Sound)
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                .SetSmallIcon(Resource.Drawable.BaggyLogo1);

            Notification notification = builder.Build();
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
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