public class UserEmailDto
{
    public long UserId { get; set; }
    public int NikeCoin { get; set; }
    public int Credit { get; set; }
    public string Prop { get; set; }
    public string Achievement { get; set; }

    public UserEmailDto()
    {

    }
    public UserEmailDto(User user)
    {
        UserId = user.GetUserId();
        NikeCoin = user.GetNikeCoin();
        Credit = user.GetCredit();
        Prop = user.GetProp();
        Achievement = user.GetAchievement();
    }
}
