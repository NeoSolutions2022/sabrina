using MimeKit;

namespace Monitor.Controllers;

public class TrabalhandoMenssagem
{
    public string GetMessageText(MimeMessage message)
    {
        var body = message.Body;

        if (body is TextPart textPart)
        {
            return textPart.Text;
        }
        
        if (body is Multipart multipart)
        {
            foreach (var part in multipart)
            {
                if (part is TextPart text)
                {
                    return text.Text;
                }
            }
        }
        
        return string.Empty;
    }
}