namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_MapScreenRemoveNpc_SC")]
    public class MSG_Ret_MapScreenRemoveNpc_SC : IExtensible
    {
        private ulong _tempid;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="tempid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
        public ulong tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }
    }
}

