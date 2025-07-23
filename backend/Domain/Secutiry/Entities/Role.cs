namespace Domain.Security.Entities;

public class Role
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;

    protected Role() { }

    public Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Role Admin => new(1, "Admin");
    public static Role User => new(2, "User");
}
