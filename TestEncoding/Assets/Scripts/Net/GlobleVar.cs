﻿using UnityEngine;
using System.Collections;

namespace Net
{
    public class GlobalVar
    {
        /**
         *名字的最大长度 
         */
        public const int MAX_NAMESIZE = 32;
        /**
         *MAC地址的最大长度 
         */
        public const int MAX_MAC_ADDR = 24;
        /**
        *UUID最大长度 
        */
        public const int MAX_IPHONE_UUID = 40;
        /**
        *账号最大长度 
        */
        public const int MAX_ACCNAMESIZE = 48;
        /**
        *IP地址最大长度
        */
        public const int MAX_IP_LENGTH = 16;
        /**
        *网关最大容纳用户数目
        */
        public const int MAX_GATEWAYUSER = 4000;
        /**
        *密码最大长度 
        */
        public const int MAX_PASSWORD = 16;
        /// <summary>
        /// 平台字符串长
        /// </summary>
        public const int MAX_FLAT_LENGTH = 100;

        /// <summary>
        /// 聊天最大长度
        /// </summary>
        public const int MAX_CHATINFO = 256;

        /// <summary>
        /// 最大角色数
        /// </summary>
        public const byte MAX_CHARINFO = 2;
    }
}

