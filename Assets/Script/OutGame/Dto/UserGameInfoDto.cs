public class UserGameInfoDto
{
    public long UserId { get; set; }
    public int Exp { get; set; }
    public int NikeCoin { get; set; }
    public int Credit { get; set; }
    public string Prop { get; set; }
    public string Card { get; set; }
    public string Manual { get; set; }
    public string Achievement { get; set; }


    public UserGameInfoDto()
    {

    }
    public UserGameInfoDto(User user)
    {
        UserId = user.GetUserId();
        Exp = user.GetExp();
        NikeCoin = user.GetNikeCoin();
        Credit = user.GetCredit();
        Prop = user.GetProp();
        Card = user.GetCard();
        Manual = user.GetManual();
        Achievement = user.GetAchievement();
    }
}
