namespace MobileExample.Services
{
    public class BluetoothService
    {
        public BluetoothService() { }
        public string Sincronizar() {
            //return string.Empty;
            // Para probar sincronizacion de elementos:
            return @"{""Codigo"": 1, ""Data"": ""uuid-1,uuid-3"" }";
            // Para probar agregar un nuevo elemento:
            //return @"{""Codigo"": 2, ""Data"": ""uuid-7"" }";
            // Para probar la alarma:
            // return "{'Codigo': 3, 'Data': true}"
        }
    }
}