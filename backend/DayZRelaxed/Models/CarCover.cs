namespace DayZRelaxed.Models
{
    public class CarCover
    {
        public DateTime Date { get; set; }
        public string PlayerName { get; set; }
        public string SteamId { get; set; }
        public string Action { get; set; }
        public string Car { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }

        private readonly double coordinateRounder = 5.0;


        public bool IsInsidePosX(double x)
        {
            return x >= this.PosX - coordinateRounder && x <= this.PosX + coordinateRounder;
        }

        public bool IsInsidePosY(double y)
        {
            return y >= this.PosY - coordinateRounder && y <= this.PosY + coordinateRounder;
        }

        public bool IsInsidePosZ(double z)
        {
            return z >= this.PosZ - coordinateRounder && z <= this.PosZ + coordinateRounder;
        }
    }
}
