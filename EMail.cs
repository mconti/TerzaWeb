public async Task<string> SendEMail()
        {

            // Lato server la Culture è en-EN, scrive "December" invece di "Dicembre"
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("it-IT");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("it-IT");

            // Ricordarsi di abilitare GMail per le app meno sicure...
            // https://myaccount.google.com/lesssecureapps?rapt=AEjHL4Mjl93ilicmgbXwuQ7DNAXazXdSpQYJtx4BEYZStvFRdcq2YN4A5K1gvVaSHXfrVQsTsWMP4gXxvFpCkIcTzktX6QXDfw


            const string fromPassword = "miapwd";
            MailAddress fromAddress = new MailAddress("posta@maurizioconti.com", "Maurizio Conti 1");
            MailAddress toAddress = new MailAddress("maurizio.conti@ittsrimini.edu.it", "Maurizio Conti 2");
            const string subject = "Conferma di ...";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("La tua Buona Azione è stata emessa.\n");
            sb.AppendLine("Ecco i dati di riepilogo:");
            sb.AppendLine($"Data: {DataAcquisto}");
            sb.AppendLine($"Quantità di Buone Azioni acquistate: {Quantita}");

            // Per fare in modo che gli importi vengano con la virgola invece del punto
            string specifier = "C";
            CultureInfo culture = CultureInfo.CreateSpecificCulture("it-IT");
            sb.AppendLine($"Importo totale: {Importo.ToString(specifier, culture)}");

            sb.AppendLine($"Paypal order ID: {PayPalOrderId}");
            sb.AppendLine($"Acquistata da {NomeCognome}");
            sb.AppendLine($"Intestata a {Intestatario}");
            sb.AppendLine($"C.F.:{CodiceFiscale}");

            string body = sb.ToString();

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                try
                {
                    message.Attachments.Add(new Attachment(bazioneJPEG));
                    message.Attachments.Add(new Attachment(bazioneRicevutaJPEG));
                    smtp.Send(message);
                }
                catch(Exception errore)
                {
                    return errore.Message;
                }
                return $"Spedizione da {fromAddress.Address} a {toAddress.Address}, eseguita correttamente.";
            }
        }