using System.Text.Json.Serialization;

namespace DesafioApi.Entidades
{
    public class Produto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Preco { get; set; }
        public int Estoque { get; set;}
        public Categoria Categoria { get; set;}

        [JsonIgnore]
        public int CategoriaId { get; set; }
    }
}
