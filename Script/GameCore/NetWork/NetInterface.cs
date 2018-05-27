//*************************************************************************
//	创建日期:	2015-6-29
//	文件名称:	NetInterface.cs
//  创 建 人:    Rect 	
//	版权所有:	MIT
//	说    明:	网络连接器接口
//*************************************************************************

//-------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace GameCore.Network
{
    /// <summary>
    /// 网络状态事件基类
    /// </summary>
    public abstract class INetworkMsgHandler//abstract 
    {
        public NetworkHandleDelegate del_OnConnectStart;      //socket开始
        public NetworkHandleDelegate del_OnConnectSuccess;    //socket连接成功的回调函数
        public NetworkHandleDelegate del_OnDisconnect;        //socket意外断开(包括服务端断开命令)的回调函数
        public NetworkHandleDelegate del_Update;              //socket处于连接中的回调函数(暂时没用到)

        public INetworkMsgHandler()
        {
            del_OnConnectStart = new NetworkHandleDelegate(OnConnectStart);
            del_OnConnectSuccess = new NetworkHandleDelegate(OnConnectSuccess);
            del_OnDisconnect = new NetworkHandleDelegate(OnDisConnect);
            del_Update = new NetworkHandleDelegate(OnUpdate);
        }

        protected virtual void OnConnectStart(INetConnect connect) { }
        protected virtual void OnConnectSuccess(INetConnect connect) { }
        protected virtual void OnDisConnect(INetConnect connect) { }
        protected virtual void OnUpdate(INetConnect connect) { }
    }

    /// <summary>
    /// 连接器接口
    /// </summary>
    public interface  INetConnect
    {
        /// <summary>
        /// 判断连接是否成功
        /// </summary>
        /// <returns></returns>
        bool IsConnect();

        /// <summary>
        /// 关闭连接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 断线重连
        /// </summary>
        /// <returns></returns>
        bool Reconnect();

        /// <summary>
        /// 连接开始
        /// </summary>
        /// <param name="ip">服务器IP地址</param>
        /// <param name="portnumber">端口信息</param>
        /// <returns></returns>
        bool Connect(int iConnectID, string ip, int portnumber, INetworkMsgHandler listener);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool SendMessage(Byte[] data);

        /// <summary>
        /// 取出消息接收缓存中所有的消息，并清空
        /// </summary>
        /// <param name="pkgList"></param>
        void ReceiveAllPkg(List<NetPacketSocket> pkgList);

        /// <summary>
        /// 连接器更新
        /// </summary>
        ENUM_SOCKET_STATE Update();

        /// <summary>
        /// 获取连接类型 tcp or udp or other
        /// </summary>
        /// <returns></returns>
        ProtocolType GetConnectType();

        /// <summary>
        /// 发送字节数总数
        /// </summary>
        /// <returns></returns>
        uint GetSendTotalBytes();

        /// <summary>
        /// 接收字节数总数
        /// </summary>
        /// <returns></returns>
        uint GetRecvTotalBytes();

        /// <summary>
        /// 获取端口
        /// </summary>
        /// <returns></returns>
        int GetPort();

        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        string GetIP();

        /// <summary>
        /// 获取连接ID
        /// </summary>
        /// <returns></returns>
        int GetConnectID();

        
    }

    /// <summary>
    /// 消息包接口
    /// </summary>
    public interface INetPacket
    {
        /// <summary>
        /// 获取消息包数据缓存 - 包括包头和包身
        /// </summary>
        /// <returns></returns>
        Byte[] GetBuffer(out int nBufferSize);

        /// <summary>
        /// 获取消息包体内容 - 包身
        /// </summary>
        /// <returns></returns>
        Byte[] GetBody(out int nBodySize);

        /// <summary>
        /// 设置包头的数据
        /// </summary>
        /// <returns>true or false</returns>
        bool SetPkgHead(Byte[] headData);

        /// <summary>
        /// 设置包身的数据
        /// </summary>
        /// <returns>true or false</returns>
        bool SetPkgBody(Byte[] bodyData);

        /// <summary>
        /// 获取包大小
        /// </summary>
        /// <returns></returns>
        int GetSize();
    }

    /// <summary>
    /// 网络管理器接口
    /// </summary>
    public interface INetwork
    {
        void Update();
        void Connect(int id, string host, int port, INetworkMsgHandler listener);
        void Reconnect(int id);
        bool SendMessage(int id, Byte[] data);
        void Disconnect(int id);
        void DisconnectAll();
        string ToNetWorkString(int id);
        bool IsConnect(int id);
    }

}
