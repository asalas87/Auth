namespace Web.API.Common.Request;
public class MultipleIdsRequest
{
    public List<Guid> Ids { get; set; } = new();
}
