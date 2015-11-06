using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using NLog;

namespace TeamTalkApp.NET.Utils
{

    public class ChatParser
    {

        private static Logger log = LogManager.GetCurrentClassLogger();

        public event EventHandler UpdateUserList;
        public event EventHandler UpdateAgendaText;
        public event EventHandler UpdateQueueList;
        public event EventHandler UpdateQueue;
        public event EventHandler UpdateStreamList;
        public event EventHandler MoveToExpert;
        public event EventHandler MoveToOrigin;

        private static ChatParser instance;

        private ChatParser() { }

        public static ChatParser Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ChatParser();
                }
                return instance;
            }
        }

        public class MessageEventArgs : EventArgs
        {
            public MessageEventArgs(String message)
            {
                this.Message = message;
            }

            public String Message { get; private set; }
        }

        public void Parse(String message)
        {
            //Message handling here
            if (message.StartsWith("USR:")) { 
                EventHandler handler = UpdateUserList;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            } else if (message.StartsWith("AGENDA:"))
            {
                HandleAgenda(message);
            }
            else if (message.StartsWith("QUEUE:"))
            {
                EventHandler handler = UpdateQueue;
                if (handler != null)
                {
                    handler(this, new MessageEventArgs(message.Substring(6)));
                }
            } else if (message.StartsWith("QUEUE_LIST:"))
            {
                EventHandler handler = UpdateQueueList;
                if (handler != null)
                {
                    handler(this, new MessageEventArgs(message.Substring(11)));
                }
            } else if (message.StartsWith("STREAM_LIST:"))
            {
                EventHandler handler = UpdateStreamList;
                if (handler != null)
                {
                    handler(this, new MessageEventArgs(message.Substring(12)));
                }
            } else if (message.StartsWith("MOVETO:EXPERT"))
            {
                EventHandler handler = MoveToExpert;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            else if (message.StartsWith("MOVETO:ORIGIN"))
            {
                EventHandler handler = MoveToOrigin;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            } 
        }

        private void HandleAgenda(string message)
        {
            EventHandler handler = UpdateAgendaText;
            if (handler != null)
            {
                try
                {
                    string compressedAgenda = message.Substring(7);
                    string decompressedAgenda = StringCompression.Decompress(compressedAgenda);
                    handler(this, new MessageEventArgs(decompressedAgenda));
                }
                catch (Exception exc)
                {
                    log.Error(exc.Message);
                }

            }
        }

    }

  
}
