//*************************************************************************
//	创建日期:	2015-6-29
//	文件名称:	NetCommon.cs
//  创 建 人:    Rect 	
//	版权所有:	MIT
//	说    明:	
//*************************************************************************

//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCore.Network;


namespace GameCore.Network
{
    //-------------------------------------------------------------------------
    public enum ENUM_SOCKET_STATE //scoket连接状态
    {
        eSocket_Connected,
        eSocket_DisConnected
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 缓存服务器地址（IP、端口号）
    /// </summary>
    public struct SIPAdressMessage
    {
        public string m_IP;                //IP
        public int    m_Port;              //端口号
        public bool   m_IsConnect;         // 是否连接中
        public bool   m_IsNeedCallConnected;
        public bool   m_IsNeedCallDisConnect;
        public bool   m_IsNeedClose;
        

        public void Clear()
        {
            m_IP = "";               //IP
            m_Port = 0;              //端口号
            m_IsConnect = false;     // 是否连接中
            m_IsNeedCallConnected = false;
            m_IsNeedCallDisConnect = false;
            m_IsNeedClose = false;

        }
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 网络连接状态委托类
    /// </summary>
    /// <param name="connect"></param>
    public delegate void NetworkHandleDelegate(INetConnect connect);

    //-------------------------------------------------------------------------
    /// <summary>
    /// 网络公共定义数据
    /// </summary>
    public struct SNetCommon
    {
        public static string NULL = "null";
    }
    //-------------------------------------------------------------------------
    /// <summary>
    /// 消息包公共定义数据
    /// </summary>
    public struct SNetPacketCommon
    {
        public static int PKG_HEAD_OFFSET = 0;         // 包头偏移
        public static int PKG_HEAD_SIZE = 2;           // 包头大小
        public static int PKG_BODY_OFFSET = 2;         // 包身偏移
    }
}
