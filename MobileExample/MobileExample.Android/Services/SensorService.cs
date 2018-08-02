using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Timers;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobileExample.Droid.Clima;
using MobileExample.Tables;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Forms;

namespace MobileExample.Droid.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "baggyFilter" })]
    public class SensorService : IntentService
    {
        public int contador = 0;
        public HttpClient client = new HttpClient();
        public string urlClima = "http://api.apixu.com/v1/forecast.json?key=d8c869676cba4733b9f230353180108&q=-34.669090,-58.564331&days=1";
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
        private async void accionTimer(object sender, ElapsedEventArgs e)
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatabaseSQLite.db3");
            var db = new SQLiteConnection(path);
            int cantidadMochilas = db.Table<Mochila>().Count();
            int cantidadRecordatorios = db.Table<Recordatorio>().Count();
            int cantidadElementos = db.Table<Elemento>().Count();
            string textoNotificacion = String.Empty;

            HttpResponseMessage respuestaClima = await client.GetAsync(urlClima);
            string respuestaString = await respuestaClima.Content.ReadAsStringAsync();
            ClimaResponse respuesta = JsonConvert.DeserializeObject<ClimaResponse>(respuestaString);
            if (respuesta.forecast.forecastday[0].day.condition.code > 1050)
            {
                textoNotificacion = "Habrá lluvias hoy. Hay " + cantidadMochilas + " mochilas, "
                                                + cantidadRecordatorios + " recordatorios y "
                                                + cantidadElementos + " elementos.";
            }
            else {
                textoNotificacion = "Hoy no llueve! Hay " + cantidadMochilas + " mochilas, "
                                                + cantidadRecordatorios + " recordatorios y "
                                                + cantidadElementos + " elementos.";
            }
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