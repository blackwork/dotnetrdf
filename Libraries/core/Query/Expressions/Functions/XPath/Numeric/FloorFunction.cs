﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF.Nodes;

namespace VDS.RDF.Query.Expressions.Functions.XPath.Numeric
{
    /// <summary>
    /// Represents the XPath fn:floor() function
    /// </summary>
    public class FloorFunction
        : BaseUnaryExpression
    {
        /// <summary>
        /// Creates a new XPath Floor function
        /// </summary>
        /// <param name="expr">Expression</param>
        public FloorFunction(ISparqlExpression expr)
            : base(expr) { }

        /// <summary>
        /// Gets the Numeric Value of the function as evaluated in the given Context for the given Binding ID
        /// </summary>
        /// <param name="context">Evaluation Context</param>
        /// <param name="bindingID">Binding ID</param>
        /// <returns></returns>
        public override IValuedNode Evaluate(SparqlEvaluationContext context, int bindingID)
        {
            IValuedNode a = this._expr.Evaluate(context, bindingID);
            if (a == null) throw new RdfQueryException("Cannot calculate an arithmetic expression on a null");

            switch (a.NumericType)
            {
#if !SILVERLIGHT
                case SparqlNumericType.Integer:
                    try
                    {
                        return new LongNode(null, Convert.ToInt64(Math.Floor(a.AsDecimal())));
                    }
                    catch (RdfQueryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new RdfQueryException("Unable to cast floor value of integer to an integer", ex);
                    }

                case SparqlNumericType.Decimal:
                    return new DecimalNode(null, Math.Floor(a.AsDecimal()));
#else
                case SparqlNumericType.Integer:
                case SparqlNumericType.Decimal:
#endif

                case SparqlNumericType.Float:
                    try
                    {
                        return new FloatNode(null, Convert.ToSingle(Math.Floor(a.AsDouble())));
                    }
                    catch (RdfQueryException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        throw new RdfQueryException("Unable to cast floor value of float to a float", ex);
                    }

                case SparqlNumericType.Double:
                    return new DoubleNode(null, Math.Floor(a.AsDouble()));

                default:
                    throw new RdfQueryException("Cannot evalute an Arithmetic Expression when the Numeric Type of the expression cannot be determined");
            }
        }

        /// <summary>
        /// Gets the String representation of the function
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "<" + XPathFunctionFactory.XPathFunctionsNamespace + XPathFunctionFactory.Floor + ">(" + this._expr.ToString() + ")";
        }

        /// <summary>
        /// Gets the Type of the Expression
        /// </summary>
        public override SparqlExpressionType Type
        {
            get
            {
                return SparqlExpressionType.Function;
            }
        }

        /// <summary>
        /// Gets the Functor of the Expression
        /// </summary>
        public override string Functor
        {
            get
            {
                return XPathFunctionFactory.XPathFunctionsNamespace + XPathFunctionFactory.Floor;
            }
        }

        /// <summary>
        /// Transforms the Expression using the given Transformer
        /// </summary>
        /// <param name="transformer">Expression Transformer</param>
        /// <returns></returns>
        public override ISparqlExpression Transform(IExpressionTransformer transformer)
        {
            return new FloorFunction(transformer.Transform(this._expr));
        }
    }
}