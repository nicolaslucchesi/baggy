using System;

using MobileExample.Tables;

namespace MobileExample.ViewModels
{
    public class VerRecordatorioViewModel : BaseViewModel
    {
        public Recordatorio item { get; set; }
        public VerRecordatorioViewModel(Recordatorio item = null)
        {
            Title = "Recordatorio";        }
    }
}
