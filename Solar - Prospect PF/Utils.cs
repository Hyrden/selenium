/* Autor: João Pedro Galvão - joao.lima@sysmap.com.br
 * Data: 29-12-2023
 * Descrição: Classe com os métodos executados pela(s) classe(s) de jornada.
*/
using System.Drawing;
using System.Drawing.Imaging;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
class Utils{
    /*  Método WaitInterval
        Descrição: Criei este método para o tempo de espera das interações ser melhor do que o uso do Thread.Sleep.
        Teoricamente, ele permite você definir segundos-limite de espera antes de dar timeout, e milisegundos para 
        o selenium checar de tempos em tempos.
        O problema de usar o Thread.sleep é que dependendo de como esteja a org do salesforce, o tempo pode variar,
        para mais ou para menos.
    */
    public static IWebElement WaitInterval(IWebDriver driver, int seconds, int miliseconds, string xpath){
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        wait.PollingInterval = TimeSpan.FromMilliseconds(miliseconds);    
        return wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
    }
    public static void WaitInterval(IWebDriver driver, int seconds, int miliseconds, string xpath, string action){
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        wait.PollingInterval = TimeSpan.FromMilliseconds(miliseconds);          
        if(action == "click"){            
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath))).Click();
        }else{
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath))).SendKeys(action);
        }
    }
    public static bool IsLoginPage(IWebDriver driver){
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
    public static void SwitchToFrame(IWebDriver driver, int index)
    {
        List<IWebElement> iframesList = driver.FindElements(By.XPath("//iframe")).ToList();
        Console.WriteLine($"Número de iFrames detectados: {iframesList.Count}");
        if (index >= 0 && index < iframesList.Count)
        {
            driver.SwitchTo().Frame(iframesList[index]);
            Console.WriteLine($"Alternado para o iframe do índice {index}.");
        }
        else
        {
            Console.WriteLine("Índice de iframe inválido.");
        }
    }
    public static void TakeScreenshot(IWebDriver driver, String text, String errorMessage){
        try{
            if (!Directory.Exists(Data.SCREENSHOTS_FOLDER)){
                Directory.CreateDirectory(Data.SCREENSHOTS_FOLDER);
            }
            string pathToSave = Data.SCREENSHOTS_FOLDER;
            string fileName = $"screenshot_{text}_{DateTime.Now:ddMMyyyy_HHmmss}.png";
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            byte[] screenshotAsByteArray = screenshot.AsByteArray;
            Bitmap screenshotAsBitmap = new Bitmap(new MemoryStream(screenshotAsByteArray));
            // Adicionando textos
            using (Graphics graphics = Graphics.FromImage(screenshotAsBitmap))
            {
                //Fonte e cor do texto
                Font font = new Font("Arial", 12);
                SolidBrush brush = new SolidBrush(Color.Red);

                //Posicionando texto na parte inferior direita                
                string texto = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                graphics.DrawString(texto, font, brush, new PointF(screenshotAsBitmap.Width - 200, screenshotAsBitmap.Height - 20));

                //Caso exista, adiciona mensagem de erro do selenium na parte inferior
                if(!String.IsNullOrEmpty(errorMessage)){
                    string errorText = $"Erro: {errorMessage}";
                    graphics.DrawString(errorText, font, brush, new PointF(10, screenshotAsBitmap.Height - 40));
                }
            }
            screenshotAsBitmap.Save(Path.Combine(pathToSave, fileName), ImageFormat.Png);
            Console.WriteLine($"Screenshot salva em: {Path.Combine(pathToSave, fileName)}");
        }catch(Exception ex){
            Console.WriteLine($"Erro ao printar a tela: {ex.Message}");
        }
    }
    public static void NextBtn(IWebDriver driver, string tela){
        IReadOnlyList<IWebElement> botoes = driver.FindElements(By.XPath("//button[@id='btn-next']"));
        foreach (IWebElement botao in botoes)
        {
            if (botao.Displayed && botao.Enabled)
            {
                botao.Click();
                Console.WriteLine($"Avançou para a tela de {tela}");
                break;
            }
        }
    }
    public static void ShowAllElements(IWebDriver driver){
        IReadOnlyList<IWebElement> elementos = driver.FindElements(By.XPath("//*"));

        List<string> idsDosElementos = new List<string>();
        foreach (IWebElement elemento in elementos)
        {
            string idElemento = elemento.GetAttribute("id");
            if (!string.IsNullOrEmpty(idElemento))
            {
                idsDosElementos.Add(idElemento);
            }
        }
        // Exibe os IDs dos elementos no console
        Console.WriteLine("IDs dos elementos na página:");
        foreach (string idElemento in idsDosElementos)
        {
            Console.WriteLine(idElemento);
        }
    }
    public static void LogoutFromSalesforce(IWebDriver driver){
        driver.Url = $"https://cec-claro--{Data.ORG_ALIAS}.sandbox.lightning.force.com/secur/logout.jsp";
    }
}