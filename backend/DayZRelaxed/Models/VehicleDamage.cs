namespace DayZRelaxed.Models
{
    public class VehicleDamage
    {
        public DateTime Date { get; set; }
        public string PlayerName { get; set; }
        public string VehicleName { get; set; }
        public string Weapon { get; set; }
        public string Ammo { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public string? Zone { get; set; }

        public bool IsInsideCircle(double x, double z, double radius)
        {
            var distance = Math.Sqrt(Math.Pow(this.PosX - x, 2) + Math.Pow(this.PosZ - z, 2));
            return distance <= radius;
        }
    }
}
