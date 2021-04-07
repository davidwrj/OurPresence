using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OurPresence.Modeller.Liquid.Util;

namespace OurPresence.Modeller.Liquid
{
    /// <summary>
    /// 
    /// </summary>
    public class Operators
    {
        private readonly ConcurrentDictionary<string, ConditionOperatorDelegate> _operators;

        /// <summary>
        /// 
        /// </summary>
        public Operators()
        {
            _operators = new ConcurrentDictionary<string, ConditionOperatorDelegate>(new[]
            {
                new KeyValuePair<string, ConditionOperatorDelegate>("==", (left, right) => EqualVariables(left, right)) ,
                new KeyValuePair<string, ConditionOperatorDelegate>( "!=", (left, right) => !EqualVariables(left, right)),
                new KeyValuePair<string, ConditionOperatorDelegate>( "<>", (left, right) => !EqualVariables(left, right) ),
                new KeyValuePair<string, ConditionOperatorDelegate>( "<", (left, right) => left != null && right != null && Comparer<object>.Default.Compare(left, Convert.ChangeType(right, left.GetType())) == -1 ),
                new KeyValuePair<string, ConditionOperatorDelegate>( ">", (left, right) => left != null && right != null && Comparer<object>.Default.Compare(left, Convert.ChangeType(right, left.GetType())) == 1 ),
                new KeyValuePair<string, ConditionOperatorDelegate>( "<=", (left, right) => left != null && right != null && Comparer<object>.Default.Compare(left, Convert.ChangeType(right, left.GetType())) <= 0 ),
                new KeyValuePair<string, ConditionOperatorDelegate>( ">=", (left, right) => left != null && right != null && Comparer<object>.Default.Compare(left, Convert.ChangeType(right, left.GetType())) >= 0 ),
                new KeyValuePair<string, ConditionOperatorDelegate>( "contains", (left, right) => ((left is string) ? ((string)left).Contains((string)right) : (left is IEnumerable) ? Any((IEnumerable)left, (element) => element.BackCompatSafeTypeInsensitiveEqual(right)) : false) ),
                new KeyValuePair<string, ConditionOperatorDelegate>( "startsWith", (left, right) => (left is IList) ? EqualVariables(((IList)left).OfType<object>().FirstOrDefault(), right) : ((left is string) ? ((string)left).StartsWith((string)right) : false) ),
                new KeyValuePair<string, ConditionOperatorDelegate>( "endsWith", (left, right) => (left is IList) ? EqualVariables(((IList)left).OfType<object>().LastOrDefault(), right) : ((left is string) ? ((string)left).EndsWith((string)right) : false) ),
                new KeyValuePair<string, ConditionOperatorDelegate>( "hasKey", (left, right) => (left is IDictionary) ? ((IDictionary)left).Contains(right) : false ),
                new KeyValuePair<string, ConditionOperatorDelegate>("hasValue", (left, right) => (left is IDictionary) ? ((IDictionary)left).Values.Cast<object>().Contains(right) : false )
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Keys => _operators.Keys;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ConditionOperatorDelegate this[string key] => _operators[key];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ConditionOperatorDelegate Add(string key, ConditionOperatorDelegate value)
        {
            return _operators.AddOrUpdate(key, value, (k, v) => _operators[key] = value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            _operators.TryRemove(key, out _);
        }

        private static bool EqualVariables(object left, object right)
        {
            if (left is Symbol leftSymbol)
            {
                return leftSymbol.EvaluationFunction(right);
            }

            if (right is Symbol rightSymbol)
            {
                return rightSymbol.EvaluationFunction(left);
            }

            return left.SafeTypeInsensitiveEqual(right);
        }

        private static bool Any(IEnumerable enumerable, Func<object, bool> condition)
        {
            foreach (var obj in enumerable)
            {
                if (condition(obj))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Container for liquid nodes which conveniently wraps decision making logic
    ///
    /// Example:
    ///
    /// c = Condition.new('1', '==', '1')
    /// c.evaluate #=> true
    /// </summary>
    public class Condition
    {
        private string _childRelation;
        private Condition _childCondition;
        private readonly Operators _operators = new Operators();
        private readonly List<object> _attachments = new();
        private readonly Template _template;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public Condition(Template template)
        {
            _template = template;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Left { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Operator { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Right { get; }

        /// <summary>
        /// 
        /// </summary>
        public Operators Operators => _operators;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<object> Attachment => _attachments;

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsElse
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Condition(string left, string @operator, string right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        /// <summary>
        /// 
        /// </summary>
        public Condition()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public virtual bool Evaluate(Context context, IFormatProvider formatProvider)
        {
            context = context ?? new Context(formatProvider);
            bool result = InterpretCondition(Left, Right, Operator, context);

            switch (_childRelation)
            {
                case "or":
                    return result || _childCondition.Evaluate(context, formatProvider);
                case "and":
                    return result && _childCondition.Evaluate(context, formatProvider);
                default:
                    return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        public void Or(Condition condition)
        {
            _childRelation = "or";
            _childCondition = condition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="condition"></param>
        public void And(Condition condition)
        {
            _childRelation = "and";
            _childCondition = condition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public IEnumerable<object> Attach(NodeList attachment)
        {
            _attachments.AddRange(attachment.GetItems());
            return _attachments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<Condition {0} {1} {2}>", Left, Operator, Right);
        }

        private bool InterpretCondition(string left, string right, string op, Context context)
        {
            // If the operator is empty this means that the decision statement is just
            // a single variable. We can just poll this variable from the context and
            // return this as the result.
            if (string.IsNullOrEmpty(op))
            {
                object result = context[left, false];
                return (result != null && (!(result is bool) || (bool)result));
            }

            object leftObject = context[left];
            object rightObject = context[right];

            var opKey = _operators.Keys.FirstOrDefault(opk => opk.Equals(op)
                                                                || opk.ToLowerInvariant().Equals(op)
                                                                || _template.NamingConvention.OperatorEquals(opk, op)
                                                     );
            if (opKey == null)
            {
                throw new Exceptions.ArgumentException(Liquid.ResourceManager.GetString("ConditionUnknownOperatorException"), op);
            }

            return _operators[opKey](leftObject, rightObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ElseCondition : Condition
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool IsElse
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public override bool Evaluate(Context context, IFormatProvider formatProvider)
        {
            return true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public delegate bool ConditionOperatorDelegate(object left, object right);
}
