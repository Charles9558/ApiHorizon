namespace ApiHorizon.Models
{
    public class Poste
    {
        public int poste_id { get; set; }
        public string poste_intitule { get; set; }
        public Departement poste_departement { get; set; }

    }
}
