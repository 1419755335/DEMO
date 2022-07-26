using System;
using System.Net.Sockets;

namespace Shikishima
{
    public class Program
    {
        //计时器，输入秒
        public static void timewait(int i)
        {
            //Console.WriteLine("开始计时");
            if (i <= 0) return;
            DateTime nowTime = DateTime.Now;//获取现在时间
            int interval = 0;
            while (interval < i)
            {
                TimeSpan spand = DateTime.Now - nowTime;//计算现在时间与过去时间的差
                interval = spand.Seconds;//赋予差值
            }
            //Console.WriteLine("计时结束");
        }
        //链接socket服务器
        public static Socket Sockeconent(string i, int p)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(i, p);
            return socket;
        }
        /*static void Main()
        {
            timewait(9);
        }*/
        public static void tests(int i)
        {
        }
    }
}
