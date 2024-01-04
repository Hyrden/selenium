/* Autor: João Pedro Galvão - joao.lima@sysmap.com.br
 * Data: 29-12-2023
 * Descrição: Jornada residencial com um produto de TV. Para perfis vendedor televendas.
*/
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
        try{
            BuscarCliente buscarCliente = new BuscarCliente(driver);
            buscarCliente.RealizarBuscaCliente();

            DadosCliente dadosCliente = new DadosCliente(driver);
            dadosCliente.PreencherDadosCliente();
            
            Endereco endereco = new Endereco(driver);
            endereco.PreencherHP();

            Carrinho carrinho = new Carrinho(driver);
            carrinho.PreencherCarrinho();

            MetodoPagamento metodoPagamento = new MetodoPagamento(driver);
            metodoPagamento.PreencherMetodoPagamento();

            Resumo resumo = new Resumo(driver);
            resumo.PreencherResumo();

        }catch(NoSuchElementException ex){
            Console.WriteLine($"Erro ao encontrar o elemento: {ex.Message}");
            Utils.TakeScreenshot(driver,"evidencia_erro",ex.Message);
        }
        catch(Exception ex){
            Console.WriteLine($"Erro não tratado: {ex.Message}");
            Utils.TakeScreenshot(driver,"evidencia_erro",ex.Message);
        }
        Thread.Sleep(3000);//Dando um tempo para verificar tudo que foi feito
        /*Executar o código abaixo para parar a execução.
        Se for comentar, lembre-se de se deslogar após o teste*/
        Utils.LogoutFromSalesforce(driver);//Sempre deslogar para evitar o bug incômodo de finalizar sessão
        Thread.Sleep(3000);
        driver.Quit();
        /******************************************************/
    }
}