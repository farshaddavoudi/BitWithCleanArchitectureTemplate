namespace Template.Domain.Options;

public class AuthOptions
{
    public string? MasterPassword { get; set; }

    public bool? EnableDirectLogin { get; set; }

    public bool? IsAppPublic { get; set; }
}