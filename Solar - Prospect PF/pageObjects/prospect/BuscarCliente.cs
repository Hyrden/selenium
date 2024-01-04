using OpenQA.Selenium;

public class BuscarCliente
{
    private IWebDriver driver;

    public BuscarCliente(IWebDriver driver)
    {
        this.driver = driver;
    }

    public void RealizarBuscaCliente()
    {
        // Tempo máximo para o Aura detectar o perfil do usuário logado e modificar a tela de busca
        Thread.Sleep(2000);

        IWebElement cpfInput = Utils.WaitUntilVisible(driver, 3, 200, "//input[contains(@id, 'input-')]");
        cpfInput.SendKeys(Data.PROSPECT_CPF);
        try{
            InteractButtons();
        }catch(Exception ex){
            InteractButtons();
        }
    }
    private void InteractButtons(){

        IWebElement buscarBtn = Utils.WaitUntilVisible(driver, 1, 200, "//button[text()='Buscar']");
        buscarBtn.Click();

        IWebElement iniciarBtn = Utils.WaitUntilVisible(driver, 6, 200, "//button[normalize-space(text())='Iniciar']");
        iniciarBtn.Click();
    }
}