using System;

namespace Dapper.SimpleSaveCore.Impl
{
    public abstract class BaseCommand
    {
        public string TableName { get; protected set; }

        public string PrimaryKeyColumn { get; protected set; }
    }
}
