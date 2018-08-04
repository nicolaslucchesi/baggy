using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using System.IO;
using SQLite;
using MobileExample.Tables;
using MobileExample.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace MobileExample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevoRecordatorio : ContentPage
    {
        public RecordatorioViewModel RecordatorioViewModel { get; set; }
        public ObservableCollection<int> Elementos { get; set; }
        
        public NuevoRecordatorio()
        {
            InitializeComponent();

            RecordatorioViewModel = new RecordatorioViewModel
            {
                DiaSemana = 1,
                Horario = new TimeSpan(12,0,0),
                Hora = 0,
                Minuto = 3
            };


            Elementos = new ObservableCollection<int>();

            BindingContext = this;
            
            MessagingCenter.Subscribe<SeleccionarElementos, ElementoViewModel>(this, "ElementoSeleccionado", (obj, elementoViewModel) =>
            {
                Elementos.Add(elementoViewModel.Id);
                //TodosLosInt.Text = TodosLosInt.Text + Elementos.ToString();
                foreach (int id in Elementos.ToList())
                {
                    TodosLosInt.Text = TodosLosInt.Text + id;
                }
                
            });


        }

        async void Guardar_Clicked(object sender, EventArgs e)
        {
            // Acá se manda el mensaje con el modelo y el titulo para que el modelo de
            // listado ejecute el código de guardado.
            RecordatorioViewModel SendRecordatorio = new RecordatorioViewModel();
            SendRecordatorio = RecordatorioViewModel;
            SendRecordatorio.Elementos = Elementos.ToList();
                

            //MessagingCenter.Send(this, "AgregarRecordatorio", SendRecordatorio);
            await Navigation.PushAsync(new SeleccionarElementos(SendRecordatorio));

        }
        

        async void ApretarBotonLunes(object sender, EventArgs e)
        {
            if (SwitchLunes.IsToggled)
            {
                SwitchLunes.IsToggled = false;
            }
            else
            {
                SwitchLunes.IsToggled = true;
            }
        }

        async void ApretarBotonMartes(object sender, EventArgs e)
        {
            if (SwitchMartes.IsToggled)
            {
                SwitchMartes.IsToggled = false;
            }
            else
            {
                SwitchMartes.IsToggled = true;
            }
        }

        async void ApretarBotonMiercoles(object sender, EventArgs e)
        {
            if (SwitchMiercoles.IsToggled)
            {
                SwitchMiercoles.IsToggled = false;
            }
            else
            {
                SwitchMiercoles.IsToggled = true;
            }
        }
        async void ApretarBotonJueves(object sender, EventArgs e)
        {
            if (SwitchJueves.IsToggled)
            {
                SwitchJueves.IsToggled = false;
            }
            else
            {
                SwitchJueves.IsToggled = true;
            }
        }
        async void ApretarBotonViernes(object sender, EventArgs e)
        {
            if (SwitchViernes.IsToggled)
            {
                SwitchViernes.IsToggled = false;
            }
            else
            {
                SwitchViernes.IsToggled = true;
            }
        }

    }
}