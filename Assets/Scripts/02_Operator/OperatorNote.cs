using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
using UniRx.Triggers;
using UnityEngine.UI;

#region 1��UniRx �� Linq �н�������Ĳ�������

///1��Linq �� UniRx ��������
/// 1��UniRx ��?������������� Where��First �ȣ���Щ����������˼�� LINQ ����������?�£�ѧϰ UniRx ������֮ǰ�ȼ�ѧϰ?�� LINQ ���������Լ������Ƕ� UniRx �����(ʹ�� Linq ��Ҫ������ System.Linq �����ռ�)
/// 
/// 2��LINQ ��飺
///     1��LINQ ���� C# 3 ������?�������Ҫ��?�ֲ�ѯ�﷨��ʵ�֣������� SQL ���?����ѯ C# �������б�XML ?�������ݿ�
///     2����򵥵� LINQ ʵ�־��� List �� ForEach��ForEach �� LINQ ����ʽд�����书�ܺ����ǳ�?�� foreach ��?����
///         --
///         var testList = new List<int>{1,2,3};
///         testList.ForEach(num => Debug.Log(num));
///         --
///         ��������1��2��3
///         --
///     3����ǰ���е� UniRx �������� Linq Ҳ��ͨ�õģ�
///         --
///         var firstNumber = testList.First();
///         Debug.Log(firstNumber);
///         testList.Where(num => num > 1).ForEach(number => Debug.Log(number));
///         --
/// 3��Ϊʲô�Ƚ��� LINQ?
/// 
///     --ѧϰ LINQ ���Լ������Ƕ� UniRx ����⣬?���еĲ�����?�ϻ�ɬ��ʹ? LINQ �����ų� UniRx �첽��?��?�š���ֻҪ���¼�Դ����?��List �ͺ������
///     
/// 4��Rx ����ʷ��
///     1��ReactiveX �� Reactive Extensions ����д��?���дΪ Rx������� LINQ ��?����չ����΢��ļܹ�ʦ Erik Meijer �쵼���Ŷӿ������� 2012 �� 11 ?��Դ��Rx ��?�����ģ�ͣ�?�����ṩ?�µı�̽�?�����������߸�?��ش����첽������
///     2��Rx ��?�� .NET��JavaScript �� C++��Rx ��?��Խ��Խ��?�������Ѿ�?��?��ȫ������?�����?��Rx ��?������?���� ReactiveX �����֯����ά����?����?���� RxJava/RxJS/Rx.NET��? Unity �İ汾������ UnIRx
///     
/// 5��ʲô�� Rx��
///     1��΢����Ķ����ǣ�Rx ��?�������⣬�ÿ����߿�����?�ɹ۲����к� LINQ ?���ѯ����������д�첽�ͻ����¼��ĳ���ʹ? Rx�������߿���? Observables ��ʾ�첽��������? LINQ ��������ѯ�첽�������� ? Schedulers �������첽�������Ĳ�������Rx �����������壺Rx = Observables + LINQ + Schedulers
///     2��ReactiveX.io(��?) ���Ķ����ǣ�Rx ��?��ʹ?�ɹ۲���������?�첽��̵ı�̽�?��ReactiveX ����˹۲���ģʽ��������ģʽ�ͺ���ʽ��̵ľ�����
///     --���Ͼ����Ƕ� Rx ���Ͻ������ˡ��������ڽ���ÿ��������֮ǰ������ͨ�� LINQ ��?���ܣ�����������Ϊ LINQ ��? UniRx ������⣬UniRx ��Ϊ����ʱ����?�������첽�ģ����Ի��ͬѧ�����?�š������� LINQ �� UniRx ��?�����Ƶģ����������ں�߽��ܡ�
/// 
/// 6��Rx ��Ӧ?��
///     1����?��֪���� Microsoft��Netflix��Github��Trello��SoundCloud ����? Rx �ĸ�����?ʵ�ְ汾
///     2��Unity �� uFrame ��QFramework �ȿ�ܶ������� UniRx������?Щ�� BindingRx��EcsRx ��Ҳ��?������Ŀ�ܣ�ֻ������̫����������Ҳ?��ֵ��ȥ�о�
///     


///2��֮ǰ��ѧ�� UniRx �������ܽ᣺
/// 1���ӱ��¿�ʼ���������������в�������������?���ĸ�ʽ��?���ܣ�
/// 2����ʽ�ݶ�����:
///     1��LINQ ����
///     2��ʾ������
///     3����ѯ���ʽ(�����)
///     4��UniRx ͼ��(�� PPT)
///     5��ʾ������
///     6����ѯ���ʽ(�����)
///     7���������༰ UniRx ֪ʶ��λ��(�� PPT)


///3��Where ��������
/// 0���ڵ�?�µ�?�¶��нӴ��� Where ����������ڿ���Ҫ��ʵ��?�������ÿ���˵�ĸ�ʽ�淶
/// 1��LINQ Where ���:
///     1��LINQ �е� Where �������� SQL �����е� Where ��?���ƣ������𵽷�Χ�޶�Ҳ���ǹ�����?�ģ��ж�������������?���ӵ�?��
///     2��LINQ Where ʾ������:
///         --
///         // ��ѯ?�� 45 ���ѧ?:
///         class Student
///         {
///             public string Name;
///             public int Age;
///         }
///         private void Start()
///         {
///             List<Student> students = new List<Student>()
///             {
///                 new Student(){Name = "����",Age = 50},
///                 new Student(){Name = "����",Age = 51},
///                 new Student(){Name = "����",Age = 52},
///             }
///             var oldStudents = students.Where(student => student.Age > 45);
///             foreach(var student in oldStudents)
///             {
///                 Debug.Log(student.Name);
///             }
///         }
///         --
///     3������?Ҫ����?���������ѯ���/�䷨(LINQ Where ��ѯʽ)��
///         --
///         //���ϴ���ĳɲ�ѯ��ʽ
///         var oldStudents = from oldStudent in students Where oldStudent.Age > 45 select oldStudent;
/// 2��Where ��������?����?�Ĳ�����
/// 3��UniRx Where ʾ�����롪��?�����¼�������
///     --
///     Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => { Debug.Log("�����"); }).AddTo(this);
///     --
/// 4��UniRx Where ��ѯʽ��
///     --
///     IObservable<long> mouseClickEventStreams = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonDown(0) select updateEvent;
///     mouseClickEventStreams.Subscribe(_ => { Debug.Log("mouse clicked"); }).AddTo(this);
///     --
///         


///4��Select ��������
/// 1������?�ڽ��� Where ������ʱ������ UniRx ���� LINQ�����ѯ���ʽ�������?�� select��������ʽ��д���У�ȴû��ʹ?���Ӧ�� Select ������������Ϊʲô��?ԭ���ǣ��� ��ѯʽд���У�������Ҫ���������� select ?�£�?��ʽд���У����ǿ�������ʡ�Ե��ˡ�
///     1����ʡ�Եİ汾��var oldStudents = students .Where(student => student.Age > 45).Select(student => student);
///     2����ʵ����ѡ����?��??������ѡ��??����ʵ������ѡ��?���������͵ı�����Ϊ�������:
///         --
///         var oldStudentNames = students.Where(student => student.Age > 45).Select(student => student.Name);
///         --
/// 2��LINQ Select ��飺
///     1��LINQ �е� Select �������� SQL �����е� Select ��?���ƣ�����λ�ò�ͬ����ѯ���ʽ�е� select ������?���Ƿ�����󲢰�?���еı���Ҳ���ǽ�����ػ�����
///         --
///         var oldStudentNames = students.Where(student => student.Age > 45) .Select(student => student.Name);
///         --
/// 3��LINQ Select ��ѯʽ��
///     --
///     var oldStudentNames = from student in students where student.Age > 45 select student.Name;
///     --
/// 4��UniRx Select ʾ�����룺
///     --
///     Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0)).Select(_ => "mouse up").Subscribe(Debug.Log).AddTo(this);
///     --
/// 5��UniRx Select ��ѯʽ���룺
///     --
///     var mouseUpEventSteam = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonUp(0) select "mouse up";
///     mouseUpEventSteam.Subscribe(Debug.Log).AddTo(this);
///     --


///5��First ������
/// 1��LINQ First ���:
///     1��ȡ�����еĵ�?��Ԫ��
///     2��First ��������ʽ��
///         1��ֱ�ӻ�ȡ��?��Ԫ�أ�var oldStudent = students.Where(student => student.Age > 45).First();
///         2��ȡ��������?�����ĵ�?��Ԫ�أ�var oldStudent = students.First(student => student.Age > 45);
///     3��First ������ Where ?����??������
/// 2��LINQ First ��ѯʽ��var oldStudent = (from student in students select student).First(student => student.Age > 45);
/// 3��First ���������ڡ���ҳ����?����
/// 4��UniRx First ʾ������:
///     Observable.EveryUpdate().First(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("mouse down")).AddTo(this);
///     


///6��Distinct ��������
/// 1��Distinct ��?��˼:�����ġ���ͬ�ġ�
/// 2��LINQ Distinct ���:ɸѡ�����в���ͬ��ֵ��?�ڲ�ѯ���ظ��Ľ������?�� SQL ���Ϊ: SELECT DISTINCT [City] FROM [Customers]
/// 3��LINQ Distinct ʾ�����룺
///     1��IEnumerable<string> distinctNames = names.Distinct();
///     2�����ڹ��˶���Ҳ��֧�ֵģ�ֻ������Щ�鷳����Ҫͨ��ʵ��һ������ʵ��
/// 4��LINQ Distinct ��ѯʽʾ�����룺
///     1��IEnumerable<string> distinctNames = (from name in names select name).Distinct();
///     2����ʵ��ѯʽ�����еㆪ�£�����ֱ��?��ʽ���ʽ
/// 5��Distict ��?�ϳ�?��?��
/// 6��UniRx Distinct ʾ�����룺
///     1��students.ToObservable().Distinct(student => student.name).Subscribe(student => Debug.Log(student.name)).AddTo(this);
///     2��UniRx �� Distinct �ڲ��� List ��ʱ�򣬳���?�� LINQ ��?�ֵ�����?ʽ�⣬��?�ִ�??���ض�����������
///         ---
///         IObservable<string> lectClickStream = Observable.EveryUpdate()
///                                                     .Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "left clicked");
///         IObservable<string> rightClickStream = Observable.EveryUpdate()
///                                                     .Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "right clicked");
///         Observable.Merge(lectClickStream, rightClickStream).Distinct().Subscribe(Debug.Log).AddTo(this);
///         //���ϴ�����Ϊ:���ܵ�����ٴ�?���������?���Ҽ������ֻ���?�� ��left clicked�� �� ��right clicked��
///         ---
///         


///7��Last ��������
/// 1��LINQ Last ��飺ȡ�����е����?��Ԫ��
/// 2��LINQ Last ����ʾ��:Student student = students.Last();
/// 3��Last ���ڷ�?������
/// 4��UniRx Last ����ʾ����
///   students.ToObservable().Last(student => student.age == 50).Subscribe(student => Debug.Log(student.name)).AddTo(this);
/// 5��Last ?�ִ�??������������? LINQ �е� LAST ֻ�ܴ�??������?����
/// 


///8��SelectMany ��������
/// 1��LINQ SelectMany ��飺
///     1��΢���?�Ķ�����: �����е�ÿ��Ԫ��ͶӰ�� IEnumerable<T> ����������кϲ�Ϊ?�����С�
///     2����ÿ���ٽ�?���������ٽ�?�ϳ����С�
/// 2��LINQ SelectMany ʾ�����룺IEnumerable singleChars = students.SelectMany(student => student.name + ":" + student.age);
///     1��SelectMany �����ã�����һ�� List<string> ���� nums �С�123������456������789�� �����ַ���
///     2��ͨ�� nums.SelectMany ֮�� (nums.SelectMany(n => n));�õ�һ���¼���{'1','2','3','4','5','6','7','8','9'}
///     3��Ϊʲô string �ַ������Ա���ɢ���ַ�����Ϊ string �̳��� IEnumerable �ӿڣ��ǿɵ�����
///     4�����ص� n ���Խ��в�����nums.SelectMany(n => n + ":");
///         1���õ�һ���¼���{'1',':','2',':','3',':','4',':','5',':'����}
///         2��ÿ����һ�� n������ n �����һ�� ':'
///     5������ֱ�ӷ��������ַ�����nums.SelectMany(n => "asd");
///         1���õ�һ���¼���{'a','s','d','a','s','d','a','s','d'����}
///         2��ÿ����һ�� n�����������ַ� 'a','s','d'
/// 3��UniRx SelectMany �ǱȽϳ��õ�
/// 4��UniRx SelectMany ����ʾ����SelectMany ���������?�ϻ�ɬ���� UniRx ����Ҫ����� Coroutine ��˳��ִ?���ܣ�
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


///9��Take ��������
/// 1��LINQ Take ��飺�����еĿ�ͷ����ָ������������Ԫ��
/// 2��LINQ Take ����ʾ����
///     ---
///     int[] grades = { 59, 84, 69, 30, 59, 94, 83 };
///     IEnumerable<int> topThreeGrades = grades.OrderByDescending(grade => grade).Take(3);//���������ȡǰ����
///     ---
/// 3��Take ������ֻ����ǰ?�� N ������
/// 4��ʹ? Take �����������޸� Observable ��?Ϊ��ֻ����ǰ?�� N �����ݣ�Ȼ�������֪ͨ������ʣ�������
/// 5��UniRx Take ʾ�����룺
///     this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Take(5).Subscribe(_ => Debug.Log(1));
/// 6��Take ���Դ���һ��ʱ��ν�����
///     ---
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))// �����������
///         .Take(TimeSpan.FromSeconds(5))// ֻ��ǰ�����ڴ�������������Ч
///         .Subscribe(_ => Debug.Log("mouse clicked"))
///         .AddTo(this);
///     ---


///10��Concat ��������
/// 1��LINQ Concat ��飺������������
/// 2��LINQ Concat ʾ�����룺
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
/// 3��Concat��������ķ����������� Observable �ķ�����
///     1��Concat ���������Ӷ�� Observable ��������ͺ���������?�� Observable����?�� Observable �ڵ�?�� Observable ������κ�����ǰ?�����������ݣ��Դ����ơ�ֱ��ǰ??�� Observable ��?��Concat �Żᶩ�Ķ����?�� Observable��
///     2��ע�⣺����㳢������?��"��" Observable������ Observable �ڴ�����?����ʼ�������ݣ���ʹû�ж����ߣ���Concat �����ῴ��Ҳ���ᷢ����֮ǰ������κ����ݡ�
/// 4��Concat �� Merge �� WhenAll ���ƣ������ϲ�����
/// 5��UniRx Concat ʾ�����룺
///     ---
///     IObservable<string> a = this.UpdateAsObservable().Take(3).Select(_ => "A");
///     IObservable<string> b = this.UpdateAsObservable().Take(2).Select(_ => "B");
///     IObservable<string> c = a.Concat(b);
///     c.Subscribe(Debug.Log);
///     ---
///     


///11��WhenAll:
/// 1��LINQ All ��飺ȷ�������е�����Ԫ���Ƿ���?����
/// 2��LINQ All ʾ�����룺
///     ---
///     bool allStartWithB = students.All(student => student.name.StartsWith("B"));
///     Debug.LogFormat("{0} student names start with 'B'.", allStartWithB ? "All" : "Not All");
///     ---
/// 3��UniRx WhenAll��
///     1���ж� Observable ��������������Ƿ���?ĳ������
///     2������?��ν�ʺ����� WhenAll �������������������ԭʼ Observable ��������ݣ����ݼ��㷵��?������ֵ��
///     3��WhenAll ����?��ֻ����?����������ֵ�� Observable�����ԭʼ Observable ������?����ÿ?�����ݶ���?�������ͷ��� true�����ԭʼ Observable ������?�����ݲ���?�����ͷ��� False
/// 4��UniRx WhenAll ʾ�����룺
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
///     // ������Ϊ(A��B��C ���˳��?��)
///     


///12��OfType ��������
/// 1��LINQ OfType ���:����ָ������ɸѡ IEnumerable ��Ԫ�ء�
/// 2��LINQ OfType ����ʾ��:
///     ---
///     ArrayList list = new ArrayList { 30, 30.0f, "test" };
///     list.OfType<float>().ToList().ForEach(num => Debug.Log(num));
///     ---
/// 3��UniRx OfType ʾ�����룺
///     ---
///     // ����һ�� Subject(Observable)
///     Subject<object> objects = new Subject<object>();
///     
///     // ���ĸ� Observable���������͹���(����֪ͨ)
///     objects.OfType<object, string>().Subscribe(Debug.Log);//�� object ����תΪ string ����
///     objects.OnNext(1);
///     objects.OnNext(2);
///     objects.OnNext("3");
///     objects.OnNext(5);
///     
///     // �ֶ������㲥
///     objects.OnCompleted();
///     ---


///13��Cast ��������
/// 1��LINQ Cast ��飺�� IEnumerable ��Ԫ��ǿ��ת��Ϊָ��������
/// 2��LINQ Cast ����ʾ����
///     ---
///     ArrayList fruits = new ArrayList { "mango", "apple", "lemon" };
///     IEnumerable fruitNames = fruits.Cast<string>();
///     foreach (var fruit in fruitNames)
///         Debug.Log(fruit);
///     ---
/// 3��UniRx Cast ����ʾ����
///     ---
///     Subject<object> objects = new Subject<object>();
///     objects.Cast<object, int>().Subscribe(i => Debug.Log(i));//�� object תΪ int ����
///     
///     // �ֶ���������(�㲥)
///     objects.OnNext(1);
///     objects.OnNext(2);
///     objects.OnNext(3);
///     //objects.OnNext("4");//����ת���쳣
///     
///     // �ֶ������㲥
///     objects.OnCompleted();
/// 4��TakeWhile ������ת���Ĳ�����


///14��GroupBy ��������
/// 1��LINQ GroupBy ��飺�������е�Ԫ�ؽ�?����
/// 2��LINQ GroupBy ����ʾ����
///     ---
///     IEnumerable<IGrouping<string,Student>> studentGroupNames = students.GroupBy(student => student.name);
///     foreach (IGrouping<string, Student> studentGroup in studentGroupNames)
///     {
///         Debug.Log($"Group Key:{studentGroup.Key}");
///         foreach (Student student in studentGroup)
///         Debug.Log($"Name:{student.name} Age:{student.age}");
///     }
///     ---
/// 3��LINQ GroupBy ��ѯ���ʽ��
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10).GroupBy(i => i % 3).Subscribe(group =>
///     {
///         group.Subscribe(number => Debug.Log($"key:{group.Key},number:{number}"));
///     });
/// 4��TakeWhile Ҳ�����ڷ�ҳ�Ĳ�����


///15��Range ��������
/// 1��LINQ Range ��飺?��ָ����Χ�ڵ�����������
/// 2��LINQ Range ����ʾ����
///     ---
///     IEnumerable<int> squares = Enumerable.Range(5, 5).Select(x => x * x);//�� 5 ��ʼ�����ǵ������� 5 6 7
///     squares.ToList().ForEach(num => Debug.Log(num));
///     ---
/// 3��UnIRx Range ����ʾ����
///     ---
///     IObservable<int> squares = Observable.Range(5, 3).Select(x => x * x);
///     squares.Subscribe(square => Debug.Log(square));
///     ---
/// 4��ͨ�� LINQ �� UniRx ��?�Ͽ��Կ�����LINQ Ҳ������ Observable �� Enumerable��? Subscribe ��ʵ�൱�� LINQ �е� foreach
/// 5��TakeWhile ������ create ���ɵĲ�����
/// 


///16��Skip ��������
/// 1��LINQ Skip ��飺����������ָ��������Ԫ�أ�Ȼ�󷵻�ʣ���Ԫ��
/// 2��LINQ Skip ����ʾ����
///     ---
///     int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };
///     grades.OrderByDescending(g => g).Skip(3).ToList().ForEach(num => Debug.Log(num));//�������������ǰ����
///     ---
/// 3��UniRx Skip ����ʾ����
///     ---
///     this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Skip(5).Subscribe(_ => Debug.Log("mouse clicked"));
///     //������Ϊ��?����������ʱ���ſ�ʼ�����mouse clicked��
/// 4��TakeWhile Ҳ�����ڷ�ҳ�Ĳ�����
/// 5��Skip ���������Դ���һ��ʱ��ι�����
///     ---
///     Observable.EveryUpdate()
///     .Where(_ => Input.GetMouseButtonDown(0))// �����������
///     .Skip(TimeSpan.FromSeconds(5))// ����ǰ���루ǰ�����ڵ������Ч�������ÿ�ε������Ч��
///     .Subscribe(_ => Debug.Log("mouse clicked"))
///     .AddTo(this);
///     ---


///17��TakeWhile ��������
/// 1��LINQ TakeWhile ��飺���ָ��������Ϊ true���򷵻������е�Ԫ�أ�Ȼ������ʣ���Ԫ��
/// 2��LINQ TakeWhile ʾ�����룺
///     ---
///     string[] fruits = new[] { "apple", "banana", "mango", "orange", "grape", "passionfruit" };
///     IEnumerable<string> fruitsAfterOrange = fruits.TakeWhile(fruit => fruit != "orange");
///     fruitsAfterOrange.ToList().ForEach(value => Debug.Log(value));
///     ---
/// 3��TakeWhile Ҳ�����ڷ�ҳ�Ĳ�����
/// 4��UniRx TakeWhile ����ʾ����
///     ---
///     this.UpdateAsObservable().TakeWhile(value => !Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("before click"));
///     ---
/// 5��TakeWhile �����أ�int ֵ��ʾ�¼������Ĵ���
///     Observable.EveryUpdate()
///             .Where(_ => Input.GetMouseButton(0))// ��갴�²���
///             .TakeWhile((_, number) => !Input.GetMouseButtonUp(0) && number < 100)//�����û��̧���Ҵ���С�� 100,number ��ʾ�¼���������
///             .Subscribe(_ => Debug.Log("mouse button clicking"));


///18��SkipWhile ��������
/// 1��LINQ SkipWhile ��飺���ָ��������Ϊ true�������������е�Ԫ�أ�Ȼ�󷵻�ʣ���Ԫ��
/// 2��LINQ SkipWhile ����ʾ����
///     ---
///     int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };
///     
///     // �������������ǰ�ɼ����ڵ��� 80����������ֵ�������ǰ�ɼ�С�� 80������ʣ���ֵ
///     IEnumerable<int> lowerGrades = grades.OrderByDescending(grade => grade).SkipWhile(grade => grade >= 80);
///     lowerGrades.ToList().ForEach(value => Debug.Log(value));
///     ---
/// 3��SkipWhile �����������ڷ�ҳ�Ĳ�����
/// 4��UniRx SkipWhile ����ʾ����
///     ---
///     this.UpdateAsObservable()
///         .SkipWhile(_ => !Input.GetMouseButtonDown(0))// ����갴��ʱ���ռ�֮����¼�
///         .Subscribe(_ => Debug.Log("mouse button down"));
///     // ʵ�ֵ��߼�Ϊ�������?��󣬳������ mouse button down
///     ---
///     


///19��Zip (.Net 4) ��������
/// 1��LINQ Zip ��飺��ָ������Ӧ�����������еĶ�ӦԪ�أ������ɽ������
/// 2��LINQ Zip ʾ����
///     ---
///     int[] numbers = { 1, 2, 3, 4 };
///     string[] words = { "one", "two", "three", "four" };
///     IEnumerable<string> numbersAndWords = numbers.Zip(words, (first, second) => $"{first} {second}");
///     // ��� 1 one��2 two����
///     numbersAndWords.ToList().ForEach(value => Debug.Log(value));
///     ---
/// 3��Zip ���������ϵĲ�����
/// 4��UniRx Zip ʾ�����룺
///     ---
///     IObservable<Unit> rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
///     IObservable<Unit> leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
///     leftStream.Zip(rightStream, (left, right) => Unit.Default).Subscribe(_ => Debug.Log("ok"));
///     // ֻҪ���Ҽ��ɶԾͻ���� ok���Ȱ�����������ٰ������Ҽ���������� ok
///     ---
/// 


///20��Repeat ��������
/// 1��LINQ Repeat ��飺��?���������ظ���ֵ�Ĵ���
/// 2��LINQ Repeat ����ʾ����
///     ---
///     IEnumerable<string> strs = Enumerable.Repeat("I like programming.", 5);
///     foreach (string str in strs)
///         Debug.Log(str);
///     ---
/// 3��Repeat ������������ Creation ����Ĳ�����
/// 4��UniRx Repeat ����ʾ����
///     ---
///     // ��ʹ�� Repeat��һ������һ���ַ�����֮�������
///     Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log("after 1 seconds"));
///     
///     // ʹ�� Repeat��ÿ�����һ���ַ���(���¼����ظ�ִ��)
///     Observable.Timer(TimeSpan.FromSeconds(1.0f)).Repeat().Subscribe(_ => Debug.Log("after 1 seconds"));
///     ---
///     


///21��TakeLast ��������
/// 1��LINQ TakeLast ��飺��ȡ���е����?��
/// 2��LINQ TakeLast ����ʾ�������� TakseLast ���������� .Net Core �ṩ�ģ����ԾͲ���?����ʾ���Ľ�����
/// 3��TakeLast �����Ƿ��� Observable �������� N ������
/// 4��TakeLast ���˿��Դ�������Ҳ���Դ�ʱ���
/// 5��TakeLast �����ڷ�ҳ�Ĳ�����
/// 6��UniRx TakeLast ����ʾ����
///     ---
///     IObservable<int> bottonThreeGrades = grades.OrderByDescending(grade => grade).ToObservable().TakeLast(3);
///     // ���� bottonThreeGrades �¼�������ĳ��ֵ���� bottonThreeGrades �¼���ִ�лص�������Ч�������� foreach��
///     bottonThreeGrades.Subscribe(buttonThreeGrade => Debug.Log(buttonThreeGrade));
///     ---
/// 7������ʱ��ν� TakeLast ������
///     ---
///     Subject<float> subject = new Subject<float>();
///     
///     subject.TakeLast(TimeSpan.FromSeconds(2.0))// �ռ� subject �����������ݣ�ִ�лص�
///         .Subscribe(clickTime => Debug.Log(clickTime));
///     
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))// �����������ִ�лص�����
///         .Subscribe(_ => subject.OnNext(Time.time));// �㲥��ǰ�¼�
///     Observable.Timer(TimeSpan.FromSeconds(5.0f))// ����󴥷��¼���ִ�лص�����
///         .Subscribe(_ => subject.OnCompleted());// �����ֹͣ subject �Ĺ㲥
///     ---
///     


///22��Single/Default ��������
/// 1��LINQ Single ��飺���������еĵ����ض�Ԫ�أ��� First ?�����ƣ����� Single Ҫȷ������?������Ԫ����������ֻ��?��
/// 2��LINQ Single ����ʾ����
///     ---
///     string fruit1 = fruits.Single(fruit => fruit.Length > 10);
///     Debug.Log(fruit1);
///     ---
/// 3��Single �������� First ���ƣ������ԭʼ Observable �����֮ǰ�������÷���?�����ݣ������׳�?�� NoSuchElementException
/// 4��Single �����������ڷ�ҳ�Ĳ�����
/// 5��UniRx Single ����ʾ����fruits.ToObservable().Single(fruit => fruit.Length > 10).Subscribe(Debug.Log);
/// 6��������� Single ���������������������ϣ����׳��쳣
/// 


///23��ToArray ��������
/// 1��LINQ ToArray ��飺�� IEnumerable<T> �д�������
/// 2��LINQ ToArray ʾ�����룺
///     ---
///     string[] studentNames = students.Select(student => student.name).ToArray();
///     foreach (var name in studentNames)
///         Debug.Log(name);
///     ---
/// 3��ToArray �������Ǻܳ��õĲ�����
/// 4��UniRx ToArray ʾ�����룺
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


///24��ToList ��������
/// 1��LINQ ToList ��飺�� IEnumerable<T> ����?�� List<T>
/// 2��LINQ ToList ʾ�����룺
///     ---
///     List<int> lengths = fruits.Select(fruit => fruit.Length).ToList();
///     lengths.ForEach(value => Debug.Log(value));
///     ---
/// 3��ToList �������Ǻܳ��õĲ�����
/// 4��UniRx ToList ʾ�����룺
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


///25��Aggregate ��������
/// 1��LINQ Aggregate ��飺������Ӧ���ۼ��������� ��ָ��������ֵ�����ۼ����ĳ�ʼֵ����ʹ��ָ���ĺ���ѡ����ֵ
/// 2��LINQ Aggregate ����ʾ����
///     ---
///     int[] numbers = new[] { 1, 2, 3, 4, 5 };
///     int result = numbers.Aggregate((total, next) => total * next);
///     Debug.Log($"5 �Ľ׳�Ϊ��{result}");// ���� 120���� 1 * 2 * 3 * 4 * 5
///     ---
/// 3��Aggregate �����������ھۺϲ����Ĳ�����
/// 4��UniRx Aggregate ����ʾ����
///     ---
///     Observable.Range(0, 8).Aggregate(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));
///     ---
/// 5��Aggregate ������ȡ���ֵ����Сֵ������
///     ---
///     int[] numbers = new[] { 1, 2, 3, 4, 5 };
///     int result = numbers.Aggregate((total, next) => total = next > total ? next : total);
///     Debug.Log($"���ֵΪ{result}");
///     ---
///     


///26��Empty ��������
/// 1��LINQ Empty ��飺���ؾ���ָ�����Ͳ����Ŀ� IEnumerable<T>
/// 2��LINQ Empty ʾ�����룺
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
/// 3��Empty ���������� Creation ������������ڲ��Ե�ʱ���õñȽ϶�
/// 4��UniRx Empty ����ʾ����
///     ---
///     IObservable<Unit> s = Observable.Empty<Unit>();
///     s.Subscribe(e => Debug.Log($"e:{e}"), () => Debug.Log("OnCompleted"));
///     ---
///     


///27��С�᣺
/// ����Ϊֹ�Ѿ�ѧ���� UniRx �� LINQ ������������в������ˣ��ⲿ�ֲ��������Լ�?��ʵ����?�ģ���Ϊ�������Щ������ LINQ ��?���ֲ�����Ҳ�Ͷ�����ˡ�
/// �ڱ���ǰ?���������ͨ�� LINQ ������ȥ��� UniRx ��������������ף����� UniRx �� LINQ �������������ޣ�LINQ ������ҲֻռUniRx �������� 50% ����
/// ��?�½ڿ�ʼѧϰ UniRx �� Rx.Net �н���Ĳ�����(Rx.Net ����Ϊ΢��ٷ��⣬��ֱ��ʹ�� System.Reactive ʹ�� Rx.Net)
/// 

#endregion


#region 2��UniRx �� Rx.Net �н���Ĳ�����(Rx ���еĲ�����)
///1��Interval ��������
/// 1��Interval ����������ʱ����صĲ�������������˼�ǡ������
/// 2��Interval ����ʾ����
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(second => Debug.Log($"��ǰʱ�䣺{second} s")).AddTo(this);
///     
///     // ����Ч��Ҳ��ʹ�� Repeat ʵ��
///     //Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe()).AddTo(this);
///     ---
///     


///2��TakeUntil ��������
/// 1������?�� Observable ������?�����ݻ�����?ʱ��ԭʼ Observable ֹͣ��������
/// 2��TakeUntil ���Ĳ���ʼ����ԭʼObservable�������������ṩ�ĵ�?�� Observable�������?�� Observable ������?�����ݻ��߷�����?����?֪ͨ��TakeUntil ���ص� Observable ��ͣ?����ԭʼ Observable ����?
/// 3��TakeUntil �����������ڷ�ҳ�����Ĳ�����
/// 4��TakeUntil ʾ�����룺
///     ---
///     // ������Ϊ��������� 123�������?�������ͣ?��� 123
///     this.UpdateAsObservable()//��һ�� Observable
///         .TakeUntil(Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)))// �������ǵڶ��� Observable
///         .Subscribe(_ => Debug.Log(123))
///         .AddTo(this);
///     ---
/// 


///3��SkipUntil ��������
/// 1������ԭʼ Observable ��������ݣ�ֱ����?�� Observable ������?������
/// 2��SkipUntil ������Ҳ�����ڷ�ҳ�����Ĳ�����
/// 3��SkipUntil ����ʾ����
///     ---
///     // ������Ϊ�����?�����֮��Ϳ�ʼ������� ��?�갴���ˡ�
///     IObservable<Unit> clickStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
///     this.UpdateAsObservable()
///         .SkipUntil(clickStream)// �� clickStream ��������֮ǰ��������ǰ�¼�
///         .Subscribe(_ => Debug.Log("��갴����"))
///         .AddTo(this);
///         


///4��Buffer ������(����)��
/// 1��Buffer ������Ҳ�����ڷ�ҳ�����Ĳ�����
/// 2������ʾ����
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(1.0f))// ÿ��һ�뷢��һ������
///                 .Buffer(TimeSpan.FromSeconds(3.0f))// ���� 3 ���ڵ�����
///                 .Subscribe(_ => Debug.Log($"currentTime:{DateTime.Now.Second}"))// �������ݺ�ִ�лص�����
///                 .AddTo(this);
///     ---
///     


///5��Throttle(������)��������
/// 1���ڹ���?��ָ����ʱ�仹û��������ʱ�ŷ���?������
/// 2��Throttle ����������������ʱ���йصĲ�����
/// 3��ʾ�����룺
///     ---
///     //�����������?��� 1 ���ڲ��ٵ����������е�������¼�ʱ 1 �������
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))// ��������ʱ
///         .Throttle(TimeSpan.FromSeconds(1))//һ����ٵ����ִ�лص����������¼�ʱһ�룬ֱ�����¼�ʱ��ʼһ����ٵ����ִ�лص�
///         .Subscribe(_ => Debug.Log("һ�����"));
///     ---
///     


///6��Delay ��������
/// 1���ӳ�?��ָ����ʱ���ٷ�����? Observable �ķ�����
/// 2��Delay ������Ҳ����ʱ����صĲ�����
/// 3��Delay ����ʾ����
///     ---
///     // �����������?�� 1 ��֮����� mouse clicked��ÿ�ε���¼����� 1 ��֮������
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Subscribe(_ => Debug.Log("mouse clicked"))
///         .AddTo(this);
///     ---
///     


///7��Return ��������
/// 1��Return ���������� Creation ���������صĲ�����
/// 2��Return ʾ�����룺
///     ---
///     Observable.Return("hello").Subscribe(Debug.Log);//ִֻ��һ�Σ����� set
///     ---
///     


///8��Timer ��������
/// 1������?�� Observable������?���������ӳٺ���?�������ֵ��
/// 2��Timer ����������ʱ�������صĲ�����
/// 3��Timer ����ʾ����
///     ---
///     // ִ?������� 5 ������ ��after 5 seconds��
///     //Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => Debug.Log("after 5 seconds")).AddTo(this);
///     Observable
///         .Timer(TimeSpan.FromSeconds(1.0f),TimeSpan.FromSeconds(2.0f))// ��һ��֮��ÿ�����뷢��һ������
///         .Subscribe(_ => Debug.Log("after 5 seconds"))
///         .AddTo(this);
///         


///9��Sample ��������
/// 1�����ڷ��� Observable ��������������
/// 2��Sample �������ᶨʱ�鿴?��Observable��Ȼ����?�ϴβ��������������������ݡ�
/// 3����ĳЩʵ���У���?�� ThrottleFirst �������Ĺ������ƣ������Ƿ�������ڼ����������ݣ�?�Ƿ������Ƕ�ʱ���ڵĵ�?������
/// 4��RxJava �����������ʵ��Ϊ sample �� throttleLast
/// 5��ע�⣺���?�ϴβ���������ԭʼ Observable û�з����κ����ݣ�����������ص� Observable ���Ƕ�ʱ����Ҳ���ᷢ���κ�����
/// 6��Sample(���� throttleLast)��?�����尴�ղ�����ָ����ʱ������ʱ������TimeUnit ָ��ʱ�䵥λ��
/// 7��Sample ���������Ĭ���� computation ��������ִ?�����������ʹ?����������ָ�������ĵ�����
/// 8��Sample ���������ÿ����?�� Observable ����?�����ݣ����ߵ�����?��ʱ�Ͷ�ԭʼ Observable ��?��������?�� Observable ͨ���������ݸ� Sample��Sample ���������Ĭ�ϲ����κ��ض��ĵ�������ִ?
/// 9��Sample ����������ʱ�������صĲ�����
/// 10��ʾ�����룺
///     ---
///     Observable.Interval(TimeSpan.FromMilliseconds(50))//ÿ�� 50 ���뷢��һ������
///                 .Sample(TimeSpan.FromSeconds(1))// ÿ��һ�����һ�β����������ǰ������������֮ǰ�Ѿ��ɹ����ˣ���ô�Ͳ��ٽ��в�����ֱ����������������֮ǰÿ�������ģ��Ž��в���
///                 .Subscribe(_ => Debug.Log(DateTime.Now.Second))
///                 .AddTo(this);
///     
///     int clickCount = 0;
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .Select(_ => clickCount++)
///         .Sample(TimeSpan.FromSeconds(3))
///         .Subscribe(_ => Debug.Log($"�� {clickCount} �ε��"))
///         .AddTo(this);
///         


///10��Timestamp ��������
/// 1���� Observable ������������?��ʱ���
/// 2��RxJava �е�ʵ��Ϊ timestamp������?������ T �������ݵ� Observable ת��Ϊ?����������ΪTimestamped<T> �����ݵ� Observable��ÿ?��������ݵ�ԭʼ����ʱ�䡣timestamp Ĭ���� immediate ��������ִ?�����ǿ���ͨ������ָ�������ĵ�������
/// 3��Timestamp ����������ʱ�������صĲ�����
/// 4��Timestamp ����ʾ����
///     ---
///     Observable.Interval(TimeSpan.FromSeconds(1.0f))
///                 .Timestamp()
///                 //.Subscribe(timestamp => Debug.Log(timestamp))
///                 .Subscribe(timestamp => Debug.Log(timestamp.Timestamp.LocalDateTime))//�������ʱ��
///                 .AddTo(this);
///                 


///11��ThrottleFirst ��������
/// 1��ThrottleFirst �� throttleLast/sample ��ͬ����ÿ�����������ڣ������Ƿ���ԭʼ Observable �ĵ�?�����ݣ�?���������?��
/// 2��ThrottleFirst ������Ĭ���� computation ��������ִ?�����������ʹ?����������ָ�������ĵ�����
/// 3��ThrottleFirst �� Sample ?������
/// 4��ThrottleFirst ����������ʱ�������صĲ�����
/// 5��ThrottleFirst ����ʾ����
///     ---
///     //��?�����?����֮��?����� ��Clicked�������֮��� 5 ���ڵ��?Ч
///     this.UpdateAsObservable()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .ThrottleFirst(TimeSpan.FromSeconds(5))
///         .Subscribe(x => Debug.Log("Clicked"))
///         .AddTo(this);
///     ---
///     


///12��TimeInterval ��������
/// 1����?���������ݵ� Observable ת��Ϊ ������Щ���� ʱ������ Observable
/// 2��TimeInterval ������������ԭʼ Observable �����������滻Ϊ�����ʾ���ڷ�����ʱ�����Ķ���
/// 3��TimeInterval ����������ʱ�������صĲ�����
/// 4��TimeInterval ����ʾ����
///     ---
///     Observable.EveryUpdate()
///                 .Where(_ => Input.GetMouseButtonDown(0))
///                 .Select(_ => "clicked")
///                 .TimeInterval()
///                 .Subscribe(timeInterval => Debug.Log($"{timeInterval.Interval} : {timeInterval.Value}"))
///                 .AddTo(this);
///     ---
///     


///13��Defer ��������
/// 1��Defet���Ƴ١����ڡ��ӳ�
/// 2��ֱ���й۲��߶���ʱ�Ŵ��� Observable������Ϊÿ���۲��ߴ���?���µ� Observable
/// 3��Defer ��������?ֱ�ȴ�ֱ���й۲��߶�������Ȼ����ʹ? Observable ???��?��?�� Observable
///     --����ÿ���۲��߶�����������˾���ÿ�������߶���Ϊ??���ĵ���ͬ?�� Observable����ʵ��ÿ�������߻�ȡ����??�ĵ�������������
///     --��ĳЩ����£��ȴ�ֱ�����?���ӣ�����֪�����ķ�?ʱ����?�� Observable ����ȷ�� Observable �������µ�����
/// 4��Defer ���������봴�������صĲ�����
/// 5��Defer ����ʾ����
///     ---
///     System.Random random = new System.Random();
///     Observable
///         .Defer(() => Observable.Start(() => random.Next()))//Observable.Start(() => random.Next()) �Ὺ��һ���̴߳���һ���µ� Observable��Ȼ������һ�������
///         .Delay(TimeSpan.FromMilliseconds(1000))
///         .Repeat()
///         .Subscribe(randomNumber => Debug.Log(randomNumber))// ÿ�ζ���ʱ��Defer �ᴴ��һ�� Observable 
///         .AddTo(this);
///         


///14��Never ��������
/// 1������?������������Ҳ����?�� Observable
/// 2��Never ���������봴�������صĲ�����
/// 3��Never ʾ�����룺
///     ---
///     /*//?ǰ����֪��?��?��û��������
///     IObservable<string> never = Observable.Never<string>();
///     // ������û�й㲥/֪ͨ�� Subject
///     Subject<string> subject = new Subject<string>(); */
///     ---
///     


///15��Scan ��������
/// 1�������ض��������е�ÿһ��Ӧ��һ��������Ȼ������������
/// 2��Scan��������ԭʼ Observable ����ĵ�?������Ӧ??��������Ȼ���Ǹ������Ľ����Ϊ??�ĵ�?�����ݷ��䡣���������Ľ��ͬ��?������?�����������������?��??�ĵ�?�����ݡ���������?�����������?ʣ����������С������������ĳЩ����±����� accumulator��
/// 3���� Aggregate ���ƣ����� Scan ��ÿ�ν�?�����? Aggregate ���ǽ���������?�����
/// 4��Scan ����������ۺϲ�����صĲ�����
/// 5��ʾ�����룺
///     ---
///     Observable.Range(0, 8).Scan(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));
///     ---
///     


///16��Switch ��������
/// 1����?�������� Observables �� Observable ת������?�������� Observable�����߷�����Щ Observables ��������������
/// 2��Switch ����?�������� Observables �� Observable����ÿ�ι۲���Щ Observables �е�?����Switch ���ص���� Observable ȡ������ǰ?���������ݵ� Observable����ʼ��������� Observable ��������ݡ�
/// 3��ע�⣺��ԭʼ Observable ������?���µ� Observable ʱ����������µ� Observable ������?������ʱ��������ȡ������֮ǰ���Ǹ� Observable������ζ�ţ��ں����Ǹ� Observable ��?֮������ʼ��������֮ǰ�����ʱ��?��ǰ?�� Observable ��������ݽ�������������ͼ���ϵ��Ǹ�??ԲȦ?������
/// 4��Switch �������������ϲ�����صĲ�����
/// 5��Switch ����ʾ����
///     ---
///     // ִ?�����������?��ʱ��� ��mouse button down�� ̧��֮����� ��mouse button up��
///     var buttonDownStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
///     var buttonUpStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0));
///     buttonDownStream.Select(_ =>
///     {
///         Debug.Log("mouse button down");
///         return buttonUpStream;
///     })
///         .Switch()// �л��¼����� buttonUpStream
///         .Subscribe(_ => Debug.Log("mouse button up"))// buttonUpStream �¼����Ļص�����
///         .AddTo(this);
///     ---
/// 6��ʹ�� Switch ������ʵ�֡��ʵ�������:����˳���� W A S D ʱ�����ʵ���
///     ---
///     var wObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.W));
///     var aObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.A));
///     var sObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.S));
///     var dObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.D));
///     
///     wObservable.Select(_ => aObservable)
///         .Switch()// �л��� aObservable �¼���
///         .Select(_ => sObservable)
///         .Switch()// �л��� sObservable �¼���
///         .Select(_ => dObservable)
///         .Switch()// �л��� dObservable �¼���
///         .Repeat()
///         .Subscribe(_ => Debug.Log("�����ʵ�"))
///         .AddTo(this);
///         


///17��StartWith ��������
/// 1���������Ҫ?�� Observable �ڷ�������֮ǰ�ȷ���?��ָ�����������У�����ʹ? StartWith �������������������?�� Observable ���������ĩβ׷��?���������п���ʹ? Concat ��������
/// 2���ɽ���?�� Iterable ���߶�� Observable ��Ϊ�����Ĳ���
/// 3��StartWith �������������ϲ�����صĲ�����
/// 4��StartWith ����ʾ����
///     --
///     Observable.Return("sikiedu.com")
///                 .StartWith("http://")
///                 .Aggregate((current, next) => current + next)// ���û�� Aggregate ������������� "http://"��������ƴ�ӳ�һ���ַ���
///                 .Subscribe(Debug.Log)
///                 .AddTo(this);
///                 


///18��CombineLatest ��������
/// 1�������� Observables �е��κ�?������������ʱ��ʹ??���������ÿ�� Observable ����������������һ�����������Ľ���������ݡ�
/// 2��CombineLatest ������?Ϊ������ zip��
///     1��ֻ�е�ԭʼ�� Observable �е�ÿ?����������?������ʱ zip �ŷ�������
///     2��CombineLatest ����ԭʼ�� Observable ������?������������ʱ����?������
/// 3����ԭʼ Observables ���κ�?��������?������ʱ��CombineLatest ʹ??������������������������ݣ�Ȼ������������ķ���ֵ��
/// 4��CombineLatest �������������ϲ�����صĲ�����
/// 5��ʾ�����룺
///     ---
///     int a = 0;
///     int i = 0;
///     var leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => (++a).ToString());
///     var rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => (++i).ToString());
///     leftStream.CombineLatest(rightStream, (left, right) => left + right).Subscribe(Debug.Log);
///     ---
///     


///19��Do ��������
/// 1��ע��?��������Ϊԭʼ Observable ?�������¼���?��ռλ��
/// 2��Do �������Ǻܳ��õĲ�����
/// 3��ʾ�����룺
///     ---
///     Observable.ReturnUnit()// ���ؿ�
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Do(_ => Debug.Log("after 1 seconds"))
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Do(_ => Debug.Log("after 2 seconds"))
///         .Delay(TimeSpan.FromSeconds(1.0f))
///         .Subscribe(_ => Debug.Log("after 3 seconds"))
///         .AddTo(this);
///     ---
///     


///20��Merge ��������
/// 1��Merge �������������ϲ�����صĲ�����
/// 2������ʾ����
///     ---
///     // �����������?�������� ��A�������?���Ҽ���� ��B��
///     var aStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "A");
///     var bStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "B");
///     aStream.Merge(bStream).Subscribe(Debug.Log);
///     ---
///     


///21��Materialize/Dematerialize ��������
/// 1��Materialize ����������¼�֪ͨ�������������
/// 2��?���Ϸ������޵� Obversable ����?���Ĺ۲��ߵ� onNext ?����λ��Σ�Ȼ���?�۲��ߵ� onCompleted �� onError ����?�Ρ�Materialize ����������?ϵ�е�?������ԭ����֪ͨ onNext ����?֪ͨ onCompleted �� onError ��ת��Ϊ?�� Observable �������������
/// 3��Dematerialize �������� Materialize ��������̣����� Materialize ת���Ľ����ԭ����ԭ������ʽ
/// 4��Dematerialize ��ת������̣���ԭʼ Observable ����� Notification ����ԭ�� Observable ��֪ͨ
/// 5��Materialize/Dematerialize �������ǳ��ò�����صĲ�����
/// 6��Materialize/Dematerialize ����ʾ����
///     ---
///     Subject<int> subject = new Subject<int>();
///     IObservable<int> onlyExceptions = subject.Materialize().Where(n => n.Exception != null).Dematerialize();
///     
///     // �� subject ����ע��
///     subject.Subscribe(i => Debug.Log($"Subscriber 1:{i}"), ex => Debug.Log($"Subscriber 1 exception: {ex.Message}"));
///     
///     // �� Dematerialize ����ע��
///     onlyExceptions.Subscribe(i => Debug.Log($"Subscriber 2:{i}"), ex => Debug.Log($"Subscriber 2 exception: {ex.Message}"));
///     subject.OnNext(123);
///     subject.OnError(new Exception("Test Exception"));
///     ---
///     


///22��IgnoreElements ��������
/// 1���������κ����ݣ�ֻ���� Observable ����?֪ͨ
/// 2��IgnoreElements ����������ԭʼ Observable ������������ݣ�ֻ����������?֪ͨ��onError �� onCompleted��ͨ��
/// 3������㲻��??�� Observable ��������ݣ�����ϣ���������ʱ������������?ʱ�յ�֪ͨ������Զ� Observable ʹ? ignoreElements ������������ȷ����Զ�����?�۲��ߵ� onNext() ?��
/// 4��ʾ�����룺
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


///23��DistinctUntilChanged ��������
/// 1��DistinctUntilChanged �������ǳ��ò�����ز�����
/// 2��ʾ�����룺
///     ---
///     Subject<int> subject = new Subject<int>();
///     IObservable<int> distinct = subject.DistinctUntilChanged();// subject ��ֵ�����ı�ʱ�������ݵ��¼���
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


///24��Create ��������
/// ����ʹ??��������ͷ��ʼ����?�� Observable
/// ��������ʹ? Create ��������ͷ��ʼ����?�� Observable�����������������?�����ܹ۲�����Ϊ�����ĺ�������д�������������?Ϊ����Ϊ?�� Observable --ǡ���ĵ�?�۲��ߵ� onNext��onError �� onCompleted?��
/// ����?����ʽ��ȷ������ Observable ���볢�Ե�?�۲��ߵ� onCompleted ����?�λ������� onError ����?�Σ�?�Ҵ˺����ٵ�?�۲��ߵ��κ�����?����
/// 4��Create ���������ڴ�����صĲ�����
/// 5��ʾ�����룺
///     ---
///     Observable.Create<int>(observable =>
///     {
///         observable.OnNext(1);
///         observable.OnNext(2);
///         observable.OnCompleted();
///         return Disposable.Create(() => Debug.Log("�۲�����ȡ������"));
///     }).Subscribe(number => Debug.Log(number)).AddTo(this);
///     ---
///     


///25��Amb ��������
/// 1�������������� Observable����ֻ�������ȷ������ݻ�֪ͨ���Ǹ� Observable ����������
/// 2�����ݶ�� Observable �� Amb ʱ����ֻ��������?�� Observable �����ݺ�֪ͨ�����ȷ���֪ͨ�� Amb ���Ǹ������ܷ������?�����ݻ���?�� onError �� onCompleted ֪ͨ��Amb �����ԺͶ����������� Observables �ķ�����
/// 3��Amb ���������벢��������صĲ�����
/// 4��Amb ����ʾ����
///     ---
///     Observable.Amb(
///     Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "3 sec"),
///         Observable.Timer(TimeSpan.FromSeconds(10)).Select(_ => "10 sec"),
///         Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "2 sec"),// ֻ�������ȷ�������(��֪ͨ)�� Observable ����������
///         Observable.Timer(TimeSpan.FromSeconds(22)).Select(_ => "30 sec"),
///         Observable.Timer(TimeSpan.FromSeconds(6)).Select(_ => "5 sec"))
///             .Subscribe(s => Debug.Log($"OnNext:{s}"), () => Debug.Log("OnCompleted"))
///             .AddTo(this);
///     ---
///     


///26��Timeout ��������
/// 1����ԭʼ Observable ��?�������������?��ָ����ʱ?��û�з������ݣ����ᷢ?������֪ͨ
/// 2�����ԭʼ Observable ����ָ����?��ʱ?û�з����κ����ݣ�Timeout ����������?�� onError ֪ͨ��?��� Observable
/// 3��Timeout ����������ʱ�������صĲ�����
/// 4��Timeout ʾ�����룺
///     ---
///     // ��?�������?����û���κβ������ᱨ�쳣
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(1))
///         .Take(10)
///         .Timeout(TimeSpan.FromSeconds(1.0))
///         .Subscribe(_ => Debug.Log("clicked"))
///         .AddTo(this);
///     
///     //���ڵȴ����磺
///     ObservableWWW.Get("http://sikiedu.com")
///         .Timeout(TimeSpan.FromSeconds(5.0f))
///         .Subscribe(_ => { }, e => Debug.Log($"�����쳣:{e.Message}"))
///         .AddTo(this);
///     ---
///     


///27��FromEvent ��������
/// 1������������Ķ������������ת��Ϊ Observable
/// 2��FromEvent �����������¼�������صĲ�����
/// 3������ʾ����
///     ---
///     // ����������ÿ�ε��ʱ����� button clicked
///     GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(() => OnClickEvent());
///     Observable.FromEvent(action => OnClickEvent += action, action => OnClickEvent -= action)
///         .Subscribe(_ => Debug.Log("button clicked"))
///         .AddTo(this);
///     ---
///     


///28��Publish ��������
/// 1������ͨ�� Observable ת��Ϊ�����ӵ� Observable
/// 2�������ӵ� Observable (connectable Observable)����ͨ�� Observable ��࣬�������������ڱ�����ʱ��ʼ�������ݣ�?��ֱ��ʹ?�� Connect ������ʱ�ŻῪʼ�������ݡ�ʹ������?������������κ�ʱ����?�� Observable ��ʼ�������ݡ�
/// 3����?���������?��������Ϊ�������������?ԭʼ Observable �����������Ϊ��������??���µ�������Ϊ ConnectableObservable �����䣬�滻ԭλ�õ������ʵ������ǩ���Ļ��������?�� Select ������
/// 4��Publish ����ʾ����
///     ---
///     IObservable<int> unshared = Observable.Range(1, 4);
///     
///     // ÿ����������һ���¶���
///     unshared.Subscribe(i => Debug.Log($"Unshared Subscription #1:{i}"));// 1 2 3 4 ����������������
///     unshared.Subscribe(i => Debug.Log($"Unshared Subscription #2:{i}"));// 1 2 3 4 ����������������
///     
///     // ͨ��ʹ�� Publish �����������ı�ɹ���ģ����ڵ��� Connect() ֮ǰ���Ὺʼ����
///     var shared = unshared.Publish();
///     shared.Subscribe(i => Debug.Log($"shared Subscription #1:{i}"));
///     shared.Subscribe(i => Debug.Log($"shared Subscription #2:{i}"));
///     shared.Connect();// �� Connect ��ʱ�򣬲Żᷢ������
///     ---
/// 5��Publish ����������󶨲�����صĲ�����


///29��RefCount ��������
/// 1����?�������ӵ� Observable ?Ϊ����ͨ�� Observable��
/// 2�������ӵ� Observable (connectable Observable) ����ͨ�� Observable ��࣬�������������ڱ�����ʱ��ʼ�������ݣ�?��ֱ��ʹ?�� Connect ������ʱ�ŻῪʼ�������ݡ�?����?������������κ�ʱ����?�� Observable ��ʼ�������ݡ�
/// 3��RefCount �������Ѵ�?�������ӵ� Observable ���ӺͶϿ��Ĺ���?�����ˡ�������?�������ӵ� Observable������?����ͨ�� Observable������?�������߶������ Observable ʱ��RefCount ���ӵ��²�Ŀ����� Observable��RefCount �����ж��ٸ��۲��߶�������ֱ�����?���۲�����ɲŶϿ����²������ Observable ������
/// 4��RefCount ����������󶨲�����صĲ�����
/// 5��RefCount ʾ�����룺
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
///         // Observable.Connect();// ���� RefCount
///         IDisposable subscription = observable.Subscribe(i => Debug.Log($"subscription:{i}"));
///         Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(__ =>
///         {
///             Debug.Log("@@@@ unSubscribe @@@@");
///             subscription.Dispose();// ���֮ǰ���ĵ��¼���Ȼ�����涩���µ��¼�
///             observable.Subscribe(i => Debug.Log($"subscription 2: {i}"));
///         });
///     });
///     
///     // ����֮�⻹��һ�� Share ���������� Publish().RefCount() һ��
///     /*public static IObservable<T> Share<T>(this IObservable<T> source)
///     {
///         return source.Publish().RefCount();
///     }*/
///     ---
///     


///30��Replay ��������
/// ������֤���еĹ۲����յ���ͬ���������У���ʹ�����ڡ�Observable����ʼ��������֮��Ŷ���
/// ���������ӵ� Observable (connectable Observable)����ͨ�� Observable ��࣬�������������ڱ�����ʱ��ʼ�������ݣ�?��ֱ��ʹ?�� Connect ������ʱ�ŻῪʼ��?����?������������κ�ʱ����?����Observable ��ʼ�������ݡ�
/// 3������ڽ�?�� Observable ת��Ϊ�����ӵ� Observable ֮ǰ����ʹ? Replay ����������?����������� Observable �����Ƿ����������������и��κ�δ���Ĺ۲��ߣ���ʹ��Щ�۲�������� Observable ��ʼ�������۲��߷�������֮��Ŷ��ġ�
/// 4����?�� replay ����?����ͨ�� Observable�������Խ���?���任����Ϊ�����������������ԭʼ Observable �����������Ϊ���������ؽ�� Observable Ҫ�����?�����ݡ���ˣ������������ʵ�� replay �任֮��������
/// 5��Replay ����������󶨲�����صĲ�����
/// 6��Replay ʾ�����룺
///     ---
///     IConnectableObservable<long> replayObservable = Observable.Interval(TimeSpan.FromSeconds(1.0f))
///                                             .Do(l => Debug.Log($"Observable:{l}"))//����ִ��
///                                             .Replay();// ���ز���ʽ��������
///     
///     replayObservable.Subscribe(l => Debug.Log($"Subscription #1:{l}"));// ���յ����ݺ�ִ�лص�
///     replayObservable.Connect();
///     
///     Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => replayObservable.Subscribe(l => Debug.Log($"Subscription #2:{l}")));
///     


///31��Connect ��������
/// 1����?�������ӵ� Observable ��ʼ�������ݸ�������
/// 2�������ӵ� Observable (connectable Observable) ����ͨ�� Observable ��࣬�������������ڱ�����ʱ��ʼ�������ݣ�?��ֱ��ʹ?�� Connect ������ʱ�ŻῪʼ��?���?��������Եȴ����еĹ۲��߶������� Observable ֮���ٿ�ʼ�������ݡ�
/// 3��Connect ����������󶨲�����صĲ�����
/// 


///32��Throw ��������
/// ��������?��������������?��������?�� Observable
/// ����ʾ�����룺
///     ---
///     //������: �����쳣: error
///     Observable.Throw<string>(new Exception("error")).Subscribe(_ => Debug.Log("�������"), e => Debug.Log($"�����쳣��{e.Message}"));
///     ---
///     


///33��Catch ��������
/// 1���� onError ֪ͨ�лָ���������
/// 2��Catch ����������ԭʼ Observable �� onError ֪ͨ�������滻Ϊ��������������������У��ò�?�� Observable �ܹ�������?���߸�������?����ĳЩ ReactiveX ��ʵ���У���?���� onErrorResumeNext �Ĳ�����������?Ϊ�� Catch ���ơ�
/// 3��ʾ�����룺
///     ---
///     // ������Ϊ: catched exception:error��1 ������ timer called
///     Observable.Throw<string>(new Exception("error"))
///         .Catch<string, Exception>(e =>
///         {
///             Debug.Log($"catched exception:{e.Message}");
///             return Observable.Timer(TimeSpan.FromSeconds(1.0f)).Select(_ => "timer called");
///         })
///         .Subscribe(result => Debug.Log(result));
///     ---
/// 4��Catch �����������쳣���������صĲ�����


///34��Finally ��������
/// 1��ע��?��������������?�� Observable ��?֮��ᱻ��?��?����������?�����쳣��?
/// 2��Finally �����������쳣���������صĲ�����
/// 3�� ʾ�����룺
///     ---
///     Subject<int> source = new Subject<int>();
///     IObservable<int> result = source.Finally(() => Debug.Log("Finally action ran"));// result �� source ��ֹʱ���¼�Դ
///     
///     result.Subscribe(number => Debug.Log($"OnNext({number})"), () => Debug.Log("OnCompleted()"));// ���� result���� result ����֪ͨ��ִ�лص�
///     source.OnNext(1);// ����֪ͨ
///     source.OnNext(2);
///     source.OnNext(3);
///     //source.OnError(new Exception());// �쳣��ֹ
///     source.OnCompleted();// ������ֹ
///     ---
///     


///35��DelaySubscription ��������
/// 1��ʾ�����룺
///     ---
///     Debug.Log(Time.time);
///     Observable.ReturnUnit().DelaySubscription(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log(Time.time));
///     ---
///     


///36��PairWise ��������
/// 1��Pair Wise����˫�ɶ�
/// 2��ʾ�����룺
///     ---
///     // Range(X,Y) ������ҿ���
///     // PairWise �������ڷ����һ�����ݵ�ʱ���ĵĻص�����ִ�У�ֻ�з��������������ݿ��Գ�˫�ɶ�ʱ�ص��Ż�����ִ��
///     Observable.Range(0, 10).Pairwise().Subscribe(pair => Debug.Log($"{pair.Previous} : {pair.Current}"));
///     ---
///     


///37��С�᣺
/// 1���ڱ���ѧϰ�� UniRx ��ԭ���� Rx.Net �Ĳ��ֲ���������Щ������?������?��ʵ?�ģ��ٲ��ֲ�̫��?�Ĳ��ֶ����ڽϺ��λ�ý����ܶ���������������ɬ�Ѷ�������??ȥ��?��ϰ?���?����?��
/// 2������?���п�ʼѧϰ UniRx ���еĲ��������ⲿ�ֲ�����ÿ����?��ʵ?����Ҫ����ѧϰ
#endregion



#region 3��UniRx ���еĲ�������

///1��
///


///2��
///


///3��
///


///4��BatchFrame ��������
/// 1��Batch �� ��һ��������������������˼��BatchFrame ���ǽ��ж�֡��������
/// 2��ʾ�����룺
///     ---
///     // ��?������ռ�ÿ 100 ֡�ڵĵ���¼���Ȼ���?ͳ?�����
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .BatchFrame(100, FrameCountType.EndOfFrame)
///         .Subscribe(clicks => Debug.Log(clicks.Count));
///         


///5��
///


///6��
///


///7��SampleFrame ��������
/// 1��Sample �ǡ�����������˼����������֡ȡһ�����ݵ���˼
/// 2������ʾ����
///     ---
///     //Observable.EveryUpdate().SampleFrame(5).Subscribe(_ => Debug.Log(Time.frameCount));
///     // ��ǰ���Ż��ֶΣ�ÿ�� 5 ֡����һ�� GC.Collect()
///     Observable.EveryUpdate().SampleFrame(5).Subscribe(_ => GC.Collect());
///


///8��
///
#endregion
public class OperatorNote : MonoBehaviour
{
    #region ����ϰ��
    class Student
    {
        public string name;
        public int age;
    }

    event Action OnClickEvent;
    #endregion

    private void Start()
    {
        #region ����ϰ��
        List<Student> students = new List<Student>()
        {
            new Student(){name = "����",age = 50},
            new Student(){name = "����",age = 51},
            new Student(){name = "����",age = 52},
            new Student(){name = "����",age = 59},
        };
        int[] grades = { 35, 46, 78, 98, 65, 87, 67, 45, 79 };
        string[] fruits = { "apple", "banana", "mango", "orange", "passionfruit", "grape" };
        #endregion


        #region 1��UniRx �� Linq �н�������Ĳ�������
        #region 1��Linq �� UniRx ��������
        /*List<int> testList = new List<int> { 1, 2, 3 };
        testList.ForEach(num => Debug.Log(num));// 1 2 3

        int firstNumber = testList.First();
        Debug.Log(firstNumber); // 1
        testList.Where(num => num > 1).ToList().ForEach(number => Debug.Log(number));// 2 3*/
        #endregion


        #region 2��֮ǰ��ѧ�� UniRx �������ܽ᣺(�޴���)

        #endregion


        #region 3��Where ��������
        /*//IEnumerable<Student> oldStudents = students.Where(student => student.age > 45);
        IEnumerable<Student> oldStudents = from oldStudent in students where oldStudent.age > 45 select oldStudent;
        foreach(var student in oldStudents)
        {
            Debug.Log(student.name);
        }

        //?�����¼�������
        //Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => { Debug.Log("�����"); }).AddTo(this);
        //UniRx Where ��ѯʽ��
        IObservable<long> mouseClickEventStreams = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonDown(0) select updateEvent;
        mouseClickEventStreams.Subscribe(_ => { Debug.Log("mouse clicked"); }).AddTo(this);*/
        #endregion


        #region 4��Select ��������
        /*
        //Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0)).Select(_ => "mouse up").Subscribe(Debug.Log).AddTo(this);
        //var mouseUpEventSteam = from updateEvent in Observable.EveryUpdate() where Input.GetMouseButtonUp(0) select "mouse up";
        //mouseUpEventSteam.Subscribe(Debug.Log).AddTo(this);
        */
        #endregion


        #region 5��First ������
        /*// ֱ�ӻ�ȡ��?��Ԫ��:
        //Student oldStudent = students.Where(student => student.age > 45).First();

        // ȡ��������?�����ĵ�?��Ԫ��:
        //Student oldStudent = students.First(student => student.age > 45);

        // LINQ First ��ѯʽ:
        Student oldStudent = (from student in students select student).First(student => student.age > 45);

        // UniRx First ʾ������:
        Observable.EveryUpdate().First(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("mouse down")).AddTo(this);
        Debug.Log(oldStudent.name);*/
        #endregion


        #region 6��Distinct ��������
        /*var names = new List<string>()
        {
            "����",
            "����",
            "����"
        };
        // LINQ Distinct ʾ�����룺
        //IEnumerable<string> distinctNames = names.Distinct();

        // LINQ Distinct ��ѯʽʾ�����룺
        IEnumerable<string> distinctNames = (from name in names select name).Distinct();
        foreach (var distinctName in distinctNames)
            Debug.Log(distinctName);

        // UniRx Distinct ʾ�����룺
        students.ToObservable().Distinct(student => student.name).Subscribe(student => Debug.Log(student.name)).AddTo(this);

        // UniRx �� Distinct �ڲ��� List ��ʱ�򣬳���?�� LINQ ��?�ֵ�����?ʽ�⣬��?�ִ�??���ض�����������
        IObservable<string> lectClickStream = Observable.EveryUpdate()
                                                .Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "left clicked");
        IObservable<string> rightClickStream = Observable.EveryUpdate()
                                                .Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "right clicked");
        Observable.Merge(lectClickStream, rightClickStream).Distinct().Subscribe(Debug.Log).AddTo(this);

        //���ϴ�����Ϊ:���ܵ�����ٴ�?���������?���Ҽ������ֻ���?�� ��left clicked�� �� ��right clicked��*/
        #endregion


        #region 7��Last ��������
        /*// LINQ Last ����ʾ��:
        *//*Student student = students.Last();
        Debug.Log(student.name);*//*

        // UniRx Last ����ʾ����
        students.ToObservable().Last(student => student.age == 50).Subscribe(student => Debug.Log(student.name)).AddTo(this);
        */
        #endregion


        #region 8��SelectMany ��������
        // LINQ SelectMany ʾ�����룺
        /*IEnumerable singleChars = students.SelectMany(student => student.name + ":" + student.age);
        foreach(object singleChar in singleChars)
        {
            Debug.Log(singleChar);
        }*/

        // SelectMany �����ã�����һ�� List<string> ���� nums �С�123������456������789�� �����ַ���
        // ͨ�� nums.SelectMany ֮�� (nums.SelectMany(n => n));
        // �õ�һ���¼���{'1','2','3','4','5','6','7','8','9'}
        // Ϊʲô string �ַ������Ա���ɢ���ַ�����Ϊ string �̳��� IEnumerable �ӿڣ��ǿɵ�����
        // ���ص� n ���Խ��в�����nums.SelectMany(n => n + ":");�õ�һ���¼���{'1',':','2',':','3',':','4',':','5',':'����}��ÿ����һ�� n������ n �����һ�� ':'
        // ����ֱ�ӷ��������ַ�����nums.SelectMany(n => "asd");�õ�һ���¼���{'a','s','d','a','s','d','a','s','d'����},ÿ����һ�� n�����������ַ� 'a','s','d'

        // UniRx SelectMany ����ʾ����
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


        #region 9��Take ��������
        // LINQ Take ����ʾ����
        /*int[] grades = { 59, 84, 69, 30, 59, 94, 83 };
        IEnumerable<int> topThreeGrades = grades.OrderByDescending(grade => grade).Take(3);//���������ȡǰ����
        foreach (int topThreeGrade in topThreeGrades)
            Debug.Log(topThreeGrade);*/

        // UniRx Take ʾ�����룺
        //this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Take(5).Subscribe(_ => Debug.Log(1));
        // ������Ϊ��ֻ��ǰ 5 ��?�����Ż���� 1

        /*// Take ���Դ���һ��ʱ��ν���
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// �����������
            .Take(TimeSpan.FromSeconds(5))// ֻ��ǰ�����ڴ�������������Ч
            .Subscribe(_ => Debug.Log("mouse clicked"))
            .AddTo(this);*/
        #endregion


        #region 10��Concat ��������
        //LINQ Concat ʾ�����룺
        /*List<Student> students2 = new List<Student>()
        {
            new Student(){name = "ddd", age = 20 },
            new Student(){name = "ccc", age = 60 },
            new Student(){name = "aaa", age = 30 },
        };
        var studentNames = students.Select(student => student.name).Concat(students2.Select(student => student.name));
        foreach (string studentName in studentNames)
            Debug.Log(studentName);*/

        // UniRx Concat ʾ�����룺
        /*IObservable<string> a = this.UpdateAsObservable().Take(3).Select(_ => "A");
        IObservable<string> b = this.UpdateAsObservable().Take(2).Select(_ => "B");
        IObservable<string> c = a.Concat(b);
        c.Subscribe(Debug.Log);*/
        #endregion


        #region 11��WhenAll:
        /*// LINQ All ʾ�����룺
        bool allStartWithB = students.All(student => student.name.StartsWith("B"));
        Debug.LogFormat("{0} student names start with 'B'.", allStartWithB ? "All" : "Not All");

        // UniRx WhenAll ʾ������(������ΪA��B��C�����˳��?��)��
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


        #region 12��OfType ��������
        // LINQ OfType ����ʾ��:
        /*ArrayList list = new ArrayList { 30, 30.0f, "test" };
        list.OfType<float>().ToList().ForEach(num => Debug.Log(num));*/

        // UniRx OfType ʾ�����룺
        // ����һ�� Subject(Observable)
        /*Subject<object> objects = new Subject<object>();

        // ���ĸ� Observable���������͹���(����֪ͨ)
        objects.OfType<object, string>().Subscribe(Debug.Log);//�� object ����תΪ string ����
        objects.OnNext(1);
        objects.OnNext(2);
        objects.OnNext("3");
        objects.OnNext(5);

        // �ֶ������㲥
        objects.OnCompleted();*/
        #endregion


        #region 13��Cast ��������
        // LINQ Cast ����ʾ����
        /*ArrayList fruits = new ArrayList { "mango", "apple", "lemon" };
        IEnumerable fruitNames = fruits.Cast<string>();
        foreach (var fruit in fruitNames)
            Debug.Log(fruit);*/

        // UniRx Cast ����ʾ����
        /*Subject<object> objects = new Subject<object>();
        objects.Cast<object, int>().Subscribe(i => Debug.Log(i));//�� object תΪ int ����

        // �ֶ���������(�㲥)
        objects.OnNext(1);
        objects.OnNext(2);
        objects.OnNext(3);
        //objects.OnNext("4");//����ת���쳣

        // �ֶ������㲥
        objects.OnCompleted();*/

        #endregion


        #region 14��GroupBy ��������
        // LINQ GroupBy ����ʾ����
        /*IEnumerable<IGrouping<string,Student>> studentGroupNames = students.GroupBy(student => student.name);
        foreach(IGrouping<string, Student> studentGroup in studentGroupNames)
        {
            Debug.Log($"Group Key:{studentGroup.Key}");
            foreach(Student student in studentGroup)
            {
                Debug.Log($"Name:{student.name} Age:{student.age}");
            }
        }*/

        // LINQ GroupBy ��ѯ���ʽ��
        /*Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(10).GroupBy(i => i % 3).Subscribe(group =>
        {
            group.Subscribe(number => Debug.Log($"key:{group.Key},number:{number}"));
        });*/
        #endregion


        #region 15��Range ��������
        // LINQ Range ����ʾ����
        /*IEnumerable<int> squares = Enumerable.Range(5, 5).Select(x => x * x);//�� 5 ��ʼ�����ǵ������� 5 6 7
        squares.ToList().ForEach(num => Debug.Log(num));*/

        // UnIRx Range ����ʾ����
        /*IObservable<int> squares = Observable.Range(5, 3).Select(x => x * x);
        squares.Subscribe(square => Debug.Log(square));*/
        #endregion


        #region 16��Skip ��������
        //LINQ Skip ����ʾ����
        /*int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };
        grades.OrderByDescending(g => g).Skip(3).ToList().ForEach(num => Debug.Log(num));//�������������ǰ����

        //UniRx Skip ����ʾ����
        this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Skip(5).Subscribe(_ => Debug.Log("mouse clicked"));
        //������Ϊ��?����������ʱ���ſ�ʼ�����mouse clicked��*/

        // Skip ���������Դ���һ��ʱ��ι�����
        /*Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// �����������
            .Skip(TimeSpan.FromSeconds(5))// ����ǰ���루ǰ�����ڵ������Ч�������ÿ�ε������Ч��
            .Subscribe(_ => Debug.Log("mouse clicked"))
            .AddTo(this);*/
        #endregion


        #region 17��TakeWhile ��������
        // LINQ TakeWhile ʾ�����룺
        //string[] fruits = new[] { "apple", "banana", "mango", "orange", "grape", "passionfruit" };
        //IEnumerable<string> fruitsAfterOrange = fruits.TakeWhile(fruit => fruit != "orange");
        //fruitsAfterOrange.ToList().ForEach(value => Debug.Log(value));

        //UniRx TakeWhile ����ʾ����
        //this.UpdateAsObservable().TakeWhile(value => !Input.GetMouseButtonDown(0)).Subscribe(_ => Debug.Log("before click"));
        /*Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButton(0))// ��갴�²���
            .TakeWhile((_, number) => !Input.GetMouseButtonUp(0) && number < 100)//�����û��̧���Ҵ���С�� 100,number ��ʾ�¼���������
            .Subscribe(_ => Debug.Log("mouse button clicking"));*/
        //��?���Ϊ��������� before mouse clicked����?����֮������� ��before mouse clicked��
        #endregion


        #region 18��SkipWhile ��������
        /*//LINQ SkipWhile ����ʾ����
        int[] grades = { 59, 33, 65, 78, 34, 89, 67, 98 };

        // �������������ǰ�ɼ����ڵ��� 80����������ֵ�������ǰ�ɼ�С�� 80������ʣ���ֵ
        IEnumerable<int> lowerGrades = grades.OrderByDescending(grade => grade).SkipWhile(grade => grade >= 80);
        lowerGrades.ToList().ForEach(value => Debug.Log(value));*/

        // UniRx SkipWhile ����ʾ����
        /*this.UpdateAsObservable()
            .SkipWhile(_ => !Input.GetMouseButtonDown(0))// ����갴��ʱ���ռ�֮����¼�
            .Subscribe(_ => Debug.Log("mouse button down"));
        // ʵ�ֵ��߼�Ϊ�������?��󣬳������ mouse button down*/
        #endregion


        #region 19��Zip (.Net 4) ��������
        // LINQ Zip ʾ����
        /*int[] numbers = { 1, 2, 3, 4 };
        string[] words = { "one", "two", "three", "four" };
        IEnumerable<string> numbersAndWords = numbers.Zip(words, (first, second) => $"{first} {second}");
        // ��� 1 one��2 two����
        numbersAndWords.ToList().ForEach(value => Debug.Log(value));*/

        // UniRx Zip ʾ�����룺
        /*IObservable<Unit> rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
        IObservable<Unit> leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1));
        leftStream.Zip(rightStream, (left, right) => Unit.Default).Subscribe(_ => Debug.Log("ok"));*/
        //ֻҪ���Ҽ��ɶԾͻ���� ok���Ȱ�����������ٰ������Ҽ���������� ok
        #endregion


        #region 20��Repeat ��������
        // LINQ Repeat ����ʾ����
        /*IEnumerable<string> strs = Enumerable.Repeat("I like programming.", 5);
        foreach (string str in strs)
            Debug.Log(str);*/

        // UniRx Repeat ����ʾ����
        // ��ʹ�� Repeat��һ������һ���ַ�����֮�������
        //Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log("after 1 seconds"));

        // ʹ�� Repeat��ÿ�����һ���ַ���(���¼����ظ�ִ��)
        //Observable.Timer(TimeSpan.FromSeconds(1.0f)).Repeat().Subscribe(_ => Debug.Log("after 1 seconds"));
        #endregion


        #region 21��TakeLast ��������
        // UniRx TakeLast ����ʾ����
        /*IObservable<int> bottonThreeGrades = grades.OrderByDescending(grade => grade).ToObservable().TakeLast(3);

        // ���� bottonThreeGrades �¼�������ĳ��ֵ���� bottonThreeGrades �¼���ִ�лص�������Ч�������� foreach��
        bottonThreeGrades.Subscribe(buttonThreeGrade => Debug.Log(buttonThreeGrade));*/

        // ����ʱ��ν� TakeLast ������
        /*Subject<float> subject = new Subject<float>();

        subject.TakeLast(TimeSpan.FromSeconds(2.0))// �ռ� subject �����������ݣ�ִ�лص�
            .Subscribe(clickTime => Debug.Log(clickTime));

        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// �����������ִ�лص�����
            .Subscribe(_ => subject.OnNext(Time.time));// �㲥��ǰ�¼�
        Observable.Timer(TimeSpan.FromSeconds(5.0f))// ����󴥷��¼���ִ�лص�����
            .Subscribe(_ => subject.OnCompleted());// �����ֹͣ subject �Ĺ㲥*/
        #endregion


        #region 22��Single/Default ��������
        // LINQ Single ����ʾ����
        /*string fruit1 = fruits.Single(fruit => fruit.Length > 10);
        Debug.Log(fruit1);*/

        //fruits.ToObservable().Single(fruit => fruit.Length > 10).Subscribe(Debug.Log);
        #endregion


        #region 23��ToArray ��������
        // LINQ ToArray ʾ�����룺
        /*string[] studentNames = students.Select(student => student.name).ToArray();
        foreach (var name in studentNames)
            Debug.Log(name);*/

        // UniRx ToArray ʾ�����룺
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


        #region 24��ToList ��������
        // LINQ ToList ʾ�����룺
        /*List<int> lengths = fruits.Select(fruit => fruit.Length).ToList();
        lengths.ForEach(value => Debug.Log(value));*/

        // UniRx ToList ʾ�����룺
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


        #region 25��Aggregate ��������
        // LINQ Aggregate ����ʾ����
        /*int[] numbers = new[] { 1, 2, 3, 4, 5 };
        int result = numbers.Aggregate((total, next) => total * next);
        Debug.Log($"5 �Ľ׳�Ϊ��{result}");// ���� 120���� 1 * 2 * 3 * 4 * 5*/

        // UniRx Aggregate ����ʾ����
        //Observable.Range(0, 8).Aggregate(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));

        /*// Aggregate ������ȡ���ֵ����Сֵ����
        int[] numbers = new[] { 1, 2, 3, 4, 5 };
        int result = numbers.Aggregate((total, next) => total = next > total ? next : total);
        Debug.Log($"���ֵΪ{result}");*/
        #endregion


        #region 26��Empty ������
        // LINQ Empty ʾ�����룺
        /*string[] names1 = { "Hartono, Tommy" };
        string[] names2 = { "Adams, Terry", "Andersen, Henriette Thaulow", "Hedlund, Magnus", "Ito, Shu" };
        string[] names3 = { "Solanki, Ajay", "Hoeing, Helge", "Andersen, Henriette Thaulow", "Potra, Cristina", "Iallo, Lucio" };
        var namesList = new List<string[]> { names1, names2, names3 };
        var allNames = namesList.Aggregate(Enumerable.Empty<string>(),(current, next) => next.Length > 3 ? current.Union(next) : current);
        foreach (var name in allNames)
        {
            Debug.Log(name);
        }*/

        // UniRx Empty ����ʾ����
        /*IObservable<Unit> s = Observable.Empty<Unit>();
        s.Subscribe(e => Debug.Log($"e:{e}"), () => Debug.Log("OnCompleted"));*/
        #endregion


        #region 27��С�᣺(�޴���)

        #endregion
        #endregion


        #region 2��UniRx �� Rx.Net �н���Ĳ�����(Rx ���еĲ�����)��
        #region 1��Interval ��������
        // ÿ�� 1 �봥��һ���¼�
        //Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(second => Debug.Log($"��ǰʱ�䣺{second} s")).AddTo(this);

        // ����Ч��Ҳ��ʹ�� Repeat ʵ��
        //Observable.Timer(TimeSpan.FromSeconds(1)).Repeat().Subscribe()).AddTo(this);
        #endregion


        #region 2��TakeUntil ��������
        // ������Ϊ��������� 123�������?�������ͣ?��� 123
        /*this.UpdateAsObservable()//��һ�� Observable
            .TakeUntil(Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)))// �������ǵڶ��� Observable
            .Subscribe(_ => Debug.Log(123))
            .AddTo(this);*/
        #endregion


        #region 3��SkipUntil ��������
        // ������Ϊ�����?�����֮��Ϳ�ʼ������� ��?�갴���ˡ�
        /*IObservable<Unit> clickStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
        this.UpdateAsObservable()
            .SkipUntil(clickStream)// �� clickStream ��������֮ǰ��������ǰ�¼�
            .Subscribe(_ => Debug.Log("��갴����"))
            .AddTo(this);*/
        #endregion


        #region 4��Buffer ������(����)��
        /*Observable.Interval(TimeSpan.FromSeconds(1.0f))// ÿ��һ�뷢��һ������
            .Buffer(TimeSpan.FromSeconds(3.0f))// ���� 3 ���ڵ�����
            .Subscribe(_ => Debug.Log($"currentTime:{DateTime.Now.Second}"))// �������ݺ�ִ�лص�����
            .AddTo(this);*/
        #endregion


        #region 5��Throttle(������)��������
        /*//�����������?��� 1 ���ڲ��ٵ����������е�������¼�ʱ 1 �������
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))// ��������ʱ
            .Throttle(TimeSpan.FromSeconds(1))//һ����ٵ����ִ�лص����������¼�ʱһ�룬ֱ�����¼�ʱ��ʼһ����ٵ����ִ�лص�
            .Subscribe(_ => Debug.Log("һ�����"));*/
        #endregion


        #region 6��Delay ��������
        /*// �����������?�� 1 ��֮����� mouse clicked��ÿ�ε���¼����� 1 ��֮������
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Delay(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => Debug.Log("mouse clicked"))
            .AddTo(this);*/
        #endregion


        #region 7��Return ��������
        /*Observable.Return("hello").Subscribe(Debug.Log);//ִֻ��һ�Σ����� set*/
        #endregion


        #region 8��Timer ��������
        // ִ?������� 5 ������ ��after 5 seconds��
        //Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => Debug.Log("after 5 seconds")).AddTo(this);
        /*Observable
            .Timer(TimeSpan.FromSeconds(1.0f),TimeSpan.FromSeconds(2.0f))// ��һ��֮��ÿ�����뷢��һ������
            .Subscribe(_ => Debug.Log("after 5 seconds"))
            .AddTo(this);*/
        #endregion


        #region 9��Sample ��������
        /*Observable.Interval(TimeSpan.FromMilliseconds(50))//ÿ�� 50 ���뷢��һ������
            .Sample(TimeSpan.FromSeconds(1))// ÿ��һ�����һ�β����������ǰ������������֮ǰ�Ѿ��ɹ����ˣ���ô�Ͳ��ٽ��в�����ֱ����������������֮ǰÿ�������ģ��Ž��в���
            .Subscribe(_ => Debug.Log(DateTime.Now.Second))
            .AddTo(this);

        int clickCount = 0;
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => clickCount++)
            .Sample(TimeSpan.FromSeconds(3))
            .Subscribe(_ => Debug.Log($"�� {clickCount} �ε��"))
            .AddTo(this);*/
        #endregion


        #region 10��Timestamp ��������
        /*Observable.Interval(TimeSpan.FromSeconds(1.0f))
            .Timestamp()
            //.Subscribe(timestamp => Debug.Log(timestamp))
            .Subscribe(timestamp => Debug.Log(timestamp.Timestamp.LocalDateTime))//�������ʱ��
            .AddTo(this);*/
        #endregion


        #region 11��ThrottleFirst ��������
        //��?�����?����֮��?����� ��Clicked�������֮��� 5 ���ڵ��?Ч
        /*this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .ThrottleFirst(TimeSpan.FromSeconds(5))
            .Subscribe(x => Debug.Log("Clicked"))
            .AddTo(this);*/
        #endregion


        #region 12��TimeInterval ��������
        /*Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => "clicked")
            .TimeInterval()
            .Subscribe(timeInterval => Debug.Log($"{timeInterval.Interval} : {timeInterval.Value}"))
            .AddTo(this);*/
        #endregion


        #region 13��Defer ��������
        /*System.Random random = new System.Random();
        Observable
            .Defer(() => Observable.Start(() => random.Next()))//Observable.Start(() => random.Next()) �Ὺ��һ���̴߳���һ���µ� Observable��Ȼ������һ�������
            .Delay(TimeSpan.FromMilliseconds(1000))
            .Repeat()
            .Subscribe(randomNumber => Debug.Log(randomNumber))// ÿ�ζ���ʱ��Defer �ᴴ��һ�� Observable 
            .AddTo(this);*/
        #endregion


        #region 14��Never ��������
        /*//?ǰ����֪��?��?��û��������
        IObservable<string> never = Observable.Never<string>();
        // ������û�й㲥/֪ͨ�� Subject
        Subject<string> subject = new Subject<string>();*/
        #endregion


        #region 15��Scan ��������
        //Observable.Range(0, 8).Scan(0, (acc, currentValue) => acc + 5).Subscribe(xx => Debug.Log(xx));
        #endregion


        #region 16��Switch ��������
        /*// ִ?�����������?��ʱ��� ��mouse button down�� ̧��֮����� ��mouse button up��
        var buttonDownStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
        var buttonUpStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonUp(0));
        buttonDownStream.Select(_ =>
            {
                Debug.Log("mouse button down");
                return buttonUpStream;
            })
            .Switch()// �л��¼����� buttonUpStream
            .Subscribe(_ => Debug.Log("mouse button up"))// buttonUpStream �¼����Ļص�����
            .AddTo(this);*/

        /*// ʹ�� Switch ������ʵ�֡��ʵ�������:����˳���� W A S D ʱ�����ʵ�
        var wObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.W));
        var aObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.A));
        var sObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.S));
        var dObservable = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.D));

        wObservable.Select(_ => aObservable)
            .Switch()// �л��� aObservable �¼���
            .Select(_ => sObservable)
            .Switch()// �л��� sObservable �¼���
            .Select(_ => dObservable)
            .Switch()// �л��� dObservable �¼���
            .Repeat()
            .Subscribe(_ => Debug.Log("�����ʵ�"))
            .AddTo(this);*/
        #endregion


        #region 17��StartWith ��������
        /*Observable.Return("sikiedu.com")
            .StartWith("http://")
            .Aggregate((current, next) => current + next)// ���û�� Aggregate ������������� "http://"��������ƴ�ӳ�һ���ַ���
            .Subscribe(Debug.Log)
            .AddTo(this);*/
        #endregion


        #region 18��CombineLatest ��������
        /*int a = 0;
        int i = 0;
        var leftStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => (++a).ToString());
        var rightStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => (++i).ToString());
        leftStream.CombineLatest(rightStream, (left, right) => left + right).Subscribe(Debug.Log);*/
        #endregion


        #region 19��Do ��������
        /*Observable.ReturnUnit().Delay(TimeSpan.FromSeconds(1.0f))
            .Do(_ => Debug.Log("after 1 seconds"))
            .Delay(TimeSpan.FromSeconds(1.0f))
            .Do(_ => Debug.Log("after 2 seconds"))
            .Delay(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => Debug.Log("after 3 seconds"))
            .AddTo(this);*/
        #endregion


        #region 20��Merge ��������
        /*// �����������?�������� ��A�������?���Ҽ���� ��B��
        var aStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0)).Select(_ => "A");
        var bStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(1)).Select(_ => "B");
        aStream.Merge(bStream).Subscribe(Debug.Log);*/
        #endregion


        #region 21��Materialize/Dematerialize ��������
        /*Subject<int> subject = new Subject<int>();
        IObservable<int> onlyExceptions = subject.Materialize().Where(n => n.Exception != null).Dematerialize();

        // �� subject ����ע��
        subject.Subscribe(i => Debug.Log($"Subscriber 1:{i}"), ex => Debug.Log($"Subscriber 1 exception: {ex.Message}"));

        // �� Dematerialize ����ע��
        onlyExceptions.Subscribe(i => Debug.Log($"Subscriber 2:{i}"), ex => Debug.Log($"Subscriber 2 exception: {ex.Message}"));
        subject.OnNext(123);
        subject.OnError(new Exception("Test Exception"));*/
        #endregion


        #region 22��IgnoreElements ��������
        /*Subject<int> subject = new Subject<int>();
        IObservable<int> noElements = subject.IgnoreElements();

        subject.Subscribe(i => Debug.Log($"subject.OnNext({i})"), () => Debug.Log("subject.OnCompleted()"));
        noElements.Subscribe(i => Debug.Log($"noElements.OnNext({i})"), () => Debug.Log("noElements.OnCompleted()"));

        subject.OnNext(1);
        subject.OnNext(2);
        subject.OnNext(3);
        subject.OnCompleted();*/
        #endregion


        #region 23��DistinctUntilChanged ��������
        /*Subject<int> subject = new Subject<int>();
        IObservable<int> distinct = subject.DistinctUntilChanged();// subject ��ֵ�����ı�ʱ�������ݵ��¼���

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


        #region 24��Create ��������
        /*Observable.Create<int>(observable =>
        {
            observable.OnNext(1);
            observable.OnNext(2);
            observable.OnCompleted();
            return Disposable.Create(() => Debug.Log("�۲�����ȡ������"));
        }).Subscribe(number => Debug.Log(number)).AddTo(this);*/
        #endregion


        #region 25��Amb ��������
        /*Observable.Amb(
            Observable.Timer(TimeSpan.FromSeconds(3)).Select(_ => "3 sec"),
            Observable.Timer(TimeSpan.FromSeconds(10)).Select(_ => "10 sec"),
            Observable.Timer(TimeSpan.FromSeconds(2)).Select(_ => "2 sec"),// ֻ�������ȷ�������(��֪ͨ)�� Observable ����������
            Observable.Timer(TimeSpan.FromSeconds(22)).Select(_ => "30 sec"),
            Observable.Timer(TimeSpan.FromSeconds(6)).Select(_ => "5 sec"))
            .Subscribe(s => Debug.Log($"OnNext:{s}"), () => Debug.Log("OnCompleted"))
            .AddTo(this);*/
        #endregion


        #region 26��Timeout ��������
        /*// ��?�������?����û���κβ������ᱨ�쳣
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(1))
            .Take(10)
            .Timeout(TimeSpan.FromSeconds(1.0))
            .Subscribe(_ => Debug.Log("clicked"))
            .AddTo(this);*/

        /*//���ڵȴ����磺
        ObservableWWW.Get("http://sikiedu.com")
            .Timeout(TimeSpan.FromSeconds(5.0f))
            .Subscribe(_ => { }, e => Debug.Log($"�����쳣:{e.Message}"))
            .AddTo(this);*/
        #endregion


        #region 27��FromEvent ��������
        /*// ����������ÿ�ε��ʱ����� button clicked
        GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(() => OnClickEvent());
        Observable.FromEvent(action => OnClickEvent += action, action => OnClickEvent -= action)
            .Subscribe(_ => Debug.Log("button clicked"))
            .AddTo(this);*/
        #endregion


        #region 28��Publish ��������
        /*IObservable<int> unshared = Observable.Range(1, 4);

        // ÿ����������һ���¶���
        unshared.Subscribe(i => Debug.Log($"Unshared Subscription #1:{i}"));// 1 2 3 4 ����������������
        unshared.Subscribe(i => Debug.Log($"Unshared Subscription #2:{i}"));// 1 2 3 4 ����������������

        // ͨ��ʹ�� Publish �����������ı�ɹ���ģ����ڵ��� Connect() ֮ǰ���Ὺʼ����
        var shared = unshared.Publish();
        shared.Subscribe(i => Debug.Log($"shared Subscription #1:{i}"));
        shared.Subscribe(i => Debug.Log($"shared Subscription #2:{i}"));
        shared.Connect();// �� Connect ��ʱ�򣬲Żᷢ������*/
        #endregion


        #region 29��RefCount ��������
        /*TimeSpan period = TimeSpan.FromSeconds(1);

        IObservable<long> observable = Observable
            .Interval(period).Do(l => Debug.Log($"Publishing:{l}"))
            .Publish()
            .RefCount();

        Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ =>
        {
            Debug.Log("@@@@ subscribe @@@@");
            // Observable.Connect();// ���� RefCount
            IDisposable subscription = observable.Subscribe(i => Debug.Log($"subscription:{i}"));
            Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(__ =>
            {
                Debug.Log("@@@@ unSubscribe @@@@");
                subscription.Dispose();// ���֮ǰ���ĵ��¼���Ȼ�����涩���µ��¼�
                observable.Subscribe(i => Debug.Log($"subscription 2: {i}"));
            });
        });

        // ����֮�⻹��һ�� Share ���������� Publish().RefCount() һ��
        *//*public static IObservable<T> Share<T>(this IObservable<T> source)
        {
            return source.Publish().RefCount();
        }*/
        #endregion


        #region 30��Replay ��������
        /*IConnectableObservable<long> replayObservable = Observable.Interval(TimeSpan.FromSeconds(1.0f))
                                        .Do(l => Debug.Log($"Observable:{l}"))//����ִ��
                                        .Replay();// ���ز���ʽ��������

        replayObservable.Subscribe(l => Debug.Log($"Subscription #1:{l}"));// ���յ����ݺ�ִ�лص�
        replayObservable.Connect();

        Observable.Timer(TimeSpan.FromSeconds(5.0f)).Subscribe(_ => replayObservable.Subscribe(l => Debug.Log($"Subscription #2:{l}")));*/
        #endregion


        #region 31��Connect ��������(�޴���)

        #endregion


        #region 32��Throw ��������
        /*//������: �����쳣: error
        Observable.Throw<string>(new Exception("error")).Subscribe(_ => Debug.Log("�������"), e => Debug.Log($"�����쳣��{e.Message}"));*/
        #endregion


        #region 33��Catch ��������
        /*// ������Ϊ: catched exception:error��1 ������ timer called
        Observable.Throw<string>(new Exception("error"))
            .Catch<string, Exception>(e =>
            {
                Debug.Log($"catched exception:{e.Message}");
                return Observable.Timer(TimeSpan.FromSeconds(1.0f)).Select(_ => "timer called");
            })
            .Subscribe(result => Debug.Log(result));*/
        #endregion


        #region 34��Finally ��������
        /*Subject<int> source = new Subject<int>();
        IObservable<int> result = source.Finally(() => Debug.Log("Finally action ran"));// result �� source ��ֹʱ���¼�Դ

        result.Subscribe(number => Debug.Log($"OnNext({number})"),() => Debug.Log("OnCompleted()"));// ���� result���� result ����֪ͨ��ִ�лص�
        source.OnNext(1);// ����֪ͨ
        source.OnNext(2);
        source.OnNext(3);
        //source.OnError(new Exception());// �쳣��ֹ
        source.OnCompleted();// ������ֹ*/
        #endregion


        #region 35��DelaySubscription ��������
        /*Debug.Log(Time.time);
        Observable.ReturnUnit().DelaySubscription(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => Debug.Log(Time.time));*/
        #endregion


        #region 36��PairWise ��������
        /*Observable.Range(0, 10).Pairwise().Subscribe(pair => Debug.Log($"{pair.Previous} : {pair.Current}"));*/
        #endregion


        #region 37��С�᣺(�޴���)

        #endregion
        #endregion


        #region 3��UniRx ���еĲ�������

        #region 4��BatchFrame ��������
        /*// ��?������ռ�ÿ 100 ֡�ڵĵ���¼���Ȼ���?ͳ?�����
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .BatchFrame(100, FrameCountType.EndOfFrame)
            .Subscribe(clicks => Debug.Log(clicks.Count));*/
        #endregion

        #region 7��SampleFrame ��������
        /*// Sample �ǡ�����������˼����������֡ȡһ�����ݵ���˼
        Observable.EveryUpdate().SampleFrame(5).Subscribe(_ => Debug.Log(Time.frameCount));

        // ��ǰ���Ż��ֶΣ�ÿ�� 5 ֡����һ�� GC.Collect()
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
