namespace IdentityServer4.MongoDB.Entities
{
    public abstract class Property
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}