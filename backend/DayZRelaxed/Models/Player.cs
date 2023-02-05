namespace DayZRelaxed.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string BattleEyeId { get; set; }
        public string SteamId { get; set; }
        public string DayzId { get; set; }
        public string Country { get; set; }
        public DateTime LastLogin { get; set; }
    }

    public class Territory
    {
        public int TerritoryId { get; set; }
        public int OwnerPlayerId { get; set; }
        public string OwnerDayzId { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string PosZ { get; set; }
        public DateTime LastFound { get; set; }
    }

    public class TerritoryMember
    {
        public int TerritoryMemberId { get; set; }
        public int TerritoryId { get; set; }
        public int? MemberPlayerId { get; set; }
        public string MemberDayzId { get; set; }
        public DateTime LastFound { get; set; }
    }
}
