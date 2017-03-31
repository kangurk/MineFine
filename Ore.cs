namespace minefine
{
    public class Ore
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int OreCount { get; set; }
        public Ore(string name, string image, int orecount)
        {
            this.Name = name;
            this.Image = name;
            this.OreCount = orecount;
        }
    }
}

