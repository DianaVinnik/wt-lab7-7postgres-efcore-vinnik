namespace wt_lab6_6minimalapi_vinnik.Models;

public class Account
{
    public int Id { get; set; }
    public string Owner { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string AccountType { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}