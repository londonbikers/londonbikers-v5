using System.IO;
using System.Xml;
using Apollo.Utilities.Files.Models;

namespace Apollo.Utilities.Files
{
    public static class MetadataParser
    {
        /// <summary>
        /// Retrieves XMP meta-data embedded in a file (commonly an image) and returns a MetaData object.
        /// </summary>
        public static MetaData RetrieveMetaData(string filePath, bool deepRetrieval)
        {
            var xml = GetXmpXmlDocFromImage(filePath);
            return string.IsNullOrEmpty(xml) ? null : ParseMetaData(xml, deepRetrieval);
        }

        /// <summary>
        /// Retrieves the RDF formatted XMP xml data that may be embedded in an image file.
        /// </summary>
        private static string GetXmpXmlDocFromImage(string filename)
        {
            const string beginCapture = "<rdf:RDF";
            const string endCapture = "</rdf:RDF>";
            var collection = string.Empty;
            var collecting = false;
            var matching = false;
            var collectionCount = 0;

            try
            {
                using (var sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        var contents = (char)sr.Read();
                        if (!matching && !collecting && contents == '<')
                            matching = true;

                        if (matching)
                        {
                            collection += contents;

                            if (collection.Contains(beginCapture))
                            {
                                // found the begin element we can stop matching and start collecting.
                                matching = false;
                                collecting = true;
                            }
                            else if (contents == beginCapture[collectionCount++])
                            {
                                // we are still looking, but on track to start collecting.
                                continue;
                            }
                            else
                            {
                                // false start reset everything.
                                collection = string.Empty;
                                matching = false;
                                collectionCount = 0;
                            }
                        }
                        else if (collecting)
                        {
                            collection += contents;
                            if (collection.Contains(endCapture))
                            {
                                // we are finished found the end of the XMP data.
                                break;
                            }
                        }
                    }
                }
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }

            return collection;
        }

        /// <summary>
        /// Parses out useful XMP data from the xml string.
        /// </summary>
        private static MetaData ParseMetaData(string xmpXmlDoc, bool includeExtendedData)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xmpXmlDoc);

            // determine meta-data format.
            // -- only one supported format for now...
            var md = new Lightroom().ParseMetaData(doc, includeExtendedData);

            // add support for:
            // -- Photo Mechanic

            return md;
        } 
    }
}