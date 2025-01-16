namespace wms.infrastructure.Logging
{
    public class LoggerBase
    {
        public string EmailBase(LogSendEmail obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj.EmailTo)) return string.Empty;

                if (AppCoreConfig.EmailConfig == null) return string.Empty;

                if (AppCoreConfig.EmailConfig.IsAllowSendEmail == null || !AppCoreConfig.EmailConfig.IsAllowSendEmail.Value) return obj.Body; // trả body để log lại

                var mail = new System.Net.Mail.MailMessage();
                mail.From = new System.Net.Mail.MailAddress(AppCoreConfig.EmailConfig.Email);
                mail.To.Add(obj.EmailTo);
                for (int i = 1; i < obj.EmailCC.Count; i++)
                {
                    mail.CC.Add(obj.EmailCC[i]);
                }

                mail.Subject = obj.Subject;
                mail.Body = obj.Body;
                mail.IsBodyHtml = true;
                mail.Priority = System.Net.Mail.MailPriority.Normal;

                var smtp = new System.Net.Mail.SmtpClient();
                smtp.Host = AppCoreConfig.EmailConfig.Host;
                smtp.Port = AppCoreConfig.EmailConfig.Port;
                smtp.Credentials = new System.Net.NetworkCredential(AppCoreConfig.EmailConfig.Email, AppCoreConfig.EmailConfig.Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string NotifyBase(LogSendNotify obj)
        {
            var webAddr = $"{AppCoreConfig.NotifyConfig.Host}{AppCoreConfig.NotifyConfig.Endpoint}";

            if (!string.IsNullOrEmpty(AppCoreConfig.NotifyConfig.DefaultGroup) && string.IsNullOrEmpty(obj.GroupID))
                obj.GroupID = AppCoreConfig.NotifyConfig.DefaultGroup;

            try
            {
                string strData = string.Empty;
                int status = 0;
                using (var client = new System.Net.Http.HttpClient())
                {
                    var httpContent = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Add("UserKey", AppCoreConfig.NotifyConfig.UserKey);
                    client.DefaultRequestHeaders.Add("SecretKey", AppCoreConfig.NotifyConfig.SecretKey);

                    var response = client.PostAsync(webAddr, httpContent).Result;
                    strData = response.Content.ReadAsStringAsync().Result;
                    status = (int)response.StatusCode;

                    if (status != 200)
                        return strData;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                string errMessage = $"Lỗi exec service {webAddr} \r\n" +
                       $"Message: {ex.Message} \r\n" +
                       $"Param: {Newtonsoft.Json.JsonConvert.SerializeObject(obj)} ";
                if (ex.InnerException != null)
                    errMessage += $"InnerException.Message: {ex.InnerException.Message}";
                return errMessage;
            }
        }
    }
}
