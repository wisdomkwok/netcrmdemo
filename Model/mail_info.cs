using System;
namespace XHD.Model
{
	/// <summary>
	/// mail_info:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class mail_info
	{
		public mail_info()
		{}
		#region Model
		private int _id;
		private string _mail_title;
		private string _mail_content;
		private int? _atta_count;
		private int? _create_id;
		private string _create_name;
		private DateTime? _create_time;
		private int? _isdelete;
		private DateTime? _delete_time;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string mail_title
		{
			set{ _mail_title=value;}
			get{return _mail_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string mail_content
		{
			set{ _mail_content=value;}
			get{return _mail_content;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? atta_count
		{
			set{ _atta_count=value;}
			get{return _atta_count;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? create_id
		{
			set{ _create_id=value;}
			get{return _create_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string create_name
		{
			set{ _create_name=value;}
			get{return _create_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? create_time
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isDelete
		{
			set{ _isdelete=value;}
			get{return _isdelete;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Delete_time
		{
			set{ _delete_time=value;}
			get{return _delete_time;}
		}
		#endregion Model

	}
}

