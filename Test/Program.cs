using System;
using System.Data.SqlClient;
using System.Data;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var connString = "server=14.232.244.235, 6789;UID=hungyen;Password=hungyen@123;Database=hungyen";
                var conn = new SqlConnection(connString);
                conn.Open();
                using (var cmd = new SqlCommand("Select * from dbo.t_Data_Logger_0001_01", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write(reader.GetValue(i) + " ");
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
