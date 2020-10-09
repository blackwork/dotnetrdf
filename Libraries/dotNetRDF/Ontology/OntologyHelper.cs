/*
// <copyright>
// dotNetRDF is free and open source software licensed under the MIT License
// -------------------------------------------------------------------------
// 
// Copyright (c) 2009-2020 dotNetRDF Project (http://dotnetrdf.org/)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
*/

using System;

namespace VDS.RDF.Ontology
{
    /// <summary>
    /// Static Helper class for the Ontology API.
    /// </summary>
    /// <remarks>
    /// <para>
    /// See <a href="http://www.dotnetrdf.org/content.asp?pageID=Ontology%20API">Using the Ontology API</a> for some informal documentation on the use of the Ontology namespace.
    /// </para>
    /// </remarks>
    public static class OntologyHelper
    {
        /// <summary>
        /// Constant URIs for properties exposed by <see cref="OntologyResource">OntologyResource</see> and its derived classes.
        /// </summary>
        public const String PropertyVersionInfo = NamespaceMapper.OWL + "versionInfo",
                            PropertySameAs = NamespaceMapper.OWL + "sameAs",
                            PropertyDifferentFrom = NamespaceMapper.OWL + "differentFrom",
                            PropertyEquivalentClass = NamespaceMapper.OWL + "equivalentClass",
                            PropertyDisjointWith = NamespaceMapper.OWL + "disjointWith",
                            PropertyEquivalentProperty = NamespaceMapper.OWL + "equivalentProperty",
                            PropertyInverseOf = NamespaceMapper.OWL + "inverseOf",
                            PropertyBackwardCompatibleWith = NamespaceMapper.OWL + "backwardCompatibleWith",
                            PropertyIncompatibleWith = NamespaceMapper.OWL + "incompatibleWith",
                            PropertyPriorVersion = NamespaceMapper.OWL + "priorVersion",
                            PropertyImports = NamespaceMapper.OWL + "imports",

                            PropertyComment = NamespaceMapper.RDFS + "comment",
                            PropertyLabel = NamespaceMapper.RDFS + "label",
                            PropertySeeAlso = NamespaceMapper.RDFS + "seeAlso",
                            PropertyIsDefinedBy = NamespaceMapper.RDFS + "isDefinedBy",
                            PropertySubClassOf = NamespaceMapper.RDFS + "subClassOf",
                            PropertySubPropertyOf = NamespaceMapper.RDFS + "subPropertyOf",
                            PropertyRange = NamespaceMapper.RDFS + "range",
                            PropertyDomain = NamespaceMapper.RDFS + "domain",

                            PropertyType = NamespaceMapper.RDF + "type";

        /// <summary>
        /// Constant URIs for properties exposed by <see cref="OntologyResource">OntologyResource</see> and its derived classes.
        /// </summary>
        public static readonly Uri PropertyVersionInfoUri = UriFactory.Create(PropertyVersionInfo),
                                   PropertySameAsUri = UriFactory.Create(PropertySameAs),
                                   PropertyDifferentFromUri = UriFactory.Create(PropertyDifferentFrom),
                                   PropertyEquivalentClassUri = UriFactory.Create(PropertyEquivalentClass),
                                   PropertyDisjointWithUri = UriFactory.Create(PropertyDisjointWith),
                                   PropertyEquivalentPropertyUri = UriFactory.Create(PropertyEquivalentProperty),
                                   PropertyInverseOfUri = UriFactory.Create(PropertyInverseOf),
                                   PropertyBackwardCompatibleWithUri = UriFactory.Create(PropertyBackwardCompatibleWith),
                                   PropertyIncompatibleWithUri = UriFactory.Create(PropertyIncompatibleWith),
                                   PropertyPriorVersionUri = UriFactory.Create(PropertyPriorVersion),
                                   PropertyImportsUri = UriFactory.Create(PropertyImports),
                                
                                   PropertyCommentUri = UriFactory.Create(PropertyComment),
                                   PropertyLabelUri = UriFactory.Create(PropertyLabel),
                                   PropertySeeAlsoUri = UriFactory.Create(PropertySeeAlso),
                                   PropertyIsDefinedByUri = UriFactory.Create(PropertyIsDefinedBy),
                                   PropertySubClassOfUri = UriFactory.Create(PropertySubClassOf),
                                   PropertySubPropertyOfUri = UriFactory.Create(PropertySubPropertyOf),
                                   PropertyRangeUri = UriFactory.Create(PropertyRange),
                                   PropertyDomainUri = UriFactory.Create(PropertyDomain),
                                
                                   PropertyTypeUri = UriFactory.Create(PropertyType);


        /// <summary>
        /// Constants for URIs for classes in Ontologies.
        /// </summary>
        public const String RdfsClass = NamespaceMapper.RDFS + "Class",
                            OwlClass = NamespaceMapper.OWL + "Class",
                            RdfProperty = NamespaceMapper.RDF + "Property",
                            RdfsResource = NamespaceMapper.RDFS + "Resource",
                            OwlObjectProperty = NamespaceMapper.OWL + "ObjectProperty",
                            OwlDatatypeProperty = NamespaceMapper.OWL + "DatatypeProperty",
                            OwlAnnotationProperty = NamespaceMapper.OWL + "AnnotationProperty",
                            OwlOntology = NamespaceMapper.OWL + "Ontology";

        /// <summary>
        /// Constants for URIs for classes in Ontologies.
        /// </summary>
        public static readonly Uri RdfsClassUri = UriFactory.Create(RdfsClass),
                                   OwlClassUri = UriFactory.Create(OwlClass),
                                   RdfPropertyUri = UriFactory.Create(RdfProperty),
                                   RdfsResourceUri = UriFactory.Create(RdfsResource),
                                   OwlObjectPropertyUri = UriFactory.Create(OwlObjectProperty),
                                   OwlDatatypePropertyUri = UriFactory.Create(OwlDatatypeProperty),
                                   OwlAnnotationPropertyUri = UriFactory.Create(OwlAnnotationProperty),
                                   OwlOntologyUri = UriFactory.Create(OwlOntology);

    }
}
