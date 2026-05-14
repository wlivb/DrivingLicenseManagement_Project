using BuisnessLogicLayer.DataManagement;
using DVLD_DTOs;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.Configuration;

namespace EmailSendingService
{
    public partial class EmailSendingService : ServiceBase
    {
        private System.Timers.Timer _timer;

        public EmailSendingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 3600000; 
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            EventLog.WriteEntry("EmailSendingService started successfully.", EventLogEntryType.Information);
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                List<ExpiredLicenseInfoDTO> expiredLicenses = clsLicense.GetExpiredLicenses();

                if (expiredLicenses != null && expiredLicenses.Count > 0)
                {
                    CheckAndSendLicenseNotifications(expiredLicenses);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Error in Timer Elapsed: " + ex.Message, EventLogEntryType.Error);
            }
        }

        private void CheckAndSendLicenseNotifications(List<ExpiredLicenseInfoDTO> list)
        {
            foreach (var driver in list)
            {
                try
                {
                    string subject = "تنبيه: انتهاء صلاحية رخصة القيادة";
                    string body = $@"<h3>مرحباً {driver.DriverName}</h3>
                                    <p>نحيطكم علماً بأن رخصتكم رقم <b>{driver.LicenseID}</b> قد انتهت بتاريخ {driver.ExpiryDate.ToShortDateString()}.</p>
                                    <p>يرجى مراجعة دائرة المرور لتجديد الرخصة تجنباً للغرامات.</p>";

                    SendEmail(driver.DriverEmail, subject, body);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry($"Failed to send email to {driver.DriverEmail}: {ex.Message}", EventLogEntryType.Warning);
                }
            }
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            int port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string senderEmail = ConfigurationManager.AppSettings["SenderEmail"];

            string password = Environment.GetEnvironmentVariable("DVLD_EMAIL_PWD");

            using (var smtpClient = new SmtpClient(host))
            {
                smtpClient.Port = port;
                smtpClient.Credentials = new NetworkCredential(senderEmail, password);
                smtpClient.EnableSsl = true;

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(senderEmail, "DVLD System");
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(toEmail);

                    smtpClient.Send(mailMessage);
                }
            }
        }

        protected override void OnStop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            EventLog.WriteEntry("EmailSendingService stopped.", EventLogEntryType.Information);
        }
    }
}