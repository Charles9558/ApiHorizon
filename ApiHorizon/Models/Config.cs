namespace ApiHorizon.Models
{
    public class Config
    {
        public int config_id { get; set; } 
        public TimeSpan config_arrive {  get; set; }
        public TimeSpan config_depart {  get; set; }
        public string config_jours { get; set; }
        public string config_theme { get; set; }
    }
}
