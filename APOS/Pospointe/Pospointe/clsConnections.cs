namespace Pospointe
{
    class clsConnections
    {



        public static string thisversion = "4.7";


        public static string baseurllogin = "https://asnitagentapi.azurewebsites.net";


        //public static string transserverprimaryurl = "https://server.mypospointe.com:444";

        public static string transserverprimaryurl = "https://transserver2.mypospointe.com:9443";

        // public static string myposapiurl = "https://api3.mypospointe.com:8843";


        //public static string marketplaceserver = "https://cartpointeapi.azurewebsites.net";

        public static string marketplaceserver = "https://premyposmarketplace-hbg8btcka8adfva3.eastus-01.azurewebsites.net";

        // public static string marketplaceserver = "https://192725ef12c0.ngrok-free.app";

        //https://192725ef12c0.ngrok-free.app/


        //public static string transserverprimaryurl = "https://transuat.mypospointe.com:9943";

        //  public static string transserverprimaryurl = "https://2696-70-19-102-144.ngrok-free.app";

        public static string transserverbackupurl = "https://backup.myposerver.com";



        // public static string myposapiurl = "https://myposapi-test.azurewebsites.net";
        public static string myposapiurl = "https://api3.mypospointe.com:8843";

        public static string transserverauth = "Basic YWRtaW46cGFzc3dvcmQ=";

        ///////////////////// //MYDB ///////////////////

        public static string mydb = "170";


        ///////////////////// //MYDB ///////////////////
        /// <summary>
        /// 
        /// </summary>
        /// 

        ///Loyalty Server
        ///
        public static string loyaltyserver = "https://pospointeloyalty.azurewebsites.net/";
        public static string basicauthcode = "cG9zcG9pbnRlOkFmc2hhbjU3MzE4NA==";
        public static string thistoreid = "";
        public static string thisstoregroupid = "STG-000000";
        public static bool loyaltyactive = false;



        //Gift Card Server Here
        public static bool allowgiftcard = false;
        public static string giftcardserver = "https://giftcard.myposerver.com";
        public static string giftcardserverauth = "YWRtaW46cGFzc3dvcmQ=";


        //Time Card Server Here
        public static bool allowtimecard = true;

        public static string timecardserver = "https://mytimecard.azurewebsites.net";
        public static string timecardserverauth = "Basic QVNOSVQubGs6QVNOMTIz";

        //public static int = 51
    }
}
