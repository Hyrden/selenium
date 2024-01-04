using OpenQA.Selenium;
class Carrinho{
    private IWebDriver driver;
    public Carrinho(IWebDriver driver){
        this.driver = driver;
    }
    public void PreencherCarrinho(){
        //Tela de Carrinho
        IWebElement btnFecharModal = Utils.WaitUntilVisible(driver,35,1000,"//*[@id='modal-content-id-1']/div[2]/span[2]/button");
        btnFecharModal.Click();
        //Aviso: dependendo de como esteja o catálogo, ajustes deverão ser feitos
        Thread.Sleep(10000);//O WaitUntilVisible de baixo não funcionou, por algum motivo bizarro... Adicionando 10 segundos pra fechar o LGPD
        //Abrindo label de TV
        //Utils.WaitUntilVisible(driver,10,10000,"//*[@id='productslist']/div/ng-include/div/div[1]/h3/button");            
        IWebElement accordeonTV = Utils.WaitUntilClickable(driver,5,200,"//*[@id='productslist']/div/ng-include/div/div[1]/h3/button");
        accordeonTV.Click();
        //Utils.WaitInterval(driver,5,200,"//*[@id='productslist']/div/ng-include/div/div[1]/h3/button", "click");
        //Adicionando primeiro produto, no meu caso foi o CLARO TV+ APP
        driver.FindElement(By.XPath("//*[@id='expando-unique-id']/div[2]/article/div[3]/button")).Click();
        Thread.Sleep(30000);//Tempo necessário para o produto entrar no carrinho
        Utils.TakeScreenshot(driver,"evidencia_tela_carrinho_prateleira",null);
        //Avançando para tela de SVAs
        Utils.NextBtn(driver,"Carrinho - SVAs");
        //Carrinho - Configurações de SVAs...
        Utils.TakeScreenshot(driver,"evidencia_tela_carrinho_svas",null);
        Thread.Sleep(6000);
        Utils.NextBtn(driver,"Carrinho - Fidelidade");
        //Carrinho - Fidelidade
        Utils.TakeScreenshot(driver,"evidencia_tela_carrinho_fidelidade",null);
        Thread.Sleep(10000);
        Utils.NextBtn(driver,"Método de Pagamento");
    }
}