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
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms;
using MobileExample.Sincronizacion;
using MobileExample.ViewModels;
using System.Threading.Tasks;
using InterfazBateria;
using Android.Support.V4.App;

namespace MobileExample.Droid.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] { "baggyFilter" })]
    public class SincronizacionService : IntentService
    {
        public int contador = 0;
        public HttpClient client = new HttpClient();
        public string urlClima = "https://api.apixu.com/v1/forecast.json?key=d8c869676cba4733b9f230353180108&q=-34.669090,-58.564331&days=1";
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
            ComenzarContador();
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
        public void ComenzarContador()
        {
            timer = new Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(AccionTimer);
            timer.Enabled = true;
        }
        /// <summary>
        /// Esta es la acción que se ejecuta cada vez que transcurre el lapso definido
        /// en el timer.
        /// Acá irían las actividades de sincronización y verificación de datos y demás.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AccionTimer(object sender, ElapsedEventArgs e)
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
                        // Si es información de una sincronización, la guardo en la BD.
                        InformacionSincronizada informacionSincronizada = new InformacionSincronizada { Fecha = DateTime.Now, Data = respuestaSincronizacion.Data.ToString()};
                        DatabaseHelper.db.DeleteAll<InformacionSincronizada>();
                        DatabaseHelper.db.Insert(informacionSincronizada);
                        break;
                    case (int)EnumCodigos.NuevoElemento:
                        // Si es información de un nuevo elemento solicitado, lo guardo en la tabla temporal.
                        string nuevoUUID = respuestaSincronizacion.Data.ToString();
                        ElementoAgregado nuevoElemento = new ElementoAgregado { UUID = nuevoUUID };
                        DatabaseHelper.db.Insert(nuevoElemento);
                        break;
                    case (int)EnumCodigos.Alarma:
                        // Si es información de la mochila en modo 'alarma', hay que guardarla también
                        // TODO: DEFINIR
                        bool mochilaAbierta = (Convert.ToBoolean(respuestaSincronizacion.Data));
                        break;
                    default:
                        break;
                }
            }

            // SEGUNDO - Chequeo si hay algún recordatorio en el proximo minuto configurado para comparar qué tengo y qué tengo que tener.
            List<Recordatorio> recordatorios = ObtenerRecordatoriosDelDia();

            if (recordatorios.Count > 0)
            {
                // Primero ordeno para el más reciente
                recordatorios = recordatorios.OrderBy(x => x.Horario).ToList();
                foreach (Recordatorio recordatorio in recordatorios)
                {
                    if (Math.Abs((recordatorio.Horario - DateTime.Now.TimeOfDay).TotalMinutes) < 1)
                    {
                        // Entonces, es hora de notificar el recordatorio
                        string mensajeNotificacion = String.Empty;
                        if (recordatorios.IndexOf(recordatorio) == 0)
                        {
                            HttpResponseMessage response = await client.GetAsync(urlClima);
                            // Es el primero del día, hay que buscar la info del clima,
                            // y el saludo.
                            //mensajeNotificacion += await ObtenerInformacionClima(response);
                            mensajeNotificacion += "Primer mensaje del día! Habrá lluvias hoy. ";
                            mensajeNotificacion += ObtenerInformacionBateria();
                        }

                        mensajeNotificacion += ObtenerMensajeElementos(recordatorio.Elementos);

                        EnviarNotificacion(mensajeNotificacion);

                        break;
                    }
                }
            }
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

        private List<Recordatorio> ObtenerRecordatoriosDelDia()
        {
            List<Recordatorio> recordatorios = new List<Recordatorio>();

            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Lunes).ToList();
                    break;
                case DayOfWeek.Thursday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Martes).ToList();
                    break;
                case DayOfWeek.Wednesday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Miercoles).ToList();
                    break;
                case DayOfWeek.Tuesday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Jueves).ToList();
                    break;
                case DayOfWeek.Friday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Viernes).ToList();
                    break;
                case DayOfWeek.Saturday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Sabado).ToList();
                    break;
                case DayOfWeek.Sunday:
                    recordatorios = DatabaseHelper.db.GetAllWithChildren<Recordatorio>().Where(x => x.Domingo).ToList();
                    break;
                default:
                    break;
            }

            return recordatorios;
        }

        private async Task<string> ObtenerInformacionClima(HttpResponseMessage respuestaClima)
        {
                string respuestaString = await respuestaClima.Content.ReadAsStringAsync();
                ClimaResponse respuesta = JsonConvert.DeserializeObject<ClimaResponse>(respuestaString);

                // CHEQUEAR PARAGUAS

                if (CodigosLluvia.Codigos.Contains(respuesta.forecast.forecastday[0].day.condition.code))
                {
                    return "Primer mensaje del día! Habrá lluvias hoy. ";
                }
                else
                {
                    return "Primer mensaje del día! Hoy no llueve! ";
                }
        }

        private string ObtenerInformacionBateria()
        {
            string mensaje = string.Empty;
            int PorcentajeBateria = DependencyService.Get<IBattery>().RemainingChargePercent;
            if (PorcentajeBateria < 50)
            {
                // CHEQUEAR CARGADOR
                mensaje += "No te olvides el cargador!";
            }

            return mensaje;
        }

        private void EnviarNotificacion(string mensajeNotificacion)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetContentTitle("Hola!")
                // Esta linea es para que vibre y suene. Por ahora queda deshabilitada porque
                // es un dolor de pelotas que suene todo el tiempo.
                //.SetDefaults(NotificationDefaults.Vibrate | NotificationDefaults.Sound)
                .SetSmallIcon(Resource.Drawable.BaggyLogo1)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(mensajeNotificacion))
                .SetContentText(mensajeNotificacion);

            Notification notification = builder.Build();
            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }

        private string ObtenerMensajeElementos(List<Elemento> elementosEnRecordatorio)
        {
            string mensaje = String.Empty;
            // Debo comparar los elementos que me llegaron por parámetro con los elementos que tengo en la mochila,
            // que los voy a sacar de la tabla de sincronización.
            InformacionSincronizada informacion = DatabaseHelper.db.Table<InformacionSincronizada>().FirstOrDefault();
            List<string> UUIDsEnMochila = informacion.Data.Split(',').ToList();
            List<string> UUIDsEnRecordatorio = elementosEnRecordatorio.Select(x => x.UUID).ToList();

            // PRIMERO - Me fijo si falta algo
            foreach (string UUIDEnRecordatorio in UUIDsEnRecordatorio)
            {
                List<Elemento> elementosOlvidados = new List<Elemento>();
                if (!UUIDsEnMochila.Contains(UUIDEnRecordatorio))
                {
                    Elemento elementoOlvidado = DatabaseHelper.db.Table<Elemento>().FirstOrDefault(e => e.UUID.Equals(UUIDEnRecordatorio));
                    elementosOlvidados.Add(elementoOlvidado);
                }

                if (elementosOlvidados.Count > 0)
                {
                    mensaje +=
                        (elementosOlvidados.Count == 1 ? "Te estás olvidando el siguiente elemento: " : "Te estás olviando los siguientes elementos: ") +
                        String.Join(", ", elementosOlvidados.Select(e => e.Descripcion)) +
                        "!";
                }
            }

            // SEGUNDO - Me fijo si sobra algo
            foreach (string UUIDEnMochila in UUIDsEnMochila)
            {
                List<Elemento> elementosSobrantes = new List<Elemento>();
                if (!UUIDsEnRecordatorio.Contains(UUIDEnMochila))
                {
                    Elemento elementoSobrante = DatabaseHelper.db.Table<Elemento>().FirstOrDefault(e => e.UUID.Equals(UUIDEnMochila));
                    elementosSobrantes.Add(elementoSobrante);
                }

                if (elementosSobrantes.Count > 0)
                {
                    mensaje +=
                        (elementosSobrantes.Count == 1 ? "Te sobra el siguiente elemento: " : "Te sobran los siguientes elementos: ") +
                        String.Join(", ", elementosSobrantes.Select(e => e.Descripcion)) +
                        "!";
                }
            }

            // Si el mensaje sigue vacío hasta acá es porque no falta nada
            if (String.IsNullOrEmpty(mensaje))
            {
                mensaje = "Tenes todo lo que necesitas!";
            }

            return mensaje;
        }
    }
}