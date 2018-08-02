namespace MobileExample.Droid.Clima
{
    public class ClimaResponse
    {
        public Location location { get; set; }
        public Current current { get; set; }
        public Forecast forecast { get; set; }
    }
}