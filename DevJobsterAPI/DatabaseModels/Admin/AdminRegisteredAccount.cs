using DevJobsterAPI.DatabaseModels.Security;

namespace DevJobsterAPI.DatabaseModels.Admin;

public class AdminRegisteredAccount
{
    private AdminRegisteredAccount()
    {
    }

    public AdminRegisteredAccount(Guid adminId, int registeredAccountId)
    {
        AdminId = adminId;
        RegisteredAccountId = registeredAccountId;
    }

    public AdminRegisteredAccount(Admin admin, RegisteredAccount registeredAccount)
    {
        Admin = admin;
        RegisteredAccount = registeredAccount;

        AdminId = admin.AdminId;
        RegisteredAccountId = registeredAccount.RegisteredAccountId;
    }

    public Guid AdminId { get; init; }
    public int RegisteredAccountId { get; init; }

    public Admin? Admin { get; init; }
    public RegisteredAccount? RegisteredAccount { get; init; }
}