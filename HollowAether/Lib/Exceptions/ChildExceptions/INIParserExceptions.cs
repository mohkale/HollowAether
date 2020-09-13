using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib.Exceptions.CE {
	class InvalidLineInINIFileException : HollowAetherException {
		public InvalidLineInINIFileException(String line, String file) : base($"'{line}' is invalid, in {file}") { }

		public InvalidLineInINIFileException(String line, String file, Exception inner) : base($"'{line}' is invalid, in {file}", inner) { }
	}
}
