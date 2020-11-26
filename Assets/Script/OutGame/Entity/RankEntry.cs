public class RankEntry
{
    public int Rank { get; set; }
    public string NickName { get; set; }
    public string Avatar { get; set; }
    public int Level { get; set; }
    public double Value { get; set; }

    public override string ToString()
    {
        return "RankEntry{" +
                "Rank=" + Rank +
                ", NickName=" + NickName +
                ", Avatar=" + Avatar +
                ", Level='" + Level + '\'' +
                ", Value='" + Value + '\'' +
                '}';
    }
}
