using Bernhoeft.Core.Mail;
using Bernhoeft.Core.Mail.Engines;
using ControlePonto.Core.Contracts;
using ControlePonto.Core.Entities;
using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Outlook;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using HtmlAgilityPack;
using System.IO;

namespace ControlePonto.Adapters.Gateways
{
    public class EmailEngine : IEmailService
    {
        Application Application = new Application();
        private object MessageBox;
        string _caminhoDestino;

        public EmailEngine()
        {

        }

        public EmailEngine(string caminhoDestino)
        {
            _caminhoDestino = caminhoDestino;
        }

        private static void EnviarEmail(string assunto, string corpo, Bernhoeft.Core.Mail.Attachment[] caminhoAnexo, List<string> emails, string imgPath)
        {
            MailGridEngine engine = new MailGridEngine();
            engine.SendMail(new MailModel()
            {
                Sender = "pontos@bernhoeft.com.br",
                SenderName = "Novo controle de jornada",
                Priority = System.Net.Mail.MailPriority.High,
                Attachments = caminhoAnexo,
                Subject = assunto,
                Body = corpo,
                To = emails,
                imagePath = imgPath
            });
        }

        public void SendEmail(string assunto, string corpo, List<string> emails, Planilha planilha, string imgPath)
        {
            Bernhoeft.Core.Mail.Attachment attachment = new Bernhoeft.Core.Mail.Attachment(null, planilha.CaminhoFonte, null);
            Bernhoeft.Core.Mail.Attachment[] attachments = { attachment };

            EnviarEmail(assunto, corpo, attachments, emails, imgPath);
        }

        public void GetEmail()
        {
            ExtrairEmail();
        }

        //private void Get

        private void ExtrairEmail()
        {
            ExchangeService service;
            #region TOBEIMPLEMENTEDAFTER
            //List<FileStream> fileStreams = new List<FileStream>(); 
            #endregion
            try
            {
                Console.WriteLine("Registering Exchange connection");

                service = new ExchangeService
                {
                    Credentials = new WebCredentials("pontos@bernhoeft.com.br", "Ber@2020C@")
                };
            }
            catch
            {
                Console.WriteLine("new ExchangeService failed. Press enter to exit:");
                throw;
            }

            service.Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx");

            try
            {
                ItemView view = new ItemView(400, 0, OffsetBasePoint.Beginning);
                view.OrderBy.Add(ItemSchema.DateTimeReceived, SortDirection.Descending);

                FindItemsResults<Item> findResults = service.FindItems(WellKnownFolderName.Inbox, view);
                ServiceResponseCollection<GetItemResponse> items = service.BindToItems(findResults.Select(item => item.Id), new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.Subject, EmailMessageSchema.From, EmailMessageSchema.ToRecipients, EmailMessageSchema.CcRecipients, EmailMessageSchema.DateTimeReceived, EmailMessageSchema.Body, ItemSchema.Attachments, EmailMessageSchema.IsRead));
                var sortedItems = items.OrderBy(x => x.Item.DateTimeReceived);
                foreach (var item in sortedItems)
                {
                    EmailMessage message = EmailMessage.Bind(service, new ItemId(item.Item.Id.ToString()), new PropertySet(BasePropertySet.IdOnly, ItemSchema.Attachments));

                    MailItem mailItem = new MailItem();
                    mailItem.From = ((Microsoft.Exchange.WebServices.Data.EmailAddress)item.Item[EmailMessageSchema.From]).Address;
                    mailItem.ToRecipients = ((Microsoft.Exchange.WebServices.Data.EmailAddressCollection)item.Item[EmailMessageSchema.ToRecipients]).Select(recipient => recipient.Address).ToArray();
                    mailItem.CcRecipients = ((Microsoft.Exchange.WebServices.Data.EmailAddressCollection)item.Item[EmailMessageSchema.CcRecipients]).Select(ccRecipients => ccRecipients.Address).ToArray();
                    mailItem.Subject = item.Item.Subject;
                    mailItem.DataRecebimento = item.Item.DateTimeReceived;
                    mailItem.Body = ConvertToPlainText(item.Item.Body.Text.ToString());
                    mailItem.isRead = Convert.ToBoolean(item.Item[EmailMessageSchema.IsRead].ToString());

                    if (mailItem.Subject.Contains("Controle de ponto")/* &&  !mailItem.isRead*/)
                    {
                        foreach (Microsoft.Exchange.WebServices.Data.Attachment attachment in message.Attachments)
                        {
                            if (attachment is FileAttachment)
                            {
                                FileAttachment fileAttachment = attachment as FileAttachment;
                                fileAttachment.Load();

                                if (!string.IsNullOrEmpty(_caminhoDestino))
                                {
                                    if (fileAttachment.Name.ToUpper().Contains(".XLSM"))
                                    {
                                        string dest = Path.Combine(_caminhoDestino, fileAttachment.Name);
                                        string fileName = fileAttachment.Name;
                                        int i = 1;

                                        while (File.Exists(dest))
                                        {
                                            string previousIterator = "(" + (i - 1) + ")";
                                            fileName = fileName.Replace(".xlsm", "").Replace(previousIterator, "") + "(" + i + ").xlsm";
                                            dest = Path.Combine(_caminhoDestino, fileName);
                                            i++;
                                        }

                                        dest = Path.Combine(_caminhoDestino, fileName);
                                        fileAttachment.Load(dest);
                                        FileStream fileStream = new FileStream(dest, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                        fileAttachment.Load(fileStream);
                                        fileStream.Close();
                                        fileStream.Dispose();
                                        #region TOBEIMPLEMENTEDAFTER
                                        //fileStreams.Add(fileStream); 
                                        #endregion
                                    }
                                }
                                else
                                {
                                    throw new ClassNotInitializedCorrectly("Para extrair o email você precisa initializar este objeto com o caminho de destino dos arquivos");
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Email lido");
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            #region TOBEIMPLEMENTEDAFTER
            //return fileStreams; 
            #endregion
        }

        private static string ConvertToPlainText(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            StringWriter sw = new StringWriter();
            ConvertTo(doc.DocumentNode, sw);
            sw.Flush();
            return sw.ToString();
        }

        private static void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }

        private static void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                        case "br":
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }


        internal class MailItem
        {
            public string From;
            public string[] ToRecipients;
            public string[] CcRecipients;
            public DateTime DataRecebimento;
            public string Subject;
            public string Body;
            public bool isRead;
        }

        public class ClassNotInitializedCorrectly : System.Exception
        {
            public ClassNotInitializedCorrectly(string message) : base(message)
            {

            }
        }
    }
}
