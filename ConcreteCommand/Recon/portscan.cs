using SharpCmd.ConcreteCommand.Jobs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
#if NET40
using System.Threading.Tasks;
#endif

namespace SharpCmd.ConcreteCommand.Recon
{
    internal class portscan : ReconBase
    {
        public override string CommandName => "portscan";

        public override string Description => "scan host port";

        public override string CommandHelp => "portscan 127.0.0.1";

        private static void e_Complete(object sender, SocketAsyncEventArgs e)
        {
        }

        private Action ExecuteDefault(Dictionary<string, string> arguments, JobItem jobItem = null)
        {
            Action action = () =>
            {
                Thread.Sleep(5000);

                string hostname = arguments.Keys.ToArray()[1];
                ArrayList connector = new ArrayList();
                for (int i = 0; i < 65535; i++)
                {
#if NET40
                    jobItem.resetEvent.WaitOne();
#endif
                    Socket socket = new Socket(AddressFamily.InterNetwork,
                                   SocketType.Stream,
                                   ProtocolType.Tcp);
                    connector.Add(socket);
                }

                for (int i = 0; i < 66535; i++)
                {
                    try
                    {
#if NET40
                        jobItem.resetEvent.WaitOne();
#endif

                        SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                        e.RemoteEndPoint = new IPEndPoint(Dns.Resolve(hostname).AddressList[0], i);
                        e.UserToken = ((Socket)connector[i]);
                        e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Complete);
                        ((Socket)connector[i]).ConnectAsync(e);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                // 对于发起 Connect的Socket  可写意味着连接成功
                Socket.Select(null, connector, null, 1000);
                foreach (var item in connector)
                {
                    Console.WriteLine(((Socket)item).RemoteEndPoint.ToString());
                }
            };
            return action;
        }

        public override void Execute(Dictionary<string, string> arguments)
        {
            ExecuteDefault(arguments)();
        }

        public override void ExecuteJob(Dictionary<string, string> arguments, JobItem jobItem)
        {
#if NET40
            Task task = Task.Run(ExecuteDefault(arguments,jobItem), jobItem.TokenSource.Token).ContinueWith(p =>
            {
                jobItem.JobStatus = JobStatus.Completed;
            });
            jobItem.JobStatus = JobStatus.Running;
            jobItem.JobTask = task;
#else
            try 
	        {	        
                // 为了在线程完成后通知 采用重载方法传递一个委托进行回调
		        Thread thread = new Thread(() => ExecuteDefault(arguments)());
                thread.Start();
                jobItem.JobStatus = JobStatus.Running;
                jobItem.JobTask = thread;
	        }
	        catch (global::System.Exception)
	        {
                jobItem.JobStatus = JobStatus.Exception;
	        }
#endif
        }
    }
}
