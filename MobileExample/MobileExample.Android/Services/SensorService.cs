﻿using System;
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
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");
            var db = new SQLiteConnection(path);
            int cantidadMochilas = db.Table<Mochila>().Count();
            int cantidadRecordatorios = db.Table<Recordatorio>().Count();
            int cantidadElementos = db.Table<Elemento>().Count();
            string textoNotificacion = "Hay " + cantidadMochilas + " mochilas, "
                                            + cantidadRecordatorios + " recordatorios y "
                                            + cantidadElementos + " elementos.";

            NotificationChannel canalNotificacion = new NotificationChannel("canalNotificacion", "Notificacion", NotificationImportance.Default);
            canalNotificacion.EnableVibration(true);

            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(this, "canalNotificacion")
                .SetContentTitle("Hola!")
                .SetContentText(textoNotificacion)
                .SetSmallIcon(Resource.Drawable.Obj1);

            // Build the notification:
            Notification notification = builder.Build();

                // Get the notification manager:
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
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