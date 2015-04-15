namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_Common_Error_SC")]
    public class MSG_Ret_Common_Error_SC : IExtensible
    {
        private uint _errorcode;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="errorcode", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint errorcode
        {
            get
            {
                return this._errorcode;
            }
            set
            {
                this._errorcode = value;
            }
        }
    }
}

