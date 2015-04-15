namespace msg
{
    using ProtoBuf;
    using System;

    [Serializable, ProtoContract(Name="MSG_Req_Stage_Info_CS")]
    public class MSG_Req_Stage_Info_CS : IExtensible
    {
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }
    }
}

