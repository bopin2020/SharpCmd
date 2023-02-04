using SharpCmd.Lib.Attributes;
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
    [Test("Job任务示例测试")]
    internal class LongtimeRunTest : JobBase
    {
        public override string CommandName => "jobtest";

        public override string Description => "longtime to run under the background";

        public override string CommandHelp => "LongtimeRunTest";

        private Action ExecuteDefault(Dictionary<string, string> arguments,JobItem completionCallback = default)
        {
            return new Action(() =>
            {
                try
                {
                    Thread.Sleep(10 * 1000);
                    Console.WriteLine("longtime over");
                    if (completionCallback != null)
                    {
                        completionCallback.JobStatus = JobStatus.Completed;
                    }
                }
                catch (Exception ex)
                {
                    if (completionCallback != null)
                    {
                        completionCallback.JobStatus = JobStatus.Exception;
                        completionCallback.Description = ex.Message;
                    }
                }

            });
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
                if(jobItem.JobStatus == JobStatus.Running)
                    jobItem.JobStatus = JobStatus.Completed;
            });
            jobItem.JobStatus = JobStatus.Running;
            jobItem.JobTask = task;
            ThreadPool.QueueUserWorkItem(p => {
                try 
	            {	
                    // 任务中的异常必须等待才能获取
		            task.Wait();
	            }
	            catch (global::System.Exception ex)
	            {
                    jobItem.JobStatus = JobStatus.Exception;
                    jobItem.Description = ex.Message;
	            }
            });



#else
            try
            {
                ThreadStart callback = () =>
                {
                    ExecuteDefault(arguments,jobItem)();
                };

		        Thread thread = new Thread(callback);
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
