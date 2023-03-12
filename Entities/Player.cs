namespace ChairsBackend.Entities
{
    public class Player:BaseEntity
    {
        public string Name { get; set; }

        public Player(string name)
        {
            Name=name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
