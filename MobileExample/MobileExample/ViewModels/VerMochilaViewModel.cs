using System;

using MobileExample.Tables;

namespace MobileExample.ViewModels
{
    public class VerMochilaViewModel : BaseViewModel
    {
        public Mochila Item { get; set; }
        public VerMochilaViewModel(Mochila item = null)
        {
            Title = item?.Descripcion;
            Item = item;
        }
    }
}
