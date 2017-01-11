public class Tuple <T1, T2>
{
	public T1 first { get; private set; }
	public T2 second { get; private set; }
	internal Tuple(T1 f, T2 s)
	{
		first = f;
		second = s;
	}
	
	public static Tuple<T, U> Create<T, U>(T item1, U item2)
	{
		return new Tuple<T, U>(item1, item2);
	}
}