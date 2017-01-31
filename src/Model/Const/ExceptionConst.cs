namespace Model.Const
{
    public static class ExceptionConst
    {
        public static string AssemblyNotExist => "Projekt o podanej nazwie nie istnieje i nie można go podpiąć do DI";
        public static string AccessDenied => "Dostęp do tego miejsca jest zabroniony z posiadanymi uprawnieniami";
        public static string OwnerAccess => "Nie jesteś właścicielem tych danych";
        public static string WrongData => "Wprowadzono błędne dane";
        public static string Unauthorized => "Unauthorized";
        public static string Forbidden => "Forbidden";
    }
}