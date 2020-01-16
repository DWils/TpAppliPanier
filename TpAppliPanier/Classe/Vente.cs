using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace TpAppliPanier.Classe
{
    public class Vente
    {
        private int id;
        private string nom;
        private string telephone;
        private decimal total;
        private List<Produit> produits;
        private static SqlCommand command;
        private static SqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Telephone { get => telephone; set => telephone = value; }
        public decimal Total { get => total; set => total = value; }
        public List<Produit> Produits { get => produits; set => produits = value; }

        public Vente()
        {
            Produits = new List<Produit>();
        }

        public static Vente GetVenteByTelephone(string telephone)
        {
            Vente vente = null;
            string request = "SELECT TOP 1 id, nom, telephone, total FROM vente where telephone = @telephone";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@telephone", telephone));
            Configuration.Connection.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                vente = new Vente()
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Telephone = reader.GetString(2),
                    Total = reader.GetDecimal(3)
                };
            }
            reader.Dispose();
            command.Dispose();
            Configuration.Connection.Close();
            return vente;
        }

        public bool Save()
        {
            bool result = false;
            string request = "INSERT INTO vente(nom, telephone, total) OUTPUT INSERTED.ID" +
                " values (@nom, @telephone, @total)";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@nom", Nom));
            command.Parameters.Add(new SqlParameter("@telephone", Telephone));
            command.Parameters.Add(new SqlParameter("@total", Total));
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
            string request = "UPDATE vente set nom=@nom, telephone=@telephone, total=@total where id=@id";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@id", Id));
            command.Parameters.Add(new SqlParameter("@nom", Nom));
            command.Parameters.Add(new SqlParameter("@telephone", Telephone));
            command.Parameters.Add(new SqlParameter("@total", Total));
            Configuration.Connection.Open();
            if (command.ExecuteNonQuery() > 0)
            {
                result = true;
            }
            command.Dispose();
            Configuration.Connection.Close();
            return result;
        }

        public static void GetAllVentes()
        {
            string request = "SELECT * FROM vente";
            command = new SqlCommand(request, Configuration.Connection);
            Configuration.Connection.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"ID {reader.GetInt32(0)}, Nom : {reader.GetString(1)}, Telephone : {reader.GetString(2)}, Total : {reader.GetDecimal(3)}");
            }
            reader.Dispose();
            command.Dispose();
            Configuration.Connection.Close();
        }

        public bool UpdateTotal()
        {
            bool result = false;
            string request = "UPDATE vente set total=@total where id=@id";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@id", Id));
            command.Parameters.Add(new SqlParameter("@total", Total));
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
            string request = "DELETE FROM vente where id=@id";
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
            string retour = $"Id : {Id}, Nom : {Nom}, Téléphone : {Telephone}, Total : {Total} \n";

            return retour;
        }
        public bool SaveVenteProduit(int idVente , int idProduit)
        {
            bool result = false;
            string request = "INSERT INTO vente_produit (id_vente, id_produit) OUTPUT INSERTED.ID" +
                " values (@idv, @idp)";
            command = new SqlCommand(request, Configuration.Connection);
            command.Parameters.Add(new SqlParameter("@idv", idVente));
            command.Parameters.Add(new SqlParameter("@idp", idProduit));
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
    }
}
