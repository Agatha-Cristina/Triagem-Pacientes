using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triagemPacientes.Infra.Repositorio
{
    public class CsvRepository<T> where T : class, new()
    {
        private readonly string _filePath;
        private readonly CsvConfiguration _config = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        public CsvRepository(string filePath)
        {
            _filePath = filePath;
            EnsureFileExists();
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                using var writer = new StreamWriter(_filePath);
                using var csv = new CsvWriter(writer, _config);
                csv.WriteHeader<T>();
                writer.WriteLine();
            }
        }

        public List<T> GetAll()
        {
            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, _config);
            return csv.GetRecords<T>().ToList();
        }

        public void SaveAll(List<T> items)
        {
            using var writer = new StreamWriter(_filePath, false);
            using var csv = new CsvWriter(writer, _config);
            csv.WriteHeader<T>();
            csv.NextRecord();
            csv.WriteRecords(items);
        }

        public void Add(T item)
        {
            var all = GetAll();

            // ID automático (se tiver "Id")
            var idProp = typeof(T).GetProperty("Id");
            if (idProp != null && idProp.PropertyType == typeof(int))
            {
                int maxId = all.Any() ? all.Max(x => (int)idProp.GetValue(x)!) : 0;
                idProp.SetValue(item, maxId + 1);
            }

            all.Add(item);
            SaveAll(all);
        }

        public void Delete(Func<T, bool> predicate)
        {
            var all = GetAll();
            var filtered = all.Where(x => !predicate(x)).ToList();
            SaveAll(filtered);
        }

        public void Update(Func<T, bool> predicate, Action<T> updateAction)
        {
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
