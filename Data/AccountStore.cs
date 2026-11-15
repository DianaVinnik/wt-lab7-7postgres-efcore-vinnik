using wt_lab6_6minimalapi_vinnik.Models;

namespace wt_lab6_6minimalapi_vinnik.Data;

public static class AccountStore
{
    public static List<Account> Accounts { get; } = new()
    {
        new Account { Id = 1, Owner = "Иван Иванов",     AccountNumber = "BY001", Balance = 2500m, AccountType = "Сберегательный", IsActive = true  },
        new Account { Id = 2, Owner = "Анна Петрова",    AccountNumber = "BY002", Balance = 5000m, AccountType = "Кредитный",      IsActive = true  },
        new Account { Id = 3, Owner = "Пётр Сидоров",    AccountNumber = "BY003", Balance = 1200m, AccountType = "Сберегательный", IsActive = false },
        new Account { Id = 4, Owner = "Мария Козлова",   AccountNumber = "BY004", Balance = 7800m, AccountType = "Сберегательный", IsActive = true  },
        new Account { Id = 5, Owner = "Алексей Смирнов", AccountNumber = "BY005", Balance = 3500m, AccountType = "Кредитный",      IsActive = true  }
    };
}