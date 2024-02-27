public static class TestHelper
{
    public const string BASE = "https://localhost:5001";


    public static class Routes
    {
        public const string HOME = BASE;
        public const string LOGIN = $"{BASE}/login";
        public const string LOGOUT = $"{BASE}/logout";
        public const string VIRTUALMACHINES = $"{BASE}/virtualmachines";
        public const string BY_ID_VIRTUALMACHINES = $"{BASE}/virtualmachine";
        public const string ADD_VIRTUALMACHINES = $"{BASE}/virtualmachines/add";
        public const string CUSTOMER_PROFILE = $"{BASE}/klant";
        public const string CUSTOMERS = $"{BASE}/klanten";
        public const string BEHEERDERS = $"{BASE}/beheerders";
        public const string SERVERS = $"{BASE}/servers";
        public const string BY_ID_SERVERS = $"{BASE}/servers";
        public const string BESCHIKBAARHEID = $"{BASE}/beschikbaarheid";

    }
}




