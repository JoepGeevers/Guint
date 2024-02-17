namespace Guint
{
	internal class Secret
	{
		public readonly byte[] Key;
		public readonly byte[] Vector;

		public Secret(byte[] key, byte[] vector)
		{
			this.Key = key;
			this.Vector = vector;
		}
	}
}