using DevJobsterAPI.Models.Security;

namespace DevJobsterAPI.Models.Admin;

public class AdminRegisteredAccount
{
    public AdminRegisteredAccount()
    {
    }

    public AdminRegisteredAccount(Admin admin, RegisteredAccount registeredAccount)
    {
        Admin = admin;
        RegisteredAccount = registeredAccount;
    }

    public int AdminId { get; set; }
    public int RegisteredAccountId { get; set; }

    public required Admin Admin { get; set; }
    public required RegisteredAccount RegisteredAccount { get; set; }
}