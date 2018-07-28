﻿using System;
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
        
        public Command ComandoEliminarMochila { get; set; }

        public MochilaViewModel() {
            this.ComandoEliminarMochila = new Command(Eliminar);
        }

        void Eliminar() {
            MessagingCenter.Send(this, "EliminarMochila", this);
        }
    }
}
