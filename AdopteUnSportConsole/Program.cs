﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace AdopteUnSportConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            
            AjouterStock("P01001");
            Console.ReadKey();
        }

        
        //Sécurité
        static void Sécurité()
        {
            Console.WriteLine(" Veuillez renseigner le pseudonyme administrateur : ");
            string Pseudo = Console.ReadLine();
            Console.WriteLine(" Veuillez renseigner le mot de passe administrateur : ");
            string MDP = Console.ReadLine();
            while (Pseudo != "gpadormi" && MDP != "aled")
            {
                Console.Clear();
                Console.WriteLine(" L'association pseudonyme / mot de passe est fausse, veuillez réessayer.");
                Console.WriteLine(" Veuillez renseigner le pseudonyme administrateur : ");
                Pseudo = Console.ReadLine();
                Console.WriteLine(" Veuillez renseigner le mot de passe administrateur : ");
                MDP = Console.ReadLine();
            }
            Console.WriteLine("WAOUH C LE BON COMBINE T TRO CHAUD");
            Console.ReadKey();
        }
    
        //Commande
        static void NouvelleCommande()                                                                                                                              // CA MARCHE
        {
            Console.WriteLine(" Une nouvelle commande vient d'être créer");
            string IDClient = ConnexionClient();
            int NBArticles = 0;
            string IDProduit = AjouterUnArticle();  //Renvoie l'ID d'un produit qui existe
            NBArticles++;
            string IDProduitsCom = IDProduit;   //Enregistrement des ID des produits à ajouter dans la commande
            Console.WriteLine(" Voulez-vous ajouter des articles supplémentaires ?");
            string RéponseArticle = OuiNon();
            while (RéponseArticle == "oui")
            {
                IDProduit = AjouterUnArticle();
                NBArticles++;
                IDProduitsCom += "," + IDProduit;
                Console.WriteLine(" Voulez-vous ajouter des articles supplémentaires ?");
                RéponseArticle = OuiNon();
            }
            Console.WriteLine(" Voici la liste des IDs des produits séléctionnés : " + IDProduitsCom);
            IDProduitsCom = VérifierStockIDProduits(IDProduitsCom, IDClient);
            Console.WriteLine(" Confirmez-vous la commande ? ('Oui' pour confirmer, 'Non' pour annuler)");
            string RConfirmation = OuiNon();
            if (RConfirmation == "oui")
            {
                string[] IDP = IDProduitsCom.Split(',');
                for (int i = 0; i < IDP.Length; i++)
                {
                    SoustraireArticle(IDP[i]);
                }
                EnregistrerCommande(IDClient, NBArticles);
            }            
        }
        static string AjouterUnArticle()                                                                                                                            // CA MARCHE
        {
            Console.WriteLine(" Veuillez renseigner l'ID du produit :");
            string IDProduit = Console.ReadLine();
            bool FindProduit = ExistenceProduit(IDProduit); //Renvoie "true" si le produit existe & "false" si le produit existe pas
            while (FindProduit == false)
            {
                Console.WriteLine(" L'ID précisé n'existe pas, veuillez renseigner un nouvel ID :");
                IDProduit = Console.ReadLine();
                FindProduit = ExistenceProduit(IDProduit);
                //Faire une fonction pour sortir du programme sinon si on a pas d'ID valide, la boucle est infinie
            }
            return IDProduit;
        }
        static void EnregistrerCommande(string IDClient, int NBArticles)                                                                                            // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            string IDCommande = CréationIDCommande();


            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "insert into Commande values ('" + IDCommande + "','" + IDClient + "','" + NBArticles + "')";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine(" La commande a bien été enregistrée.");
            maConnexion.Close();
        }
        static string CréationIDCommande()
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "select count(IDCommande)+1 from Commande";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            string IDCommande = "";
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    IDCommande = reader.GetValue(i).ToString();
                }
            }
            while (IDCommande.Length < 4)
            {
                IDCommande = "0" + IDCommande;
            }
            IDCommande = "C" + IDCommande;
            maConnexion.Close();
            return IDCommande;
        }

        //Client
        static void CréationClient()                                                                                                                                // CA MARCHE
        {
            Console.WriteLine(" Veuillez rentrer les informations suivantes du client :");
            Console.WriteLine(" Nom :");
            string Nom = Console.ReadLine();
            Console.WriteLine(" Prénom :");
            string Prénom = Console.ReadLine();
            Console.WriteLine(" Année de naissance :");
            int AnnéeNaiss = int.Parse(Console.ReadLine());
            Console.WriteLine(" Adresse :");
            string Adresse = Console.ReadLine();
            Console.WriteLine(" Ville :");
            string Ville = Console.ReadLine();
            Console.WriteLine(" Email :");
            string Email = Console.ReadLine();
            EnregistrementClient(Nom, Prénom, AnnéeNaiss, Adresse, Ville, Email);

        }                                                                                                         
        static void EnregistrementClient(string Nom, string Prénom, int AnnéeNaiss, string Adresse, string Ville, string Email)                                     // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            string IDClient = CréationIDClient();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "insert into Clients values ('" + IDClient + "','" + Nom + "','" + Prénom + "','" + AnnéeNaiss + "','" + Adresse + "','" + Ville + "','" + 0 + "','" + Email + "')";
            Console.WriteLine(command.CommandText);
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine("Le client a bien été enregistré.");
            maConnexion.Close();
        }
        static string CréationIDClient()                                                                                                                            // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "select count(IDClients)+1 from Clients";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            string IDClient = "";
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    IDClient = reader.GetValue(i).ToString();
                }
            }
            while (IDClient.Length < 4)
            {
                IDClient = "0" + IDClient;
            }
            IDClient = "A" + IDClient;
            maConnexion.Close();
            return IDClient;
        }                               
        static void InformationsClient()                                                                                                                            // CA MARCHE
        {
            Console.WriteLine("Par quel moyen souhaitez-vous retrouver les informations du client ? (IDClient, Nom, Prénom, AnnéeNaiss, Adresse, Ville, Dépense, Email)");
            string Moyen = Console.ReadLine();
            Moyen = Moyen.ToLower();
            while(Moyen != "idclient" && Moyen != "nom" && Moyen != "prénom" && Moyen != "annéenaiss" && Moyen != "adresse" && Moyen != "ville" && Moyen != "dépense" && Moyen != "email")
            {
                Console.WriteLine(" Veuillez renseigner un moyen valide s'il-vous-plaît : (IDClient, Nom, Prénom, AnnéeNaiss, Adresse, Ville, Dépense, Email)");
                Moyen = Console.ReadLine();
                Moyen = Moyen.ToLower();
            }
            string InfoB = "";
            if (Moyen == "idclient")
            {
                Console.WriteLine("Veuillez renseigner l'ID du client :");
                InfoB = Console.ReadLine();
            }
            if (Moyen == "nom")
            {
                Console.WriteLine("Veuillez renseigner le nom du client :");
                InfoB = Console.ReadLine();
            }
            if (Moyen == "prénom")
            {
                Console.WriteLine("Veuillez renseigner le prénom du client :");
                InfoB = Console.ReadLine();
            }
            if (Moyen == "annéenaiss")
            {
                Console.WriteLine("Veuillez renseigner l'année de naissance du client :");
                InfoB = Console.ReadLine();
            }
            if (Moyen == "adresse")
            {
                Console.WriteLine("Veuillez renseigner l'adresse du client :");
                InfoB = Console.ReadLine();
            }
            if (Moyen == "ville")
            {
                Console.WriteLine("Veuillez renseigner la ville du client :");
                InfoB = Console.ReadLine();
            }
            if (Moyen == "dépense")
            {
                Console.WriteLine("Veuillez renseigner les dépenses du client :");
                InfoB = Console.ReadLine();
            }
            if (Moyen == "email")
            {
                Console.WriteLine("Veuillez renseigner l'email du client :");
                InfoB = Console.ReadLine();
            }
            RetrouverInformationsclient(Moyen, InfoB);
        }
        static void RetrouverInformationsclient(string Moyen, string InfoB)                                                                                         // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();
            string IDClient = ""; string Nom = ""; string Prénom = ""; int dateNaiss = 0; string adresse = ""; string ville = ""; int depenses = 0; string email = "";
            MySqlCommand command = maConnexion.CreateCommand();
            MySqlDataReader reader;
            if (Moyen == "IDClient")
            {
                bool Existence = false;
                Existence = ExistenceIDClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("L'ID Client donné n'existe pas dans les bases de données, veuillez en donner un nouveau :");
                    InfoB = Console.ReadLine();
                    Existence = ExistenceNomClient(InfoB);
                }
                command.CommandText = "select nom , prenom, dateNaiss , adresse, ville, depenses, email from Clients where IDClients = '" + InfoB + "'";
                
                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }                    
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = InfoB;
                Nom = TabInfoClient[0];
                Prénom = TabInfoClient[1];
                dateNaiss = Convert.ToInt32(TabInfoClient[2]);
                adresse = TabInfoClient[3];
                ville = TabInfoClient[4];
                depenses = Convert.ToInt32(TabInfoClient[5]);
                email = TabInfoClient[6];
            }
            if (Moyen == "nom")
            {
                bool Existence = false;
                Existence = ExistenceNomClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("Le nom donné n'existe pas dans les bases de données, veuillez en donner un nouveau :");
                    InfoB = Console.ReadLine();
                    Existence = ExistenceNomClient(InfoB);
                }
                command.CommandText = "select IDClients , prenom, dateNaiss , adresse, ville, depenses, email from Clients where nom = '" + InfoB + "'";

                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = TabInfoClient[0];
                Nom = InfoB;
                Prénom = TabInfoClient[1];
                dateNaiss = Convert.ToInt32(TabInfoClient[2]);
                adresse = TabInfoClient[3];
                ville = TabInfoClient[4];
                depenses = Convert.ToInt32(TabInfoClient[5]);
                email = TabInfoClient[6];
            }
            if (Moyen == "prenom")
            {
                bool Existence = false;
                Existence = ExistenceNomClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("Le prenom donné n'existe pas dans les bases de données, veuillez en donner un nouveau :");
                    InfoB = Console.ReadLine();
                    Existence = ExistencePrenomClient(InfoB);
                }
                command.CommandText = "select IDClients , nom, dateNaiss , adresse, ville, depenses, email from Clients where prenom = '" + InfoB + "'";

                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = TabInfoClient[0];
                Nom = TabInfoClient[1];
                Prénom = InfoB;
                dateNaiss = Convert.ToInt32(TabInfoClient[2]);
                adresse = TabInfoClient[3];
                ville = TabInfoClient[4];
                depenses = Convert.ToInt32(TabInfoClient[5]);
                email = TabInfoClient[6];
            }
            if (Moyen == "annéenaiss")
            {
                bool Existence = false;
                Existence = ExistenceNomClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("La date de naissance donnée n'existe pas dans les bases de données, veuillez en donner une nouvelle :");
                    InfoB = Console.ReadLine();
                    Existence = ExistenceDateNaissClient(InfoB);
                }
                command.CommandText = "select IDClients, nom, prenom , adresse, ville, depenses, email from Clients where dateNaiss = " + InfoB;

                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = TabInfoClient[0];
                Nom = TabInfoClient[1];
                Prénom = TabInfoClient[2];
                dateNaiss = Convert.ToInt32(InfoB);
                adresse = TabInfoClient[3];
                ville = TabInfoClient[4];
                depenses = Convert.ToInt32(TabInfoClient[5]);
                email = TabInfoClient[6];
            }
            if (Moyen == "adresse")
            {
                bool Existence = false;
                Existence = ExistenceAdresseClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("L'adresse donnée n'existe pas dans les bases de données, veuillez en donner une nouvelle :");
                    InfoB = Console.ReadLine();
                    Existence = ExistenceNomClient(InfoB);
                }
                command.CommandText = "select IDClients, nom, prenom , dateNaiss, ville, depenses, email from Clients where adresse = '" + InfoB + "'";

                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = TabInfoClient[0];
                Nom = TabInfoClient[1];
                Prénom = TabInfoClient[2];
                dateNaiss = Convert.ToInt32(TabInfoClient[3]);
                adresse = InfoB;
                ville = TabInfoClient[4];
                depenses = Convert.ToInt32(TabInfoClient[5]);
                email = TabInfoClient[6];
            }
            if (Moyen == "ville")
            {
                bool Existence = false;
                Existence = ExistenceVilleClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("La ville donnée n'existe pas dans les bases de données, veuillez en donner une nouvelle :");
                    InfoB = Console.ReadLine();
                    Existence = ExistenceNomClient(InfoB);
                }
                command.CommandText = "select IDClients, nom, prenom , dateNaiss, adresse, depenses, email from Clients where ville = '" + InfoB + "'";

                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = TabInfoClient[0];
                Nom = TabInfoClient[1];
                Prénom = TabInfoClient[2];
                dateNaiss = Convert.ToInt32(TabInfoClient[3]);
                adresse = TabInfoClient[4];
                ville = InfoB;
                depenses = Convert.ToInt32(TabInfoClient[5]);
                email = TabInfoClient[6];
            }
            if (Moyen == "dépense")
            {
                bool Existence = false;
                Existence = ExistenceDepensesClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("Les dépenses données n'existent pas dans les bases de données, veuillez en donner de nouvelles :");
                    InfoB = Console.ReadLine();
                    Existence = ExistenceNomClient(InfoB);
                }
                command.CommandText = "select IDClients, nom, prenom , dateNaiss, adresse, ville, email from Clients where depenses = " + InfoB;

                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = TabInfoClient[0];
                Nom = TabInfoClient[1];
                Prénom = TabInfoClient[2];
                dateNaiss = Convert.ToInt32(TabInfoClient[3]);
                adresse = TabInfoClient[4];
                ville = TabInfoClient[5];
                depenses = Convert.ToInt32(InfoB);
                email = TabInfoClient[6];
            }
            if (Moyen == "email")
            {
                bool Existence = false;
                Existence = ExistenceEmailClient(InfoB);
                while (Existence == false)
                {
                    Console.WriteLine("L'email donné n'existe pas dans les bases de données, veuillez en donner un nouveau :");
                    InfoB = Console.ReadLine();
                    Existence = ExistenceNomClient(InfoB);
                }
                command.CommandText = "select IDClients, nom, prenom , dateNaiss, adresse, ville, depenses from Clients where email = '" + InfoB + "'";

                reader = command.ExecuteReader();
                string InfoClient = "";
                while (reader.Read())       // parcours
                {
                    InfoClient = "";
                    for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(i).ToString();
                        InfoClient += valeurattribut + ",";
                    }
                }
                string[] TabInfoClient = InfoClient.Split(',');
                IDClient = TabInfoClient[0];
                Nom = TabInfoClient[1];
                Prénom = TabInfoClient[2];
                dateNaiss = Convert.ToInt32(TabInfoClient[3]);
                adresse = TabInfoClient[4];
                ville = TabInfoClient[5];
                depenses = Convert.ToInt32(TabInfoClient[6]);
                email = InfoB;
            }
            maConnexion.Close();
            AffichageInfoClient(IDClient, Nom, Prénom, dateNaiss, adresse, ville, depenses, email);
        }
        static void AffichageInfoClient(string IDClient, string Nom, string Prénom, int DateNaiss, string Adresse, string Ville, int Dépenses, string Email)        // CA MARCHE
        {
            Console.Clear();
            Console.WriteLine("     Voici les informations du client :");
            Console.WriteLine("");
            Console.WriteLine(" IDClient : " + IDClient);
            Console.WriteLine(" Nom : " + Nom);
            Console.WriteLine(" Prénom : " + Prénom);
            Console.WriteLine(" DateNaiss : " + DateNaiss);
            Console.WriteLine(" Adresse : " + Adresse);
            Console.WriteLine(" Ville : " + Ville);
            Console.WriteLine(" Dépenses : " + Dépenses);
            Console.WriteLine(" Email : " + Email);
        }
        static void AffichageInfoMeilleurClient(string IDClient, string Nom, string Prénom, int Dépenses)                                                           // CA MARCHE
        {
            Console.Clear();
            Console.WriteLine("     Voici les informations du meilleur client :");
            Console.WriteLine("");
            Console.WriteLine(" IDClient : " + IDClient);
            Console.WriteLine(" Nom : " + Nom);
            Console.WriteLine(" Prénom : " + Prénom);
            Console.WriteLine(" Dépenses : " + Dépenses + " euros");
        }
        static string ConnexionClient()                                                                                                                             // CA MARCHE
        {
            Console.WriteLine(" Est-ce que le client a déjà un compte existant ?");
            string RéponseClient1 = OuiNon();
            string IDClient = "";
            while (IDClient == "")
            {
                if (RéponseClient1 == "oui")
                {
                    Console.WriteLine(" Avez-vous l'ID du client ?");
                    string RéponseClient2 = OuiNon();                    
                    if (RéponseClient2 == "oui")
                    {
                        bool ExistenceClient = false;
                        Console.WriteLine(" Veuillez renseigner l'ID du client");
                        IDClient = Console.ReadLine();
                        ExistenceClient = ExistenceIDClient(IDClient);
                        while (ExistenceClient == false)
                        {
                            Console.WriteLine(" Veuillez renseigner un IDClient valide s'il-vous-plaît :");
                            IDClient = Console.ReadLine();
                            ExistenceClient = ExistenceIDClient(IDClient);
                        }
                        Console.Clear();
                        Console.WriteLine(" Le client a bien été trouvé.");
                    }
                    else
                    {
                        InformationsClient();
                    }
                }
                else
                {
                    CréationClient();
                    RéponseClient1 = "oui";
                }
            }
            return IDClient;
        }
        
        //Produit
        static void AjouterStock(string IDProduit)                                                                                                                  // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            Console.WriteLine(" Quelle quantité voulez-vous ajouter au stock du produit ?");
            int qte = int.Parse(Console.ReadLine());
            command.CommandText = "UPDATE Produit SET stock = stock + " + qte + " WHERE IDProduit = '" + IDProduit + "'";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine(" Le stock du produit " + IDProduit + " a été augmenté de " + qte + " avec succès.");
            maConnexion.Close();
        }
        static void InformationProduit()                                                                                                                            // CA MARCHE
        {
            Console.WriteLine("             Information d'un produit");
            Console.WriteLine();
            string IDProduit = AjouterUnArticle();
            RetrouverInformationsProduit(IDProduit);
        }
        static void RetrouverInformationsProduit(string IDProduit)                                                                                                  // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();
            string IDFournisseur = ""; int prix = 0; int stock = 0; string objet = "";
            MySqlCommand command = maConnexion.CreateCommand();
            MySqlDataReader reader;

            command.CommandText = "select IDFournisseur, prix , stock, objet from Produit where IDProduit = '" + IDProduit + "'";

            reader = command.ExecuteReader();
            string InfoProduit = "";
            while (reader.Read())       // parcours ligne par ligne
            {
                InfoProduit = "";
                for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                {
                    string valeurattribut = reader.GetValue(i).ToString();
                    InfoProduit += valeurattribut + ",";
                }
            }
            string[] TabInfoProduit = InfoProduit.Split(',');            
            IDFournisseur = TabInfoProduit[0];
            prix = Convert.ToInt32(TabInfoProduit[1]);
            stock = Convert.ToInt32(TabInfoProduit[2]);
            objet = TabInfoProduit[3];
            maConnexion.Close();
            AffichageInfoProduit(IDProduit, IDFournisseur, prix, stock, objet);
        }
        static void AffichageInfoProduit(string IDProduit, string IDFournisseur, int prix, int stock, string objet)                                                 // CA MARCHE
        {
            Console.Clear();
            Console.WriteLine("     Voici les informations du produit :");
            Console.WriteLine("");
            Console.WriteLine(" IDProduit : " + IDProduit);
            Console.WriteLine(" IDFournisseur : " + IDFournisseur);
            Console.WriteLine(" prix : " + prix + " Euros");
            Console.WriteLine(" stock : " + stock);
            Console.WriteLine(" description du produit : " + objet);

        }

        //Livraison
        static void EnregisterLivraison(string IDClient, string IDProduit, string IDFournisseur)                                                                    // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            int numLivraison = CréationNumLivraison();
            string InformationClient = RécupérationInformationClient(IDClient);
            string[] TabInformationClient = InformationClient.Split(',');
            string nom = TabInformationClient[0];
            string Prénom = TabInformationClient[1];
            string Adresse = TabInformationClient[2];
            string Ville = TabInformationClient[3];
            
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "insert into Livraison values ('" + numLivraison + "','" + IDClient + "','" + nom + "','" + Prénom + "','" + Adresse + "','" + Ville + "','" + IDProduit + "','" + IDFournisseur + "')";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.Clear();
            Console.WriteLine("La livraison a bien été enregistrée.");
            maConnexion.Close();
        }
        static int CréationNumLivraison()                                                                                                                           // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();
            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "select count(numLivraison)+1 from Livraison";
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            string SnumLivraison = "";
            int numLivraison = 0;
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    SnumLivraison = reader.GetValue(i).ToString();
                }
            }
            numLivraison = Convert.ToInt32(SnumLivraison);
            
            maConnexion.Close();
            return numLivraison;
        }
        static string RécupérationInformationClient(string IDClient)                                                                                                // CA MARCHE
        {
            string InformationClient = "";

            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT nom, prenom, adresse, ville from Clients where IDClients = '" + IDClient + "'"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    InformationClient += ligne + ",";
                }
            }
            maConnexion.Close();
            InformationClient = InformationClient.Substring(0, InformationClient.Length - 1);
            return InformationClient;
        }
        static void SelectionFournisseur(string IDProduit)                                                                                                          // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();
            MySqlCommand command = maConnexion.CreateCommand();
            MySqlDataReader reader;
            command.CommandText = "select IDFournisseur from Produit where IDProduit = '" + IDProduit + "';";
            reader = command.ExecuteReader();
            maConnexion.Close();
        }

        //Autre
        static void MeilleurClient()                                                                                                                                // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();         
            MySqlCommand command = maConnexion.CreateCommand();
            MySqlDataReader reader;
            command.CommandText = "select depenses, nom, prenom, IDClients from Clients where depenses >= all (select depenses from Clients);";
            reader = command.ExecuteReader();
            int dépense = 0;  string InfoClient = ""; string nom = ""; string prenom = ""; string IDClients = ""; 
            while (reader.Read())       // parcours ligne par ligne
            {
                InfoClient = "";
                for (int i = 0; i < reader.FieldCount; i++)  //parcours cellule par cellule
                {
                    string valeurattribut = reader.GetValue(i).ToString();
                    InfoClient += valeurattribut + ",";
                }
            }
            string[] TabInfoClient = InfoClient.Split(',');
            dépense = Convert.ToInt32(TabInfoClient[0]);
            nom = TabInfoClient[1];
            prenom = TabInfoClient[2];
            IDClients = TabInfoClient[3];
            AffichageInfoMeilleurClient(IDClients, nom, prenom, dépense);
            maConnexion.Close();
        }
        
        //Fonctions outils
        static bool ExistenceProduit(string IDProduit)                                                                                                              // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT IDProduit from Produit"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == IDProduit)
                    {
                        Existence = true;
                        Console.WriteLine(" Le produit a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static string OuiNon()                                                                                                                                      // CA MARCHE
        {
            string Réponse = Console.ReadLine();
            Réponse = Réponse.ToLower();
            while (Réponse != "oui" && Réponse != "non")
            {
                Console.WriteLine("\n   Il y a eu une erreur de compréhension, veuillez renseigner de nouveau par 'oui' ou 'non' s'il-vous-plaît: ");
                Réponse = Console.ReadLine();
                Réponse = Réponse.ToLower();
            }
            Console.Clear();
            return Réponse;
        } 
        static void SoustraireArticle(string IDProduit)                                                                                                             // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "UPDATE Produit SET stock = stock - 1 WHERE IDProduit = '"+IDProduit+"'";    
            MySqlDataReader reader;
            reader = command.ExecuteReader();
            Console.WriteLine(" Le stock du produit " + IDProduit + " a été baissé de 1 avec succès.");
            maConnexion.Close();
        }

        static bool ExistenceIDClient(string IDClients)                                                                                                             // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT IDClients from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == IDClients)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static bool ExistenceNomClient(string nom)                                                                                                                  // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT nom from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == nom)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static bool ExistencePrenomClient(string prenom)                                                                                                            // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT prenom from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == prenom)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static bool ExistenceDateNaissClient(string dateNaiss)                                                                                                      // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT dateNaiss from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == dateNaiss)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static bool ExistenceAdresseClient(string adresse)                                                                                                          // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT adresse from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == adresse)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static bool ExistenceVilleClient(string ville)                                                                                                              // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT ville from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == ville)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static bool ExistenceDepensesClient(string depenses)                                                                                                        // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT depenses from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == depenses)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }
        static bool ExistenceEmailClient(string email)                                                                                                              // CA MARCHE
        {
            bool Existence = false;
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "SELECT email from Clients"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    if (ligne == email)
                    {
                        Existence = true;
                        Console.WriteLine(" Le client a été trouvé.");
                    }
                }
            }
            maConnexion.Close();
            return Existence;
        }

        static string VérifierStockIDProduits(string ListeIDProduits, string IDClient)                                                                              // CA MARCHE
        {
            string[] TabIDProduits = ListeIDProduits.Split(',');

            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            
            int stock = 0; string Livraison = ""; string ProduitDispo = "";
            
            for (int i = 0; i < TabIDProduits.Length; i++)
            {
                maConnexion.Open();
                MySqlCommand command = maConnexion.CreateCommand();
                MySqlDataReader reader;
                command.CommandText = "select IDProduit, objet , stock from Produit where IDProduit = '" + TabIDProduits[i] + "'";

                reader = command.ExecuteReader();
                string InfoProduit = "";
                while (reader.Read())       // parcours ligne par ligne
                {
                    InfoProduit = "";
                    for (int j = 0; j < reader.FieldCount; j++)  //parcours cellule par cellule
                    {
                        string valeurattribut = reader.GetValue(j).ToString();
                        InfoProduit += valeurattribut + ",";
                    }
                }
                string[] TabInfoProduit = InfoProduit.Split(',');
                stock = Convert.ToInt32(TabInfoProduit[2]);
                if (stock == 0)
                {
                    Livraison += i + 1 + ",";
                }
                else ProduitDispo += i + ",";
                Console.WriteLine(TabInfoProduit[0] + "    " + TabInfoProduit[2] + "    " + TabInfoProduit[1]);
                maConnexion.Close();
            }
            if (Livraison != "")
            {
                Livraison = Livraison.Substring(0, Livraison.Length - 1);
                if (Livraison.Length == 1)
                {
                    Console.WriteLine(" L'article " + Livraison + " n'est plus disponible en magasin.");
                    Console.WriteLine(" Est-ce que le client veut se le faire livrer à son domicile ?");
                }
                else
                {
                    Console.WriteLine(" Les articles " + Livraison + " ne sont plus disponibles en magasin.");
                    Console.WriteLine(" Est-ce que le client veut se les faire livrer à son domicile ?");
                }                
                string RéponseLivraison = OuiNon();
                if (RéponseLivraison == "oui")
                {                    
                    string[] tabProduitLivraison = Livraison.Split(',');
                    int InterTab;
                    Livraison = "";
                    for (int a = 0; a < tabProduitLivraison.Length; a++)
                    {
                        InterTab = Convert.ToInt32(tabProduitLivraison[a]);
                        InterTab--;
                        Livraison += TabIDProduits[InterTab] + ",";
                    }
                    LivraisonProduits(Livraison, IDClient);
                }
                else
                {
                    ProduitDispo = ProduitDispo.Substring(0, ProduitDispo.Length - 1);
                    ListeIDProduits = "";
                    string[] TabProduitDispo = ProduitDispo.Split(',');                    
                    int TabInter;
                    for (int k = 0; k < TabProduitDispo.Length; k++)
                    {
                        TabInter = Convert.ToInt32(TabProduitDispo[k]);
                        ListeIDProduits += TabIDProduits[TabInter] +",";
                    }
                    ListeIDProduits = ListeIDProduits.Substring(0, ListeIDProduits.Length - 1);
                }                    
            }
            Console.WriteLine(" La commande contient maintenant les articles : " + ListeIDProduits);
            return ListeIDProduits;
            
        }
        static void LivraisonProduits(string Produits, string IDClient)                                                                                             // CA MARCHE
        {
            Produits = Produits.Substring(0, Produits.Length - 1);
            Console.WriteLine(" Voici l'ID des produits à livrer : " + Produits);
            string AdresseClient = RetrouverAdresse(IDClient);
            Console.WriteLine(AdresseClient);
            string[] tabProduits = Produits.Split(',');
            string IDFournisseur; string ListeFournisseur = "";
            for (int i = 0; i < tabProduits.Length; i++)
            {
                IDFournisseur = RetrouverFournisseur(tabProduits[i]);
                ListeFournisseur += IDFournisseur + ",";
                EnregisterLivraison(IDClient, tabProduits[i], IDFournisseur);
            }
            if (ListeFournisseur.Length == 6)
            {
                ListeFournisseur = ListeFournisseur.Substring(0, ListeFournisseur.Length - 1);
                Console.WriteLine(" Le fournisseur " + ListeFournisseur + " a été contacté.");
            }
            else
            {
                ListeFournisseur = ListeFournisseur.Substring(0, ListeFournisseur.Length - 1);
                Console.WriteLine(" Les fournisseurs " + ListeFournisseur + " ont été contactés.");
            }
        }
        static string RetrouverAdresse(string IDClient)                                                                                                             // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "select adresse, ville from Clients where IDClients = '" + IDClient + "'"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            string AdresseClient = "";
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string ligne = reader.GetValue(i).ToString();
                    AdresseClient += ligne + ",";
                }
            }
            maConnexion.Close();
            string[] TabAdresseClient = AdresseClient.Split(',');
            AdresseClient = " L'adresse du client est " + TabAdresseClient[0] +", " + TabAdresseClient[1] + ".";
            return AdresseClient;
        }
        static string RetrouverFournisseur(string IDProduit)                                                                                                        // CA MARCHE
        {
            string infoConnexion = "SERVER = localhost; PORT = 3306; DATABASE = magasinAdopteUnSport; UID = root; PASSWORD = MATIbol78;";
            MySqlConnection maConnexion = new MySqlConnection(infoConnexion);
            maConnexion.Open();

            MySqlCommand command = maConnexion.CreateCommand();
            command.CommandText = "select IDFournisseur from Produit where IDProduit = '" + IDProduit + "'"; // exemple de requête

            MySqlDataReader reader;
            reader = command.ExecuteReader();
            string IDFournisseur = "";
            while (reader.Read())
            {
                IDFournisseur = reader.GetValue(0).ToString();
            }
            
            maConnexion.Close();
            return IDFournisseur;
        }
    }
}