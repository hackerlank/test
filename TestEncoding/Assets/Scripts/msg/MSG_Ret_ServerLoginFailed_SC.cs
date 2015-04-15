namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_ServerLoginFailed_SC")]
    public class MSG_Ret_ServerLoginFailed_SC : IExtensible
    {
        private uint _returncode;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="returncode", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint returncode
        {
            get
            {
                return this._returncode;
            }
            set
            {
                this._returncode = value;
            }
        }
    }
}

