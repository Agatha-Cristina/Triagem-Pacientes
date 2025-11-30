using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using triagemPacientes.Services;

namespace triagemPacientes.Infra.Repositorio
{
    // Generic MongoDB repository. Implements the same methods used by the CSV repository via IRepository<T>.
    public class MongoRepository<T> : IRepository<T> where T : class, new()
    {
        private readonly IMongoCollection<T> _collection;

        // Novo construtor usando IConfiguration e collectionName
        public MongoRepository(IConfiguration configuration, string collectionName)
        {
            var connectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
            var databaseName = configuration["DataBaseName"] ?? "TriagemDB";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<T>(collectionName);
        }

        // Construtor alternativo: apenas IConfiguration, tenta obter o collectionName via atributo Repository
        public MongoRepository(IConfiguration configuration) : this(configuration, GetCollectionNameFromAttribute())
        {
        }

        private static string GetCollectionNameFromAttribute()
        {
            var attr = (RepositoryAttribute?)Attribute.GetCustomAttribute(typeof(T), typeof(RepositoryAttribute));
            if (attr != null && !string.IsNullOrWhiteSpace(attr.CollectionName))
                return attr.CollectionName;
            // fallback: tipo em minusculas
            return typeof(T).Name.ToLower();
        }

        public List<T> GetAll()
        {
            return _collection.Find(Builders<T>.Filter.Empty).ToList();
        }

        public void Add(T item)
        {
            _collection.InsertOne(item);
        }

        public void SaveAll(List<T> items)
        {
            // replace the collection content: delete all then insert many
            _collection.DeleteMany(Builders<T>.Filter.Empty);
            if (items != null && items.Any())
            {
                _collection.InsertMany(items);
            }
        }

        public void Delete(Func<T, bool> predicate)
        {
            // Interface uses Func<T,bool>, so perform in-memory filtering
            var all = GetAll();
            var filtered = all.Where(x => !predicate(x)).ToList();
            SaveAll(filtered);
        }

        public void Update(Func<T, bool> predicate, Action<T> updateAction)
        {
            // Perform in-memory update then persist
            var all = GetAll();
            foreach (var item in all.Where(predicate))
            {
                updateAction(item);
            }
            SaveAll(all);
        }

        public List<T> Buscar(Func<T, bool> filtro)
        {
            var todos = GetAll();
            return todos.Where(filtro).ToList();
        }
    }
}
