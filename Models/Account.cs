using System.ComponentModel.DataAnnotations;

namespace wt_lab7_7postgres_efcore_vinnik.Models;

public class Account
{
    public int Id { get; set; }

    [Required]
    public string Owner { get; set; } = string.Empty;

    [Required]
    public string AccountNumber { get; set; } = string.Empty;

    public decimal Balance { get; set; }

    public string AccountType { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string Description { get; set; } = string.Empty;
}

