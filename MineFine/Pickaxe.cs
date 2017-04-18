namespace MineFine
{
    public class Pickaxe
    {
        public string Name { get; set; }
        public int CoolDownRate { get; set; }
        public int Cost { get; set; }

        public Pickaxe(string name, int cdrate, int cost)
        {
            this.Name = name;
            this.CoolDownRate = cdrate;
            this.Cost = cost;

        }
    }
}


