namespace Plants
{
    public static class StaticDetails
    {
        public static string? PlantsAPIBaseUrl { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        /// <summary>
        /// Type of View represented on the Image
        /// </summary>
        public enum ViewType
        {
            GeneralView,
            Flower,
            Bud,
            Fruit,
            Leaf,
            Stem
        }
    }
}
