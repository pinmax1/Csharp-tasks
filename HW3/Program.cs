using System;
using System.Collections.Generic;

public interface IEntity
{
    int Id { get; }
}

public class Repository<T> where T : IEntity
{
    private readonly Dictionary<int, T> _storage = new();

    public void Add(T item)
    {
        if (_storage.ContainsKey(item.Id))
            throw new InvalidOperationException($"Item with Id={item.Id} already exists");

        _storage[item.Id] = item;
    }

    public bool Remove(int id)
    {
        return _storage.Remove(id);
    }

    public T? GetById(int id)
    {
        if (_storage.TryGetValue(id, out var value))
            return value;

        return default;
    }

    public IReadOnlyList<T> GetAll()
    {
        return new List<T>(_storage.Values);
    }

    public int Count => _storage.Count;

    public IReadOnlyList<T> Find(Predicate<T> predicate)
    {
        var result = new List<T>();

        foreach (var item in _storage.Values)
        {
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }
}

public class Product : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }

    public override string ToString()
    {
        return $"{Id}: {Name} - {Price}";
    }
}

public class User : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";

    public override string ToString()
    {
        return $"{Id}: {Name}";
    }
}
public static class CollectionUtils
{
    public static List<T> Distinct<T>(List<T> source)
    {
        var result = new List<T>();
        var seen = new HashSet<T>();

        foreach (var item in source)
        {
            if (!seen.Contains(item))
            {
                seen.Add(item);
                result.Add(item);
            }
        }

        return result;
    }

    public static Dictionary<TKey, List<TValue>> GroupBy<TValue, TKey>(
        List<TValue> source,
        Func<TValue, TKey> keySelector) where TKey : notnull
    {
        var result = new Dictionary<TKey, List<TValue>>();

        foreach (var item in source)
        {
            var key = keySelector(item);

            if (!result.ContainsKey(key))
                result[key] = new List<TValue>();

            result[key].Add(item);
        }

        return result;
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second,
        Func<TValue, TValue, TValue> conflictResolver) where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>();

        foreach (var pair in first)
            result[pair.Key] = pair.Value;

        foreach (var pair in second)
        {
            if (result.ContainsKey(pair.Key))
            {
                result[pair.Key] = conflictResolver(result[pair.Key], pair.Value);
            }
            else
            {
                result[pair.Key] = pair.Value;
            }
        }

        return result;
    }

    public static T MaxBy<T, TKey>(List<T> source, Func<T, TKey> selector)
        where TKey : IComparable<TKey>
    {
        if (source.Count == 0)
            throw new InvalidOperationException("Collection is empty");

        var maxItem = source[0];
        var maxKey = selector(maxItem);

        for (int i = 1; i < source.Count; i++)
        {
            var currentKey = selector(source[i]);

            if (currentKey.CompareTo(maxKey) > 0)
            {
                maxKey = currentKey;
                maxItem = source[i];
            }
        }

        return maxItem;
    }
}

class Program
{
    static void Main()
    {
        var productRepo = new Repository<Product>();
        var userRepo = new Repository<User>();

        productRepo.Add(new Product { Id = 1, Name = "Laptop", Price = 1500 });
        productRepo.Add(new Product { Id = 2, Name = "Mouse", Price = 50 });

        userRepo.Add(new User { Id = 1, Name = "Alice" });
        userRepo.Add(new User { Id = 2, Name = "Bob" });

        Console.WriteLine(productRepo.GetById(1));

        var expensive = productRepo.Find(p => p.Price > 1000);
        Console.WriteLine("Expensive products:");
        foreach (var p in expensive)
            Console.WriteLine(p);

        try
        {
            productRepo.Add(new Product { Id = 1, Name = "Duplicate", Price = 0 });
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        // -------- CollectionUtils --------

        var numbers = new List<int> { 1, 2, 2, 3, 1 };
        var distinctNumbers = CollectionUtils.Distinct(numbers);
        Console.WriteLine("Distinct ints: " + string.Join(", ", distinctNumbers));

        var words = new List<string> { "a", "bb", "a", "ccc" };
        var distinctWords = CollectionUtils.Distinct(words);
        Console.WriteLine("Distinct strings: " + string.Join(", ", distinctWords));

        var grouped = CollectionUtils.GroupBy(words, w => w.Length);
        Console.WriteLine("GroupBy length:");
        foreach (var pair in grouped)
        {
            Console.WriteLine($"{pair.Key}: {string.Join(", ", pair.Value)}");
        }


        var dict1 = new Dictionary<string, int>
        {
            ["a"] = 1,
            ["b"] = 2
        };

        var dict2 = new Dictionary<string, int>
        {
            ["b"] = 3,
            ["c"] = 4
        };

        var merged = CollectionUtils.Merge(dict1, dict2, (x, y) => x + y);

        Console.WriteLine("Merged:");
        foreach (var pair in merged)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }


        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Phone", Price = 800 },
            new Product { Id = 2, Name = "TV", Price = 2000 },
            new Product { Id = 3, Name = "Tablet", Price = 600 }
        };

        var maxProduct = CollectionUtils.MaxBy(products, p => p.Price);
        Console.WriteLine($"Most expensive: {maxProduct}");
    }
}