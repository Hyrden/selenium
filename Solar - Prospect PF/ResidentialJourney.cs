/* Autor: João Pedro Galvão - joao.lima@sysmap.com.br
 * Data: 29-12-2023
 * Descrição: Jornada residencial com um produto de TV. Para perfis vendedor televendas.
*/
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
class ResidentialJourney{
    public ResidentialJourney(){
        ChromeOptions options = new ChromeOptions();
        options.AddArgument(@"user-data-dir=C:\Users\jpgal\AppData\Local\Google\Chrome\User Data\Default");//Tentativa de armazenar cache pelo chrome, mas não deu certo
        var driver = new ChromeDriver(options);
        driver.Url = $"https://cec-claro--{Data.ORG_ALIAS}.sandbox.lightning.force.com/lightning/n/Buscar_Cliente"; //Redirecionando diretamente para a tela prospect de buscar cliente
        
        LoginPage loginPage = new LoginPage(driver);
        
        if (loginPage.IsLoginPage()){
            loginPage.EnterUserName(Credentials.USERNAME);
            loginPage.EnterPassword(Credentials.PASSWORD);
            loginPage.ClickLoginButton();
        }
        //Desenvolver tudo dentro do Try, para que o selenium faça o logout do usuário mesmo se estourar um erro
        try{
            Thread.Sleep(2000);//Tempo máximo pro aura detectar o perfil do usuário logado e modificar a tela de busca
            Utils.WaitInterval(driver,3,200,"//input[contains(@id, 'input-')]", Data.PROSPECT_CPF);
            //Try-catch de busca. Necessário porque às vezes o aura não refresha rapidamente a tela com as permissões do perfil do vendedor
            try{          
                Utils.WaitInterval(driver,1,200,"//button[text()='Buscar']", "click");
                Utils.WaitInterval(driver,3,200,"//button[normalize-space(text())='Iniciar']", "click");
            }catch(Exception ex){
                Utils.WaitInterval(driver,1,200,"//button[text()='Buscar']", "click");
                Utils.WaitInterval(driver,6,200,"//button[normalize-space(text())='Iniciar']", "click");
            }           

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
                "//input[@id='CEC_ASClienteEmailAccount']"
            };
            // Valores a serem preenchidos
            string[] fieldValues = {                
                Data.PROSPECT_CELPHONE,
                Data.PROSPECT_NAME,
                Data.PROSPECT_RG,
                Data.PROSPECT_BIRTHDAY,
                Data.PROSPECT_MOTHER_NAME,
                Data.PROSPECT_EMAIL
            };
            //try-catch criado para contornar o problema descrito no comentário abaixo. Se os campos já estiverem preenchidos, vai dar erro. Então, o script vai continuar rodando.
            try{
                // Preencho os campos apenas se estiverem vazios (não deu certo para quando a conta é existente... Está dando o erro element not interactable, sendo necessário deletar manualmente a account sempre que ela existir previamente)
                for (int i = 0; i < fieldXPaths.Length; i++)
                {
                    IWebElement field = driver.FindElement(By.XPath(fieldXPaths[i]));
                    field.SendKeys(string.IsNullOrEmpty(field.GetAttribute("value")) ? fieldValues[i] : string.Empty);
                }
            }catch(Exception ex){
                Console.WriteLine($"Erro não tratado: {ex.Message}");
            }            
            //Preenchendo a busca de endereço            
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteTxtCEP']")).SendKeys(Data.PROSPECT_CEP);
            driver.FindElement(By.XPath("//input[@id='CEC_ASClienteNumNumero']")).SendKeys(Data.PROSPECT_HOUSENUMBER);            
            IWebElement picklistEstado = driver.FindElement(By.XPath("//*[@id='CEC_ASClienteSelectEstado']"));
            new SelectElement(picklistEstado).SelectByText(Data.PROSPECT_STATE);
            driver.FindElement(By.XPath("//*[@id='FiltrarCidade']")).SendKeys(Data.PROSPECT_CITY);            
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
            Thread.Sleep(2000);

            //Tela de HP
            driver.FindElement(By.XPath("//*[@id='IPAGetAddressList']")).Click();
            Utils.WaitInterval(driver,10,200,"//*[@id='radio-button-label-1']/span[1]", "click");

            Utils.NextBtn(driver,"HP - Número");

            //Thread.Sleep(5000);
            
            Utils.WaitInterval(driver,10,200,"/html/body/div/span/span/div/ng-view/div/div[2]/div/bptree/child[30]/div[1]/section/form/div[1]/div/child/div/ng-form/div/div/div[2]/child[17]/div/ng-form/div/ng-include/div/div/table/tbody/tr[6]/td/span/label/span[1]", "click");
            Utils.NextBtn(driver,"HP - Viabilidade técnica e comercial");
            Thread.Sleep(30000);//Tempo necessário para a tela de viabilidade técnica/comercial
            Utils.TakeScreenshot(driver,"evidencia_tela_hp_viabilidade",null);
            //Seguindo para tela de carrinho
            Utils.NextBtn(driver,"Carrinho");

            //Thread.Sleep(35000);//Tempo necessário para a tela de carrinho

            //Tela de Carrinho
            IWebElement btnFecharModal = Utils.WaitInterval(driver,35,1000,"//*[@id='modal-content-id-1']/div[2]/span[2]/button");
            btnFecharModal.Click();
            //driver.FindElement(By.XPath("//*[@id='modal-content-id-1']/div[2]/span[2]/button")).Click();//Fechando modal LGPD

            //Aviso: dependendo de como esteja o catálogo, ajustes deverão ser feitos
            Thread.Sleep(10000);//O WaitInterval de baixo não funcionou, por algum motivo bizarro... Adicionando 10 segundos pra fechar o LGPD
            //Abrindo label de TV
            Utils.WaitInterval(driver,5,200,"//*[@id='productslist']/div/ng-include/div/div[1]/h3/button", "click");
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
            //Carrinho - Método de Pagamento
            Thread.Sleep(15000);
            //Alterando para Boleto
            driver.FindElement(By.XPath("//*[@id='RadioPaymeMethod|0']/div/div[1]/label[2]/span[1]")).Click();
            Thread.Sleep(12000);//Demora muito alterar pro boleto, então tive que adicionar essa Thread
            IWebElement picklistVencimento = driver.FindElement(By.XPath("//*[@id='CEC_DiadeVencimentoDebitoBoletoCreditoGUI']"));
            new SelectElement(picklistVencimento).SelectByText("5");
            //Avançando pra tela de resumo
            Utils.NextBtn(driver,"Resumo");
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
            

        }catch(NoSuchElementException ex){
            Console.WriteLine($"Erro ao encontrar o elemento: {ex.Message}");
            Utils.TakeScreenshot(driver,"evidencia_erro",ex.Message);
        }
        catch(Exception ex){
            Console.WriteLine($"Erro não tratado: {ex.Message}");
            Utils.TakeScreenshot(driver,"evidencia_erro",ex.Message);
        }
        Thread.Sleep(3000);//Dando um tempo para verificar tudo que foi feito
        Utils.TakeScreenshot(driver,"evidencia_tela_order",null);
        /*Executar o código abaixo para parar a execução.
        Se for comentar, lembre-se de se deslogar após o teste*/
        Utils.LogoutFromSalesforce(driver);//Sempre deslogar para evitar o bug incômodo de finalizar sessão
        Thread.Sleep(3000);
        driver.Quit();
        /******************************************************/
    }
}