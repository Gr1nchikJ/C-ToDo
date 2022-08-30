namespace ToDo.Captcha
{
    public interface ICaptchaValidator
    {
        Task<bool> IsCaptchaPassedAsync(string token);
    }
}
