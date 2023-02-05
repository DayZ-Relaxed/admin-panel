namespace DayZRelaxed.Models
{
    public class MapPosition
    {
        public DateTime Date { get; set; }
        public string PlayerName { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public double Radius { get; set; }

    public MapPosition(double radius)
    {
           Radius = radius;
    }

    public bool IsInsideCircle(double x, double z)
        {
            var distance = Math.Sqrt(Math.Pow(this.PosX - x, 2) + Math.Pow(this.PosY - z, 2));
            return distance <= Radius;
        }
    }
}
