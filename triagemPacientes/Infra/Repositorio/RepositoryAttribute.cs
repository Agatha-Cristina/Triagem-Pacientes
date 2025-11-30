using System;

namespace triagemPacientes.Infra.Repositorio
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RepositoryAttribute : Attribute
    {
        // Nome da collection no MongoDB
        public string CollectionName { get; }

        // Nome do arquivo CSV (opcional)
        public string FileName { get; set; }

        public RepositoryAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
