using System;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

class VulnerabilityExample
{
    // Insecure file handling (path traversal)
    public static void ReadFile(string filePath)
    {
        try
        {
            string content = File.ReadAllText(filePath);  // Path traversal vulnerability
            Console.WriteLine(content);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading file: " + ex.Message);
        }
    }

    // SQL Injection vulnerability
    public static void ExecuteSqlQuery(string userInput)
    {
        string connString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";
        string query = $"SELECT * FROM Users WHERE Username = '{userInput}'";  // SQL injection vulnerability
        using (SqlConnection conn = new SqlConnection(connString))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["Username"]);
            }
        }
    }

    // Insecure cryptographic algorithm (MD5)
    public static void HashPasswordUsingInsecureAlgorithm(string password)
    {
        using (MD5 md5 = MD5.Create())  // Insecure cryptographic algorithm (MD5)
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            Console.WriteLine("MD5 Hash: " + hash);
        }
    }

    // Unsafe resource access (memory leak example)
    public static void UnsafeResourceAccess()
    {
        string[] largeArray = new string[1000000];  // Allocating a large array without releasing it
        Console.WriteLine("Array allocated, but not properly disposed.");
        // No release mechanism, leading to a memory leak
    }

    // Insecure deserialization
    public static void InsecureDeserialization(string filePath)
    {
        try
        {
            byte[] data = File.ReadAllBytes(filePath);
            // Simulating unsafe deserialization
            var deserializedObject = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(object)); // Insecure deserialization
            Console.WriteLine("Object deserialized: " + deserializedObject.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during deserialization: " + ex.Message);
        }
    }

    // Thread race condition vulnerability
    public static void RaceConditionExample()
    {
        int counter = 0;
        Thread t1 = new Thread(() =>
        {
            for (int i = 0; i < 1000; i++) counter++;
        });
        Thread t2 = new Thread(() =>
        {
            for (int i = 0; i < 1000; i++) counter++;
        });

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        // The final value of counter may not be 2000 due to race conditions
        Console.WriteLine("Counter value after threads: " + counter);
    }

    // Hardcoded credentials vulnerability
    public static void HardcodedCredentials()
    {
        string username = "admin";  // Hardcoded username
        string password = "password123";  // Hardcoded password
        Console.WriteLine("Using hardcoded credentials: " + username + "/" + password);
    }

    // Insecure input handling (Command Injection)
    public static void ProcessUserCommand(string userInput)
    {
        string command = "ping " + userInput;  // Insecure command execution
        System.Diagnostics.Process.Start("CMD.exe", "/C " + command);  // Command injection vulnerability
    }

    // Improper exception handling
    public static void ImproperExceptionHandling()
    {
        try
        {
            // Simulating a divide by zero exception
            int result = 10 / int.Parse("0");
        }
        catch (DivideByZeroException ex)
        {
            // No logging, user-facing error only
            Console.WriteLine("An error occurred: Division by zero.");
        }
    }

    public static void Main(string[] args)
    {
        // Example 1: Path Traversal in file reading
        ReadFile("../../../../../etc/passwd"); // Vulnerable to path traversal

        // Example 2: SQL Injection in database query execution
        Console.WriteLine("Enter username for SQL injection test: ");
        string usernameInput = Console.ReadLine();
        ExecuteSqlQuery(usernameInput);

        // Example 3: Insecure cryptographic algorithm usage (MD5)
        Console.WriteLine("Enter password to hash: ");
        string password = Console.ReadLine();
        HashPasswordUsingInsecureAlgorithm(password);

        // Example 4: Unsafe resource access (memory leak)
        UnsafeResourceAccess();

        // Example 5: Insecure deserialization
        InsecureDeserialization("serialized_data.dat");

        // Example 6: Thread race condition
        RaceConditionExample();

        // Example 7: Hardcoded credentials vulnerability
        HardcodedCredentials();

        // Example 8: Insecure input handling (Command Injection)
        Console.WriteLine("Enter a command to execute (e.g., google.com): ");
        string userCommand = Console.ReadLine();
        ProcessUserCommand(userCommand);

        // Example 9: Improper exception handling
        ImproperExceptionHandling();
    }
}
