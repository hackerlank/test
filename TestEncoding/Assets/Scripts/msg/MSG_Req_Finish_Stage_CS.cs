namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Req_Finish_Stage_CS")]
    public class MSG_Req_Finish_Stage_CS : IExtensible
    {
        private uint _starnum;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="starnum", DataFormat=DataFormat.TwosComplement)]
        public uint starnum
        {
            get
            {
                return this._starnum;
            }
            set
            {
                this._starnum = value;
            }
        }
    }
}

