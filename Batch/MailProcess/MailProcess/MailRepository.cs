using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text;
using MailProcess.DBContext;
using MailProcess.Models;
using System.Diagnostics;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.Reflection;
using MailKit.Security;

namespace MailProcess
{
    #region enum
    /// <summary>
    /// ReceiveFlag
    /// </summary>
    public enum ReceiveFlag
    {
        None = 0,
        Receive = 1
    }

    /// <summary>
    /// SSLCheck
    /// </summary>
    public enum SSLCheck
    {
        No = 0,
        Yes = 1
    }

    /// <summary>
    /// IniFileString
    /// </summary>
    public enum IniFileString
    {
        ServerName,
        UserName,
        Password,
        DataBase,
        EmailAddress,
        EmailPassword,
        ReceiveInterval,
        SavePath,
        HostImap4,
        HostSMTP,
        PortImap4,
        PortSMTP,
        SSLImap4,
        SSLSMTP
    }
    #endregion

    /// <summary>
    /// MailRepository
    /// </summary>
    public class MailRepository
    {
        #region Variable

        private const string BRACKET_LEFT = "【";
        private const string BRACKET_RIGHT = "】";
        private const char EQUAL = '=';
        private const string FILE_NAME = "{0}.{1}";
        private const string CONNECTION = "data source={0};initial catalog={1};user id={2};password={3}";
        private const string CONNECTION_WINDOW = "data source={0};initial catalog={1};Integrated Security=true";
        private const string INI_FILE_NAME = "Info.ini";
        private const string EXT = "eml";
        private const string FOLDER_SEND_STRING = "sent";
        private const string CONST_NM_REPLACE = "【社員名】";
        public const string CONFIG_MAIL_SEND_NAME = "C014";

        private string serverName, userName, password, dataBase, connectionString;
        private string emailAddress, emailPassword, hostImap4, hostSMTP;
        private string savePath;
        private int portImap4, portSmtp, sslImap4, sslSMTP;

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public MailRepository()
        {
            getValueIni();
        }

        #endregion

        #region getValueIni
        /// <summary>
        /// getValueIni
        /// </summary>
        protected void getValueIni()
        {
            WriteLog(Environment.NewLine + string.Format("Start Method: getValueIni"));
            var getDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string iniPath = Path.Combine(getDirectory, INI_FILE_NAME);
            if (File.Exists(iniPath))
            {
                using (StreamReader reader = new StreamReader(iniPath))
                {
                    while (reader.Peek() >= 0)
                    {
                        string[] line = reader.ReadLine().Split(EQUAL);
                        IniFileString key;
                        if (Enum.TryParse(line[0], true, out key))
                        {
                            switch (key)
                            {
                                case IniFileString.ServerName:
                                    this.serverName = line[1];
                                    break;
                                case IniFileString.UserName:
                                    this.userName = line[1];
                                    break;
                                case IniFileString.Password:
                                    this.password = line[1];
                                    break;
                                case IniFileString.DataBase:
                                    this.dataBase = line[1];
                                    break;
                                case IniFileString.EmailAddress:
                                    this.emailAddress = line[1];
                                    break;
                                case IniFileString.EmailPassword:
                                    this.emailPassword = line[1];
                                    break;
                                case IniFileString.SavePath:
                                    this.savePath = line[1];
                                    break;
                                case IniFileString.HostImap4:
                                    this.hostImap4 = line[1];
                                    break;
                                case IniFileString.HostSMTP:
                                    this.hostSMTP = line[1];
                                    break;
                                case IniFileString.PortImap4:
                                    this.portImap4 = GetConfig.getIntValue(line[1]);
                                    break;
                                case IniFileString.PortSMTP:
                                    this.portSmtp = GetConfig.getIntValue(line[1]);
                                    break;
                                case IniFileString.SSLImap4:
                                    this.sslImap4 = GetConfig.getIntValue(line[1]);
                                    break;
                                case IniFileString.SSLSMTP:
                                    this.sslSMTP = GetConfig.getIntValue(line[1]);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(this.userName))
                {
                    this.connectionString = string.Format(CONNECTION_WINDOW, this.serverName, this.dataBase);
                }
                else
                {
                    this.connectionString = string.Format(CONNECTION, this.serverName, this.dataBase, this.userName, this.password);
                }
            }
            WriteLog(string.Format("End Method: getValueIni"));
        }
        #endregion

        #region Main Process

        #region Receive
        /// <summary>
        /// ReceiveMails
        /// </summary>
        public void ReceiveMails()
        {
            Stopwatch watch = Stopwatch.StartNew();
            int success = 0, fail = 0;
            WriteLog("＊ReceiveMails start at " + DateTime.Now);
            try
            {
                var allEmails = GetAllMails().OrderBy(m => m.Value.Date);
                var listEmailMove = new List<UniqueId>();
                foreach (var item in allEmails)
                {
                    var email = item.Value;
                    if (string.IsNullOrEmpty(email.Subject) || string.IsNullOrEmpty(email.InReplyTo))
                    {
                        fail++;
                        continue;
                    }

                    int bracketLeftPos = email.Subject.IndexOf(BRACKET_LEFT);
                    int bracketRightPos = email.Subject.IndexOf(BRACKET_RIGHT);
                    string subject = bracketLeftPos < 0 || bracketRightPos < 0 ? string.Empty :
                                        email.Subject.Substring(bracketLeftPos + 1, bracketRightPos - bracketLeftPos - 1);
                    int hId = 0;
                    if (!int.TryParse(subject, out hId))
                    {
                        fail++;
                        continue;
                    }

                    string hidPath = Path.GetDirectoryName(string.Format(savePath + "//{0}//", hId));
                    string path = Path.Combine(hidPath, string.Format(FILE_NAME, email.MessageId, EXT));
                    if (!SaveEmailsToDB(email, hId, path) || !SaveEmails(email, path))
                    {
                        WriteLog(hId + "Fail");
                        fail++;
                        continue;
                    }
                    listEmailMove.Add(item.Key);
                    success++;
                }

                //if (listEmailMove.Count > 0)
                //{
                //    MoveMailToFolder(listEmailMove);
                //}
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }

            watch.Stop();
            WriteLog("Sucess: " + success);
            WriteLog("Fail: " + fail);
            WriteLog(string.Format("Processing Time: {0}s", Math.Round((double)watch.ElapsedMilliseconds / 1000, 2)));
            WriteLog("Done at " + DateTime.Now);
        }

        #region SaveEmailsToDB
        /// <summary>
        /// Save Emails To Database
        /// </summary>
        /// <param name="email"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected bool SaveEmailsToDB(MimeMessage email, int hId, string path)
        {
            WriteLog(string.Format("Start Method: SaveEmailsToDB"));
            bool ret = false;
            try
            {
                using (DB db = new DB(this.connectionString, System.Data.IsolationLevel.Serializable))
                {
                    MailboxAddress from = email.From.Mailboxes.FirstOrDefault();
                    string mailAddress = from != null ? from.Address : string.Empty;

                    MailsService service = new MailsService(db);
                    T_Mail_D detail = service.GetDetailByMailAddress(hId, mailAddress);
                    if (detail != null)
                    {
                        detail.SetValue(DateTime.Parse(email.Date.ToString()), (int)ReceiveFlag.Receive, path);

                        if (detail.IsChange)
                        {
                            service.Update(detail);
                            db.Commit();
                        }
                        ret = true;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                ret = false;
            }

            WriteLog(string.Format("End Method: SaveEmailsToDB"));
            return ret;
        }
        #endregion

        #region SaveEmails
        /// <summary>
        /// Save Emails File
        /// </summary>
        /// <param name="email"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected bool SaveEmails(MimeMessage email, string path)
        {
            WriteLog(string.Format("Start Method: SaveEmails"));
            bool ret = false;
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                email.WriteTo(path);

                ret = true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                ret = false;
            }
            WriteLog(string.Format("End Method: SaveEmails"));
            return ret;
        }
        #endregion

        #region GetAll
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected Dictionary<UniqueId, MimeMessage> GetAllMails()
        {
            WriteLog(string.Format("Start Method: GetAllMails"));
            var messages = new Dictionary<UniqueId, MimeMessage>();
            
            using (var client = new ImapClient())
            {
                client.Connect(this.hostImap4, this.portImap4, this.sslImap4 == (int)SSLCheck.Yes);
                client.Authenticate(emailAddress, emailPassword);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);
                var ret = inbox.Search(SearchQuery.NotSeen);
                foreach(var uid in ret)
                {
                    MimeMessage message = inbox.GetMessage(uid);
                    inbox.SetFlags(uid, MessageFlags.Seen, true);
                    messages.Add(uid, message);
                }
                client.Disconnect(true);
            }
            WriteLog(string.Format("End Method: GetAllMails"));

            return messages;
        }
        #endregion

        #region Move To
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="uniqueId"></param>
        //protected void MoveMailToFolder(List<UniqueId> uniqueId)
        //{
        //    using (var client = new ImapClient())
        //    {
        //        client.Connect(this.hostImap4, this.portImap4, this.sslImap4 == (int)SSLCheck.Yes);
        //        client.Authenticate(emailAddress, emailPassword);

        //        var inbox = client.Inbox;
        //        inbox.Open(FolderAccess.ReadWrite);
        //        var readed = inbox.GetSubfolders().Where(m => m.Name == "Reader").FirstOrDefault();
        //        if (readed == null)
        //        {
        //            readed = inbox.Create("Reader", false);
        //        }
        //        inbox.MoveTo(uniqueId, readed);

        //        client.Disconnect(true);
        //    }
        //}
        #endregion

        #endregion

        #region Send
        /// <summary>
        /// SendMail
        /// </summary>
        /// <param name="args"></param>
        public void SendMail(string args)
        {
            WriteLog("＊SendMail start at " + DateTime.Now);
            Stopwatch watch = Stopwatch.StartNew();
            var hIDs = args.Split(',');
            int success = 0, fail = 0;
            WriteLog(string.Format("S Method: GetNameConfig"));
            var mail_Name = GetNameConfig();
            WriteLog(string.Format("E Method: GetNameConfig"));
            foreach (string hID in hIDs)
            {
                IList<MailRepInfo> listSendMail;
                using (DB db = new DB(this.connectionString))
                {
                    MailsService mailsService = new MailsService(db);
                    WriteLog(string.Format("S Method: GetListSendMail"));
                    listSendMail = mailsService.GetListSendMail(int.Parse(hID));
                    WriteLog(string.Format("E Method: GetListSendMail"));
                    if (listSendMail.Count > 0)
                    {
                        // List SendMail
                        for (int i = 0; i < listSendMail.Count; i++)
                        {
                            var message = new MimeMessage();
                            message.To.Add(new MailboxAddress(listSendMail[i].UserNm, listSendMail[i].MailAddress));
                            message.From.Add(new MailboxAddress(mail_Name, this.emailAddress));
                            message.Subject = BRACKET_LEFT + listSendMail[i].HID + BRACKET_RIGHT + listSendMail[i].Subject;

                            var builder = new BodyBuilder();

                            builder.TextBody = listSendMail[i].BodyMail.Replace(CONST_NM_REPLACE, listSendMail[i].UserNm);

                            if (listSendMail[i].FilePath1 != null && listSendMail[i].FilePath1 != "")
                            {
                                builder.Attachments.Add(listSendMail[i].FilePath1);
                            }
                            if (listSendMail[i].FilePath2 != null && listSendMail[i].FilePath2 != "")
                            {
                                builder.Attachments.Add(listSendMail[i].FilePath2);
                            }
                            if (listSendMail[i].FilePath3 != null && listSendMail[i].FilePath3 != "")
                            {
                                builder.Attachments.Add(listSendMail[i].FilePath3);
                            }

                            message.Body = builder.ToMessageBody();

                            try
                            {
                                using (var client = new SmtpClient())
                                {
                                    client.Connect(this.hostSMTP, portSmtp, sslSMTP == (int)SSLCheck.Yes);
                                    client.Authenticate(this.emailAddress, this.emailPassword);
                                    client.Send(message);
                                    ImapClient imap = new ImapClient();
                                    IMailFolder sendFolder = this.GetFolderSent(imap);
                                    if (sendFolder != null)
                                    {
                                        sendFolder.Append(message);
                                        success++;
                                    }
                                    else { fail++; }
                                    client.Disconnect(true);
                                    imap.Disconnect(true);
                                }
                            }
                            catch (Exception e)
                            {
                                fail++;
                                WriteLog("Send Mail Failed : " + e.Message);
                            }
                        }
                    }
                }
            }
            watch.Stop();
            WriteLog("Sucess: " + success);
            WriteLog("Fail: " + fail);
            WriteLog(string.Format("Processing Time: {0}s", Math.Round((double)watch.ElapsedMilliseconds / 1000, 2)));
            WriteLog("Done at " + DateTime.Now);
        }

        /// <summary>
        /// Get folder sent on server
        /// </summary>
        /// <param name="imap"></param>
        /// <returns></returns>
        private IMailFolder GetFolderSent(ImapClient imap)
        {

            WriteLog(string.Format("Start Method: GetFolderSent"));
            try
            {
                imap.Connect(this.hostImap4, this.portImap4, this.sslImap4 == (int)SSLCheck.Yes);
                imap.Authenticate(this.emailAddress, this.emailPassword);

                if (imap.IsConnected)
                {
                    IMailFolder sendFolder = imap.GetFolders(imap.PersonalNamespaces[0]).FirstOrDefault(x => FOLDER_SEND_STRING.Contains(x.Name)); //get folder
                    if (sendFolder == null)
                    {
                        var personal = imap.GetFolder(imap.PersonalNamespaces[0]);
                        sendFolder = personal.Create("sent", true);
                    }
                    WriteLog(string.Format("END Method: GetFolderSent"));
                    return sendFolder;
                }
                WriteLog(string.Format("END Method: GetFolderSent"));
                return null;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                WriteLog(string.Format("END Method: GetFolderSent"));
                return null;
            }
        }
        #endregion

        #endregion

        #region getConfig
        /// <summary>
        /// get config name
        /// </summary>
        /// <returns></returns>
        private string GetNameConfig()
        {
            WriteLog(string.Format("START Method: GetNameConfig"));
            using (DB db = new DB(this.connectionString))
            {
                try
                {
                    MailsService mailsService = new MailsService(db);
                    var Isdefalt = mailsService.GetDefaultValueDrop(CONFIG_MAIL_SEND_NAME);
                    if (Isdefalt != null)
                    {   if(Isdefalt.Value3 == "1")
                        {
                            var lstConfig = mailsService.GetValue2(CONFIG_MAIL_SEND_NAME, 1);
                            WriteLog(string.Format("END Method: GetNameConfig"));
                            return lstConfig.Value2;
                        }
                    }
                    WriteLog(string.Format("END Method: GetNameConfig"));
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    WriteLog(ex.Message);
                    WriteLog(string.Format("END Method: GetNameConfig"));
                    return string.Empty;
                }
            }
        }
        #endregion

        #region WriteLog
        /// <summary>
        /// WriteLog
        /// </summary>
        /// <param name="message"></param>
        protected void WriteLog(string message)
        {
            string logPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Log\\";

            if (!Directory.Exists(Path.GetDirectoryName(logPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));
            }

            string path = Path.Combine(logPath, string.Format(FILE_NAME, "Log-" + DateTime.Today.ToString("yyyyMMdd"), "log"));

            using (StreamWriter file = new StreamWriter(path, true, Encoding.Unicode))
            {
                file.WriteLine(message);
            }
        }

        #endregion
    }
}