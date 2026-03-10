namespace ApiHorizon.Models
{
    public class Pointage
    {
        public int pointage_id { get; set; } 
        public DateTime pointage_jour { get; set; }
        public TimeSpan pointage_arrive { get; set; }
        public TimeSpan pointage_depart { get; set; }
        public Personnel pointage_personnel { get; set; }
    }
}
