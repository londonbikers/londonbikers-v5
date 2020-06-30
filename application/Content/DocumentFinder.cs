using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Apollo.Models.Interfaces;
using Apollo.Utilities;

namespace Apollo.Content
{
	public class DocumentFinder : Finder
    {
        #region members
        /// <summary>
        /// DateTime.ToString() operations aren't respecting the application culture for some reason, so we're bodging it to work.
        /// </summary>
        private const string DateFormatter = "dd/MM/yyyy HH:mm:ss";
        #endregion

        #region constructors
        /// <summary>
		/// Creates a new DocumentFinder object.
		/// </summary>
		public DocumentFinder() 
		{
			var fields = new Hashtable();
			fields["Title"] = "f_title";
			fields["Author"] = "f_author";
			fields["Created"] = "f_creation_date";
			fields["Published"] = "f_publish_date";
			fields["Type"] = "Type";
			fields["LeadStatement"] = "f_lead_statement";
			fields["Abstract"] = "f_abstract";
			fields["Body"] = "f_body";
			fields["Version"] = "f_version";
			fields["Parent"] = "f_parent";
			fields["ActiveVersion"] = "f_active_version";
			fields["Status"] = "f_status";
			fields["Tags"] = "Tags";

			Fields = fields;
			SqlTable = "apollo_content";
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a collection of Document's relating a specific Section.
		/// </summary>
		public List<long> GetPublishedSectionDocuments(long sectionId, int limit) 
		{
			var command = new SqlCommand("GetPublishedDocumentsForSection") {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@SectionID", sectionId));
			command.Parameters.Add(new SqlParameter("@Limit", limit));
			return PerformCustomQuery(command);
		}

		/// <summary>
		/// Returns a collection of Document's relating a specific Section, with a specific Tag.
		/// </summary>
		public List<long> GetPublishedSectionDocumentsWithTag(long sectionId, ITag tag, int limit) 
		{
			var command = new SqlCommand("GetPublishedDocumentsForSectionWithTag") {CommandType = CommandType.StoredProcedure};
		    command.Parameters.Add(new SqlParameter("@SectionID", sectionId));
			command.Parameters.Add(new SqlParameter("@Tag", tag.Name));
			command.Parameters.Add(new SqlParameter("@Limit", limit));
			return PerformCustomQuery(command);
		}

		/// <summary>
		/// Allows a complex search to be performed for a set of document(s).
		/// </summary>
		public List<long> GetDocumentsByCriteria(string title, 
												string tag,
												string type, 
												List<string> status, 
												string author, 
												string site,
												DateTime rangeFrom,
												DateTime rangeTo,
												int limit) 
		{
			var criteriaCount = 0;
			var query = new StringBuilder();
			var criteria = new StringBuilder();
			query.AppendFormat("SELECT TOP {0} c.ID FROM apollo_content c ", limit);

			criteria.Append("WHERE ");
			if (title != string.Empty)
			{
				criteria.AppendFormat("f_title like '%{0}%'", Sql.ToSql(title));
				criteriaCount++;
			}

			if (tag != string.Empty)
			{
				if (criteriaCount > 0)
					criteria.Append(" and ");

				criteria.AppendFormat("contains(Tags, '{0}')", Sql.ToSql(tag));
				criteriaCount++;
			}

			if (site != string.Empty)
			{
				// add in the doc mappings join.
				query.Append(" INNER JOIN DocumentMappings dm ON dm.DocumentID = c.ID");
				query.Append(" INNER JOIN Sections s ON s.ID = dm.ParentSectionID ");

				if (criteriaCount > 0)
                    criteria.Append(" AND");

				criteria.AppendFormat(" s.ParentSiteID = {0}", site);
				criteriaCount++;
			}

			if (type != string.Empty)
			{
				if (criteriaCount > 0)
                    criteria.Append(" AND");

				criteria.AppendFormat(" c.Type = {0}", type);
				criteriaCount++;
			}

			if (status != null && status.Count > 0)
			{
				if (criteriaCount > 0)
					criteria.Append(" AND");

                if (status.Count == 1)
                {
                    criteria.AppendFormat(" c.f_status = '{0}'", status[0]);
                }
                else
                {
                    criteria.Append("(");
                    for (var i = 0; i < status.Count; i++)
                    {
                        var stat = status[i];
                        criteria.AppendFormat(" c.f_status = '{0}'", stat);
                        if (i < status.Count - 1)
                            criteria.Append(" OR");
                    }

                    criteria.Append(")");
                }

                criteriaCount++;
			}

			if (author != string.Empty)
			{
				if (criteriaCount > 0)
					criteria.Append(" AND");

				criteria.AppendFormat(" c.f_author = '{0}'", author);
				criteriaCount++;
			}

			if (rangeFrom != DateTime.MinValue && rangeTo != DateTime.MinValue)
			{
				if (criteriaCount > 0)
                    criteria.Append(" AND");

                criteria.AppendFormat(" (c.f_creation_date BETWEEN '{0}' AND '{1}')", rangeFrom.ToString(DateFormatter), rangeTo.ToString(DateFormatter));
			}

			query.Append(criteria.ToString());
			query.Append(" ORDER BY c.f_creation_date DESC");
			var command = new SqlCommand(query.ToString());
			return PerformCustomQuery(command);
		}
		#endregion
	}
}