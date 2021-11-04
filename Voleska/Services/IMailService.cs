using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Voleska.Services
{
    public interface IMailService
    {

        // to sporočilo bo poslano uporabniku ob resetiranju gesla
        Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string subject, string link);


        // to sporočilo bo poslano uporabniku in administratorju ob  kreiranju novega oglasa
        Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, IDictionary<string, string> content);


        // to sporočilo bosta prejela tako admin kot uporabnik, kadar bo opravljena transakcija
        Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string name, string subject, int transactionId);


        // to sporočilo bo prejel administrator ali uporabnik
        Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string name, string surname, string subject, string content, int pogovorId);
        
        // to sporočilo prejmejo uporabniki naročeni na novice, ob kreiranju novega bloga
        Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string subject, string blogTitle, string blogSynopsis, string blogDate,  string blogImage );


        // to sporočilo prejmejo uporabniki naročeni na novice, ob kreiranju novega izdelka
        Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string subject, string productName, string typeOfProduct);

    }

    public class SendGridMailService : IMailService {

        private IConfiguration _configuration;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public SendGridMailService(IConfiguration configuration, IWebHostEnvironment  webHostEnvironment) {
            _configuration = configuration;
             WebHostEnvironment = webHostEnvironment;
        }

        //Nov oglas
        public async Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, IDictionary<string, string> content)
        {



            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);


            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(fromEmail, emailSenderName));
            msg.AddTo(new EmailAddress(toEmail, toEmailUsername));

            var templateData = new TemplateData();


           

            msg.SetTemplateId("d-08a6fe42ed664e769863db1e7118620a");
            

            foreach (var item in content)
            {
                if (item.Key == "naslov") {
                    templateData.Subject = item.Value;
                } else if (item.Key =="vsebina") {
                    var rezultat = Regex.Replace(item.Value, @"\\", "");
                    templateData.Content = rezultat;
                }
                
                

            }
            

            msg.SetTemplateData(templateData);


            var response = await client.SendEmailAsync(msg);

        }

        //Resetiranje gesla
        public async Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string subject, string link)
        {



            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);


            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(fromEmail, emailSenderName));
            msg.AddTo(new EmailAddress(toEmail, toEmailUsername));

            var templateData = new TemplateData();


            msg.SetTemplateId("d-af5b444ec17d4bcab3df602418ee6150");
            templateData.Subject = subject;
            templateData.Link = link;

            msg.SetTemplateData(templateData);


            var response = await client.SendEmailAsync(msg);

        }

        public async Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail,string toEmailUsername, string name, string surname, string subject, string content, int pogovorId) {

           

            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);


            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(fromEmail, emailSenderName));
            msg.AddTo(new EmailAddress(toEmail, toEmailUsername));
            
            var templateData = new TemplateData();

            
            msg.SetTemplateId("d-0d540935c17f405ca4f28916b93a07b2");
            templateData.Subject = subject;
            templateData.Name = name;
            templateData.Surname = surname;
            templateData.Content = content;


            //Na produkciji je potrebno spremeniti naslov na ->> http://192.168.43.42:6001/Sporocila/Details/
            if (name == "Administrator")
            {
                templateData.Conversation = "https://localhost:44321/Sporocila/Details/" + pogovorId;
                
            }
            else {
                templateData.Conversation = "https://localhost:44321/Identity/Account/Login";
            }

            

            msg.SetTemplateData(templateData);
            

            
          
            var response = await client.SendEmailAsync(msg);

        }

        public async Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string subject, string blogTitle, string blogSynopsis, string blogDate, string blogImage) {

            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);


            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(fromEmail, emailSenderName));
            msg.AddTo(new EmailAddress(toEmail, toEmailUsername));

            var slika = File.ReadAllBytes("C:\\Users\\CRIMSON\\source\\repos\\Voleska\\Voleska\\wwwroot\\slike\\" + blogImage);
            var pretvoriSliko = Convert.ToBase64String(slika);

            var templateData = new TemplateData();
            

            msg.SetTemplateId("d-ac5f8accbae84084b2387a92b1d37632");
           // msg.SetTemplateId("d-160fa090b33649f79c674187d7cfcd93");
            templateData.Subject = subject;
            templateData.BlogTitle = blogTitle;
            templateData.BlogSynopsis = blogSynopsis;
            templateData.BlogDate = blogDate;
            /*templateData.BlogImage = $"<img src='data:image/jpeg;base64,{pretvoriSliko}' alt='' />";*/
            //templateData.BlogImage = "data:image/jpeg;base64,"+pretvoriSliko+"";
            msg.SetTemplateData(templateData);
            /*
            var imeSlike = "";
            string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, "C:/Users/CRIMSON/source/repos/Voleska/Voleska/wwwroot/slike");
            imeSlike = blogImage;
            string filePath = Path.Combine(uploadDir, imeSlike);

            
            using (var slika = File.OpenRead(filePath))
            {
                await msg.AddAttachmentAsync(blogImage, slika);
               
                var response = await client.SendEmailAsync(msg);
            }
            
            *//*
            var slika = File.ReadAllBytes("C:\\Users\\CRIMSON\\source\\repos\\Voleska\\Voleska\\wwwroot\\slike\\"+blogImage);

            var image = new SendGrid.Helpers.Mail.Attachment()
            {
                
                Type = "image/png",
                Filename = blogImage,
                Disposition = "inline",
                ContentId = "slika",
                Content = Convert.ToBase64String(slika)

            };
            msg.AddAttachment(image);
            */
            var response = await client.SendEmailAsync(msg);
            



        }

        // Pošiljanje sporočila ob novemu izdelku
        public async Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string subject, string productName, string typeOfProduct)
        {

            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);


            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(fromEmail, emailSenderName));
            msg.AddTo(new EmailAddress(toEmail, toEmailUsername));

         
            var templateData = new TemplateData();


            msg.SetTemplateId("d-c37809e0612c48f4ad61b4f30addf3dd");
           
            templateData.Subject = subject;
            templateData.ProductName = productName;
            templateData.ProductType = typeOfProduct;
       

            switch (typeOfProduct)
            {
                case "Uhani":
                    templateData.ProductTitle = "Na voljo imamo nove uhane!";
                    break;
                case "Broške":
                    templateData.ProductTitle = "Na voljo imamo novo broško!";
                    break;
                case "Zapestnice":
                    templateData.ProductTitle = "Na voljo imamo novo zapestnico!";
                    break;
                case "Ogrlice":
                    templateData.ProductTitle = "Na voljo imamo novo ogrlico!";
                    break;
                case "Prstani":
                    templateData.ProductTitle = "Na voljo imamo nov prstan!";
                    break;

                case "Obeski za ključe":
                    templateData.ProductTitle = "Na voljo imamo nov obesek za ključe!";
                    break;

                default:
                    templateData.ProductTitle = "Na voljo imamo nov izdelek!";
                    break;
            }


            msg.SetTemplateData(templateData);
       
            var response = await client.SendEmailAsync(msg);

        }


        //Za transakcije
        public async Task SendEmailAsync(string emailSenderName, string fromEmail, string toEmail, string toEmailUsername, string name, string subject, int transactionId)
        {



            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);


            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(fromEmail, emailSenderName));
            msg.AddTo(new EmailAddress(toEmail, toEmailUsername));

            var templateData = new TemplateData();


            msg.SetTemplateId("d-ce7417d97a8a4bd0a47112bef0e1bc6c");
            templateData.Subject = subject;
            templateData.Name = name;


            //Na produkciji je potrebno spremeniti naslov na ->> http://192.168.43.42:6001/Sporocila/Details/
           
           templateData.TransactionDetails = "https://localhost:44321/Transakcije/Details/" + transactionId;
            

           

            msg.SetTemplateData(templateData);




            var response = await client.SendEmailAsync(msg);

        }



        private class TemplateData
        {

            /* Te atribute uporabljamo, kadar pošiljemo mail administratorju -- ALI -- kadar se pošilja novi oglas (subject in content)*/
            [JsonProperty("subject")]
            public string Subject { get; set; }

            [JsonProperty("surname")]
            public string Surname { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }


            [JsonProperty("content")]
            public string Content { get; set; }

            [JsonProperty("conversation")]
            public string Conversation { get; set; }

            /* Ti atributi se uporabijo, kadar se pošilja mail ob kreiranju novega bloga*/
            [JsonProperty("blogTitle")]
            public string BlogTitle { get; set; }

            [JsonProperty("blogSynopsis")]
            public string BlogSynopsis { get; set; }

            [JsonProperty("blogDate")]
            public string BlogDate { get; set; }

            [JsonProperty("blogImage")]
            public string BlogImage { get; set; }

            /* Ti atributi se uporabijo, kadar se pošilja mail ob kreiranju novega izdelka*/
            [JsonProperty("productName")]
            public string ProductName { get; set; }

            [JsonProperty("productType")]
            public string ProductType { get; set; }


            [JsonProperty("productTitle")]
            public string ProductTitle { get; set; }

            /* Ti atributi se uporabijo, kadar se pošilja mail ob opravljeni transakciji*/
            [JsonProperty("transactionDetails")]
            public string TransactionDetails { get; set; }

            /* Ti atributi se uporabijo, kadar se pošilja mail ob resetiranju gesla*/
            [JsonProperty("link")]
            public string Link { get; set; }

          

        }

    }

    
}

