using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
	USERNAME,
	PASSWORD,
	ISBN,
	BOOK_NAME,
	PRESS_NAME,
	PRESS_CITY,
	PRESS_YEAR
};
public enum ModifyType
{
	UPDATE,
	INSERT
};

public class Record
{
	public string ISBN;
	public string bookName;
	public string bookIntro;
	public int bookImage;
	public string pressName;
	public string pressCity;
	public string pressYear;

	public Record() { }

	public Record(string ISBN, string bookName, string pressName, string pressCity, string pressYear)
	{
		this.ISBN = ISBN;
		this.bookName = bookName;
		this.pressName = pressName;
		this.pressCity = pressCity;
		this.pressYear = pressYear;
	}
}

public class ManageUI : MonoBehaviour
{
	[Header("结果文本")]
	public Text resText;
	[Header("record容器")]
	public Transform recordContainer;
	[Header("record预制体")]
	public GameObject recordPrefab;
	[Header("书名文本")]
	public InputField booknameInput;
	[Header("索书号文本")]
	public InputField ISBNInput;
	[Header("出版城市下拉框")]
	public Dropdown cityDropdown;
	[Header("出版社下拉框")]
	public Dropdown pressDropdown;
	[Header("出版年份文本")]
	public InputField yearInput;
	[Header("模糊查询toggle")]
	public Toggle fuzzySearch;
	[Header("查询按钮")]
	public Button searchBtn;
	[Header("关闭按钮")]
	public Button closeBtn;
	[Header("修改按钮")]
	public Button modifyBtn;
	[Header("删除按钮")]
	public Button deleteBtn;

	private string pressCity;
	private string pressName;
	private string bookName;
	private string ISBN;
	private string pressYear;

	private List<int> currentIndexList = new List<int>();

	Dictionary<Record, ModifyType> modifyDic = new Dictionary<Record, ModifyType>();
	List<Image> maskList = new List<Image>();
	List<Record> recordList = new List<Record>();
	List<GameObject> recordObjs = new List<GameObject>();

	void Start()
	{

		//deleteBtn.onClick.AddListener(delegate { OnDeleteBtnClick(); });
		OnSearchBtnClick();
	}

	void OnEnable()
	{
		recordObjs.Clear();

		//InitPressDropdown();
		// 清空原来的查询结果
		for (int i = 0; i < recordContainer.childCount; i++)
		{
			Destroy(recordContainer.GetChild(i).gameObject);
		}
	}

	HashSet<string> pressSet = new HashSet<string>();
	HashSet<string> citySet = new HashSet<string>();


	void OnSearchBtnClick()
	{
		// 清空原来的查询结果
		for (int i = 0; i < recordContainer.childCount; i++)
		{
			Destroy(recordContainer.GetChild(i).gameObject);
		}
		resText.text = "";
		maskList.Clear();
		currentIndexList.Clear();
		recordList.Clear();
		isMulti = false;
		recordObjs.Clear();

		// 数据库查询
		string[] selCols = { "*" };
		string[] tables = { "BookView" };
		string[] cols = { "BookName", "ISBN", "PressName", "PressCity", "PressYear" };
		string[] operations_a = { " = ", " = ", " = ", " = ", " = " };
		string[] operations_f = { " like ", " like ", " like ", " like ", " like " };
		string[] values_a = { bookName, ISBN, pressName, pressCity, pressYear };
		string[] values_f = { "%" + bookName + "%", "%" + ISBN + "%", "%" + pressName + "%", "%" + pressCity + "%", "%" + pressYear + "%" };
		string[] operations;
		string[] values;

		// 开启模糊查询
		if (fuzzySearch.isOn)
		{
			operations = operations_f;
			values = values_f;
		}
		else
		{
			operations = operations_a;
			values = values_a;
		}


		// 显示查询结果
		for (int i = 0; i < 5; i++)
		{
			GameObject record = Instantiate(recordPrefab, recordContainer);
			recordObjs.Add(record);
			InputField ISBNInput = record.transform.Find("ISBN").GetComponent<InputField>();
			InputField booknameInput = record.transform.Find("bookname").GetComponent<InputField>();
			InputField pressnameInput = record.transform.Find("pressname").GetComponent<InputField>();
			InputField presscityInput = record.transform.Find("presscity").GetComponent<InputField>();
			InputField pressyearInput = record.transform.Find("pressyear").GetComponent<InputField>();
			ISBNInput.text = "傻逼1232121";
			booknameInput.text = "小傻逼";
			pressyearInput.text = "1999";
			pressnameInput.text = "骚逼出版社";
			presscityInput.text = "hehe";

			// 填充遮罩列表（蓝色选择条）
			Image mask = record.GetComponent<Image>();
			maskList.Add(mask);

			Record rec = new Record(ISBNInput.text, booknameInput.text, pressnameInput.text, presscityInput.text, pressyearInput.text);
			recordList.Add(rec);

			// 选择按钮及各个文本框绑定事件
			int index = i;
			ISBNInput.onValueChanged.AddListener(delegate { OnValueChanged(index, rec, ItemType.ISBN, ISBNInput.text); });
			booknameInput.onValueChanged.AddListener(delegate { OnValueChanged(index, rec, ItemType.BOOK_NAME, booknameInput.text); });
			pressnameInput.onValueChanged.AddListener(delegate { OnValueChanged(index, rec, ItemType.PRESS_NAME, pressnameInput.text); });
			presscityInput.onValueChanged.AddListener(delegate { OnValueChanged(index, rec, ItemType.PRESS_CITY, presscityInput.text); });
			pressyearInput.onValueChanged.AddListener(delegate { OnValueChanged(index, rec, ItemType.PRESS_YEAR, pressyearInput.text); });
			Button chooseBtn = record.GetComponentInChildren<Button>();
			chooseBtn.onClick.AddListener(delegate { OnChooseBtnClick(index); });

			// 不可编辑
			//ISBNInput.enabled = false;
			//booknameInput.enabled = false;
			//pressnameInput.enabled = false;
			//presscityInput.enabled = false;
			//pressyearInput.enabled = false;
		}
	}

	void OnCloseBtnClick()
	{
		this.gameObject.SetActive(false);
	}


	private bool isMulti = false;
	void OnChooseBtnClick(int index)
	{
		// 非多选时先清空选中的index集合
		if (!currentIndexList.Contains(index) && !isMulti) currentIndexList.Clear();
		currentIndexList.Add(index);
		StartCoroutine("MultiChoose", index);
	}

	IEnumerator MultiChoose(int index)
	{
		while (true)
		{
			if (!Input.GetKey(KeyCode.RightControl) && !Input.GetKey(KeyCode.LeftControl))
			{
				for (int i = 0; i < maskList.Count; i++)
				{
					isMulti = false;
					if (i != index)
						maskList[i].enabled = false;
					maskList[index].enabled = true;
				}
				yield break;
			}
			else // 长按Ctrl键可多选
			{
				isMulti = true;
				maskList[index].enabled = true;
				yield break;
			}
		}
	}

	Dictionary<int, Record> updateRecordDic = new Dictionary<int, Record>();// 需要update的record集
	Dictionary<int, Record> insertRecordDic = new Dictionary<int, Record>(); // 需要insert的record集
	Dictionary<int, Record> deleteRecordDic = new Dictionary<int, Record>(); // 需要delete的record集

	void OnValueChanged(int index, Record record, ItemType type, string value)
	{
		switch (type)
		{
			case ItemType.ISBN:
				if (!insertRecordDic.ContainsKey(index))
				{
					Record originRec = new Record(record.ISBN, record.bookName, record.pressName, record.pressCity, record.pressYear);
					insertRecordDic.Add(index, record);
					deleteRecordDic.Add(index, originRec);
					Debug.Log("原有记录 " + deleteRecordDic[index].ISBN);
				}
				insertRecordDic[index].ISBN = value;
				Debug.Log("更改后记录：" + deleteRecordDic[index].ISBN);
				// 保证update和insert无重复元素
				if (updateRecordDic.ContainsKey(index))
				{
					updateRecordDic.Remove(index);
				}
				break;

			case ItemType.BOOK_NAME:
				if (insertRecordDic.ContainsKey(index)) // 修改ISBN意味着该条记录加入插入集
				{
					insertRecordDic[index].bookName = value;
				}
				else if (!updateRecordDic.ContainsKey(index))
				{
					updateRecordDic.Add(index, record);
				}
				else
				{
					updateRecordDic[index].bookName = value;
				}
				break;

			case ItemType.PRESS_CITY:
				if (insertRecordDic.ContainsKey(index)) // 修改ISBN意味着该条记录加入插入集
				{
					insertRecordDic[index].pressCity = value;
				}
				else if (!updateRecordDic.ContainsKey(index))
				{
					updateRecordDic.Add(index, record);
				}
				else
				{
					updateRecordDic[index].pressCity = value;
				}
				break;

			case ItemType.PRESS_NAME:
				if (insertRecordDic.ContainsKey(index)) // 修改ISBN意味着该条记录加入插入集
				{
					insertRecordDic[index].pressName = value;
				}
				else if (!updateRecordDic.ContainsKey(index))
				{
					updateRecordDic.Add(index, record);
				}
				else
				{
					updateRecordDic[index].pressName = value;
				}
				break;

			case ItemType.PRESS_YEAR:
				if (insertRecordDic.ContainsKey(index)) // 修改ISBN意味着该条记录加入插入集
				{
					insertRecordDic[index].pressYear = value;
				}
				else if (!updateRecordDic.ContainsKey(index))
				{
					updateRecordDic.Add(index, record);
				}
				else
				{
					updateRecordDic[index].pressYear = value;
				}
				break;
		}

	}


}
