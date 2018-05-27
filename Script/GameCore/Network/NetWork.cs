//*************************************************************************
//	创建日期:	2015-6-29
//	文件名称:	Network.cs
//  创 建 人:    Rect 	
//	版权所有:	MIT
//	说    明:	网络中心处理
//*************************************************************************

//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCore.Network
{
    public enum NetworkType
    {
        NONE = 0, // 可用于作为 loopback 类型连接
        TCP,
        UDP
    }

    public class NetworkMgr : Singleton<NetworkMgr>
    {
        public CNetworkTCP Tcp { get; private set; }
        public NetworkMgr()
        {
            Tcp = new CNetworkTCP();
        }

        ~NetworkMgr()
        {
            Tcp = null;
        }
    }

    public class CNetworkTCP : INetwork
    {
        #region Member variables
        private Dictionary<int, INetConnect> m_TCPConnects;

        #endregion
        //-------------------------------------------------------------------------
        public CNetworkTCP()
        {
            m_TCPConnects = new Dictionary<int, INetConnect>();

            DisconnectAll();
        }
        //-------------------------------------------------------------------------
        ~CNetworkTCP()
        {
            DisconnectAll();
        }
        //-------------------------------------------------------------------------
        #region public method
        //-------------------------------------------------------------------------
        /// <summary>
        /// 生命周期内每侦更新
        /// </summary>
        public void Update()
        {
            if (0 == m_TCPConnects.Count)
            {
                return;
            }

            var iter = m_TCPConnects.GetEnumerator();
            while (iter.MoveNext())
            {
                INetConnect connect = iter.Current.Value;

                __Update(connect);
            }
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="id"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="listener"></param>
        public void Connect(int id, string host, int port, INetworkMsgHandler listener)
        {
            if (m_TCPConnects.ContainsKey(id))
            {
                Debug.Log("NetTCPWork::Connect already connect the Socket ID = " + id);

                if (m_TCPConnects[id].IsConnect())
                {
                    m_TCPConnects[id].Disconnect();
                }

                m_TCPConnects[id].Connect(id, host, port, listener);
                return;
            }

            CNetConnectTCPSocket connect = new CNetConnectTCPSocket();
            connect.Connect(id, host, port, listener);
            m_TCPConnects.Add(id, connect);
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 重连
        /// </summary>
        public void Reconnect(int id)
        {
            INetConnect c = null;
            if (m_TCPConnects.TryGetValue(id, out c))
            {
                c.Disconnect();
                c.Reconnect();
            }
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="id"></param>
        public void Disconnect(int id)
        {
            INetConnect c = null;

            if (m_TCPConnects.TryGetValue(id, out c))
            {
                if (null != c)
                {
                    Debug.Log("CNetTCPWork::Disconnect Remove ID = " + id);
                    c.Disconnect();
                    m_TCPConnects[id] = null;
                }

                m_TCPConnects.Remove(id);
            }
            
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 关闭所有连接
        /// </summary>
        public void DisconnectAll()
        {
            foreach (KeyValuePair<int, INetConnect> p in m_TCPConnects)
            {
                if (null != p.Value)
                {
                    p.Value.Disconnect();
                }

            }
            m_TCPConnects.Clear();
            __Clear();
        }
        //-------------------------------------------------------------------------
        public bool SendMessage(int id, Byte[] data)
        {
            if (null == data)
            {
                return false;
            }

            INetConnect c = null;
            if (m_TCPConnects.TryGetValue(id, out c))
            {
                if (null != c || c.IsConnect())
                {
                    c.SendMessage(data);
                    return true;
                }
            }

            Debug.Log("CNetWork::SendMessage false id = " + id);
            return false;
        }
        //-------------------------------------------------------------------------
        public string ToNetWorkString(int id)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("have no connect!");
            uint unSendData = 0;
            uint unRecvData = 0;

            INetConnect c = null;
            if (m_TCPConnects.TryGetValue(id, out c))
            {
                if (null != c )
                {
                    strBuilder.Remove(0, strBuilder.Length);
                    unSendData = c.GetSendTotalBytes();
                    unRecvData = c.GetRecvTotalBytes();

                    if (c.IsConnect())
                    {
                        strBuilder.Append("Connect:" + c.GetIP() + ":" + c.GetPort());
                        
                    }
                    else
                    {
                        strBuilder.Append("Connect Failed ");
                    }
                    strBuilder.Append(" - ");
                    strBuilder.Append(unSendData);
                    strBuilder.Append("/");
                    strBuilder.Append(unRecvData);

                }
            }
            return strBuilder.ToString();
        }
        //-------------------------------------------------------------------------
        public bool IsConnect(int id)
        {
            INetConnect c = null;
            if (m_TCPConnects.TryGetValue(id, out c))
            {
                return c.IsConnect();
            }

            return false;
        }
        //-------------------------------------------------------------------------
        #endregion

        #region private method
        //-------------------------------------------------------------------------
        private void __Clear()
        {

        }
        //-------------------------------------------------------------------------
        private void __Update(INetConnect connect)
        {
            if (null == connect)
            {
                return;
            }

            // 连接更新
            ENUM_SOCKET_STATE sState = connect.Update();
        }
        #endregion
    }
}
