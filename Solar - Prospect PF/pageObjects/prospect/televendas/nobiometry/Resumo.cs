using OpenQA.Selenium;
class Resumo{
    private IWebDriver driver;
    public Resumo(IWebDriver driver){
        this.driver = driver;
    }
    public void PreencherResumo(){
        Thread.Sleep(18000);
        //Tela de Resumo
        Utils.TakeScreenshot(driver,"evidencia_tela_resumo",null);
        //Botão de Enviar Protocolo (precisa ser clicado até desaparecer)
        IWebElement botao = driver.FindElement(By.XPath("//*[@id='IPA_SOLAR_SendCommunication']"));
        // Loop para clicar no botão até que ele desapareça
        while (botao.Displayed)
        {
            // Clica no botão
            botao.Click();
            Console.WriteLine("Clicou no botão!");

            // Espera 10 segundos entre cliques
            Thread.Sleep(10000);
        }
        Utils.TakeScreenshot(driver,"evidencia_tela_resumo_2",null);
        //Envia o pedido pro OM
        driver.FindElement(By.XPath("//*[@id='StepResumoDoPedido_redirectBtn0']/p")).Click();
        Thread.Sleep(15000);            
        Utils.TakeScreenshot(driver,"evidencia_tela_order",null);
    }
}