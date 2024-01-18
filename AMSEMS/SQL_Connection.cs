namespace AMSEMS
{
    internal class SQL_Connection
    {
        //azure cloud
        //public static string connection = @"Server=tcp:amsems.database.windows.net,1433;Initial Catalog=db_Amsems;Persist Security Info=False;User ID=CloudSA230f235e@amsems;Password=Nozurbnorberto27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;";

        //google cloud
        //public static string connection = @"Server=104.197.95.130;Database=db_Amsems;User Id=sqlserver;Password=Nozurbnorberto27;";

        //local
        public static string connection = @"Data Source=LAPTOP-78S661F5;Initial Catalog=db_Amsems;Integrated Security=True";

        //Connect locally
        //public static string connection = @"Server=192.168.1.100;Database=db_Amsems;User Id=nor;Password=12345;";

        //ngrok
        //static string server = "0.tcp.ap.ngrok.io,10632"; // Ngrok tunnel URL
        //static string database = "db_Amsems"; // Database name
        //static string username = "nor"; // Database username
        //static string password = "12345"; // Database password

        //public static string connection = $"Server={server};Database={database};User Id={username};Password={password};";
    }
}
