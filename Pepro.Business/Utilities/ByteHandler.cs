namespace Pepro.Business.Utilities;

static class ByteHandler
{
    /// <summary>
    /// Combines multiple byte arrays into a single continuous byte array.
    /// </summary>
    /// <param name="arrays">
    /// An array of byte arrays to be combined.
    /// </param>
    /// <returns>
    /// A single byte array that contains the concatenated data
    /// of all input arrays in the provided order.
    /// </returns>
    public static byte[] Combine(params byte[][] arrays)
    {
        byte[] result = new byte[arrays.Sum(array => array.Length)];
        int currentOffset = 0;

        foreach (byte[] array in arrays)
        {
            // Efficiently copies each array's content into the result buffer.
            Buffer.BlockCopy(array, 0, result, currentOffset, array.Length);
            currentOffset += array.Length;
        }

        return result;
    }
}
