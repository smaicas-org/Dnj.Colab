using System.Collections;

namespace Dnj.Colab.Samples.StreamReaderEnumerable;
public static class SREnumerable
{
    public static void TestStreamReaderEnumerable(string filepath)
    {
        IEnumerable<string> stringsFound;
        // Open a file with the StreamReaderEnumerable and check for a string.
        try
        {
            stringsFound =
                  from line in new StreamReaderEnumerable(filepath)
                  where line.Contains("\"")
                  select line;
            //Console.WriteLine("Found: " + stringsFound.Count());
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(@$"This example requires a file named {filepath}");
            return;
        }
    }

    public static void TestReadingFile(string filepath)
    {
        StreamReader sr;
        try
        {
            sr = File.OpenText(filepath);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine(@$"This example requires a file named {filepath}.");
            return;
        }

        // Add the file contents to a generic list of strings.
        List<string> fileContents = new();
        while (!sr.EndOfStream)
        {
            fileContents.Add(sr.ReadLine());
        }

        //Check for the string.
        IEnumerable<string> stringsFound =
            from line in fileContents
            where line.Contains("\"")
            select line;

        sr.Close();
        //Console.WriteLine("Found: " + stringsFound.Count());
    }
}

// A custom class that implements IEnumerable(T). When you implement IEnumerable(T),
// you must also implement IEnumerable and IEnumerator(T).
public class StreamReaderEnumerable : IEnumerable<string>
{
    private readonly string _filePath;
    public StreamReaderEnumerable(string filePath) => _filePath = filePath;

    // Must implement GetEnumerator, which returns a new StreamReaderEnumerator.
    public IEnumerator<string> GetEnumerator() => new StreamReaderEnumerator(_filePath);

    // Must also implement IEnumerable.GetEnumerator, but implement as a private method.
    private IEnumerator GetEnumerator1() => this.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator1();
}

// When you implement IEnumerable(T), you must also implement IEnumerator(T),
// which will walk through the contents of the file one line at a time.
// Implementing IEnumerator(T) requires that you implement IEnumerator and IDisposable.
public class StreamReaderEnumerator : IEnumerator<string>
{
    private readonly StreamReader _sr;
    public StreamReaderEnumerator(string filePath) => _sr = new StreamReader(filePath);

    private string _current;
    // Implement the IEnumerator(T).Current publicly, but implement
    // IEnumerator.Current, which is also required, privately.
    public string Current
    {

        get
        {
            if (_sr == null || _current == null)
            {
                throw new InvalidOperationException();
            }

            return _current;
        }
    }

    private object Current1 => this.Current;

    object IEnumerator.Current => Current1;

    // Implement MoveNext and Reset, which are required by IEnumerator.
    public bool MoveNext()
    {
        _current = _sr.ReadLine();
        if (_current == null)
            return false;
        return true;
    }

    public void Reset()
    {
        _sr.DiscardBufferedData();
        _sr.BaseStream.Seek(0, SeekOrigin.Begin);
        _current = null;
    }

    // Implement IDisposable, which is also implemented by IEnumerator(T).
    private bool disposedValue = false;
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposedValue)
        {
            if (disposing)
            {
                // Dispose of managed resources.
            }
            _current = null;
            if (_sr != null)
            {
                _sr.Close();
                _sr.Dispose();
            }
        }

        this.disposedValue = true;
    }

    ~StreamReaderEnumerator()
    {
        Dispose(disposing: false);
    }
}

