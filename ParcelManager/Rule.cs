
namespace ParcelManager
{
    /// <summary>
    /// This class contains rule containing business logic to calculate cost
    /// based on given codition expression
    /// </summary>
    public class Rule
    {
        public RulePriority Priority { get; set; }  //This will define in which order rule should be applied
        public string RuleCategory { get; set; }    //This will be used for display purpose (e.g. Parcel Category)
        public ExpressionString Condition { get; set; }  //Rule condition formula defined as expression
        public ExpressionString Cost { get; set; }   //Cost calculation formula defined as expression

        public Rule(RulePriority priority, string category, ExpressionString condition, ExpressionString costExpression)
        {
            this.Priority = priority;
            this.RuleCategory = category;
            this.Condition = condition;
            this.Cost = costExpression;
        }

    }

    public enum RulePriority
    {
        Highest = 1,
        High = 2,
        Medium = 3,
        Low = 4,
        Lowest = 5
    }

}
