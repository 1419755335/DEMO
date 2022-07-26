using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Shikishima;

namespace SQL_SOCKET_SERVER
{
    internal class Program
    {
        static public string sqlset= "Server=localhost;Database=useforchat;uid=root;charset=utf8;pwd=";
        static public string sqlselect = "SELECT * FROM chat ORDER BY time DESC limit 1;";
        static public string sqlinsent = "insert into chat Values (NOW(),'系统','测试','0.0.255.0');";
        //输入登录状态 根据语句查询返回结果
        //static public string scoketIP = "127.0.0.1";
        //static public int scoketprot = 8888;
        static List<Socket> online = new List<Socket>();
        static public string Sqlselcet(string sqlselect)
        {
            MySqlConnection conn = Sqlconect(1);
            string str="0";
            //Console.Write("开始查询。。。\n");
            MySqlCommand cmd = new MySqlCommand(sqlselect,conn);
            Console.Write(cmd+"\n");
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                str = reader.GetString("time") + "|" + reader.GetString("name") + "|" + reader.GetString("taking") /*+ "|" + reader.GetString("IP")*/;
            }
            //Console.Write(str + "\n");
            return str;
            conn.Close();
           

        }
        //像服务器插入信息
        static public void Sqlinsert(string str)
        {
            string pt = "insert into chat Values (NOW()," + str + ");";
            MySqlConnection conn = Sqlconect(1);
            MySqlCommand cmd= new MySqlCommand(pt, conn);
            int result = cmd.ExecuteNonQuery();
            //Console.Write(result + "\n");
            conn.Close();
        }
        //登录服务器返回登录状态
        static public MySqlConnection Sqlconect(int i)
        {
            //Console.Write("等待数据库链接。。。\n");
            MySqlConnection conn = new MySqlConnection(sqlset);
            conn.Open();
            //if(conn.State==System.Data.ConnectionState.Open)
            //{
            //    Console.Write("成功链接数据库\n");
            //}
            return conn;
        }
        static void waitClient()
        {
            IPEndPoint pos=new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            Socket lisener= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            lisener.Bind(pos);
            lisener.Listen(30);
            while(true)
            {
                Console.Write("等待链接\n");
                Socket s = lisener.Accept();
                Console.Write("检测到链接\n");
                online.Add(s);
                Thread listen = new Thread(new ParameterizedThreadStart(lisen));
                listen.Start();

            }
        }
        //监听客户端
        static void lisen(object obj)
        {
            string str = "0";
            Socket sclient = obj as Socket;
            byte[] buf = new byte[1024];
            while (true)
            {
                try
                {
                    int len = sclient.Receive(buf);
                    str = Encoding.UTF8.GetString(buf, 0, len);
                    Sqlinsert(str);
                    Console.Write(str);
                }
                catch(Exception e)
                {
                    break;
                }
            }
        }
        static void sendgmsg(Socket client,string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str); 
            client.Send(data);
        }
        static void Main(string[] args)
        {
            //创建线程等待客户端链接
            Thread wait=new Thread(new ThreadStart(waitClient));
            wait.Start();
            //获取登录状态

            //Socket socket= Shikishima.Program.Sockeconent(scoketIP, scoketprot);
           // waitClient();
            //string str= Sqlselcet(sqlselect);
            //Sqlinsert(sqlinsent);
            //waitClient();
            Shikishima.Program.timewait(30);
        }
        
    }
}
