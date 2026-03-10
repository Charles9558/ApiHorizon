namespace ApiHorizon.Models
{
    public class Conge
    {
        public int conge_id { get; set; }
        public DateTime conge_date_depart { get; set; }
        public DateTime conge_date_retour { get; set; } 
        public string conge_motif { get; set; }
        public string conge_motif_description { get;set; }
        public string conge_statut { get; set; }
        public Personnel conge_personnel { get; set; }
    }
}
