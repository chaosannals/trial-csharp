﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.SQLite;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using DynUpdateLibrary.Core;

namespace DynUpdateLibrary
{
    public class DynWorker
    {
        private ILog log = null;

        private string root = null;

        public void Init(string root)
        {
            this.root = root;
            string logpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dyn.log");

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = "%date [%thread] %-5level %logger - %message%newline";
            patternLayout.ActivateOptions();

            RollingFileAppender roller = new RollingFileAppender();
            roller.AppendToFile = true;
            roller.File = logpath;
            roller.Layout = patternLayout;
            roller.MaxSizeRollBackups = 5;
            roller.MaximumFileSize = "4MB";
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.StaticLogFileName = true;
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;

            log = LogManager.GetLogger(GetType().FullName);
        }

        public void Start()
        {
            try
            {
                log.InfoFormat("Start {0}   {1}", AppDomain.CurrentDomain.BaseDirectory, root);
                string dbpath = Path.Combine(root, "sqlite.db");
                using (SQLiteSession sqlite = new SQLiteSession(dbpath))
                {
                    string tn = DateTime.Now.ToString("yyyyMMddHHmmss");
                    sqlite.Execute($"CREATE TABLE table_{tn} (id  INTEGER PRIMARY KEY AUTOINCREMENT, iii INTEGER,ddd DATE DEFAULT((date('now', 'localtime'))) NOT NULL);");
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("异常： {0} {1}", e.Message, e.StackTrace);
            }
        }

        public void Stop()
        {
            log.Info("Stop");
        }
    }
}
