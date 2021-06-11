using System;

namespace Events
{
    class Program
    {
        public delegate void Notify(string address, string message);
        public event Notify Notification;
        static void Main(string[] args)
        {
            var instance = new Program();
            instance.Notification += EmailNotification;
            instance.Notification += SmsNotification;

            instance.Notification("mdemon@gmail.com", "Hello world");
        }

        public static void EmailNotification(string email, string text)
        {
            Console.WriteLine($"To {email}. Message: {text}");
        }

        public static void SmsNotification(string mobile, string text)
        {
            Console.WriteLine($"Moblie Number: {mobile}. Message: {text}");
        }
    }
}
