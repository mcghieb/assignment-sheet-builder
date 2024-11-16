using Newtonsoft.Json;

namespace Models;

public class Course
{
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("enrollment_term_id")]
    public int EnrollmentTermId { get; set; }
}