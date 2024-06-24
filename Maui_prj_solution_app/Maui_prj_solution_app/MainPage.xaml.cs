using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Maui.Controls;

namespace Maui_prj_solution_app
{
    public partial class MainPage : ContentPage
    {
        private static readonly HttpClient client = new HttpClient();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnRechercherClicked(object sender, EventArgs e)
        {
            string prenom = PrenomEntry.Text;
            string annee = AnneeEntry.Text;

            if (string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(annee))
            {
                await DisplayAlert("Erreur", "Veuillez entrer un prénom et une année.", "OK");
                return;
            }

            string result = await GetNombreEnfantsPrenom(prenom, annee);
            ResultatLabel.Text = result;
        }

        private async Task<string> GetNombreEnfantsPrenom(string prenom, string annee)
        {
            string url = $"https://data.nantesmetropole.fr/api/records/1.0/search/?dataset=244400404_prenoms-enfants-nes-nantes&q=&facet=prenom&facet=annee&refine.prenom={prenom}&refine.annee={annee}";

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseBody);
                int count = json["nhits"].Value<int>();

                return $"Nombre d'enfants nés avec le prénom {prenom} en {annee} : {count}";
            }
            else
            {
                return "Erreur lors de la récupération des données.";
            }
        }
    }
}