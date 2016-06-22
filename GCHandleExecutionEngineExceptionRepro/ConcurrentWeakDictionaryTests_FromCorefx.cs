	using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GCHandleExecutionEngineExceptionRepro
{
	public static class Assert{
		public static void AreEqual(int o, int ö)
		{
			if(o != ö){
				throw new Exception();
			}
		}
		
		public static void True(bool o, string m){
			if(!o)
			{
				throw new Exception(m);
			}
		}
	}
	
    public class ConcurrentWeakDictionaryTests_FromCorefx
    {
        public class A
        {
            public int Value { get; set; }

            public override int GetHashCode()
            {
                return Value;
            }

            public override bool Equals(object obj)
            {
                return Value.Equals(((A)obj).Value);
            }
        }

        
        public static void TestAdd1()
        {
            //var instance1 = new A { Value = 1 };
            //var instance2 = new A { Value = 2 };

            // Func<int, A> generator = _ => _ % 2 == 0 ? new A { Value = 1 } : new A { Value = 2 };
            Func<int, int> generator = _ => _;

            TestAdd1(1, 1, 1, 10000, generator, false);
            TestAdd1(5, 1, 1, 10000, generator, false);
            TestAdd1(1, 1, 2, 5000, generator, false);
            TestAdd1(1, 1, 5, 2000, generator, false);
            TestAdd1(4, 0, 4, 2000, generator, false);
            TestAdd1(16, 31, 4, 2000, generator, false);
            TestAdd1(64, 5, 5, 5000, generator, false);
            TestAdd1(5, 5, 5, 2500, generator, false);
        }

        private static void TestAdd1<T>(int cLevel, int initSize, int threads, int addsPerThread, Func<int, T> valueGenerator, bool holdReferences)
        {
            threads = 1;

            ConcurrentWeakDictionary<T, T> dictConcurrent = new ConcurrentWeakDictionary<T, T>(cLevel, 1);
            IDictionary<T, T> dict = dictConcurrent;

            var references = new List<T>();

            int count = threads;
            using (ManualResetEvent mre = new ManualResetEvent(false))
            {
                for (int i = 0; i < threads; i++)
                {
                    int ii = i;
                    Task.Run(
                        () =>
                        {
                            for (int j = 0; j < addsPerThread; j++)
                            {
                                var int1 = valueGenerator(j + ii * addsPerThread);
                                var int2 = valueGenerator(-(j + ii * addsPerThread));

                                if (holdReferences)
                                {
                                    references.Add(int1);
                                    references.Add(int2);
                                }

                                dict.Add(int1, int1);
                            }
                            if (Interlocked.Decrement(ref count) == 0) mre.Set();
                        });
                }
                mre.WaitOne();
            }

            List<T> gotKeys = new List<T>();
            foreach (var pair in dict)
                gotKeys.Add(pair.Key);

            //gotKeys.Sort();

            List<int> expectKeys = new List<int>();
            int itemCount = threads * addsPerThread;
            for (int i = 0; i < itemCount; i++)
                expectKeys.Add(i);

            if (holdReferences)
            {
                Assert.AreEqual(expectKeys.Count, gotKeys.Count);
            }
            else
            {
                GC.Collect(2);
                GC.WaitForPendingFinalizers();
                GC.Collect(2);

                gotKeys = new List<T>();
                foreach (var pair in dict)
                    gotKeys.Add(pair.Key);

                Assert.AreEqual(0, gotKeys.Count);
            }

            if (holdReferences)
            {
                for (int i = 0; i < expectKeys.Count; i++)
                {
                    Assert.True(expectKeys[i].Equals(gotKeys[i]),
                        String.Format("The set of keys in the dictionary is are not the same as the expected" + "\n" +
                                "TestAdd1(cLevel={0}, initSize={1}, threads={2}, addsPerThread={3})", cLevel, initSize, threads, addsPerThread)
                       );
                }

                // Finally, let's verify that the count is reported correctly.
                int expectedCount = threads * addsPerThread;
                Assert.AreEqual(expectedCount, dict.Count);
                Assert.AreEqual(expectedCount, dictConcurrent.ToArray().Length);
            }
        }

    

        private class OrdinalStringComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                var xlower = x.ToLowerInvariant();
                var ylower = y.ToLowerInvariant();
                return string.CompareOrdinal(xlower, ylower) == 0;
            }

            public int GetHashCode(string obj)
            {
                return 0;
            }
        }

        #region Helper Classes and Methods
        
        private struct Struct16 : IEqualityComparer<Struct16>
        {
            public long L1, L2;
            public Struct16(long l1, long l2)
            {
                L1 = l1;
                L2 = l2;
            }

            public bool Equals(Struct16 x, Struct16 y)
            {
                return x.L1 == y.L1 && x.L2 == y.L2;
            }

            public int GetHashCode(Struct16 obj)
            {
                return (int)L1;
            }
        }

        #endregion
    }
}
