using Android.App;
using Android.Content;
using Android.Support.V4.App;
using MobileExample.Database;
using MobileExample.Droid;
using MobileExample.Services;
using MobileExample.Sincronizacion;
using MobileExample.Tables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

[Service(Exported = true)]
[IntentFilter(new[] { "baggyFilter" })]
public class ServicioBluetooth : IntentService
{
    public BluetoothService bluetoothService = new BluetoothService();

    public ServicioBluetooth(Context contexto) : base("ServicioBluetooth")
    {
        Console.WriteLine("Empezó el servicio.");
    }

    public ServicioBluetooth()
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
    }

    private Timer timer;
    /// <summary>
    /// Este método es el que se ejecuta cuando empieza el servicio.
    /// </summary>
    public void ComenzarContador()
    {
        timer = new Timer(3001);
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
    private void AccionTimer(object sender, ElapsedEventArgs e)
    {
        string data = bluetoothService.Sincronizar();

        if (!string.IsNullOrEmpty(data))
        {
            ModeloRespuesta respuestaSincronizacion = JsonConvert.DeserializeObject<ModeloRespuesta>(data);
            Mochila mochilaActiva = DatabaseHelper.db.Table<Mochila>().FirstOrDefault(m => m.Activa);
            switch (respuestaSincronizacion.Codigo)
            {
                case (int)EnumCodigos.Elemento:
                    if (DatabaseHelper.db.Table<Configuracion>().FirstOrDefault().Vinculando)
                    {
                        // Si es información de un nuevo elemento solicitado, lo guardo en la tabla temporal.
                        string nuevoUUID = respuestaSincronizacion.Data.ToString();
                        ElementoAgregado nuevoElemento = new ElementoAgregado { UUID = nuevoUUID };
                        DatabaseHelper.db.Insert(nuevoElemento);
                    }
                    else
                    {
                        // Actualizar en la mochila activa/conectada la lista de elementos
                        // SI EXISTE Y ESTA REGISTRADO
                        string elementoRecibido = respuestaSincronizacion.Data.ToString();
                        if (DatabaseHelper.db.Table<Elemento>().Any(x => string.Equals(x.UUID, elementoRecibido)))
                        {
                            List<string> elementosEnMochila = mochilaActiva.Elementos.Split(',').ToList();
                            if (elementosEnMochila.Contains(elementoRecibido))
                            {
                                // Salió
                                elementosEnMochila.Remove(elementoRecibido);
                            }
                            else
                            {
                                // Entró
                                elementosEnMochila.Add(elementoRecibido);
                            }

                            mochilaActiva.Elementos = string.Join(",", elementosEnMochila);
                            DatabaseHelper.db.Update(mochilaActiva);
                        }
                        else
                        {
                            // Ver qué hacer si el elemento ingresado no está registrado.
                        }
                    }
                    break;
                case (int)EnumCodigos.Bateria:
                    // Definir qué pasa cuando Baggy se queda sin batería
                    break;
                case (int)EnumCodigos.CierreAbierto:
                    if (mochilaActiva.EstadoAlarma)
                    {
                        this.EnviarNotificacion("TE ESTAN CHOREANDO PAPÁ");
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void EnviarNotificacion(string mensajeNotificacion)
    {
        NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
            .SetContentTitle("Cuidado!")
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
}