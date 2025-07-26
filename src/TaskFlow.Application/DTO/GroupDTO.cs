namespace TaskFlow.Application.DTO
{
    public class GroupDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Users { get; set; }
        public List<string> Tasks { get; set; }
        public string OwnerId { get; set; }
        public GroupDTO(string id, string name, string description, List<string> users, List<string> tasks, string ownerId)
        {
            Id = id;
            Name = name;
            Description = description;
            Users = users ?? new List<string>();
            Tasks = tasks ?? new List<string>();
            OwnerId = ownerId;
        }
    }
}
