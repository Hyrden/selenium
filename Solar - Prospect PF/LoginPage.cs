using OpenQA.Selenium;
public class LoginPage
{
    private IWebDriver driver;

    // Construtor que recebe o driver como parâmetro
    public LoginPage(IWebDriver driver)
    {
        this.driver = driver;
    }

    // Mapeamento dos elementos da página
    private IWebElement UserNameField => driver.FindElement(By.Id("username"));
    private IWebElement PasswordField => driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => driver.FindElement(By.Id("Login"));

    // Métodos para interagir com os elementos da página
    public void EnterUserName(string userName)
    {
        UserNameField.SendKeys(userName);
    }

    public void EnterPassword(string password)
    {
        PasswordField.SendKeys(password);
    }

    public void ClickLoginButton()
    {
        LoginButton.Click();
    }

    // Método para verificar se está na página de login
    public bool IsLoginPage()
    {
        try
        {
            // Verificar a presença de algum elemento exclusivo da página de login
            return UserNameField.Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
}