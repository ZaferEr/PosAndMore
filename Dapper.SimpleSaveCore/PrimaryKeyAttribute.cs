using System;

namespace Dapper.SimpleSaveCore
{
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute() : this(false)
        {
        }

        public PrimaryKeyAttribute(bool isUserAssigned)
        {
            IsUserAssigned = isUserAssigned;
        }

        public bool IsUserAssigned { get; private set; }
    }
}
