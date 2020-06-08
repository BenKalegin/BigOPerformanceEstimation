# BigO Performance Estimation for .Net projects

Working with big data structures can be challenging if you want to predict performance impact. To describe the complexity of the algorithm, most developers  use Big O notation. It simply specifies the dependency of the upper boundary of the run time in respect to number of elements in array.

Some examples in complexity ascending order:

| Typical complexity  | Worst case (when different) | Scenario |
| ------------------- | --------------------------- | ---------|
| O(1)                | O(1)                        | Read first element value in an array |
| O(1)                | O(_n_)                      | HashMap. Usually it takes O(1), but, in worst case that every lookup hit a collision, O(Log(N)) for Java implementation and O(N) for .Net HashSet/Dictionary<> |
| O(log _n_)          |                             | Newton's method of binary search.  If you have sorted array, you can search for the target value comparing to middle element in the array and recurse into the left or right half.  Array.BinarySearch/SortedDictionary |
| O(_n_)              |                             | linear search in the array. Iterate all elements of the array, comparing the element to the target.  |
| O(_n_ log _n_)      |                             | Merge sort |
| O(_n_ log _n_)      | O(_n<sup>2</sup>_)          | Quick sort |
| O(_n<sup>2</sup>_)  |                             | Bubble sort |
| O(2<sup>_n_</sup>)  |                             | Travelling salesman problem or similar recursive backtracking algorithms. |
| O(_n_!)             |                             | List all permutations of an array |

Log(arithm) mentioned above has 2 basis for most of algorithm we use and can be estimated like

log(1,000) ≈ 10
log(1,000,000) ≈ 20
log(1,000,000,000) ≈ 30

Usually you don't notice the performance penalty of slow algorithms because of small data volume used in local tests.  For simple operations there is a magic number billion per second when CPU starts to saturate. I arranged classes into the table:

| Complexity         | Is it good        | N to spend 100 ms in high CPU(when slowness starts to feel) |
| ------------------ | ----------------- | ----------------------------------------------------------- |
| O(1)               | Good news         |-|
| O(log _n_)         |                   |-|                    
| O(_n_)             |                   | 100M |
| O(_n_ log _n_)     | Tolerated         | 5M                                                          |
| O(_n<sup>2</sup>_) | Bad news          | 10K                                                         |
| O(2<sup>_n_</sup>) |                   | 27
| O(_n_!)            |                   | 11


Sometimes it is hard predict Big O theoretically because and you can make practical measurements, benchmark with different N and find BigO by approximation. 


This project is a sample solution that can help to verify the big O by benchmarking. Intention is to detect O(1, LogN, N, N*LogN, N2).  

It uses [benchmark.net](http://benchmark.net/) library that takes deal of warming up, ignore GC spikes, other outliers and basically makes the performance numbers smooth.


 ~~~~
 [Params(1000, 2000, 3000, 5000, 10000, 20000)]

public int N;
 
 
[GlobalSetup]
public void Setup()
{
 var chars = new char[N];
     ...
     subject = new ReassembleMarkdown();
}
 
[Benchmark]
public string OldMethod() => subject.OldEscapeMarkDownCharacters(data);
 
[Benchmark]
public string NewMethod() => subject.NewEscapeMarkDownCharacters(data);
 ~~~~





