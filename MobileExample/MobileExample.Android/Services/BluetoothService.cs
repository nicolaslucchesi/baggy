using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MobileExample.Droid.Services
{
    public class BluetoothService
    {
        public BluetoothService() { }
        public string Sincronizar() {
            return string.Empty;
            // Para probar sincronizacion de elementos:
            // return "{'Codigo': 1, 'Data': '{1, 2, 3}'}";
            // Para probar agregar un nuevo elemento:
            // return "{'Codigo': 2, 'Data': 1}"
            // Para probar la alarma:
            // return "{'Codigo': 3, 'Data': true}"
        }
    }
}