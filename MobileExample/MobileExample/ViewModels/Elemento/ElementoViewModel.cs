using MobileExample.Tables;
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

        public bool Seleccionado { get; set; }

        public string UUID { get; set; }

        public Command ComandoEliminarElemento { get; set; }

        public Command ComandoEditarElemento { get; set; }

        public int IdInterno { get; set; }

        public ElementoViewModel()
        {
            this.ComandoEliminarElemento = new Command(Eliminar);
            this.ComandoEditarElemento = new Command(Editar);
        }

        void Eliminar()
        {
          MessagingCenter.Send(this, "EliminarElemento", this);          
        }

        void Editar()
        {
            MessagingCenter.Send(this, "EditarElemento", this);
        }

        public static explicit operator Elemento(ElementoViewModel viewModel)
        {
            return new Elemento
            {
                Descripcion = viewModel.Descripcion,
                Id = viewModel.Id,
                Imprescindible = viewModel.Imprescindible,
                RutaIcono = viewModel.RutaIcono,
                UUID = viewModel.UUID,
                Vinculado = viewModel.Vinculado
            };
        }

        public static explicit operator ElementoViewModel(Elemento entity)
        {
            return new ElementoViewModel
            {
                Descripcion = entity.Descripcion,
                Id = entity.Id,
                Imprescindible = entity.Imprescindible,
                RutaIcono = entity.RutaIcono,
                UUID = entity.UUID,
                Vinculado = entity.Vinculado
            };
        }
    }
}
