using System;
using System.Data;
using System.Collections.Generic;
using XHD.Common;
using XHD.Model;
namespace XHD.BLL
{
	/// <summary>
	/// mail_info
	/// </summary>
	public partial class mail_info
	{
		private readonly XHD.DAL.mail_info dal=new XHD.DAL.mail_info();
		public mail_info()
		{}
		#region  Method

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(XHD.Model.mail_info model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(XHD.Model.mail_info model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// Ԥɾ��
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
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int id)
		{
			
			return dal.Delete(id);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public XHD.Model.mail_info GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public XHD.Model.mail_info GetModelByCache(int id)
		{
			
			string CacheKey = "mail_infoModel-" + id;
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
			return (XHD.Model.mail_info)objModel;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<XHD.Model.mail_info> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<XHD.Model.mail_info> DataTableToList(DataTable dt)
		{
			List<XHD.Model.mail_info> modelList = new List<XHD.Model.mail_info>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				XHD.Model.mail_info model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new XHD.Model.mail_info();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["mail_title"]!=null && dt.Rows[n]["mail_title"].ToString()!="")
					{
					model.mail_title=dt.Rows[n]["mail_title"].ToString();
					}
					if(dt.Rows[n]["mail_content"]!=null && dt.Rows[n]["mail_content"].ToString()!="")
					{
					model.mail_content=dt.Rows[n]["mail_content"].ToString();
					}
					if(dt.Rows[n]["atta_count"]!=null && dt.Rows[n]["atta_count"].ToString()!="")
					{
						model.atta_count=int.Parse(dt.Rows[n]["atta_count"].ToString());
					}
					if(dt.Rows[n]["create_id"]!=null && dt.Rows[n]["create_id"].ToString()!="")
					{
						model.create_id=int.Parse(dt.Rows[n]["create_id"].ToString());
					}
					if(dt.Rows[n]["create_name"]!=null && dt.Rows[n]["create_name"].ToString()!="")
					{
					model.create_name=dt.Rows[n]["create_name"].ToString();
					}
					if(dt.Rows[n]["create_time"]!=null && dt.Rows[n]["create_time"].ToString()!="")
					{
						model.create_time=DateTime.Parse(dt.Rows[n]["create_time"].ToString());
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
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
		}

		#endregion  Method
	}
}

