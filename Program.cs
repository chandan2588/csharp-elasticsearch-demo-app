using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nest;
using Nest.Resolvers;
using Nest.DSL.Query.Behaviour;
using Nest.Domain;
using Nest.SerializationExtensions;
using Nest.DSL;
using Nest.Resolvers.Converters;
using Nest.DSL.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ESConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(uri).SetDefaultIndex("contacts");
            var client = new ElasticClient(settings);


            if (client.ClusterHealth().ConnectionStatus.Success)
            {
                Console.WriteLine("Connection Successful");

                //search or retriving data from Elastic search
                //var c = client.Get<contacts>("AUxsTEkTbGF_NoCk0psr");
                //var response2 = client.Get<contacts>(g => g.Id("AUxsTEkTbGF_NoCk0psr"));
                //var name = response2.Fields.FieldValues<string>(p => p.name);


                //Pushing data into elastic search
                if (client.IndexExists("contacts").Exists)
                {
                    Console.WriteLine("Index Exists");
                    Program.UpsertArticle(client, new Article("The Last Airbender", "Siddharth"), "blog", "article", 1);
                    Program.UpsertContact(client, new contacts("Siddharth Mehta", "India"), "contacts", "contacts", 2);
                    Console.WriteLine("Data Indexed Successfully");
                }
                else
                {
                    Program.UpsertArticle(client, new Article("The Last Airbender", "Siddharth"), "blog", "article", 1);
                    Program.UpsertContact(client, new contacts("Siddharth Mehta", "India"), "contacts", "contacts", 2);

                    Console.WriteLine("Index Created and Data Indexed Successfully");
                }

            }
            else
            {
                Console.Write("Connection Failed");
            }

            Console.ReadKey();

        }

        public class Article
        {
            public string title { get; set; }
            public string artist { get; set; }
            public Article(string Title, string Artist)
            {
                title = Title; artist = Artist;
            }
        }

        public class contacts
        {
            public string name { get; set; }
            public string country { get; set; }
            public contacts(string Name, string Country)
            {
                name = Name; country = Country;
            }
        }

        public static void UpsertArticle(ElasticClient client, Article article, string index, string type, int id)
        {
            var RecordInserted = client.Index(article).Id;

            if (RecordInserted.ToString() != "")
            {
                Console.WriteLine("Transaction Successful !");
            }
            else
            {
                Console.WriteLine("Transaction Failed");
            }
        }

        public static void UpsertContact(ElasticClient client, contacts contact, string index, string type, int id)
        {
            var RecordInserted = client.Index(contact).Id;

            if (RecordInserted.ToString() != "")
            {
                Console.WriteLine("Transaction Successful !");
            }
            else
            {
                Console.WriteLine("Transaction Failed");
            }
        }
    }
}