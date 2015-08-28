using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ParcelManager
{
    /// <summary>
    /// This class contains rules definitions and method to apply rule to calculate cost
    /// </summary>    
    public static class RuleEngine
    {
        private static List<Rule> _rules = new List<Rule>();    //list of rule definitions

        static RuleEngine()
        {
            InitializeRules();            
        }

        /// <summary>
        /// Populates rule lists with predefined rules and cost calculation expressions
        /// </summary>
        private static void InitializeRules()
        {
            _rules.AddRange(
                    new Rule[] 
                    {
                            new Rule(RulePriority.Highest, "Rejected", new ExpressionString("Weight", "GreaterThan", "50"), null),
                            new Rule(RulePriority.High, "Heavy Parcel", new ExpressionString("Weight", "GreaterThan", "10"), new ExpressionString("Weight","Multiply","15")),
                            new Rule(RulePriority.Medium, "Small Parcel", new ExpressionString("Volume", "LessThan", "1500"), new ExpressionString("Volume","Multiply","0.05")),
                            new Rule(RulePriority.Low, "Medium Parcel", new ExpressionString("Volume", "LessThan", "2500"), new ExpressionString("Volume","Multiply","0.04")),
                            new Rule(RulePriority.Lowest, "Large Parcel", null, new ExpressionString("Volume","Multiply","0.03"))
                    }
                );
        }

        /// <summary>
        /// Applies rule based on priority to the Parcel object 
        /// and calculates cost as per the formula defined in the matching rule
        /// </summary>
        /// <param name="item">Object on which to apply rule</param>
        /// <returns>
        /// Tuple containing RuleCategory and Cost
        /// </returns>
        public static Tuple<string, double> ApplyRule<T>(T item)
        {
            //Output to be returned to the caller
            Tuple<string, double> output = null;
            
            //Functions to store compiled lambda expression
            Func<T, double> getCost;   
            Func<T, bool> isRuleMatch;

            //Start with rules from Highest to Lowest priority
            foreach(Rule rule in _rules.OrderBy(r=>r.Priority))
            {
                if (rule.Condition != null) //If condition is specified
                {
                    isRuleMatch = CompileExpression<T, bool>(rule.Condition);  //Check if the rule condition is satisfied
                    if (isRuleMatch(item))
                    {
                        if (rule.Cost != null)
                        {
                            getCost = CompileExpression<T, double>(rule.Cost);

                            //Calculate cost using the formula defined in the rule
                            double cost = getCost(item);

                            output = new Tuple<string, double>(rule.RuleCategory, cost);
                            break;
                        }
                        else  //Cost formula is not defined, so return NaN to indicate it
                        {
                            output = new Tuple<string, double>(rule.RuleCategory, double.NaN);
                            break;
                        }
                    }
                }
                else  //Condition not defined, proceed with cost calculation
                {
                    if (rule.Cost != null)
                    {
                        getCost = CompileExpression<T, double>(rule.Cost);
                        double cost = getCost(item);
                        output = new Tuple<string, double>("Large Parcel", cost);
                        break;
                    }
                    else
                    {
                        output = new Tuple<string, double>("Large Parcel", double.NaN);
                        break;
                    }
                }                
            }
            return output;
        }

        /// <summary>
        /// Returns compiled lambda expression for given expression
        /// </summary>
        /// <typeparam name="T1">Input type on which to apply expression</typeparam>
        /// <typeparam name="T2">Return type for the expression</typeparam>
        /// <param name="exString">expression formula</param>
        /// <returns>Compiled lambda expression</returns>
        public static Func<T1, T2> CompileExpression<T1, T2>(ExpressionString exString)
        {
            var param = Expression.Parameter(typeof(T1));
            var expr = BuildExpression<T1>(exString, param);
            return Expression.Lambda<Func<T1, T2>>(expr, param).Compile();
        }

        /// <summary>
        /// Build and returns binary expression based on given expression formula
        /// </summary>
        /// <typeparam name="T">Type of expression to return</typeparam>
        /// <param name="exString">expression formula</param>
        /// <param name="param">Type used to identify operand in the expression</param>
        /// <returns>Returns binary expression based on given formula</returns>
        public static Expression BuildExpression<T>(ExpressionString exString, ParameterExpression param)
        {
            var left = MemberExpression.Property(param, exString.LeftSide);
            var leftPropType = typeof(T).GetProperty(exString.LeftSide).PropertyType;
            ExpressionType exprBinary;

            if (ExpressionType.TryParse(exString.Operator, out exprBinary))
            {
                var right = Expression.Constant(Convert.ChangeType(exString.RightSide, leftPropType));
                return Expression.MakeBinary(exprBinary, left, right);
            }
            else
                throw new Exception("Invalid Expression");
        }
    }
    
    /// <summary>
    /// This class defines generic expression formula in the form of operand and operator
    /// It is used by RuleEngine to calculate formula values
    /// </summary>
    public class ExpressionString
    {
        public string LeftSide { get; set; }
        public string Operator { get; set; }
        public string RightSide { get; set; }

        public ExpressionString(string Left, string Operator, string Right)
        {
            this.LeftSide = Left;
            this.Operator = Operator;
            this.RightSide = Right;
        }

    }
}
