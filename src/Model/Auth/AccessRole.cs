namespace Model.Auth
{
    public class AccessRole
    {
        public AccessRole(int access, int? objectId = null)
        {
            AccessLevel = access;
            ObjectId = objectId;
        }

        public int AccessLevel { get; set; }
        public int? ObjectId { get; set; }

        public bool Asert(int access, int? objectId)
        {
            return (access >= AccessLevel) && ((ObjectId == null) || (ObjectId == objectId));
        }
    }
}