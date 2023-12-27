using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

class Program
{
    static void Main()
    {        
        ChromeOptions options = new ChromeOptions();
        options.AddArgument(@"user-data-dir=C:\Users\jpgal\AppData\Local\Google\Chrome\User Data\Default");//Tentativa de armazenar cache pelo chrome, mas não deu certo
        var driver = new ChromeDriver(options);
        driver.Url = "https://cec-claro--bugfix22.sandbox.lightning.force.com/lightning/n/Buscar_Cliente"; //Redirecionando diretamente para a tela prospect de buscar cliente
        if (IsLoginPage(driver)){
            driver.FindElement(By.Name("username")).SendKeys(Credentials.USERNAME);
            driver.FindElement(By.Name("pw")).SendKeys(Credentials.PASSWORD + Keys.Return);
        }
        Thread.Sleep(3000);//Tempo de espera para o salesforce carregar
        try{//Desenvolver tudo dentro do Try, para que o selenium faça o logout do usuário mesmo se estourar um erro
            driver.FindElement(By.XPath("//input[contains(@id, 'input-')]")).SendKeys("61126403687");
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//button[text()='Buscar']")).Click();
            Thread.Sleep(6000);
            driver.FindElement(By.XPath("//button[normalize-space(text())='Iniciar']")).Click();
            Thread.Sleep(14000);  

            //Alterando o iFrame para preenchimento da janela do prospect
            List<IWebElement> iframesList = driver.FindElements(By.XPath("//iframe")).ToList();
            Console.WriteLine($"Número de iFrames detectados: {iframesList.Count}");
            SwitchToFrame(driver, iframesList, 0);

            //Preenchendo os elementos do prospect

            /*driver.FindElement(By.XPath("//input[@id='CEC_ASClienteTextName']")).SendKeys("Poe Dameron");
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteTxtRG']")).SendKeys("8453855");
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteDateBirthDateText']")).SendKeys("26111991");
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteTextMothersName']")).SendKeys("Poe Damerons Mother");
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteEmailAccount']")).SendKeys("poe.dameron@starwars.com");
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteCellPhone__c']")).SendKeys("81982022935");*/

            // XPath dos campos
            string[] fieldXPaths = {
                "//input[@id='CEC_ASClienteTextName']",
                "//input[@id='CEC_ASClienteTxtRG']",
                "//input[@id='CEC_ASClienteDateBirthDateText']",
                "//input[@id='CEC_ASClienteTextMothersName']",
                "//input[@id='CEC_ASClienteEmailAccount']",
                "//input[@id='CEC_ASClienteCellPhone__c']"
            };
            // Valores a serem preenchidos
            string[] fieldValues = {
                "Poe Dameron",
                "8453855",
                "26111991",
                "Poe Damerons Mother",
                "poe.dameron@starwars.com",
                "81982022935"
            };
            // Preencho os campos apenas se estiverem vazios (não deu certo para quando a conta é existente... Está dando o erro element not interactable, sendo necessário deletar manualmente a account sempre que ela existir previamente)
            for (int i = 0; i < fieldXPaths.Length; i++)
            {
                IWebElement field = driver.FindElement(By.XPath(fieldXPaths[i]));
                field.SendKeys(string.IsNullOrEmpty(field.GetAttribute("value")) ? fieldValues[i] : string.Empty);
            }
            //Preenchendo a busca de endereço            
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteTxtCEP']")).SendKeys("13101229");
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteNumNumero']")).SendKeys("123");            
            IWebElement picklistEstado = driver.FindElement(By.XPath("//*[@id='CEC_ASClienteSelectEstado']"));
            new SelectElement(picklistEstado).SelectByText("SP");
            driver.FindElement(By.XPath("//*[@id='FiltrarCidade']")).SendKeys("CAMPINAS");            
            driver.FindElement(By.XPath("//*[@id='CEC_ASClienteSelectCidade']")).Click();
            Thread.Sleep(5000);//Tempo necessário pra carregar a picklist
            IWebElement picklistCidade = driver.FindElement(By.XPath("//*[@id='CEC_ASClienteSelectCidade']"));
            new SelectElement(picklistCidade).SelectByText("CAMPINAS");            
            driver.FindElement(By.XPath("//*[@id='IPABuscarGED']")).Click();            
            Thread.Sleep(8000);//Tempo necessário para carregar o modal        
            driver.FindElement(By.XPath("//*[@id='radio-button-label-01']")).Click();            
            driver.FindElement(By.XPath("//*[@id='modal-content-id-1']/div[3]/button[2]")).Click();
            Thread.Sleep(1000);            
            driver.FindElement(By.XPath("//*[@id='IPAChecarCredito']/p")).Click();
            Thread.Sleep(10000);
            takeScreenshot(driver,"evidencia_tela_dadoscliente");            
            //Seguindo para a tela de HP
            driver.FindElement(By.XPath("//*[@id='btn-next']/p")).Click();
            Thread.Sleep(2000);
            //Evidenciando a última tela
            takeScreenshot(driver,"desenvolvimento_ultima_tela");
            

        }catch(NoSuchElementException ex){
            Console.WriteLine($"Erro ao encontrar o elemento: {ex.Message}");
            takeScreenshot(driver,"evidencia_erro");
        }
        catch(Exception ex){
            Console.WriteLine($"Erro não tratado: {ex.Message}");
            takeScreenshot(driver,"evidencia_erro");
        }
        Thread.Sleep(3000);//Dando um tempo para verificar tudo que foi feito
        /*Executar o código abaixo para parar a execução.
        Se for comentar, lembre-se de se deslogar após o teste*/
        logoutFromSalesforce(driver);//Sempre deslogar para evitar o bug incômodo de finalizar sessão
        Thread.Sleep(3000);
        driver.Quit();
        /******************************************************/
    }
    static bool IsLoginPage(IWebDriver driver){
        try
        {
            IWebElement loginElement = driver.FindElement(By.Id("username"));
            return true;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
    }
    static void SwitchToFrame(IWebDriver driver, List<IWebElement> iframesList, int index)
    {
        if (index >= 0 && index < iframesList.Count)
        {
            driver.SwitchTo().Frame(iframesList[index]);
            Console.WriteLine($"Alternado para o iframe no índice {index}.");
        }
        else
        {
            Console.WriteLine("Índice de iframe inválido.");
        }
    }
    static void takeScreenshot(IWebDriver driver, String text){
        try{
            string pathToSave = @"C:\Users\jpgal\OneDrive\Imagens\Selenium Screenshots\Jornada Prospect PF";
            //string fileName = $"screenshot_{text}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string fileName = $"screenshot_{text}_{DateTime.Now:ddMMyyyy_HHmmss}.png";
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            //screenshot.SaveAsFile(Path.Combine(pathToSave, fileName), ImageFormat.Png);
            byte[] screenshotAsByteArray = screenshot.AsByteArray;
            Bitmap screenshotAsBitmap = new Bitmap(new MemoryStream(screenshotAsByteArray));
            // Adicionando texto (data e hora)
            using (Graphics graphics = Graphics.FromImage(screenshotAsBitmap))
            {
                //Fonte e cor do texto
                Font font = new Font("Arial", 12);
                SolidBrush brush = new SolidBrush(Color.Red);

                //Posicionando texto na parte inferior direita
                string texto = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                graphics.DrawString(texto, font, brush, new PointF(screenshotAsBitmap.Width - 200, screenshotAsBitmap.Height - 20));
            }
            screenshotAsBitmap.Save(Path.Combine(pathToSave, fileName), ImageFormat.Png);
            Console.WriteLine($"Screenshot salva em: {Path.Combine(pathToSave, fileName)}");
        }catch(Exception ex){
            Console.WriteLine($"Erro ao printar a tela: {ex.Message}");
        }
    }
    static void logoutFromSalesforce(IWebDriver driver){
        driver.Url = "https://cec-claro--bugfix22.sandbox.lightning.force.com/secur/logout.jsp";
    }
}
