using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// mail_flow
	/// </summary>
	public partial class mail_flow
	{
		private readonly XHD.DAL.mail_flow dal=new XHD.DAL.mail_flow();
		public mail_flow()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(XHD.Model.mail_flow model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(XHD.Model.mail_flow model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 预删除
		/// </summary>
		/// <param name="id"></param>
		/// <param name="isDelete"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public bool AdvanceDelete(int id, int isDelete, string time)
		{
			return dal.AdvanceDelete(id, isDelete, time);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			return dal.Delete(id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public XHD.Model.mail_flow GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public XHD.Model.mail_flow GetModelByCache(int id)
		{
			
			string CacheKey = "mail_flowModel-" + id;
			object objModel = XHD.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(id);
					if (objModel != null)
					{
						int ModelCache = XHD.Common.ConfigHelper.GetConfigInt("ModelCache");
						XHD.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (XHD.Model.mail_flow)objModel;
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
		public List<XHD.Model.mail_flow> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<XHD.Model.mail_flow> DataTableToList(DataTable dt)
		{
			List<XHD.Model.mail_flow> modelList = new List<XHD.Model.mail_flow>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				XHD.Model.mail_flow model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new XHD.Model.mail_flow();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["mail_id"]!=null && dt.Rows[n]["mail_id"].ToString()!="")
					{
						model.mail_id=int.Parse(dt.Rows[n]["mail_id"].ToString());
					}
					if(dt.Rows[n]["mail_title"]!=null && dt.Rows[n]["mail_title"].ToString()!="")
					{
					model.mail_title=dt.Rows[n]["mail_title"].ToString();
					}
					if(dt.Rows[n]["atta_count"]!=null && dt.Rows[n]["atta_count"].ToString()!="")
					{
						model.atta_count=int.Parse(dt.Rows[n]["atta_count"].ToString());
					}
					if(dt.Rows[n]["receive_id"]!=null && dt.Rows[n]["receive_id"].ToString()!="")
					{
						model.receive_id=int.Parse(dt.Rows[n]["receive_id"].ToString());
					}
					if(dt.Rows[n]["receive_name"]!=null && dt.Rows[n]["receive_name"].ToString()!="")
					{
					model.receive_name=dt.Rows[n]["receive_name"].ToString();
					}
					if(dt.Rows[n]["isView"]!=null && dt.Rows[n]["isView"].ToString()!="")
					{
						model.isView=int.Parse(dt.Rows[n]["isView"].ToString());
					}
					if(dt.Rows[n]["view_time"]!=null && dt.Rows[n]["view_time"].ToString()!="")
					{
						model.view_time=DateTime.Parse(dt.Rows[n]["view_time"].ToString());
					}
					if(dt.Rows[n]["recive_type_id"]!=null && dt.Rows[n]["recive_type_id"].ToString()!="")
					{
						model.recive_type_id=int.Parse(dt.Rows[n]["recive_type_id"].ToString());
					}
					if(dt.Rows[n]["recive_type"]!=null && dt.Rows[n]["recive_type"].ToString()!="")
					{
					model.recive_type=dt.Rows[n]["recive_type"].ToString();
					}
					if(dt.Rows[n]["sender_id"]!=null && dt.Rows[n]["sender_id"].ToString()!="")
					{
						model.sender_id=int.Parse(dt.Rows[n]["sender_id"].ToString());
					}
					if(dt.Rows[n]["sender_name"]!=null && dt.Rows[n]["sender_name"].ToString()!="")
					{
					model.sender_name=dt.Rows[n]["sender_name"].ToString();
					}
					if(dt.Rows[n]["sender_time"]!=null && dt.Rows[n]["sender_time"].ToString()!="")
					{
						model.sender_time=DateTime.Parse(dt.Rows[n]["sender_time"].ToString());
					}
					if(dt.Rows[n]["isDelete"]!=null && dt.Rows[n]["isDelete"].ToString()!="")
					{
						model.isDelete=int.Parse(dt.Rows[n]["isDelete"].ToString());
					}
					if(dt.Rows[n]["Delete_time"]!=null && dt.Rows[n]["Delete_time"].ToString()!="")
					{
						model.Delete_time=DateTime.Parse(dt.Rows[n]["Delete_time"].ToString());
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

