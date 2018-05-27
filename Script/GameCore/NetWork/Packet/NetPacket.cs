//*************************************************************************
//	创建日期:	2015-6-29
//	文件名称:	NetPacket.cs
//  创 建 人:    Rect 	
//	版权所有:	MIT
//	说    明:	包消息封装类
//*************************************************************************

//-------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GameCore.Network
{
    
    public class NetPacketSocket : INetPacket
    {
        #region Member variables
        private Byte[]  m_Buffer;          // 网络包数括体，包头加包身，包头只描述包身长度，2字节
        #endregion

        #region Construct function
        public NetPacketSocket(int nBodySize)
        {
            m_Buffer = new Byte[SNetPacketCommon.PKG_HEAD_SIZE + nBodySize + 1];
            m_Buffer[SNetPacketCommon.PKG_HEAD_SIZE + nBodySize] = 0;
        }
        #endregion

        #region interface NetPacket
        //-------------------------------------------------------------------------
        /// <summary>
        /// 获取数据包数据缓存 - 包括包头和包身
        /// </summary>
        /// <returns></returns>
        public Byte[] GetBuffer(out int nBufferSize)
        {
            if (null == m_Buffer)
            {
                Debug.Log("SocketNetPacket::GetBuffer error m_Buffer is null");
                nBufferSize = 0;
                return null;
            }

            // -1 是创建整包数据的时候加入的 一个字节 '\0' 
            nBufferSize = m_Buffer.Length - 1;

            return m_Buffer;
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 获取消息包体内容 - 包身
        /// </summary>
        /// <returns></returns>
        public Byte[] GetBody(out int nBodySize)
        {
            if (null == m_Buffer || m_Buffer.Length <= SNetPacketCommon.PKG_HEAD_SIZE)
            {
                Debug.Log("SocketNetPacket::GetBody error  m_buffer == null or  buffer.Length <= PACK_HEAD_SIZE");
                nBodySize = 0;
                return null;
            }

            ///获得包身的数据
            Byte[] data = new Byte[m_Buffer.Length - SNetPacketCommon.PKG_HEAD_SIZE];
            Array.Copy(m_Buffer, SNetPacketCommon.PKG_HEAD_SIZE, data, 0, data.Length);

            // -1 是创建整包数据的时候加入的 一个字节 '\0' 
            nBodySize = m_Buffer.Length - SNetPacketCommon.PKG_HEAD_SIZE - 1;

            return data;
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 设置包头的数据
        /// </summary>
        /// <returns>true or false</returns>
        public bool SetPkgHead(Byte[] headData)
        {
            if (null == headData || headData.Length != SNetPacketCommon.PKG_HEAD_SIZE)
            {
                Debug.Log("SocketNetPacket::SetPkgHead error  headData.Length = " + headData.Length);
                return false;
            }

            if (null == m_Buffer || m_Buffer.Length < SNetPacketCommon.PKG_HEAD_SIZE)
            {
                Debug.Log("SocketNetPacket::SetPkgHead error  m_Buffer.Length = " + m_Buffer.Length);
                return false;
            }

            Array.Copy(headData, 0, m_Buffer, 0, headData.Length);

            return true;
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 设置包身的数据
        /// </summary>
        /// <returns>true or false</returns>
        public bool SetPkgBody(Byte[] bodyData)
        {
            if (null == bodyData)
            {
                return false;
            }
            if (null == m_Buffer || m_Buffer.Length < bodyData.Length + SNetPacketCommon.PKG_HEAD_SIZE)
            {
                return false;
            }

            Array.Copy(bodyData, 0, m_Buffer, SNetPacketCommon.PKG_BODY_OFFSET, bodyData.Length);

            return true;
        }
        //-------------------------------------------------------------------------
        /// <summary>
        /// 获取包大小
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            if (m_Buffer == null)
            {
                return 0;
            }
            else
            {
                return m_Buffer.Length - 1;
            }
        }
        //-------------------------------------------------------------------------
        #endregion
        
    }
}
