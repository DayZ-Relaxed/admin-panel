namespace DayZRelaxed.Models
{
    public class PlayerLog
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string PlayerName { get; set; }
        public string DayzId { get; set; } 
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }

        private readonly double radius = 60.0;

        public bool IsInsideCircle(Territory territory) { 
            var distance = Math.Sqrt(Math.Pow(this.PosX - territory.PosX, 2) + Math.Pow(this.PosY - territory.PosY, 2));
            return distance <= radius;
        }
    }

    
}
