public interface ISatisfier : IFurnitureUser
{
    void Satisfy(NeedData needData);
    void StopSatisfying(NeedData needData);
}

