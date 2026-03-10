namespace ApiHorizon.Models
{
    public class Notification
    {
        public int notification_id { get; set; }
        public string notification_objet { get; set; }
        public DateTime notification_date_creation {  get; set; }
        public string notification_statut { get; set; }
        public string notification_description { get; set; }
        public Personnel notification_concerne { get; set; }
        public Personnel notification_destinataire { get; set; }
    }
}
