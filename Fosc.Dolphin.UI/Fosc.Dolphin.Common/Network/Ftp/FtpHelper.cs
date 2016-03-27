/*------------------------------------------------
<copyright file="FtpHelper.cs" company="RRMall">
Copyright (c) RRMall.All Rights Reserved.
</copyright>
CLRVersion:4.0.30319.42000
NameSpace:Fosc.Dolphin.Common.Network.Ftp 
Author:Administrator
Email:jiangxiaoqiang@renrenmall.com
CreateDate:2016/3/18 17:13:33
Stamp:7f47624b-6835-48ac-a759-a02c18fe48d6
UserDomain:DOLPHIN

---------------------------
Modifier:
ModifyDate:
ModifyDescription:
-----------------------------------------------*/

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Fosc.Dolphin.Common.Network.Ftp
{
    /// <summary>
    ///     Write the class summary.
    /// </summary>
    public class FtpHelper
    {
        #region 变量声明

        /// <summary>
        ///     服务器连接地址
        /// </summary>
        public string Server;

        /// <summary>
        ///     登陆帐号
        /// </summary>
        public string User;

        /// <summary>
        ///     登陆口令
        /// </summary>
        public string Pass;

        /// <summary>
        ///     端口号
        /// </summary>
        public int Port;

        /// <summary>
        ///     无响应时间（FTP在指定时间内无响应）
        /// </summary>
        public int Timeout;

        /// <summary>
        ///     服务器错误状态信息
        /// </summary>
        public string Errormessage;


        /// <summary>
        ///     服务器状态返回信息
        /// </summary>
        private string _messages;

        /// <summary>
        ///     服务器的响应信息
        /// </summary>
        private string _responseStr;

        /// <summary>
        ///     链接模式（主动或被动，默认为被动）
        /// </summary>
        private bool _passiveMode;

        /// <summary>
        ///     上传或下载信息字节数
        /// </summary>
        private long _bytesTotal;

        /// <summary>
        ///     上传或下载的文件大小
        /// </summary>
        private long _fileSize;

        /// <summary>
        ///     主套接字
        /// </summary>
        private Socket _mainSock;

        /// <summary>
        ///     要链接的网络地址终结点
        /// </summary>
        private IPEndPoint _mainIpEndPoint;

        /// <summary>
        ///     侦听套接字
        /// </summary>
        private Socket _listeningSock;

        /// <summary>
        ///     数据套接字
        /// </summary>
        private Socket _dataSock;

        /// <summary>
        ///     要链接的网络数据地址终结点
        /// </summary>
        private IPEndPoint _dataIpEndPoint;

        /// <summary>
        ///     用于上传或下载的文件流对象
        /// </summary>
        private FileStream _file;

        /// <summary>
        ///     与FTP服务器交互的状态值
        /// </summary>
        private int _response;

        /// <summary>
        ///     读取并保存当前命令执行后从FTP服务器端返回的数据信息
        /// </summary>
        private string _bucket;

        #endregion

        #region 构造函数

        /// <summary>
        ///     构造函数
        /// </summary>
        public FtpHelper()
        {
            Server = null;
            User = null;
            Pass = null;
            Port = 21;
            _passiveMode = true;
            _mainSock = null;
            _mainIpEndPoint = null;
            _listeningSock = null;
            _dataSock = null;
            _dataIpEndPoint = null;
            _file = null;
            _bucket = "";
            _bytesTotal = 0;
            Timeout = 10000; //无响应时间为10秒
            _messages = "";
            Errormessage = "";
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="server">服务器IP或名称</param>
        /// <param name="user">登陆帐号</param>
        /// <param name="pass">登陆口令</param>
        public FtpHelper(string server, string user, string pass)
        {
            Server = server;
            User = user;
            Pass = pass;
            Port = 21;
            _passiveMode = true;
            _mainSock = null;
            _mainIpEndPoint = null;
            _listeningSock = null;
            _dataSock = null;
            _dataIpEndPoint = null;
            _file = null;
            _bucket = "";
            _bytesTotal = 0;
            Timeout = 10000; //无响应时间为10秒
            _messages = "";
            Errormessage = "";
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="server">服务器IP或名称</param>
        /// <param name="port">端口号</param>
        /// <param name="user">登陆帐号</param>
        /// <param name="pass">登陆口令</param>
        public FtpHelper(string server, int port, string user, string pass)
        {
            Server = server;
            User = user;
            Pass = pass;
            Port = port;
            _passiveMode = true;
            _mainSock = null;
            _mainIpEndPoint = null;
            _listeningSock = null;
            _dataSock = null;
            _dataIpEndPoint = null;
            _file = null;
            _bucket = "";
            _bytesTotal = 0;
            Timeout = 10000; //无响应时间为10秒
            _messages = "";
            Errormessage = "";
        }


        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="server">服务器IP或名称</param>
        /// <param name="port">端口号</param>
        /// <param name="user">登陆帐号</param>
        /// <param name="pass">登陆口令</param>
        /// <param name="mode">链接方式</param>
        public FtpHelper(string server, int port, string user, string pass, int mode)
        {
            Server = server;
            User = user;
            Pass = pass;
            Port = port;
            _passiveMode = mode <= 1 ? true : false;
            _mainSock = null;
            _mainIpEndPoint = null;
            _listeningSock = null;
            _dataSock = null;
            _dataIpEndPoint = null;
            _file = null;
            _bucket = "";
            _bytesTotal = 0;
            Timeout = 10000; //无响应时间为10秒
            _messages = "";
            Errormessage = "";
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="server">服务器IP或名称</param>
        /// <param name="port">端口号</param>
        /// <param name="user">登陆帐号</param>
        /// <param name="pass">登陆口令</param>
        /// <param name="mode">链接方式</param>
        /// <param name="timeout">无响应时间(限时),单位:秒 (小于或等于0为不受时间限制)</param>
        public FtpHelper(string server, int port, string user, string pass, int mode, int timeout_sec)
        {
            Server = server;
            User = user;
            Pass = pass;
            Port = port;
            _passiveMode = mode <= 1 ? true : false;
            _mainSock = null;
            _mainIpEndPoint = null;
            _listeningSock = null;
            _dataSock = null;
            _dataIpEndPoint = null;
            _file = null;
            _bucket = "";
            _bytesTotal = 0;
            Timeout = (timeout_sec <= 0) ? int.MaxValue : (timeout_sec*1000); //无响应时间
            _messages = "";
            Errormessage = "";
        }

        #endregion

        #region 属性

        /// <summary>
        ///     当前是否已连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (_mainSock != null)
                    return _mainSock.Connected;
                return false;
            }
        }

        /// <summary>
        ///     当message缓冲区有数据则返回
        /// </summary>
        public bool MessagesAvailable
        {
            get
            {
                if (_messages.Length > 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        ///     获取服务器状态返回信息, 并清空messages变量
        /// </summary>
        public string Messages
        {
            get
            {
                var tmp = _messages;
                _messages = "";
                return tmp;
            }
        }

        /// <summary>
        ///     最新指令发出后服务器的响应
        /// </summary>
        public string ResponseString
        {
            get { return _responseStr; }
        }


        /// <summary>
        ///     在一次传输中,发送或接收的字节数
        /// </summary>
        public long BytesTotal
        {
            get { return _bytesTotal; }
        }

        /// <summary>
        ///     被下载或上传的文件大小,当文件大小无效时为0
        /// </summary>
        public long FileSize
        {
            get { return _fileSize; }
        }

        /// <summary>
        ///     链接模式:
        ///     true 被动模式 [默认]
        ///     false: 主动模式
        /// </summary>
        public bool PassiveMode
        {
            get { return _passiveMode; }
            set { _passiveMode = value; }
        }

        #endregion

        #region 操作

        /// <summary>
        ///     操作失败
        /// </summary>
        private void Fail()
        {
            Disconnect();
            Errormessage += _responseStr;
            //throw new Exception(responseStr);
        }

        /// <summary>
        ///     下载文件类型
        /// </summary>
        /// <param name="mode">true:二进制文件 false:字符文件</param>
        private void SetBinaryMode(bool mode)
        {
            if (mode)
                SendCommand("TYPE I");
            else
                SendCommand("TYPE A");

            ReadResponse();
            if (_response != 200)
                Fail();
        }

        /// <summary>
        ///     发送命令
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(string command)
        {
            var cmd = Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());

            if (command.Length > 3 && command.Substring(0, 4) == "PASS")
            {
                _messages = "\rPASS xxx";
            }
            else
            {
                _messages = "\r" + command;
            }

            try
            {
                _mainSock.Send(cmd, cmd.Length, 0);
            }
            catch (Exception ex)
            {
                try
                {
                    Disconnect();
                    Errormessage += ex.Message;
                }
                catch
                {
                    _mainSock.Close();
                    _file.Close();
                    _mainSock = null;
                    _mainIpEndPoint = null;
                    _file = null;
                }
            }
        }


        private void FillBucket()
        {
            var bytes = new byte[512];
            var msecsPassed = 0;
            while (_mainSock.Available < 1)
            {
                Thread.Sleep(50);
                msecsPassed += 50;
                //当等待时间到,则断开链接
                if (msecsPassed > Timeout)
                {
                    Disconnect();
                    Errormessage += "Timed out waiting on server to respond.";
                    return;
                }
            }

            while (_mainSock.Available > 0)
            {
                long bytesgot = _mainSock.Receive(bytes, 512, 0);
                _bucket += Encoding.ASCII.GetString(bytes, 0, (int) bytesgot);
                Thread.Sleep(50);
            }
        }


        private string GetLineFromBucket()
        {
            int i;
            var buf = "";

            if ((i = _bucket.IndexOf('\n')) < 0)
            {
                while (i < 0)
                {
                    FillBucket();
                    i = _bucket.IndexOf('\n');
                }
            }

            buf = _bucket.Substring(0, i);
            _bucket = _bucket.Substring(i + 1);

            return buf;
        }


        /// <summary>
        ///     返回服务器端返回信息
        /// </summary>
        private void ReadResponse()
        {
            _messages = "";
            while (true)
            {
                var buf = GetLineFromBucket();

                if (Regex.Match(buf, "^[0-9]+ ").Success)
                {
                    _responseStr = buf;
                    _response = int.Parse(buf.Substring(0, 3));
                    break;
                }
                _messages += Regex.Replace(buf, "^[0-9]+-", "") + "\n";
            }
        }


        /// <summary>
        ///     打开数据套接字
        /// </summary>
        private void OpenDataSocket()
        {
            if (_passiveMode)
            {
                string[] pasv;
                Connect();
                SendCommand("PASV");
                ReadResponse();
                if (_response != 227)
                    Fail();

                try
                {
                    int i1, i2;

                    i1 = _responseStr.IndexOf('(') + 1;
                    i2 = _responseStr.IndexOf(')') - i1;
                    pasv = _responseStr.Substring(i1, i2).Split(',');
                }
                catch (Exception)
                {
                    Disconnect();
                    Errormessage += "Malformed PASV response: " + _responseStr;
                    return;
                }

                if (pasv.Length < 6)
                {
                    Disconnect();
                    Errormessage += "Malformed PASV response: " + _responseStr;
                    return;
                }

                var server = string.Format("{0}.{1}.{2}.{3}", pasv[0], pasv[1], pasv[2], pasv[3]);
                var port = (int.Parse(pasv[4]) << 8) + int.Parse(pasv[5]);

                try
                {
                    CloseDataSocket();

                    _dataSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

#if NET1
                    data_ipEndPoint = new IPEndPoint(Dns.GetHostByName(server).AddressList[0], port);
#else
                    _dataIpEndPoint = new IPEndPoint(Dns.GetHostEntry(server).AddressList[0], port);
#endif

                    _dataSock.Connect(_dataIpEndPoint);
                }
                catch (Exception ex)
                {
                    Errormessage += "Failed to connect for data transfer: " + ex.Message;
                }
            }
            else
            {
                Connect();

                try
                {
                    CloseDataSocket();

                    _listeningSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    // 对于端口,则发送IP地址.下面则提取相应信息
                    var sLocAddr = _mainSock.LocalEndPoint.ToString();
                    var ix = sLocAddr.IndexOf(':');
                    if (ix < 0)
                    {
                        Errormessage += "Failed to parse the local address: " + sLocAddr;
                        return;
                    }
                    var sIPAddr = sLocAddr.Substring(0, ix);
                    // 系统自动绑定一个端口号(设置 port = 0)
                    var localEP = new IPEndPoint(IPAddress.Parse(sIPAddr), 0);

                    _listeningSock.Bind(localEP);
                    sLocAddr = _listeningSock.LocalEndPoint.ToString();
                    ix = sLocAddr.IndexOf(':');
                    if (ix < 0)
                    {
                        Errormessage += "Failed to parse the local address: " + sLocAddr;
                    }
                    var nPort = int.Parse(sLocAddr.Substring(ix + 1));

                    // 开始侦听链接请求
                    _listeningSock.Listen(1);
                    var sPortCmd = string.Format("PORT {0},{1},{2}",
                        sIPAddr.Replace('.', ','),
                        nPort/256, nPort%256);
                    SendCommand(sPortCmd);
                    ReadResponse();
                    if (_response != 200)
                        Fail();
                }
                catch (Exception ex)
                {
                    Errormessage += "Failed to connect for data transfer: " + ex.Message;
                }
            }
        }


        private void ConnectDataSocket()
        {
            if (_dataSock != null) // 已链接
                return;

            try
            {
                _dataSock = _listeningSock.Accept(); // Accept is blocking
                _listeningSock.Close();
                _listeningSock = null;

                if (_dataSock == null)
                {
                    throw new Exception("Winsock error: " +
                                        Convert.ToString(Marshal.GetLastWin32Error()));
                }
            }
            catch (Exception ex)
            {
                Errormessage += "Failed to connect for data transfer: " + ex.Message;
            }
        }


        private void CloseDataSocket()
        {
            if (_dataSock != null)
            {
                if (_dataSock.Connected)
                {
                    _dataSock.Close();
                }
                _dataSock = null;
            }

            _dataIpEndPoint = null;
        }

        /// <summary>
        ///     关闭所有链接
        /// </summary>
        public void Disconnect()
        {
            CloseDataSocket();

            if (_mainSock != null)
            {
                if (_mainSock.Connected)
                {
                    SendCommand("QUIT");
                    _mainSock.Close();
                }
                _mainSock = null;
            }

            if (_file != null)
                _file.Close();

            _mainIpEndPoint = null;
            _file = null;
        }

        /// <summary>
        ///     链接到FTP服务器
        /// </summary>
        /// <param name="server">要链接的IP地址或主机名</param>
        /// <param name="port">端口号</param>
        /// <param name="user">登陆帐号</param>
        /// <param name="pass">登陆口令</param>
        public void Connect(string server, int port, string user, string pass)
        {
            Server = server;
            User = user;
            Pass = pass;
            Port = port;

            Connect();
        }

        /// <summary>
        ///     链接到FTP服务器
        /// </summary>
        /// <param name="server">要链接的IP地址或主机名</param>
        /// <param name="user">登陆帐号</param>
        /// <param name="pass">登陆口令</param>
        public void Connect(string server, string user, string pass)
        {
            Server = server;
            User = user;
            Pass = pass;

            Connect();
        }

        /// <summary>
        ///     链接到FTP服务器
        /// </summary>
        public bool Connect()
        {
            if (Server == null)
            {
                Errormessage += "No server has been set.\r\n";
            }
            if (User == null)
            {
                Errormessage += "No server has been set.\r\n";
            }

            if (_mainSock != null)
                if (_mainSock.Connected)
                    return true;

            try
            {
                _mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
#if NET1
                main_ipEndPoint = new IPEndPoint(Dns.GetHostByName(server).AddressList[0], port);
#else
                _mainIpEndPoint = new IPEndPoint(Dns.GetHostEntry(Server).AddressList[0], Port);
#endif

                _mainSock.Connect(_mainIpEndPoint);
            }
            catch (Exception ex)
            {
                Errormessage += ex.Message;
                return false;
            }

            ReadResponse();
            if (_response != 220)
                Fail();

            SendCommand("USER " + User);
            ReadResponse();

            switch (_response)
            {
                case 331:
                    if (Pass == null)
                    {
                        Disconnect();
                        Errormessage += "No password has been set.";
                        return false;
                    }
                    SendCommand("PASS " + Pass);
                    ReadResponse();
                    if (_response != 230)
                    {
                        Fail();
                        return false;
                    }
                    break;
                case 230:
                    break;
            }

            return true;
        }

        /// <summary>
        ///     获取FTP当前(工作)目录下的文件列表
        /// </summary>
        /// <returns>返回文件列表数组</returns>
        public ArrayList List()
        {
            var bytes = new byte[512];
            var file_list = "";
            long bytesgot = 0;
            var msecs_passed = 0;
            var list = new ArrayList();

            Connect();
            OpenDataSocket();
            SendCommand("LIST");
            ReadResponse();

            switch (_response)
            {
                case 125:
                case 150:
                    break;
                default:
                    CloseDataSocket();
                    throw new Exception(_responseStr);
            }
            ConnectDataSocket();

            while (_dataSock.Available < 1)
            {
                Thread.Sleep(50);
                msecs_passed += 50;

                if (msecs_passed > (Timeout/10))
                {
                    break;
                }
            }

            while (_dataSock.Available > 0)
            {
                bytesgot = _dataSock.Receive(bytes, bytes.Length, 0);
                file_list += Encoding.ASCII.GetString(bytes, 0, (int) bytesgot);
                Thread.Sleep(50);
            }

            CloseDataSocket();

            ReadResponse();
            if (_response != 226)
                throw new Exception(_responseStr);

            foreach (var f in file_list.Split('\n'))
            {
                if (f.Length > 0 && !Regex.Match(f, "^total").Success)
                    list.Add(f.Substring(0, f.Length - 1));
            }

            return list;
        }

        /// <summary>
        ///     获取到文件名列表
        /// </summary>
        /// <returns>返回文件名列表</returns>
        public ArrayList ListFiles()
        {
            var list = new ArrayList();

            foreach (string f in List())
            {
                if ((f.Length > 0))
                {
                    if ((f[0] != 'd') && (f.ToUpper().IndexOf("<DIR>") < 0))
                        list.Add(f);
                }
            }

            return list;
        }

        /// <summary>
        ///     获取路径列表
        /// </summary>
        /// <returns>返回路径列表</returns>
        public ArrayList ListDirectories()
        {
            var list = new ArrayList();

            foreach (string f in List())
            {
                if (f.Length > 0)
                {
                    if ((f[0] == 'd') || (f.ToUpper().IndexOf("<DIR>") >= 0))
                        list.Add(f);
                }
            }

            return list;
        }

        /// <summary>
        ///     获取原始数据信息.
        /// </summary>
        /// <param name="fileName">远程文件名</param>
        /// <returns>返回原始数据信息.</returns>
        public string GetFileDateRaw(string fileName)
        {
            Connect();

            SendCommand("MDTM " + fileName);
            ReadResponse();
            if (_response != 213)
            {
                Errormessage += _responseStr;
                return "";
            }

            return (_responseStr.Substring(4));
        }

        /// <summary>
        ///     得到文件日期.
        /// </summary>
        /// <param name="fileName">远程文件名</param>
        /// <returns>返回远程文件日期</returns>
        public DateTime GetFileDate(string fileName)
        {
            return ConvertFTPDateToDateTime(GetFileDateRaw(fileName));
        }

        private DateTime ConvertFTPDateToDateTime(string input)
        {
            if (input.Length < 14)
                throw new ArgumentException("Input Value for ConvertFTPDateToDateTime method was too short.");

            //YYYYMMDDhhmmss":
            int year = Convert.ToInt16(input.Substring(0, 4));
            int month = Convert.ToInt16(input.Substring(4, 2));
            int day = Convert.ToInt16(input.Substring(6, 2));
            int hour = Convert.ToInt16(input.Substring(8, 2));
            int min = Convert.ToInt16(input.Substring(10, 2));
            int sec = Convert.ToInt16(input.Substring(12, 2));

            return new DateTime(year, month, day, hour, min, sec);
        }

        /// <summary>
        ///     获取FTP上的当前(工作)路径
        /// </summary>
        /// <returns>返回FTP上的当前(工作)路径</returns>
        public string GetWorkingDirectory()
        {
            //PWD - 显示工作路径
            Connect();
            SendCommand("PWD");
            ReadResponse();

            if (_response != 257)
            {
                Errormessage += _responseStr;
            }

            string pwd;
            try
            {
                pwd = _responseStr.Substring(_responseStr.IndexOf("\"", 0) + 1); //5);
                pwd = pwd.Substring(0, pwd.LastIndexOf("\""));
                pwd = pwd.Replace("\"\"", "\""); // 替换带引号的路径信息符号
            }
            catch (Exception ex)
            {
                Errormessage += ex.Message;
                return null;
            }

            return pwd;
        }


        /// <summary>
        ///     跳转服务器上的当前(工作)路径
        /// </summary>
        /// <param name="path">要跳转的路径</param>
        public bool ChangeDir(string path)
        {
            Connect();
            SendCommand("CWD " + path);
            ReadResponse();
            if (_response != 250)
            {
                Errormessage += _responseStr;
                return false;
            }
            return true;
        }

        /// <summary>
        ///     创建指定的目录
        /// </summary>
        /// <param name="dir">要创建的目录</param>
        public void MakeDir(string dir)
        {
            Connect();
            SendCommand("MKD " + dir);
            ReadResponse();

            switch (_response)
            {
                case 257:
                case 250:
                    break;
                default:
                {
                    Errormessage += _responseStr;
                    break;
                }
            }
        }

        /// <summary>
        ///     移除FTP上的指定目录
        /// </summary>
        /// <param name="dir">要移除的目录</param>
        public void RemoveDir(string dir)
        {
            Connect();
            SendCommand("RMD " + dir);
            ReadResponse();
            if (_response != 250)
            {
                Errormessage += _responseStr;
                ;
            }
        }

        /// <summary>
        ///     移除FTP上的指定文件
        /// </summary>
        /// <param name="filename">要移除的文件名称</param>
        public void RemoveFile(string filename)
        {
            Connect();
            SendCommand("DELE " + filename);
            ReadResponse();
            if (_response != 250)
            {
                Errormessage += _responseStr;
            }
        }

        /// <summary>
        ///     重命名FTP上的文件
        /// </summary>
        /// <param name="oldfilename">原文件名</param>
        /// <param name="newfilename">新文件名</param>
        public void RenameFile(string oldfilename, string newfilename)
        {
            Connect();
            SendCommand("RNFR " + oldfilename);
            ReadResponse();
            if (_response != 350)
            {
                Errormessage += _responseStr;
            }
            else
            {
                SendCommand("RNTO " + newfilename);
                ReadResponse();
                if (_response != 250)
                {
                    Errormessage += _responseStr;
                }
            }
        }

        /// <summary>
        ///     获得指定文件的大小(如果FTP支持)
        /// </summary>
        /// <param name="filename">指定的文件</param>
        /// <returns>返回指定文件的大小</returns>
        public long GetFileSize(string filename)
        {
            Connect();
            SendCommand("SIZE " + filename);
            ReadResponse();
            if (_response != 213)
            {
                Errormessage += _responseStr;
            }

            return long.Parse(_responseStr.Substring(4));
        }

        /// <summary>
        ///     上传指定的文件
        /// </summary>
        /// <param name="filename">要上传的文件</param>
        public bool OpenUpload(string filename)
        {
            return OpenUpload(filename, filename, false);
        }

        /// <summary>
        ///     上传指定的文件
        /// </summary>
        /// <param name="filename">本地文件名</param>
        /// <param name="remotefilename">远程要覆盖的文件名</param>
        public bool OpenUpload(string filename, string remotefilename)
        {
            return OpenUpload(filename, remotefilename, false);
        }

        /// <summary>
        ///     上传指定的文件
        /// </summary>
        /// <param name="filename">本地文件名</param>
        /// <param name="resume">如果存在,则尝试恢复</param>
        public bool OpenUpload(string filename, bool resume)
        {
            return OpenUpload(filename, filename, resume);
        }

        /// <summary>
        ///     上传指定的文件
        /// </summary>
        /// <param name="filename">本地文件名</param>
        /// <param name="remote_filename">远程要覆盖的文件名</param>
        /// <param name="resume">如果存在,则尝试恢复</param>
        public bool OpenUpload(string filename, string remote_filename, bool resume)
        {
            Connect();
            SetBinaryMode(true);
            OpenDataSocket();

            _bytesTotal = 0;

            try
            {
                _file = new FileStream(filename, FileMode.Open);
            }
            catch (Exception ex)
            {
                _file = null;
                Errormessage += ex.Message;
                return false;
            }

            _fileSize = _file.Length;

            if (resume)
            {
                var size = GetFileSize(remote_filename);
                SendCommand("REST " + size);
                ReadResponse();
                if (_response == 350)
                    _file.Seek(size, SeekOrigin.Begin);
            }

            SendCommand("STOR " + remote_filename);
            ReadResponse();

            switch (_response)
            {
                case 125:
                case 150:
                    break;
                default:
                    _file.Close();
                    _file = null;
                    Errormessage += _responseStr;
                    return false;
            }
            ConnectDataSocket();

            return true;
        }

        /// <summary>
        ///     下载指定文件
        /// </summary>
        /// <param name="filename">远程文件名称</param>
        public void OpenDownload(string filename)
        {
            OpenDownload(filename, filename, false);
        }

        /// <summary>
        ///     下载并恢复指定文件
        /// </summary>
        /// <param name="filename">远程文件名称</param>
        /// <param name="resume">如文件存在,则尝试恢复</param>
        public void OpenDownload(string filename, bool resume)
        {
            OpenDownload(filename, filename, resume);
        }

        /// <summary>
        ///     下载指定文件
        /// </summary>
        /// <param name="filename">远程文件名称</param>
        /// <param name="localfilename">本地文件名</param>
        public void OpenDownload(string remote_filename, string localfilename)
        {
            OpenDownload(remote_filename, localfilename, false);
        }

        /// <summary>
        ///     打开并下载文件
        /// </summary>
        /// <param name="remote_filename">远程文件名称</param>
        /// <param name="local_filename">本地文件名</param>
        /// <param name="resume">如果文件存在则恢复</param>
        public void OpenDownload(string remote_filename, string local_filename, bool resume)
        {
            Connect();
            SetBinaryMode(true);

            _bytesTotal = 0;

            try
            {
                _fileSize = GetFileSize(remote_filename);
            }
            catch
            {
                _fileSize = 0;
            }

            if (resume && File.Exists(local_filename))
            {
                try
                {
                    _file = new FileStream(local_filename, FileMode.Open);
                }
                catch (Exception ex)
                {
                    _file = null;
                    throw new Exception(ex.Message);
                }

                SendCommand("REST " + _file.Length);
                ReadResponse();
                if (_response != 350)
                    throw new Exception(_responseStr);
                _file.Seek(_file.Length, SeekOrigin.Begin);
                _bytesTotal = _file.Length;
            }
            else
            {
                try
                {
                    _file = new FileStream(local_filename, FileMode.Create);
                }
                catch (Exception ex)
                {
                    _file = null;
                    throw new Exception(ex.Message);
                }
            }

            OpenDataSocket();
            SendCommand("RETR " + remote_filename);
            ReadResponse();

            switch (_response)
            {
                case 125:
                case 150:
                    break;
                default:
                    _file.Close();
                    _file = null;
                    Errormessage += _responseStr;
                    return;
            }
            ConnectDataSocket();
        }

        /// <summary>
        ///     上传文件(循环调用直到上传完毕)
        /// </summary>
        /// <returns>发送的字节数</returns>
        public long DoUpload()
        {
            var bytes = new byte[512];
            long bytes_got;

            try
            {
                bytes_got = _file.Read(bytes, 0, bytes.Length);
                _bytesTotal += bytes_got;
                _dataSock.Send(bytes, (int) bytes_got, 0);

                if (bytes_got <= 0)
                {
                    //上传完毕或有错误发生
                    _file.Close();
                    _file = null;

                    CloseDataSocket();
                    ReadResponse();
                    switch (_response)
                    {
                        case 226:
                        case 250:
                            break;
                        default: //当上传中断时
                        {
                            Errormessage += _responseStr;
                            return -1;
                        }
                    }

                    SetBinaryMode(false);
                }
            }
            catch (Exception ex)
            {
                _file.Close();
                _file = null;
                CloseDataSocket();
                ReadResponse();
                SetBinaryMode(false);
                //throw ex;
                //当上传中断时
                Errormessage += ex.Message;
                return -1;
            }

            return bytes_got;
        }

        /// <summary>
        ///     下载文件(循环调用直到下载完毕)
        /// </summary>
        /// <returns>接收到的字节点</returns>
        public long DoDownload()
        {
            var bytes = new byte[512];
            long bytes_got;

            try
            {
                bytes_got = _dataSock.Receive(bytes, bytes.Length, 0);

                if (bytes_got <= 0)
                {
                    //下载完毕或有错误发生
                    CloseDataSocket();
                    _file.Close();
                    _file = null;

                    ReadResponse();
                    switch (_response)
                    {
                        case 226:
                        case 250:
                            break;
                        default:
                        {
                            Errormessage += _responseStr;
                            return -1;
                        }
                    }

                    SetBinaryMode(false);

                    return bytes_got;
                }

                _file.Write(bytes, 0, (int) bytes_got);
                _bytesTotal += bytes_got;
            }
            catch (Exception ex)
            {
                CloseDataSocket();
                _file.Close();
                _file = null;
                ReadResponse();
                SetBinaryMode(false);
                //throw ex;
                //当下载中断时
                Errormessage += ex.Message;
                return -1;
            }

            return bytes_got;
        }

        #endregion
    }
}