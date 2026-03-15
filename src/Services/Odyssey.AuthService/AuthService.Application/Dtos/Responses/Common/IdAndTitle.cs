namespace AuthService.Application.Dtos.Responses.Common;

public class IdAndTitle
{
    public IdAndTitle(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; set; }
    public string Title { get; set; }
}