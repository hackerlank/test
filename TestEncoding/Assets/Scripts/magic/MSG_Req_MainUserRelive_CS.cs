namespace magic
{
    using ProtoBuf;
    using System;

    [Serializable, ProtoContract(Name="MSG_Req_MainUserRelive_CS")]
    public class MSG_Req_MainUserRelive_CS : IExtensible
    {
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }
    }
}

