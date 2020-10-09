/*
dotNetRDF is free and open source software licensed under the MIT License

-----------------------------------------------------------------------------

Copyright (c) 2009-2012 dotNetRDF Project (dotnetrdf-developer@lists.sf.net)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;

namespace VDS.RDF
{

    public class CompareToTests
    {
        private void CheckCombinations(List<INode> nodes)
        {
            this.CheckCombinations(nodes, new SparqlOrderingComparer());
        }

        private void CheckCombinations<T>(List<T> nodes) where T : IComparable<T>
        {
            this.CheckCombinations<T>(nodes, Comparer<T>.Default);
        }

        private void CheckCombinations(List<INode> nodes, IComparer<INode> comparer)
        {
            if (nodes.Count == 0) Assert.True(false, "No Input");

            Console.WriteLine("INode Typed Tests");
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    INode a = nodes[i];
                    INode b = nodes[j];
                    if (i == j || ReferenceEquals(a, b))
                    {
                        Assert.Equal(0, a.CompareTo(b));
                        Assert.Equal(0, b.CompareTo(a));
                        Console.WriteLine(i + " compareTo " + j + " => i == j");

                        Assert.Equal(0, comparer.Compare(a, b));
                        Assert.Equal(0, comparer.Compare(b, a));
                    }
                    else
                    {
                        int c = a.CompareTo(b);
                        int d = comparer.Compare(a, b);
                        Assert.Equal(c * -1, b.CompareTo(a));
                        Assert.Equal(d * -1, comparer.Compare(b, a));

                        if (c > 0)
                        {
                            Console.WriteLine(i + " compareTo " + j + " => i > j");
                        }
                        else if (c == 0)
                        {
                            Console.WriteLine(i + " compareTo " + j + " => i == j");
                        }
                        else
                        {
                            Console.WriteLine(i + " compareTo " + j + " => i < j");
                        }
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private void CheckCombinations<T>(List<T> nodes, IComparer<T> comparer)
        {
            if (nodes.Count == 0) Assert.True(false, "No Input");

            Console.WriteLine("Strongly Typed Tests - " + nodes.GetType().ToString());
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = 0; j < nodes.Count; j++)
                {
                    T a = nodes[i];
                    T b = nodes[j];
                    if (i == j || ReferenceEquals(a, b))
                    {
                        Assert.Equal(0, comparer.Compare(a, b));
                        Assert.Equal(0, comparer.Compare(b, a));
                        Console.WriteLine(i + " compareTo " + j + " => i == j");
                    }
                    else
                    {
                        int c = comparer.Compare(a, b);
                        Assert.Equal(c * -1, comparer.Compare(b, a));

                        if (c > 0)
                        {
                            Console.WriteLine(i + " compareTo " + j + " => i > j");
                        }
                        else if (c == 0)
                        {
                            Console.WriteLine(i + " compareTo " + j + " => i == j");
                        }
                        else
                        {
                            Console.WriteLine(i + " compareTo " + j + " => i < j");
                        }
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private void ShowOrdering(IEnumerable<INode> nodes)
        {
            Console.WriteLine("Standard Ordering");
            NTriplesFormatter formatter = new NTriplesFormatter();
            foreach (INode n in nodes.OrderBy(x => x))
            {
                Console.WriteLine(n.ToString(formatter));
            }

            Console.WriteLine();

            Console.WriteLine("SPARQL Ordering");
            foreach (INode n in nodes.OrderBy(x => x, (IComparer<INode>) new SparqlOrderingComparer()))
            {
                Console.WriteLine(n.ToString(formatter));
            }

            Console.WriteLine();
        }

        private void ShowOrdering(IEnumerable<INode> nodes, IComparer<INode> comparer)
        {
            Console.WriteLine("Standard Ordering");
            NTriplesFormatter formatter = new NTriplesFormatter();
            foreach (INode n in nodes.OrderBy(x => x))
            {
                Console.WriteLine(n.ToString(formatter));
            }

            Console.WriteLine();

            Console.WriteLine(comparer.GetType().Name + " Ordering");
            foreach (INode n in nodes.OrderBy(x => x, comparer))
            {
                Console.WriteLine(n.ToString(formatter));
            }

            Console.WriteLine();
        }

        [Fact]
        public void NodeCompareToBlankNodes()
        {
            Graph g = new Graph();
            Graph h = new Graph();

            IBlankNode b = g.CreateBlankNode();
            List<INode> nodes = new List<INode>()
            {
                b,
                g.CreateBlankNode(),
                g.CreateBlankNode("id"),
                h.CreateBlankNode(),
                h.CreateBlankNode("id"),
                b
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<IBlankNode>(nodes.OfType<IBlankNode>().ToList());
            this.CheckCombinations<BlankNode>(nodes.OfType<BlankNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodes()
        {
            Graph g = new Graph();

            ILiteralNode plain = g.CreateLiteralNode("plain");
            List<INode> nodes = new List<INode>()
            {
                plain,
                g.CreateLiteralNode("plain english", "en"),
                g.CreateLiteralNode("plain french", "fr"),
                g.CreateLiteralNode("typed", XmlSpecsHelper.XmlSchemaDataTypeStringUri),
                (1234).ToLiteral(g),
                (12.34m).ToLiteral(g),
                (12.34d).ToLiteral(g),
                (false).ToLiteral(g),
                (true).ToLiteral(g),
                plain
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToMalformedLiteralNodes()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToWithCompareOptions()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something"),
                g.CreateLiteralNode("Something"),
                g.CreateLiteralNode("thing")
            };

            CompareOptions current = Options.DefaultComparisonOptions;

            try
            {
                // Test each comparison mode
                foreach (CompareOptions comparison in Enum.GetValues(typeof(StringComparison)))
                {
                    Options.DefaultComparisonOptions = comparison;
                    this.ShowOrdering(nodes);

                    this.CheckCombinations(nodes);
                    this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
                    this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
                }
            }
            finally
            {
                Options.DefaultComparisonOptions = current;
            }
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdBytes()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdUnsignedBytes()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeUnsignedByteUri),
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeUnsignedByteUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeUnsignedByteUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeUnsignedByteUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeUnsignedByteUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdIntegers()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdShorts()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeShortUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeShortUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeShortUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeShortUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeShortUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdLongs()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeLongUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeLongUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeLongUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeLongUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeLongUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdUnsignedIntegers()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeUnsignedIntUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeUnsignedIntUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeUnsignedIntUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeUnsignedIntUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeUnsignedIntUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdUnsignedShorts()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeUnsignedShortUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeUnsignedShortUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeUnsignedShortUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeUnsignedShortUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeUnsignedShortUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdUnsignedLongs()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeUnsignedLongUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeUnsignedLongUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeUnsignedLongUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeUnsignedLongUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeUnsignedLongUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdNegativeIntegers()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeNegativeIntegerUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeNegativeIntegerUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeNegativeIntegerUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeNegativeIntegerUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeNegativeIntegerUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdNonPositiveIntegers()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeNonPositiveIntegerUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeNonPositiveIntegerUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeNonPositiveIntegerUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeNonPositiveIntegerUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeNonPositiveIntegerUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdNonNegativeIntegers()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeNonNegativeIntegerUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeNonNegativeIntegerUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeNonNegativeIntegerUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeNonNegativeIntegerUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeNonNegativeIntegerUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdPositiveIntegers()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypePositiveIntegerUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypePositiveIntegerUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypePositiveIntegerUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypePositiveIntegerUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypePositiveIntegerUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdHexBinary()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeHexBinaryUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeHexBinaryUri),
                g.CreateLiteralNode((1).ToString("X2"), XmlSpecsHelper.XmlSchemaDataTypeHexBinaryUri),
                g.CreateLiteralNode((10).ToString("X2"), XmlSpecsHelper.XmlSchemaDataTypeHexBinaryUri),
                g.CreateLiteralNode("-03", XmlSpecsHelper.XmlSchemaDataTypeHexBinaryUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdDoubles()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("1.2e4", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("1.2e3", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("1.2", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("1.2e1", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("1.2E4", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("1.2e0", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("1.2e-1", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("10e14", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdFloats()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2e4", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2e3", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2e1", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2E4", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2e0", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2e-1", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("10e14", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdUris()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("http://example.org", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("http://example.org:8080", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("http://example.org/path", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("ftp://ftp.example.org", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("mailto:someone@example.org", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("ex:custom", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToLiteralNodesXsdDateTimes()
        {
            Graph g = new Graph();

            List<INode> nodes = new List<INode>()
            {
                g.CreateLiteralNode("something", XmlSpecsHelper.XmlSchemaDataTypeDateTimeUri),
                DateTime.Now.ToLiteral(g),
                DateTime.Now.AddYears(3).AddDays(1).ToLiteral(g),
                DateTime.Now.AddYears(-25).AddMinutes(-17).ToLiteral(g),
                new DateTime(1, 2, 3, 4, 5, 6).ToLiteral(g),
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,
                    DateTime.Now.Minute, DateTime.Now.Second, DateTimeKind.Utc).ToLiteral(g),
                g.CreateLiteralNode("thing", XmlSpecsHelper.XmlSchemaDataTypeDateTimeUri),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeDateTimeUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeDateTimeUri),
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<ILiteralNode>(nodes.OfType<ILiteralNode>().ToList());
            this.CheckCombinations<LiteralNode>(nodes.OfType<LiteralNode>().ToList());
        }

        [Fact]
        public void NodeCompareToUriNodes()
        {
            Graph g = new Graph();

            IUriNode u = g.CreateUriNode("rdf:type");
            List<INode> nodes = new List<INode>()
            {
                u,
                g.CreateUriNode(new Uri("http://example.org")),
                g.CreateUriNode(new Uri("http://example.org:80")),
                g.CreateUriNode(new Uri("http://example.org:8080")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org/configuration#")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org/Configuration#")),
                g.CreateUriNode(new Uri("mailto:rvesse@dotnetrdf.org")),
                u,
                g.CreateUriNode(new Uri("ftp://ftp.example.org")),
                g.CreateUriNode(new Uri("ftp://anonymous@ftp.example.org")),
                g.CreateUriNode(new Uri("ftp://user:password@ftp.example.org"))
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
            this.CheckCombinations<IUriNode>(nodes.OfType<IUriNode>().ToList());
            this.CheckCombinations<UriNode>(nodes.OfType<UriNode>().ToList());
        }

        [Fact]
        public void NodeCompareToMixedNodes()
        {
            Graph g = new Graph();
            Graph h = new Graph();

            IBlankNode b = g.CreateBlankNode();
            ILiteralNode plain = g.CreateLiteralNode("plain");
            IUriNode u = g.CreateUriNode("rdf:type");
            List<INode> nodes = new List<INode>()
            {
                b,
                g.CreateBlankNode(),
                g.CreateBlankNode("id"),
                h.CreateBlankNode(),
                h.CreateBlankNode("id"),
                b,
                plain,
                g.CreateLiteralNode("plain english", "en"),
                g.CreateLiteralNode("plain french", "fr"),
                g.CreateLiteralNode("typed", XmlSpecsHelper.XmlSchemaDataTypeStringUri),
                (1234).ToLiteral(g),
                (12.34m).ToLiteral(g),
                (12.34d).ToLiteral(g),
                (false).ToLiteral(g),
                (true).ToLiteral(g),
                plain,
                u,
                g.CreateUriNode(new Uri("http://example.org")),
                g.CreateUriNode(new Uri("http://example.org:8080")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org/configuration#")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org/Configuration#")),
                g.CreateUriNode(new Uri("mailto:rvesse@dotnetrdf.org")),
                u
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
        }

        [Fact]
        public void NodeCompareToMixedNodes3()
        {
            Graph g = new Graph();
            Graph h = new Graph();

            IBlankNode b = g.CreateBlankNode();
            ILiteralNode plain = g.CreateLiteralNode("plain");
            IUriNode u = g.CreateUriNode("rdf:type");
            List<INode> nodes = new List<INode>()
            {
                b,
                g.CreateBlankNode(),
                g.CreateBlankNode("id"),
                g.CreateLiteralNode("1.2e3", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("1.2e1", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeShortUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeShortUri),
                g.CreateLiteralNode("1.2E4", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("http://example.org:8080", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("http://example.org/path", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("ftp://ftp.example.org", XmlSpecsHelper.XmlSchemaDataTypeAnyUriUri),
                g.CreateLiteralNode("1.2e0", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                DateTime.Now.ToLiteral(g),
                DateTime.Now.AddYears(3).AddDays(1).ToLiteral(g),
                DateTime.Now.AddYears(-25).AddMinutes(-17).ToLiteral(g),
                g.CreateLiteralNode("1.2e-1", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                g.CreateLiteralNode("10e14", XmlSpecsHelper.XmlSchemaDataTypeFloatUri),
                h.CreateBlankNode(),
                h.CreateBlankNode("id"),
                b,
                plain,
                g.CreateLiteralNode("plain english", "en"),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri),
                g.CreateLiteralNode("plain french", "fr"),
                g.CreateLiteralNode("typed", XmlSpecsHelper.XmlSchemaDataTypeStringUri),
                (1234).ToLiteral(g),
                (12.34m).ToLiteral(g),
                (12.34d).ToLiteral(g),
                (false).ToLiteral(g),
                g.CreateLiteralNode((1).ToString("X2"), XmlSpecsHelper.XmlSchemaDataTypeHexBinaryUri),
                g.CreateLiteralNode((10).ToString("X2"), XmlSpecsHelper.XmlSchemaDataTypeHexBinaryUri),
                (true).ToLiteral(g),
                plain,
                u,
                g.CreateUriNode(new Uri("http://example.org")),
                g.CreateUriNode(new Uri("http://example.org:8080")),
                g.CreateLiteralNode("1", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
                g.CreateLiteralNode("10", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
                g.CreateLiteralNode("-3", XmlSpecsHelper.XmlSchemaDataTypeByteUri),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org/configuration#")),
                g.CreateUriNode(new Uri("http://www.dotnetrdf.org/Configuration#")),
                g.CreateUriNode(new Uri("mailto:rvesse@dotnetrdf.org")),
                u
            };

            this.ShowOrdering(nodes);

            this.CheckCombinations(nodes);
        }

        [Fact]
        public void NodeCompareToMixedNodes2()
        {
            Graph g = new Graph();
            IBlankNode b = g.CreateBlankNode();
            ILiteralNode l = g.CreateLiteralNode("literal", "en");
            IUriNode u = g.CreateUriNode(new Uri("http://example.org"));
            IVariableNode v = g.CreateVariableNode("var");

            int c = b.CompareTo(l);
            Assert.Equal(c * -1, l.CompareTo(b));
            c = b.CompareTo(u);
            Assert.Equal(c * -1, u.CompareTo(b));
            c = b.CompareTo(v);
            Assert.Equal(c * -1, v.CompareTo(b));

            c = l.CompareTo(b);
            Assert.Equal(c * -1, b.CompareTo(l));
            c = l.CompareTo(u);
            Assert.Equal(c * -1, u.CompareTo(l));
            c = l.CompareTo(v);
            Assert.Equal(c * -1, v.CompareTo(l));

            c = u.CompareTo(b);
            Assert.Equal(c * -1, b.CompareTo(u));
            c = u.CompareTo(l);
            Assert.Equal(c * -1, l.CompareTo(u));
            c = u.CompareTo(v);
            Assert.Equal(c * -1, v.CompareTo(u));

            c = v.CompareTo(b);
            Assert.Equal(c * -1, b.CompareTo(v));
            c = v.CompareTo(l);
            Assert.Equal(c * -1, l.CompareTo(v));
            c = v.CompareTo(u);
            Assert.Equal(c * -1, u.CompareTo(v));
        }

        //[Fact]
        //public void NodeCompareToRdfXml()
        //{
        //    Graph g = new Graph();
        //    List<INode> nodes = new List<INode>()
        //    {
        //        g.CreateBlankNode(),
        //        g.CreateUriNode("rdf:type"),
        //        g.CreateBlankNode("node"),
        //        g.CreateUriNode("rdfs:Class"),
        //        g.CreateLiteralNode("string")
        //    };

        //    NTriplesFormatter formatter = new NTriplesFormatter();

        //    Console.WriteLine("Normal Sort Order:");
        //    nodes.Sort();
        //    foreach (INode n in nodes)
        //    {
        //        Console.WriteLine(n.ToString(formatter));
        //    }

        //    Console.WriteLine();
        //    Console.WriteLine("RDF/XML Sort Order:");
        //    nodes.Sort(new RdfXmlTripleComparer());
        //    foreach (INode n in nodes)
        //    {
        //        Console.WriteLine(n.ToString(formatter));
        //    }
        //}

        [Fact]
        public void NodeCompareToEquivalentLiterals1()
        {
            Graph g = new Graph();
            ILiteralNode canonical = (1).ToLiteral(g);
            ILiteralNode alternate =
                g.CreateLiteralNode("01", XmlSpecsHelper.XmlSchemaDataTypeIntegerUri);

            List<INode> ns = new List<INode>()
            {
                canonical,
                alternate
            };

            Assert.NotEqual(canonical, alternate);
            Assert.Equal(0, canonical.CompareTo(alternate));

            this.ShowOrdering(ns);
            this.CheckCombinations(ns);
            this.CheckCombinations<ILiteralNode>(ns.OfType<ILiteralNode>().ToList());
            this.CheckCombinations(ns, new FastNodeComparer());
        }

        [Fact]
        public void NodeCompareToEquivalentLiterals2()
        {
            Graph g = new Graph();
            ILiteralNode canonical = (true).ToLiteral(g);
            ILiteralNode alternate =
                g.CreateLiteralNode("TRUE", XmlSpecsHelper.XmlSchemaDataTypeBooleanUri);

            List<INode> ns = new List<INode>()
            {
                canonical,
                alternate
            };

            Assert.NotEqual(canonical, alternate);
            Assert.Equal(0, canonical.CompareTo(alternate));

            this.ShowOrdering(ns);
            this.CheckCombinations(ns);
            this.CheckCombinations<ILiteralNode>(ns.OfType<ILiteralNode>().ToList());
            this.CheckCombinations(ns, new FastNodeComparer());
        }

        [Fact]
        public void NodeCompareToEquivalentLiterals3()
        {
            Graph g = new Graph();
            ILiteralNode canonical = (1d).ToLiteral(g);
            ILiteralNode alternate =
                g.CreateLiteralNode("1.00000", XmlSpecsHelper.XmlSchemaDataTypeDoubleUri);

            List<INode> ns = new List<INode>()
            {
                canonical,
                alternate
            };

            Assert.NotEqual(canonical, alternate);
            Assert.Equal(0, canonical.CompareTo(alternate));

            this.ShowOrdering(ns);
            this.CheckCombinations(ns);
            this.CheckCombinations<ILiteralNode>(ns.OfType<ILiteralNode>().ToList());
            this.CheckCombinations(ns, new FastNodeComparer());
        }
    }
}
