using System;

namespace DA.WI.NSGHSM.Core.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(Type resourceType, object key)
        : base($"Resource of type [{resourceType.Name}] with key [{key}] not found.")
        {
        }

        public NotFoundException(string resourceType, object key)
        : base($"Resource of type [{resourceType}] with key [{key}] not found.")
        {
        }

        public NotFoundException(Type resourceType, object key1, object key2)
        : base($"Resources of type [{resourceType}] with keys [{key1}] and [{key2}] are not found.")
        {
        }

    }
}
