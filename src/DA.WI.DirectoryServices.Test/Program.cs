using DA.WI.NSGHSM.Core.Services;
using Microsoft.Extensions.Options;
using System;

namespace DA.WI.DirectoryServices.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string LDAPDomain;
                string LDAPUserName;
                string LDAPUserPassword;

                string userNameToAuth;
                string userPasswordToAuth;



                Console.WriteLine("-- Connection to LDAP Service; Parameters are optional: if not given the current domain user parameters will be used------------------------");

                Console.Write("Enter LDAP Domain URL (optional): ");
                LDAPDomain = Console.ReadLine();
                Console.Write("Enter LDAP Service UserName (optional) : ");
                LDAPUserName = Console.ReadLine();
                Console.Write("Enter LDAP Service UserPassword (optional) : ");
                LDAPUserPassword = ReadPassword();


                Console.WriteLine("-- User Authentication------------------------");


                Console.Write("Enter User Name : ");
                userNameToAuth = Console.ReadLine();
                Console.Write("Enter Password  : ");
                userPasswordToAuth = ReadPassword();

                Console.WriteLine("Contacting LDAP Server...");

                var options = new OptionsWrapper<PrincipalContextOptions<PrincipalContextManager>>(new PrincipalContextOptions<PrincipalContextManager>
                {
                    IsEnabled = true,
                    Domain = LDAPDomain,
                    ServiceUserName = LDAPUserName,
                    ServiceUserPassword = LDAPUserPassword
                });

                var principalContext = new PrincipalContextManager(options, new ConsoleLogger<PrincipalContextManager>());
                bool isValid = principalContext.ValidateCredential(userNameToAuth, userPasswordToAuth);

                if (isValid == true)
                {
                    Console.WriteLine(string.Format(@"The user {0} is valid", userNameToAuth));
                }
                else
                {
                    Console.WriteLine(string.Format(@"The user {0} is NOT valid", userNameToAuth));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey(true);
        }


        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;
                }
                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        // remove one character from the list of password characters
                        password = password.Substring(0, password.Length - 1);
                        // get the location of the cursor
                        int pos = Console.CursorLeft;
                        // move the cursor to the left by one character
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        // replace it with space
                        Console.Write(" ");
                        // move the cursor to the left by one character again
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);
            }

            // add a new line because user pressed enter at the end of their password
            Console.WriteLine();
            return password;
        }

    }
}


