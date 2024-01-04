using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
class MetodoPagamento{
    private IWebDriver driver;
    public MetodoPagamento(IWebDriver driver){
        this.driver = driver;
    }
    public void PreencherMetodoPagamento(){
        //Método de Pagamento
        Thread.Sleep(15000);
        //Alterando para Boleto
        driver.FindElement(By.XPath("//*[@id='RadioPaymeMethod|0']/div/div[1]/label[2]/span[1]")).Click();
        Thread.Sleep(12000);//Demora muito alterar pro boleto, então tive que adicionar essa Thread
        IWebElement picklistVencimento = driver.FindElement(By.XPath("//*[@id='CEC_DiadeVencimentoDebitoBoletoCreditoGUI']"));
        new SelectElement(picklistVencimento).SelectByText("5");
        //Avançando pra tela de resumo
        Utils.NextBtn(driver,"Resumo");
    }
}