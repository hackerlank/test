namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="CharacterMapData")]
    public class CharacterMapData : IExtensible
    {
        private uint _dir;
        private uint _hp;
        private uint _level;
        private uint _maxhp;
        private uint _movespeed;
        private readonly List<uint> _state = new List<uint>();
        private uint _x;
        private uint _y;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(4, IsRequired=false, Name="dir", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint dir
        {
            get
            {
                return this._dir;
            }
            set
            {
                this._dir = value;
            }
        }

        [ProtoMember(6, IsRequired=false, Name="hp", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint hp
        {
            get
            {
                return this._hp;
            }
            set
            {
                this._hp = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(1, IsRequired=false, Name="level", DataFormat=DataFormat.TwosComplement)]
        public uint level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        [ProtoMember(7, IsRequired=false, Name="maxhp", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint maxhp
        {
            get
            {
                return this._maxhp;
            }
            set
            {
                this._maxhp = value;
            }
        }

        [ProtoMember(5, IsRequired=false, Name="movespeed", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint movespeed
        {
            get
            {
                return this._movespeed;
            }
            set
            {
                this._movespeed = value;
            }
        }

        [ProtoMember(8, Name="state", DataFormat=DataFormat.TwosComplement)]
        public List<uint> state
        {
            get
            {
                return this._state;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(2, IsRequired=false, Name="x", DataFormat=DataFormat.TwosComplement)]
        public uint x
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(3, IsRequired=false, Name="y", DataFormat=DataFormat.TwosComplement)]
        public uint y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }
    }
}

