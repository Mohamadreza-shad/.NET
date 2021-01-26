namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string Gender { get; set; }
        public string CurrentUsername { get; set; }
        public int MinAge { get; set; } = 27;
        public int MaxAge { get; set; } = 35;

        public string OrderBy { get; set; } = "DateOfBirth";
    }
}
