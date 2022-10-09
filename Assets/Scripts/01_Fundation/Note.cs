using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;



/// 1��ʲô�� UniRx��
///     1��UniRx ��һ�� Unity3D �ı�̿�ܣ�רע�ڽ���첽�߼���ʹ�첽�߼���ʵ�ָ��Ӽ������
///     2��UniRx ȫ�� Unity Reactive Extensions���� Unity �汾�� Reactive Extensions
///     3��Reactive Extensions �ó�����ʱ�����첽���߼�
///     4��Rx ֻ��һ�ױ�׼��������������Ҳ��ʵ�֣������ Unity ����Ϥ�����ױ�׼��������������Ҳ�ǿ��Ժܿ����ֵģ����� RxJava��Rx.cpp��SwiftRx �ȵ�


/// 2��ΪʲôҪʹ�� UniRx��
///     1��UniRx ���ŵ㣺
///         1������ʵ���� UniRx ������һ��������¼������ܣ����ʹ�ô�ͳ�ķ�ʽʵ�֣��������ܴ󣬻�Ҫ����һ����Ա��������¼����������� Update �����м������̧���¼������� Update ������Ҫ����ܶ������߼�����ʹ����ӷ�ף������Ķ�
///         2��UniRx �ṩ��һ�ֱ��˼ά���������ɸ㶨ƽʱ�Ƚ���ʵ�ֵ��첽�߼��������飬���пɶ��ԡ�
///         3������֮�⣬UniRx ���ܣ�
///             1������ʵ�� MVP(MVC) �ܹ�
///             2���� UGUI/Unity API �ṩ����ǿ���ܶ���Ҫ��������� UI �߼�������ʹ�� UniRx ����ʵ��
///             3������ʵ�ַǳ����ӵ��첽������
///             4������
///         4��UniRx ����Ҫ���ǣ�����������������ǵı���Ч�ʣ��������ǵĴ����ṩ��һ��ǿ�����ı��ģ��
///         5��UniRx �ǳ�ֵ���о�ѧϰ���������������� uFrame ����� 1.6 �汾֮��Ҳʹ���� UniRx ���˴���ع����ײ�ʹ�� UniRx ǿ����������Ь��ʦ�� QFramework Ҳ������ UniRx����һ�����Ŀ���û�����Ϊ�� UniRx Ľ������
///     2�������ʹ�� UniRx������Ϸ�����л����ٵ����⣺
///         1����Ϸϵͳ�ܶ���ʱ�������첽�ģ�Unity ������Ҫʵ�ֵ��첽�����Ƿǳ���ģ����綯���Ĳ��š������Ĳ��š�����������Դ����/ж�ء�Tween���������ɵȶ�����ʱ�����첽���߼�����Ҳ��Ϊʲô Unity �ٷ��ṩ�� Coroutine��Э�̣��ĸ����������Ϸѭ��(Every Update,OnCollisionEnter��)������������(Kinect, Leap Motion,VR Input��)��Ҳ��ʱ�����첽���¼��������ڽ��������߼�ʵ�ֵ�ʱ�򾭳���Ҫ�õ������ص�������������Ŀ����չ�ᵼ�´�˵�еġ��ص���������
///         2�������������Ϻõķ�����ʹ�á���Ϣ/�¼����ķ��ͣ���������Ŀ���Żᵼ�¡���Ϣ����ɡ������󣬴���ǳ������Ķ�
///         3��ʹ��Э�̽����������Ҳ�Ƿǳ�����ģ����� Coroutine ����Ķ�������һ�������ĸ�ʽ����ģ�д���Ƿǳ�������̵ģ����߼���΢����һЩ�ͺ�������� Coroutine Ƕ�� Coroutine �����󣬴���ǳ��������Ķ���ǿ��ϣ�
///         4��UniRx �ĳ��ֺܺõؽ�����������⣬�����ڻص����¼�֮�䣬���¼��ĸ��ֻ���������¼�����ˮһ���������ģ�����Ҫ����ֻ�Ǽ򵥵ؽ�����֯���任�����ˡ��ϲ���
///             1���������� UniRx ��������������ӣ���Ҳ�õ��˻ص���ֻ�����¼���֮֯��ֻ��һ���򵥵Ļص��Ϳ��Խ����¼��Ĵ����ˣ�����ԭ��� Coroutine��������ģʽ���ǳ����ƣ����Ǳ� Coroutine ǿ��ö�
///             2��UniRx ����ʱ���ϣ��첽�¼�ת��Ϊ����Ӧʽ�ģ�ʱ�����У�����ͨ�� LINQ �����򵥵���֯���������⻹֧���¼�����
///     3���ܽ᣺ΪʲôҪ�� UniRx��
///         --��Ϸ�����д�����ʱ���ϵģ��첽�߼����� UniRx ǡ���ó�����ʱ���ϵģ��첽�߼���ʹ�� UniRx �������Խ�ʡ�¼������ظ��ô�����Ӽ���׶�
///     


///3����ʱ���ܣ���ʱ������ʵ�֣�
/// 1�����淽����ʹ�ñ����ۼ� Time.deltaTime��Ȼ���� Update �����н����߼��ж�
/// 2��Coroutine������Э�̣�Ȼ���� Start �����п���Э��
/// 3��UniRx���� Start ������һ�д������ʵ��


///4�������� Update��
/// 1��MonoBehavior �е� Update ����һ��ᴦ��������߼�������Щ�߼�֮��ܶ඼�ǻ��������ģ����紦���ɫ���˶���ͬʱ�����ܻ��ᴦ��һЩ��ʱ���ܣ�ʹ����ӷ�ײ����Ķ�
/// 2��ʹ�� UniRx ���Դ����������� MonoBehavior Update ����������ͬ�ĺ���


///5��AddTo��
/// 1��������⣺��ӵ�
/// 2�����⣺��ӵ����
///     --��ʵ������ӵ� Unity �� GameObject ���� MonoBehaviour
/// 3�����⣺ΪʲôҪ��ӵ� GameObject ���� MonoBehaviour��
///     --��Ϊ GameObject �� MonoBehaviour ���Ի�ȡ�� OnDestroy �����¼�
/// 4�����⣺�� OnDestroy �¼���ʲô��ϵ��
///     --UniRx ��Ҫͨ�� GameObject �� MoveBehavior ��ȡ�� OnDestroy �����¼����� UniRx ���������¼��İ󶨣��� GameObject �� MonoBehavior ������ʱ��ͬ���������ڽ��е� UniRx ����
/// 5���ܽ᣺AddTo �����ã�����ǰ�� UniRx �۲�����ĳ����Ϸ����(GameObject)�����(MonoBehavior)���������¼��İ󶨣�����Ϸ����(GameObject)�����(MonoBehavior)������ʱ����ǰ�Ĺ۲���Ҳһ������
/// 6���÷���Observable.Timer(TimeSpan.FromSeconds(1.0f))
///         .Subscribe()
///         .AddTo(MonoBehaviour/GameObject);
/// 7��Ϊʲô��
///     --AddTo ��һ����̬��չ�ؼ��֣��� IDisposable ��������չ��ֻҪʵ���� IDisposable �ӿڣ�������ʹ�� AddTo API�������ǲ��� UniRx �� API���� GameObject ����ʱ���ͻ���� IDisposable �� OnDIspose ����
/// 8��AddTo ��������ʲô��
///     --���� AddTo���ڿ��� Observable.EveryUpdate ���õ�ǰ�ű��ķ���ʱ������������쳣�ȴ������û�� AddTo����ǰ�ű������ٺ�Observable ���������У������ڵ��õ�ǰ�ű��ķ���������������쳣����ʹ UniRx ��ʹ�ø��Ӱ�ȫ


///6��UniRx �����﷨��ʽ��
/// 1��Observable.XXX().Subscribe() �Ƿǳ����͵� UniRx ��ʽ��ֻҪ�ѽ�ʲô��˼�Ϳ��Կ����󲿷� UniRx �����ˣ��磺
///     --Observable.Timer(TimeSpan.FromSeconds(1.0f))
///         .Subscribe()
///         .AddTo(MonoBehaviour/GameObject);
/// 2������/�ʻ㣺
///     1��Observable���ɹ۲�ģ����ݴʣ��������ݺ�������ʣ��� Observable.Timer �ɼ����Ϊ ���ɹ۲�ļ�ʱ������һ�����ǻ�� Observable ���Ϊ �۲��ߣ���ߵ��������Ϊ������
///     2��Timer����ʱ������ Observable �������Ƿ����ߣ����¼��ķ��ͷ������Ĳ�����ʾĳһ���¼��������ĳһ���¼���Timer �ͻᷢ���¼���������Ϣ
///     3��Subscribe�����ģ����ʣ���ʾ����ǰ�ߵ� Timer�����Ķ�ʱ�����¼������� Subscribe �������Ϊ�����ߣ�Ҳ�����¼��Ľ����ߣ�����ܵ� Timer ���͵���Ϣ��
///     4��AddTo������ǰ���д�������������̺�ĳ����Ϸ����(GameObject)�����(MovoBehavior)�������¼��󶨵�һ�𣬵����Ǳ�����ʱ����ǰ���д��������Ҳ��������
///     5�����������ǣ����Ŀ��Ա��۲�Ķ�ʱ��
/// 3��ע�⣺��Ȼ�ӷ����ߺͶ����ߵĽǶȸ������ UniRx������ UniRx �Ĳ��ص㲢���ڷ����ߺͶ������������������ʹ�ã������¼��ӷ����ߵ�������֮��Ĺ�����δ���Ҫ��ע�Ĳ��Ƿ����ߺͶ����ߣ���������֮��������ߣ����¼����ݵĹ��̣������˽���Щ������������ݺ����������壩


///7�������� Where��
/// 1���� ��6��UniRx �����﷨��ʽ�� ���ᵽ��UniRx �Ĳ��ص㲻�ڷ����ߺͶ�����������������������¼��ӷ����ߵ�������֮��Ĺ�������δ���ġ���ô������δ�����أ��ȿ�һ�δ��룺
///     --
///     Observable.EveryUpdate()
///         .Subscribe(_ => 
///         {
///           if(Input.GetMouseButtonUp(0))
///           {
///             //do something
///           }
///         });
///     --��δ���ʵ�������ĵ���¼���������δ��뻹�������,����ʹ�� Where ������ʹ������ࣺ
///     --Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonUp(0))
///         .Subscribe(_ =>
///         {
///             //do something
///         }
/// 2��Where �������� if ��䣬�����жϣ���Ҫ������һ�����˲��������˵��������������¼�
/// 3��������ͣ�
///     1��EveryUpdate���¼��ķ����ߣ�ÿ֡����һ���¼�����
///     2��Subscribe���¼��Ľ����ߣ����� EveryUpdate ���͵��¼�
///     3��Where�����¼��ķ����ߺͽ�����֮���һ�����˲���������˵��������������¼������� Subscribe ������¼��������� Where �������¼�
/// 4���¼��ı�������ǲ��������� EveryUpdate û�в����������� Where ���д����в���Ҫ���ܲ���������ʹ�á�_������ʾ���ò�������Ȼ���Ͳ��ò�������ô Subscribe Ҳ����һ����_�������ܲ���


///8�������� First��
/// 1��First() ��������ʾ��ֻ���˵�һ�������������¼���
/// 2�������´��룺
///     --
///     Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .First()
///         .Subscribe(_ =>
///         {
///             //do something
///         })
///         .AddTo(this);
///     --Observable.EveryUpdate ÿ֡�ᷢ��һ���¼�����
///     --Where ��������� Observable.EveryUpdate ���͵��¼����й��ˣ�ֻ�������������¼��Ż�������´���
///     --First ��������� Where ���˵Ĳ������������ˣ�ֻ�� Where ���˵ĵ�һ���¼��Żᱻ������ȥ
///     --Subscribe ֻ����յ�һ������ Where �������¼�����������¼��ǵ�һ������ Where �������¼�
///     --AddTo������ǰ���д�����ĳ����Ϸ���� GameObject ��ĳ����� MonoBehaviour �������¼��󶨵�һ�𣨹�������
///     --��Ȼ�����뻹���Ը���ࣺ
///     Observable.EveryUpdate()
///         .First(_ => Input.GetMouseButtonDown(0))
///         .Subscribe(_ =>
///         {
///             //do something
///         })
///         .AddTo(this);


///9���� UGUI ��֧�֣�
/// 0��ԭ���� UnityEvent �ṩ�� AsObservable ����
/// 1��UniRx �� UGUI ������֧��:
/// 
///     --������õİ�ť����¼�ע��:
///     Button button = GameObject.Find("09_Button").transform.GetComponent<Button>();
///     button.OnClickAsObservable()
///         .Subscribe(_ =>
///         {
///             Debug.Log("do something");
///         });
///     --��Ȼ����ťҲ����ʹ�� First �������������ťֻ�ܱ����һ��
///         
///     --Toggle:
///     Toggle toggle = GameObject.Find("09_Toggle").transform.GetComponent<Toggle>();
///     toggle.OnValueChangedAsObservable()
///         .Subscribe(on =>//on ��ʾ Toggle �Ƿ�ѡ��
///         {
///             Debug.Log(on);
///         });
///     --��Ȼ��Toggle Ҳ����ʹ�� Where ������        
///     toggle.OnValueChangedAsObservable()
///         .Where(on => on)//ֻ���� on Ϊ True ���¼�
///         .Subscribe(on =>
///         {
///             Debug.Log(on);
///         });
///     
///     ����
/// 2��UniRx ��֧�� EventSystem �ĸ��� Trigger �ӿڵļ�����
///     1�����磬Image ������ Graphic ���͵ģ�Graphic ��ֻҪʵ�� IDragHandler �Ϳ��Խ�����ק�¼�����(Graphic ��������л���һ�� Raycast target ���ԣ������Թ�ѡ֮��Ϳ���������ק�ˣ�Text ���Ҳ���������)
///     2������ʹ�� UniRx ������ק�¼��ļ����Ͳ�����ô�鷳--�������£�
///         using UniRx.Triggers;//��Ҫ�ȵ���������ռ�
///         image.OnBeginDragAsObservable().Subscribe(_ => {})
///         image.OnDragAsObservable().Subscribe(eventArgs => {})
///         image.OnEndDragAsObservable().Subscribe(_ => {});
///         //ע�⣬һ��Ҫ�����������¼������м��������ֻ��������һ����û��Ч��
///     
/// 3��Unity �� Event Ҳ����ʹ�� AsObservable ���ж��ģ�
///     UnityEvent event;
///     void Start()
///     {
///         event.AsObservable()
///             .Subscribe(_ =>
///             {
///                 //process event
///             });
///     }
/// 4����֮��UniRx �� Unity ���˷ǳ������ǿ


///10��Reactive Property ��Ӧʽ���ԣ�
/// 1����Ӧʽ���� Reactive Property �� UniRx һ���ǳ�ǿ��ĸ���
/// 2����Ӧʽ���Ե�ǿ��֮�����������Դ���һ�б��������Ҹ����������˺ܶ๦��--�������һ��������ֵ�Ƿ����ı䣺
///     1������ʵ�ַ�ʽ��
///     
///         private int age;
///         public int Age
///         {
///             get{ return age;}
///             set
///             {
///                 //����ֵ�ĸı�
///                 if(age != value)
///                 {
///                     age = value;//ֵ�����ı�
///                     OnAgeChange();//���÷���
///                 }
///             }
///         }
///     
///         public void OnAgeChange()
///         {
///             Debug.Log("���䷢���ı�");
///         }
///     
/// 
///     2��ʹ��ί��ʵ�֣�
///         --���ϵ�ֵ�����ı�ʱ����õķ���������ڲ��������������ⲿ�������ֵ�ĸı䣬����Ҫʹ��ί���ˣ�ί�еĳɱ��Ƚϵͣ��ǿ��Խ��ܵ�
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
///             OnAgeChanged += age =>{ Debug.Log("����ı�Ϊ" + Age);}
///         }
///             
/// 
///     3��ʹ�� UniRx ʵ�֣�
///         //��Ӧʽ���� Age����ʼֵΪ 0
///         //public ReactiveProperty<int> Age = new ReactiveProperty<int>(0);//�������л����鷳
///         public IntReactiveProperty Age = new IntReactiveProperty(0);//ʹ�� IntReactiveProperty Ч��һ�������л�����
///         private void Start()
///         {
///             Debug.Log("��ʼ�����ǣ�" + Age);
///             Age.Subscribe(age => { Debug.Log("���ڵ������ǣ�" + age); });
///         }
///     
/// 3��ʹ����Ӧʽ���������� Age�ĺô����ڣ�
///     1��Age ���Ա� Subscribe ���
///     2��֧�� First��Where �Ȳ�����
///     3����������ʵ�� MVP �ܹ�ģʽ���� Controller �н��� Model �� View �İ󶨣�Model ���������Զ��� ReactiveProperty��Ȼ���� Controller �н��ж��ģ�ͨ�� View ���� Model ������ֵ���γ�һ�� View->Controller->Model->Controller->View ��ôһ���¼���Ӧ��


///11��MVP ��ܵ�ʵ�֣�MVP �� MVC ��ܵĸ����棩��
/// 1���� ��9���� UGUI ��֧�֡��н�����UniRx �� UGUI ��ǿ��ԭ��ܼ򵥣����Ƕ� UnityEvent �ṩ�� AsObservable ����
///     --�磺
///     Button button = GameObject.Find("Button").transform.GetComponent<Button>();
///     button.OnClick.AsObservable().Subscribe(_ => Debug.Log("clicked"));
///     --AsObservable ����Դ�����£�
///     public static IObservable<Unit> OnClickAsObservable(this Button button)
///     {
///         return button.onClick.AsObservable();
///     }
///     --�ɼ������õ��� button �� onClick �� AsObservable �������� UniRx �� UGUI ����ǿ���Ƕ� UnityEvent �ṩ�� AsObservable ����
///     --�ڴ˻����ϣ���һ����ÿ�� UGUI �ؼ����з�װ���Ӷ����������·�ʽ�� UGUI ��ʹ�� UniRx
///         //�� Toggle ֵ�ı�ʱ�� button �ɽ���
///         toggle.OnValueChangeAsObservable().SubscribeToInteractable(button);
///         
///         //��������ֵ�ı�ʱ
///         input.OnValueChangedAsObservable()
///             .Where(x => x != null)//ֻҪ�����ֵ��Ϊ�գ�
///             .SubscribeToText(text);//���ı���ʾ������
///         
///         //�����鷢���ı�ʱ����ʾ�ı�
///         slider.OnValueChangedAsObservable()//
///             .SubscribeToText(text, x => Math.Round(x,2).ToString());
///            
/// 
/// 2��������Ӧʽ����֮�⣬������Ӧʽ���� ReactiveCollection
/// 
/// 
/// 
/// 3����ʵ�� MVP ģʽ֮ǰ���ȿ����´��룺
///     public class Enemy
///     {
///         public ReactiveProperty<long> CurrentHp{ get; private set;}//ֻ�����������޸� currentHP
///         public IReadOnlyReactiveProperty<bool> IsDead{ get; private set;}
///         
///         public Enemy(int initialHP)//��ʼ�� Enemy ��Ѫ�����Ƿ�����״̬
///         {
///             CurrentHp = new ReactiveProperty<long>(initialHp);
///             IsDead = CurrentHp.Select(x => x <= 0).ToReactiveProperty();//��� x С�ڵ��� 0����ʾ������������֮��������״̬
///             //x <= 0 ���ʽ�᷵��һ������ֵ��ToReactiveProperty ����ݱ��ʽ���صĲ���ֱֵ�Ӵ���һ����Ӧ�� ReactiveProperty���ڴ��������� ReactiveProperty<bool>�� ʵ�������أ���ֵ�� IsDead��
///         }
///     }
///     
///     void Start()
///     {
///         button.OnClickAsObservable().Subscribe(_ => enemy.CurrentHp.Value -= 99);//�����ť������Ѫ���� 99
///         enemy.CurrentHp.SubscribeToText(MyText);//������Ѫ����ʾ���ı�����
///         enemy.IsDead.Where(isDead => isDead)//������˴�������״̬���ð�ť���ɽ����������ټ�Ѫ�ˣ�
///             .Subscribe(_ =>
///             {
///                 button.interactable = false;
///             });
///     }
///     
///     --�������ϴ��룬��ʵ�ǳ�����⣺
///         1��Enemy �൱��һ�� Model
///             --Enemy �б��������ݣ���һ�������࣬�������Ϊ Model
///         2��Unity �� UI �����൱�� View
///         3��Start �������ڵ����൱�� Controller
///             --Start �����еĴ��뽫 Hierarchy �е� UI �ؼ� �� Model ������?�𡣵� Model ���ݸı�ʱ֪ͨ UI ���£��� UI ���յ�����¼��� Model ��?ֵ�ĸ���
///         4�����ϴ����� Unity �о��൱��һ�� MVC ���С�����������?��?���򵥵� MVP ģʽ
///   
/// 
/// 
/// 
/// 4�����ǿ����� UnityEvent.AsObservable �� ReactiveProperties��ReactiveCollections �� Observables ��������������� UI ������ṩ�� XXXAsObservable
/// 5���� Unity ����л���һ������Ҫ�Ĺ��ܣ�����������л������ڱ༭���ϾͿ������������� ReactiveProperty �Ƿ��͵ģ����л������Ƚ��鷳��Ϊ�˽��������⣬UniRx ֧���˿����л��� ReactiveProperty ���ͣ����� Int/Long/Float/Double/String/BoolReactiveProperty��
/// 6�������Զ����ö�� ReactiveProperty��дһ���ɼ��ӵ� ReactiveProperty[T] Ҳ������
/// 7�������Ҫ[Multiline] ���� [Range] ��ӵ� ReactiveProperty �ϣ�����ʹ�� MultilineReactivePropertyAttribute �� RangeReactivePropertyAttribute �滻 Multiline �� Range
/// 8����Щ InspectableReactiveProperties ������ Inspector �����ʾ�����ҵ����ǵ�ֵ�����仯�ǻᷢ��֪ͨ�������ڱ༭����仯Ҳ����
/// 9����� ReactiveProperty ��ֵֻ�� Stream �и��£�����ʹ? ReadOnlyReactiveProperty ���������ֻ��
/// 
/// 
/// 
/// 10��MVP ���ģʽ��Model-View-��Reactive��Presenter Pattern
///     1��ʹ�� UniRx ���Ժܷ���ʵ�� MVP(MVRP) ģʽ���������ӣ�
///     2��ΪʲôӦ���� MVP ģʽ������ MVVM ģʽ�أ�
///         1��MVVM(Model-View-ViewModel)�Ƕ� MVC(Model-View-Control)�� MVP(Model-View-Presenter)�Ľ�һ���Ľ�
///         2��Unity û���ṩ UI �󶨻��ƣ�����һ���󶨲���ڸ��Ӳ��һ���������Ӱ�죨��Ϊ��ʹ�õ����䣩
///         3��Unity ����ͼ��UI����Ҫ���£�Presenters ��֪�� View ������ܸ������ǣ�����ʹ�� Mvp ģʽ���ã���Ȼ MVP ģʽû������ȥ�� UI���� Observables ����֪ͨ�����ߣ�������Ҳ��ࡣ����ģʽ���� Reactive Presenter
///     3������� Unity �е� MVP ģʽ��
///         1���� Unity �У����ǰ� Scene �е� GameObject ������ͼ�㣬��Щ���� Unity �� Hierarchy �ж���ġ�
///         2��չʾ/���Ʋ��� Unity ��ʼ��ʱ����ͼ��󶨡�SubscribeToText �� SubscribeToInteractable ���Ǽ������ư󶨵ĸ�����������Ȼ��Щ?�ߺܼ򵥣�����?��ʵ?���� Unity ��ʹ?��������?��ƽ��������Ҳ�ܺã�����Ҫ���ǿ��������ǵĴ�������
///         3��View -> ReactiveProperty -> Model -> RectiveProperty -> View ����ȫ?��Ӧʽ��?ʽ���ӣ�UniRx �ṩ�����е�����?�����࣬���������� MVVM (or MV*) ���Ҳ����ʹ?
///         4��UniRx / ReactiveProperty ֻ��?���򵥵�?�߰�


///12��Merge ��������
/// 1��Merge �Ǻϲ�����˼
/// 2�������ǣ��ϲ�ʲô��
///     --�� UniRx ��������κζ����������¼�������ʽ���ڵ�
///     --��֮ǰ�� EveryUpdate��Timer �ȵȶ��ǿ�����һ���¼��������ǣ�UniRx ֻ�ܿ���һ���¼�����
///     --���ǣ�UniRx ���Կ�������¼�����������ʹ�� Merge �����¼����ĺϲ�
///     --����Merge �ϲ������¼���
/// 3���磺
///     var leftMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
///     var rightMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(1));
///     Observable.Merge(leftMouseClickStream,rightMouseClickStream)
///         .Subscribe(_ =>
///         {
///             Debug.Log("�����������������Ҽ�");
///         });
///     --���ϴ���ʵ�ֵ��߼��ǣ������������Ҽ����ʱ������д���
///     --����Merge �������� leftMouseClickStream �� rightMouseClickStream �ϲ�����һ���¼���
/// 4��Merge ���ڴ��������Ĳ�����������֮�⻹�и������ƵĲ������� Zip �ȡ�ѧϰ������֮�����ʵ��?�����ӵ��߼���?�� Coroutine ��?�֣�����ʵ�ְ�˳��ִ?��� Coroutine ��Ҳ?�ֵȴ����� Coroutine ִ?������ָ��ӵĲ�����


///13��ʵ�֣���ĳ����ť���ʱ��ʹ���е�ǰҳ��İ�ť���ɱ���������� Select ��������
/// 1��ע�⣺��Ȼ�����еㆪ�£���������������Ǿ���������
/// 2������������������һ���ʵ���Ǽ���һ���¼������ֲ㣬���ߴ���һ�� bool ���б�ǣ�����ʹ�� UniRx ��򵥵öࣺ
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
///                     Debug.Log("A/B/C ��ť�������");
///                 });
///         }
///     }
/// 3�����ϵĴ���ʵ�ֵ���������ť������һ����ť������ʹ����¼������������Ҫ֪���ĸ���ť�������Ӧ����ô����
///     --ʹ�� Select ����������
/// 4��Select ��������
///     1��Select ��ѡ�����˼
///     2��Select ������������ Linq �Ĳ�������һ���Ǵ���һ������ index ��Ȼ������������ؾ����ֵ
///     3������һ�� List��ʲô���� Select�أ������´��룺
///         var testNumbers = new List<int>(){ 1,2,3};
///         var selectedValue = testNumbers[2];
///         --���� testNumbers[2] ����һ��ѡ�����
///     4��Select ���������ں���������ϸѧϰ
///     5��ĿǰֻҪ֪����ʹ? Select ��������᷵��?��ֵ��ֵ��������?������


///14��Unity �� Rx��
/// UniRx ֮���Խ� UniRx ����Ϊ���� Unity �汾�� Reactive Extensions��Unity ����Ӧʽ��չ���������� Unity ���˺ܶ๦���ϵ���ǿ
///     1��UI ��ǿ
///     2��GameObject/MonoBehaviour ��ǿ�Լ�������¼���ǿ�� OnApplicationPause��UnityEvent
///     3��Coroutine/Thread ��ǿ
///     4����������WWW �ȣ���ǿ
///     5��ReactiveProperty��ReactiveCollection��ReactiveDictionary ��
///     6��ReactiveCommand ����ϵͳ
///     ��������


///15��UI ��ǿ + ��¼ע��ҳ�����ϰ��
/// 1����Ϊ��ѧ�ߣ����ճ������нӴ����ľ��� UGUI���� UGUI �Ŀ��������Ҫ��ѭ MVC ģʽ
/// 2��MVC ģʽ�Ժܶ�����˵��һ���ǳ�ģ���ļܹ�ģʽ�����Ǳ��ʺܼ򵥣���취�ѱ��ֲ�����ݲ���뼴�ɣ�Ҳ���Ƿ��� View �� Model
/// 3��Unity ʵ�� MVC �ķ�ʽ�ɺܶ��֣��� UniRx �� ReactiveProperty ������ȫʵ�� MVC ��һ�����֡���MVP��UniRx �� MVP ��ʵ�����Ƿǳ���ȷ�ģ��������ڿ�����ʱ���þ�����ôʵ�֣�������һ����� UGUI �Ŀ������˺ܶ�
/// 4������֮�⣬UniRx ��֧�ַǳ���� UGUI �ؼ������е� UGUI �ؼ�֧�����£�
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
/// 5�����Ͼ������� Observable ֧�֣���Ȼ��֧�� Subscribe ��ǿ���� SubscribeToText
///     --
///     Text resultText = GetComponent<Text>();
///     inputField.OnValueChangedAsObsservable().SubscribeToText(resultText);
///     --
///     1����δ���ʵ�ֵĹ����ǣ��� inputField ������ֵ�ı�ʱ����������ʾ�� resultText �ϣ������ inputField �� resultText �� ��
/// 6������֮�⻹֧�� SubscribeToInteractable����������Щ�͹�����
/// 7����ϰ����¼ע����棺���·� ��15��UI ��ǿ + ��¼ע��ҳ�����ϰ + AddTo��飺�� ����
/// 


///16��Unity ������������ Triggers��
/// 1������ Unity �������ڵ� Observable ��ǿ������ Update ��֧�ַǳ���ϸ�����͵� Update �¼�����
///     --
///     Observable.EveryUpdate.Subscribe(_ => {});//�൱�� Update
///     Observable.EveryFixedUpdate().Subscribe(_ => {});//�൱�� FixedUpdate
///     Observable.EveryEndOfFrame().Subscribe(_ => {});//����ÿ֡�Ľ���
///     Observable.EveryLateUpdate().Subscribe(_ => {});//�൱�� LateUpdate
///     Observable.EveryAfterUpdate().Subscribe(_ => {});//���� Update �Ľ���
///     --
/// 2������ Update ��֧�������¼����� ApplicationPause��Quit �ȣ�
///     --
///     Observable.EveryApplicationPause().SUbscribe(paused => {});//����������ͣ��paused ��ʾ�����Ƿ���ͣ
///     Observable.EveryApplicationFocus().Subscribe(focused => {});//�Ƿ�۽��ڳ���������ڳ���֮��������ʾû�о۽������� false
///     Observable.EveryApplicationQuit().Subscribe(_ => {});//���������˳�
///     //ѧϰ������Щ������??����Ϳ���ʵ�����硰Ӧ?�����˳��¼����������߼��ˣ���?�ٴ���?��������ȥʵ��
///     --
/// 3��Trigger (��Ҫ���������ռ� UniRx.Trigger��ʹ���� Trigger �Ľű����ڵĶ���������ʱ���Զ���� Observable Update Trigger.cs)��
///     1����ʱ����ĳ���ű���ʵ�� Observable.EveryUpdate() ��� API ��Ҫ�� MonoBehaviour ���������ڣ���Ҫ�ǽ���󶨵� OnDestroy����Ȼ�� API �е�ʱ��Ҳ����ȫ��?����Զ���ᱻ���ٵģ����󶨷�ʽ?���򵥣�ֻ��Ҫ?�� AddTo �Ϳ��Խ�?���ˣ��������£�
///         --
///         Observable.EveryUpdate().Subscribe(_ => {}).AddTo(this);
///         //�����д����
///         this.UpdateAsObservable().Subscribe(_ => {});//UpdateAsObservable �������͵� Observable ���� Trigger����������
///         --
///     2��ʲô�� Trigger��
///         1���������⣬�������ǹ��� GameObject ���������� GameObject ĳ���¼���?�Ĺ��ܽű����¼���?ʱ��ص���ע������ Subscribe ������ĳ���¼���?ʱ���Ὣ���¼����͵� Subscribe ������
///         2���������Ĳ����������¼�Դ (Observable) ��?���ģ���?�� Where��First��Merge �Ȳ�����
///         3��Trigger ���͵� Observable ��֮ǰ��ѧ�� Observable �ڱ������е㲻?��:
///             1��Trigger ?���ֶ����� XXXAsObsrevable ������ʽ��
///             2����ʹ? Trigger �� GameObject �϶�����϶�Ӧ�� Observable XXXTrigger.cs �ű�
///         4����֮ǰ������ʵ���Ѿ��Ӵ��� Trigger ��AddTo() ��� API ��ʵ�ͷ�װ��?�� Trigger: ObservableDestroyTrigger������˼�壬���ǵ� GameObject ����ʱ��ȡ�¼���?����������ʹ�� AddTo ���൱��ʹ���˴�������������ʱ�ű����ڵĶ�����Զ���� Observable Update Trigger.cs
///         5��?��� Trigger ������� MonoBehaviour ?��ʹ?��?�� ObservableDestroyTrigger:
///             --
///             this.OnDestroyAsObservable().Subscribe(_ => {});
///             --
///         6������ Destroy ����?����� Trigger��
///             1������ϸ�����͵� Update��
///                 --
///                 this.FixedUpdateAsObservable().Subscribe(_ => {});
///                 this.LateUpdateAsObservable().Subscribe(_ => {});
///                 this.UpdateAsObservable().Subscribe(_ => {});
///                 --
///             2��������ײ�� Trigger:
///                 --
///                 this.OnCollisionEnterAsObservable(collision => {});
///                 this.OnCollisionStayAsObservable(collision => {});
///                 this.OnCollisionExitAsObservable(collision => {});
///                 --
///                 this.OnCollision2DEnterAsObservale(collision2D => {});
///                 this.OnCollision2DStayAsObservable(collision2D => {});
///                 this.OnCollision2DExitAsObservable(collision2D => {});
///                 --
///             3�����ֽű��Ĳ���������
///                 --
///                 this.OnEnableAsObservable().Subscribe(_ => {});
///                 this.OnDisableAsObservable().Subscribe(_ => {});
///                 --
///         7������ MonoBehaviour ��Trigger Ҳ?��������������ͣ��� RectTransform��Transform��UIBehaviour �ȵȣ�����ɲ鿴 ObservableTriggerExtensions.cs �� ObervableTriggerExtensions.Component.cs �е� API��


///17��UI Trigger��
/// 1��֮ǰѧ�� Observable �� UI ����ǿ��Trigger Ҳ�� UI ����ǿ���֣���16�����н��� Trigger ���� MonoBehaviour ��?�������������� Transform��RectTransform��UIBehaviour������ UIBehavior ���Ƕ� UI ����ǿ��Ҳ�Ǳ��ڵ��ص�
/// 2��Ϊʲô˵֧�� UIBehavior ���Ƕ� UI ����ǿ��
///     --��Ϊ UIBehaivour �� UGUI ���пؼ��Ļ��࣬����ֻҪ?�� UIBehaivour�����е� UGUI �ؼ�����̳� UIBehaviour ��?��
/// 3��Trigger ���ķ���� UI ����֧�֣�
///     1��Trigger �ǴӸ����¼���ʼ?�� UI �ģ�?�����е� Graphic ���Ͷ�?�� OnPointerDownAsObservable��OnPointerEnterAsObservable��OnPointerExitAsObservable �� Trigger��
///     2��ʲô�� Graphic�������� Inspector �Ϲ�ѡ�� Raycast Target ѡ����Ķ��� Graphic ���ͣ�����Image��Text ��
///     3��Ҳ����˵��Image��Text ȫ��?�� OnPointerDownAsObservable��OnPointerEnterAsObservable �� Trigger
///     4������֪���������??ȥ����?�� OnPointerDown �¼�����Ҫʵ��?�� IPointerDownHandler ��?��? UniRx ������е� IXXXHandler ��?������ Trigger�ˣ���������Ҳ��?��Ҫ?�ϵ��������� UIEventListener.Get(gameObejct).onClick ����?ʽ�ˣ���Ϊ����?ʽ����ܶ࣬?�磬�������̳��� EventTriggers��ʵ�������е��¼���?�����ͻ����ɵ� OnScroll ���¼������Ա���������ܱ���������? UniRx ��ʵ��?��ϸ��?�� IXXXHandler ����?�� Trigger��
///     5����Ҫ?��ȫ��ʵ�ֲ����̲��¼��İ汾Ҳû��ϵ��UniRx Ҳʵ����?�� ObservableEventTrigger���� UIEventListener ?����
///     6������?��?��?�϶��?�� Trigger:
///         ---
///         image.OnBeginDragAsObservable().Subscribe(dragEvent => {});
///         graphic.OnDragAsObservable().Subscribe(dragEvent => {});
///         text.OnEndDragAsObservable().Subscribe(dragEvent => {});
///         image.OnPointerClickAsObservable().SUbscribe(clickEvent => {});
///         //��Щ API ��ʹ��?��?�㣬���� QFramework ��?Щ�ű�����?��
///         --
///     7�����˳�?��?�� Trigger ֮�⻹��?����ʵ?�� Trigger���� OnSubmitAsObservable��OnDropAsObservable �ȣ�������Բο� ObservableTriggerExtensions.Component.cs��ֻҪ���뵽�Ļ����� UniRx ��?�֡�


///18��Coroutine �Ĳ�����
/// 1��UniRx �� Unity �� Coroutine Ҳ�ṩ?�֣����Խ�?�� Coroutine ת��Ϊ�¼�Դ��Observable��
///     --
///     IEnumerator CoroutineA()
///     {
///         yield return new WaitForSeconds(1.0f);
///         Debug.Log("A");
///     }
///     private void Start()
///     {
///         Observable.FromCoroutine(_ => CoroutineA())//Couroutine A ִ�к󴥷��¼���ִ�ж��ĵķ������ B
///             .Subscribe(_ =>
///             {
///                 Debug.Log("B");
///             }).AddTo(this);
///     }
///     --
/// 2��UnitRx Ҳ?�ֽ� Observable ת��Ϊ?�� Coroutine �е� yield ����:
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
/// 3��FromCoroutine �� ToYieldInstruction ʵ���� Observable �� Coroutine ֮��Ļ���ת��
/// 4��֮ǰѧ����Observable ��?���¼������� UniRx �Ĳ������� Merge ���Խ�����?�ϲ��Ӷ��������¼�������Ȼ���˺ϲ�Ҳ?�ֱ�Ĳ�����?��˳�� (����) ִ? Coroutine����?ִ? Coroutine �ȵȡ���֮��ͨ��ѧϰ�µĲ������������� Coroutine ����ǿ?��


///19��WhenAll:Coroutine �Ĳ�?����
/// 1��WhenAll ��˼�� ����> �����еģ���ʾ����˼�ǣ������е��¼������������ͻᴥ�� Subscribe ע��Ļص�
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
///                 Debug.Log("A��B �¼�����������");
///             }).AddTo(this);
///     }
/// 2��WhenAll �� Merge ��ͬ���͵ģ����Ǵ��������Ĳ�����
/// 3��WhenAll ���˲�?ʵ�� Coroutine ֮�⣬������ʵ�֡������еİ�ť�������?�ε��߼���:
///     Button _aButton;
///     Button _bButton;
///     Button _cButton;
///     private void Start()
///     {
///         Button _aButton = GameObject.Find("19_A_Button").GetComponent<Button>();
///         _aButton.GetComponentInChildren<Text>().text = "A ��ť";
///         Button _bButton = GameObject.Find("19_B_Button").GetComponent<Button>();
///         _bButton.GetComponentInChildren<Text>().text = "B ��ť";
///         Button _cButton = GameObject.Find("19_C_Button").GetComponent<Button>();
///         _cButton.GetComponentInChildren<Text>().text = "C ��ť";
///         var aStream = _aButton.OnClickAsObservable().First();
///         var bStream = _bButton.OnClickAsObservable().First();
///         var cStream = _cButton.OnClickAsObservable().First();
///         Observable.WhenAll(aStream, bStream, cStream).Subscribe(_ =>
///         {
///             Debug.Log("A��B��C ��ť�������һ����");
///         }).AddTo(this);
///     }


///20���¼����Ľ��� OnCompleted��Ҳ��Ϊʲô WhenAll ���Խ��ܵ������¼��Ĺؼ�ԭ�򣩣�
/// 1��OnCompleted��UniRx �Ľ����¼���UniRx �¼��������������ڵģ�OnCompleted �ɿ����������ڵĽ�����
/// 2��ע�⣺������ÿ���¼������н����¼��ģ����� Timer��First��Coroutine ���н����¼����� EveryUpdate ��û�н����¼��ģ�����ÿִ֡��һ�β�һֱִ����ȥ����Ҳ������Ϊʲô��Ҫʹ�� AddTo ������ĳ����Ϸ����� OnDestroy �󶨵�һ����ʵ����Ϊ���һ�������������ڵĺ�����
/// 3��ʹ? Subscribe API ��?���ĵ�ʱ�򣬵�?�������� OnNext �ص���ע�ᣬ��Ҳ������?���������ʹ?�Ļص�����?���������� OnComplete��


///21��Start �ö��̸߳��򵥣�
/// 1�����߳�����Ϊ?�������߱���߱���?�ּ������˽��˶��߳̿��������ǳ����?����ƶ��˵ļ������ƣ�Ҳ���������ǵ���Ϸ�����ƽ����
/// 2���� Unity ������?��? Thread.Start ����?���̣߳����߼�?�����ӵ�ʱ����߳�?�����Թ���? UniRx ��������?��״����
/// 3�����������߳���?��ɺ������߳�ִ?ĳ������ ������ܣ�ʹ? UniRx ʵ������:
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
///         Observable.WhenAll(threadAStream, threadBStream)//��Ϊ�߳��п������н��������� WhenAll ���Խ��յ��̵߳� OnCompleted
///             .ObserveOnMainThread().Subscribe(result =>
///             {
///                 //Debug.Log($"{result[0]}:{result[1]}");
///                 Debug.LogFormat("{0},{1}", result[0], result[1]);
///             });
///     }
/// 4����?�������µ� API��
///     1��Observable.Start������?���߳���
///     2��ObserveOnMainThread���� WhellAll ���ת�����߳��ϡ�
///     --���� Subscribe ?�Ļص��Ϳ���ʹ? Unity �� API �ˣ�Unity �ܶ� API �������������߳���ʹ?����������߳���ĳһʱ��ʹ�� Unity �� API������ʹ�����ַ�ʽ��ĳһʱ�̱�ʾ�߳���ɣ�Ȼ���� WhenAll ��ʹ�� Unity �� API��
/// 5��ʹ? UniRx �����߳��߼�?�����㣬�̺߳� Coroutine (Э�̣�������ʹ? WhenAll ���������� WhenAll ֮�⻹�кܶ������Ĳ���������ʹ�ã���֮������ѧϰ��


///22��ObservableWWW ���ŵ��������������
/// 1������������ʹ? WWW ���� UnityWebRequest ��Ҫʹ? Coroutine ȥ��������ʹ?Э��д�����Ĵ�����Ҫ?���жϣ�����?�����ң�? UniRx ���Լ�����?���ṩ��?�������?�֣�
///     --
///     ObservableWWW.Get("http://sikiedu.com")
///         .Subscribe(_ =>
///         {
///             Debug.Log("do something");
///         }).AddTo(this);
///     --
/// 2��ObservableWWW ͬ��?�� WhenAll ��������
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
/// 3��ObservableWWW ���� Get Ҳ?���� Post������ GetWWW �� PostWWW ���ֵĸ�����װ������ GetAndGetBytes �� PostAndGetBytes��
///     --
///     //QFramework ��?������?���Ĵ��룺
///     //�����ļ� url�� http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage
///     ScheduledNotifier<float> progressListener = new ScheduledNotifier<float>();
///     ObservableWWW.GetAndGetBytes("http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage", progress: progressListener)
///         .Subscribe(bytes =>
///         {
///             Debug.Log("�����ص��ļ��ŵ�ĳ���ط�");
///         });
///     progressListener.Subscribe(progress =>
///     {
///         Debug.Log($"����Ϊ{progress}");//����Ϊ 1 ��ʾ�������
///     });
///     --
/// 4��ObservableWWW �� API �����Դ���ȥ?�� ScheduledNotifier<T>() ?���������ؽ��ȣ�Subscribe ֮�󴫻�����ֵ���ǵ�ǰ�Ľ��ȣ�?�� ObservableWWW �� Get �� Post ���󶼿���??����Ӧ�� header �� WWWForm
/// 5�����˳�?�� Get �� Post ����ObservableWWW Ҳ�� AssetBundle �ļ���Ҳ���˼򵥵ķ�װ���ṩ������ ObservableWWW.LoadFromCacheOrDownload ������ API���������?�˽⣬���Բο� ObservableWWW.cs 
/// 6��UniRx �� WWW �ṩ�� API ?��������Ҳ?��ʹ?


///23��ReactiveCommand��
/// 1��ReactiveCommand �� ���壺
///     --
///     public interface IReactiveCommand<T> : IObservable<T>
///     {
///         IReadOnlyReactiveProperty<bool> canExecute { get; }
///         bool Execute(T parameter);
///     }
///     --
/// 2��ReactiveCommand �ṩ������ API��
///     1��canExecute��CanExecute ���ڲ�ʹ?�ģ�ֻ���ⲿ�ṩ��ֻ������
///     2��Execute��Execute ?���Ǳ��ⲿ��?�ģ�Execute ��ִ�о������ Command ��ִ?
/// 3��canExecute �� Execute �Ĺ�ϵ��
///     1���� CanExecute Ϊ false ʱ�����ⲿ��? Execute ��Ч���� Command ���ᱻִ?
///     2���� CanExecute Ϊ true ʱ�����ⲿ��? Execute ���� Command �ᱻִ?
/// 4����ʲô���� CanExecute Ϊ false �� true ��?
///     --�������� Observable��
/// 5���´����� ReactiveCommand Ĭ�� CanExecute Ϊ true��
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
/// 6��ReactiveCommand ��ʹ��?���򵥣�ֻҪ��? ReactiveCommand �� Execute ������command �ͻ�֪ͨ Subscribe �Ļص�(��Ϊ CanExecute Ϊ true)��
/// 7��CanExecute �Ŀ����ر����� Observable ���¼�Դ�������ģ�
///     --
///     IObservable<bool> leftMouseClickStream = Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonDown(0))
///         .Select(_ => true);
///     IObservable<bool> leftMouseUpStream = Observable.EveryUpdate()
///         .Where(_ => Input.GetMouseButtonUp(0))
///         .Select(_ => false);
///     IObservable<bool> mouseUp = Observable.Merge(leftMouseClickStream, leftMouseUpStream);
///     ReactiveCommand reactiveCommand = new ReactiveCommand(mouseUp, false);//mouseUp ��ʾ ReactiveCommand ���¼�Դ��canExecute Ĭ��Ϊ false�����¼�Դ mouseUp ���� true ����갴�£�ʱ canExecute Ϊ true
///     reactiveCommand.Subscribe(x =>//û�д��ݹ������������� x ��ʾ���� "()"
///     {
///         Debug.Log(x);
///     });
///     Observable.EveryUpdate()
///         .Subscribe(_ =>
///         {
///             reactiveCommand.Execute();
///         });
///     --
/// 8��ReactiveCommand Ҳ�ǿ��Ա�����(Subscribe) �ģ��ڶ���֮ǰҲ����ʹ? Where �Ȳ�������?�¼�������
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
/// 9��ReactiveCommand ��������������ʾ��?Щ�򵥵������⣬��ʵ������?����ǿ?�Ĺ��ܣ�ֻ���ڽ���?��ǿ?�Ĺ���֮ǰҪ��ѧ�� UniRx ��??������?���ԭ�������ٶ���ǿ���ܽ���ѧϰ


///24��ReactiveCollection �� ReactiveDictionary��
/// 1��ReactiveCollection��
///     1��ReactiveCollection ������ List��
///     2�����ǿ���ʹ?���µĲ�����:
///         1��ObserverAdd // �� �µ� Item �����ᴥ��
///         2��ObserverRemove // ɾ�� Item ����
///         3��ObserverReplace // �滻(Update)  Item ����
///         4��ObserverMove // �ƶ� Item ����
///         5��ObserverCountChanged // �����иı�(Add��Remove) ʱ����
///     3��ʾ����
///         --
///         _ages.ObserveAdd().Subscribe(addAge =>
///         {
///             Debug.Log($"��ӵ����� addAge ������ CollectionAddEvent<int> :{addAge}");
///             Debug.Log($"��ӵ������� ReactionCollection �е��±�:{addAge.Index}");
///             Debug.Log($"��ӵ������ֵ:{addAge.Value}");
///             Debug.Log("����ͬ��");
///         });
///         _ages.ObserveRemove().Subscribe(removeAge =>
///         {
///             Debug.Log($"�Ƴ�������:{removeAge}");
///         });
///         _ages.ObserveReplace().Subscribe(replaceAge =>
///         {
///             Debug.Log($"�滻�������ǣ�{replaceAge}");
///         });
///         _ages.ObserveMove().Subscribe(moveAge =>
///         {
///             Debug.Log($"�ƶ��������ǣ�{moveAge}");
///         });
///         _ages.ObserveCountChanged().Subscribe(count =>
///         {
///             Debug.Log($"����������ı�Ϊ��{count}");
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
/// 2��ReactiveDictionary��
///     1��ReactiveDictionary ������ Dictionary ?��
///     2��ReactiveDictionary ͬ��?������?��������:
///         --
///         ObserverAdd // �� �µ� Item �����ᴥ��
///         ObserverRemove // ɾ��
///         ObserverReplace // �滻(Update)
///         ObserverMove // �ƶ�
///         ObserverCountChanged // �����иı�(Add��Remove)
///         --
///     3��ʾ����
///         --
///         private ReactiveDictionary<string, string> languageCode = new ReactiveDictionary<string, string>()
///         {
///             { "en", "Ӣ��"},
///             { "cn", "����"}
///         };
///         private void Start()
///         {
///             languageCode.ObserveAdd().Subscribe(addLanguage =>
///             {
///                 Debug.Log($"��ӵ����Լ�ֵ�� addLanguage ������ DictionaryAddEvent<string,string> :{addLanguage}");
///                 Debug.Log($"��ӵ����Լ�ֵ�Եļ�:{addLanguage.Key}");
///                 Debug.Log($"��ӵ����Լ�ֵ�Ե�ֵ:{addLanguage.Value}");
///                 Debug.Log("����ͬ��");
///             });
///             languageCode.ObserveRemove().Subscribe(removeLanguage =>
///             {
///                 Debug.Log($"�Ƴ������Լ�ֵ��:{removeLanguage}");
///             });
///             languageCode.ObserveReplace().Subscribe(replaceLanguage =>
///             {
///                 Debug.Log($"�滻�����Լ�ֵ���ǣ�{replaceLanguage}");
///             });
///             languageCode.ObserveCountChanged().Subscribe(count =>
///             {
///                 Debug.Log($"�����ֵ������ı�Ϊ��{count}");
///             });
///             foreach (KeyValuePair<string, string> keyValue in languageCode)
///             {
///                 Debug.Log($"{keyValue.Key}:{keyValue.Value}");
///             }
///             
///             languageCode.Add("jp", "����");
///             languageCode.Remove("en");
///         }
///         --


///25�� ���س��� AsyncOperation��
/// 1���������첽������Դ�����첽���س�����ʱ��������?�� AsyncOperation��UniRx �� AsyncOperation ����?�֣�ʹ�ü��ز������Ժ����׵ؼ������ؽ���
///     --
///     ScheduledNotifier<float> progressObservable = new ScheduledNotifier<float>();
///     SceneManager.LoadSceneAsync(1).AsAsyncOperationObservable(progressObservable)//�첽�����±�Ϊ 1 �ĳ���
///         .Subscribe(asyncOperation =>//����������ɻص�
///         {
///             Debug.Log("load done");
///             Resources.LoadAsync<GameObject>("TestCanvas").AsAsyncOperationObservable()
///             .Subscribe(resourceRequest =>//�첽������Դ��asyncOperation �̳��� AsyncOperation����ʾ�������Դ��
///             {
///                 Instantiate(resourceRequest.asset);//ʵ�����������Դ
///             });
///         });
///     progressObservable.Subscribe(progress =>
///     {
///         Debug.Log($"�����ˣ�{progress}");
///     });


///26��TodoList ʵս��
/// 1��TodoList App ��һ����������Ӧ�ã���Ҫ�������£�
///     1�������嵥������ӡ����ġ�ɾ����������
///     2����������������
///     3��������������ݿ��Ա༭
/// 2��Model ������£�
///     --
///     // ��������
///     public class TodoItem
///     {
///         /// ��ʾ��
///         public int id;
///         
///         /// ����
///         public StringReactiveProperty Content;
///         
///         /// �Ƿ����
///         public BoolReactiveProperty Completed;
///     }
///         
///     // �����б�
///     public class TodoList
///     {
///         public ReactiveCollection<TodoItem> todoItems = new ReactiveCollection<TodoItem>()
///         {
///             new TodoItem()
///             {
///                 id = 0,
///                 Content = new StringReactiveProperty("���ܲ�"),
///                 Completed = new BoolReactiveProperty(false)
///             },
///             new TodoItem()
///             {
///                 id = 1,
///                 Content = new StringReactiveProperty("ע�� sikiedu.com"),
///                 Completed = new BoolReactiveProperty(false)
///             }
///         };
///     }
/// 3��������Ƽ�����չʾ��
///     1���½� Canvas���� Canvas Scale ����� UI Scale Mode ��Ϊ Scale With Screen Size���� Reference Resolution��Ϊ��710��1280
///     2������ Game ��ͼ�ķֱ��� 640 * 1136
///     3������ Scroll View�����򵥲��֣�ɾ������������ Scroll View �� Scroll Rect ����� Horizontal ȡ����ѡ��ֻ������ֱ�������
///     4���� Scroll View �� Content ����� Grid Layout Group ���
///     5���� Grid Layout Group ����� Constraint ����Ϊ Fixed Coloum Count���� Constraint Count ����Ϊ 1
///     6���� Grid Layout Group �� Child Alignment ����Ϊ Upper Center�����õ��� Content ���Ӷ���Ķ��䷽ʽ
///     7���޸� Grid Layout Group �� Cell Size��������Ӽ��� Image ���Աȣ����˴�������Ϊ 680
///     8���޸� Grid Layout Group �� Spacing��������֮��ļ��
///     9������ Grid Layout Group �� Padding
///     10���� Scroll View ͬһ�㼶�´���һ�� Image������Ϊ TodoItemPrototype(Prototype:ԭ��)����Ϊ TodoItem ��ԭ��
///     11��Ϊ TodoItemPrototype ����һ�� Button ��ʾ����ɡ���ť������ŵ��Ҳ�
///     12��Ϊ TodoItemPrototype ����һ�� Text ��ʾ�����ݡ��ı������������࣬�ı����ı�ˮƽ��������У���ֱ�������
///     13��Ϊ TodoItemPrototype ����һ���ű�������Ϊ UITodoItem���������ȡ����ʼ����ť����������ı����󣨼� UIToDoItem.cs��
///     14���� TodoItemPrototype Ĭ��Ϊ����ʾ
///     15��Ϊ Canvas ����һ�� UITodoList �ű��������н��� TodoList �ĳ�ʼ������ UITodoList.cs��
///     16������ʵ�ּ�����
/// 4������ɡ���ť���ܣ��� UITodoList.cs��UITodoItem.cs ���룩��
///     1��ע�ᡰ��ɡ���ť����¼��������ʱ���� Item �� Complete ����Ϊ true(��Ϊ Complete ��һ����Ӧʽ���ԣ�ֵ�ı�ʱ�ᷢ��֪ͨ)
///     2������ item �� Complete Ϊ True �� Item������ɵĲ���ʾ
/// 5����� TodoItem ����Ӳ�����
///     1�����һ�� InputField ��������һ������ӡ���ť�����¡���ӡ���ť��� TodoItem���������û��ֵʱ����Ӱ�ť���ɵ��
///     2��������ť����¼����ڻص��ķ����д����µ� Item������༭�����ݣ��� item ��ӵ� todoItems ��
///     3�������б�� todoItems �����ı仯��������������仯���ص� OnDataChange ����
/// 6�����ݴ洢�����:
///     1���� TodoList��TodoItem ���л�Ϊ Json
///     2��Ҫ��ĳ��������л�����Ҫ����[Serializable]
///     3��ע�⣺��Ϊ ReactiveCollection �Ƿ��ͼ��ϣ���֧��ֱ�����л������Դ˴���Ҫ�����޸�Ϊ List
/// 7���༭���ݣ�
///     1�����ĳһ��ʱ��ĳһ���ı���ʾ��������У��ӺŰ�ť��Ϊ�����¡���ť���޸ĺ�����ť�����¸�����ı�
///     2���ٴ���һ�����°�ť��Ĭ�ϲ��ɼ����ɽ���
///     3��Ϊ TodoItemPrototype ���һ�� Button ���������ɵ��
///     4���� �������Ӱ�ť�����°�ť ������һ�� InputController ����Ϸ�����£���Ϊ����� UIInputController.cs �ű�
///     ����
/// 8���༭�����Ż���
///     1�����ȡ���༭״̬�İ�ť
///     2�������ڱ༭״̬ʱ���б�ÿһ�Ӧ���޷����:ʹ���¼����֡������ͼƬ����ͼƬ��С���б��С��ͬ��Ȼ��ͼƬ����Ϊ͸����ɫ��Ĭ�ϲ��ɼ��������ڱ༭״̬ʱ��ͼƬ�ɼ���

public class Note : MonoBehaviour
{
    #region

    #endregion

    #region 1��2��������ŵ� UniRx ���밸����
    /*private void Start()
    {
        Observable.EveryUpdate()//���� Update ���¼�����
            .Where(_ => Input.GetMouseButtonDown(0))//�ж�/�����¼��������ÿ֡�м�����������¼�����ôִ���·����ĵ��¼�
            .First()//ֻ�����һ�δ������¼�
            .Subscribe(_ =>//����/���� �¼�
            {
                Debug.Log("�������");
            });
    }*/
    #endregion

    #region 3����ʱ��

    ///1����ʱ���������ʵ�ַ�ʽ
    /// 1����������������һ�� timer �����ۼ� Time.deltaTime��һ�� targetTime �����ж� timer �Ƿ���� targetTime����������򴥷��¼�
    /// 2����������һ���¼��󣬽� targetTime ��Ϊ float �������ֵ����ô timer - targetTime ����Զ������� 0����֤��ʱ����ֻ�ܴ���һ��
    /*float timer = 0f;
    float targetTime = 5f;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer - targetTime > 0)
        {
            Debug.Log("do something");
            targetTime = float.MaxValue;//������һ�ζ�ʱ����֮�󣬽� targetTime ����Ϊ float �������ֵ����ô timer - targetTime ����Զ������� 0����֤��ʱ����ֻ�ܴ���һ��
        }
    }*/



    ///2��ʹ��Э��ʵ�ֶ�ʱ��
    /// 1������ IEnumerator �࣬�ȴ� seconds �룬Ȼ��ִ�лص�����
    /// 2���� Start �����п���Э��
    /*private void Start()
    {
        StartCoroutine(Timer(5, () =>
        {
            Debug.Log("do something");
        }));//�ص�����
    }

    IEnumerator Timer(float seconds,Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }*/



    ///3��ʹ�� UniRx ��ʽʵ�ֶ�ʱ����
    /// 1���Աȳ��淽ʽ�����ö��ⶨ��һ��������Ҳ������ Update �����н����߼��ж�
    /// 2���Ա� Coroutine�����ö��ⶨ��Э�̣�ֻ��Ҫһ�д���͸㶨
    /*private void Start()
    {
        Observable.Timer(TimeSpan.FromSeconds(5.0f))
            .Subscribe(_ =>
            {
                Debug.Log("do something");
            });
    }*/


    #endregion

    #region 4�������� Update
    /*private void Start()
    {
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Debug.Log("���� A �߼�");
            });
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                Debug.Log("���� B �߼�");
            });
    }*/
    #endregion

    #region 5��AddTo
    /*private void Start()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ =>
            {
                Debug.Log("�������");
            })
            .AddTo(this);//�����ǰ�������٣��������Ҳ�ᱻ����
    }*/
    #endregion

    #region 6��UniRx �����﷨��ʽ���޴��룩

    #endregion

    #region 7�������� Where��
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

    #region 8�������� First��
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

        //�򻯴��룺
        *//*Observable.EveryUpdate()
         .First(_ => Input.GetMouseButtonDown(0))
         .Subscribe(_ =>
         {
             Debug.Log("do something");
         })
         .AddTo(this);*//*
    }*/
    #endregion

    #region 9���� UGUI ��֧�֣�
    /*private void Start()
    {
        //UniRx �԰�ť����¼��ļ���
        Button button = GameObject.Find("09_Button").transform.GetComponent<Button>();
        button.OnClickAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("�����ť�������¼�");
            });

        //UniRx �� Toggle ����¼��ļ���
        Toggle toggle = GameObject.Find("09_Toggle").transform.GetComponent<Toggle>();
        toggle.OnValueChangedAsObservable()
            .Subscribe(on =>//on ��ʾ Toggle �Ƿ�ѡ��
            {
                Debug.Log(on);
            });

        toggle.OnValueChangedAsObservable()
            .Where(on => on)//ֻ���� on Ϊ true ���¼�������ѡ��ѡʱ�Ŵ����¼�
            .Subscribe(on =>
            {
                Debug.Log(on);
            });

        //UniRx ��ͼƬ��ק�¼��ļ���:Ҫ���ȵ��� UniRx.Triggers �����ռ�
        Image image = GameObject.Find("09_Image").transform.GetComponent<Image>();
        image.OnBeginDragAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("��ʼ�϶�ͼƬ");
            });
        image.OnDragAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("�����϶�ͼƬ");
            });
        image.OnEndDragAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log("�����϶�ͼƬ");
            });
        //ע�⣬һ��Ҫ�����������¼������м��������ֻ��������һ����û��Ч��
    }*/
    #endregion

    #region 10��Reactive Property ��Ӧʽ���ԣ�

    ///����ʵ�ַ�ʽ��
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
        Debug.Log("���䷢���ı䣬��ʱ����Ϊ��" + Age);
    }

    private void Start()
    {
        age = 18;
        Debug.Log("��ʱ����Ϊ��" + age);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Age++;
    }*/

    ///ʹ�� UniRx ʵ�֣�
    ///
    /*//��Ӧʽ���� Age����ʼֵΪ 0
    //public ReactiveProperty<int> Age = new ReactiveProperty<int>(0);//�������л����鷳
    public IntReactiveProperty Age = new IntReactiveProperty(0);//�Ƿ���
    private void Start()
    {
        Debug.Log("��ʼ�����ǣ�" + Age);
        Age.Subscribe(age =>
        {
            Debug.Log("���ڵ������ǣ�" + age);
        });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Age.Value++;
    }*/

    #endregion

    #region 11��MVP ��ܵ�ʵ�֣�
    ///1��ʹ�� UniRx ʵ�ֵ����ť�õ���Ѫ���� 99���������Ѫ��С�ڵ��� 0�����˴�������״̬����ť�������ٽ����������ٿ�Ѫ�ˣ�
    /// 1��Enemy ���൱��һ�� Model
    /// 2��Unity �е� UI ���൱�� View
    /// 3��Start �������ڵ����൱��һ�� Controller
    /*public class Enemy
    {
        public ReactiveProperty<long> CurrentHp { get; private set; }//ֻ�����������޸� currentHP
        public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }

        public Enemy(int initialHP)//��ʼ�� Enemy ��Ѫ�����Ƿ�����״̬
        {
            CurrentHp = new ReactiveProperty<long>(initialHP);
            IsDead = CurrentHp.Select(x => x <= 0).ToReactiveProperty();//��� x С�ڵ��� 0����ʾ������������֮��������״̬
                                                                        //x <= 0 ���ʽ�᷵��һ������ֵ��ToReactiveProperty ����ݱ��ʽ���صĲ���ֱֵ�Ӵ���һ����Ӧ�� ReactiveProperty���ڴ��������� ReactiveProperty<bool>�� ʵ�������أ���ֵ�� IsDead��
        }
    }

    void Start()
    {
        //��ʼ�����˶���
        Enemy enemy = new Enemy(100);

        //��ʼ�� UI
        Button button = GameObject.Find("11_Attack_Button").GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = "Attack";//��ť����ʾ�ı�
        Text text = GameObject.Find("11_EnemyHp_Text").GetComponent<Text>();
        text.text = enemy.CurrentHp.ToString();

        //ע�ᰴť����¼�
        button.OnClickAsObservable().Subscribe(_ => enemy.CurrentHp.Value -= 99);//�����ť������Ѫ���� 99
        enemy.CurrentHp.SubscribeToText(text);//������Ѫ����ʾ�� Text �ı���
        enemy.IsDead.Where(isDead => isDead)//������˴�������״̬���ð�ť���ɽ����������ټ�Ѫ�ˣ�
            .Subscribe(_ =>
            {
                button.interactable = false;
            });
    }*/
    #endregion

    #region 12��Merge ��������
    /*private void Start()
    {
        IObservable<long> leftMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0));
        IObservable<long> rightMouseClickStream = Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(1));
        Observable.Merge(leftMouseClickStream, rightMouseClickStream)
             .Subscribe(_ =>
             {
                 Debug.Log("�����������������Ҽ�");
             });
    }*/


    #endregion

    #region 13��ʵ�֣���ĳ����ť���ʱ��ʹ���е�ǰҳ��İ�ť���ɱ���������� Select ��������
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
            .First()//��Ϊִֻ�е�һ�Σ�����ĳ����ť���������ǰҳ������а�ť�Ͳ����ٱ������
            .Subscribe(_ =>
            {
                Debug.Log("A/B/C ��ť������ˣ�ĳ����ť���������ǰҳ������а�ť�Ͳ����ٱ�����ˣ�");
                Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => { gameObject.SetActive(false); });
            });*//*

        //���ϴ���ֻ��֪��������ť����һ������������Ҫ����֪����һ�����������Ҫʹ�� Select ������
        IObservable<String> buttonAStream = buttonA.onClick.AsObservable().Select(_ => "A");//buttonAStream �¼��᷵�ز��� ��A��
        IObservable<String> buttonBStream = buttonB.onClick.AsObservable().Select(_ => "B");//buttonBStream �¼��᷵�ز��� ��B��
        IObservable<String> buttonCStream = buttonC.onClick.AsObservable().Select(_ => "C");//buttonCStream �¼��᷵�ز��� ��C��
        Observable.Merge(buttonAStream, buttonBStream, buttonCStream)
            .First()
            .Subscribe(buttonId =>//����һ������ buttonId���������Զ��壩����������ɰ�ť����¼�����
            {
                if(buttonId == "A")
                    Debug.Log("A ��ť�������");
                if (buttonId == "B")
                    Debug.Log("B ��ť�������");
                if (buttonId == "C")
                    Debug.Log("C ��ť�������");
                Observable.Timer(TimeSpan.FromSeconds(1.0f)).Subscribe(_ => { gameObject.SetActive(false); });
            });
    }*/
    #endregion

    #region 14��Unity �� Rx���޴��룩��

    #endregion

    #region 15��UI ��ǿ + ��¼ע��ҳ�����ϰ + AddTo��飺
    /*GameObject loginPanel;
    GameObject registerPanel;
    InputField userNameInputField;
    InputField passwordInputField;
    Button loginButton;
    Button registerButton;

    private void Start()
    {
        //Ӧ���������ű��ֱ���Ƶ�¼�����ע�������߼�������ֻ�����¼�����߼���ע�����ͬ��
        //��ʼ��
        loginPanel = GameObject.Find("Login_Panel");
        registerPanel = GameObject.Find("Register_Panel");


        //��ȡ��¼����İ�ť
        userNameInputField = GameObject.Find("UserName_InputField").GetComponent<InputField>();
        userNameInputField.placeholder.GetComponent<Text>().text = "�������û���...";//��������ʾ�ı�
        passwordInputField = GameObject.Find("Password_InputField").GetComponent<InputField>();
        passwordInputField.contentType = InputField.ContentType.Password;//���������Ϊ�ı���������ʱ���� * ��ʾ
        passwordInputField.placeholder.GetComponent<Text>().text = "����������...";//��ʾ�ı�
        loginButton = GameObject.Find("Login_Button").GetComponent<Button>();
        registerButton = GameObject.Find("Register_Button").GetComponent<Button>();

        loginPanel.SetActive(true);
        registerPanel.SetActive(false);

        //ע������¼�
        loginButton.OnClickAsObservable().Subscribe(_ =>
        {
            Debug.Log("�����¼��ť��");
        });//�����ť�����Ϣ
        registerButton.OnClickAsObservable().Subscribe(_ =>
        {
            Debug.Log("��ת��ע��ҳ�棡");
        });//�����ť�����Ϣ

        userNameInputField.OnEndEditAsObservable().Subscribe(result =>
        {
            Debug.Log("Username: " + result);
            passwordInputField.Select();//�û���������ɺ�ѡ�������
        });//�������༭��ʱ�򣬻ص�ע��ķ��������û����ı�������ȥ

        passwordInputField.OnEndEditAsObservable().Subscribe(result =>
        {
            Debug.Log("Password��" + result);
        });//�������༭��ʱ�򣬻ص�ע��ķ������������ı�������ȥ
    }*/
    #endregion

    #region 16��Unity ������������ Triggers��
    /*private void Start()
    {

        *//*//ȫ�ֵ��¼���������ʹ��ǰ�Ľű�����ɾ���������ǻ���ڣ�����ص������ڵ����˵�ǰ�ű����󣬻ᷢ����ָ���쳣
        //���������ʹ�� AddTo ����ǰ�����¼��������뵱ǰ�ű������ٰ󶨵�һ�𣬵�ǰ�ű��������٣���ǰ�����������
        Observable.EveryUpdate().Subscribe(_ =>//EveryUpdate ��� API �Ǵ�һ���� MainThreadDispatcher �����л�ȡ�ģ�ʹ�� UniRx ������ʱ Hierarchy �����Զ�������һ������չ������� MainThreadDispatcher��
        {
            Debug.Log("Update" + this.gameObject.name);
        });*/


    /*Observable.EveryUpdate().Subscribe(_ =>
    {
        Debug.Log("Update" + this.gameObject.name);
    }).AddTo(this);*//*

    //������д��������Ҫ AddTo����ȻҲ���Լ���
    *//*this.UpdateAsObservable().Subscribe(_ =>
    {
        Debug.Log("UpdateAsObservable" + this.gameObject.name);
    });*//*
}*/
    #endregion

    #region 17��UI Trigger��
    /*Image _image;
    private void Start()
    {
        _image = GameObject.Find("17_Image").GetComponent<Image>();
        //���Ҫ OnBeginDragAsObservable �¼������������ OnDragAsObservable �� OnEndDragAsObservable Ҳע����
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

    #region 18��Coroutine �Ĳ�����
    //UniRx �� Unity �� Coroutine Ҳ�ṩ?�֣����Խ�?�� Coroutine ת��Ϊ�¼�Դ��Observable��
    /*IEnumerator CoroutineA()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("A");
    }
    private void Start()
    {
        Observable.FromCoroutine(_ => CoroutineA())//Couroutine A ִ�к󴥷��¼���ִ�ж��ĵķ������ B
            .Subscribe(_ =>
            {
                Debug.Log("B");
            }).AddTo(this);
    }*/

    //UnitRx Ҳ?�ֽ� Observable ת��Ϊ?�� Coroutine �е� yield ����
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

    #region 19��WhenAll:Coroutine �Ĳ�?����
    //���д���Э��
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
                Debug.Log("���д��� A��B Э����");
            }).AddTo(this);
    }*/

    //�������а�ť������������߼�
    /*Button _aButton;
    Button _bButton;
    Button _cButton;
    private void Start()
    {
        Button _aButton = GameObject.Find("19_A_Button").GetComponent<Button>();
        _aButton.GetComponentInChildren<Text>().text = "A ��ť";
        Button _bButton = GameObject.Find("19_B_Button").GetComponent<Button>();
        _bButton.GetComponentInChildren<Text>().text = "B ��ť";
        Button _cButton = GameObject.Find("19_C_Button").GetComponent<Button>();
        _cButton.GetComponentInChildren<Text>().text = "C ��ť";

        var aStream = _aButton.OnClickAsObservable().First();
        var bStream = _bButton.OnClickAsObservable().First();
        var cStream = _cButton.OnClickAsObservable().First();

        Observable.WhenAll(aStream, bStream, cStream).Subscribe(_ =>
        {
            Debug.Log("A��B��C ��ť�������һ����");
        }).AddTo(this);
    }*/
    #endregion

    #region 20���¼����Ľ��� OnCompleted
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

    #region 21��Start �ö��̸߳��򵥣�
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

    #region 22��ObservableWWW ���ŵ��������������
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

    //QFramework ��?������?���Ĵ��룺
    //�����ļ� url�� http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage
    ScheduledNotifier<float> progressListener = new ScheduledNotifier<float>();
    ObservableWWW.GetAndGetBytes("http://liangxiegame.com/media/QFramework_v0.0.9.unitypackage", progress:progressListener)
        .Subscribe(bytes =>
        {
            Debug.Log("�����ص��ļ��ŵ�ĳ���ط�");
        });
    progressListener.Subscribe(progress =>
    {
        Debug.Log($"����Ϊ{progress}");//����Ϊ 1 ��ʾ�������
    });
}*/

    #endregion

    #region 23��ReactiveCommand��
    //ReactiveCommand �Ķ��壺
    /*public interface IReactiveCommand<T> : IObservable<T>
    {
        IReadOnlyReactiveProperty<bool> canExecute { get; }
        bool Execute(T parameter);
    }*/

    /*private void Start()
    {
        //�´����� ReactiveCommand Ĭ�� CanExecute Ϊ true��
        *//*ReactiveCommand command = new ReactiveCommand();
        command.Subscribe(_ =>
        {
            Debug.Log("command executed");
        });
        command.Execute();
        command.Execute();
        command.Execute();*//*

        //CanExecute �Ŀ����ر����� Observable ���¼�Դ�������ģ�
        *//*IObservable<bool> leftMouseClickStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => true);

        IObservable<bool> leftMouseUpStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Select(_ => false);

        IObservable<bool> mouseUp = Observable.Merge(leftMouseClickStream, leftMouseUpStream);

        ReactiveCommand reactiveCommand = new ReactiveCommand(mouseUp, false);//mouseUp ��ʾ ReactiveCommand ���¼�Դ��canExecute Ĭ��Ϊ false�����¼�Դ mouseUp ���� true ����갴�£�ʱ canExecute Ϊ true
        reactiveCommand.Subscribe(x =>//û�д��ݹ������������� x ��ʾ���� "()"
        {
            Debug.Log(x);
        });
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                reactiveCommand.Execute();
            });*//*

        //ReactiveCommand Ҳ�ǿ��Ա�����(Subscribe) �ģ��ڶ���֮ǰҲ����ʹ? Where �Ȳ�������?�¼�������
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

    #region 24��ReactiveCollection �� ReactiveDictionary��
    /*ReactiveCollection<int> _ages = new ReactiveCollection<int>
    {
        1,2,3,4,5
    };
    private void Start()
    {
        _ages.ObserveAdd().Subscribe(addAge =>
        {
            Debug.Log($"��ӵ����� addAge ������ CollectionAddEvent<int> :{addAge}");
            Debug.Log($"��ӵ������� ReactionCollection �е��±�:{addAge.Index}");
            Debug.Log($"��ӵ������ֵ:{addAge.Value}");
            Debug.Log("����ͬ��");
        });
        _ages.ObserveRemove().Subscribe(removeAge =>
        {
            Debug.Log($"�Ƴ�������:{removeAge}");
        });
        _ages.ObserveReplace().Subscribe(replaceAge =>
        {
            Debug.Log($"�滻�������ǣ�{replaceAge}");
        });
        _ages.ObserveMove().Subscribe(moveAge =>
        {
            Debug.Log($"�ƶ��������ǣ�{moveAge}");
        });
        _ages.ObserveCountChanged().Subscribe(count =>
        {
            Debug.Log($"����������ı�Ϊ��{count}");
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
        {"en", "Ӣ��"},
        {"cn", "����"}
    };
    private void Start()
    {
        languageCode.ObserveAdd().Subscribe(addLanguage =>
        {
            Debug.Log($"��ӵ����Լ�ֵ�� addLanguage ������ DictionaryAddEvent<string,string> :{addLanguage}");
            Debug.Log($"��ӵ����Լ�ֵ�Եļ�:{addLanguage.Key}");
            Debug.Log($"��ӵ����Լ�ֵ�Ե�ֵ:{addLanguage.Value}");
            Debug.Log("����ͬ��");
        });
        languageCode.ObserveRemove().Subscribe(removeLanguage =>
        {
            Debug.Log($"�Ƴ������Լ�ֵ��:{removeLanguage}");
        });
        languageCode.ObserveReplace().Subscribe(replaceLanguage =>
        {
            Debug.Log($"�滻�����Լ�ֵ���ǣ�{replaceLanguage}");
        });
        languageCode.ObserveCountChanged().Subscribe(count =>
        {
            Debug.Log($"�����ֵ������ı�Ϊ��{count}");
        });


        foreach (KeyValuePair<string, string> keyValue in languageCode)
        {
            Debug.Log($"{keyValue.Key}:{keyValue.Value}");
        }

        languageCode.Add("jp", "����");
        languageCode.Remove("en");
    }*/
    #endregion

    #region 25�� ���س��� AsyncOperation��
    /*private void Start()
    {
        ScheduledNotifier<float> progressObservable = new ScheduledNotifier<float>();
        SceneManager.LoadSceneAsync(1).AsAsyncOperationObservable(progressObservable)//�첽�����±�Ϊ 1 �ĳ���
            .Subscribe(asyncOperation =>//����������ɻص�
            {
                Debug.Log("load done");
                Resources.LoadAsync<GameObject>("TestCanvas").AsAsyncOperationObservable()
                    .Subscribe(resourceRequest =>//�첽������Դ��asyncOperation �̳��� AsyncOperation����ʾ�������Դ��
                    {
                        Instantiate(resourceRequest.asset);//ʵ�����������Դ
                    });
            });
        progressObservable.Subscribe(progress =>
        {
            Debug.Log($"�����ˣ�{progress}");
        });
    }*/
    #endregion

    #region 26��TodoList ʵս��
    // ��������
    [Serializable]
    public class TodoItem
    {
        /// <summary>
        /// ��ʾ��
        /// </summary>
        public int id;

        /// <summary>
        /// ����
        /// </summary>
        public StringReactiveProperty Content;

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public BoolReactiveProperty Completed;
    }

    // �����б�
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
                Content = new StringReactiveProperty("���ܲ�"),
                Completed = new BoolReactiveProperty(false)
            },
            new TodoItem()
            {
                id = -1,
                Content = new StringReactiveProperty("ע�� sikiedu.com"),
                Completed = new BoolReactiveProperty(false)
            }
        };

        public void Save()//���棺���л�
        {
            PlayerPrefs.SetString("todoId", JsonUtility.ToJson(this));
        }

        public static TodoList Load()//���أ������л�
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
