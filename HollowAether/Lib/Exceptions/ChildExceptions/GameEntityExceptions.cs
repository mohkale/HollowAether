#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.MapZone;
#endregion

using System.Runtime.Serialization;

namespace HollowAether.Lib.Exceptions.CE {
	public class EntityException : HollowAetherException {
		public EntityException() : base() { }

		public EntityException(String msg) : base(msg) { }

		public EntityException(String msg, Exception inner) : base(msg, inner) { }

		public EntityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	public class EntityAttributeInvalidTypeAssignmentException : EntityException {
		public EntityAttributeInvalidTypeAssignmentException(Type arg, Type desired)
		: base($"Cannot Assign Value Of Type '{arg}' To Entity Attribute Of Type {desired}") { }
		public EntityAttributeInvalidTypeAssignmentException(Type arg, Type desired, Exception inner)
		: base($"Cannot Assign Value Of Type '{arg}' To Entity Attribute Of Type {desired}", inner) { }
	}

	public class EntityAttributeReadOnlyAssignmentAttemptException : EntityException {
		public EntityAttributeReadOnlyAssignmentAttemptException() 
			: base($"Cannot Assign Value For Read Only Entity Attribute") { }
		public EntityAttributeReadOnlyAssignmentAttemptException(Exception inner) 
			: base($"Cannot Assign Value For Read Only Entity Attribute", inner) { }
	}

	public class EntityAttributeReadOnlyDeletionException : EntityException {
		public EntityAttributeReadOnlyDeletionException()
			: base($"Cannot Delete A Read Only Entity Attributes Value") { }
		public EntityAttributeReadOnlyDeletionException(Exception inner)
			: base($"Cannot Delete A Read Only Entity Attributes Value", inner) { }
	}

	public class EntityAttributeUnassignedValueRetrievalException : EntityException {
		public EntityAttributeUnassignedValueRetrievalException()
			: base($"Cannot Get Unassigned Entity Attribute") { }
		public EntityAttributeUnassignedValueRetrievalException(Exception inner)
			: base($"Cannot Get Unassigned Entity Attribute", inner) { }
	}
}
