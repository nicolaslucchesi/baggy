using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using MobileExample.Droid.Services;
using SQLite;
using System.IO;
using MobileExample.Tables;
using BottomNavigationBar;
using CarouselView.FormsPlugin.Android;
using FFImageLoading.Forms.Droid;

namespace MobileExample.Droid
{
    [Activity(Label = "MobileExample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent intentRecordatorio;
        Intent intentBluetooth;
        private ServicioRecordatorio servicioRecordatorio;
        private ServicioBluetooth servicioBluetooth;
        Context contexto;

        public Context obtenerContexto()
        {
            return contexto;
        }
        /// <summary>
        /// Este método handlea la creación de la aplicación (cuando inicia por primera vez)
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Rg.Plugins.Popup.Popup.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            FFImageLoading.Forms.Droid.CachedImageRenderer.Init(true);
            CarouselViewRenderer.Init();

            IniciarServicioRecordatorio();

            IniciarServicioBluetooth();
         
            LoadApplication(new App());
        }
        /// <summary>
        /// Este método handlea el evento de destrucción de la aplicación.
        /// Detiene el servicio que corre en segundo plano y envía un evento al constructor base.
        /// </summary>
        protected override void OnDestroy()
        {
            StopService(intentRecordatorio);
            Console.WriteLine("Servicio parado.");
            base.OnDestroy();
        }
        /// <summary>
        /// Este método inicia el servicio que corre en segundo plano
        /// si es que no está corriendo.
        /// </summary>
        private void IniciarServicioRecordatorio()
        {
            contexto = this;
            servicioRecordatorio = new ServicioRecordatorio(this.obtenerContexto());
            intentRecordatorio = new Intent(this.obtenerContexto(), typeof(ServicioRecordatorio));
            if (!servicioCorriendo(servicioRecordatorio))
            {
                StartService(intentRecordatorio);
            }
            
        }

        /// <summary>
        /// Este método inicia el servicio que corre en segundo plano
        /// si es que no está corriendo.
        /// </summary>
        private void IniciarServicioBluetooth()
        {
            contexto = this;
            servicioBluetooth = new ServicioBluetooth(this.obtenerContexto());
            intentBluetooth = new Intent(this.obtenerContexto(), typeof(ServicioBluetooth));
            if (!servicioCorriendo(servicioBluetooth))
            {
                StartService(intentBluetooth);
            }

        }
        /// <summary>
        /// Este método verifica si el servicio en segundo plano está corriendo
        /// para no iniciarlo dos veces.
        /// </summary>
        /// <param name="servicioBackground">La instancia del tipo de servicio a verificar</param>
        /// <returns>Un booleano que indica si el servicio está corriendo o no.</returns>
        private bool servicioCorriendo(IntentService servicioBackground)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            foreach (ActivityManager.RunningServiceInfo servicio in manager.GetRunningServices(int.MaxValue))
            {
                if (servicioBackground.Class.Name.Equals(servicio.Service.ClassName))
                {
                    Console.WriteLine("El servicio está corriendo!");
                    return true;
                }
            }
            Console.WriteLine("El servicio no está corriendo");
            return false;
        }
    }
}

