namespace MineFine
{
    public class Ore
    {
        public string Name { get; set; }
        public int Image { get; set; }
        public int OreCount { get; set; }
        public int OreCurrencyValue { get; set; }
        public int OreExpRate { get; set; }
        public bool IsOreUnlockedByUser { get; set; }
        public Ore(string name, int image, int orecount, int orecurrencyvalue, int oreexprate, bool IsOreUnlockedByUser)
        {
            this.Name = name;
            this.Image = image;
            this.OreCount = orecount;
            this.OreCurrencyValue = orecurrencyvalue;
            this.OreExpRate = oreexprate;
            this.IsOreUnlockedByUser = IsOreUnlockedByUser;
        }
    }
}

