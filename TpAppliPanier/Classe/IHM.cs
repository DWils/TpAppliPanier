using System;
using System.Text;
using System;
using System.Data.SqlClient;
using TpAppliPanier.Classe;

namespace TpAppliPanier
{
    public class IHM
    {
        

        public void Menu()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("1---Acheter un produit");
            Console.WriteLine("2---Afficher les ventes");
            Console.WriteLine("3---Afficher les produits");
            Console.WriteLine("4---Ajouter un produit");
        }

        public void Start()
        {
            string choix;
            do
            {
                Menu();
                choix = Console.ReadLine();
                switch (choix)
                {
                    case "1":
                        AcheterProduit();
                        break;
                    case "2":
                        AfficherVentes();
                        break;
                    case "3":
                        AfficherProduits();
                        break;
                    case "4":
                        AjouterProduit();
                        break;
                }
            }
            while (choix != "0");
        }

        public void AjouterProduit()
        {
            Console.Write("Nom de votre produit : ");
            string label = Console.ReadLine();
            if(Produit.GetProduitByLabel(label) != null)
            {
                Console.WriteLine("Label déjà utilisé");
            }
            else
            {
                Console.Write("Prix du produit ?");
                decimal prix;
                Decimal.TryParse(Console.ReadLine(), out prix);
                Produit produit = new Produit() { Label = label , Prix = prix };
                if (produit.Save(produit))
                    Console.WriteLine("Produit ajouté");
            }

        }

        public void AcheterProduit()
        {
            Console.Write("Le téléphone du contact ? ");
            string telephone = Console.ReadLine();
            Vente vente = Vente.GetVenteByTelephone(telephone);
            if (vente != null)
            {
                Console.WriteLine("Numéro de téléphone déjà enregistré");
                RegistreAchat(vente);
            }
            else
            {
                Console.Write("Nom de l'acheteur ?");
                string nom = Console.ReadLine();
                vente = new Vente{ Nom = nom, Telephone = telephone, Total = 0 };
                if (vente.Save())
                {
                    RegistreAchat(vente);                   
                }
            }
        }

        public void AfficherVentes()
        {
            Vente.GetAllVentes();
        }

        public void AfficherProduits()
        {
            Produit.GetAllProduits();
        }

        public void RegistreAchat(Vente vente)
        {
            string again;
            int choixProduit;
            do
            {
                Console.WriteLine("Que souhaitez-vous acheter ?");
                Produit.GetAllProduits();
                Produit produitChoisi;
                Int32.TryParse(Console.ReadLine(), out choixProduit);
                produitChoisi = Produit.GetProduitById(choixProduit);
                if ( produitChoisi != null)
                {
                    vente.SaveVenteProduit(vente.Id, choixProduit);
                    vente.Total += produitChoisi.Prix;
                    if (vente.Update())
                    {
                        Console.WriteLine("Total modifé");
                    }

                }
                else
                {
                    Console.WriteLine("Produit Introuvable");
                }
                
                

                Console.WriteLine("Voulez vous autre chose ? oui/non");
                again = Console.ReadLine();
            } while (again == "oui");

            Console.WriteLine("Registre de vente mis à jour");
        }

        

    }
}
