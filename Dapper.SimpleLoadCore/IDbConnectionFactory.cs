using System;
using System.Data;

namespace Dapper.SimpleLoadCore
{
    public interface IDbConnectionFactory : IDisposable
    {
        IDbConnection GetConnection();
    }
}
