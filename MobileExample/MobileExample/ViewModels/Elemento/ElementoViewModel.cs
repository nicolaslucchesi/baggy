using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileExample.ViewModels
{
    public class ElementoViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string RutaIcono { get; set; }

        public bool Imprescindible { get; set; }

        public bool Vinculado { get; set; }

        public string UUID { get; set; }

        public Command ComandoEliminarElemento { get; set; }

        public ElementoViewModel()
        {
            this.ComandoEliminarElemento = new Command(Eliminar);
        }

        void Eliminar()
        {
          MessagingCenter.Send(this, "EliminarElemento", this);          
        }
    }
}
