using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MobileExample.Database;
using MobileExample.Services;
using MobileExample.Sincronizacion;
using MobileExample.Tables;
using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
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
        timer = new Timer(2000);
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

        string textoNotificacion = string.Empty;
        // PRIMERO - Intentar sincronizar la información

        if (!string.IsNullOrEmpty(data))
        {
            ModeloRespuesta respuestaSincronizacion = JsonConvert.DeserializeObject<ModeloRespuesta>(data);
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
                    }
                    break;
                case (int)EnumCodigos.Bateria:

                    break;
                case (int)EnumCodigos.CierreAbierto:
                    // Si es información de la mochila en modo 'alarma', hay que guardarla también
                    // TODO: DEFINIR
                    bool mochilaAbierta = (Convert.ToBoolean(respuestaSincronizacion.Data));
                    break;
                default:
                    break;
            }
        }
    }
}