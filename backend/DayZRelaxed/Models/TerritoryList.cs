namespace DayZRelaxed.Models
{
    public class TerritoryList : Territory
    {
        public string OwnerPlayerName { get; set; }
        public string OwnerBattleEyeId { get; set; }
        public string OwnerSteamId { get; set; }
        public string OwnerDayzId { get; set; }
        public string OwnerCountry { get; set; }
        public DateTime OwnerLastLogin { get; set; }
    }
}
