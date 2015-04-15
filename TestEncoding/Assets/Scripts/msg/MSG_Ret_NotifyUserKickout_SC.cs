namespace msg
{
    using ProtoBuf;
    using System;

    [Serializable, ProtoContract(Name="MSG_Ret_NotifyUserKickout_SC")]
    public class MSG_Ret_NotifyUserKickout_SC : IExtensible
    {
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }
    }
}

