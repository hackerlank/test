namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MSG_Ret_Enter_Stage_SC")]
    public class MSG_Ret_Enter_Stage_SC : IExtensible
    {
        private readonly List<stageDropInfo> _drops = new List<stageDropInfo>();
        private uint _errorcode;
        private uint _stcritnum;
        private uint _sthitnum;
        private uint _ucritnum;
        private uint _uhitnum;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(2, Name="drops", DataFormat=DataFormat.Default)]
        public List<stageDropInfo> drops
        {
            get
            {
                return this._drops;
            }
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

        [ProtoMember(6, IsRequired=false, Name="stcritnum", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint stcritnum
        {
            get
            {
                return this._stcritnum;
            }
            set
            {
                this._stcritnum = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="sthitnum", DataFormat=DataFormat.TwosComplement)]
        public uint sthitnum
        {
            get
            {
                return this._sthitnum;
            }
            set
            {
                this._sthitnum = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="ucritnum", DataFormat=DataFormat.TwosComplement)]
        public uint ucritnum
        {
            get
            {
                return this._ucritnum;
            }
            set
            {
                this._ucritnum = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(3, IsRequired=false, Name="uhitnum", DataFormat=DataFormat.TwosComplement)]
        public uint uhitnum
        {
            get
            {
                return this._uhitnum;
            }
            set
            {
                this._uhitnum = value;
            }
        }
    }
}

