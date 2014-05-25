![](http://pomma89.altervista.org/hippie/logo-mini.png "Hippie Logo") Hippie
============================================================================

Fully managed library providing five types of heap.

| Item                                               | URL                                                  |
| -------------------------------------------------- | ---------------------------------------------------- |
| ![](http://is.gd/1wCmXL) NuGet package             | https://nuget.org/packages/Hippie/                   |
| ![](http://is.gd/1wCmXL) NuGet package (unchecked) | https://nuget.org/packages/Hippie.Unchecked/         |
| ![](http://is.gd/4uKNfs) Tutorial                  | TODO                                                 |
| ![](http://is.gd/U2M21W) Documentation (HTML)      | http://pomma89.altervista.org/hippie/html/index.html |
| ![](http://is.gd/I7ThMS) Documentation (PDF)       | http://pomma89.altervista.org/hippie/refman.pdf      |

Currently implemented heaps are:

| Heap type | Wikipedia page                               |
| --------- | -------------------------------------------- |
| Array     | https://en.wikipedia.org/wiki/D-ary_heap     |
| Binary    | https://en.wikipedia.org/wiki/Binary_heap    |
| Binomial  | https://en.wikipedia.org/wiki/Binomial_heap  |
| Fibonacci | https://en.wikipedia.org/wiki/Fibonacci_heap |
| Pairing   | https://en.wikipedia.org/wiki/Pairing_heap   |

For example, using this library you can write a very simple heap sort in this way:

```cs
T[] HeapSort<T>(IEnumerable<T> elems) where T : IComparable<T>
{
    var heap = HeapFactory.NewBinaryHeap<T>();
    foreach (var elem in elems) {
        heap.Add(elem);
    }
    var orderedElems = new T[heap.Count];
    for (var i = 0; heap.Count > 0; ++i) {
        orderedElems[i] = heap.RemoveMin();
    }
    return orderedElems;
}
```
