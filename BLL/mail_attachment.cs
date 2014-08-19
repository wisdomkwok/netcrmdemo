using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// mail_attachment
	/// </summary>
	public partial class mail_attachment
	{
		private readonly XHD.DAL.mail_attachment dal=new XHD.DAL.mail_attachment();
		public mail_attachment()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(XHD.Model.mail_attachment model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.mail_attachment model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete();
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public XHD.Model.mail_attachment GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel();
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public XHD.Model.mail_attachment GetModelByCache()
		{
			//该表无主键信息，请自定义主键/条件字段
			string CacheKey = "mail_attachmentModel-" ;
			object objModel = XHD.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel();
					if (objModel != null)
					{
						int ModelCache = XHD.Common.ConfigHelper.GetConfigInt("ModelCache");
						XHD.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (XHD.Model.mail_attachment)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<XHD.Model.mail_attachment> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<XHD.Model.mail_attachment> DataTableToList(DataTable dt)
		{
			List<XHD.Model.mail_attachment> modelList = new List<XHD.Model.mail_attachment>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				XHD.Model.mail_attachment model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new XHD.Model.mail_attachment();
					if(dt.Rows[n]["mail_id"]!=null && dt.Rows[n]["mail_id"].ToString()!="")
					{
						model.mail_id=int.Parse(dt.Rows[n]["mail_id"].ToString());
					}
					if(dt.Rows[n]["page_id"]!=null && dt.Rows[n]["page_id"].ToString()!="")
					{
					model.page_id=dt.Rows[n]["page_id"].ToString();
					}
					if(dt.Rows[n]["file_id"]!=null && dt.Rows[n]["file_id"].ToString()!="")
					{
					model.file_id=dt.Rows[n]["file_id"].ToString();
					}
					if(dt.Rows[n]["file_name"]!=null && dt.Rows[n]["file_name"].ToString()!="")
					{
					model.file_name=dt.Rows[n]["file_name"].ToString();
					}
					if(dt.Rows[n]["real_name"]!=null && dt.Rows[n]["real_name"].ToString()!="")
					{
					model.real_name=dt.Rows[n]["real_name"].ToString();
					}
					if(dt.Rows[n]["create_id"]!=null && dt.Rows[n]["create_id"].ToString()!="")
					{
						model.create_id=int.Parse(dt.Rows[n]["create_id"].ToString());
					}
					if(dt.Rows[n]["create_name"]!=null && dt.Rows[n]["create_name"].ToString()!="")
					{
					model.create_name=dt.Rows[n]["create_name"].ToString();
					}
					if(dt.Rows[n]["create_date"]!=null && dt.Rows[n]["create_date"].ToString()!="")
					{
						model.create_date=DateTime.Parse(dt.Rows[n]["create_date"].ToString());
					}
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
		}

		#endregion  Method
	}
}

