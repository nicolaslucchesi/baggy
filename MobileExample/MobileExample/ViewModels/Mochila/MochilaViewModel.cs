using MobileExample.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileExample.ViewModels
{
    public class MochilaViewModel : BaseViewModel
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string UUID { get; set; }

        public bool Activa { get; set; }

        public bool EstadoAlarma { get; set; }

        public string Elementos { get; set; }

        public Command ComandoEliminarMochila { get; set; }

        public Command ComandoActivarMochila { get; set; }

        public Command ComandoActivarAlarma { get; set; }

        public Command ComandoVerMochila { get; set; }

        public string RutaIcono { get; set; }

        public MochilaViewModel()
        {
            this.ComandoEliminarMochila = new Command(Eliminar);
            this.ComandoActivarMochila = new Command(Activar);
            this.ComandoActivarAlarma = new Command(ActivarAlarma);
            this.ComandoVerMochila = new Command(Ver);
        }

        void Eliminar()
        {
            MessagingCenter.Send(this, "EliminarMochila", this);
        }

        void Activar()
        {
            MessagingCenter.Send(this, "ActivarMochila", this);
        }

        void ActivarAlarma()
        {
            MessagingCenter.Send(this, "ActivarAlarma", this);
        }

        void Ver()
        {
            MessagingCenter.Send(this, "VerMochila", this);
        }

        public static explicit operator MochilaViewModel(Mochila mochila)
        {
            return new MochilaViewModel
            {
                Activa = mochila.Activa,
                Descripcion = mochila.Descripcion,
                EstadoAlarma = mochila.EstadoAlarma,
                Id = mochila.Id,
                UUID = mochila.UUID,
                RutaIcono = mochila.RutaIcono,
                Elementos = mochila.Elementos
            };
        }
    }
}
