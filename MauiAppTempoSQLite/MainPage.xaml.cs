using System.Collections.ObjectModel;
using System.Diagnostics;
using MauiAppTempoSQLite.Models;
using MauiAppTempoSQLite.Services;

namespace MauiAppTempoSQLite
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Tempo> lista = new ObservableCollection<Tempo>();

        public MainPage()
        {
            InitializeComponent();

            lst_previsoes_tempo.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            try
            {
                lista.Clear();
                App.Db.GetAll().Result.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null)
                    {
                        t.Cidade = txt_cidade.Text; // Atribui a cidade ao objeto Tempo
                        t.DataConsulta = DateTime.Now; // Atribui a data atual ao objeto Tempo

                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n";

                        //lbl_res.Text = dados_previsao;

                       await App.Db.Insert(t);

                        //string msg = $"Total de {total_registros} registros salvos!";

                        //await DisplayAlert("Sucesso", msg, "OK");

                        lista.Clear();
                        App.Db.GetAll().Result.ForEach(i => lista.Add(i));
                    }
                    else
                    {
                        lbl_res.Text = "Sem dados de Previsão";
                    } // Fecha if t=null

                }
                else
                {
                    lbl_res.Text = "Preencha a cidade.";
                } // fecha if string is null or empty

            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private void lst_previsoes_tempo_Refreshing(object sender, EventArgs e)
        {

        }

        private void lst_previsoes_tempo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;

                lista.Clear();

                List<Tempo> tmp = await App.Db.Search(q);

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }            
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem selecinado = sender as MenuItem;
                Tempo t = selecinado.BindingContext as Tempo;

                bool confirm = await DisplayAlert(
                    "Tem Certeza?", $"Remover previsão para {t.Cidade}?", "Sim", "Não");

                if (confirm)
                {
                    await App.Db.Delete(t.Id);
                    lista.Remove(t);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

}
