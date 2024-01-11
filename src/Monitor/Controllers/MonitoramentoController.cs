using System.Reflection;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Monitor.Banco;
using Monitor.Entity;

namespace Monitor.Controllers;

[ApiController]
[Route("[controller]")]
public class MonitoramentoController : ControllerBase
{
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TrabalhandoMenssagem _trabalhandoMenssagem;
    private TesteBanco _testeBanco;

    public MonitoramentoController()
    {
        _trabalhandoMenssagem = new TrabalhandoMenssagem();
        _cancellationTokenSource = new CancellationTokenSource();
        _testeBanco = new TesteBanco();
        Task.Run(() => CheckEmailsAsync(_cancellationTokenSource.Token));
    }

    [HttpGet]
    public IActionResult StopCheckingEmails()
    {
        _cancellationTokenSource.Cancel();
        return Ok("Parou");
    }

    private async Task CheckEmailsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using (var client = new ImapClient())
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate("neosolution2022@gmail.com", "dmgd yeol xpyu osdb");

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);

                    var uids = inbox.Search(SearchQuery.NotSeen);

                    var ticket = new Ticket();

                    foreach (var uid in uids)
                    {
                        var message = inbox.GetMessage(uid);
                        inbox.AddFlags(uid, MessageFlags.Seen, true);
                        var text = _trabalhandoMenssagem.GetMessageText(message);

                        ticket.Id = _testeBanco.ListaTickets.Count + 1;
                        var remetente = message.From.Mailboxes.FirstOrDefault();
                        ticket.Responsavel = remetente!.Address;
                        ticket.Mensagem = text;
                        ticket.Arquivado = false;
                        ticket.CanalDeComunicacaoTipo = 1;
                        ticket.CreatedAt = DateTime.Now.Date;
                        ticket.Categoria = message.Subject;
                        _testeBanco.ListaTickets.Add(ticket);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                foreach (var tik in _testeBanco.ListaTickets)
                {
                    MostrarPropriedades(tik);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar emails: {ex.Message}");
            }
        }
    }
    
    private void MostrarPropriedades(object entidade)
    {
        Type tipoEntidade = entidade.GetType();
        PropertyInfo[] propriedades = tipoEntidade.GetProperties();

        foreach (var propriedade in propriedades)
        {
            string nomePropriedade = propriedade.Name;
            object valorPropriedade = propriedade.GetValue(entidade);

            Console.WriteLine($"{nomePropriedade}: {valorPropriedade}");
        }
    }
}