using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SettingType
{
	number,
	text,
	boolean
}

public class SettingIntItem
{
	//显示在菜单上的描述
	public string desc = "";

	public int value;

	//item的所有可选值
	public int[] values;
}
public class SettingStrItem
{
	//显示在菜单上的描述
	public string desc = "";

	//0类型中描述后面跟的数字,1类型中选定的值索引
	public string value;

	//item的所有可选值
	public string[] values;
}
public enum PlayType
{
	single,
	two
}

public class Setting
{

	private Dictionary<string, SettingIntItem> app_setting = new Dictionary<string, SettingIntItem>();
	private Dictionary<string, SettingIntItem> game_int_setting = new Dictionary<string, SettingIntItem>();
	private Dictionary<string, SettingStrItem> game_str_setting = new Dictionary<string, SettingStrItem>();

	private static Setting instance;

	public PlayType play_type;

	public static Setting Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Setting();
			}
			return instance;
		}
	}

	private Setting()
	{
		game_int_setting.Add("printer", new SettingIntItem() { desc = "热敏打印机", value = 1, values = new int[2] { 1, 2 } });
		game_int_setting.Add("all_rounds_num", new SettingIntItem() { desc = "一局场数", value = 66, values = new int[] { 30, 45, 66 } });
		game_int_setting.Add("check_waybill_tm", new SettingIntItem() { desc = "对单时间", value = 30, values = new int[] { 30, 60 } });
		game_int_setting.Add("bet_tm", new SettingIntItem() { desc = "押分时间", value = 30, values = new int[] { 30, 60 } });
		game_int_setting.Add("big_chip_facevalue", new SettingIntItem() { desc = "大筹码", value = 100, values = new int[] { 100, 500, 1000 } });
		game_int_setting.Add("mini_chip_facevalue", new SettingIntItem() { desc = "小筹码", value = 10, values = new int[] { 1, 10, 100 } });
		game_int_setting.Add("total_limit_red", new SettingIntItem() { desc = "总限红", value = 3000, values = new int[] { 3000, 5000 } });
		game_int_setting.Add("desk_limit_red", new SettingIntItem() { desc = "单台限红", value = 3000, values = new int[] { 3000, 5000, 30000 } });
		game_int_setting.Add("tie_limit_red", new SettingIntItem() { desc = "和限红", value = 3000, values = new int[] { 3000, 5000, 30000 } });

		game_str_setting.Add("bgm_on", new SettingStrItem() { desc = "背景音乐开关", value = "背景音乐开", values = new string[] { "背景音乐开", "背景音乐关" } });
		game_str_setting.Add("waybill_font", new SettingStrItem() { desc = "露单字体", value = "大字体露单", values = new string[] { "大字体露单", "小字体露单" } });
		game_str_setting.Add("break_machine", new SettingStrItem() { desc = "是否爆机", value = "爆机无", values = new string[] { "爆机无", "爆机有" } });
		game_str_setting.Add("open_3_sec", new SettingStrItem() { desc = "3秒功能开", value = "3秒功能开", values = new string[] { "3秒功能开", "3秒功能关" } });
		game_str_setting.Add("print_waybill", new SettingStrItem() { desc = "打印露单", value = "打印露单", values = new string[] { "打印露单" } });
	}

	public int GetGameIntSetting(string key)
	{
		var item = game_int_setting[key];
		return item.value;
	}

	public string GetGameStrSetting(string key)
	{
		var item = game_str_setting[key];
		return item.value;
	}
}
