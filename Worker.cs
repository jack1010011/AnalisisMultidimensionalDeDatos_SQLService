using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient.Base.Messages;
using System.Configuration;
using System.ServiceProcess;



namespace WorkerService1
{
    public delegate void NewMessageHandler(Message message);
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private SqlTableDependency<ModelClass.Message> sqlTableDependency;
        public event NewMessageHandler OnNewMessage;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        #region ExeAsyncTask
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("***** Test *****  Process Initiated ");
            // new System.Threading.Thread(StartService).Start();
            string conectionString = @"Server=.\SQLSERVER2019;Database=DB23;User Id=sa;Password=@PATITO123@;";
            sqlTableDependency = new SqlTableDependency<WorkerService1.ModelClass.Message>(conectionString, "Messages");
            sqlTableDependency.OnChanged += HandleOnChanged;
            sqlTableDependency.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(2000, stoppingToken);
            }
            //if (stoppingToken.WaitHandle.Handle()) { }
        }
        #endregion


        #region HandleEvents
        private void HandleOnChanged(object sender, RecordChangedEventArgs<ModelClass.Message> e)
        {
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
            {
                _logger.LogInformation(" ==>        " + e.Entity);
            }
        }
        #endregion



        private void MessageRepository_OnNewMessage(Message message)
        {
            Console.WriteLine($"t{message.Body}");
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && sqlTableDependency != null)
                {
                    sqlTableDependency.Stop();
                    sqlTableDependency.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            sqlTableDependency.Stop();
            sqlTableDependency.Dispose();
            Dispose(true);
        }
        #endregion
    }


}




