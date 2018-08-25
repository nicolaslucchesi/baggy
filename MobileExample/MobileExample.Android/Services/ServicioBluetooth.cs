using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using MobileExample.Services;
using MobileExample.Tables;
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
        Handler mainHandler = new Handler(Looper.MainLooper);
        string data = bluetoothService.Sincronizar();
    }
}