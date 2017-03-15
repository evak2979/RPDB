using System;
using System.Diagnostics;
using System.Linq;
using Nest;
using RPDB.Domain.Models;

namespace RODB.ElasticSearch
{
    public static class ElasticClientFactory
    {
        private static ElasticClient Client { get; set; }


        public static ElasticClient GetClient()
        {
            if (Client == null)
            {
                Client = new ElasticClient(
                    new ConnectionSettings(CreateUri(9200))
                        .DefaultIndex("rpdb"));
            }
            DeleteIndexIfExists();
            return Client;
        }

        //public static CreateIndexDescriptor()
        //{
        //    Client.CreateIndex("rpdb", i => i.Mappings(
        //        m => m.Map<User>(
        //                map => map
        //                .AutoMap()
        //                .Properties(ps =>
        //                    ps
        //                    .Nested<Character>(nn => nn
        //                                       .Name(pv => "characters")
        //                                       .AutoMap()))
        //                )
        //        ))
                
        //}

        static void DeleteIndexIfExists()
        {
            if (Client.IndexExists("rpdb").Exists)
                Client.DeleteIndex("rpdb");
        }

        public static Uri CreateUri(int port)
        {
            var host = "localhost";
            if (Process.GetProcessesByName("fiddler").Any())
                host = "ipv4.fiddler";

            return new Uri("http://" + host + ":" + port);
        }
    }
}
