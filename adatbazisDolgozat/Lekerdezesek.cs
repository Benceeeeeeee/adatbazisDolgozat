using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adatbazisDolgozat
{
    internal class Lekerdezesek
    {
        private string connectionString;

        public Lekerdezesek()
        {
            this.connectionString = "server=localhost;database=jozsefattila;user=root;password='';";
            Feladatok();
        }

        private void Feladatok()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string q1 = "SELECT szemely.nev FROM `kituntetes` JOIN szemely ON szemely.az = kituntetes.szemaz WHERE kituntetes.ev = 2016;";
                    Execute(connection, q1, "2dij2016");

                    string q2 = "SELECT szemely.nev, foglalkozas.fognev FROM `szemely` JOIN foglalkozas ON szemely.az = foglalkozas.szemaz WHERE foglalkozas.fognev LIKE \"%kritikus%\" ORDER BY szemely.nev;";
                    Execute(connection, q2, "3kritikus");

                    string q3 = "SELECT nev, count(*) AS darab FROM `szemely` JOIN kituntetes ON kituntetes.szemaz = szemely.az GROUP BY szemely.nev HAVING darab >= 3;";
                    Execute(connection, q3, "4tobbszor");

                    string q4 = "SELECT szemely.nev AS Nev FROM kituntetes JOIN foglalkozas ON kituntetes.szemaz = foglalkozas.szemaz JOIN szemely ON kituntetes.szemaz = szemely.az\r\nWHERE foglalkozas.fognev = (SELECT foglalkozas.fognev FROM kituntetes JOIN foglalkozas ON kituntetes.szemaz = foglalkozas.szemaz GROUP BY foglalkozas.fognev ORDER BY COUNT(*) DESC LIMIT 1);";
                    Execute(connection, q4, "5nevsor");

                    string q5 = "SELECT szemely.nev, kituntetes.ev FROM szemely JOIN kituntetes ON kituntetes.szemaz = szemely.az WHERE kituntetes.ev IN (SELECT kituntetes.ev FROM kituntetes JOIN szemely ON szemely.az = kituntetes.szemaz WHERE szemely.nev LIKE \"Bertha Bulcsu\");";
                    Execute(connection, q5, "6bertha");
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        private void Execute(MySqlConnection connection, string query, string leiras)
        {
            Console.WriteLine($"\n---------------------------------\n\t{leiras}");

            using(var command = new MySqlCommand(query, connection))
            {
                using(var reader = command.ExecuteReader())
                {
                    DataTable schema = reader.GetSchemaTable();

                    foreach (DataRow row in schema.Rows)
                    {
                        Console.WriteLine($"{row["columnName"]}\t");
                    }
                    Console.WriteLine();

                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine($"{reader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}
