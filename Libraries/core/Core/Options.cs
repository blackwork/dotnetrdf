﻿/*

Copyright Robert Vesse 2009-10
rvesse@vdesign-studios.com

------------------------------------------------------------------------

This file is part of dotNetRDF.

dotNetRDF is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

dotNetRDF is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with dotNetRDF.  If not, see <http://www.gnu.org/licenses/>.

------------------------------------------------------------------------

dotNetRDF may alternatively be used under the LGPL or MIT License

http://www.gnu.org/licenses/lgpl.html
http://www.opensource.org/licenses/mit-license.php

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms.

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using VDS.RDF.Query;

namespace VDS.RDF
{
    /// <summary>
    /// Possible Literal Equality Mode Settings
    /// </summary>
    public enum LiteralEqualityMode {
        /// <summary>
        /// Strict Mode compares Literals according to the official W3C RDF Specification
        /// </summary>
        /// <remarks>
        /// This means Literals are equal if and only if:
        /// <ol>
        /// <li>The Lexical Values are identical when compared using Ordinal Comparison</li>
        /// <li>The Language Tags if present are identical</li>
        /// <li>The Datatypes if present are identical</li>
        /// </ol>
        /// </remarks>
        Strict,
        /// <summary>
        /// Loose Mode compares Literals based on values (if they have known Datatypes)
        /// </summary>
        /// <remarks>
        /// This means Literals can be equal if they have lexically different values which are equivalent when converted to the Datatype.
        /// <br /><br />
        /// Literals without Datatypes and those whose Datatypes are unknown or not handled by the Library will be compared using lexical equivalence as with <see cref="LiteralEqualityMode.Strict">Strict</see> mode.
        /// </remarks>
        Loose
    }

    /// <summary>
    /// Configures Global Static Options for the Library
    /// </summary>
    /// <remarks>
    /// Some of these are Debug Build only, please see the Remarks on individual members for more detail
    /// </remarks>
    public static class Options
    {
        private static LiteralEqualityMode _litEqualityMode = LiteralEqualityMode.Strict;
        private static long _queryExecutionTimeout = 300000;
        private static int _defaultCompressionLevel = WriterCompressionLevel.More;
        private static bool _fullIndexing = true;
        private static bool _queryOptimisation = true;
        private static bool _algebraOptimisation = true;
        private static SparqlQuerySyntax _queryDefaultSyntax = SparqlQuerySyntax.Extended;
        private static SparqlEngine _queryEngine = SparqlEngine.Leviathan;//SparqlEngine.Labyrinth;
        private static bool _queryAllowUnknownFunctions = true;
        private static bool _uriLoaderCaching = true;
        private static int _uriLoaderTimeout = 15000;

        #if DEBUG
        //Debug Build Only
        private static bool _httpDebug = false;
        private static bool _httpFullDebug = false;
        #endif

        /// <summary>
        /// Gets/Sets the Mode used to compute Literal Equality (Default is <see cref="LiteralEqualityMode.Strict">Strict</see> which enforces the W3C RDF Specification)
        /// </summary>
        public static LiteralEqualityMode LiteralEqualityMode
        {
            get
            {
                return _litEqualityMode;
            }
            set
            {
                _litEqualityMode = value;
            }
        }

        /// <summary>
        /// Gets/Sets the Hard Timeout limit for Sparql Query Execution (in milliseconds)
        /// </summary>
        /// <remarks>
        /// This is used to stop Sparql queries running away and never completing execution, it defaults to 5 mins (300,000 milliseconds)
        /// </remarks>
        public static long QueryExecutionTimeout
        {
            get
            {
                return _queryExecutionTimeout;
            }
            set
            {
                if (value > 0)
                {
                    _queryExecutionTimeout = value;
                }
            }
        }

        /// <summary>
        /// Gets/Sets whether Query Optimisation should be used
        /// </summary>
        public static bool QueryOptimisation
        {
            get
            {
                return _queryOptimisation;
            }
            set
            {
                _queryOptimisation = value;
            }
        }

        /// <summary>
        /// Gets/Sets whether Algebra Optimisation should be used
        /// </summary>
        public static bool AlgebraOptimisation
        {
            get
            {
                return _algebraOptimisation;
            }
            set
            {
                _algebraOptimisation = value;
            }
        }

        /// <summary>
        /// Gets/Sets the default syntax used for parsing SPARQL queries
        /// </summary>
        public static SparqlQuerySyntax QueryDefaultSyntax
        {
            get
            {
                return _queryDefaultSyntax;
            }
            set
            {
                _queryDefaultSyntax = value;
            }
        }

        /// <summary>
        /// Gets/Sets the engine used for processing SPARQL queries
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default engine is the <see cref="SparqlEngine.Leviathan">Leviathan</see> engine
        /// </para>
        /// <para>
        /// As of the 0.3.0 release there will once again be only one SPARQL engine present in the library since the obsolete Labyrinth engine has been removed
        /// </para>
        /// </remarks>
        public static SparqlEngine QueryEngine
        {
            get
            {
                return _queryEngine;
            }
            set
            {
                _queryEngine = value;
            }
        }

        /// <summary>
        /// Gets/Sets whether functions that can't be parsed into Expressions should be represented by the <see cref="NullExpression">NullExpression</see>
        /// </summary>
        /// <remarks>When set to false a Parser Error will be thrown if the Function cannot be parsed into an Expression</remarks>
        public static bool QueryAllowUnknownFunctions
        {
            get
            {
                return _queryAllowUnknownFunctions;
            }
            set
            {
                _queryAllowUnknownFunctions = value;
            }
        }

        /// <summary>
        /// Gets/Sets the Default Compression Level used for Writers returned by the <see cref="MimeTypesHelper">MimeTypesHelper</see> class when the writers implement <see cref="ICompressingWriter">ICompressingWriter</see>
        /// </summary>
        public static int DefaultCompressionLevel
        {
            get
            {
                return _defaultCompressionLevel;
            }
            set
            {
                _defaultCompressionLevel = value;
            }
        }

        /// <summary>
        /// Controls whether the <see cref="IndexedTripleCollection">IndexedTripleCollection</see> will create full indexes for the Triples inserted into it
        /// </summary>
        /// <remarks>
        /// <para>
        /// By default the <see cref="IndexedTripleCollection">IndexedTripleCollection</see> creates indexes on Triples based upon Subjects, Predicates and Objects alone.  When full indexing is enabled it also creates indexes based on Subject-Predicate, Predicate-Object and Subject-Object pairs which may improve query speed but will use additional memory.
        /// </para>
        /// <para>
        /// Default setting for Full Indexing is enabled, enabling/disabling it only has an effect on <see cref="IndexedTripleCollection">IndexedTripleCollection</see> instances instantiated after full indexing was enabled/disabled i.e. existing Graphs in memory using the <see cref="IndexedTripleCollection">IndexedTripleCollection</see> continue to use the full indexing setting that was present when they were instantiated.
        /// </para>
        /// </remarks>
        public static bool FullTripleIndexing
        {
            get
            {
                return _fullIndexing;
            }
            set
            {
                _fullIndexing = value;
            }
        }

#if !NO_URICACHE

        /// <summary>
        /// Gets/Sets whether the <see cref="UriLoader">UriLoader</see> uses caching
        /// </summary>
        public static bool UriLoaderCaching
        {
            get
            {
                return _uriLoaderCaching;
            }
            set
            {
                _uriLoaderCaching = value;
            }
        }
#endif

        /// <summary>
        /// Gets/Sets the Timeout for URI Loader requests (Defaults to 15 seconds)
        /// </summary>
        public static int UriLoaderTimeout
        {
            get
            {
                return _uriLoaderTimeout;
            }
            set
            {
                if (value > 0)
                {
                    _uriLoaderTimeout = value;
                }
            }
        }

        #region Debug Build Only Options

        #if DEBUG

        /// <summary>
        /// Gets/Sets whether HTTP Request and Response Information should be output to the Console Standard Out for Debugging purposes
        /// </summary>
        /// <remarks>
        /// <strong>Only available in Debug builds</strong>
        /// <br /><br />
        /// This does not guarentee that this information is output to the Console Standard Out, most code that makes HTTP requests should do this but it may vary depending on the Library build and the exact classes used.
        /// </remarks>
        public static bool HttpDebugging {
            get
            {
                return _httpDebug;
            }
            set
            {
                _httpDebug = value;
            }
        }

        /// <summary>
        /// Gets/Sets whether the HTTP Response Stream should be output to the Console Standard Output for Debugging purposes
        /// </summary>
        /// <remarks>
        /// <strong>Only available in Debug builds</strong>
        /// <br /><br />
        /// Only applies if <see cref="HttpDebugging">HttpDebugging</see> is enabled
        /// </remarks>
        public static bool HttpFullDebugging
        {
            get
            {
                return _httpFullDebug;
            }
            set
            {
                _httpFullDebug = value;
            }
        }

        #endif

        #endregion
    }
}
