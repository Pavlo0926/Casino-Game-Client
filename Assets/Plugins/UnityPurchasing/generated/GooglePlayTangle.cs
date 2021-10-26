#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("dK8sUrL5buH9tvueq8hWRSKIQMYFL57Ch4S9iFPEL82ZtMk4o1gUtmR+Gax1HixPiOf+i1gVavH2UxxX7P4Z3nJn6+J/4wye3BxUbKK2jrS8SmRumov0+0QTOIjgbV2lK04RsAIj/Ylg3bwo49zNcU2jbNG+oQiDzH2ij7rj3LFZy1lN/B4IVIWSJ+AWNZh+/lA/mRyXy/ZHc//mJAfAWackKiUVpyQvJ6ckJCWbVpk1mV3EM7LscDJgqiKbiEx3HjPaPEAGa7JHQuKyj+H1vqfO4Qpaek3JYvYp369g7++cXY4Q6G9fJWQYx4UndWaaZ58EUhuRgndHCRJWRtV4/L3w/k4VpyQHFSgjLA+jbaPSKCQkJCAlJh9cPhrjLJGJJicmJCUk");
        private static int[] order = new int[] { 7,6,12,9,12,9,11,12,11,10,10,13,13,13,14 };
        private static int key = 37;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
