using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using MobileExample.Droid.Services;

namespace MobileExample.Droid
{
    [Activity(Label = "MobileExample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        Intent intentSegundoPlano;
        private SensorService servicioSegundoPlano;
        Context contexto;

        public Context obtenerContexto()
        {
            return contexto;
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            contexto = this;
            servicioSegundoPlano = new SensorService(this.obtenerContexto());
            intentSegundoPlano = new Intent(this.obtenerContexto(), typeof(SensorService));
            if (!servicioCorriendo(servicioSegundoPlano))
            {
                StartService(intentSegundoPlano);
            }

            LoadApplication(new App());
        }

        protected override void OnDestroy()
        {
            StopService(intentSegundoPlano);
            Console.WriteLine("Servicio parado.");
            base.OnDestroy();
        }

        private Boolean servicioCorriendo(SensorService servicioBackground)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            foreach (ActivityManager.RunningServiceInfo servicio in manager.GetRunningServices(int.MaxValue))
            {
                if (servicio.Class.Equals(servicio.Service.Class))
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

