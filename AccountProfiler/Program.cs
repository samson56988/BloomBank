using AccountProfiler.Services;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine($"Account Profiler Job Started {DateTime.Now.ToString()} . Press CTRL+C to exit.");

        ProfileService profileService = new ProfileService();

        // Define a flag to control the loop
        bool running = true;

        // Attach a cancellation handler to gracefully exit the loop on CTRL+C
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true; // Prevent the process from terminating immediately
            running = false; // Set the flag to exit the loop
        };

        // Main loop
        while (running)
        {
            try
            {
                var getAccount = await profileService.GetCustomers();

                if (getAccount == null)
                {
                    Console.WriteLine("No Account Available for profiling");
                }
                else
                {
                    foreach (var account in getAccount)
                    {
                        Console.WriteLine($"Validating Account for web portal profiling {account.AccountNo}");

                        var checkIfAccountExistOnWeb = await profileService.IsAccountExistsOnBloomPortal(account.AccountNo);

                        if (checkIfAccountExistOnWeb)
                        {
                            Console.WriteLine($"Account already exist on web portal {account.AccountNo}");
                        }
                        else
                        {
                            Console.WriteLine($"Profiling {account.AccountNo} on web portal");

                            var createProfileOnWeb = await profileService.ProfileCustomerOnWebPlatform(account);

                            if (createProfileOnWeb == null)
                            {
                                Console.WriteLine($"Unable to Profile User on Web {account.AccountNo}");
                            }
                            else
                            {
                                Console.WriteLine($"Profiled User on Web Successfully {account.AccountNo}");
                            }
                        }

                        var checkIfAccountExistOnUssd = await profileService.IsAccountExistsOnUssd(account.AccountNo);

                        if (checkIfAccountExistOnUssd)
                        {
                            Console.WriteLine($"Account already exist on ussd platform {account.AccountNo}");
                        }
                        else
                        {
                            Console.WriteLine($"Profiling {account.AccountNo} on ussd portal");

                            var createProfileOnUssd = await profileService.ProfileCustomerOnUssdPlatform(account);

                            if (createProfileOnUssd == null)
                            {
                                Console.WriteLine($"Unable to Profile User on ussd portal {account.AccountNo}");
                            }
                            else
                            {
                                Console.WriteLine($"Profiled User on ussd portal Successfully {account.AccountNo}");
                            }
                        }
                    }
                }

                // Sleep for a specified interval before checking for new accounts
                await Task.Delay(TimeSpan.FromMinutes(5)); // Adjust interval as needed
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error occured while running profiling job {ex.Message}");
            }
           
        }

        // Optional: Perform cleanup tasks before exiting
        Console.WriteLine("Exiting Account Profiler Job. Press any key to close.");
        Console.ReadKey(); // Wait for user input before exiting
    }
}

