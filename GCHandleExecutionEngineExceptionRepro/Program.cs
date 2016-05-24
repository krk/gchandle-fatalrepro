using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GCHandleExecutionEngineExceptionRepro
{
	class Program
	{
		static void Main(string[] args)
		{
			ConcurrentWeakDictionaryTests_FromCorefx.TestAdd1();
		}
	}
}
