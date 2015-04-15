namespace map
{
    using ProtoBuf;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    [Serializable, ProtoContract(Name="GameMapFile")]
    public class GameMapFile : IExtensible
    {
        private GameMapFileHeader _fileHeader;
        private readonly List<NewTile> _tiles = new List<NewTile>();
        private IExtension extensionObject;

        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired=false, Name="fileHeader", DataFormat=DataFormat.Default), DefaultValue((string) null)]
        public GameMapFileHeader fileHeader
        {
            get
            {
                return this._fileHeader;
            }
            set
            {
                this._fileHeader = value;
            }
        }

        [ProtoMember(2, Name="tiles", DataFormat=DataFormat.Default)]
        public List<NewTile> tiles
        {
            get
            {
                return this._tiles;
            }
        }
    }
}

