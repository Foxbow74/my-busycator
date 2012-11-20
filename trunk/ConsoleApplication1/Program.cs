using System;
using System.Collections.Generic;
using XTransport;
using XTransport.Client;
using XTransport.Server;
using XTransport.Server.Storage;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] _args)
		{
			var srv = new Server();
			var cli = new Client();

			var r = cli.GetRoot<Root>();

			var item = new A();
			r.As.Add(item);
			item.V2 = 51;

			cli.Save(r);
		}
	}

	class Client:AbstractXClient<EKind>
	{
		public override Guid UserUid
		{
			get { return Guid.Empty; }
		}

		protected override IEnumerable<KeyValuePair<EKind, EKind>> GetAbstractRootKindMap()
		{
			yield break;
		}

		protected override int KindToInt(EKind _kind)
		{
			return (int)_kind;
		}

		protected override EKind IntToKind(int _kind)
		{
			return (EKind)_kind;
		}

		protected override void ObjectReleased(Guid _uid, EKind _kind)
		{
			
		}
	}

	class Server : AbstractXServer
	{
		protected override IStorage CreateStorage()
		{
			return new SQLiteStorage("db");
		}
	}

	abstract class XObject:ClientXObject<EKind>
	{
	}

	class A : XObject
	{
		[X("Int")]
		private IXValue<int> m_int;

		[X("Int2")]
		private IXValue<int> m_int2;

		public override EKind Kind
		{
			get { return EKind.A; }
		}

		public int V
		{
			get { return m_int.Value; }
			set { m_int.Value = value; }
		}


		public int V2
		{
			get { return m_int2.Value; }
			set { m_int2.Value = value; }
		}
	}

	class B : XObject
	{
		[X("Str")]
		private IXValue<string> m_str;

		public override EKind Kind
		{
			get { return EKind.B; }
		}

		public string V
		{
			get { return m_str.Value; }
			set { m_str.Value = value; }
		}
	}

	class C : XObject
	{
		[X("Dbl")]
		private IXValue<double> m_double;

		public override EKind Kind
		{
			get { return EKind.C; }
		}

		public double V
		{
			get { return m_double.Value; }
			set { m_double.Value = value; }
		}
	}

	class Root:XObject
	{
		public override EKind Kind
		{
			get { return EKind.ROOT;}
		}

		public ICollection<A> As
		{
			get { return m_as; }
		}

		public ICollection<B> Bs
		{
			get { return m_bs; }
		}

		public ICollection<C> Cs
		{
			get { return m_cs; }
		}

		[X((int)EKind.A)]
		private ICollection<A> m_as;

		[X((int)EKind.B)]
		private ICollection<B> m_bs;

		[X((int)EKind.C)]
		private ICollection<C> m_cs;
	}

	enum EKind
	{
		A,
		B,
		C,
		ROOT,
	}
}
