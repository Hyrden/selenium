/* Autor: João Pedro Galvão - joao.lima@sysmap.com.br
 * Data: 29-12-2023
 * Descrição: Classe de dados. Alterar as quatro primeiras strings caso for utilizar em uma nova máquina/org.
*/
class Data{
    /***********Informações para alterar***************/    
    /*------------------Descrição-----------------------

    /*USERNAME: O login de um usuário do tipo Vendedor Televendas,
    que tenha as permissões para a jornada prospect PF. Quando for criar,
    usar como exemplo o usuário Darth Vader de bugfix22.
    OBS: Não pode ser um usuário de acesso coletivo, ele deve ser exclusivo
    para esse script.*/    

    /*PASSWORD: Senha do usuário*/

    /*ORG: alias da organização na qual o teste irá ocorrer*/

    /*SCREENSHOTS_FOLDER: Caminho na qual serão salvas as capturas de tela.
    Criar as pastas com antecedência!!!
    --------------------------------------------------*/    
    public const string USERNAME = Credentials.USERNAME;
    public const string PASSWORD = Credentials.PASSWORD;
    public const string ORG_ALIAS = "bugfix22";
    public static string SCREENSHOTS_FOLDER = @"C:\Users\jpgal\OneDrive\Imagens\Selenium Screenshots\Jornada Prospect PF\" + $"{DateTime.Now:ddMMyyyy_HHmmss}";
    /**************************************************/

    /***********Informações para a jornada*************/
    public const string PROSPECT_CPF = "61126403687";
    public const string PROSPECT_NAME = "Poe Dameron";
    public const string PROSPECT_RG = "8453855";
    public const string PROSPECT_BIRTHDAY = "26111991";
    public const string PROSPECT_MOTHER_NAME = "Poe Damerons Mother";
    public const string PROSPECT_EMAIL = "poe.dameron@starwars.com";
    public const string PROSPECT_CELPHONE = "81982022935";
    /*  Não alterar endereço por enquanto, 
        para não ter que remapear o SIHP */
    public const string PROSPECT_CEP = "13101229";
    public const string PROSPECT_HOUSENUMBER = "123";
    public const string PROSPECT_STATE = "SP";
    public const string PROSPECT_CITY = "CAMPINAS";
    /************************************************/
}