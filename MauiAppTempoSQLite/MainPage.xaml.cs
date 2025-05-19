using System.Diagnostics;
using MauiAppTempoSQLite.Models;
using MauiAppTempoSQLite.Services;

namespace MauiAppTempoSQLite
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
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
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n";

                        lbl_res.Text = dados_previsao;

                        await App.Db.Insert(t);                      

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
    }

}
