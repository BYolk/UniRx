using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
using UniRx.Triggers;
using UnityEngine.UI;

#region 1、UniRx 从 Linq 中借鉴而来的操作符：

///1、Linq 与 UniRx 操作符：
/// 1、UniRx 有?常多操作符，如 Where、First 等，这些操作符的意思与 LINQ 操作符基本?致，学习 UniRx 操作符之前先简单学习?下 LINQ 操作符可以加深我们对 UniRx 的理解(使用 Linq 需要先引入 System.Linq 命名空间)
/// 
/// 2、LINQ 简介：
///     1、LINQ 是在 C# 3 发布的?个概念，主要是?种查询语法的实现，可以像 SQL 语句?样查询 C# 的数据列表、XML ?件和数据库
///     2、最简单的 LINQ 实现就是 List 的 ForEach，ForEach 是 LINQ 的链式写法，其功能和我们常?的 foreach 是?样的
///         --
///         var testList = new List<int>{1,2,3};
///         testList.ForEach(num => Debug.Log(num));
///         --
///         输出结果：1、2、3
///         --
///     3、此前所有的 UniRx 操作符和 Linq 也是通用的：
///         --
///         var firstNumber = testList.First();
///         Debug.Log(firstNumber);
///         testList.Where(num => num > 1).ForEach(number => Debug.Log(number));
///         --
/// 3、为什么先介绍 LINQ?
/// 
///     --学习 LINQ 可以加深我们对 UniRx 的理解，?且有的操作符?较晦涩，使? LINQ 可以排除 UniRx 异步的?个?扰——只要把事件源当做?个List 就好理解了
///     
/// 4、Rx 的历史：
///     1、ReactiveX 是 Reactive Extensions 的缩写，?般简写为 Rx，最初是 LINQ 的?个扩展，由微软的架构师 Erik Meijer 领导的团队开发，在 2012 年 11 ?开源，Rx 是?个编程模型，?标是提供?致的编程接?，帮助开发者更?便地处理异步数据流
///     2、Rx 库?持 .NET、JavaScript 和 C++，Rx 近?年越来越流?，现在已经?持?乎全部的流?编程语?，Rx 的?部分语?库由 ReactiveX 这个组织负责维护，?较流?的有 RxJava/RxJS/Rx.NET，? Unity 的版本，就是 UnIRx
///     
/// 5、什么是 Rx？
///     1、微软给的定义是：Rx 是?个函数库，让开发者可以利?可观察序列和 LINQ ?格查询操作符来编写异步和基于事件的程序，使? Rx，开发者可以? Observables 表示异步数据流，? LINQ 操作符查询异步数据流， ? Schedulers 参数化异步数据流的并发处理，Rx 可以这样定义：Rx = Observables + LINQ + Schedulers
///     2、ReactiveX.io(官?) 给的定义是：Rx 是?个使?可观察数据流进?异步编程的编程接?，ReactiveX 结合了观察者模式、迭代器模式和函数式编程的精华。
///     --以上就算是对 Rx 的严谨定义了。接下来在介绍每个操作符之前都会先通过 LINQ 进?介绍，这样做是因为 LINQ 相? UniRx 更好理解，UniRx 因为是在时间上?部分是异步的，所以会对同学们造成?扰。本质上 LINQ 和 UniRx 是?常相似的，这个具体会在后边介绍。
/// 
/// 6、Rx 的应?：
///     1、像?较知名的 Microsoft、Netflix、Github、Trello、SoundCloud 都在? Rx 的各个语?实现版本
///     2、Unity 的 uFrame 、QFramework 等框架都集成了 UniRx。还有?些如 BindingRx、EcsRx 等也是?常优秀的框架，只不过不太出名。但是也?常值得去研究
///     


///2、之前所学的 UniRx 操作符总结：
/// 1、从本章开始，我们遇到的所有操作符，都按照?定的格式进?介绍：
/// 2、格式暂定如下:
///     1、LINQ 定义
///     2、示例代码
///     3、查询表达式(如果有)
///     4、UniRx 图解(见 PPT)
///     5、示例代码
///     6、查询表达式(如果有)
///     7、所属分类及 UniRx 知识树位置(见 PPT)


///3、Where 操作符：
/// 0、在第?章第?章都有接触过 Where 操作符，这节课主要是实践?下上两堂课所说的格式规范
/// 1、LINQ Where 简介:
///     1、LINQ 中的 Where 操作符与 SQL 命令中的 Where 作?相似，都是起到范围限定也就是过滤作?的，判断条件就是它后?所接的?句
///     2、LINQ Where 示例代码:
///         --
///         // 查询?于 45 岁的学?:
///         class Student
///         {
///             public string Name;
///             public int Age;
///         }
///         private void Start()
///         {
///             List<Student> students = new List<Student>()
///             {
///                 new Student(){Name = "张三",Age = 50},
///                 new Student(){Name = "李四",Age = 51},
///                 new Student(){Name = "王五",Age = 52},
///             }
///             var oldStudents = students.Where(student => student.Age > 45);
///             foreach(var student in oldStudents)
///             {
///                 Debug.Log(student.Name);
///             }
///         }
///         --
///     3、在这?要介绍?个概念——查询语句/句法(LINQ Where 查询式)：
///         --
///         //以上代码改成查询句式
///         var oldStudents = from oldStudent in students Where oldStudent.Age > 45 select oldStudent;
/// 2、Where 操作符是?常常?的操作符
/// 3、UniRx Where 示例代码——?标点击事件监听：
///     --
///     Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => { Debug.Log("鼠标点击"); }).AddTo(this);
///     --
/// 4、UniRx Where 查询式：
///     --
///     IObservable<long> mouseClickEventStreams = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonDown(0) select updateEvent;
///     mouseClickEventStreams.Subscribe(_ => { Debug.Log("mouse clicked"); }).AddTo(this);
///     --
///         


///4、Select 操作符：
/// 1、在上?节介绍 Where 操作符时，不管 UniRx 还是 LINQ，其查询表达式都多出了?个 select，但在链式的写法中，却没有使?其对应的 Select 操作符，这是为什么呢?原因是，在 查询式写法中，编译器要求必须在最后 select ?下，?链式写法中，我们可以理解成省略掉了。
///     1、不省略的版本：var oldStudents = students .Where(student => student.Age > 45).Select(student => student);
///     2、其实就是选择了?下??。除了选择??，其实还可以选择?个其他类型的变量作为结果，如:
///         --
///         var oldStudentNames = students.Where(student => student.Age > 45).Select(student => student.Name);
///         --
/// 2、LINQ Select 简介：
///     1、LINQ 中的 Select 操作符与 SQL 命令中的 Select 作?相似，但是位置不同，查询表达式中的 select 及所接?句是放在最后并把?句中的变量也就是结果返回回来：
///         --
///         var oldStudentNames = students.Where(student => student.Age > 45) .Select(student => student.Name);
///         --
/// 3、LINQ Select 查询式：
///     --
///     var oldStudentNames = from student in students where student.Age > 45 select student.Name;
///     --
/// 4、UniRx Select 示例代码：
///     --
///     Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0)).Select(_ => "mouse up").Subscribe(Debug.Log).AddTo(this);
///     --
/// 5、UniRx Select 查询式代码：
///     --
///     var mouseUpEventSteam = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonUp(0) select "mouse up";
///     mouseUpEventSteam.Subscribe(Debug.Log).AddTo(this);
///     --


///5、First 操作符
/// 1、LINQ First 简介:
///     1、取序列中的第?个元素
///     2、First 有两种形式：
///         1、直接获取第?个元素：var oldStudent = students.Where(student => student.Age > 45).First();
///         2、取序列中满?条件的第?个元素：var oldStudent = students.First(student => student.Age > 45);
///     3、First 可以像 Where ?样传??个条件
/// 2、LINQ First 查询式：var oldStudent = (from student in students select student).First(student => student.Age > 45);
/// 3、First 操作符属于“分页”的?部分
/// 4、UniRx First 示例代码:
///     Observable.EveryUpdate().First(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("mouse down")).AddTo(this);
///     


///6、Distinct 操作符：
/// 1、Distinct 中?意思:清晰的、不同的。
/// 2、LINQ Distinct 简介:筛选序列中不相同的值，?于查询不重复的结果集。?成 SQL 语句为: SELECT DISTINCT [City] FROM [Customers]
/// 3、LINQ Distinct 示例代码：
///     1、IEnumerable<string> distinctNames = names.Distinct();
///     2、用于过滤对象也是支持的，只不过有些麻烦，需要通过实现一个类来实现
/// 4、LINQ Distinct 查询式示例代码：
///     1、IEnumerable<string> distinctNames = (from name in names select name).Distinct();
///     2、其实查询式还是有点啰嗦，不如直接?链式表达式
/// 5、Distict 是?较常?的?具
/// 6、UniRx Distinct 示例代码：
///     1、students.ToObservable().Distinct(student => student.name).Subscribe(student => Debug.Log(student.name)).AddTo(this);
///     2、UniRx 的 Distinct 在操作 List 的时候，除了?持 LINQ 所?持的两种?式外，还?持传??个特定条件函数：
///         ---
///         IObservable<string> lectClickStream = Observable.EveryUpdate()
///                                                     .Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "left clicked");
///         IObservable<string> rightClickStream = Observable.EveryUpdate()
///                                                     .Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "right clicked");
///         Observable.Merge(lectClickStream, rightClickStream).Distinct().Subscribe(Debug.Log).AddTo(this);
///         //以上代码结果为:不管点击多少次?标左键还是?标右键，输出只输出?次 “left clicked” 和 “right clicked”
///         ---
///         


///7、Last 操作符：
/// 1、LINQ Last 简介：取序列中的最后?个元素
/// 2、LINQ Last 代码示例:Student student = students.Last();
/// 3、Last 属于分?操作符
/// 4、UniRx Last 代码示例：
///   students.ToObservable().Last(student => student.age == 50).Subscribe(student => Debug.Log(student.name)).AddTo(this);
/// 5、Last ?持传??个条件函数，? LINQ 中的 LAST 只能传??个条件?法。
/// 


///8、SelectMany 操作符：
/// 1、LINQ SelectMany 简介：
///     1、微软官?的定义是: 将序列的每个元素投影到 IEnumerable<T> 并将结果序列合并为?个序列。
///     2、对每项再进?遍历处理再进?合成序列。
/// 2、LINQ SelectMany 示例代码：IEnumerable singleChars = students.SelectMany(student => student.name + ":" + student.age);
///     1、SelectMany 的作用：假设一个 List<string> 集合 nums 有“123”、“456”、“789” 三个字符串
///     2、通过 nums.SelectMany 之后 (nums.SelectMany(n => n));得到一个新集合{'1','2','3','4','5','6','7','8','9'}
///     3、为什么 string 字符串可以被打散成字符？因为 string 继承了 IEnumerable 接口，是可迭代的
///     4、返回的 n 可以进行操作：nums.SelectMany(n => n + ":");
///         1、得到一个新集合{'1',':','2',':','3',':','4',':','5',':'……}
///         2、每遍历一个 n，返回 n 本身和一个 ':'
///     5、或者直接返回其它字符串：nums.SelectMany(n => "asd");
///         1、得到一个新集合{'a','s','d','a','s','d','a','s','d'……}
///         2、每遍历一个 n，返回三个字符 'a','s','d'
/// 3、UniRx SelectMany 是比较常用的
/// 4、UniRx SelectMany 代码示例：SelectMany 理解起来会?较晦涩，在 UniRx 中主要是完成 Coroutine 的顺序执?功能：
///     ---
///     IEnumerator A()
///     {
///         yield return new WaitForSeconds(1.0f);
///         Debug.Log("A");
///     }
///     
///     IEnumerator B()
///     {
///         yield return new WaitForSeconds(2.0f);
///         Debug.Log("B");
///     }
///     
///     IEnumerator C()
///     {
///         yield return new WaitForSeconds(3.0f);
///         Debug.Log("C");
///     }
///     
///     var streamA = Observable.FromCoroutine(A);
///     var streamB = Observable.FromCoroutine(B);
///     var streamC = Observable.FromCoroutine(C);
///     streamA.SelectMany(streamB.SelectMany(streamC)).Subscribe(_ => Debug.Log("Hello"));
///     ---
/// 


///9、Take 操作符：
/// 1、LINQ Take 简介：从序列的开头返回指定数量的相邻元素
/// 2、LINQ Take 代码示例：
///     ---
///     int[] grades = { 59, 84, 69, 30, 59, 94, 83 };
///     IEnumerable<int> topThreeGrades = grades.OrderByDescending(grade => grade).Take(3);//降序排序后取前三个
///     ---
/// 3、Take 操作符只发射前?的 N 项数据
/// 4、使? Take 操作符可以修改 Observable 的?为，只返回前?的 N 项数据，然后发射完成通知，忽略剩余的数据
/// 5、UniRx Take 示例代码：
///     this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Take(5).Subscribe(_ => Debug.Log(1));
/// 6、Take 可以传递一个时间段进来：
///     ---
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))// 当点击鼠标左键
///         .Take(TimeSpan.FromSeconds(5))// 只在前五秒内处理，超过五秒无效
///         .Subscribe(_ => Debug.Log("mouse clicked"))
///         .AddTo(this);
///     ---


///10、Concat 操作符：
/// 1、LINQ Concat 简介：连接两个序列
/// 2、LINQ Concat 示例代码：
///     ---
///     List<Student> students2 = new List<Student>()
///     {
///         new Student() { name = "ddd", age = 20 },
///         new Student() { name = "ccc", age = 60 },
///         new Student() { name = "aaa", age = 30 },
///     };
///     var studentNames = students.Select(student => student.name).Concat(students2.Select(student => student.name));
///     foreach (string studentName in studentNames)
///         Debug.Log(studentName);
///     ---
/// 3、Concat：不交错的发射两个或多个 Observable 的发射物
///     1、Concat 操作符连接多个 Observable 的输出，就好像它们是?个 Observable，第?个 Observable 在第?个 Observable 发射的任何数据前?发射所有数据，以此类推。直到前??个 Observable 终?，Concat 才会订阅额外的?个 Observable。
///     2、注意：如果你尝试连接?个"热" Observable（这种 Observable 在创建后?即开始发射数据，即使没有订阅者），Concat 将不会看到也不会发射它之前发射的任何数据。
/// 4、Concat 与 Merge 和 WhenAll 类似，是联合操作符
/// 5、UniRx Concat 示例代码：
///     ---
///     IObservable<string> a = this.UpdateAsObservable().Take(3).Select(_ => "A");
///     IObservable<string> b = this.UpdateAsObservable().Take(2).Select(_ => "B");
///     IObservable<string> c = a.Concat(b);
///     c.Subscribe(Debug.Log);
///     ---
///     


///11、WhenAll:
/// 1、LINQ All 简介：确定序列中的所有元素是否都满?条件
/// 2、LINQ All 示例代码：
///     ---
///     bool allStartWithB = students.All(student => student.name.StartsWith("B"));
///     Debug.LogFormat("{0} student names start with 'B'.", allStartWithB ? "All" : "Not All");
///     ---
/// 3、UniRx WhenAll：
///     1、判定 Observable 发射的所有数据是否都满?某个条件
///     2、传递?个谓词函数给 WhenAll 操作符，这个函数接受原始 Observable 发射的数据，根据计算返回?个布尔值。
///     3、WhenAll 返回?个只发射?个单个布尔值的 Observable，如果原始 Observable 正常终?并且每?项数据都满?条件，就返回 true；如果原始 Observable 的任意?项数据不满?条件就返回 False
/// 4、UniRx WhenAll 示例代码：
///     ---
///     IEnumerator A()
///     {
///         yield return new WaitForSeconds(1.0f);
///         Debug.Log("A");
///     }
///     IEnumerator B()
///     {
///         yield return new WaitForSeconds(1.0f);
///         Debug.Log("B");
///     }
///     IEnumerator C()
///     {
///         yield return new WaitForSeconds(1.0f);
///         Debug.Log("C");
///     }
///     IObservable<Unit> streamA = Observable.FromCoroutine(A);
///     IObservable<Unit> streamB = Observable.FromCoroutine(B);
///     IObservable<Unit> streamC = Observable.FromCoroutine(C);
///     Observable.WhenAll(streamA, streamB, streamC).Subscribe(_ => Debug.Log("Completed"));
///     // 输出结果为(A、B、C 输出顺序不?定)
///     


///12、OfType 操作符：
/// 1、LINQ OfType 简介:根据指定类型筛选 IEnumerable 的元素。
/// 2、LINQ OfType 代码示例:
///     ---
///     ArrayList list = new ArrayList { 30, 30.0f, "test" };
///     list.OfType<float>().ToList().ForEach(num => Debug.Log(num));
///     ---
/// 3、UniRx OfType 示例代码：
///     ---
///     // 创建一个 Subject(Observable)
///     Subject<object> objects = new Subject<object>();
///     
///     // 订阅改 Observable，进行类型过滤(发布通知)
///     objects.OfType<object, string>().Subscribe(Debug.Log);//从 object 类型转为 string 类型
///     objects.OnNext(1);
///     objects.OnNext(2);
///     objects.OnNext("3");
///     objects.OnNext(5);
///     
///     // 手动结束广播
///     objects.OnCompleted();
///     ---


///13、Cast 操作符：
/// 1、LINQ Cast 简介：将 IEnumerable 的元素强制转换为指定的类型
/// 2、LINQ Cast 代码示例：
///     ---
///     ArrayList fruits = new ArrayList { "mango", "apple", "lemon" };
///     IEnumerable fruitNames = fruits.Cast<string>();
///     foreach (var fruit in fruitNames)
///         Debug.Log(fruit);
///     ---
/// 3、UniRx Cast 代码示例：
///     ---
///     Subject<object> objects = new Subject<object>();
///     objects.Cast<object, int>().Subscribe(i => Debug.Log(i));//将 object 转为 int 类型
///     
///     // 手动发送数据(广播)
///     objects.OnNext(1);
///     objects.OnNext(2);
///     objects.OnNext(3);
///     //objects.OnNext("4");//报错，转换异常
///     
///     // 手动结束广播
///     objects.OnCompleted();
/// 4、TakeWhile 是用于转换的操作符


///14、GroupBy 操作符：
/// 1、LINQ GroupBy 简介：对序列中的元素进?分组
/// 2、LINQ GroupBy 代码示例：
///     ---
///     IEnumerable<IGrouping<string,Student>> studentGroupNames = students.GroupBy(student => student.name);
///     foreach (IGrouping<string, Student> studentGroup in studentGroupNames)
///     {
///         Debug.Log($"Group Key:{studentGroup.Key}");
///         foreach (Student student in studentGroup)
///         Debug.Log($"Name:{student.name} Age:{student.age}");
///     }
///     ---
/// 3、LINQ GroupBy 查询表达式：
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10).GroupBy(i => i % 3).Subscribe(group =>
///     {
///         group.Subscribe(number => Debug.Log($"key:{group.Key},number:{number}"));
///     });
/// 4、TakeWhile 也是用于分页的操作符


///15、Range 操作符：
/// 1、LINQ Range 简介：?成指定范围内的整数的序列
/// 2、LINQ Range 代码示例：
///     ---
///     IEnumerable<int> squares = Enumerable.Range(5, 5).Select(x => x * x);//从 5 开始往上涨的三个数 5 6 7
///     squares.ToList().ForEach(num => Debug.Log(num));
///     ---
/// 3、UnIRx Range 代码示例：
///     ---
///     IObservable<int> squares = Observable.Range(5, 3).Select(x => x * x);
///     squares.Subscribe(square => Debug.Log(square));
///     ---
/// 4、通过 LINQ 与 UniRx 的?较可以看到，LINQ 也有类似 Observable 的 Enumerable，? Subscribe 其实相当于 LINQ 中的 foreach
/// 5、TakeWhile 是用于 create 生成的操作符
/// 


///16、Skip 操作符：
/// 1、LINQ Skip 简介：跳过序列中指定数量的元素，然后返回剩余的元素
/// 2、LINQ Skip 代码示例：
///     ---
///     int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };
///     grades.OrderByDescending(g => g).Skip(3).ToList().ForEach(num => Debug.Log(num));//降序排序后跳过前三名
///     ---
/// 3、UniRx Skip 代码示例：
///     ---
///     this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Skip(5).Subscribe(_ => Debug.Log("mouse clicked"));
///     //输出结果为，?标点击第六次时，才开始输出”mouse clicked”
/// 4、TakeWhile 也是用于分页的操作符
/// 5、Skip 操作符可以传递一个时间段过来：
///     ---
///     Observable.EveryUpdate()
///     .Where(_ => Input.GetMouseButtonDown(0))// 当点击鼠标左键
///     .Skip(TimeSpan.FromSeconds(5))// 跳过前五秒（前五秒内点击都无效，五秒后每次点击都有效）
///     .Subscribe(_ => Debug.Log("mouse clicked"))
///     .AddTo(this);
///     ---


///17、TakeWhile 操作符：
/// 1、LINQ TakeWhile 简介：如果指定的条件为 true，则返回序列中的元素，然后跳过剩余的元素
/// 2、LINQ TakeWhile 示例代码：
///     ---
///     string[] fruits = new[] { "apple", "banana", "mango", "orange", "grape", "passionfruit" };
///     IEnumerable<string> fruitsAfterOrange = fruits.TakeWhile(fruit => fruit != "orange");
///     fruitsAfterOrange.ToList().ForEach(value => Debug.Log(value));
///     ---
/// 3、TakeWhile 也是用于分页的操作符
/// 4、UniRx TakeWhile 代码示例：
///     ---
///     this.UpdateAsObservable().TakeWhile(value => !Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("before click"));
///     ---
/// 5、TakeWhile 的重载：int 值表示事件触发的次数
///     Observable.EveryUpdate()
///             .Where(_ => Input.GetMouseButton(0))// 鼠标按下不动
///             .TakeWhile((_, number) => !Input.GetMouseButtonUp(0) && number < 100)//当鼠标没有抬起并且次数小于 100,number 表示事件触发次数
///             .Subscribe(_ => Debug.Log("mouse button clicking"));


///18、SkipWhile 操作符：
/// 1、LINQ SkipWhile 简介：如果指定的条件为 true，则跳过序列中的元素，然后返回剩余的元素
/// 2、LINQ SkipWhile 代码示例：
///     ---
///     int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };
///     
///     // 降序排序，如果当前成绩大于等于 80，则跳过该值，如果当前成绩小于 80，返回剩余的值
///     IEnumerable<int> lowerGrades = grades.OrderByDescending(grade => grade).SkipWhile(grade => grade >= 80);
///     lowerGrades.ToList().ForEach(value => Debug.Log(value));
///     ---
/// 3、SkipWhile 操作符是用于分页的操作符
/// 4、UniRx SkipWhile 代码示例：
///     ---
///     this.UpdateAsObservable()
///         .SkipWhile(_ => !Input.GetMouseButtonDown(0))// 当鼠标按下时，收集之后的事件
///         .Subscribe(_ => Debug.Log("mouse button down"));
///     // 实现的逻辑为，当点击?标后，持续输出 mouse button down
///     ---
///     


///19、Zip (.Net 4) 操作符：
/// 1、LINQ Zip 简介：将指定函数应用于两个序列的对应元素，以生成结果序列
/// 2、LINQ Zip 示例：
///     ---
///     int[] numbers = { 1, 2, 3, 4 };
///     string[] words = { "one", "two", "three", "four" };
///     IEnumerable<string> numbersAndWords = numbers.Zip(words, (first, second) => $"{first} {second}");
///     // 输出 1 one，2 two……
///     numbersAndWords.ToList().ForEach(value => Debug.Log(value));
///     ---
/// 3、Zip 是用于联合的操作符
/// 4、UniRx Zip 示例代码：
///     ---
///     IObservable<Unit> rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
///     IObservable<Unit> leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
///     leftStream.Zip(rightStream, (left, right) => Unit.Default).Subscribe(_ => Debug.Log("ok"));
///     // 只要左右键成对就会输出 ok，先按两次左键，再按两次右键，输出两次 ok
///     ---
/// 


///20、Repeat 操作符：
/// 1、LINQ Repeat 简介：在?成序列中重复该值的次数
/// 2、LINQ Repeat 代码示例：
///     ---
///     IEnumerable<string> strs = Enumerable.Repeat("I like programming.", 5);
///     foreach (string str in strs)
///         Debug.Log(str);
///     ---
/// 3、Repeat 操作符是用于 Creation 创造的操作符
/// 4、UniRx Repeat 代码示例：
///     ---
///     // 不使用 Repeat，一秒后输出一次字符串，之后不再输出
///     Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log("after 1 seconds"));
///     
///     // 使用 Repeat，每秒输出一次字符串(让事件流重复执行)
///     Observable.Timer(TimeSpan.FromSeconds(1.0f)).Repeat().Subscribe(_ => Debug.Log("after 1 seconds"));
///     ---
///     


///21、TakeLast 操作符：
/// 1、LINQ TakeLast 简介：获取序列的最后?项
/// 2、LINQ TakeLast 代码示例：由于 TakseLast 操作符是在 .Net Core 提供的，所以就不进?代码示例的介绍了
/// 3、TakeLast 作用是发射 Observable 发射的最后 N 项数据
/// 4、TakeLast 除了可以传个数，也可以传时间段
/// 5、TakeLast 是用于分页的操作符
/// 6、UniRx TakeLast 代码示例：
///     ---
///     IObservable<int> bottonThreeGrades = grades.OrderByDescending(grade => grade).ToObservable().TakeLast(3);
///     // 订阅 bottonThreeGrades 事件，当有某个值满足 bottonThreeGrades 事件，执行回调方法（效果类似于 foreach）
///     bottonThreeGrades.Subscribe(buttonThreeGrade => Debug.Log(buttonThreeGrade));
///     ---
/// 7、传递时间段进 TakeLast 操作符
///     ---
///     Subject<float> subject = new Subject<float>();
///     
///     subject.TakeLast(TimeSpan.FromSeconds(2.0))// 收集 subject 最后两秒的内容，执行回调
///         .Subscribe(clickTime => Debug.Log(clickTime));
///     
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))// 点击鼠标左键，执行回调方法
///         .Subscribe(_ => subject.OnNext(Time.time));// 广播当前事件
///     Observable.Timer(TimeSpan.FromSeconds(5.0f))// 五秒后触发事件，执行回调方法
///         .Subscribe(_ => subject.OnCompleted());// 五秒后停止 subject 的广播
///     ---
///     


///22、Single/Default 操作符：
/// 1、LINQ Single 简介：返回序列中的单个特定元素，与 First ?常类似，但是 Single 要确保其满?条件的元素在序列中只有?个
/// 2、LINQ Single 代码示例：
///     ---
///     string fruit1 = fruits.Single(fruit => fruit.Length > 10);
///     Debug.Log(fruit1);
///     ---
/// 3、Single 操作符与 First 类似，但如果原始 Observable 在完成之前不是正好发射?次数据，它会抛出?个 NoSuchElementException
/// 4、Single 操作符是用于分页的操作符
/// 5、UniRx Single 代码示例：fruits.ToObservable().Single(fruit => fruit.Length > 10).Subscribe(Debug.Log);
/// 6、如果满足 Single 的条件有两个或两个以上，会抛出异常
/// 


///23、ToArray 操作符：
/// 1、LINQ ToArray 简介：从 IEnumerable<T> 中创建数组
/// 2、LINQ ToArray 示例代码：
///     ---
///     string[] studentNames = students.Select(student => student.name).ToArray();
///     foreach (var name in studentNames)
///         Debug.Log(name);
///     ---
/// 3、ToArray 操作符是很常用的操作符
/// 4、UniRx ToArray 示例代码：
///     ---
///     Subject<int> subject = new Subject<int>();
///     subject.ToArray().Subscribe(intArray =>
///     {
///         Debug.Log(intArray.GetType().ToString());
///         foreach (int i in intArray)
///             Debug.Log(i);
///     });
///     subject.OnNext(1);
///     subject.OnNext(2);
///     subject.OnNext(3);
///     subject.OnCompleted();
///     


///24、ToList 操作符：
/// 1、LINQ ToList 简介：从 IEnumerable<T> 创建?个 List<T>
/// 2、LINQ ToList 示例代码：
///     ---
///     List<int> lengths = fruits.Select(fruit => fruit.Length).ToList();
///     lengths.ForEach(value => Debug.Log(value));
///     ---
/// 3、ToList 操作符是很常用的操作符
/// 4、UniRx ToList 示例代码：
///     ---
///     Subject<int> subject = new Subject<int>();
///     subject.ToList().Subscribe(intList =>
///     {
///         Debug.Log(intList.GetType().ToString());
///         foreach (int i in intList)
///             Debug.Log(i);
///     });
///     subject.OnNext(1);
///     subject.OnNext(2);
///     subject.OnNext(3);
///     subject.OnCompleted();
///     


///25、Aggregate 操作符：
/// 1、LINQ Aggregate 简介：对序列应用累加器函数。 将指定的种子值用作累加器的初始值，并使用指定的函数选择结果值
/// 2、LINQ Aggregate 代码示例：
///     ---
///     int[] numbers = new[] { 1, 2, 3, 4, 5 };
///     int result = numbers.Aggregate((total, next) => total * next);
///     Debug.Log($"5 的阶乘为：{result}");// 返回 120，即 1 * 2 * 3 * 4 * 5
///     ---
/// 3、Aggregate 操作符是用于聚合操作的操作符
/// 4、UniRx Aggregate 代码示例：
///     ---
///     Observable.Range(0, 8).Aggregate(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));
///     ---
/// 5、Aggregate 可用于取最大值和最小值操作：
///     ---
///     int[] numbers = new[] { 1, 2, 3, 4, 5 };
///     int result = numbers.Aggregate((total, next) => total = next > total ? next : total);
///     Debug.Log($"最大值为{result}");
///     ---
///     


///26、Empty 操作符：
/// 1、LINQ Empty 简介：返回具有指定类型参数的空 IEnumerable<T>
/// 2、LINQ Empty 示例代码：
///     ---
///     string[] names1 = { "Hartono, Tommy" };
///     string[] names2 = { "Adams, Terry", "Andersen, Henriette Thaulow", "Hedlund, Magnus", "Ito, Shu" };
///     string[] names3 = { "Solanki, Ajay", "Hoeing, Helge", "Andersen, Henriette Thaulow", "Potra, Cristina", "Iallo, Lucio" };
///     var namesList = new List<string[]> { names1, names2, names3 };
///     var allNames = namesList.Aggregate(Enumerable.Empty<string>(), (current, next) => next.Length > 3 ? current.Union(next) : current);
///     foreach (var name in allNames)
///     {
///         Debug.Log(name);
///     }
/// 3、Empty 操作符属于 Creation 创造操作符，在测试的时候用得比较多
/// 4、UniRx Empty 代码示例：
///     ---
///     IObservable<Unit> s = Observable.Empty<Unit>();
///     s.Subscribe(e => Debug.Log($"e:{e}"), () => Debug.Log("OnCompleted"));
///     ---
///     


///27、小结：
/// 到此为止已经学完了 UniRx 从 LINQ 所借鉴来的所有操作符了，这部分操作符的性价?其实是最?的，因为理解了这些操作符 LINQ 的?部分操作符也就都理解了。
/// 在本章前?节有提过，通过 LINQ 操作符去理解 UniRx 操作符会更加容易，但是 UniRx 的 LINQ 操作符数量有限，LINQ 操作符也只占UniRx 操作符的 50% 左右
/// 下?章节开始学习 UniRx 从 Rx.Net 中借鉴的操作符(Rx.Net 已作为微软官方库，可直接使用 System.Reactive 使用 Rx.Net)
/// 

#endregion


#region 2、UniRx 从 Rx.Net 中借鉴的操作符(Rx 独有的操作符)
///1、Interval 操作符：
/// 1、Interval 操作符是于时间相关的操作符，中文意思是“间隔”
/// 2、Interval 代码示例：
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(second => Debug.Log($"当前时间：{second} s")).AddTo(this);
///     
///     // 以上效果也可使用 Repeat 实现
///     //Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe()).AddTo(this);
///     ---
///     


///2、TakeUntil 操作符：
/// 1、当第?个 Observable 发射了?项数据或者终?时，原始 Observable 停止发射数据
/// 2、TakeUntil 订阅并开始发射原始Observable，它还监视你提供的第?个 Observable。如果第?个 Observable 发射了?项数据或者发射了?个终?通知，TakeUntil 返回的 Observable 会停?发射原始 Observable 并终?
/// 3、TakeUntil 操作符是用于分页操作的操作符
/// 4、TakeUntil 示例代码：
///     ---
///     // 输出结果为：持续输出 123，当点击?标左键后，停?输出 123
///     this.UpdateAsObservable()//第一个 Observable
///         .TakeUntil(Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)))// 括号内是第二个 Observable
///         .Subscribe(_ => Debug.Log(123))
///         .AddTo(this);
///     ---
/// 


///3、SkipUntil 操作符：
/// 1、丢弃原始 Observable 发射的数据，直到第?个 Observable 发射了?项数据
/// 2、SkipUntil 操作符也是用于分页操作的操作符
/// 3、SkipUntil 代码示例：
///     ---
///     // 输出结果为：点击?标左键之后就开始持续输出 “?标按过了”
///     IObservable<Unit> clickStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
///     this.UpdateAsObservable()
///         .SkipUntil(clickStream)// 在 clickStream 发射数据之前，跳过当前事件
///         .Subscribe(_ => Debug.Log("鼠标按过了"))
///         .AddTo(this);
///         


///4、Buffer 操作符(缓冲)：
/// 1、Buffer 操作符也是用于分页操作的操作符
/// 2、代码示例：
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(1.0f))// 每隔一秒发射一次数据
///                 .Buffer(TimeSpan.FromSeconds(3.0f))// 缓存 3 秒内的数据
///                 .Subscribe(_ => Debug.Log($"currentTime:{DateTime.Now.Second}"))// 缓存数据后执行回调方法
///                 .AddTo(this);
///     ---
///     


///5、Throttle(节流阀)操作符：
/// 1、在过了?段指定的时间还没发射数据时才发射?个数据
/// 2、Throttle 节流阀操作符是与时间有关的操作符
/// 3、示例代码：
///     ---
///     //输出结果：点击?标后 1 秒内不再点击则输出，有点击则重新计时 1 秒再输出
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))// 当点击鼠标时
///         .Throttle(TimeSpan.FromSeconds(1))//一秒后不再点击则执行回调，否则重新计时一秒，直到重新计时开始一秒后不再点击，执行回调
///         .Subscribe(_ => Debug.Log("一秒过后"));
///     ---
///     


///6、Delay 操作符：
/// 1、延迟?段指定的时间再发射来? Observable 的发射物
/// 2、Delay 操作符也是与时间相关的操作符
/// 3、Delay 代码示例：
///     ---
///     // 输出结果：点击?标 1 秒之后输出 mouse clicked，每次点击事件都是 1 秒之后才输出
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Subscribe(_ => Debug.Log("mouse clicked"))
///         .AddTo(this);
///     ---
///     


///7、Return 操作符：
/// 1、Return 操作符是与 Creation 创造操作相关的操作符
/// 2、Return 示例代码：
///     ---
///     Observable.Return("hello").Subscribe(Debug.Log);//只执行一次，类似 set
///     ---
///     


///8、Timer 操作符：
/// 1、创建?个 Observable，它在?个给定的延迟后发射?个特殊的值。
/// 2、Timer 操作符是与时间操作相关的操作符
/// 3、Timer 代码示例：
///     ---
///     // 执?结果：在 5 秒后输出 “after 5 seconds”
///     //Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => Debug.Log("after 5 seconds")).AddTo(this);
///     Observable
///         .Timer(TimeSpan.FromSeconds(1.0f),TimeSpan.FromSeconds(2.0f))// 在一秒之后，每隔两秒发射一次数据
///         .Subscribe(_ => Debug.Log("after 5 seconds"))
///         .AddTo(this);
///         


///9、Sample 操作符：
/// 1、定期发射 Observable 最近发射的数据项
/// 2、Sample 操作符会定时查看?个Observable，然后发射?上次采样以来它最近发射的数据。
/// 3、在某些实现中，有?个 ThrottleFirst 操作符的功能类似，但不是发射采样期间的最近的数据，?是发射在那段时间内的第?项数据
/// 4、RxJava 将这个操作符实现为 sample 和 throttleLast
/// 5、注意：如果?上次采样以来，原始 Observable 没有发射任何数据，这个操作返回的 Observable 在那段时间内也不会发射任何数据
/// 6、Sample(别名 throttleLast)的?个变体按照参数中指定的时间间隔定时采样（TimeUnit 指定时间单位）
/// 7、Sample 的这个变体默认在 computation 调度器上执?，但是你可以使?第三个参数指定其它的调度器
/// 8、Sample 的这个变体每当第?个 Observable 发射?个数据（或者当它终?）时就对原始 Observable 进?采样。第?个 Observable 通过参数传递给 Sample，Sample 的这个变体默认不在任何特定的调度器上执?
/// 9、Sample 操作符是与时间操作相关的操作符
/// 10、示例代码：
///     ---
///     Observable.Interval(TimeSpan.FromMilliseconds(50))//每隔 50 毫秒发射一次数据
///                 .Sample(TimeSpan.FromSeconds(1))// 每隔一秒进行一次采样，如果当前采样的数据在之前已经采过样了，那么就不再进行采样，直到采样到的数据是之前每采样过的，才进行采样
///                 .Subscribe(_ => Debug.Log(DateTime.Now.Second))
///                 .AddTo(this);
///     
///     int clickCount = 0;
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .Select(_ => clickCount++)
///         .Sample(TimeSpan.FromSeconds(3))
///         .Subscribe(_ => Debug.Log($"第 {clickCount} 次点击"))
///         .AddTo(this);
///         


///10、Timestamp 操作符：
/// 1、给 Observable 发射的数据项附加?个时间戳
/// 2、RxJava 中的实现为 timestamp，它将?个发射 T 类型数据的 Observable 转换为?个发射类型为Timestamped<T> 的数据的 Observable，每?项都包含数据的原始发射时间。timestamp 默认在 immediate 调度器上执?，但是可以通过参数指定其它的调度器。
/// 3、Timestamp 操作符是与时间操作相关的操作符
/// 4、Timestamp 代码示例：
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(1.0f))
///                 .Timestamp()
///                 //.Subscribe(timestamp => Debug.Log(timestamp))
///                 .Subscribe(timestamp => Debug.Log(timestamp.Timestamp.LocalDateTime))//输出当地时间
///                 .AddTo(this);
///                 


///11、ThrottleFirst 操作符：
/// 1、ThrottleFirst 与 throttleLast/sample 不同，在每个采样周期内，它总是发射原始 Observable 的第?项数据，?不是最近的?项
/// 2、ThrottleFirst 操作符默认在 computation 调度器上执?，但是你可以使?第三个参数指定其它的调度器
/// 3、ThrottleFirst 与 Sample ?常类似
/// 4、ThrottleFirst 操作符是与时间操作相关的操作符
/// 5、ThrottleFirst 代码示例：
///     ---
///     //运?结果：?标点击之后，?即输出 “Clicked”，输出之后的 5 秒内点击?效
///     this.UpdateAsObservable()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .ThrottleFirst(TimeSpan.FromSeconds(5))
///         .Subscribe(x => Debug.Log("Clicked"))
///         .AddTo(this);
///     ---
///     


///12、TimeInterval 操作符：
/// 1、将?个发射数据的 Observable 转换为 发射那些数据 时间间隔的 Observable
/// 2、TimeInterval 操作符会拦截原始 Observable 发射的数据项，替换为发射表示相邻发射物时间间隔的对象
/// 3、TimeInterval 操作符是与时间操作相关的操作符
/// 4、TimeInterval 代码示例：
///     ---
///     Observable.EveryUpdate()
///                 .Where(_ => Input.GetMouseButtonDown(0))
///                 .Select(_ => "clicked")
///                 .TimeInterval()
///                 .Subscribe(timeInterval => Debug.Log($"{timeInterval.Interval} : {timeInterval.Value}"))
///                 .AddTo(this);
///     ---
///     


///13、Defer 操作符：
/// 1、Defet：推迟、延期、延迟
/// 2、直到有观察者订阅时才创建 Observable，并且为每个观察者创建?个新的 Observable
/// 3、Defer 操作符会?直等待直到有观察者订阅它，然后它使? Observable ???法?成?个 Observable
///     --它对每个观察者都这样做，因此尽管每个订阅者都以为??订阅的是同?个 Observable，事实上每个订阅者获取的是??的单独的数据序列
///     --在某些情况下，等待直到最后?分钟（就是知道订阅发?时）才?成 Observable 可以确保 Observable 包含最新的数据
/// 4、Defer 操作符是与创造操作相关的操作符
/// 5、Defer 代码示例：
///     ---
///     System.Random random = new System.Random();
///     Observable
///         .Defer(() => Observable.Start(() => random.Next()))//Observable.Start(() => random.Next()) 会开启一个线程创建一个新的 Observable，然后获得下一个随机数
///         .Delay(TimeSpan.FromMilliseconds(1000))
///         .Repeat()
///         .Subscribe(randomNumber => Debug.Log(randomNumber))// 每次订阅时，Defer 会创建一个 Observable 
///         .AddTo(this);
///         


///14、Never 操作符：
/// 1、创建?个不发射数据也不终?的 Observable
/// 2、Never 操作符是与创造操作相关的操作符
/// 3、Never 示例代码：
///     ---
///     /*//?前还不知道?嘛?，没有输出结果
///     IObservable<string> never = Observable.Never<string>();
///     // 类似于没有广播/通知的 Subject
///     Subject<string> subject = new Subject<string>(); */
///     ---
///     


///15、Scan 操作符：
/// 1、连续地对数据序列的每一项应用一个函数，然后连续发射结果
/// 2、Scan操作符对原始 Observable 发射的第?项数据应??个函数，然后将那个函数的结果作为??的第?项数据发射。它将函数的结果同第?项数据?起填充给这个函数来产?它??的第?项数据。它持续进?这个过程来产?剩余的数据序列。这个操作符在某些情况下被叫做 accumulator。
/// 3、与 Aggregate 类似，但是 Scan 是每次进?输出，? Aggregate 则是结束计算后进?输出。
/// 4、Scan 操作符是与聚合操作相关的操作符
/// 5、示例代码：
///     ---
///     Observable.Range(0, 8).Scan(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));
///     ---
///     


///16、Switch 操作符：
/// 1、将?个发射多个 Observables 的 Observable 转换成另?个单独的 Observable，后者发射那些 Observables 最近发射的数据项
/// 2、Switch 订阅?个发射多个 Observables 的 Observable。它每次观察那些 Observables 中的?个，Switch 返回的这个 Observable 取消订阅前?个发射数据的 Observable，开始发射最近的 Observable 发射的数据。
/// 3、注意：当原始 Observable 发射了?个新的 Observable 时（不是这个新的 Observable 发射了?条数据时），它将取消订阅之前的那个 Observable。这意味着，在后来那个 Observable 产?之后到它开始发射数据之前的这段时间?，前?个 Observable 发射的数据将被丢弃（就像图例上的那个??圆圈?样）。
/// 4、Switch 操作符是与联合操作相关的操作符
/// 5、Switch 代码示例：
///     ---
///     // 执?结果：当按下?标时输出 “mouse button down” 抬起之后输出 “mouse button up”
///     var buttonDownStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
///     var buttonUpStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0));
///     buttonDownStream.Select(_ =>
///     {
///         Debug.Log("mouse button down");
///         return buttonUpStream;
///     })
///         .Switch()// 切换事件流到 buttonUpStream
///         .Subscribe(_ => Debug.Log("mouse button up"))// buttonUpStream 事件流的回调方法
///         .AddTo(this);
///     ---
/// 6、使用 Switch 操作符实现“彩蛋”功能:当按顺序按下 W A S D 时触发彩蛋：
///     ---
///     var wObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.W));
///     var aObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.A));
///     var sObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.S));
///     var dObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.D));
///     
///     wObservable.Select(_ => aObservable)
///         .Switch()// 切换至 aObservable 事件流
///         .Select(_ => sObservable)
///         .Switch()// 切换至 sObservable 事件流
///         .Select(_ => dObservable)
///         .Switch()// 切换至 dObservable 事件流
///         .Repeat()
///         .Subscribe(_ => Debug.Log("触发彩蛋"))
///         .AddTo(this);
///         


///17、StartWith 操作符：
/// 1、如果你想要?个 Observable 在发射数据之前先发射?个指定的数据序列，可以使? StartWith 操作符。（如果你想让?个 Observable 发射的数据末尾追加?个数据序列可以使? Concat 操作符）
/// 2、可接受?个 Iterable 或者多个 Observable 作为函数的参数
/// 3、StartWith 操作符是与联合操作相关的操作符
/// 4、StartWith 代码示例：
///     --
///     Observable.Return("sikiedu.com")
///                 .StartWith("http://")
///                 .Aggregate((current, next) => current + next)// 如果没有 Aggregate 操作，会先输出 "http://"，而不是拼接成一个字符串
///                 .Subscribe(Debug.Log)
///                 .AddTo(this);
///                 


///18、CombineLatest 操作符：
/// 1、当两个 Observables 中的任何?个发射了数据时，使??个函数结合每个 Observable 发射的最近数据项，并且基于这个函数的结果发射数据。
/// 2、CombineLatest 操作符?为类似于 zip：
///     1、只有当原始的 Observable 中的每?个都发射了?条数据时 zip 才发射数据
///     2、CombineLatest 则在原始的 Observable 中任意?个发射了数据时发射?条数据
/// 3、当原始 Observables 的任何?个发射了?条数据时，CombineLatest 使??个函数结合它们最近发射的数据，然后发射这个函数的返回值。
/// 4、CombineLatest 操作符是与联合操作相关的操作符
/// 5、示例代码：
///     ---
///     int a = 0;
///     int i = 0;
///     var leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => (++a).ToString());
///     var rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => (++i).ToString());
///     leftStream.CombineLatest(rightStream, (left, right) => left + right).Subscribe(Debug.Log);
///     ---
///     


///19、Do 操作符：
/// 1、注册?个动作作为原始 Observable ?命周期事件的?种占位符
/// 2、Do 操作符是很常用的操作符
/// 3、示例代码：
///     ---
///     Observable.ReturnUnit()// 返回空
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Do(_ => Debug.Log("after 1 seconds"))
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Do(_ => Debug.Log("after 2 seconds"))
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Subscribe(_ => Debug.Log("after 3 seconds"))
///         .AddTo(this);
///     ---
///     


///20、Merge 操作符：
/// 1、Merge 操作符是与联合操作相关的操作符
/// 2、代码示例：
///     ---
///     // 输出结果：点击?标左键输出 “A”，点击?标右键输出 “B”
///     var aStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "A");
///     var bStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "B");
///     aStream.Merge(bStream).Subscribe(Debug.Log);
///     ---
///     


///21、Materialize/Dematerialize 操作符：
/// 1、Materialize 将数据项和事件通知都当做数据项发射
/// 2、?个合法的有限的 Obversable 将调?它的观察者的 onNext ?法零次或多次，然后调?观察者的 onCompleted 或 onError 正好?次。Materialize 操作符将这?系列调?，包括原来的通知 onNext 和终?通知 onCompleted 或 onError 都转换为?个 Observable 发射的数据序列
/// 3、Dematerialize 操作符是 Materialize 的逆向过程，它将 Materialize 转换的结果还原成它原本的形式
/// 4、Dematerialize 反转这个过程，将原始 Observable 发射的 Notification 对象还原成 Observable 的通知
/// 5、Materialize/Dematerialize 操作符是常用操作相关的操作符
/// 6、Materialize/Dematerialize 代码示例：
///     ---
///     Subject<int> subject = new Subject<int>();
///     IObservable<int> onlyExceptions = subject.Materialize().Where(n => n.Exception != null).Dematerialize();
///     
///     // 对 subject 进行注册
///     subject.Subscribe(i => Debug.Log($"Subscriber 1:{i}"), ex => Debug.Log($"Subscriber 1 exception: {ex.Message}"));
///     
///     // 对 Dematerialize 进行注册
///     onlyExceptions.Subscribe(i => Debug.Log($"Subscriber 2:{i}"), ex => Debug.Log($"Subscriber 2 exception: {ex.Message}"));
///     subject.OnNext(123);
///     subject.OnError(new Exception("Test Exception"));
///     ---
///     


///22、IgnoreElements 操作符：
/// 1、不发射任何数据，只发射 Observable 的终?通知
/// 2、IgnoreElements 操作符抑制原始 Observable 发射的所有数据，只允许它的终?通知（onError 或 onCompleted）通过
/// 3、如果你不关??个 Observable 发射的数据，但是希望在它完成时或遇到错误终?时收到通知，你可以对 Observable 使? ignoreElements 操作符，它会确保永远不会调?观察者的 onNext() ?法
/// 4、示例代码：
///     ---
///     Subject<int> subject = new Subject<int>();
///     IObservable<int> noElements = subject.IgnoreElements();
///     
///     subject.Subscribe(i => Debug.Log($"subject.OnNext({i})"), () => Debug.Log("subject.OnCompleted()"));
///     noElements.Subscribe(i => Debug.Log($"noElements.OnNext({i})"), () => Debug.Log("noElements.OnCompleted()"));
///     
///     subject.OnNext(1);
///     subject.OnNext(2);
///     subject.OnNext(3);
///     subject.OnCompleted();
///     ---
///     


///23、DistinctUntilChanged 操作符：
/// 1、DistinctUntilChanged 操作符是常用操作相关操作符
/// 2、示例代码：
///     ---
///     Subject<int> subject = new Subject<int>();
///     IObservable<int> distinct = subject.DistinctUntilChanged();// subject 的值发生改变时发送数据的事件流
///     
///     subject.Subscribe(i => Debug.Log($"{i}"), () => Debug.Log("subject.OnCompleted()"));
///     distinct.Subscribe(i => Debug.Log($"distinct.OnNext({i})"), () => Debug.Log("distinct.OnCompleted()"));
///     
///     subject.OnNext(1);
///     subject.OnNext(2);
///     subject.OnNext(3);
///     subject.OnNext(1);
///     subject.OnNext(1);
///     subject.OnNext(4);
///     subject.OnCompleted();
///     ---
///     


///24、Create 操作符：
/// １、使??个函数从头开始创建?个 Observable
/// ２、可以使? Create 操作符从头开始创建?个 Observable，给这个操作符传递?个接受观察者作为参数的函数，编写这个函数让它的?为表现为?个 Observable --恰当的调?观察者的 onNext，onError 和 onCompleted?法
/// ３、?个形式正确的有限 Observable 必须尝试调?观察者的 onCompleted 正好?次或者它的 onError 正好?次，?且此后不能再调?观察者的任何其它?法。
/// 4、Create 操作符属于创造相关的操作符
/// 5、示例代码：
///     ---
///     Observable.Create<int>(observable =>
///     {
///         observable.OnNext(1);
///         observable.OnNext(2);
///         observable.OnCompleted();
///         return Disposable.Create(() => Debug.Log("观察者已取消订阅"));
///     }).Subscribe(number => Debug.Log(number)).AddTo(this);
///     ---
///     


///25、Amb 操作符：
/// 1、给定两个或多个 Observable，它只发射最先发射数据或通知的那个 Observable 的所有数据
/// 2、传递多个 Observable 给 Amb 时，它只发射其中?个 Observable 的数据和通知：最先发送通知给 Amb 的那个，不管发射的是?项数据还是?个 onError 或 onCompleted 通知，Amb 将忽略和丢弃其它所有 Observables 的发射物
/// 3、Amb 操作符是与并发操作相关的操作符
/// 4、Amb 代码示例：
///     ---
///     Observable.Amb(
///     Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "3 sec"),
///         Observable.Timer(TimeSpan.FromSeconds(10)).Select(_ => "10 sec"),
///         Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "2 sec"),// 只发射最先发射数据(或通知)的 Observable 的所有数据
///         Observable.Timer(TimeSpan.FromSeconds(22)).Select(_ => "30 sec"),
///         Observable.Timer(TimeSpan.FromSeconds(6)).Select(_ => "5 sec"))
///             .Subscribe(s => Debug.Log($"OnNext:{s}"), () => Debug.Log("OnCompleted"))
///             .AddTo(this);
///     ---
///     


///26、Timeout 操作符：
/// 1、是原始 Observable 的?个镜像，如果过了?个指定的时?仍没有发射数据，它会发?个错误通知
/// 2、如果原始 Observable 过了指定的?段时?没有发射任何数据，Timeout 操作符会以?个 onError 通知终?这个 Observable
/// 3、Timeout 操作符是与时间操作相关的操作符
/// 4、Timeout 示例代码：
///     ---
///     // 运?结果：当?秒内没做任何操作，会报异常
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(1))
///         .Take(10)
///         .Timeout(TimeSpan.FromSeconds(1.0))
///         .Subscribe(_ => Debug.Log("clicked"))
///         .AddTo(this);
///     
///     //用于等待网络：
///     ObservableWWW.Get("http://sikiedu.com")
///         .Timeout(TimeSpan.FromSeconds(5.0f))
///         .Subscribe(_ => { }, e => Debug.Log($"发生异常:{e.Message}"))
///         .AddTo(this);
///     ---
///     


///27、FromEvent 操作符：
/// 1、将其它种类的对象和数据类型转换为 Observable
/// 2、FromEvent 操作符是与事件操作相关的操作符
/// 3、代码示例：
///     ---
///     // 输出结果：当每次点击时，输出 button clicked
///     GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(() => OnClickEvent());
///     Observable.FromEvent(action => OnClickEvent += action, action => OnClickEvent -= action)
///         .Subscribe(_ => Debug.Log("button clicked"))
///         .AddTo(this);
///     ---
///     


///28、Publish 操作符：
/// 1、将普通的 Observable 转换为可连接的 Observable
/// 2、可连接的 Observable (connectable Observable)与普通的 Observable 差不多，不过它并不会在被订阅时开始发射数据，?是直到使?了 Connect 操作符时才会开始发射数据。使用这种?法，你可以在任何时候让?个 Observable 开始发射数据。
/// 3、有?个变体接受?个函数作为参数。这个函数?原始 Observable 发射的数据作为参数，产??个新的数据作为 ConnectableObservable 给发射，替换原位置的数据项。实质是在签名的基础上添加?个 Select 操作。
/// 4、Publish 代码示例：
///     ---
///     IObservable<int> unshared = Observable.Range(1, 4);
///     
///     // 每个订阅启动一个新队列
///     unshared.Subscribe(i => Debug.Log($"Unshared Subscription #1:{i}"));// 1 2 3 4 都会立即发射数据
///     unshared.Subscribe(i => Debug.Log($"Unshared Subscription #2:{i}"));// 1 2 3 4 都会立即发射数据
///     
///     // 通过使用 Publish 操作符，订阅变成共享的，但在调用 Connect() 之前不会开始队列
///     var shared = unshared.Publish();
///     shared.Subscribe(i => Debug.Log($"shared Subscription #1:{i}"));
///     shared.Subscribe(i => Debug.Log($"shared Subscription #2:{i}"));
///     shared.Connect();// 在 Connect 的时候，才会发射数据
///     ---
/// 5、Publish 操作符是与绑定操作相关的操作符


///29、RefCount 操作符：
/// 1、让?个可连接的 Observable ?为像普通的 Observable：
/// 2、可连接的 Observable (connectable Observable) 与普通的 Observable 差不多，不过它并不会在被订阅时开始发射数据，?是直到使?了 Connect 操作符时才会开始发射数据。?这种?法，你可以在任何时候让?个 Observable 开始发射数据。
/// 3、RefCount 操作符把从?个可连接的 Observable 连接和断开的过程?动化了。它操作?个可连接的 Observable，返回?个普通的 Observable。当第?个订阅者订阅这个 Observable 时，RefCount 连接到下层的可连接 Observable。RefCount 跟踪有多少个观察者订阅它，直到最后?个观察者完成才断开与下层可连接 Observable 的连接
/// 4、RefCount 操作符是与绑定操作相关的操作符
/// 5、RefCount 示例代码：
///     ---
///     TimeSpan period = TimeSpan.FromSeconds(1);
///     
///     IObservable<long> observable = Observable
///         .Interval(period).Do(l => Debug.Log($"Publishing:{l}"))
///         .Publish()
///         .RefCount();
///     
///     Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ =>
///     {
///         Debug.Log("@@@@ subscribe @@@@");
///         // Observable.Connect();// 改用 RefCount
///         IDisposable subscription = observable.Subscribe(i => Debug.Log($"subscription:{i}"));
///         Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(__ =>
///         {
///             Debug.Log("@@@@ unSubscribe @@@@");
///             subscription.Dispose();// 清除之前订阅的事件，然后下面订阅新的事件
///             observable.Subscribe(i => Debug.Log($"subscription 2: {i}"));
///         });
///     });
///     
///     // 除此之外还有一个 Share 操作符：与 Publish().RefCount() 一样
///     /*public static IObservable<T> Share<T>(this IObservable<T> source)
///     {
///         return source.Publish().RefCount();
///     }*/
///     ---
///     


///30、Replay 操作符：
/// １、保证所有的观察者收到相同的数据序列，即使它们在　Observable　开始发射数据之后才订阅
/// ２、可连接的 Observable (connectable Observable)与普通的 Observable 差不多，不过它并不会在被订阅时开始发射数据，?是直到使?了 Connect 操作符时才会开始。?这种?法，你可以在任何时候让?个　Observable 开始发射数据。
/// 3、如果在将?个 Observable 转换为可连接的 Observable 之前对它使? Replay 操作符，产?的这个可连接 Observable 将总是发射完整的数据序列给任何未来的观察者，即使那些观察者在这个 Observable 开始给其它观察者发射数据之后才订阅。
/// 4、有?种 replay 返回?个普通的 Observable，它可以接受?个变换函数为参数，这个函数接受原始 Observable 发射的数据项为参数，返回结果 Observable 要发射的?项数据。因此，这个操作符其实是 replay 变换之后的数据项。
/// 5、Replay 操作符是与绑定操作相关的操作符
/// 6、Replay 示例代码：
///     ---
///     IConnectableObservable<long> replayObservable = Observable.Interval(TimeSpan.FromSeconds(1.0f))
///                                             .Do(l => Debug.Log($"Observable:{l}"))//立即执行
///                                             .Replay();// 以重播方式发射数据
///     
///     replayObservable.Subscribe(l => Debug.Log($"Subscription #1:{l}"));// 接收到数据后执行回调
///     replayObservable.Connect();
///     
///     Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => replayObservable.Subscribe(l => Debug.Log($"Subscription #2:{l}")));
///     


///31、Connect 操作符：
/// 1、让?个可连接的 Observable 开始发射数据给订阅者
/// 2、可连接的 Observable (connectable Observable) 与普通的 Observable 差不多，不过它并不会在被订阅时开始发射数据，?是直到使?了 Connect 操作符时才会开始。?这个?法，你可以等待所有的观察者都订阅了 Observable 之后再开始发射数据。
/// 3、Connect 操作符是与绑定操作相关的操作符
/// 


///32、Throw 操作符：
/// １、创建?个不发射数据以?个错误终?的 Observable
/// ２、示例代码：
///     ---
///     //输出结果: 发现异常: error
///     Observable.Throw<string>(new Exception("error")).Subscribe(_ => Debug.Log("不会输出"), e => Debug.Log($"发现异常：{e.Message}"));
///     ---
///     


///33、Catch 操作符：
/// 1、从 onError 通知中恢复发射数据
/// 2、Catch 操作符拦截原始 Observable 的 onError 通知，将它替换为其它的数据项或数据序列，让产?的 Observable 能够正常终?或者根本不终?，在某些 ReactiveX 的实现中，有?个叫 onErrorResumeNext 的操作符，它的?为与 Catch 相似。
/// 3、示例代码：
///     ---
///     // 输出结果为: catched exception:error，1 秒后输出 timer called
///     Observable.Throw<string>(new Exception("error"))
///         .Catch<string, Exception>(e =>
///         {
///             Debug.Log($"catched exception:{e.Message}");
///             return Observable.Timer(TimeSpan.FromSeconds(1.0f)).Select(_ => "timer called");
///         })
///         .Subscribe(result => Debug.Log(result));
///     ---
/// 4、Catch 操作符是与异常处理操作相关的操作符


///34、Finally 操作符：
/// 1、注册?个动作，当它产?的 Observable 终?之后会被调?，?论是正常终?还是异常终?
/// 2、Finally 操作符是与异常处理操作相关的操作符
/// 3、 示例代码：
///     ---
///     Subject<int> source = new Subject<int>();
///     IObservable<int> result = source.Finally(() => Debug.Log("Finally action ran"));// result 是 source 终止时的事件源
///     
///     result.Subscribe(number => Debug.Log($"OnNext({number})"), () => Debug.Log("OnCompleted()"));// 订阅 result，当 result 发射通知，执行回调
///     source.OnNext(1);// 发布通知
///     source.OnNext(2);
///     source.OnNext(3);
///     //source.OnError(new Exception());// 异常终止
///     source.OnCompleted();// 正常终止
///     ---
///     


///35、DelaySubscription 操作符：
/// 1、示例代码：
///     ---
///     Debug.Log(Time.time);
///     Observable.ReturnUnit().DelaySubscription(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log(Time.time));
///     ---
///     


///36、PairWise 操作符：
/// 1、Pair Wise：成双成对
/// 2、示例代码：
///     ---
///     // Range(X,Y) 是左闭右开的
///     // PairWise 操作符在发射第一个数据的时候订阅的回调不会执行，只有发生超过两个数据可以成双成对时回调才会正常执行
///     Observable.Range(0, 10).Pairwise().Subscribe(pair => Debug.Log($"{pair.Previous} : {pair.Current}"));
///     ---
///     


///37、小结：
/// 1、在本章学习了 UniRx 中原属于 Rx.Net 的部分操作符，这些操作符?部分是?常实?的，少部分不太常?的部分都放在较后的位置讲，很多操作符理解起来晦涩难懂，不过??去动?练习?遍就?概明?了
/// 2、在下?章中开始学习 UniRx 独有的操作符，这部分操作符每个都?常实?，需要认真学习
#endregion



#region 3、UniRx 独有的操作符：

///1、
///


///2、
///


///3、
///


///4、BatchFrame 操作符：
/// 1、Batch 是 “一批”、“分批处理”的意思，BatchFrame 就是进行多帧的批处理
/// 2、示例代码：
///     ---
///     // 运?结果：收集每 100 帧内的点击事件，然后进?统?的输出
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .BatchFrame(100, FrameCountType.EndOfFrame)
///         .Subscribe(clicks => Debug.Log(clicks.Count));
///         


///5、
///


///6、
///


///7、SampleFrame 操作符：
/// 1、Sample 是“采样”的意思，即隔多少帧取一个数据的意思
/// 2、代码示例：
///     ---
///     //Observable.EveryUpdate().SampleFrame(5).Subscribe(_ => Debug.Log(Time.frameCount));
///     // 以前的优化手段：每隔 5 帧调用一次 GC.Collect()
///     Observable.EveryUpdate().SampleFrame(5).Subscribe(_ => GC.Collect());
///


///8、
///
#endregion
public class OperatorNote : MonoBehaviour
{
    #region 作练习用
    class Student
    {
        public string name;
        public int age;
    }

    event Action OnClickEvent;
    #endregion

    private void Start()
    {
        #region 作练习用
        List<Student> students = new List<Student>()
        {
            new Student(){name = "张三",age = 50},
            new Student(){name = "李四",age = 51},
            new Student(){name = "王五",age = 52},
            new Student(){name = "王五",age = 59},
        };
        int[] grades = { 35, 46, 78, 98, 65, 87, 67, 45, 79 };
        string[] fruits = { "apple", "banana", "mango", "orange", "passionfruit", "grape" };
        #endregion


        #region 1、UniRx 从 Linq 中借鉴而来的操作符：
        #region 1、Linq 与 UniRx 操作符：
        /*List<int> testList = new List<int> { 1, 2, 3 };
        testList.ForEach(num => Debug.Log(num));// 1 2 3

        int firstNumber = testList.First();
        Debug.Log(firstNumber); // 1
        testList.Where(num => num > 1).ToList().ForEach(number => Debug.Log(number));// 2 3*/
        #endregion


        #region 2、之前所学的 UniRx 操作符总结：(无代码)

        #endregion


        #region 3、Where 操作符：
        /*//IEnumerable<Student> oldStudents = students.Where(student => student.age > 45);
        IEnumerable<Student> oldStudents = from oldStudent in students where oldStudent.age > 45 select oldStudent;
        foreach(var student in oldStudents)
        {
            Debug.Log(student.name);
        }

        //?标点击事件监听：
        //Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => { Debug.Log("鼠标点击"); }).AddTo(this);
        //UniRx Where 查询式：
        IObservable<long> mouseClickEventStreams = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonDown(0) select updateEvent;
        mouseClickEventStreams.Subscribe(_ => { Debug.Log("mouse clicked"); }).AddTo(this);*/
        #endregion


        #region 4、Select 操作符：
        /*
        //Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0)).Select(_ => "mouse up").Subscribe(Debug.Log).AddTo(this);
        //var mouseUpEventSteam = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonUp(0) select "mouse up";
        //mouseUpEventSteam.Subscribe(Debug.Log).AddTo(this);
        */
        #endregion


        #region 5、First 操作符
        /*// 直接获取第?个元素:
        //Student oldStudent = students.Where(student => student.age > 45).First();

        // 取序列中满?条件的第?个元素:
        //Student oldStudent = students.First(student => student.age > 45);

        // LINQ First 查询式:
        Student oldStudent = (from student in students select student).First(student => student.age > 45);

        // UniRx First 示例代码:
        Observable.EveryUpdate().First(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("mouse down")).AddTo(this);
        Debug.Log(oldStudent.name);*/
        #endregion


        #region 6、Distinct 操作符：
        /*var names = new List<string>()
        {
            "张三",
            "张三",
            "李四"
        };
        // LINQ Distinct 示例代码：
        //IEnumerable<string> distinctNames = names.Distinct();

        // LINQ Distinct 查询式示例代码：
        IEnumerable<string> distinctNames = (from name in names select name).Distinct();
        foreach (var distinctName in distinctNames)
            Debug.Log(distinctName);

        // UniRx Distinct 示例代码：
        students.ToObservable().Distinct(student => student.name).Subscribe(student => Debug.Log(student.name)).AddTo(this);

        // UniRx 的 Distinct 在操作 List 的时候，除了?持 LINQ 所?持的两种?式外，还?持传??个特定条件函数：
        IObservable<string> lectClickStream = Observable.EveryUpdate()
                                                .Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "left clicked");
        IObservable<string> rightClickStream = Observable.EveryUpdate()
                                                .Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "right clicked");
        Observable.Merge(lectClickStream, rightClickStream).Distinct().Subscribe(Debug.Log).AddTo(this);

        //以上代码结果为:不管点击多少次?标左键还是?标右键，输出只输出?次 “left clicked” 和 “right clicked”*/
        #endregion


        #region 7、Last 操作符：
        /*// LINQ Last 代码示例:
        *//*Student student = students.Last();
        Debug.Log(student.name);*//*

        // UniRx Last 代码示例：
        students.ToObservable().Last(student => student.age == 50).Subscribe(student => Debug.Log(student.name)).AddTo(this);
        */
        #endregion


        #region 8、SelectMany 操作符：
        // LINQ SelectMany 示例代码：
        /*IEnumerable singleChars = students.SelectMany(student => student.name + ":" + student.age);
        foreach(object singleChar in singleChars)
        {
            Debug.Log(singleChar);
        }*/

        // SelectMany 的作用：假设一个 List<string> 集合 nums 有“123”、“456”、“789” 三个字符串
        // 通过 nums.SelectMany 之后 (nums.SelectMany(n => n));
        // 得到一个新集合{'1','2','3','4','5','6','7','8','9'}
        // 为什么 string 字符串可以被打散成字符？因为 string 继承了 IEnumerable 接口，是可迭代的
        // 返回的 n 可以进行操作：nums.SelectMany(n => n + ":");得到一个新集合{'1',':','2',':','3',':','4',':','5',':'……}，每遍历一个 n，返回 n 本身和一个 ':'
        // 或者直接返回其它字符串：nums.SelectMany(n => "asd");得到一个新集合{'a','s','d','a','s','d','a','s','d'……},每遍历一个 n，返回三个字符 'a','s','d'

        // UniRx SelectMany 代码示例：
        /*IEnumerator A()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("A");
        }
        
        IEnumerator B()
        {
            yield return new WaitForSeconds(2.0f);
            Debug.Log("B");
        }
        
        IEnumerator C()
        {
            yield return new WaitForSeconds(3.0f);
            Debug.Log("C");
        }

        var streamA = Observable.FromCoroutine(A);
        var streamB = Observable.FromCoroutine(B);
        var streamC = Observable.FromCoroutine(C);
        streamA.SelectMany(streamB.SelectMany(streamC)).Subscribe(_ => Debug.Log("Hello"));*/
        #endregion


        #region 9、Take 操作符：
        // LINQ Take 代码示例：
        /*int[] grades = { 59, 84, 69, 30, 59, 94, 83 };
        IEnumerable<int> topThreeGrades = grades.OrderByDescending(grade => grade).Take(3);//降序排序后取前三个
        foreach (int topThreeGrade in topThreeGrades)
            Debug.Log(topThreeGrade);*/

        // UniRx Take 示例代码：
        //this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Take(5).Subscribe(_ => Debug.Log(1));
        // 输出结果为，只有前 5 次?标点击才会输出 1

        /*// Take 可以传递一个时间段进来
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// 当点击鼠标左键
            .Take(TimeSpan.FromSeconds(5))// 只在前五秒内处理，超过五秒无效
            .Subscribe(_ => Debug.Log("mouse clicked"))
            .AddTo(this);*/
        #endregion


        #region 10、Concat 操作符：
        //LINQ Concat 示例代码：
        /*List<Student> students2 = new List<Student>()
        {
            new Student(){name = "ddd", age = 20 },
            new Student(){name = "ccc", age = 60 },
            new Student(){name = "aaa", age = 30 },
        };
        var studentNames = students.Select(student => student.name).Concat(students2.Select(student => student.name));
        foreach (string studentName in studentNames)
            Debug.Log(studentName);*/

        // UniRx Concat 示例代码：
        /*IObservable<string> a = this.UpdateAsObservable().Take(3).Select(_ => "A");
        IObservable<string> b = this.UpdateAsObservable().Take(2).Select(_ => "B");
        IObservable<string> c = a.Concat(b);
        c.Subscribe(Debug.Log);*/
        #endregion


        #region 11、WhenAll:
        /*// LINQ All 示例代码：
        bool allStartWithB = students.All(student => student.name.StartsWith("B"));
        Debug.LogFormat("{0} student names start with 'B'.", allStartWithB ? "All" : "Not All");

        // UniRx WhenAll 示例代码(输出结果为A、B、C，输出顺序不?定)：
        IEnumerator A()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("A");
        }
        IEnumerator B()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("B");
        }
        IEnumerator C()
        {
            yield return new WaitForSeconds(1.0f);
            Debug.Log("C");
        }
        IObservable<Unit> streamA = Observable.FromCoroutine(A);
        IObservable<Unit> streamB = Observable.FromCoroutine(B);
        IObservable<Unit> streamC = Observable.FromCoroutine(C);
        Observable.WhenAll(streamA, streamB, streamC).Subscribe(_ => Debug.Log("Completed"));*/
        #endregion


        #region 12、OfType 操作符：
        // LINQ OfType 代码示例:
        /*ArrayList list = new ArrayList { 30, 30.0f, "test" };
        list.OfType<float>().ToList().ForEach(num => Debug.Log(num));*/

        // UniRx OfType 示例代码：
        // 创建一个 Subject(Observable)
        /*Subject<object> objects = new Subject<object>();

        // 订阅改 Observable，进行类型过滤(发布通知)
        objects.OfType<object, string>().Subscribe(Debug.Log);//从 object 类型转为 string 类型
        objects.OnNext(1);
        objects.OnNext(2);
        objects.OnNext("3");
        objects.OnNext(5);

        // 手动结束广播
        objects.OnCompleted();*/
        #endregion


        #region 13、Cast 操作符：
        // LINQ Cast 代码示例：
        /*ArrayList fruits = new ArrayList { "mango", "apple", "lemon" };
        IEnumerable fruitNames = fruits.Cast<string>();
        foreach (var fruit in fruitNames)
            Debug.Log(fruit);*/

        // UniRx Cast 代码示例：
        /*Subject<object> objects = new Subject<object>();
        objects.Cast<object, int>().Subscribe(i => Debug.Log(i));//将 object 转为 int 类型

        // 手动发送数据(广播)
        objects.OnNext(1);
        objects.OnNext(2);
        objects.OnNext(3);
        //objects.OnNext("4");//报错，转换异常

        // 手动结束广播
        objects.OnCompleted();*/

        #endregion


        #region 14、GroupBy 操作符：
        // LINQ GroupBy 代码示例：
        /*IEnumerable<IGrouping<string,Student>> studentGroupNames = students.GroupBy(student => student.name);
        foreach(IGrouping<string, Student> studentGroup in studentGroupNames)
        {
            Debug.Log($"Group Key:{studentGroup.Key}");
            foreach(Student student in studentGroup)
            {
                Debug.Log($"Name:{student.name} Age:{student.age}");
            }
        }*/

        // LINQ GroupBy 查询表达式：
        /*Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10).GroupBy(i => i % 3).Subscribe(group =>
        {
            group.Subscribe(number => Debug.Log($"key:{group.Key},number:{number}"));
        });*/
        #endregion


        #region 15、Range 操作符：
        // LINQ Range 代码示例：
        /*IEnumerable<int> squares = Enumerable.Range(5, 5).Select(x => x * x);//从 5 开始往上涨的三个数 5 6 7
        squares.ToList().ForEach(num => Debug.Log(num));*/

        // UnIRx Range 代码示例：
        /*IObservable<int> squares = Observable.Range(5, 3).Select(x => x * x);
        squares.Subscribe(square => Debug.Log(square));*/
        #endregion


        #region 16、Skip 操作符：
        //LINQ Skip 代码示例：
        /*int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };
        grades.OrderByDescending(g => g).Skip(3).ToList().ForEach(num => Debug.Log(num));//降序排序后跳过前三名

        //UniRx Skip 代码示例：
        this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Skip(5).Subscribe(_ => Debug.Log("mouse clicked"));
        //输出结果为，?标点击第六次时，才开始输出”mouse clicked”*/

        // Skip 操作符可以传递一个时间段过来：
        /*Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// 当点击鼠标左键
            .Skip(TimeSpan.FromSeconds(5))// 跳过前五秒（前五秒内点击都无效，五秒后每次点击都有效）
            .Subscribe(_ => Debug.Log("mouse clicked"))
            .AddTo(this);*/
        #endregion


        #region 17、TakeWhile 操作符：
        // LINQ TakeWhile 示例代码：
        //string[] fruits = new[] { "apple", "banana", "mango", "orange", "grape", "passionfruit" };
        //IEnumerable<string> fruitsAfterOrange = fruits.TakeWhile(fruit => fruit != "orange");
        //fruitsAfterOrange.ToList().ForEach(value => Debug.Log(value));

        //UniRx TakeWhile 代码示例：
        //this.UpdateAsObservable().TakeWhile(value => !Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("before click"));
        /*Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButton(0))// 鼠标按下不动
            .TakeWhile((_, number) => !Input.GetMouseButtonUp(0) && number < 100)//当鼠标没有抬起并且次数小于 100,number 表示事件触发次数
            .Subscribe(_ => Debug.Log("mouse button clicking"));*/
        //运?结果为，持续输出 before mouse clicked，当?标点击之后不再输出 “before mouse clicked”
        #endregion


        #region 18、SkipWhile 操作符：
        /*//LINQ SkipWhile 代码示例：
        int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };

        // 降序排序，如果当前成绩大于等于 80，则跳过该值，如果当前成绩小于 80，返回剩余的值
        IEnumerable<int> lowerGrades = grades.OrderByDescending(grade => grade).SkipWhile(grade => grade >= 80);
        lowerGrades.ToList().ForEach(value => Debug.Log(value));*/

        // UniRx SkipWhile 代码示例：
        /*this.UpdateAsObservable()
            .SkipWhile(_ => !Input.GetMouseButtonDown(0))// 当鼠标按下时，收集之后的事件
            .Subscribe(_ => Debug.Log("mouse button down"));
        // 实现的逻辑为，当点击?标后，持续输出 mouse button down*/
        #endregion


        #region 19、Zip (.Net 4) 操作符：
        // LINQ Zip 示例：
        /*int[] numbers = { 1, 2, 3, 4 };
        string[] words = { "one", "two", "three", "four" };
        IEnumerable<string> numbersAndWords = numbers.Zip(words, (first, second) => $"{first} {second}");
        // 输出 1 one，2 two……
        numbersAndWords.ToList().ForEach(value => Debug.Log(value));*/

        // UniRx Zip 示例代码：
        /*IObservable<Unit> rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
        IObservable<Unit> leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
        leftStream.Zip(rightStream, (left, right) => Unit.Default).Subscribe(_ => Debug.Log("ok"));*/
        //只要左右键成对就会输出 ok，先按两次左键，再按两次右键，输出两次 ok
        #endregion


        #region 20、Repeat 操作符：
        // LINQ Repeat 代码示例：
        /*IEnumerable<string> strs = Enumerable.Repeat("I like programming.", 5);
        foreach (string str in strs)
            Debug.Log(str);*/

        // UniRx Repeat 代码示例：
        // 不使用 Repeat，一秒后输出一次字符串，之后不再输出
        //Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log("after 1 seconds"));

        // 使用 Repeat，每秒输出一次字符串(让事件流重复执行)
        //Observable.Timer(TimeSpan.FromSeconds(1.0f)).Repeat().Subscribe(_ => Debug.Log("after 1 seconds"));
        #endregion


        #region 21、TakeLast 操作符：
        // UniRx TakeLast 代码示例：
        /*IObservable<int> bottonThreeGrades = grades.OrderByDescending(grade => grade).ToObservable().TakeLast(3);

        // 订阅 bottonThreeGrades 事件，当有某个值满足 bottonThreeGrades 事件，执行回调方法（效果类似于 foreach）
        bottonThreeGrades.Subscribe(buttonThreeGrade => Debug.Log(buttonThreeGrade));*/

        // 传递时间段进 TakeLast 操作符
        /*Subject<float> subject = new Subject<float>();

        subject.TakeLast(TimeSpan.FromSeconds(2.0))// 收集 subject 最后两秒的内容，执行回调
            .Subscribe(clickTime => Debug.Log(clickTime));

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// 点击鼠标左键，执行回调方法
            .Subscribe(_ => subject.OnNext(Time.time));// 广播当前事件
        Observable.Timer(TimeSpan.FromSeconds(5.0f))// 五秒后触发事件，执行回调方法
            .Subscribe(_ => subject.OnCompleted());// 五秒后停止 subject 的广播*/
        #endregion


        #region 22、Single/Default 操作符：
        // LINQ Single 代码示例：
        /*string fruit1 = fruits.Single(fruit => fruit.Length > 10);
        Debug.Log(fruit1);*/

        //fruits.ToObservable().Single(fruit => fruit.Length > 10).Subscribe(Debug.Log);
        #endregion


        #region 23、ToArray 操作符：
        // LINQ ToArray 示例代码：
        /*string[] studentNames = students.Select(student => student.name).ToArray();
        foreach (var name in studentNames)
            Debug.Log(name);*/

        // UniRx ToArray 示例代码：
        /*Subject<int> subject = new Subject<int>();
        subject.ToArray().Subscribe(intArray =>
        {
            Debug.Log(intArray.GetType().ToString());
            foreach (int i in intArray)
                Debug.Log(i);
        });
        subject.OnNext(1);
        subject.OnNext(2);
        subject.OnNext(3);
        subject.OnCompleted();*/
        #endregion


        #region 24、ToList 操作符：
        // LINQ ToList 示例代码：
        /*List<int> lengths = fruits.Select(fruit => fruit.Length).ToList();
        lengths.ForEach(value => Debug.Log(value));*/

        // UniRx ToList 示例代码：
        /*Subject<int> subject = new Subject<int>();
        subject.ToList().Subscribe(intList =>
        {
            Debug.Log(intList.GetType().ToString());
            foreach (int i in intList)
                Debug.Log(i);
        });
        subject.OnNext(1);
        subject.OnNext(2);
        subject.OnNext(3);
        subject.OnCompleted();*/
        #endregion


        #region 25、Aggregate 操作符：
        // LINQ Aggregate 代码示例：
        /*int[] numbers = new[] { 1, 2, 3, 4, 5 };
        int result = numbers.Aggregate((total, next) => total * next);
        Debug.Log($"5 的阶乘为：{result}");// 返回 120，即 1 * 2 * 3 * 4 * 5*/

        // UniRx Aggregate 代码示例：
        //Observable.Range(0, 8).Aggregate(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));

        /*// Aggregate 可用于取最大值和最小值操作
        int[] numbers = new[] { 1, 2, 3, 4, 5 };
        int result = numbers.Aggregate((total, next) => total = next > total ? next : total);
        Debug.Log($"最大值为{result}");*/
        #endregion


        #region 26、Empty 操作符
        // LINQ Empty 示例代码：
        /*string[] names1 = { "Hartono, Tommy" };
        string[] names2 = { "Adams, Terry", "Andersen, Henriette Thaulow", "Hedlund, Magnus", "Ito, Shu" };
        string[] names3 = { "Solanki, Ajay", "Hoeing, Helge", "Andersen, Henriette Thaulow", "Potra, Cristina", "Iallo, Lucio" };
        var namesList = new List<string[]> { names1, names2, names3 };
        var allNames = namesList.Aggregate(Enumerable.Empty<string>(),(current, next) => next.Length > 3 ? current.Union(next) : current);
        foreach (var name in allNames)
        {
            Debug.Log(name);
        }*/

        // UniRx Empty 代码示例：
        /*IObservable<Unit> s = Observable.Empty<Unit>();
        s.Subscribe(e => Debug.Log($"e:{e}"), () => Debug.Log("OnCompleted"));*/
        #endregion


        #region 27、小结：(无代码)

        #endregion
        #endregion


        #region 2、UniRx 从 Rx.Net 中借鉴的操作符(Rx 独有的操作符)：
        #region 1、Interval 操作符：
        // 每隔 1 秒触发一次事件
        //Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(second => Debug.Log($"当前时间：{second} s")).AddTo(this);

        // 以上效果也可使用 Repeat 实现
        //Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe()).AddTo(this);
        #endregion


        #region 2、TakeUntil 操作符：
        // 输出结果为：持续输出 123，当点击?标左键后，停?输出 123
        /*this.UpdateAsObservable()//第一个 Observable
            .TakeUntil(Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)))// 括号内是第二个 Observable
            .Subscribe(_ => Debug.Log(123))
            .AddTo(this);*/
        #endregion


        #region 3、SkipUntil 操作符：
        // 输出结果为：点击?标左键之后就开始持续输出 “?标按过了”
        /*IObservable<Unit> clickStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
        this.UpdateAsObservable()
            .SkipUntil(clickStream)// 在 clickStream 发射数据之前，跳过当前事件
            .Subscribe(_ => Debug.Log("鼠标按过了"))
            .AddTo(this);*/
        #endregion


        #region 4、Buffer 操作符(缓冲)：
        /*Observable.Interval(TimeSpan.FromSeconds(1.0f))// 每隔一秒发射一次数据
            .Buffer(TimeSpan.FromSeconds(3.0f))// 缓存 3 秒内的数据
            .Subscribe(_ => Debug.Log($"currentTime:{DateTime.Now.Second}"))// 缓存数据后执行回调方法
            .AddTo(this);*/
        #endregion


        #region 5、Throttle(节流阀)操作符：
        /*//输出结果：点击?标后 1 秒内不再点击则输出，有点击则重新计时 1 秒再输出
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// 当点击鼠标时
            .Throttle(TimeSpan.FromSeconds(1))//一秒后不再点击则执行回调，否则重新计时一秒，直到重新计时开始一秒后不再点击，执行回调
            .Subscribe(_ => Debug.Log("一秒过后"));*/
        #endregion


        #region 6、Delay 操作符：
        /*// 输出结果：点击?标 1 秒之后输出 mouse clicked，每次点击事件都是 1 秒之后才输出
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Delay(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => Debug.Log("mouse clicked"))
            .AddTo(this);*/
        #endregion


        #region 7、Return 操作符：
        /*Observable.Return("hello").Subscribe(Debug.Log);//只执行一次，类似 set*/
        #endregion


        #region 8、Timer 操作符：
        // 执?结果：在 5 秒后输出 “after 5 seconds”
        //Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => Debug.Log("after 5 seconds")).AddTo(this);
        /*Observable
            .Timer(TimeSpan.FromSeconds(1.0f),TimeSpan.FromSeconds(2.0f))// 在一秒之后，每隔两秒发射一次数据
            .Subscribe(_ => Debug.Log("after 5 seconds"))
            .AddTo(this);*/
        #endregion


        #region 9、Sample 操作符：
        /*Observable.Interval(TimeSpan.FromMilliseconds(50))//每隔 50 毫秒发射一次数据
            .Sample(TimeSpan.FromSeconds(1))// 每隔一秒进行一次采样，如果当前采样的数据在之前已经采过样了，那么就不再进行采样，直到采样到的数据是之前每采样过的，才进行采样
            .Subscribe(_ => Debug.Log(DateTime.Now.Second))
            .AddTo(this);

        int clickCount = 0;
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => clickCount++)
            .Sample(TimeSpan.FromSeconds(3))
            .Subscribe(_ => Debug.Log($"第 {clickCount} 次点击"))
            .AddTo(this);*/
        #endregion


        #region 10、Timestamp 操作符：
        /*Observable.Interval(TimeSpan.FromSeconds(1.0f))
            .Timestamp()
            //.Subscribe(timestamp => Debug.Log(timestamp))
            .Subscribe(timestamp => Debug.Log(timestamp.Timestamp.LocalDateTime))//输出当地时间
            .AddTo(this);*/
        #endregion


        #region 11、ThrottleFirst 操作符：
        //运?结果：?标点击之后，?即输出 “Clicked”，输出之后的 5 秒内点击?效
        /*this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .ThrottleFirst(TimeSpan.FromSeconds(5))
            .Subscribe(x => Debug.Log("Clicked"))
            .AddTo(this);*/
        #endregion


        #region 12、TimeInterval 操作符：
        /*Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => "clicked")
            .TimeInterval()
            .Subscribe(timeInterval => Debug.Log($"{timeInterval.Interval} : {timeInterval.Value}"))
            .AddTo(this);*/
        #endregion


        #region 13、Defer 操作符：
        /*System.Random random = new System.Random();
        Observable
            .Defer(() => Observable.Start(() => random.Next()))//Observable.Start(() => random.Next()) 会开启一个线程创建一个新的 Observable，然后获得下一个随机数
            .Delay(TimeSpan.FromMilliseconds(1000))
            .Repeat()
            .Subscribe(randomNumber => Debug.Log(randomNumber))// 每次订阅时，Defer 会创建一个 Observable 
            .AddTo(this);*/
        #endregion


        #region 14、Never 操作符：
        /*//?前还不知道?嘛?，没有输出结果
        IObservable<string> never = Observable.Never<string>();
        // 类似于没有广播/通知的 Subject
        Subject<string> subject = new Subject<string>();*/
        #endregion


        #region 15、Scan 操作符：
        //Observable.Range(0, 8).Scan(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));
        #endregion


        #region 16、Switch 操作符：
        /*// 执?结果：当按下?标时输出 “mouse button down” 抬起之后输出 “mouse button up”
        var buttonDownStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
        var buttonUpStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0));
        buttonDownStream.Select(_ =>
            {
                Debug.Log("mouse button down");
                return buttonUpStream;
            })
            .Switch()// 切换事件流到 buttonUpStream
            .Subscribe(_ => Debug.Log("mouse button up"))// buttonUpStream 事件流的回调方法
            .AddTo(this);*/

        /*// 使用 Switch 操作符实现“彩蛋”功能:当按顺序按下 W A S D 时触发彩蛋
        var wObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.W));
        var aObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.A));
        var sObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.S));
        var dObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.D));

        wObservable.Select(_ => aObservable)
            .Switch()// 切换至 aObservable 事件流
            .Select(_ => sObservable)
            .Switch()// 切换至 sObservable 事件流
            .Select(_ => dObservable)
            .Switch()// 切换至 dObservable 事件流
            .Repeat()
            .Subscribe(_ => Debug.Log("触发彩蛋"))
            .AddTo(this);*/
        #endregion


        #region 17、StartWith 操作符：
        /*Observable.Return("sikiedu.com")
            .StartWith("http://")
            .Aggregate((current, next) => current + next)// 如果没有 Aggregate 操作，会先输出 "http://"，而不是拼接成一个字符串
            .Subscribe(Debug.Log)
            .AddTo(this);*/
        #endregion


        #region 18、CombineLatest 操作符：
        /*int a = 0;
        int i = 0;
        var leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => (++a).ToString());
        var rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => (++i).ToString());
        leftStream.CombineLatest(rightStream, (left, right) => left + right).Subscribe(Debug.Log);*/
        #endregion


        #region 19、Do 操作符：
        /*Observable.ReturnUnit().Delay(TimeSpan.FromSeconds(1.0f))
            .Do(_ => Debug.Log("after 1 seconds"))
            .Delay(TimeSpan.FromSeconds(1.0f))
            .Do(_ => Debug.Log("after 2 seconds"))
            .Delay(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => Debug.Log("after 3 seconds"))
            .AddTo(this);*/
        #endregion


        #region 20、Merge 操作符：
        /*// 输出结果：点击?标左键输出 “A”，点击?标右键输出 “B”
        var aStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "A");
        var bStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "B");
        aStream.Merge(bStream).Subscribe(Debug.Log);*/
        #endregion


        #region 21、Materialize/Dematerialize 操作符：
        /*Subject<int> subject = new Subject<int>();
        IObservable<int> onlyExceptions = subject.Materialize().Where(n => n.Exception != null).Dematerialize();

        // 对 subject 进行注册
        subject.Subscribe(i => Debug.Log($"Subscriber 1:{i}"), ex => Debug.Log($"Subscriber 1 exception: {ex.Message}"));

        // 对 Dematerialize 进行注册
        onlyExceptions.Subscribe(i => Debug.Log($"Subscriber 2:{i}"), ex => Debug.Log($"Subscriber 2 exception: {ex.Message}"));
        subject.OnNext(123);
        subject.OnError(new Exception("Test Exception"));*/
        #endregion


        #region 22、IgnoreElements 操作符：
        /*Subject<int> subject = new Subject<int>();
        IObservable<int> noElements = subject.IgnoreElements();

        subject.Subscribe(i => Debug.Log($"subject.OnNext({i})"), () => Debug.Log("subject.OnCompleted()"));
        noElements.Subscribe(i => Debug.Log($"noElements.OnNext({i})"), () => Debug.Log("noElements.OnCompleted()"));

        subject.OnNext(1);
        subject.OnNext(2);
        subject.OnNext(3);
        subject.OnCompleted();*/
        #endregion


        #region 23、DistinctUntilChanged 操作符：
        /*Subject<int> subject = new Subject<int>();
        IObservable<int> distinct = subject.DistinctUntilChanged();// subject 的值发生改变时发送数据的事件流

        subject.Subscribe(i => Debug.Log($"{i}"), () => Debug.Log("subject.OnCompleted()"));
        distinct.Subscribe(i => Debug.Log($"distinct.OnNext({i})"), () => Debug.Log("distinct.OnCompleted()"));

        subject.OnNext(1);
        subject.OnNext(2);
        subject.OnNext(3);
        subject.OnNext(1);
        subject.OnNext(1);
        subject.OnNext(4);
        subject.OnCompleted();*/
        #endregion


        #region 24、Create 操作符：
        /*Observable.Create<int>(observable =>
        {
            observable.OnNext(1);
            observable.OnNext(2);
            observable.OnCompleted();
            return Disposable.Create(() => Debug.Log("观察者已取消订阅"));
        }).Subscribe(number => Debug.Log(number)).AddTo(this);*/
        #endregion


        #region 25、Amb 操作符：
        /*Observable.Amb(
            Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "3 sec"),
            Observable.Timer(TimeSpan.FromSeconds(10)).Select(_ => "10 sec"),
            Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "2 sec"),// 只发射最先发射数据(或通知)的 Observable 的所有数据
            Observable.Timer(TimeSpan.FromSeconds(22)).Select(_ => "30 sec"),
            Observable.Timer(TimeSpan.FromSeconds(6)).Select(_ => "5 sec"))
            .Subscribe(s => Debug.Log($"OnNext:{s}"), () => Debug.Log("OnCompleted"))
            .AddTo(this);*/
        #endregion


        #region 26、Timeout 操作符：
        /*// 运?结果：当?秒内没做任何操作，会报异常
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(1))
            .Take(10)
            .Timeout(TimeSpan.FromSeconds(1.0))
            .Subscribe(_ => Debug.Log("clicked"))
            .AddTo(this);*/

        /*//用于等待网络：
        ObservableWWW.Get("http://sikiedu.com")
            .Timeout(TimeSpan.FromSeconds(5.0f))
            .Subscribe(_ => { }, e => Debug.Log($"发生异常:{e.Message}"))
            .AddTo(this);*/
        #endregion


        #region 27、FromEvent 操作符：
        /*// 输出结果：当每次点击时，输出 button clicked
        GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(() => OnClickEvent());
        Observable.FromEvent(action => OnClickEvent += action, action => OnClickEvent -= action)
            .Subscribe(_ => Debug.Log("button clicked"))
            .AddTo(this);*/
        #endregion


        #region 28、Publish 操作符：
        /*IObservable<int> unshared = Observable.Range(1, 4);

        // 每个订阅启动一个新队列
        unshared.Subscribe(i => Debug.Log($"Unshared Subscription #1:{i}"));// 1 2 3 4 都会立即发射数据
        unshared.Subscribe(i => Debug.Log($"Unshared Subscription #2:{i}"));// 1 2 3 4 都会立即发射数据

        // 通过使用 Publish 操作符，订阅变成共享的，但在调用 Connect() 之前不会开始队列
        var shared = unshared.Publish();
        shared.Subscribe(i => Debug.Log($"shared Subscription #1:{i}"));
        shared.Subscribe(i => Debug.Log($"shared Subscription #2:{i}"));
        shared.Connect();// 在 Connect 的时候，才会发射数据*/
        #endregion


        #region 29、RefCount 操作符：
        /*TimeSpan period = TimeSpan.FromSeconds(1);

        IObservable<long> observable = Observable
            .Interval(period).Do(l => Debug.Log($"Publishing:{l}"))
            .Publish()
            .RefCount();

        Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ =>
        {
            Debug.Log("@@@@ subscribe @@@@");
            // Observable.Connect();// 改用 RefCount
            IDisposable subscription = observable.Subscribe(i => Debug.Log($"subscription:{i}"));
            Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(__ =>
            {
                Debug.Log("@@@@ unSubscribe @@@@");
                subscription.Dispose();// 清除之前订阅的事件，然后下面订阅新的事件
                observable.Subscribe(i => Debug.Log($"subscription 2: {i}"));
            });
        });

        // 除此之外还有一个 Share 操作符：与 Publish().RefCount() 一样
        *//*public static IObservable<T> Share<T>(this IObservable<T> source)
        {
            return source.Publish().RefCount();
        }*/
        #endregion


        #region 30、Replay 操作符：
        /*IConnectableObservable<long> replayObservable = Observable.Interval(TimeSpan.FromSeconds(1.0f))
                                        .Do(l => Debug.Log($"Observable:{l}"))//立即执行
                                        .Replay();// 以重播方式发射数据

        replayObservable.Subscribe(l => Debug.Log($"Subscription #1:{l}"));// 接收到数据后执行回调
        replayObservable.Connect();

        Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => replayObservable.Subscribe(l => Debug.Log($"Subscription #2:{l}")));*/
        #endregion


        #region 31、Connect 操作符：(无代码)

        #endregion


        #region 32、Throw 操作符：
        /*//输出结果: 发现异常: error
        Observable.Throw<string>(new Exception("error")).Subscribe(_ => Debug.Log("不会输出"), e => Debug.Log($"发现异常：{e.Message}"));*/
        #endregion


        #region 33、Catch 操作符：
        /*// 输出结果为: catched exception:error，1 秒后输出 timer called
        Observable.Throw<string>(new Exception("error"))
            .Catch<string, Exception>(e =>
            {
                Debug.Log($"catched exception:{e.Message}");
                return Observable.Timer(TimeSpan.FromSeconds(1.0f)).Select(_ => "timer called");
            })
            .Subscribe(result => Debug.Log(result));*/
        #endregion


        #region 34、Finally 操作符：
        /*Subject<int> source = new Subject<int>();
        IObservable<int> result = source.Finally(() => Debug.Log("Finally action ran"));// result 是 source 终止时的事件源

        result.Subscribe(number => Debug.Log($"OnNext({number})"),() => Debug.Log("OnCompleted()"));// 订阅 result，当 result 发射通知，执行回调
        source.OnNext(1);// 发布通知
        source.OnNext(2);
        source.OnNext(3);
        //source.OnError(new Exception());// 异常终止
        source.OnCompleted();// 正常终止*/
        #endregion


        #region 35、DelaySubscription 操作符：
        /*Debug.Log(Time.time);
        Observable.ReturnUnit().DelaySubscription(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log(Time.time));*/
        #endregion


        #region 36、PairWise 操作符：
        /*Observable.Range(0, 10).Pairwise().Subscribe(pair => Debug.Log($"{pair.Previous} : {pair.Current}"));*/
        #endregion


        #region 37、小结：(无代码)

        #endregion
        #endregion


        #region 3、UniRx 独有的操作符：

        #region 4、BatchFrame 操作符：
        /*// 运?结果：收集每 100 帧内的点击事件，然后进?统?的输出
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .BatchFrame(100, FrameCountType.EndOfFrame)
            .Subscribe(clicks => Debug.Log(clicks.Count));*/
        #endregion

        #region 7、SampleFrame 操作符：
        /*// Sample 是“采样”的意思，即隔多少帧取一个数据的意思
        Observable.EveryUpdate().SampleFrame(5).Subscribe(_ => Debug.Log(Time.frameCount));

        // 以前的优化手段：每隔 5 帧调用一次 GC.Collect()
        Observable.EveryUpdate().SampleFrame(5).Subscribe(_ => GC.Collect());*/
        #endregion
        #endregion
        #region

        #endregion


        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion

        #region

        #endregion
    }
}
