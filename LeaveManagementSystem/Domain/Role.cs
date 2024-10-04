namespace LeaveManagementSystem.Domain
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public Types.State State { get; set; } = Types.State.Active;
        public class Types
        {
            public enum State
            {
                Active = 0,
                InActive = 1,
            }
        }
    }
}
