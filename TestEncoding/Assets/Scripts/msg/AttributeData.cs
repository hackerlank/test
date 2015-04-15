namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="AttributeData")]
    public class AttributeData : IExtensible
    {
        private uint _attackspeed;
        private uint _crit;
        private uint _hit;
        private uint _matt;
        private uint _mdef;
        private uint _parry;
        private uint _patt;
        private uint _pdef;
        private uint _power;
        private uint _toughness;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue((long) 0L), ProtoMember(11, IsRequired=false, Name="attackspeed", DataFormat=DataFormat.TwosComplement)]
        public uint attackspeed
        {
            get
            {
                return this._attackspeed;
            }
            set
            {
                this._attackspeed = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(10, IsRequired=false, Name="crit", DataFormat=DataFormat.TwosComplement)]
        public uint crit
        {
            get
            {
                return this._crit;
            }
            set
            {
                this._crit = value;
            }
        }

        [ProtoMember(8, IsRequired=false, Name="hit", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint hit
        {
            get
            {
                return this._hit;
            }
            set
            {
                this._hit = value;
            }
        }

        [ProtoMember(3, IsRequired=false, Name="matt", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint matt
        {
            get
            {
                return this._matt;
            }
            set
            {
                this._matt = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="mdef", DataFormat=DataFormat.TwosComplement)]
        public uint mdef
        {
            get
            {
                return this._mdef;
            }
            set
            {
                this._mdef = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(7, IsRequired=false, Name="parry", DataFormat=DataFormat.TwosComplement)]
        public uint parry
        {
            get
            {
                return this._parry;
            }
            set
            {
                this._parry = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="patt", DataFormat=DataFormat.TwosComplement)]
        public uint patt
        {
            get
            {
                return this._patt;
            }
            set
            {
                this._patt = value;
            }
        }

        [ProtoMember(6, IsRequired=false, Name="pdef", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint pdef
        {
            get
            {
                return this._pdef;
            }
            set
            {
                this._pdef = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="power", DataFormat=DataFormat.TwosComplement)]
        public uint power
        {
            get
            {
                return this._power;
            }
            set
            {
                this._power = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(9, IsRequired=false, Name="toughness", DataFormat=DataFormat.TwosComplement)]
        public uint toughness
        {
            get
            {
                return this._toughness;
            }
            set
            {
                this._toughness = value;
            }
        }
    }
}

