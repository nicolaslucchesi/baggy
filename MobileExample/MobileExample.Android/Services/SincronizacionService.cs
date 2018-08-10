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
using MobileExample.Database;
using MobileExample.Tables;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Forms;
using MobileExample.Sincronizacion;

namespace MobileExample.Droid.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "baggyFilter" })]
    public class SincronizacionService : IntentService
    {
        public int contador = 0;
        public HttpClient client = new HttpClient();
        public string urlClima = "http://api.apixu.com/v1/forecast.json?key=d8c869676cba4733b9f230353180108&q=-34.669090,-58.564331&days=1";
        public BluetoothService bluetoothService = new BluetoothService();

        public SincronizacionService(Context contexto) : base("SincronizacionService")
        {
            Console.WriteLine("Empezó el servicio.");
        }

        public SincronizacionService()
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
            string textoNotificacion = string.Empty;

            // PRIMERO - Intentar sincronizar la información
            string data = bluetoothService.Sincronizar();

            if (data != "")
            {
                ModeloRespuesta respuestaSincronizacion = JsonConvert.DeserializeObject<ModeloRespuesta>(data);
                switch (respuestaSincronizacion.Codigo)
                {
                    case (int)EnumCodigos.Sincronizar:
                        ModeloSincronizacion modeloSincronizacion = JsonConvert.DeserializeObject<ModeloSincronizacion>(respuestaSincronizacion.Data);
                        InformacionSincronizada informacionSincronizada = new InformacionSincronizada(respuestaSincronizacion);
                        // Si es información de una sincronización, la guardo en la BD
                        DatabaseHelper.db.Insert(informacionSincronizada);
                        break;
                    case (int)EnumCodigos.NuevoElemento:
                        ModeloNuevoElemento modeloNuevoElemento = JsonConvert.DeserializeObject<ModeloNuevoElemento>(respuestaSincronizacion.Data);
                        break;
                    case (int)EnumCodigos.Alarma:
                        ModeloAlarma modeloAlarma = JsonConvert.DeserializeObject<ModeloAlarma>(respuestaSincronizacion.Data);
                        break;
                    default:
                        break;
                }
            }

            // SEGUNDO - chequeo si hay algún recordatorio en el proximo minuto configurado para comparar qué tengo y qué tengo que tener.
            List<Recordatorio> recordatorios = new List<Recordatorio>();

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(x => x.Lunes).ToList();
                    break;
                case DayOfWeek.Thursday:
                    recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(x => x.Martes).ToList();
                    break;
                case DayOfWeek.Wednesday:
                    recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(x => x.Miercoles).ToList();
                    break;
                case DayOfWeek.Tuesday:
                    recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(x => x.Jueves).ToList();
                    break;
                case DayOfWeek.Friday:
                    recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(x => x.Viernes).ToList();
                    break;
                case DayOfWeek.Saturday:
                    //recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(x => x.Sabado).ToList();
                    break;
                case DayOfWeek.Sunday:
                    //recordatorios = DatabaseHelper.db.Table<Recordatorio>().Where(x => x.Domingo).ToList();
                    break;
                default:
                    break;
            }

            if (recordatorios.Count > 0)
            {
                // Chequear si hay algún recordatorio en el próximo minuto para tirar la alarma de si se olvida o no algo
                // ademas, revisar si es el primero del día para consultar el clima.
            }

            //HttpResponseMessage respuestaClima = await client.GetAsync(urlClima);
            //string respuestaString = await respuestaClima.Content.ReadAsStringAsync();
            //ClimaResponse respuesta = JsonConvert.DeserializeObject<ClimaResponse>(respuestaString);

            //if (CodigosLluvia.Codigos.Contains(respuesta.forecast.forecastday[0].day.condition.code))
            //{
            //    textoNotificacion = "Habrá lluvias hoy.";
            //}
            //else
            //{
            //    textoNotificacion = "Hoy no llueve!";
            //}
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