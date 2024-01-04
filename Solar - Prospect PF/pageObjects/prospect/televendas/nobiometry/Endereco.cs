using OpenQA.Selenium;
class Endereco{
    private IWebDriver driver;
    public Endereco(IWebDriver driver){
        this.driver = driver;
    }
    public void PreencherHP(){
        //Tela de HP
        Thread.Sleep(2000);
        driver.FindElement(By.XPath("//*[@id='IPAGetAddressList']")).Click();
        Utils.WaitInterval(driver,10,200,"//*[@id='radio-button-label-1']/span[1]", "click");

        Utils.NextBtn(driver,"HP - Número");
        
        Utils.WaitInterval(driver,10,200,"/html/body/div/span/span/div/ng-view/div/div[2]/div/bptree/child[30]/div[1]/section/form/div[1]/div/child/div/ng-form/div/div/div[2]/child[17]/div/ng-form/div/ng-include/div/div/table/tbody/tr[6]/td/span/label/span[1]", "click");
        Utils.NextBtn(driver,"HP - Viabilidade técnica e comercial");
        Thread.Sleep(30000);//Tempo necessário para a tela de viabilidade técnica/comercial
        Utils.TakeScreenshot(driver,"evidencia_tela_hp_viabilidade",null);
        //Seguindo para tela de carrinho
        Utils.NextBtn(driver,"Carrinho");
    }
}