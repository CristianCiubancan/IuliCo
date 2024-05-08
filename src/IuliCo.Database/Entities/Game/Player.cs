namespace IuliCo.Database.Entities.Game
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }

        public Player()
        {
            Name = string.Empty;
            Level = 0;
            Experience = 0;
        }
    }
}