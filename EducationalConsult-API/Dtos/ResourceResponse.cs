namespace EducationalConsultAPI.Dtos {
    public class ResourceResponse {
        /// <summary>
        /// Link to the resource
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Title of the resource
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type of the resource = note, assignment, videos
        /// </summary>
        public string Type { get; set; }
    }
}
