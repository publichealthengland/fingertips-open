﻿using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace PholioVisualisation.SearchIndexing
{
    /// <summary>
    /// Custom analyser that enables synonyms to be added to the indexes.
    /// </summary>
    public class SynonymAnalyzer : Analyzer
    {
        public override TokenStream TokenStream (string fieldName, TextReader reader)
        {
            //create the tokenizer
            TokenStream tokenStream = new StandardTokenizer(Version.LUCENE_29, AttributeSource.AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, reader);

            //add in filters
            // first normalize the StandardTokenizer
            tokenStream = new StandardFilter(tokenStream);

            // makes sure everything is lower case
            tokenStream = new LowerCaseFilter(tokenStream);

            // use the default list of Stop Words, provided by the StopAnalyzer class.
            tokenStream = new StopFilter(false, tokenStream, StopAnalyzer.ENGLISH_STOP_WORDS_SET);

            // injects the synonyms. 
            tokenStream = new SynonymFilter(tokenStream);

            //return the built token stream.
            return tokenStream;
        }
    }
}