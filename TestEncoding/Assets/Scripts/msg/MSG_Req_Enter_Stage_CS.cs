namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_Enter_Stage_CS")]
    public class MSG_Req_Enter_Stage_CS : IExtensible
    {
        private uint _stageid;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="stageid", DataFormat=DataFormat.TwosComplement)]
        public uint stageid
        {
            get
            {
                return this._stageid;
            }
            set
            {
                this._stageid = value;
            }
        }
    }
}

