using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:mail_attachment
	/// </summary>
	public partial class mail_attachment
	{
		public mail_attachment()
		{}
		#region  Method



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(XHD.Model.mail_attachment model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into mail_attachment(");
			strSql.Append("mail_id,page_id,file_id,file_name,real_name,create_id,create_name,create_date)");
			strSql.Append(" values (");
			strSql.Append("@mail_id,@page_id,@file_id,@file_name,@real_name,@create_id,@create_name,@create_date)");
			SqlParameter[] parameters = {
					new SqlParameter("@mail_id", SqlDbType.Int,4),
					new SqlParameter("@page_id", SqlDbType.VarChar,250),
					new SqlParameter("@file_id", SqlDbType.VarChar,250),
					new SqlParameter("@file_name", SqlDbType.VarChar,250),
					new SqlParameter("@real_name", SqlDbType.VarChar,250),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@create_date", SqlDbType.DateTime)};
			parameters[0].Value = model.mail_id;
			parameters[1].Value = model.page_id;
			parameters[2].Value = model.file_id;
			parameters[3].Value = model.file_name;
			parameters[4].Value = model.real_name;
			parameters[5].Value = model.create_id;
			parameters[6].Value = model.create_name;
			parameters[7].Value = model.create_date;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.mail_attachment model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update mail_attachment set ");
			strSql.Append("mail_id=@mail_id,");
			strSql.Append("page_id=@page_id,");
			strSql.Append("file_id=@file_id,");
			strSql.Append("file_name=@file_name,");
			strSql.Append("real_name=@real_name,");
			strSql.Append("create_id=@create_id,");
			strSql.Append("create_name=@create_name,");
			strSql.Append("create_date=@create_date");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@mail_id", SqlDbType.Int,4),
					new SqlParameter("@page_id", SqlDbType.VarChar,250),
					new SqlParameter("@file_id", SqlDbType.VarChar,250),
					new SqlParameter("@file_name", SqlDbType.VarChar,250),
					new SqlParameter("@real_name", SqlDbType.VarChar,250),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@create_date", SqlDbType.DateTime)};
			parameters[0].Value = model.mail_id;
			parameters[1].Value = model.page_id;
			parameters[2].Value = model.file_id;
			parameters[3].Value = model.file_name;
			parameters[4].Value = model.real_name;
			parameters[5].Value = model.create_id;
			parameters[6].Value = model.create_name;
			parameters[7].Value = model.create_date;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from mail_attachment ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public XHD.Model.mail_attachment GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 mail_id,page_id,file_id,file_name,real_name,create_id,create_name,create_date from mail_attachment ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

			XHD.Model.mail_attachment model=new XHD.Model.mail_attachment();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["mail_id"]!=null && ds.Tables[0].Rows[0]["mail_id"].ToString()!="")
				{
					model.mail_id=int.Parse(ds.Tables[0].Rows[0]["mail_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["page_id"]!=null && ds.Tables[0].Rows[0]["page_id"].ToString()!="")
				{
					model.page_id=ds.Tables[0].Rows[0]["page_id"].ToString();
				}
				if(ds.Tables[0].Rows[0]["file_id"]!=null && ds.Tables[0].Rows[0]["file_id"].ToString()!="")
				{
					model.file_id=ds.Tables[0].Rows[0]["file_id"].ToString();
				}
				if(ds.Tables[0].Rows[0]["file_name"]!=null && ds.Tables[0].Rows[0]["file_name"].ToString()!="")
				{
					model.file_name=ds.Tables[0].Rows[0]["file_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["real_name"]!=null && ds.Tables[0].Rows[0]["real_name"].ToString()!="")
				{
					model.real_name=ds.Tables[0].Rows[0]["real_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["create_id"]!=null && ds.Tables[0].Rows[0]["create_id"].ToString()!="")
				{
					model.create_id=int.Parse(ds.Tables[0].Rows[0]["create_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["create_name"]!=null && ds.Tables[0].Rows[0]["create_name"].ToString()!="")
				{
					model.create_name=ds.Tables[0].Rows[0]["create_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["create_date"]!=null && ds.Tables[0].Rows[0]["create_date"].ToString()!="")
				{
					model.create_date=DateTime.Parse(ds.Tables[0].Rows[0]["create_date"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select mail_id,page_id,file_id,file_name,real_name,create_id,create_name,create_date ");
			strSql.Append(" FROM mail_attachment ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" mail_id,page_id,file_id,file_name,real_name,create_id,create_name,create_date ");
			strSql.Append(" FROM mail_attachment ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			StringBuilder strSql = new StringBuilder();
			StringBuilder strSql1 = new StringBuilder();
			strSql.Append("select ");
			strSql.Append(" top " + PageSize + " * FROM mail_attachment ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM mail_attachment ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM mail_attachment ");
			if (strWhere.Trim() != "")
			{
			    strSql.Append(" and " + strWhere);
			    strSql1.Append(" where " + strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			Total = DbHelperSQL.Query(strSql1.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql.ToString());
		}

		#endregion  Method
	}
}

