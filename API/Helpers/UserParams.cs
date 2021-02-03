namespace API.Helpers
{
    public class UserParams : PaginationParams
    {
        public string Gender { get; set; }
        public string CurrentUsername { get; set; }
        public int MinAge { get; set; } = 27;
        public int MaxAge { get; set; } = 35;

        public string OrderBy { get; set; } = "DateOfBirth";
    }
}
