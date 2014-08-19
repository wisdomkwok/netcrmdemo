using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
	/// <summary>
	/// 数据访问类:mail_flow
	/// </summary>
	public partial class mail_flow
	{
		public mail_flow()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "mail_flow"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from mail_flow");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(XHD.Model.mail_flow model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into mail_flow(");
			strSql.Append("mail_id,mail_title,atta_count,receive_id,receive_name,isView,view_time,recive_type_id,recive_type,sender_id,sender_name,sender_time,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@mail_id,@mail_title,@atta_count,@receive_id,@receive_name,@isView,@view_time,@recive_type_id,@recive_type,@sender_id,@sender_name,@sender_time,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@mail_id", SqlDbType.Int,4),
					new SqlParameter("@mail_title", SqlDbType.VarChar,250),
					new SqlParameter("@atta_count", SqlDbType.Int,4),
					new SqlParameter("@receive_id", SqlDbType.Int,4),
					new SqlParameter("@receive_name", SqlDbType.VarChar,250),
					new SqlParameter("@isView", SqlDbType.Int,4),
					new SqlParameter("@view_time", SqlDbType.DateTime),
					new SqlParameter("@recive_type_id", SqlDbType.Int,4),
					new SqlParameter("@recive_type", SqlDbType.VarChar,250),
					new SqlParameter("@sender_id", SqlDbType.Int,4),
					new SqlParameter("@sender_name", SqlDbType.VarChar,250),
					new SqlParameter("@sender_time", SqlDbType.DateTime),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.mail_id;
			parameters[1].Value = model.mail_title;
			parameters[2].Value = model.atta_count;
			parameters[3].Value = model.receive_id;
			parameters[4].Value = model.receive_name;
			parameters[5].Value = model.isView;
			parameters[6].Value = model.view_time;
			parameters[7].Value = model.recive_type_id;
			parameters[8].Value = model.recive_type;
			parameters[9].Value = model.sender_id;
			parameters[10].Value = model.sender_name;
			parameters[11].Value = model.sender_time;
			parameters[12].Value = model.isDelete;
			parameters[13].Value = model.Delete_time;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.mail_flow model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update mail_flow set ");
			strSql.Append("mail_id=@mail_id,");
			strSql.Append("mail_title=@mail_title,");
			strSql.Append("atta_count=@atta_count,");
			strSql.Append("receive_id=@receive_id,");
			strSql.Append("receive_name=@receive_name,");
			strSql.Append("isView=@isView,");
			strSql.Append("view_time=@view_time,");
			strSql.Append("recive_type_id=@recive_type_id,");
			strSql.Append("recive_type=@recive_type,");
			strSql.Append("sender_id=@sender_id,");
			strSql.Append("sender_name=@sender_name,");
			strSql.Append("sender_time=@sender_time,");
			strSql.Append("isDelete=@isDelete,");
			strSql.Append("Delete_time=@Delete_time");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@mail_id", SqlDbType.Int,4),
					new SqlParameter("@mail_title", SqlDbType.VarChar,250),
					new SqlParameter("@atta_count", SqlDbType.Int,4),
					new SqlParameter("@receive_id", SqlDbType.Int,4),
					new SqlParameter("@receive_name", SqlDbType.VarChar,250),
					new SqlParameter("@isView", SqlDbType.Int,4),
					new SqlParameter("@view_time", SqlDbType.DateTime),
					new SqlParameter("@recive_type_id", SqlDbType.Int,4),
					new SqlParameter("@recive_type", SqlDbType.VarChar,250),
					new SqlParameter("@sender_id", SqlDbType.Int,4),
					new SqlParameter("@sender_name", SqlDbType.VarChar,250),
					new SqlParameter("@sender_time", SqlDbType.DateTime),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.mail_id;
			parameters[1].Value = model.mail_title;
			parameters[2].Value = model.atta_count;
			parameters[3].Value = model.receive_id;
			parameters[4].Value = model.receive_name;
			parameters[5].Value = model.isView;
			parameters[6].Value = model.view_time;
			parameters[7].Value = model.recive_type_id;
			parameters[8].Value = model.recive_type;
			parameters[9].Value = model.sender_id;
			parameters[10].Value = model.sender_name;
			parameters[11].Value = model.sender_time;
			parameters[12].Value = model.isDelete;
			parameters[13].Value = model.Delete_time;
			parameters[14].Value = model.id;

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
		/// 预删除
		/// </summary>
		public bool AdvanceDelete(int id, int isDelete, string time)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update mail_flow set ");
			strSql.Append("isDelete=" + isDelete);
			strSql.Append(",Delete_time='" + time + "'");
			strSql.Append(" where id=" + id);
			int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from mail_flow ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from mail_flow ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public XHD.Model.mail_flow GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,mail_id,mail_title,atta_count,receive_id,receive_name,isView,view_time,recive_type_id,recive_type,sender_id,sender_name,sender_time,isDelete,Delete_time from mail_flow ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			XHD.Model.mail_flow model=new XHD.Model.mail_flow();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["mail_id"]!=null && ds.Tables[0].Rows[0]["mail_id"].ToString()!="")
				{
					model.mail_id=int.Parse(ds.Tables[0].Rows[0]["mail_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["mail_title"]!=null && ds.Tables[0].Rows[0]["mail_title"].ToString()!="")
				{
					model.mail_title=ds.Tables[0].Rows[0]["mail_title"].ToString();
				}
				if(ds.Tables[0].Rows[0]["atta_count"]!=null && ds.Tables[0].Rows[0]["atta_count"].ToString()!="")
				{
					model.atta_count=int.Parse(ds.Tables[0].Rows[0]["atta_count"].ToString());
				}
				if(ds.Tables[0].Rows[0]["receive_id"]!=null && ds.Tables[0].Rows[0]["receive_id"].ToString()!="")
				{
					model.receive_id=int.Parse(ds.Tables[0].Rows[0]["receive_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["receive_name"]!=null && ds.Tables[0].Rows[0]["receive_name"].ToString()!="")
				{
					model.receive_name=ds.Tables[0].Rows[0]["receive_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["isView"]!=null && ds.Tables[0].Rows[0]["isView"].ToString()!="")
				{
					model.isView=int.Parse(ds.Tables[0].Rows[0]["isView"].ToString());
				}
				if(ds.Tables[0].Rows[0]["view_time"]!=null && ds.Tables[0].Rows[0]["view_time"].ToString()!="")
				{
					model.view_time=DateTime.Parse(ds.Tables[0].Rows[0]["view_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["recive_type_id"]!=null && ds.Tables[0].Rows[0]["recive_type_id"].ToString()!="")
				{
					model.recive_type_id=int.Parse(ds.Tables[0].Rows[0]["recive_type_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["recive_type"]!=null && ds.Tables[0].Rows[0]["recive_type"].ToString()!="")
				{
					model.recive_type=ds.Tables[0].Rows[0]["recive_type"].ToString();
				}
				if(ds.Tables[0].Rows[0]["sender_id"]!=null && ds.Tables[0].Rows[0]["sender_id"].ToString()!="")
				{
					model.sender_id=int.Parse(ds.Tables[0].Rows[0]["sender_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["sender_name"]!=null && ds.Tables[0].Rows[0]["sender_name"].ToString()!="")
				{
					model.sender_name=ds.Tables[0].Rows[0]["sender_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["sender_time"]!=null && ds.Tables[0].Rows[0]["sender_time"].ToString()!="")
				{
					model.sender_time=DateTime.Parse(ds.Tables[0].Rows[0]["sender_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["isDelete"]!=null && ds.Tables[0].Rows[0]["isDelete"].ToString()!="")
				{
					model.isDelete=int.Parse(ds.Tables[0].Rows[0]["isDelete"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Delete_time"]!=null && ds.Tables[0].Rows[0]["Delete_time"].ToString()!="")
				{
					model.Delete_time=DateTime.Parse(ds.Tables[0].Rows[0]["Delete_time"].ToString());
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
			strSql.Append("select id,mail_id,mail_title,atta_count,receive_id,receive_name,isView,view_time,recive_type_id,recive_type,sender_id,sender_name,sender_time,isDelete,Delete_time ");
			strSql.Append(" FROM mail_flow ");
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
			strSql.Append(" id,mail_id,mail_title,atta_count,receive_id,receive_name,isView,view_time,recive_type_id,recive_type,sender_id,sender_name,sender_time,isDelete,Delete_time ");
			strSql.Append(" FROM mail_flow ");
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
			strSql.Append(" top " + PageSize + " * FROM mail_flow ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM mail_flow ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM mail_flow ");
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

