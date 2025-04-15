using System.Data;
using Dapper;

namespace DevJobsterAPI.Common;

public class UserTypeHandler : SqlMapper.TypeHandler<UserType>
{
    public override void SetValue(IDbDataParameter parameter, UserType value)
    {
        parameter.Value = value.ToString();
    }

    public override UserType Parse(object value)
    {
        return Enum.Parse<UserType>(value.ToString(), true);
    }
}