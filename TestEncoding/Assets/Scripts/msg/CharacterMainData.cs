namespace msg
{
    using ProtoBuf;
    using System;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="CharacterMainData")]
    public class CharacterMainData : IExtensible
    {
        private AttributeData _attridata;
        private CharacterBaseData _basedata;
        private MapUserData _mapdata;
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(2, IsRequired=false, Name="attridata", DataFormat=DataFormat.Default), DefaultValue((string) null)]
        public AttributeData attridata
        {
            get
            {
                return this._attridata;
            }
            set
            {
                this._attridata = value;
            }
        }

        [ProtoMember(1, IsRequired=false, Name="basedata", DataFormat=DataFormat.Default), DefaultValue((string) null)]
        public CharacterBaseData basedata
        {
            get
            {
                return this._basedata;
            }
            set
            {
                this._basedata = value;
            }
        }

        [ProtoMember(3, IsRequired=false, Name="mapdata", DataFormat=DataFormat.Default), DefaultValue((string) null)]
        public MapUserData mapdata
        {
            get
            {
                return this._mapdata;
            }
            set
            {
                this._mapdata = value;
            }
        }
    }
}

