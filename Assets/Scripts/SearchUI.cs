using System.Collections;
using System.Collections.Generic;
//using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class SearchUI : MonoBehaviour {

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

    private string pressCity;
    private string pressName;
    private string bookName;
    private string ISBN;
    private string pressYear;

	private List<int> currentIndexList = new List<int>();
	List<Image> maskList = new List<Image>();
	List<Record> recordList = new List<Record>();
	List<GameObject> recordObjs = new List<GameObject>();
	void Start()
    {
        searchBtn.onClick.AddListener(delegate { OnSearchBtnClick(); });
    }

    void OnEnable()
    {
        for (int i = 0; i < recordContainer.childCount; i++)
        {
            Destroy(recordContainer.GetChild(i).gameObject);
        } 

    }


    void OnSearchBtnClick()
    {
        // 清空原来的查询结果
        for (int i = 0; i < recordContainer.childCount; i++)
        {
            Destroy(recordContainer.GetChild(i).gameObject);
        } 

        // 显示查询结果
        for (int i = 0; i < 8; i++)
        {
            GameObject record = Instantiate(recordPrefab, recordContainer);
            InputField ISBNInput = record.transform.Find("ISBN").GetComponent<InputField>();
            InputField booknameInput = record.transform.Find("bookname").GetComponent<InputField>();
            InputField pressnameInput = record.transform.Find("pressname").GetComponent<InputField>();
            InputField presscityInput = record.transform.Find("presscity").GetComponent<InputField>();
            InputField pressyearInput = record.transform.Find("pressyear").GetComponent<InputField>();
			ISBNInput.text = "jkldjk";
			booknameInput.text = "估计快了";
			pressyearInput.text = "iopiopiopi";
			pressnameInput.text = "iooqewqfgenl";
			presscityInput.text = "904-0-239";
			// 填充遮罩列表（蓝色选择条）
			Image mask = record.GetComponent<Image>();
			maskList.Add(mask);

			Record rec = new Record(ISBNInput.text, booknameInput.text, pressnameInput.text, presscityInput.text, pressyearInput.text);
			recordList.Add(rec);

			int index = i;
			Button chooseBtn = record.GetComponentInChildren<Button>();
			chooseBtn.onClick.AddListener(delegate { OnChooseBtnClick(index); });
		}        
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

	void OnCloseBtnClick()
    {
        this.gameObject.SetActive(false);
    }
	
}
