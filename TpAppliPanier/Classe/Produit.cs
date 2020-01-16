using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TpAppliPanier.Classe
{
    public class Produit
    {
        private int id;
        private string label;
        private decimal prix;
        private static SqlCommand command;
        private static SqlDataReader reader;
        

      

        public int Id { get => id; set => id = value; }
        public string Label { get => label; set => label = value; }
        public decimal Prix { get => prix; set => prix = value; }

        public static Produit GetProduitByLabel(string label)
        {
            Produit produit = null;
            string request = "SELECT TOP 1 id, label, prix FROM produit where label = @label";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@label", label));
            Configuration.Connection.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                produit = new Produit()
                {
                    Id = reader.GetInt32(0),
                    Label = label,
                    Prix = reader.GetDecimal(2)
                };
            }
            reader.Dispose();
            command.Dispose();
            Configuration.Connection.Close();
            return produit;
        }

        public static Produit GetProduitById(int id)
        {
            Produit produit = null;
            string request = "SELECT TOP 1 id, label, prix FROM produit where id = @id";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@id", id));
            Configuration.Connection.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                produit = new Produit()
                {
                    Id = id,
                    Label = reader.GetString(1),
                    Prix = reader.GetDecimal(2)
                };
            }
            reader.Dispose();
            command.Dispose();
            Configuration.Connection.Close();
            return produit;
        }

        public bool Save(Produit p)
        {
            bool result = false;
            string request = "INSERT INTO produit(label, prix) OUTPUT INSERTED.ID" +
                " values (@label, @prix)";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@label", p.Label));
            command.Parameters.Add(new SqlParameter("@prix", p.Prix));
            Configuration.Connection.Open();
            Id = (int)command.ExecuteScalar();
            if (Id > 0)
            {
                result = true;
            }
            command.Dispose();
            Configuration.Connection.Close();
            return result;
        }

        public bool Update()
        {
            bool result = false;
            string request = "UPDATE produit set label=@label, prix=@prix where id=@id";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@label", Label));
            command.Parameters.Add(new SqlParameter("@prix", Prix));
            command.Parameters.Add(new SqlParameter("@id", Id));
            Configuration.Connection.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            command.Dispose();
            Configuration.Connection.Close();
            return result;
        }

        public bool Delete()
        {
            bool result = false;
            string request = "DELETE FROM produit where id=@id";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@id", Id));
            Configuration.Connection.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            command.Dispose();
            Configuration.Connection.Close();
            return result;
        }
        public override string ToString()
        {
            string retour = $"Id : {Id}, Label : {Label}, Prix : {Prix} \n";
            
            return retour;
        }

        public static void GetAllProduits()
        {
            
            string request = "SELECT * FROM produit";
            command = new SqlCommand(request, Configuration.Connection);
            Configuration.Connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ID {reader.GetInt32(0)}, Label : {reader.GetString(1)}, Prix : {reader.GetDecimal(2)}");
            }
            reader.Dispose();
            command.Dispose();
            Configuration.Connection.Close();
            
        }

    }
}
