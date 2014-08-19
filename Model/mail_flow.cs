using System;
namespace XHD.Model
{
	/// <summary>
	/// mail_flow:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class mail_flow
	{
		public mail_flow()
		{}
		#region Model
		private int _id;
		private int? _mail_id;
		private string _mail_title;
		private int? _atta_count;
		private int? _receive_id;
		private string _receive_name;
		private int? _isview;
		private DateTime? _view_time;
		private int? _recive_type_id;
		private string _recive_type;
		private int? _sender_id;
		private string _sender_name;
		private DateTime? _sender_time;
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
		public int? mail_id
		{
			set{ _mail_id=value;}
			get{return _mail_id;}
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
		public int? atta_count
		{
			set{ _atta_count=value;}
			get{return _atta_count;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? receive_id
		{
			set{ _receive_id=value;}
			get{return _receive_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string receive_name
		{
			set{ _receive_name=value;}
			get{return _receive_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? isView
		{
			set{ _isview=value;}
			get{return _isview;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? view_time
		{
			set{ _view_time=value;}
			get{return _view_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? recive_type_id
		{
			set{ _recive_type_id=value;}
			get{return _recive_type_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string recive_type
		{
			set{ _recive_type=value;}
			get{return _recive_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? sender_id
		{
			set{ _sender_id=value;}
			get{return _sender_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sender_name
		{
			set{ _sender_name=value;}
			get{return _sender_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? sender_time
		{
			set{ _sender_time=value;}
			get{return _sender_time;}
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

