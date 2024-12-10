using Library.Clinic.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Library.Clinic.DTO;

namespace Api.Clinic.Database
{
    public class MySQLContext
    {  
        private string connectionString;

        //hid all MySQL login secrets 
        public MySQLContext()
        {
            // Retrieve environment variables
            var server = Environment.GetEnvironmentVariable("DB_CLINIC_SERVER");
            var database = Environment.GetEnvironmentVariable("DB_CLINIC_NAME");
            var user = Environment.GetEnvironmentVariable("DB_CLINIC_USER");
            var password = Environment.GetEnvironmentVariable("DB_CLINIC_PASSWORD");

            connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
        }

        public IEnumerable<Physician>? SearchPhysicians(string query)
        {
            var returnVal = new List<Physician>();

            try{
                using (var conn = new MySqlConnection(connectionString)){
                    var procedure = "PhysicianSearch";
                    using (var cmd = new MySqlCommand(procedure, conn)){
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("@query", query));

                        conn.Open();
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            var newApp = new Physician
                            {
                                Id = (int)reader["Id"],
                                Name = reader["PhysicianName"].ToString()
                            };

                            returnVal.Add(newApp);
                        }
                        conn.Close();
                    }
                }
            } 
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
            return returnVal;
            
        }

        public IEnumerable<Physician> GetPhysicians()
        {
            var returnVal = new List<Physician>();

            try{
                using (var conn = new MySqlConnection(connectionString))
                {
                    var str = "SELECT * from Physician";
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = str;

                        try
                        {
                            conn.Open();
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                var newApp = new Physician
                                {
                                    Id = (int)reader["Id"],
                                    Name = reader["PhysicianName"].ToString()
                                };

                                returnVal.Add(newApp);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"MySQL error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
            return returnVal;
        }

        public void DeletePhysician(int id)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                var str = "PhysicianDelete";
                using (var cmd = new MySqlCommand(str, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var param = new MySqlParameter("@PhysicianId", id);
                    cmd.Parameters.Add(param);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public void UpdatePhysician(Physician p)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                var str = "PhysicianUpdate";
                using (var cmd = new MySqlCommand(str, conn))
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("@PhysicianId", p.Id));
                    cmd.Parameters.Add(new MySqlParameter("@NewName", p.Name));
                    cmd.Parameters.Add(new MySqlParameter("@NewLicenseNumber", p.LicenseNumber));
                    cmd.Parameters.Add(new MySqlParameter("@NewGradDate", p.GradDate));
                    cmd.Parameters.Add(new MySqlParameter("@NewSpecializations", p.Specializations));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public Physician AddPhysician(Physician p)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                var str = "PhysicianAdd";
                using (var cmd = new MySqlCommand(str, conn))
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("@PhysicianName", p.Name));
                    cmd.Parameters.Add(new MySqlParameter("@LicenseNumber", p.LicenseNumber));
                    cmd.Parameters.Add(new MySqlParameter("@GradDate", p.GradDate));
                    cmd.Parameters.Add(new MySqlParameter("@Specializations", p.Specializations));
                    var param = new MySqlParameter("@PhysicianId", p.Id);
                    param.Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    p.Id = (int)param.Value;
                    conn.Close();
                }

                return p;
            }
        }

    }
}