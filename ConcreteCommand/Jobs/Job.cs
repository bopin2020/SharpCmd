using SharpCmd.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
#if NET40
using System.Threading.Tasks;
#endif

namespace SharpCmd.ConcreteCommand.Jobs
{
    internal enum JobStatus
    {
        /// <summary>
        /// 任务初始化
        /// </summary>
        Init,
        /// <summary>
        /// 任务运行中
        /// </summary>
        Running,
        /// <summary>
        /// 任务挂起
        /// </summary>
        Suspend,
        /// <summary>
        /// 任务预恢复
        /// </summary>
        ResumePending,
        /// <summary>
        /// 任务取消
        /// </summary>
        Canceled,
        /// <summary>
        /// 任务完成
        /// </summary>
        Completed,
        /// <summary>
        /// 发生异常
        /// </summary>
        Exception,
        /// <summary>
        /// NotSupport
        /// </summary>
        NotSupport,
        /// <summary>
        /// 任务死亡
        /// </summary>
        Extinct,
    }

    internal class JobItem
    {
        public string JobID { get; set; } = Guid.NewGuid().ToString().Substring(0,6);

        public string Command { get; set; }

        public string Description { get; set; } = "N/A";

        public DateTime StartTime { get; set; } = DateTime.Now;

        public double Runtime { get; set; }
#if NET35
        public Thread JobTask { get; set; }
#endif
#if NET40
        public Task JobTask { get; set; }

        /// <summary>
        /// Task 取消
        /// </summary>
        public CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();

        public ManualResetEvent resetEvent { get; set; } = new ManualResetEvent(true);
#endif

        public JobStatus JobStatus { get; set; }
    }

    internal class Job : JobBase
    {
        public override string CommandName => "job";

        public override string Description => "job managed";

        public override string CommandHelp => @"
对于非长期运行任务 不建议加入到此任务管理队列中
job add portscan localhost
job list
job kill xxx
job suspend xxx         may be failed
job resume xxx
";
        public IList<JobItem> JobManage = new List<JobItem>();  

        private void CancelJob(params JobItem[] jobs)
        {
            foreach (var job in jobs)
            {
#if NET40
                job.TokenSource.Cancel();

#else
                job.JobTask.Abort();

#endif
                job.JobStatus = JobStatus.Canceled;
            }

        }

        private void ResumeJob(params JobItem[] jobs)
        {
            foreach (var job in jobs)
            {
#if NET40
                job.resetEvent.Set();

#else
                job.JobTask.Resume();

#endif
                job.JobStatus = JobStatus.ResumePending;
            }
        }

        private void SuspendJob(params JobItem[] jobs)
        {
            foreach (var job in jobs)
            {
#if NET40
                job.resetEvent.Reset();

#else
                job.JobTask.Suspend();

#endif
                job.JobStatus = JobStatus.Suspend;
            }

        }


        public override void Execute(Dictionary<string, string> arguments)
        {
            if (base.HelpCheck(arguments)) return;

            if(arguments.ContainsKey("list"))
            {
                Console.WriteLine("JobID\tCommand\tDescription\tTaskId(ThreadId)\tStartTime\tRuntime\tStatus");
#if NET40
                Parallel.ForEach(JobManage,
                    job =>
                    {
                        job.Runtime = (DateTime.Now - job.StartTime).TotalMilliseconds;
                        Console.WriteLine(job.JobID + "\t" + job.Command + "\t" + job.Description + "\t" + job.JobTask?.Id + "\t" +  job.StartTime.ToString() +  "\t" + job.Runtime + "\t" + job.JobStatus.ToString());
                    });
#else
                foreach (var job in JobManage)
	            {
                        job.Runtime = (DateTime.Now - job.StartTime).TotalMilliseconds;
                        Console.WriteLine(job.JobID + "\t" + job.Command + "\t" + job.Description + "\t" + job.JobTask?.ManagedThreadId + "\t" +  job.StartTime.ToString() +  "\t" + job.Runtime + "\t" + job.JobStatus.ToString());
                }
#endif
            }

            if (arguments.ContainsKey("job") && arguments.ContainsKey("kill"))
            {
                if(arguments.Keys.ToArray()[2] == "*")
                {
                    CancelJob(JobManage.ToArray());
                }
                else
                {
                    var job = JobManage.Where(x => x.JobID == arguments.Keys.ToArray()[2]).FirstOrDefault();
                    CancelJob(job);
                }
            }

            if (arguments.ContainsKey("job") && arguments.ContainsKey("suspend"))
            {
                if (arguments.Keys.ToArray()[2] == "*")
                {
                    SuspendJob(JobManage.ToArray());
                }
                else
                {
                    var job = JobManage.Where(x => x.JobID == arguments.Keys.ToArray()[2]).FirstOrDefault();
                    SuspendJob(job);
                }
            }

            if (arguments.ContainsKey("job") && arguments.ContainsKey("resume"))
            {
                if (arguments.Keys.ToArray()[2] == "*")
                {
                    ResumeJob(JobManage.ToArray());
                }
                else
                {
                    var job = JobManage.Where(x => x.JobID == arguments.Keys.ToArray()[2]).FirstOrDefault();
                    ResumeJob(job);
                }
            }


            if (arguments.ContainsKey("job") && arguments.ContainsKey("add"))
            {
                arguments.Remove("job");
                arguments.Remove("add");

                JobItem jobitem = new JobItem();
                jobitem.Command = arguments.Keys.First();

                JobManage.Add(jobitem);

                SharpCmd.JobProxy(arguments,jobitem);
            }

        }
    }
}
