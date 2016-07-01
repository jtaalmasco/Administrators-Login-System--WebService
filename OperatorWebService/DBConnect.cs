using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace OperatorWebService
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            string connectionString;
            server = "sql.z150.vhostgo.com";
            database = "baiyun888";
            uid = "baiyun888";
            password = "k5f4e3d6";

            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" +
                                "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect at the server");
                        break;
                    case 1045:
                        Console.WriteLine("Password/username not valid.Please try again");
                        break;
                }
                return false;
            }
        }


        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public string Insert(string lat, string lon, string text)
        {
            string query = "INSERT INTO Cartello VALUES (" + lat + "," + lon + ",'" + text + "');";

            //apertura connessione
            if (this.OpenConnection() == true)
            {
                //creazione ed istanziazione comando ed assegnazione della query e della connessione al comando
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //esecuzione query
                cmd.ExecuteNonQuery();
            }
            this.CloseConnection();

            return "Inserimento dati completato con successo";
        }

        public string operatorLogin(string userName, string passWord)
        {
            try
            {
                if (this.OpenConnection() == true)
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandText = "SELECT OperatorName FROM testing_operators where UserName=@user and Password=@pass;";
                    cmd.Parameters.AddWithValue("@user", userName);
                    cmd.Parameters.AddWithValue("@pass", passWord);
                    cmd.Connection = connection;
                    MySqlDataReader myReader;

                    try
                    {
                        myReader = cmd.ExecuteReader();
                        while (myReader.Read())
                        {
                            string URL = "";
                            if (myReader.FieldCount <= 1)
                            {
                                URL = myReader.GetValue(0).ToString();
                            }
                            else
                            {
                                for (int i = 0; i < myReader.FieldCount; i++)
                                {
                                    URL += myReader.GetValue(i).ToString() + ",";
                                }
                            }

                            return URL;
                        }
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }

                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return "";
        }

        public List<string> GetDomainID()
        {
            List<string> result = new List<string>();
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT Domain from testing_domain_group";
                cmd.Connection = connection;
                MySqlDataReader myReader;
                try
                {
                    myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            result.Add(myReader.GetValue(i).ToString());
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            return result;
        }

        public List<string> getClientsPerGroupFunc(int GroupID)
        {
            List<string> result = new List<string>();
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT UserName from testing_users where DomainGroup=@GroupID";
                cmd.Parameters.AddWithValue("@GroupID", GroupID);
                cmd.Connection = connection;
                MySqlDataReader myReader;
                try
                {
                    myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            result.Add(myReader.GetValue(i).ToString());
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return result;
            }
            return result;
        }

        public void AddDomain(string operatorName, int ID, string Domain)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "INSERT into testing_domain_group(DomainID,Domain) VALUES (@ID,@Domain)";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Domain", Domain);
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                InsertLog(operatorName, ID, Domain, "Add");
            }
        }

        public void EditDomainF(string operatorName, int ID, string OrigDomain, string newDomain)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "Update testing_domain_group SET DomainID=@ID,Domain=@newDomain where DomainID=@ID and Domain=@orig";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@newDomain", newDomain);
                cmd.Parameters.AddWithValue("@orig", OrigDomain);
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                InsertLog(operatorName, ID, newDomain, "Edit");
            }
        }

        public void DeleteDomainF(string operatorName, int ID, string Domain)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "Delete FROM testing_domain_group where DomainID=@ID and Domain=@Domain";
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@Domain", Domain);
                cmd.Connection = connection;
                InsertLog(operatorName, ID, Domain, "Delete");
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertLog(string operatorName, int ID, string Domain, string Type)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.CommandText = "INSERT into testing_operators_log(GroupID,OperatorName,OperatingType,Domain) VALUES (@ID,@OperatorName,@Type,@Domain)";
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@Domain", Domain);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@OperatorName", operatorName);
            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
        }


        public List<string> GetOperatorsLogF()
        {
            if (this.OpenConnection() == true)
            {
                List<string> result = new List<string>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM testing_operators_log";
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                MySqlDataReader myReader;

                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        result.Add(myReader.GetValue(i).ToString());
                    }
                }

                return result;
            }
            return null;
        }

        public List<Prop> GetOperatorsLogFS()
        {
            if (this.OpenConnection() == true)
            {
                var list = new List<Prop>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM testing_operators_log";
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                MySqlDataReader myReader;

                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    list.Add(new Prop
                    {
                        ID = (int) myReader["ID"],
                        GroupID = (int)(myReader["GroupID"]),
                        OperatorName = myReader["OperatorName"].ToString(),
                        Time = myReader["Time"].ToString(),
                        OperatingType = myReader["OperatingType"].ToString(),
                        Domain = myReader["Domain"].ToString()
                    });   
                }

                return list;
            }
            return null;
        }

    }
}