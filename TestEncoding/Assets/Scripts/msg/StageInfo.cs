namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="StageInfo")]
    public class StageInfo : IExtensible
    {
        private uint _leftTimes;
        private uint _stageid;
        private uint _star;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((long) 0L), ProtoMember(3, IsRequired=false, Name="leftTimes", DataFormat=DataFormat.TwosComplement)]
        public uint leftTimes
        {
            get
            {
                return this._leftTimes;
            }
            set
            {
                this._leftTimes = value;
            }
        }

        [ProtoMember(1, IsRequired=false, Name="stageid", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [ProtoMember(2, IsRequired=false, Name="star", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint star
        {
            get
            {
                return this._star;
            }
            set
            {
                this._star = value;
            }
        }
    }
}

