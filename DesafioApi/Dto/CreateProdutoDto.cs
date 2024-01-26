namespace DesafioApi.Dto
{
    public class CreateProdutoDto
    {
        public string Descricao { get; set; }
        public int Preco { get; set; }
        public int Estoque { get; set; }
        public int CategoriaId { get; set; }
    }
}
