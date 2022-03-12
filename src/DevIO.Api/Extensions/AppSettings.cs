namespace DevIO.Api.Extensions;

public class AppSettings
{
    public string Secret { get; set; } //Chave de criptografia
    public int ExpiracaoHoras { get; set; } //Expiração em horas de quanto tempo o token vai perder a validade
    public string Emissor { get; set; } //Quem emite o token (no caso é a nossa aplicação)
    public string ValidoEm { get; set; } //Em quais URL's esse token é válido
}