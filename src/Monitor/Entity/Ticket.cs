namespace Monitor.Entity;

public class Ticket
{
    public int Id { get; set; }
    public string Responsavel { get; set; } = null!;
    public int CanalDeComunicacaoTipo { get; set; }
    public string Mensagem { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool Arquivado { get; set; }
    public string Categoria { get; set; } = null!;
}