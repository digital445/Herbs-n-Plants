namespace Plants
{
    public static class Enumerations
    {
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
		/// <summary>
		/// A language the PlantName is written with
		/// </summary>
		public enum Language
		{
			Unknown = int.MaxValue,
			Latina = 1,
			Russian = 2,
			English = 3
		}
	}
}
