public class UserHairTimeDto
{
    public long UserId { get; set; }
    public int Hair { get; set; }
    public string LastActiveTime { get; set; }

    public UserHairTimeDto()
    {

    }
    public UserHairTimeDto(User user)
    {
        UserId = user.GetUserId();
        Hair = user.GetHair();
        LastActiveTime = user.GetLastActiveTime();
    }
}
