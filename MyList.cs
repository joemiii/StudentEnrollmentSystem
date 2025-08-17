namespace StudentEnrollmentSystem;

public class MyList
{
    // Fixed-size list of integers for storing indices
    private int[] arr;
    private int count;

    // Create list with a fixed capacity (at least 1)
    public MyList(int capacity)
    {
        if (capacity < 1) capacity = 1;
        arr = new int[capacity];
        count = 0;
    }

    // Add a value if there is space (ignore if full)
    public void Add(int value)
    {
        if (count < arr.Length)
        {
            arr[count++] = value;
        }
    }

    // Get value at index or -1 if out of range
    public int Get(int index)
    {
        if (index >= 0 && index < count)
            return arr[index];
        return -1;
    }

    // Remove item at index (shifts the rest left)
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= count) return;
        for (int i = index; i < count - 1; i++)
            arr[i] = arr[i + 1];
        count--;
    }

    // Number of stored items
    public int Count()
    {
        return count;
    }

    // Check if the list contains a value
    public bool Contains(int value)
    {
        for (int i = 0; i < count; i++)
            if (arr[i] == value) return true;
        return false;
    }
}
