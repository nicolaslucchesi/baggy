using System;

using MobileExample.Models;

namespace MobileExample.ViewModels
{
    public class VerMochilaViewModel : BaseViewModel
    {
        public Mochila Item { get; set; }
        public VerMochilaViewModel(Mochila item = null)
        {
            Title = item?.Text;
            Item = item;
        }
    }
}
