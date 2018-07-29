using System;

using MobileExample.Tables;

namespace MobileExample.ViewModels
{
    public class VerRecordatorioViewModel : BaseViewModel
    {
        public RecordatorioViewModel Item { get; set; }
        public VerRecordatorioViewModel(RecordatorioViewModel item = null)
        {
            Title = "Recordatorio";
            Item = item;
        }
    }
}
