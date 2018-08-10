using System;

using MobileExample.Tables;

namespace MobileExample.ViewModels
{
    public class VerElementoViewModel : BaseViewModel
    {
        public Elemento Item { get; set; }
        public VerElementoViewModel(Elemento item = null)
        {
            Title = item?.Descripcion;
            Item = item;
        }
    }
}

