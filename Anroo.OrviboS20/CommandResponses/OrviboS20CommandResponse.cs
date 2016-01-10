using Anroo.Common;

namespace Anroo.OrviboS20
{
    public abstract class OrviboS20CommandResponse
    {
        public HostIdentity NetworkAddress;
        internal byte[] CommandId;
    }
}