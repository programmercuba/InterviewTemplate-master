namespace Football.API.Models.Output
{
    public class Statistics
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Total { get; set; }
    }

    public enum StatisticsType { YellowCard, RedCard, MinutesPlayed }
}
