using System;

namespace tyd8_keyvault_test
{
    class Program
    {
        static void Main(string[] args)
        {
            var payload = "VgT1jH/x8Wa1v2Hth6Mv0wRN+XMGfX+UlEMBZ+vbQWRe6s9rwiuX/T9HCBSdFY0MrhglzKH+QmOsPcGCjFON67YTQfDuiVOQ74PTLw5mT4WLhwiJv4MEsXM1TR0VQnQS2Co3k7yUglo+tubLc9muhiPM3ItyHSlUR84Qwu3QBTHfLAh2AYS8WAXwlBH9fF8d9kno0TaPcLeGOg7w/fiE4RBVARTkKlr0Y6muWv9N+WIVVJXmJzt2LwPZvL043bzT6II/LSCZAze7OALbbcGe3nKTS2l3UAEpQ67pKHy9VQOTWqVwRXv7sxhCGKIo0K3YKw42pHgHwpGz/kkivGoVwQ==";
            byte[] toDecryptInBytes = Convert.FromBase64String(payload);
            Console.WriteLine("Hello World!");
        }
    }
}
