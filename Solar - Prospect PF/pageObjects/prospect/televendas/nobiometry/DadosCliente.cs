using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
class DadosCliente{
    private IWebDriver driver;
    public DadosCliente(IWebDriver driver){
        this.driver = driver;
    }
    public void PreencherDadosCliente(){
        Thread.Sleep(14000);//Tempo médio de carregamento do omniscript
        //Alterando o iFrame para preenchimento da janela do prospect 
        Utils.SwitchToFrame(driver, 0);   
        //Preenchendo os elementos do prospect via Xpath
        string[] fieldXPaths = {                
            "//input[@id='CEC_ASClienteCellPhone__c']",
            "//input[@id='CEC_ASClienteTextName']",
            "//input[@id='CEC_ASClienteTxtRG']",
            "//input[@id='CEC_ASClienteDateBirthDateText']",
            "//input[@id='CEC_ASClienteTextMothersName']",
            "//input[@id='CEC_ASClienteEmailAccount']",
            "//input[@id='CEC_ASClienteTxtCEP']",
            "//input[@id='CEC_ASClienteNumNumero']",
            "//*[@id='FiltrarCidade']"
        };
        // Valores a serem preenchidos
        string[] fieldValues = {           
            Data.PROSPECT_CELPHONE,
            Data.PROSPECT_NAME,
            Data.PROSPECT_RG,
            Data.PROSPECT_BIRTHDAY,
            Data.PROSPECT_MOTHER_NAME,
            Data.PROSPECT_EMAIL,
            Data.PROSPECT_CEP,
            Data.PROSPECT_HOUSENUMBER,
            Data.PROSPECT_CITY                
        }; 
        for (int i = 0; i < fieldXPaths.Length; i++){
                IWebElement field = driver.FindElement(By.XPath(fieldXPaths[i]));
                field.SendKeys(string.IsNullOrEmpty(field.GetAttribute("value")) ? fieldValues[i] : string.Empty);
        }         
        //Preenchendo a busca de endereço                     
        IWebElement picklistEstado = driver.FindElement(By.XPath("//*[@id='CEC_ASClienteSelectEstado']"));
        new SelectElement(picklistEstado).SelectByText(Data.PROSPECT_STATE);       
        
        driver.FindElement(By.XPath("//*[@id='CEC_ASClienteSelectCidade']")).Click();

        Thread.Sleep(5000);//Tempo necessário pra carregar a picklist

        IWebElement picklistCidade = driver.FindElement(By.XPath("//*[@id='CEC_ASClienteSelectCidade']"));
        new SelectElement(picklistCidade).SelectByText(Data.PROSPECT_CITY);            
        driver.FindElement(By.XPath("//*[@id='IPABuscarGED']")).Click();  

        Utils.WaitInterval(driver,9,3000,"//*[@id='radio-button-label-01']", "click");   

        driver.FindElement(By.XPath("//*[@id='modal-content-id-1']/div[3]/button[2]")).Click();
        Thread.Sleep(1000);            
        driver.FindElement(By.XPath("//*[@id='IPAChecarCredito']/p")).Click();
        Thread.Sleep(10000);
        Utils.TakeScreenshot(driver,"evidencia_tela_dadoscliente",null);            
        //Seguindo para a tela de HP
        //driver.FindElement(By.XPath("//button[@id='btn-next']")).Click();
        Utils.NextBtn(driver,"HP");
    }
}