namespace ApiHorizon.Models
{
    public class Personnel
    {
        public string Matricul { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Profil { get; set; }
        public string ProfilColor { get; set; }
        public string Pseudo { get; set; }
        public string Pass {  get; set; }
        public string Rol { get; set; }
        public string Statut { get; set; }
        public Poste poste { get; set; }
        public Config config { get; set; }
    }
}
