namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_MapScreenRemoveCharacter_SC")]
    public class MSG_Ret_MapScreenRemoveCharacter_SC : IExtensible
    {
        private ulong _charid;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="charid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }
    }
}

