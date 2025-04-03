namespace MailProcess
{
    class Program    {
        static void Main(string[] args)
        {
            var mailRepository = new MailRepository();
            if (args.Length == 0)
            {
                mailRepository.ReceiveMails();
            }
            else
            {
                if (args[0] == "0")
                {
                    mailRepository.ReceiveMails();
                }
                else
                {
                    mailRepository.SendMail(args[1]);
                }
            }
        }
    }
}
