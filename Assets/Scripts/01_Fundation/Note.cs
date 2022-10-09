using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;



/// 1、什么是 UniRx：
///     1、UniRx 是一个 Unity3D 的编程框架，专注于解决异步逻辑，使异步逻辑的实现更加简洁优雅
///     2、UniRx 全称 Unity Reactive Extensions，是 Unity 版本的 Reactive Extensions
///     3、Reactive Extensions 擅长处理时间上异步的逻辑
///     4、Rx 只是一套标准，在其它语言上也有实现，如果在 Unity 中熟悉了这套标准，在其它语言上也是可以很快上手的，比如 RxJava、Rx.cpp、SwiftRx 等等


/// 2、为什么要使用 UniRx？
///     1、UniRx 的优点：
///         1、如下实现了 UniRx 处理“第一次鼠标点击事件”功能，如果使用传统的方式实现，代码量很大，还要创建一个成员变量来记录点击次数并在 Update 方法中监听鼠标抬起事件，而且 Update 方法还要处理很多其它逻辑，会使代码臃肿，不易阅读
///         2、UniRx 提供了一种编程思维，可以轻松搞定平时比较难实现的异步逻辑，代码简介，具有可读性。
///         3、除此之外，UniRx 还能：
///             1、优雅实现 MVP(MVC) 架构
///             2、对 UGUI/Unity API 提供了增强，很多需要大量代码的 UI 逻辑都可以使用 UniRx 优雅实现
///             3、轻松实现非常复杂的异步任务处理
///             4、……
///         4、UniRx 最重要的是：它不仅可以提高我们的编码效率，还给我们的大脑提供了一个强有力的编程模型
///         5、UniRx 非常值得研究学习，就连大名鼎鼎的 uFrame 框架在 1.6 版本之后也使用了 UniRx 做了大幅重构，底层使用 UniRx 强力驱动；凉鞋老师的 QFramework 也引入了 UniRx，有一大批的框架用户都是为了 UniRx 慕名而来
///     2、如果不使用 UniRx，在游戏开发中会面临的难题：
///         1、游戏系统很多在时间上是异步的，Unity 开发者要实现的异步任务是非常多的，比如动画的播放、声音的播放、网络请求、资源加载/卸载、Tween、场景过渡等都是在时间上异步的逻辑（这也是为什么 Unity 官方提供了 Coroutine（协程）的概念），甚至游戏循环(Every Update,OnCollisionEnter等)、传感器数据(Kinect, Leap Motion,VR Input等)等也是时间上异步的事件。我们在进行以上逻辑实现的时候经常需要用到大量回调，最终随着项目的扩展会导致传说中的“回调地狱”。
///         2、针对以上问题较好的方法是使用“消息/事件”的发送，但随着项目扩张会导致“消息满天飞”的现象，代码非常难以阅读
///         3、使用协程解决上述问题也是非常不错的，但是 Coroutine 本身的定义是以一个方法的格式定义的，写法是非常面向过程的，当逻辑稍微复杂一些就很容易造成 Coroutine 嵌套 Coroutine 的现象，代码非常不容易阅读（强耦合）
///         4、UniRx 的出现很好地解决了上述问题，它介于回调和事件之间，有事件的概念，只不过它的事件是像水一样六过来的，我们要做的只是简单地进行组织、变换、过滤、合并。
///             1、例如以下 UniRx 监听鼠标点击的例子，它也用到了回调，只不过事件组织之后只有一个简单的回调就可以进行事件的处理了，它的原理和 Coroutine（迭代器模式）非常类似，但是比 Coroutine 强大得多
///             2、UniRx 将（时间上）异步事件转化为（响应式的）时间序列，可以通过 LINQ 操作简单地组织起来，此外还支持事件操作
///     3、总结：为什么要用 UniRx？
///         --游戏本身有大量（时间上的）异步逻辑，而 UniRx 恰好擅长处理（时间上的）异步逻辑，使用 UniRx 不仅可以节省事件，还呢个让代码更加简洁易读
///     


///3、定时功能（定时器）的实现：
/// 1、常规方法：使用变量累加 Time.deltaTime，然后在 Update 方法中进行逻辑判断
/// 2、Coroutine：定义协程，然后在 Start 方法中开启协程
/// 3、UniRx：在 Start 方法中一行代码就能实现


///4、独立的 Update：
/// 1、MonoBehavior 中的 Update 函数一般会处理大量的逻辑，而这些逻辑之间很多都是互不关联的，例如处理角色的运动的同时，可能还会处理一些计时功能，使代码臃肿不易阅读
/// 2、使用 UniRx 可以创建独立的与 MonoBehavior Update 函数功能相同的函数


///5、AddTo：
/// 1、字面理解：添加到
/// 2、问题：添加到哪里？
///     --其实就是添加到 Unity 的 GameObject 或者 MonoBehaviour
/// 3、问题：为什么要添加到 GameObject 或者 MonoBehaviour？
///     --因为 GameObject 和 MonoBehaviour 可以获取到 OnDestroy 销毁事件
/// 4、问题：跟 OnDestroy 事件有什么关系？
///     --UniRx 需要通过 GameObject 和 MoveBehavior 获取到 OnDestroy 销毁事件来与 UniRx 进行销毁事件的绑定，当 GameObject 或 MonoBehavior 被销毁时，同样销毁正在进行的 UniRx 任务
/// 5、总结：AddTo 的作用：将当前的 UniRx 观察者与某个游戏对象(GameObject)或组件(MonoBehavior)进行销毁事件的绑定，当游戏对象(GameObject)或组件(MonoBehavior)被销毁时，当前的观察者也一起被销毁
/// 6、用法：Observable.Timer(TimeSpan.FromSeconds(1.0f))
///         .Subscribe()
///         .AddTo(MonoBehaviour/GameObject);
/// 7、为什么？
///     --AddTo 是一个静态扩展关键字，对 IDisposable 进行了扩展，只要实现了 IDisposable 接口，都可以使用 AddTo API，不管是不是 UniRx 的 API，当 GameObject 销毁时，就会调用 IDisposable 的 OnDIspose 方法
/// 8、AddTo 能用来做什么？
///     --有了 AddTo，在开启 Observable.EveryUpdate 调用当前脚本的方法时不会造成引用异常等错误（如果没有 AddTo，当前脚本被销毁后，Observable 依旧在运行，依旧在调用当前脚本的方法，会造成引用异常），使 UniRx 的使用更加安全


///6、UniRx 基本语法格式：
/// 1、Observable.XXX().Subscribe() 是非常典型的 UniRx 格式，只要裂解什么意思就可以看懂大部分 UniRx 代码了，如：
///     --Observable.Timer(TimeSpan.FromSeconds(1.0f))
///         .Subscribe()
///         .AddTo(MonoBehaviour/GameObject);
/// 2、概念/词汇：
///     1、Observable：可观察的，形容词，用于形容后面的名词，如 Observable.Timer 可简单理解为 “可观察的计时器”，一般我们会把 Observable 理解为 观察者，后边的名词理解为发布者
///     2、Timer：定时器，被 Observable 描述，是发布者，即事件的发送方，它的参数表示某一个事件，当达成某一个事件，Timer 就会发送事件触发的消息
///     3、Subscribe：订阅，动词，表示订阅前边的 Timer，订阅定时器的事件，所以 Subscribe 可以理解为订阅者，也就是事件的接收者，会接受到 Timer 发送的消息。
///     4、AddTo：将当前整行代码所代表的流程和某个游戏对象(GameObject)或组件(MovoBehavior)的销毁事件绑定到一起，当他们被销毁时，当前整行代码的任务也跟着销毁
///     5、连起来就是：订阅可以被观察的定时器
/// 3、注意：虽然从发布者和订阅者的角度更好理解 UniRx，但是 UniRx 的侧重点并不在发布者和订阅者这两个概念如何使用，而是事件从发布者到订阅者之间的过程如何处理，要关注的不是发布者和订阅者，而是他们之间的那条线，即事件传递的过程（暂且了解这些，再深入的内容后续慢慢理清）


///7、操作符 Where：
/// 1、在 “6、UniRx 基本语法格式” 中提到：UniRx 的侧重点不在发布者和订阅者这两个概念本身，而在于事件从发布者到订阅者之间的过程是如何处理的。那么，是如何处理的呢？先看一段代码：
///     --
///     Observable.EveryUpdate()
///         .Subscribe(_ => 
///         {
///           if(Input.GetMouseButtonUp(0))
///           {
///             //do something
///           }
///         });
///     --这段代码实现了鼠标的点击事件处理，但这段代码还不够简介,可以使用 Where 操作符使其更整洁：
///     --Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonUp(0))
///         .Subscribe(_ =>
///         {
///             //do something
///         }
/// 2、Where 可以理解成 if 语句，用于判断，主要做的是一个过滤操作，过滤掉不满足条件的事件
/// 3、代码解释：
///     1、EveryUpdate：事件的发布者，每帧发送一个事件过来
///     2、Subscribe：事件的接受者，接受 EveryUpdate 发送的事件
///     3、Where：是事件的发布者和接收者之间的一个过滤操作，会过滤掉不满足条件的事件，所以 Subscribe 处理的事件都是满足 Where 条件的事件
/// 4、事件的本身可以是参数，但是 EveryUpdate 没有参数，所以在 Where 这行代码中不需要接受参数，所以使用“_”来表示不用参数，既然发送不用参数，那么 Subscribe 也是用一个“_”来接受参数


///8、操作符 First：
/// 1、First() 操作符表示“只过滤第一次满足条件的事件”
/// 2、有如下代码：
///     --
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .First()
///         .Subscribe(_ =>
///         {
///             //do something
///         })
///         .AddTo(this);
///     --Observable.EveryUpdate 每帧会发布一个事件过来
///     --Where 操作符会对 Observable.EveryUpdate 发送的事件进行过滤，只有满足条件的事件才会接着往下传递
///     --First 操作符会对 Where 过滤的操作符继续过滤，只有 Where 过滤的第一个事件才会被传递下去
///     --Subscribe 只会接收到一个满足 Where 条件的事件，而且这个事件是第一个满足 Where 条件的事件
///     --AddTo：将当前整行代码与某个游戏对象 GameObject 或某个组件 MonoBehaviour 的销毁事件绑定到一起（共存亡）
///     --当然，代码还可以更简洁：
///     Observable.EveryUpdate()
///         .First(_ => Input.GetMouseButtonDown(0))
///         .Subscribe(_ =>
///         {
///             //do something
///         })
///         .AddTo(this);


///9、对 UGUI 的支持：
/// 0、原理：对 UnityEvent 提供了 AsObservable 方法
/// 1、UniRx 对 UGUI 进行了支持:
/// 
///     --比如最常用的按钮点击事件注册:
///     Button button = GameObject.Find("09_Button").transform.GetComponent<Button>();
///     button.OnClickAsObservable()
///         .Subscribe(_ =>
///         {
///             Debug.Log("do something");
///         });
///     --当然，按钮也可以使用 First 操作符让这个按钮只能被点击一次
///         
///     --Toggle:
///     Toggle toggle = GameObject.Find("09_Toggle").transform.GetComponent<Toggle>();
///     toggle.OnValueChangedAsObservable()
///         .Subscribe(on =>//on 表示 Toggle 是否被选中
///         {
///             Debug.Log(on);
///         });
///     --当然，Toggle 也可以使用 Where 操作符        
///     toggle.OnValueChangedAsObservable()
///         .Where(on => on)//只处理 on 为 True 的事件
///         .Subscribe(on =>
///         {
///             Debug.Log(on);
///         });
///     
///     ……
/// 2、UniRx 还支持 EventSystem 的各种 Trigger 接口的监听：
///     1、比如，Image 本身是 Graphic 类型的，Graphic 类只要实现 IDragHandler 就可以进行拖拽事件监听(Graphic 类型组件中会有一个 Raycast target 属性，改属性勾选之后就可以用于拖拽了，Text 组件也有这个属性)
///     2、但是使用 UniRx 进行拖拽事件的监听就不用那么麻烦--代码如下：
///         using UniRx.Triggers;//需要先导入此命名空间
///         image.OnBeginDragAsObservable().Subscribe(_ => {})
///         image.OnDragAsObservable().Subscribe(eventArgs => {})
///         image.OnEndDragAsObservable().Subscribe(_ => {});
///         //注意，一定要对以上三个事件都进行监听，如果只监听其中一个会没有效果
///     
/// 3、Unity 的 Event 也可以使用 AsObservable 进行订阅：
///     UnityEvent event;
///     void Start()
///     {
///         event.AsObservable()
///             .Subscribe(_ =>
///             {
///                 //process event
///             });
///     }
/// 4、总之，UniRx 对 Unity 做了非常多的增强


///10、Reactive Property 响应式属性：
/// 1、响应式属性 Reactive Property 是 UniRx 一个非常强大的概念
/// 2、响应式属性的强大之处在于它可以代替一切变量，并且给变量创造了很多功能--比如监听一个变量的值是否发生改变：
///     1、正常实现方式：
///     
///         private int age;
///         public int Age
///         {
///             get{ return age;}
///             set
///             {
///                 //监听值的改变
///                 if(age != value)
///                 {
///                     age = value;//值发生改变
///                     OnAgeChange();//调用方法
///                 }
///             }
///         }
///     
///         public void OnAgeChange()
///         {
///             Debug.Log("年龄发生改变");
///         }
///     
/// 
///     2、使用委托实现：
///         --以上当值发生改变时候调用的方法在类的内部，如果想在类的外部监听这个值的改变，就需要使用委托了，委托的成本比较低，是可以接受的
///         public Action<int> onAgeChange = null;
///         private int age = 0;
///         public int Age
///         {
///             get {return age};
///             set
///             {
///                 if(age != value)
///                 {
///                     age = value;
///                     if(OnAgeChanged != null)
///                         OnAgeChange(value);
///                 }
///             }
///         }
///     
///         void Start()
///         {
///             OnAgeChanged += age =>{ Debug.Log("年龄改变为" + Age);}
///         }
///             
/// 
///     3、使用 UniRx 实现：
///         //响应式属性 Age，初始值为 0
///         //public ReactiveProperty<int> Age = new ReactiveProperty<int>(0);//泛型序列化很麻烦
///         public IntReactiveProperty Age = new IntReactiveProperty(0);//使用 IntReactiveProperty 效果一样但序列化方便
///         private void Start()
///         {
///             Debug.Log("初始年龄是：" + Age);
///             Age.Subscribe(age => { Debug.Log("现在的年龄是：" + age); });
///         }
///     
/// 3、使用响应式属性来监听 Age的好处在于：
///     1、Age 可以被 Subscribe 多次
///     2、支持 First、Where 等操作符
///     3、可以用于实现 MVP 架构模式，在 Controller 中进行 Model 和 View 的绑定，Model 的所有属性都用 ReactiveProperty，然后在 Controller 中进行订阅，通过 View 更改 Model 的属性值，形成一个 View->Controller->Model->Controller->View 这么一个事件响应环


///11、MVP 框架的实现（MVP 是 MVC 框架的改良版）：
/// 1、在 “9、对 UGUI 的支持”中讲过，UniRx 对 UGUI 增强的原理很简单，就是对 UnityEvent 提供了 AsObservable 方法
///     --如：
///     Button button = GameObject.Find("Button").transform.GetComponent<Button>();
///     button.OnClick.AsObservable().Subscribe(_ => Debug.Log("clicked"));
///     --AsObservable 方法源码如下：
///     public static IObservable<Unit> OnClickAsObservable(this Button button)
///     {
///         return button.onClick.AsObservable();
///     }
///     --可见，调用的是 button 的 onClick 的 AsObservable 方法，即 UniRx 对 UGUI 的增强就是对 UnityEvent 提供了 AsObservable 方法
///     --在此基础上，进一步对每个 UGUI 控件进行封装，从而可以像如下方式在 UGUI 中使用 UniRx
///         //当 Toggle 值改变时让 button 可交互
///         toggle.OnValueChangeAsObservable().SubscribeToInteractable(button);
///         
///         //当输入框的值改变时
///         input.OnValueChangedAsObservable()
///             .Where(x => x != null)//只要输入的值不为空？
///             .SubscribeToText(text);//把文本显示出来？
///         
///         //当滑块发生改变时，显示文本
///         slider.OnValueChangedAsObservable()//
///             .SubscribeToText(text, x => Math.Round(x,2).ToString());
///            
/// 
/// 2、除了响应式属性之外，还有响应式集合 ReactiveCollection
/// 
/// 
/// 
/// 3、在实现 MVP 模式之前，先看以下代码：
///     public class Enemy
///     {
///         public ReactiveProperty<long> CurrentHp{ get; private set;}//只能在类里面修改 currentHP
///         public IReadOnlyReactiveProperty<bool> IsDead{ get; private set;}
///         
///         public Enemy(int initialHP)//初始化 Enemy 的血量和是否死亡状态
///         {
///             CurrentHp = new ReactiveProperty<long>(initialHp);
///             IsDead = CurrentHp.Select(x => x <= 0).ToReactiveProperty();//如果 x 小于等于 0，表示敌人死亡，反之不是死亡状态
///             //x <= 0 表达式会返回一个布尔值，ToReactiveProperty 会根据表达式返回的布尔值直接创建一个相应的 ReactiveProperty（在此例子中是 ReactiveProperty<bool>） 实例并返回（赋值给 IsDead）
///         }
///     }
///     
///     void Start()
///     {
///         button.OnClickAsObservable().Subscribe(_ => enemy.CurrentHp.Value -= 99);//点击按钮，敌人血量减 99
///         enemy.CurrentHp.SubscribeToText(MyText);//将敌人血量显示到文本框中
///         enemy.IsDead.Where(isDead => isDead)//如果敌人处于死亡状态，让按钮不可交互（不能再减血了）
///             .Subscribe(_ =>
///             {
///                 button.interactable = false;
///             });
///     }
///     
///     --对于以上代码，其实非常好理解：
///         1、Enemy 相当于一个 Model
///             --Enemy 中保存了数据，是一个数据类，可以理解为 Model
///         2、Unity 的 UI 界面相当于 View
///         3、Start 方法所在的类相当于 Controller
///             --Start 方法中的代码将 Hierarchy 中的 UI 控件 与 Model 绑定在了?起。当 Model 数据改变时通知 UI 更新，从 UI 接收到点击事件对 Model 进?值的更改
///         4、以上代码在 Unity 中就相当于一个 MVC 框架小案例，这就是?个?常简单的 MVP 模式
///   
/// 
/// 
/// 
/// 4、我们可以用 UnityEvent.AsObservable 将 ReactiveProperties、ReactiveCollections 和 Observables 都组合起来，所有 UI 组件都提供了 XXXAsObservable
/// 5、在 Unity 里，序列化是一个很重要的功能，如果不可序列化，则在编辑器上就看不到参数，而 ReactiveProperty 是泛型的，序列化起来比较麻烦，为了解决这个问题，UniRx 支持了可序列化的 ReactiveProperty 类型，比如 Int/Long/Float/Double/String/BoolReactiveProperty等
/// 6、对于自定义的枚举 ReactiveProperty，写一个可检视的 ReactiveProperty[T] 也很容易
/// 7、如果需要[Multiline] 或者 [Range] 添加到 ReactiveProperty 上，可以使用 MultilineReactivePropertyAttribute 和 RangeReactivePropertyAttribute 替换 Multiline 和 Range
/// 8、这些 InspectableReactiveProperties 可以在 Inspector 面板显示，并且当他们的值发生变化是会发出通知，甚至在编辑器里变化也可以
/// 9、如果 ReactiveProperty 的值只在 Stream 中更新，可以使? ReadOnlyReactiveProperty 让这个属性只读
/// 
/// 
/// 
/// 10、MVP 设计模式：Model-View-（Reactive）Presenter Pattern
///     1、使用 UniRx 可以很方便实现 MVP(MVRP) 模式（如上例子）
///     2、为什么应该用 MVP 模式而不是 MVVM 模式呢？
///         1、MVVM(Model-View-ViewModel)是对 MVC(Model-View-Control)和 MVP(Model-View-Presenter)的进一步改进
///         2、Unity 没有提供 UI 绑定机制，创建一个绑定层过于复杂并且会对性能造成影响（因为会使用到反射）
///         3、Unity 的视图（UI）需要更新，Presenters 层知道 View 组件并能更新它们，所以使用 Mvp 模式更好，虽然 MVP 模式没有真正去绑定 UI，但 Observables 可以通知订阅者，功能上也差不多。这种模式叫做 Reactive Presenter
///     3、简单理解 Unity 中的 MVP 模式：
///         1、在 Unity 中，我们把 Scene 中的 GameObject 当做视图层，这些是在 Unity 的 Hierarchy 中定义的。
///         2、展示/控制层在 Unity 初始化时将视图层绑定。SubscribeToText 和 SubscribeToInteractable 都是简洁的类似绑定的辅助函数。虽然这些?具很简单，但是?常实?，在 Unity 中使?开发体验?常平滑，性能也很好，最重要的是可以让我们的代码更简洁
///         3、View -> ReactiveProperty -> Model -> RectiveProperty -> View ：完全?响应式的?式连接，UniRx 提供了所有的适配?法和类，不过其他的 MVVM (or MV*) 框架也可以使?
///         4、UniRx / ReactiveProperty 只是?个简单的?具包


///12、Merge 操作符：
/// 1、Merge 是合并的意思
/// 2、问题是：合并什么？
///     --在 UniRx 的世界里，任何东西都是以事件流的形式存在的
///     --在之前的 EveryUpdate、Timer 等等都是开启了一条事件流，但是，UniRx 只能开启一条事件流吗？
///     --不是，UniRx 可以开启多个事件流，并可以使用 Merge 进行事件流的合并
///     --即：Merge 合并的是事件流
/// 3、如：
///     var leftMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
///     var rightMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(1));
///     Observable.Merge(leftMouseClickStream,rightMouseClickStream)
///         .Subscribe(_ =>
///         {
///             Debug.Log("按下了鼠标左键或者右键");
///         });
///     --以上代码实现的逻辑是：当鼠标左键或右键点击时都会进行处理。
///     --即：Merge 操作符将 leftMouseClickStream 和 rightMouseClickStream 合并成了一个事件流
/// 4、Merge 属于处理多个流的操作符，除此之外还有更多类似的操作符如 Zip 等。学习完它们之后可以实现?常复杂的逻辑，?如 Coroutine 的?持，可以实现按顺序执?多个 Coroutine ，也?持等待所有 Coroutine 执?完成这种复杂的操作。


///13、实现：当某个按钮点击时，使所有当前页面的按钮不可被点击：（和 Select 操作符）
/// 1、注意：虽然标题有点啰嗦，但是这个需求我们经常会遇到
/// 2、对于上述需求，我们一般的实现是加上一个事件的遮罩层，或者创建一个 bool 进行标记，但是使用 UniRx 会简单得多：
///     pubic class TestPanel : XXXPanel
///     {
///         [SerializeField] Button buttonA;
///         [SerializeField] Button buttonB;
///         [SerializeField] Button buttonC;
///         
///         void Start()
///         {
///             var buttonAStream = buttonA.OnClickAsObservable();
///             var buttonBStream = buttonB.OnClickAsObservable();
///             var buttonCStream = buttonC.OnClickAsObservable();
///             
///             Observable.Merge(buttonAStream,buttonBStream,buttonVStream)
///                 .First()
///                 .Subscribe(_ =>
///                 {
///                     Debug.Log("A/B/C 按钮被点击了");
///                 });
///         }
///     }
/// 3、以上的代码实现的是三个按钮的任意一个按钮被点击就处理事件，如果我们想要知道哪个按钮被点击，应该怎么做？
///     --使用 Select 操作符即可
/// 4、Select 操作符：
///     1、Select 是选择的意思
///     2、Select 操作符本身是 Linq 的操作符，一般是传入一个索引 index ，然后根据索引返回具体的值
///     3、对于一个 List，什么叫做 Select呢？看以下代码：
///         var testNumbers = new List<int>(){ 1,2,3};
///         var selectedValue = testNumbers[2];
///         --其中 testNumbers[2] 就是一个选择操作
///     4、Select 操作符会在后续进行详细学习
///     5、目前只要知道，使? Select 操作符后会返回?个值，值的类型是?个泛型


///14、Unity 与 Rx：
/// UniRx 之所以叫 UniRx 是因为它是 Unity 版本的 Reactive Extensions（Unity 的响应式扩展），单独对 Unity 做了很多功能上的增强
///     1、UI 增强
///     2、GameObject/MonoBehaviour 增强以及引擎的事件增强如 OnApplicationPause、UnityEvent
///     3、Coroutine/Thread 增强
///     4、网络请求（WWW 等）增强
///     5、ReactiveProperty、ReactiveCollection、ReactiveDictionary 等
///     6、ReactiveCommand 命令系统
///     …………


///15、UI 增强 + 登录注册页面的练习：
/// 1、作为初学者，在日常开发中接触最多的就是 UGUI，而 UGUI 的开发大多需要遵循 MVC 模式
/// 2、MVC 模式对很多人来说是一个非常模糊的架构模式，但是本质很简单，想办法把表现层和数据层分离即可，也就是分离 View 和 Model
/// 3、Unity 实现 MVC 的方式由很多种，用 UniRx 的 ReactiveProperty 可以完全实现 MVC 的一个变种——MVP，UniRx 在 MVP 的实现上是非常明确的，开发者在开发的时候不用纠结怎么实现，单单这一点就让 UGUI 的开发简化了很多
/// 4、除此之外，UniRx 还支持非常多的 UGUI 控件，所有的 UGUI 控件支持如下：
///     [SerializeField] Button button;
///     [SerializeField] Toggle toggle;
///     [SerializeField] Scrollbar scrollbar;
///     [SerializeField] ScrollReac scrollReac;
///     [SerializeField] Slider slider;
///     [SerializeField] InputField inputField;
///     
///     void Start()
///     {
///         button.OnClickAsObservable().Subscribe(_ => Debug.Log("On Button CLicked"));
///         toggle.OnValueChangeAsObservable().Subscribe(on => Debug.Log("Toggle " + on));
///         scrollbar.OnValueChangedAsObservable().Subscribe(scrollValue => Debug.Log("Scrolled " + scrollValue));
///         scrollRect.OnValueChangedAsObservable().Subscribe(scrollValue => Debug.Log("Scrolled " + scrollValue));
///         slider.OnValueChangedAsObservable().Subscribe(sliderValue => Debug.Log("Slider Value " + sliderValue));
///         inputField.OnValueChangedAsObservable().Subscribe(inputText => Debug.Log("Input Text: " + inputText));
///         inputField.OnEndEditAsObservable().Subscribe(result => Debug.Log("Result :" + result));
///     }
/// 5、以上就是所有 Observable 支持，当然还支持 Subscribe 增强，如 SubscribeToText
///     --
///     Text resultText = GetComponent<Text>();
///     inputField.OnValueChangedAsObsservable().SubscribeToText(resultText);
///     --
///     1、这段代码实现的功能是，当 inputField 的输入值改变时，会马上显示在 resultText 上，完成了 inputField 和 resultText 的 绑定
/// 6、除此之外还支持 SubscribeToInteractable，基本上这些就够用了
/// 7、练习：登录注册界面：见下方 “15、UI 增强 + 登录注册页面的练习 + AddTo简介：” 代码
/// 


///16、Unity 的生命周期与 Triggers：
/// 1、对于 Unity 生命周期的 Observable 增强，单单 Update 就支持非常多细分类型的 Update 事件捕获：
///     --
///     Observable.EveryUpdate.Subscribe(_ => {});//相当于 Update
///     Observable.EveryFixedUpdate().Subscribe(_ => {});//相当于 FixedUpdate
///     Observable.EveryEndOfFrame().Subscribe(_ => {});//监听每帧的结束
///     Observable.EveryLateUpdate().Subscribe(_ => {});//相当于 LateUpdate
///     Observable.EveryAfterUpdate().Subscribe(_ => {});//监听 Update 的结束
///     --
/// 2、除了 Update 还支持其它事件，如 ApplicationPause、Quit 等：
///     --
///     Observable.EveryApplicationPause().SUbscribe(paused => {});//监听程序暂停，paused 表示程序是否暂停
///     Observable.EveryApplicationFocus().Subscribe(focused => {});//是否聚焦在程序，若鼠标在程序之外点击，表示没有聚焦，返回 false
///     Observable.EveryApplicationQuit().Subscribe(_ => {});//监听程序退出
///     //学习以上这些后命名??代码就可以实现诸如“应?程序退出事件监听”的逻辑了，不?再创建?个单例类去实现
///     --
/// 3、Trigger (需要引入命名空间 UniRx.Trigger，使用了 Trigger 的脚本所在的对象在运行时会自动添加 Observable Update Trigger.cs)：
///     1、有时候在某个脚本中实现 Observable.EveryUpdate() 这个 API 需要绑定 MonoBehaviour 的生命周期（主要是将其绑定到 OnDestroy，当然该 API 有的时候也会是全局?且永远不会被销毁的），绑定方式?常简单，只需要?个 AddTo 就可以进?绑定了，代码如下：
///         --
///         Observable.EveryUpdate().Subscribe(_ => {}).AddTo(this);
///         //更简洁写法：
///         this.UpdateAsObservable().Subscribe(_ => {});//UpdateAsObservable 这种类型的 Observable 就是 Trigger，即触发器
///         --
///     2、什么是 Trigger？
///         1、字如其意，触发器是挂在 GameObject 上用来监听 GameObject 某个事件发?的功能脚本，事件发?时会回调给注册它的 Subscribe ，即当某个事件发?时它会将该事件发送到 Subscribe 函数中
///         2、触发器的操作和其他事件源 (Observable) 是?样的，都?持 Where、First、Merge 等操作符
///         3、Trigger 类型的 Observable 和之前所学的 Observable 在表现上有点不?样:
///             1、Trigger ?部分都是以 XXXAsObsrevable 命名形式的
///             2、在使? Trigger 的 GameObject 上都会挂上对应的 Observable XXXTrigger.cs 脚本
///         4、在之前我们其实就已经接触过 Trigger ，AddTo() 这个 API 其实就封装了?种 Trigger: ObservableDestroyTrigger，顾名思义，就是当 GameObject 销毁时获取事件的?个触发器，使用 AddTo 就相当于使用了触发器，在运行时脚本所在的对象会自动添加 Observable Update Trigger.cs
///         5、?般的 Trigger 都会配合 MonoBehaviour ?起使?，?如 ObservableDestroyTrigger:
///             --
///             this.OnDestroyAsObservable().Subscribe(_ => {});
///             --
///         6、除了 Destroy 还有?常多的 Trigger：
///             1、各种细分类型的 Update：
///                 --
///                 this.FixedUpdateAsObservable().Subscribe(_ => {});
///                 this.LateUpdateAsObservable().Subscribe(_ => {});
///                 this.UpdateAsObservable().Subscribe(_ => {});
///                 --
///             2、各种碰撞的 Trigger:
///                 --
///                 this.OnCollisionEnterAsObservable(collision => {});
///                 this.OnCollisionStayAsObservable(collision => {});
///                 this.OnCollisionExitAsObservable(collision => {});
///                 --
///                 this.OnCollision2DEnterAsObservale(collision2D => {});
///                 this.OnCollision2DStayAsObservable(collision2D => {});
///                 this.OnCollision2DExitAsObservable(collision2D => {});
///                 --
///             3、各种脚本的参数监听：
///                 --
///                 this.OnEnableAsObservable().Subscribe(_ => {});
///                 this.OnDisableAsObservable().Subscribe(_ => {});
///                 --
///         7、除了 MonoBehaviour ，Trigger 也?持了其他组件类型，如 RectTransform、Transform、UIBehaviour 等等，详情可查看 ObservableTriggerExtensions.cs 和 ObervableTriggerExtensions.Component.cs 中的 API。


///17、UI Trigger：
/// 1、之前学过 Observable 对 UI 的增强，Trigger 也有 UI 的增强部分，“16、”中讲过 Trigger 除了 MonoBehaviour 还?持其他的类型如 Transform、RectTransform、UIBehaviour，其中 UIBehavior 就是对 UI 的增强，也是本节的重点
/// 2、为什么说支持 UIBehavior 就是对 UI 的增强：
///     --因为 UIBehaivour 是 UGUI 所有控件的基类，所以只要?持 UIBehaivour，所有的 UGUI 控件都会继承 UIBehaviour 的?持
/// 3、Trigger 从哪方面对 UI 进行支持？
///     1、Trigger 是从各种事件开始?持 UI 的，?如所有的 Graphic 类型都?持 OnPointerDownAsObservable、OnPointerEnterAsObservable、OnPointerExitAsObservable 等 Trigger。
///     2、什么是 Graphic：所有在 Inspector 上勾选了 Raycast Target 选定框的都是 Graphic 类型，包括Image、Text 等
///     3、也就是说，Image、Text 全部?持 OnPointerDownAsObservable、OnPointerEnterAsObservable 等 Trigger
///     4、我们知道，如果想??去接收?个 OnPointerDown 事件，需要实现?个 IPointerDownHandler 接?，? UniRx 则把所有的 IXXXHandler 接?都做成 Trigger了，这样就再也不?需要?上到处流传的 UIEventListener.Get(gameObejct).onClick 这种?式了，因为这种?式问题很多，?如，由于它继承了 EventTriggers，实现了所有的事件接?，他就会吞噬掉 OnScroll 等事件（可以被点击，不能被滑动）。? UniRx 的实现?常细，?个 IXXXHandler 就是?个 Trigger。
///     5、需要?个全部实现并且吞并事件的版本也没关系，UniRx 也实现了?个 ObservableEventTrigger，和 UIEventListener ?样。
///     6、在项?中?的?较多的?个 Trigger:
///         ---
///         image.OnBeginDragAsObservable().Subscribe(dragEvent => {});
///         graphic.OnDragAsObservable().Subscribe(dragEvent => {});
///         text.OnEndDragAsObservable().Subscribe(dragEvent => {});
///         image.OnPointerClickAsObservable().SUbscribe(clickEvent => {});
///         //这些 API 的使用?常?便，导致 QFramework 的?些脚本都弃?了
///         --
///     7、除了常?的?个 Trigger 之外还有?常多实?的 Trigger，如 OnSubmitAsObservable、OnDropAsObservable 等，具体可以参考 ObservableTriggerExtensions.Component.cs，只要能想到的基本上 UniRx 都?持。


///18、Coroutine 的操作：
/// 1、UniRx 对 Unity 的 Coroutine 也提供?持，可以将?个 Coroutine 转化为事件源（Observable）
///     --
///     IEnumerator CoroutineA()
///     {
///         yield return new WaitForSeconds(1.0f);
///         Debug.Log("A");
///     }
///     private void Start()
///     {
///         Observable.FromCoroutine(_ => CoroutineA())//Couroutine A 执行后触发事件，执行订阅的方法输出 B
///             .Subscribe(_ =>
///             {
///                 Debug.Log("B");
///             }).AddTo(this);
///     }
///     --
/// 2、UnitRx 也?持将 Observable 转化为?个 Coroutine 中的 yield 对象:
///     --
///     IEnumerator Delay1Aecond()
///     {
///         yield return Observable.Timer(TimeSpan.FromSeconds(1.0f)).ToYieldInstruction();
///         Debug.Log("C");
///     }
///     private void Start()
///     {
///         StartCoroutine(Delay1Aecond());
///     }
///     --
/// 3、FromCoroutine 和 ToYieldInstruction 实现了 Observable 与 Coroutine 之间的互相转化
/// 4、之前学过，Observable 是?条事件流，而 UniRx 的操作符如 Merge 可以将流进?合并从而处理多个事件流，当然除了合并也?持别的操作，?如顺序 (依赖) 执? Coroutine、并?执? Coroutine 等等。在之后，通过学习新的操作符，可以让 Coroutine 更加强?。


///19、WhenAll:Coroutine 的并?操作
/// 1、WhenAll 意思是 ——> 当所有的，表示的意思是：当所有的事件流都结束，就会触发 Subscribe 注册的回调
///     --
///     IEnumerator A()
///     {
///         yield return new WaitForSeconds(1.0f);
///         Debug.Log("A");
///     }
///     IEnumerator B()
///     {
///         yield return new WaitForSeconds(2.0f);
///         Debug.Log("B");
///     }
///     private void Start()
///     {
///         var aStream = Observable.FromCoroutine(_ => A());
///         var bStream = Observable.FromCoroutine(_ => B());
///         Observable.WhenAll(aStream, bStream)
///             .Subscribe(_ =>
///             {
///                 Debug.Log("A、B 事件流都触发了");
///             }).AddTo(this);
///     }
/// 2、WhenAll 和 Merge 是同类型的，都是处理多个流的操作符
/// 3、WhenAll 除了并?实现 Coroutine 之外，还可以实现“当所有的按钮都点击过?次的逻辑”:
///     Button _aButton;
///     Button _bButton;
///     Button _cButton;
///     private void Start()
///     {
///         Button _aButton = GameObject.Find("19_A_Button").GetComponent<Button>();
///         _aButton.GetComponentInChildren<Text>().text = "A 按钮";
///         Button _bButton = GameObject.Find("19_B_Button").GetComponent<Button>();
///         _bButton.GetComponentInChildren<Text>().text = "B 按钮";
///         Button _cButton = GameObject.Find("19_C_Button").GetComponent<Button>();
///         _cButton.GetComponentInChildren<Text>().text = "C 按钮";
///         var aStream = _aButton.OnClickAsObservable().First();
///         var bStream = _bButton.OnClickAsObservable().First();
///         var cStream = _cButton.OnClickAsObservable().First();
///         Observable.WhenAll(aStream, bStream, cStream).Subscribe(_ =>
///         {
///             Debug.Log("A、B、C 按钮都点击过一次了");
///         }).AddTo(this);
///     }


///20、事件流的结束 OnCompleted（也是为什么 WhenAll 可以接受到所有事件的关键原因）：
/// 1、OnCompleted：UniRx 的结束事件（UniRx 事件流是有生命周期的，OnCompleted 可看作生命周期的结束）
/// 2、注意：并不是每个事件流都有结束事件的，比如 Timer、First、Coroutine 都有结束事件，而 EveryUpdate 是没有结束事件的，它会每帧执行一次并一直执行下去，这也是我们为什么需要使用 AddTo 将其与某个游戏对象的 OnDestroy 绑定到一起，其实就是为其绑定一个结束生命周期的函数。
/// 3、使? Subscribe API 进?订阅的时候，第?个参数是 OnNext 回调的注册，这也是我们?部分情况下使?的回调；第?个参数则是 OnComplete：


///21、Start 让多线程更简单：
/// 1、多线程是作为?级开发者必须具备的?种技术，了解了多线程可以让我们充分利?多核移动端的计算优势，也可以让我们的游戏体验更平滑。
/// 2、在 Unity 中我们?般? Thread.Start 开启?个线程，当逻辑?常复杂的时候多线程?常难以管理。? UniRx 改善了这?种状况。
/// 3、“当所有线程运?完成后，在主线程执?某个任务” 这个功能，使? UniRx 实现如下:
///     private void Start()
///     {
///         IObservable<int> threadAStream = Observable.Start(() =>
///         {
///         System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
///         return 10;
///         });
///         IObservable<int> threadBStream = Observable.Start(() =>
///         {
///             System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
///             return 10;
///         });
///         Observable.WhenAll(threadAStream, threadBStream)//因为线程有开启就有结束，所以 WhenAll 可以接收到线程的 OnCompleted
///             .ObserveOnMainThread().Subscribe(result =>
///             {
///                 //Debug.Log($"{result[0]}:{result[1]}");
///                 Debug.LogFormat("{0},{1}", result[0], result[1]);
///             });
///     }
/// 4、这?有两个新的 API：
///     1、Observable.Start：开启?个线程流
///     2、ObserveOnMainThread：把 WhellAll 结果转到主线程上。
///     --这样 Subscribe ?的回调就可以使? Unity 的 API 了（Unity 很多 API 不可以在其他线程中使?，如果想在线程中某一时刻使用 Unity 的 API，可以使用这种方式在某一时刻表示线程完成，然后在 WhenAll 中使用 Unity 的 API）
/// 5、使? UniRx 处理线程逻辑?常方便，线程和 Coroutine (协程）都可以使? WhenAll 操作符，除 WhenAll 之外还有很多其他的操作符可以使用，在之后慢慢学习。


///22、ObservableWWW 优雅的网络请求操作：
/// 1、以往不管是使? WWW 还是 UnityWebRequest 都要使? Coroutine 去驱动，但使?协程写出来的代码需要?堆判断，代码?常混乱，? UniRx 能以简练的?格提供对?络请求的?持：
///     --
///     ObservableWWW.Get("http://sikiedu.com")
///         .Subscribe(_ =>
///         {
///             Debug.Log("do something");
///         }).AddTo(this);
///     --
/// 2、ObservableWWW 同样?持 WhenAll 操作符：
///     --
///     IObservable<string> aStream = ObservableWWW.Get("http://sikiedu.com");
///     IObservable<string> bStream = ObservableWWW.Get("http://sikiedu.com");
///     Observable.WhenAll(aStream, bStream)
///         .Subscribe(responseTexts =>
///         {
///             Debug.Log(responseTexts[0].Substring(0, 10));
///             Debug.Log(responseTexts[1].Substring(0, 10));
///         }).AddTo(this);
///     --
/// 3、ObservableWWW 除了 Get 也?持了 Post，还有 GetWWW 和 PostWWW 这种的辅助封装，还有 GetAndGetBytes 和 PostAndGetBytes：
///     --
///     //QFramework 中?段下载?件的代码：
///     //下载文件 url： http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage
///     ScheduledNotifier<float> progressListener = new ScheduledNotifier<float>();
///     ObservableWWW.GetAndGetBytes("http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage", progress: progressListener)
///         .Subscribe(bytes =>
///         {
///             Debug.Log("将下载的文件放到某个地方");
///         });
///     progressListener.Subscribe(progress =>
///     {
///         Debug.Log($"进度为{progress}");//进度为 1 表示下载完成
///     });
///     --
/// 4、ObservableWWW 的 API 都可以传进去?个 ScheduledNotifier<T>() ?来监听下载进度，Subscribe 之后传回来的值则是当前的进度，?且 ObservableWWW 的 Get 和 Post 请求都可以??传对应的 header 和 WWWForm
/// 5、除了常?的 Get 和 Post 请求，ObservableWWW 也对 AssetBundle 的加载也做了简单的封装，提供了诸如 ObservableWWW.LoadFromCacheOrDownload 这样的 API。如果想深?了解，可以参考 ObservableWWW.cs 
/// 6、UniRx 对 WWW 提供的 API ?常简练，也?够使?


///23、ReactiveCommand：
/// 1、ReactiveCommand 的 定义：
///     --
///     public interface IReactiveCommand<T> : IObservable<T>
///     {
///         IReadOnlyReactiveProperty<bool> canExecute { get; }
///         bool Execute(T parameter);
///     }
///     --
/// 2、ReactiveCommand 提供了两个 API：
///     1、canExecute：CanExecute 是内部使?的，只对外部提供了只读访问
///     2、Execute：Execute ?法是被外部调?的，Execute 的执行就是这个 Command 的执?
/// 3、canExecute 和 Execute 的关系：
///     1、当 CanExecute 为 false 时，在外部调? Execute 无效，该 Command 不会被执?
///     2、当 CanExecute 为 true 时，在外部调? Execute ，该 Command 会被执?
/// 4、是什么决定 CanExecute 为 false 或 true 的?
///     --是其他的 Observable。
/// 5、新创建的 ReactiveCommand 默认 CanExecute 为 true：
///     --
///     private void Start()
///     {
///         ReactiveCommand command = new ReactiveCommand();
///         command.Subscribe(_ =>
///         {
///             Debug.Log("command executed");
///         });
///         command.Execute();
///         command.Execute();
///         command.Execute();
///     }
///     --
/// 6、ReactiveCommand 的使用?常简单，只要调? ReactiveCommand 的 Execute 方法，command 就会通知 Subscribe 的回调(因为 CanExecute 为 true)。
/// 7、CanExecute 的开启关闭是由 Observable （事件源）决定的：
///     --
///     IObservable<bool> leftMouseClickStream = Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .Select(_ => true);
///     IObservable<bool> leftMouseUpStream = Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonUp(0))
///         .Select(_ => false);
///     IObservable<bool> mouseUp = Observable.Merge(leftMouseClickStream, leftMouseUpStream);
///     ReactiveCommand reactiveCommand = new ReactiveCommand(mouseUp, false);//mouseUp 表示 ReactiveCommand 的事件源，canExecute 默认为 false；当事件源 mouseUp 传递 true （鼠标按下）时 canExecute 为 true
///     reactiveCommand.Subscribe(x =>//没有传递过来参数，所以 x 表示的是 "()"
///     {
///         Debug.Log(x);
///     });
///     Observable.EveryUpdate()
///         .Subscribe(_ =>
///         {
///             reactiveCommand.Execute();
///         });
///     --
/// 8、ReactiveCommand 也是可以被订阅(Subscribe) 的，在订阅之前也可以使? Where 等操作符进?事件操作：
///     --
///     ReactiveCommand<int> reactiveCommand = new ReactiveCommand<int>();
///     
///     reactiveCommand.Where(x => (x % 2 == 0))
///         .Subscribe(x => Debug.Log($"{x} is even numbers ."));
///     
///     reactiveCommand.Where(x => (x % 2 != 0))
///         .Timestamp()
///         .Subscribe(x => Debug.Log($"{x.Value} is Odd,{x.Timestamp}"));
///     
///     reactiveCommand.Execute(2);
///     reactiveCommand.Execute(3);
///     --
/// 9、ReactiveCommand 除了能做以上所示的?些简单的事情外，其实可以做?常多强?的功能，只是在介绍?常强?的功能之前要先学好 UniRx 的??基础及?点点原理，后续再对其强大功能进行学习


///24、ReactiveCollection 与 ReactiveDictionary：
/// 1、ReactiveCollection：
///     1、ReactiveCollection 类似于 List。
///     2、我们可以使?如下的操作符:
///         1、ObserverAdd // 当 新的 Item 添加则会触发
///         2、ObserverRemove // 删除 Item 触发
///         3、ObserverReplace // 替换(Update)  Item 触发
///         4、ObserverMove // 移动 Item 触发
///         5、ObserverCountChanged // 数量有改变(Add、Remove) 时触发
///     3、示例：
///         --
///         _ages.ObserveAdd().Subscribe(addAge =>
///         {
///             Debug.Log($"添加的年龄 addAge 类型是 CollectionAddEvent<int> :{addAge}");
///             Debug.Log($"添加的年龄在 ReactionCollection 中的下标:{addAge.Index}");
///             Debug.Log($"添加的年龄的值:{addAge.Value}");
///             Debug.Log("以下同理");
///         });
///         _ages.ObserveRemove().Subscribe(removeAge =>
///         {
///             Debug.Log($"移除的年龄:{removeAge}");
///         });
///         _ages.ObserveReplace().Subscribe(replaceAge =>
///         {
///             Debug.Log($"替换的年龄是：{replaceAge}");
///         });
///         _ages.ObserveMove().Subscribe(moveAge =>
///         {
///             Debug.Log($"移动的年龄是：{moveAge}");
///         });
///         _ages.ObserveCountChanged().Subscribe(count =>
///         {
///             Debug.Log($"年龄表数量改变为：{count}");
///         });
///         
///         foreach (int age in _ages)
///         {
///             Debug.Log(age);
///         }
///         _ages.Add(6);
///         _ages.Remove(2);
///         _ages[4] = 7;
///         --
/// 2、ReactiveDictionary：
///     1、ReactiveDictionary 功能与 Dictionary ?样
///     2、ReactiveDictionary 同样?持以下?个操作符:
///         --
///         ObserverAdd // 当 新的 Item 添加则会触发
///         ObserverRemove // 删除
///         ObserverReplace // 替换(Update)
///         ObserverMove // 移动
///         ObserverCountChanged // 数量有改变(Add、Remove)
///         --
///     3、示例：
///         --
///         private ReactiveDictionary<string, string> languageCode = new ReactiveDictionary<string, string>()
///         {
///             { "en", "英语"},
///             { "cn", "中文"}
///         };
///         private void Start()
///         {
///             languageCode.ObserveAdd().Subscribe(addLanguage =>
///             {
///                 Debug.Log($"添加的语言键值对 addLanguage 类型是 DictionaryAddEvent<string,string> :{addLanguage}");
///                 Debug.Log($"添加的语言键值对的键:{addLanguage.Key}");
///                 Debug.Log($"添加的语言键值对的值:{addLanguage.Value}");
///                 Debug.Log("以下同理");
///             });
///             languageCode.ObserveRemove().Subscribe(removeLanguage =>
///             {
///                 Debug.Log($"移除的语言键值对:{removeLanguage}");
///             });
///             languageCode.ObserveReplace().Subscribe(replaceLanguage =>
///             {
///                 Debug.Log($"替换的语言键值对是：{replaceLanguage}");
///             });
///             languageCode.ObserveCountChanged().Subscribe(count =>
///             {
///                 Debug.Log($"语言字典数量改变为：{count}");
///             });
///             foreach (KeyValuePair<string, string> keyValue in languageCode)
///             {
///                 Debug.Log($"{keyValue.Key}:{keyValue.Value}");
///             }
///             
///             languageCode.Add("jp", "日语");
///             languageCode.Remove("en");
///         }
///         --


///25、 加载场景 AsyncOperation：
/// 1、我们在异步加载资源或者异步加载场景的时候往往会?到 AsyncOperation，UniRx 对 AsyncOperation 做了?持，使得加载操作可以很容易地监听加载进度
///     --
///     ScheduledNotifier<float> progressObservable = new ScheduledNotifier<float>();
///     SceneManager.LoadSceneAsync(1).AsAsyncOperationObservable(progressObservable)//异步加载下标为 1 的场景
///         .Subscribe(asyncOperation =>//场景加载完成回调
///         {
///             Debug.Log("load done");
///             Resources.LoadAsync<GameObject>("TestCanvas").AsAsyncOperationObservable()
///             .Subscribe(resourceRequest =>//异步加载资源（asyncOperation 继承了 AsyncOperation，表示请求的资源）
///             {
///                 Instantiate(resourceRequest.asset);//实例化请求的资源
///             });
///         });
///     progressObservable.Subscribe(progress =>
///     {
///         Debug.Log($"加载了：{progress}");
///     });


///26、TodoList 实战：
/// 1、TodoList App 是一个待办事项应用，主要功能如下：
///     1、代办清单可以添加、更改、删除代办事项
///     2、待办事项可以完成
///     3、待办事项的内容可以编辑
/// 2、Model 设计如下：
///     --
///     // 代办事项
///     public class TodoItem
///     {
///         /// 标示符
///         public int id;
///         
///         /// 内容
///         public StringReactiveProperty Content;
///         
///         /// 是否完成
///         public BoolReactiveProperty Completed;
///     }
///         
///     // 代办列表
///     public class TodoList
///     {
///         public ReactiveCollection<TodoItem> todoItems = new ReactiveCollection<TodoItem>()
///         {
///             new TodoItem()
///             {
///                 id = 0,
///                 Content = new StringReactiveProperty("卖萝卜"),
///                 Completed = new BoolReactiveProperty(false)
///             },
///             new TodoItem()
///             {
///                 id = 1,
///                 Content = new StringReactiveProperty("注册 sikiedu.com"),
///                 Completed = new BoolReactiveProperty(false)
///             }
///         };
///     }
/// 3、界面设计及数据展示：
///     1、新建 Canvas，将 Canvas Scale 组件的 UI Scale Mode 改为 Scale With Screen Size，将 Reference Resolution改为：710，1280
///     2、新增 Game 视图的分辨率 640 * 1136
///     3、创建 Scroll View，做简单布局，删除滚动条，将 Scroll View 的 Scroll Rect 组件的 Horizontal 取消勾选，只进行竖直方向滚动
///     4、在 Scroll View 的 Content 上添加 Grid Layout Group 组件
///     5、将 Grid Layout Group 组件的 Constraint 设置为 Fixed Coloum Count，将 Constraint Count 设置为 1
///     6、将 Grid Layout Group 的 Child Alignment 设置为 Upper Center，设置的是 Content 的子对象的对其方式
///     7、修改 Grid Layout Group 的 Cell Size（可以添加几个 Image 做对比），此处可设置为 680
///     8、修改 Grid Layout Group 的 Spacing，即孩子之间的间隔
///     9、调整 Grid Layout Group 的 Padding
///     10、在 Scroll View 同一层级下创建一个 Image，命名为 TodoItemPrototype(Prototype:原型)，作为 TodoItem 的原型
///     11、为 TodoItemPrototype 创建一个 Button 表示“完成”按钮，将其放到右侧
///     12、为 TodoItemPrototype 创建一个 Text 表示“内容”文本，将其放在左侧，文本在文本水平方向左居中，竖直方向居中
///     13、为 TodoItemPrototype 创建一个脚本，命名为 UITodoItem，在里面获取并初始化按钮对象和内容文本对象（见 UIToDoItem.cs）
///     14、将 TodoItemPrototype 默认为不显示
///     15、为 Canvas 创建一个 UITodoList 脚本，在其中进行 TodoList 的初始化（见 UITodoList.cs）
///     16、具体实现见代码
/// 4、“完成”按钮功能（见 UITodoList.cs、UITodoItem.cs 代码）：
///     1、注册“完成”按钮点击事件，当点击时，该 Item 的 Complete 设置为 true(因为 Complete 是一个响应式属性，值改变时会发送通知)
///     2、过滤 item 中 Complete 为 True 的 Item，已完成的不显示
/// 5、添加 TodoItem 的添加操作：
///     1、添加一个 InputField 输入框，添加一个“添加”按钮，按下“添加”按钮添加 TodoItem，当输入框没有值时，添加按钮不可点击
///     2、监听按钮点击事件，在回调的方法中创建新的 Item，保存编辑的内容，将 item 添加到 todoItems 中
///     3、监听列表的 todoItems 数量的变化，如果数量发生变化，回调 OnDataChange 方法
/// 6、数据存储与加载:
///     1、将 TodoList、TodoItem 序列化为 Json
///     2、要让某个类可序列化，需要加上[Serializable]
///     3、注意：因为 ReactiveCollection 是泛型集合，不支持直接序列化，所以此处需要将其修改为 List
/// 7、编辑内容：
///     1、点击某一项时，某一项文本显示在输入框中，加号按钮变为“更新”按钮，修改后点击按钮，更新该项的文本
///     2、再创建一个更新按钮，默认不可见不可交互
///     3、为 TodoItemPrototype 添加一个 Button 组件，让其可点击
///     4、将 输入框、添加按钮、更新按钮 放置在一个 InputController 空游戏对象下，并为其添加 UIInputController.cs 脚本
///     ……
/// 8、编辑功能优化：
///     1、添加取消编辑状态的按钮
///     2、当处于编辑状态时，列表每一项都应该无法点击:使用事件遮罩——添加图片，将图片大小与列表大小相同，然后将图片设置为透明灰色，默认不可见，当处于编辑状态时，图片可见。

public class Note : MonoBehaviour
{
    #region

    #endregion

    #region 1、2、简介优雅的 UniRx 代码案例：
    /*private void Start()
    {
        Observable.EveryUpdate()//开启 Update 的事件监听
            .Where(_ => Input.GetMouseButtonDown(0))//判断/过滤事件：如果在每帧中监听到鼠标点击事件，那么执行下方订阅的事件
            .First()//只处理第一次触发的事件
            .Subscribe(_ =>//订阅/处理 事件
            {
                Debug.Log("鼠标点击了");
            });
    }*/
    #endregion

    #region 3、定时器

    ///1、定时器最基本的实现方式
    /// 1、定义两个变量，一个 timer 用于累加 Time.deltaTime，一个 targetTime 用于判断 timer 是否大于 targetTime，如果大于则触发事件
    /// 2、当触发过一次事件后，将 targetTime 设为 float 变量最大值，那么 timer - targetTime 就永远不会大于 0，保证定时功能只能触发一次
    /*float timer = 0f;
    float targetTime = 5f;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer - targetTime > 0)
        {
            Debug.Log("do something");
            targetTime = float.MaxValue;//触发过一次定时功能之后，将 targetTime 设置为 float 变量最大值，那么 timer - targetTime 就永远不会大于 0，保证定时功能只能触发一次
        }
    }*/



    ///2、使用协程实现定时器
    /// 1、定义 IEnumerator 类，等待 seconds 秒，然后执行回调函数
    /// 2、在 Start 方法中开启协程
    /*private void Start()
    {
        StartCoroutine(Timer(5, () =>
        {
            Debug.Log("do something");
        }));//回调函数
    }

    IEnumerator Timer(float seconds,Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }*/



    ///3、使用 UniRx 方式实现定时器：
    /// 1、对比常规方式：不用额外定义一个变量，也不用在 Update 方法中进行逻辑判断
    /// 2、对比 Coroutine：不用额外定义协程，只需要一行代码就搞定
    /*private void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(5.0f))
            .Subscribe(_ =>
            {
                Debug.Log("do something");
            });
    }*/


    #endregion

    #region 4、独立的 Update
    /*private void Start()
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Debug.Log("处理 A 逻辑");
            });
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Debug.Log("处理 B 逻辑");
            });
    }*/
    #endregion

    #region 5、AddTo
    /*private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                Debug.Log("鼠标点击了");
            })
            .AddTo(this);//如果当前对象被销毁，这个任务也会被销毁
    }*/
    #endregion

    #region 6、UniRx 基本语法格式（无代码）

    #endregion

    #region 7、操作符 Where：
    /*private void Start()
    {
        Observable.EveryUpdate()
         .Where(_ => Input.GetMouseButtonDown(0))
         .Subscribe(_ =>
         {
             Debug.Log("do something");
         })
         .AddTo(this); 
    }*/
    #endregion

    #region 8、操作符 First：
    /*private void Start()
    {
        *//*Observable.EveryUpdate()
         .Where(_ => Input.GetMouseButtonDown(0))
         .First()
         .Subscribe(_ =>
         {
             Debug.Log("do something");
         })
         .AddTo(this);*//*

        //简化代码：
        *//*Observable.EveryUpdate()
         .First(_ => Input.GetMouseButtonDown(0))
         .Subscribe(_ =>
         {
             Debug.Log("do something");
         })
         .AddTo(this);*//*
    }*/
    #endregion

    #region 9、对 UGUI 的支持：
    /*private void Start()
    {
        //UniRx 对按钮点击事件的监听
        Button button = GameObject.Find("09_Button").transform.GetComponent<Button>();
        button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("点击按钮，触发事件");
            });

        //UniRx 对 Toggle 点击事件的监听
        Toggle toggle = GameObject.Find("09_Toggle").transform.GetComponent<Toggle>();
        toggle.OnValueChangedAsObservable()
            .Subscribe(on =>//on 表示 Toggle 是否被选中
            {
                Debug.Log(on);
            });

        toggle.OnValueChangedAsObservable()
            .Where(on => on)//只处理 on 为 true 的事件，即单选框勾选时才处理事件
            .Subscribe(on =>
            {
                Debug.Log(on);
            });

        //UniRx 对图片拖拽事件的监听:要事先导入 UniRx.Triggers 命名空间
        Image image = GameObject.Find("09_Image").transform.GetComponent<Image>();
        image.OnBeginDragAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("开始拖动图片");
            });
        image.OnDragAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("正在拖动图片");
            });
        image.OnEndDragAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("结束拖动图片");
            });
        //注意，一定要对以上三个事件都进行监听，如果只监听其中一个会没有效果
    }*/
    #endregion

    #region 10、Reactive Property 响应式属性：

    ///正常实现方式：
    ///
    /*int age;
    public int Age
    {
        get { return age; }
        set 
        { 
            if( age != value)
            {
                age = value;
                OnAgeChange();
            }
        }
    }

    public void OnAgeChange()
    {
        Debug.Log("年龄发生改变，此时年龄为：" + Age);
    }

    private void Start()
    {
        age = 18;
        Debug.Log("此时年龄为：" + age);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Age++;
    }*/

    ///使用 UniRx 实现：
    ///
    /*//响应式属性 Age，初始值为 0
    //public ReactiveProperty<int> Age = new ReactiveProperty<int>(0);//泛型序列化很麻烦
    public IntReactiveProperty Age = new IntReactiveProperty(0);//非泛型
    private void Start()
    {
        Debug.Log("初始年龄是：" + Age);
        Age.Subscribe(age =>
        {
            Debug.Log("现在的年龄是：" + age);
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Age.Value++;
    }*/

    #endregion

    #region 11、MVP 框架的实现：
    ///1、使用 UniRx 实现点击按钮让敌人血量减 99，如果敌人血量小于等于 0，敌人处于死亡状态，按钮不可以再交互（不能再扣血了）
    /// 1、Enemy 类相当于一个 Model
    /// 2、Unity 中的 UI 就相当于 View
    /// 3、Start 方法所在的类相当于一个 Controller
    /*public class Enemy
    {
        public ReactiveProperty<long> CurrentHp { get; private set; }//只能在类里面修改 currentHP
        public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }

        public Enemy(int initialHP)//初始化 Enemy 的血量和是否死亡状态
        {
            CurrentHp = new ReactiveProperty<long>(initialHP);
            IsDead = CurrentHp.Select(x => x <= 0).ToReactiveProperty();//如果 x 小于等于 0，表示敌人死亡，反之不是死亡状态
                                                                        //x <= 0 表达式会返回一个布尔值，ToReactiveProperty 会根据表达式返回的布尔值直接创建一个相应的 ReactiveProperty（在此例子中是 ReactiveProperty<bool>） 实例并返回（赋值给 IsDead）
        }
    }

    void Start()
    {
        //初始化敌人对象
        Enemy enemy = new Enemy(100);

        //初始化 UI
        Button button = GameObject.Find("11_Attack_Button").GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = "Attack";//按钮的显示文本
        Text text = GameObject.Find("11_EnemyHp_Text").GetComponent<Text>();
        text.text = enemy.CurrentHp.ToString();

        //注册按钮点击事件
        button.OnClickAsObservable().Subscribe(_ => enemy.CurrentHp.Value -= 99);//点击按钮，敌人血量减 99
        enemy.CurrentHp.SubscribeToText(text);//将敌人血量显示到 Text 文本中
        enemy.IsDead.Where(isDead => isDead)//如果敌人处于死亡状态，让按钮不可交互（不能再减血了）
            .Subscribe(_ =>
            {
                button.interactable = false;
            });
    }*/
    #endregion

    #region 12、Merge 操作符：
    /*private void Start()
    {
        IObservable<long> leftMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
        IObservable<long> rightMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(1));
        Observable.Merge(leftMouseClickStream, rightMouseClickStream)
             .Subscribe(_ =>
             {
                 Debug.Log("按下了鼠标左键或者右键");
             });
    }*/


    #endregion

    #region 13、实现：当某个按钮点击时，使所有当前页面的按钮不可被点击：（和 Select 操作符）
    /*private void Start()
    {
        Button buttonA = GameObject.Find("ButtonA").GetComponent<Button>();
        buttonA.GetComponentInChildren<Text>().text = "A";
        Button buttonB = GameObject.Find("ButtonB").GetComponent<Button>();
        buttonB.GetComponentInChildren<Text>().text = "B";
        Button buttonC = GameObject.Find("ButtonC").GetComponent<Button>();
        buttonC.GetComponentInChildren<Text>().text = "C";

        *//*IObservable<Unit> buttonAStream = buttonA.onClick.AsObservable();
        IObservable<Unit> buttonBStream = buttonB.onClick.AsObservable();
        IObservable<Unit> buttonCStream = buttonC.onClick.AsObservable();

        Observable.Merge(buttonAStream, buttonBStream, buttonCStream)
            .First()//因为只执行第一次，所以某个按钮被点击，当前页面的所有按钮就不能再被点击了
            .Subscribe(_ =>
            {
                Debug.Log("A/B/C 按钮被点击了（某个按钮被点击，当前页面的所有按钮就不能再被点击了）");
                Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => { gameObject.SetActive(false); });
            });*//*

        //以上代码只能知道三个按钮任意一个被点击，如果要具体知道哪一个被点击，需要使用 Select 操作符
        IObservable<String> buttonAStream = buttonA.onClick.AsObservable().Select(_ => "A");//buttonAStream 事件会返回参数 “A”
        IObservable<String> buttonBStream = buttonB.onClick.AsObservable().Select(_ => "B");//buttonBStream 事件会返回参数 “B”
        IObservable<String> buttonCStream = buttonC.onClick.AsObservable().Select(_ => "C");//buttonCStream 事件会返回参数 “C”
        Observable.Merge(buttonAStream, buttonBStream, buttonCStream)
            .First()
            .Subscribe(buttonId =>//接收一个参数 buttonId（参数名自定义），这个参数由按钮点击事件返回
            {
                if(buttonId == "A")
                    Debug.Log("A 按钮被点击了");
                if (buttonId == "B")
                    Debug.Log("B 按钮被点击了");
                if (buttonId == "C")
                    Debug.Log("C 按钮被点击了");
                Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => { gameObject.SetActive(false); });
            });
    }*/
    #endregion

    #region 14、Unity 与 Rx（无代码）：

    #endregion

    #region 15、UI 增强 + 登录注册页面的练习 + AddTo简介：
    /*GameObject loginPanel;
    GameObject registerPanel;
    InputField userNameInputField;
    InputField passwordInputField;
    Button loginButton;
    Button registerButton;

    private void Start()
    {
        //应该用两个脚本分别控制登录界面和注册界面的逻辑，以下只处理登录界面逻辑，注册界面同理
        //初始化
        loginPanel = GameObject.Find("Login_Panel");
        registerPanel = GameObject.Find("Register_Panel");


        //获取登录界面的按钮
        userNameInputField = GameObject.Find("UserName_InputField").GetComponent<InputField>();
        userNameInputField.placeholder.GetComponent<Text>().text = "请输入用户名...";//输入框的提示文本
        passwordInputField = GameObject.Find("Password_InputField").GetComponent<InputField>();
        passwordInputField.contentType = InputField.ContentType.Password;//输入框内容为文本，当输入时会以 * 显示
        passwordInputField.placeholder.GetComponent<Text>().text = "请输入密码...";//提示文本
        loginButton = GameObject.Find("Login_Button").GetComponent<Button>();
        registerButton = GameObject.Find("Register_Button").GetComponent<Button>();

        loginPanel.SetActive(true);
        registerPanel.SetActive(false);

        //注册相关事件
        loginButton.OnClickAsObservable().Subscribe(_ =>
        {
            Debug.Log("点击登录按钮！");
        });//点击按钮输出消息
        registerButton.OnClickAsObservable().Subscribe(_ =>
        {
            Debug.Log("跳转到注册页面！");
        });//点击按钮输出消息

        userNameInputField.OnEndEditAsObservable().Subscribe(result =>
        {
            Debug.Log("Username: " + result);
            passwordInputField.Select();//用户名输入完成后，选中密码框
        });//当结束编辑的时候，回调注册的方法，将用户名文本传递下去

        passwordInputField.OnEndEditAsObservable().Subscribe(result =>
        {
            Debug.Log("Password：" + result);
        });//当结束编辑的时候，回调注册的方法，将密码文本传递下去
    }*/
    #endregion

    #region 16、Unity 的生命周期与 Triggers：
    /*private void Start()
    {

        *//*//全局的事件监听，即使当前的脚本对象被删除，它还是会存在，如果回调方法内调用了当前脚本对象，会发生空指针异常
        //解决方法是使用 AddTo 将当前监听事件的任务与当前脚本的销毁绑定到一起，当前脚本对象销毁，当前任务跟着销毁
        Observable.EveryUpdate().Subscribe(_ =>//EveryUpdate 这个 API 是从一个叫 MainThreadDispatcher 单例中获取的（使用 UniRx 在运行时 Hierarchy 面板会自动创建处一个对象，展开后就有 MainThreadDispatcher）
        {
            Debug.Log("Update" + this.gameObject.name);
        });*/


    /*Observable.EveryUpdate().Subscribe(_ =>
    {
        Debug.Log("Update" + this.gameObject.name);
    }).AddTo(this);*//*

    //更简洁的写法：不需要 AddTo，当然也可以加上
    *//*this.UpdateAsObservable().Subscribe(_ =>
    {
        Debug.Log("UpdateAsObservable" + this.gameObject.name);
    });*//*
}*/
    #endregion

    #region 17、UI Trigger：
    /*Image _image;
    private void Start()
    {
        _image = GameObject.Find("17_Image").GetComponent<Image>();
        //如果要 OnBeginDragAsObservable 事件触发，必须把 OnDragAsObservable 和 OnEndDragAsObservable 也注册上
        _image.OnBeginDragAsObservable().Subscribe(_ =>
        {
            Debug.Log("Begin Drag");
        }).AddTo(this);

        _image.OnDragAsObservable().Subscribe(_ =>
        {
            Debug.Log("Darg");
        }).AddTo(this);

        _image.OnEndDragAsObservable().Subscribe(_ =>
        {
            Debug.Log("End Drag");
        }).AddTo(this);

        _image.OnPointerClickAsObservable().Subscribe(_ =>
        {
            Debug.Log("On Pointer click");
        });
    }*/
    #endregion

    #region 18、Coroutine 的操作：
    //UniRx 对 Unity 的 Coroutine 也提供?持，可以将?个 Coroutine 转化为事件源（Observable）
    /*IEnumerator CoroutineA()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("A");
    }
    private void Start()
    {
        Observable.FromCoroutine(_ => CoroutineA())//Couroutine A 执行后触发事件，执行订阅的方法输出 B
            .Subscribe(_ =>
            {
                Debug.Log("B");
            }).AddTo(this);
    }*/

    //UnitRx 也?持将 Observable 转化为?个 Coroutine 中的 yield 对象
    /*IEnumerator Delay1Aecond()
    {
        yield return Observable.Timer(TimeSpan.FromSeconds(1.0f)).ToYieldInstruction();
        Debug.Log("C");
    }
    private void Start()
    {
        StartCoroutine(Delay1Aecond());
    }*/
    #endregion

    #region 19、WhenAll:Coroutine 的并?操作
    //并行处理协程
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

    private void Start()
    {
        var aStream = Observable.FromCoroutine(_ => A());
        var bStream = Observable.FromCoroutine(_ => B());
        Observable.WhenAll(aStream, bStream)
            .Subscribe(_ =>
            {
                Debug.Log("并行处理 A、B 协程流");
            }).AddTo(this);
    }*/

    //处理“所有按钮都点击过”的逻辑
    /*Button _aButton;
    Button _bButton;
    Button _cButton;
    private void Start()
    {
        Button _aButton = GameObject.Find("19_A_Button").GetComponent<Button>();
        _aButton.GetComponentInChildren<Text>().text = "A 按钮";
        Button _bButton = GameObject.Find("19_B_Button").GetComponent<Button>();
        _bButton.GetComponentInChildren<Text>().text = "B 按钮";
        Button _cButton = GameObject.Find("19_C_Button").GetComponent<Button>();
        _cButton.GetComponentInChildren<Text>().text = "C 按钮";

        var aStream = _aButton.OnClickAsObservable().First();
        var bStream = _bButton.OnClickAsObservable().First();
        var cStream = _cButton.OnClickAsObservable().First();

        Observable.WhenAll(aStream, bStream, cStream).Subscribe(_ =>
        {
            Debug.Log("A、B、C 按钮都点击过一次了");
        }).AddTo(this);
    }*/
    #endregion

    #region 20、事件流的结束 OnCompleted
    /*private void Start()
    {
        *//*Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ =>
        {
            Debug.Log("OnNext");
        }, () =>
        {
            Debug.Log("OnCompleted");
        });*//*

        Observable.EveryUpdate().First().Subscribe(_ =>
        {
            Debug.Log("OnNext:First");
        }, () =>
        {
            Debug.Log("OnCompleted");
        }).AddTo(this);
    }*/

    /*IEnumerator A()
    {
        yield return new WaitForSeconds(2.0f);
    }

    private void Start()
    {
        Observable.FromCoroutine(A).Subscribe(_ =>
        {
            Debug.Log("OnNext");
        }, () =>
        {
            Debug.Log("OnCompleted");
        });
    }*/
    #endregion

    #region 21、Start 让多线程更简单：
    /*private void Start()
    {
        IObservable<int> threadAStream = Observable.Start(() =>
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            return 10;
        });
        IObservable<int> threadBStream = Observable.Start(() =>
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            return 10;
        });

        Observable.WhenAll(threadAStream, threadBStream).ObserveOnMainThread().Subscribe(result =>
        {
            //Debug.Log($"{result[0]}:{result[1]}");
            Debug.LogFormat("{0},{1}", result[0], result[1]);
        });
    }*/
    #endregion

    #region 22、ObservableWWW 优雅的网络请求操作：
    /*private void Start()
    {
        *//*ObservableWWW.Get("http://sikiedu.com")
            .Subscribe(responseText =>
            {
                Debug.Log(responseText);
            },exception =>
            {
                Debug.Log($"Error:{exception}");
            }).AddTo(this);*/

    /*IObservable<string> aStream = ObservableWWW.Get("http://sikiedu.com");
    IObservable<string> bStream = ObservableWWW.Get("http://sikiedu.com");
    Observable.WhenAll(aStream, bStream)
        .Subscribe(responseTexts =>
        {
            Debug.Log(responseTexts[0].Substring(0,10));
            Debug.Log(responseTexts[1].Substring(0,10));
        }).AddTo(this);*//*

    //QFramework 中?段下载?件的代码：
    //下载文件 url： http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage
    ScheduledNotifier<float> progressListener = new ScheduledNotifier<float>();
    ObservableWWW.GetAndGetBytes("http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage", progress:progressListener)
        .Subscribe(bytes =>
        {
            Debug.Log("将下载的文件放到某个地方");
        });
    progressListener.Subscribe(progress =>
    {
        Debug.Log($"进度为{progress}");//进度为 1 表示下载完成
    });
}*/

    #endregion

    #region 23、ReactiveCommand：
    //ReactiveCommand 的定义：
    /*public interface IReactiveCommand<T> : IObservable<T>
    {
        IReadOnlyReactiveProperty<bool> canExecute { get; }
        bool Execute(T parameter);
    }*/

    /*private void Start()
    {
        //新创建的 ReactiveCommand 默认 CanExecute 为 true：
        *//*ReactiveCommand command = new ReactiveCommand();
        command.Subscribe(_ =>
        {
            Debug.Log("command executed");
        });
        command.Execute();
        command.Execute();
        command.Execute();*//*

        //CanExecute 的开启关闭是由 Observable （事件源）决定的：
        *//*IObservable<bool> leftMouseClickStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => true);

        IObservable<bool> leftMouseUpStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Select(_ => false);

        IObservable<bool> mouseUp = Observable.Merge(leftMouseClickStream, leftMouseUpStream);

        ReactiveCommand reactiveCommand = new ReactiveCommand(mouseUp, false);//mouseUp 表示 ReactiveCommand 的事件源，canExecute 默认为 false；当事件源 mouseUp 传递 true （鼠标按下）时 canExecute 为 true
        reactiveCommand.Subscribe(x =>//没有传递过来参数，所以 x 表示的是 "()"
        {
            Debug.Log(x);
        });
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                reactiveCommand.Execute();
            });*//*

        //ReactiveCommand 也是可以被订阅(Subscribe) 的，在订阅之前也可以使? Where 等操作符进?事件操作：
        *//*ReactiveCommand<int> reactiveCommand = new ReactiveCommand<int>();

        reactiveCommand.Where(x => (x % 2 == 0))
            .Subscribe(x => Debug.Log($"{x} is even numbers ."));

        reactiveCommand.Where(x => (x % 2 != 0))
            .Timestamp()
            .Subscribe(x => Debug.Log($"{x.Value} is Odd,{x.Timestamp}"));

        reactiveCommand.Execute(2);
        reactiveCommand.Execute(3);*//*
    }*/


    #endregion

    #region 24、ReactiveCollection 与 ReactiveDictionary：
    /*ReactiveCollection<int> _ages = new ReactiveCollection<int>
    {
        1,2,3,4,5
    };
    private void Start()
    {
        _ages.ObserveAdd().Subscribe(addAge =>
        {
            Debug.Log($"添加的年龄 addAge 类型是 CollectionAddEvent<int> :{addAge}");
            Debug.Log($"添加的年龄在 ReactionCollection 中的下标:{addAge.Index}");
            Debug.Log($"添加的年龄的值:{addAge.Value}");
            Debug.Log("以下同理");
        });
        _ages.ObserveRemove().Subscribe(removeAge =>
        {
            Debug.Log($"移除的年龄:{removeAge}");
        });
        _ages.ObserveReplace().Subscribe(replaceAge =>
        {
            Debug.Log($"替换的年龄是：{replaceAge}");
        });
        _ages.ObserveMove().Subscribe(moveAge =>
        {
            Debug.Log($"移动的年龄是：{moveAge}");
        });
        _ages.ObserveCountChanged().Subscribe(count =>
        {
            Debug.Log($"年龄表数量改变为：{count}");
        });
        

        foreach(int age in _ages)
        {
            Debug.Log(age);
        }

        _ages.Add(6);
        _ages.Remove(2);
        _ages[4] = 7;
    }*/

    /*private ReactiveDictionary<string, string> languageCode = new ReactiveDictionary<string, string>()
    {
        {"en", "英语"},
        {"cn", "中文"}
    };
    private void Start()
    {
        languageCode.ObserveAdd().Subscribe(addLanguage =>
        {
            Debug.Log($"添加的语言键值对 addLanguage 类型是 DictionaryAddEvent<string,string> :{addLanguage}");
            Debug.Log($"添加的语言键值对的键:{addLanguage.Key}");
            Debug.Log($"添加的语言键值对的值:{addLanguage.Value}");
            Debug.Log("以下同理");
        });
        languageCode.ObserveRemove().Subscribe(removeLanguage =>
        {
            Debug.Log($"移除的语言键值对:{removeLanguage}");
        });
        languageCode.ObserveReplace().Subscribe(replaceLanguage =>
        {
            Debug.Log($"替换的语言键值对是：{replaceLanguage}");
        });
        languageCode.ObserveCountChanged().Subscribe(count =>
        {
            Debug.Log($"语言字典数量改变为：{count}");
        });


        foreach (KeyValuePair<string, string> keyValue in languageCode)
        {
            Debug.Log($"{keyValue.Key}:{keyValue.Value}");
        }

        languageCode.Add("jp", "日语");
        languageCode.Remove("en");
    }*/
    #endregion

    #region 25、 加载场景 AsyncOperation：
    /*private void Start()
    {
        ScheduledNotifier<float> progressObservable = new ScheduledNotifier<float>();
        SceneManager.LoadSceneAsync(1).AsAsyncOperationObservable(progressObservable)//异步加载下标为 1 的场景
            .Subscribe(asyncOperation =>//场景加载完成回调
            {
                Debug.Log("load done");
                Resources.LoadAsync<GameObject>("TestCanvas").AsAsyncOperationObservable()
                    .Subscribe(resourceRequest =>//异步加载资源（asyncOperation 继承了 AsyncOperation，表示请求的资源）
                    {
                        Instantiate(resourceRequest.asset);//实例化请求的资源
                    });
            });
        progressObservable.Subscribe(progress =>
        {
            Debug.Log($"加载了：{progress}");
        });
    }*/
    #endregion

    #region 26、TodoList 实战：
    // 代办事项
    [Serializable]
    public class TodoItem
    {
        /// <summary>
        /// 标示符
        /// </summary>
        public int id;

        /// <summary>
        /// 内容
        /// </summary>
        public StringReactiveProperty Content;

        /// <summary>
        /// 是否完成
        /// </summary>
        public BoolReactiveProperty Completed;
    }

    // 代办列表
    [Serializable]
    public class TodoList
    {
        public int topId;
        public void Add(string content)
        {
            todoItems.Add(new TodoItem
            {
                id = topId,
                Content = new StringReactiveProperty(content),
                Completed = new BoolReactiveProperty(false)
            });
            topId++;
        }
        public List<TodoItem> todoItems = new List<TodoItem> ()
        {
            new TodoItem()
            {
                id = -2,
                Content = new StringReactiveProperty("卖萝卜"),
                Completed = new BoolReactiveProperty(false)
            },
            new TodoItem()
            {
                id = -1,
                Content = new StringReactiveProperty("注册 sikiedu.com"),
                Completed = new BoolReactiveProperty(false)
            }
        };

        public void Save()//保存：序列化
        {
            PlayerPrefs.SetString("todoId", JsonUtility.ToJson(this));
        }

        public static TodoList Load()//加载：反序列化
        {
            string jsonContent = PlayerPrefs.GetString("todoId", String.Empty);
            if (string.IsNullOrEmpty(jsonContent))
                return new TodoList();
            else
                return JsonUtility.FromJson<TodoList>(jsonContent);
        }
    }
    #endregion

}
