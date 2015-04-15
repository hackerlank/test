namespace msg
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="MapNpcData")]
    public class MapNpcData : IExtensible
    {
        private uint _attspeed;
        private ulong _baseid;
        private uint _dir;
        private uint _hp;
        private string _master_name = string.Empty;
        private uint _maxhp;
        private uint _movespeed;
        private string _name = string.Empty;
        private CharacterMapShow _showdata;
        private readonly List<uint> _state = new List<uint>();
        private ulong _tempid;
        private uint _visit;
        private uint _x;
        private uint _y;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(10, IsRequired=false, Name="attspeed", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
        public uint attspeed
        {
            get
            {
                return this._attspeed;
            }
            set
            {
                this._attspeed = value;
            }
        }

        [DefaultValue((float) 0f), ProtoMember(1, IsRequired=false, Name="baseid", DataFormat=DataFormat.TwosComplement)]
        public ulong baseid
        {
            get
            {
                return this._baseid;
            }
            set
            {
                this._baseid = value;
            }
        }

        [ProtoMember(8, IsRequired=false, Name="dir", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

        [DefaultValue((long) 0L), ProtoMember(4, IsRequired=false, Name="hp", DataFormat=DataFormat.TwosComplement)]
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

        [DefaultValue(""), ProtoMember(11, IsRequired=false, Name="master_name", DataFormat=DataFormat.Default)]
        public string master_name
        {
            get
            {
                return this._master_name;
            }
            set
            {
                this._master_name = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(5, IsRequired=false, Name="maxhp", DataFormat=DataFormat.TwosComplement)]
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

        [DefaultValue((long) 0L), ProtoMember(9, IsRequired=false, Name="movespeed", DataFormat=DataFormat.TwosComplement)]
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

        [DefaultValue(""), ProtoMember(3, IsRequired=false, Name="name", DataFormat=DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [ProtoMember(14, IsRequired=false, Name="showdata", DataFormat=DataFormat.Default), DefaultValue((string) null)]
        public CharacterMapShow showdata
        {
            get
            {
                return this._showdata;
            }
            set
            {
                this._showdata = value;
            }
        }

        [ProtoMember(13, Name="state", DataFormat=DataFormat.TwosComplement)]
        public List<uint> state
        {
            get
            {
                return this._state;
            }
        }

        [ProtoMember(2, IsRequired=false, Name="tempid", DataFormat=DataFormat.TwosComplement), DefaultValue((float) 0f)]
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

        [DefaultValue((long) 0L), ProtoMember(12, IsRequired=false, Name="visit", DataFormat=DataFormat.TwosComplement)]
        public uint visit
        {
            get
            {
                return this._visit;
            }
            set
            {
                this._visit = value;
            }
        }

        [DefaultValue((long) 0L), ProtoMember(6, IsRequired=false, Name="x", DataFormat=DataFormat.TwosComplement)]
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

        [ProtoMember(7, IsRequired=false, Name="y", DataFormat=DataFormat.TwosComplement), DefaultValue((long) 0L)]
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

