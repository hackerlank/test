namespace msg
{
    using ProtoBuf;
    using System;

    [Serializable, ProtoContract(Name="MSG_Req_UserGameTime_SC")]
    public class MSG_Req_UserGameTime_SC : IExtensible
    {
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }
    }
}

