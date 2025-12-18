using System;

namespace Dapper.SimpleSaveCore
{
    public class ForeignKeyReferenceAttribute : Attribute
    {
        public Type ReferencedDto { get; private set; }

        public ForeignKeyReferenceAttribute(Type referencedDto)
        {
            ReferencedDto = referencedDto;
        }
    }
}
