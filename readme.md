# CSharpJson
This is a json library for C#. 

## version
1.0.4

## Support
.Net 6.0
## Usage
To use this library, import json.dll or copy the code of this project to your project, and then
``` 
using Json
```
We provide class `JsonObject` as the object in json and class `JsonArray` as the array in json. We provide class `JsonItem` as the item in json.
### Supported type of item
These types are supported as an item of json:

| Json type           | C# type                                         | Item class name |
|:--------------------|:------------------------------------------------|-----------------|
| object              | JsonObject, Dictionary\<string, T>, IJsonObject | JsonObject      |
| array               | JsonArray, T[], List\<T>                        | JsonArray       |
| string              | string                                          | JsonString      |
| string (Length == 1)| string, char                                    | JsonString      |
| number              | decimal, double, float                          | JsonFloat       |
| number (integer)    | long, int, short, decimal, double, float        | JsonInteger     |
| true                | bool                                            | JsonBool        |
| false               | bool                                            | JsonBool        |
| null                | null                                            | JsonNull        |

where T is any type supported as an item of json.<br>
Notice that we use `long` to store integers, whose range is `-9,223,372,036,854,775,808` to `9,223,372,036,854,775,807`; we use `decimal` to store non-integer numbers, whose range is `±1.0 x 10^-28` to `±7.9228 x 10^28`. Out-of-range numbers are not supported.<br>

### JsonItem
#### Create a JsonItem
You can create a JsonItem from a value by `CreateFromValue`.
```
//public static JsonItem CreateFromValue(object? value);
JsonItem item1 = JsonItem.CreateFromValue("value1");
JsonItem item2 = JsonItem.CreateFromValue(1.5);
JsonItem item3 = JsonItem.CreateFromValue(true);
JsonItem item4 = JsonItem.CreateFromValue(new object?[]{"value2", 2, false});
JsonItem item5 = JsonItem.CreateFromValue(new Dictionary<string, object?>(){{"key1", null}});
```
You can also create a JsonItem by newing an object of a JsonItem's subclass as following.
```
// public JsonString(string value);
// public JsonString(char value);
JsonItem item1 = new JsonString("value1");

// public JsonFloat(decimal value);
JsonItem item2 = new JsonFloat(1.5);

// public JsonInteger(long value);
JsonItem item3 = new JsonInteger(1);

// public JsonBool(bool value);
JsonItem item4 = new JsonBool(true);

// public JsonNull();
JsonItem item5 = new JsonNull();

// public JsonArray();
// public JsonArray(Array value);
JsonItem item6 = new JsonArray(new object?[]{"value2", 2, false});

// public JsonObject();
// public JsonObject(IDictionary value); // the keys should be string or JsonString.
JsonItem item7 = new JsonObject(new Dictionary<string, object?>(){{"key1", null}});
```

You can parse a string to a JsonItem by `Parse`.
```
//public static JsonItem Parse(string str);
JsonItem item1 = JsonItem.Parse("\"value1\"");
JsonItem item2 = JsonItem.Parse("1.5");
JsonItem item3 = JsonItem.Parse("true");
JsonItem item4 = JsonItem.Parse("{\"key1\": [0, 1, 2.5, true]}");
```

#### Get value from a JsonItem
You can get the value in the specified type by `GetValue`. You can provide a default value, and the default value will be returned when the specified type is not valid for the item.
```
JsonItem item = JsonItem.CreateFromValue(1.5);

// public T GetValue<T>();
double v1 = item.GetValue<double>(); // v1: 1.5

// public T GetValue<T>(T defaultValue);
double v2 = item.GetValue<double>(0.0); // v2: 1.5
string v3 = item.GetValue<string>("default"); // v3: "default"

```

---

### JsonObject
#### Create a JsonObject
You can create a JsonObject in a similar way to create a Dictionary. 
```
JsonObject obj1 = new JsonObject();
JsonObject obj2 = new JsonObject()
{
    {"key1", "value1"},
    {"key2", 0 },
    {"key3", true}, 
    {"key4", null}
};
```
You can also parse a string to a JsonObject by `Parse`.
```
string str = "{\"key1\": \"value1\", \"key2\": 0, \"key3\": true, \"key4\": null}";

// public static JsonObject Parse(string str);
JsonObject obj = JsonObject.Parse(str);
```

#### Set value to a JsonObject
You can add an item by `Add`.
```
JsonObject obj = new JsonObject();

// public void Add(string key, object? value);
obj.Add("key1", "value1");
obj.Add("key2", 0);
obj.Add("key3", true);
obj.Add("key4", null);
```
You can also add or modify an item by key.
```
JsonObject obj = new JsonObject();

// public object? this[string key]{ get; set; }
obj["key1"] = "value1";
obj["key1"] = "modified_value";
obj["key2"] = 0;
obj["key3"] = true;
```

#### Get value from a JsonObject
You can get a value in the specified type by a key by `Get`. You can provide a default value, and the default value will be returned when the key is not found or the specified type is not valid for the item.
```
JsonObject obj = new JsonObject() { { "key", 1.5 } };

// public T Get<T>(string key);
double v1 = obj.Get<double>("key"); // v1: 1.5

// public T Get<T>(string key, T defaultValue);
double v2 = obj.Get<double>("key", 0.0); // v2: 1.5
string v3 = obj.Get<string>("key", "default"); // v3: "default"
```
You can also get the JsonItem by key in these three ways, and then get value by `JsonItem.GetValue`.
```
JsonObject obj = new JsonObject() { { "key", 1.5 } };
JsonItem item;

// These three statements do the same thing
item = (JsonItem)obj["key"];
item = JsonItem.CreateFromValue(obj["key"]);
item = obj.GetValue<Dictionary<JsonString, JsonItem>>()["key"];

double v = item.GetValue<double>() // v: 1.5
```

You can get the JsonItem by path in multi-JsonObject/JsonArray by `GetByPath`. You can provide a default value, and the default value will be returned when the path is incorrect or the specified type is not valid for the item.
```
JsonObject obj = JsonObject.Parse("{\"key1\": {\"key2\": {\"key3\": 1}}}");

// The path will be split to keys by splitCharacter.
// Be sure that each key in the path does not contain splitCharacter.
// public T GetByPath<T>(string path, char splitCharacter = '.');
int v1 = obj.GetByPath<int>("key1.key2.key3"); // v1: 1

// If you don't know what character will never be contained by the keys, you can input an array as path.
// public T GetByPath<T>(string[] path);
int v2 = obj.GetByPath<int>(new string[]{"key1", "key2", "key3"}); // v2: 1

// public T GetByPath<T>(string path, T defaultValue, char splitCharacter = '.');
// public T GetByPath<T>(string[] path, T defaultValue);
int v3 = obj.GetByPath<int>("incorrect_path", 0); // v3: 0

```

#### Convert to string
You can convert a JsonObject to string by `ToString` or `ToFormattedString`.
```
JsonObject obj = new JsonObject()
{
    {"key1", "value1"},
    {"key2", 0},
    {"key3", true},
    {"key4", null}
};

// public string ToString();
string str1 = obj.ToString();
/* 
str1 is a string:
{"key1": "value1", "key2": 0, "key3": true, "key4": null}
*/

// public string ToFomattedString();
string str2 = obj.ToFormattedString();
/*
str2 is a string: 
{
    "key1": "value1",
    "key2": 0,
    "key3": true,
    "key4": null
}
*/
```

#### Convert to dictionary
You can convert a JsonObject to a dictionary with keys in string and values in a specified type by `ToDictionary`. Also, you can get a Dictionary value with `string` or `JsonString` keys from a JsonObject by `GetValue`. All values of the JsonObject should be in the specified type. 
```
JsonObject obj = new JsonObject()
{
    {"key1", 0},
    {"key2", 1},
    {"key3", 2},
    {"key4", 3}
};

// public Dictionary<string, T> ToDictionary<T>();
Dictionary<string, int> dict1 = obj.ToDictionary<int>();

// public T GetValue<T>();
Dictionary<string, int> dict2 = obj.GetValue<Dictionary<string, int>>();
```

#### Other fields and methods
JsonObject provides some fields and methods that are similar to Dictionary as following.
```
public string[] Keys;
public int Count;
public bool ContainsKey(string key);
public bool Remove(string key);
```

---

### JsonArray
#### Create a JsonArray
You can create a JsonArray in a similar way to create a List, or create a JsonArray by an array.
```
JsonArray arr1 = new JsonArray();
JsonArray arr2 = new JsonArray() { "value1", 0, true, null};
object?[] objects = new object?[]{ "value1", 0, true, null};
JsonArray arr3 = new JsonArray(objects);
```
You can also parse a string to a JsonArray by `Parse`.
```
string str = "[\"value1\", 1.5, true, null]";

// public static JsonArray Parse(string str);
JsonArray arr3 = JsonArray.Parse(str);
```

#### Set Value to a JsonArray
You can add an item by `Add`.
```
JsonArray arr = new JsonArray();

// public void Add(object? value);
arr.Add("value1");
arr.Add(1.5);
arr.Add(true);
```

You can modify an item by index.
```
JsonArray arr = new JsonArray() { "value1", 1.5, true, null };

// public object? this[int index]{ get; set; }
arr[1] = "modified_value";
```

#### Get value from a JsonArray
You can get a value in the specified type by a index by `Get`. You can provide a default value, and the default value will be returned when the index is out of range or the specified type is not valid for the item.
```
JsonArray arr = new JsonArray() { "value1", 1.5, true, null };

// public T Get<T>(int index);
double v1 = arr.Get<double>(0); // v1: 1.5

// public T Get<T>(int index, T defaultValue);
double v2 = obj.Get<double>(0, 0.0); // v2: 1.5
string v3 = obj.Get<string>(0, "default"); // v3: "default"
```

You can also get the JsonItem by index in these three ways, and then get value by `JsonItem.GetValue`.
```
JsonArray arr = new JsonArray() { 1.5 };
JsonItem item;

// These three statements do the same thing
item = (JsonItem)arr[0]
item = JsonItem.CreateFromValue(arr[0])
item = arr.GetValue<List<JsonItem>>()[0]

double v = item.GetValue<double>(); // v: 1.5
```
You can get the JsonItem by path in multi-JsonObject/JsonArray by `GetByPath`. You can provide a default value, and the default value will be returned when the path is incorrect or the specified type is not valid for the item.
```
JsonArray arr = JsonArray.Parse("[true, [{\"key1\": [12345]}], false]");

// The path will be split to keys by splitCharacter.
// Be sure that each key in the path does not contain splitCharacter.
// public T GetByPath<T>(string path, char splitCharacter = '.');
int v1 = arr.GetByPath<int>("1.0.key1.0"); // v1: 12345

// If you don't know what character will never be contained by the keys, you can input an array as path.
// public T GetByPath<T>(string[] path);
int v2 = arr.GetByPath<int>(new string[]{"1", "0", "key1", "0"}); // v2: 12345

// public T GetByPath<T>(string path, T defaultValue, char splitCharacter = '.');
// public T GetByPath<T>(string[] path, T defaultValue);
int v3 = arr.GetByPath<int>("incorrect_path", 0); // v3: 0
```

#### Convert to string
You can convert a JsonObject to string by `ToString` or `ToFormattedString`.
```
JsonArray arr = new JsonArray() { "value1", 1.5, true, null};

// public string ToString();
string str1 = arr.ToString();
/*
str1 is a string:
["value1", 1.5, true, null]
*/

// public string ToFormattedString();
string str2 = arr.ToFormattedString();
/*
str2 is a string:
[
    "value1",
    1.5,
    true,
    null
]
*/
```

#### Convert to list or array
You can convert a JsonArray to a list or an array with elements in a specified type by `ToList` or `ToArray`. Also, you can get a List or Array value from a JsonArray by `GetValue`. All elements of the JsonArray should be in the specified type.
```
JsonArray arr = new JsonArray{0, 1, 2, 3};

// public List<T> ToList<T>();
List<int> list1 = arr.ToList<int>();

// public T[] ToArray<T>();
int[] array1 = arr.ToArray<int>();

// public T GetValue<T>();
List<int> list2 = arr.GetValue<List<int>>();
int[] array2 = arr.GetValue<int[]>();
```

#### Other fields and methods
JsonArray provides some fields and methods that are similar to List as following.
```
public int Count;
public void RemoveAt(int index);
public void Insert(int index, object? item);
```

---
### IJsonObject
If you want your class to be supported as a json item, you can:
1. Implement interface IJsonObject with a method `public JsonObject ToJson();`"`;
2. Add a public constractor with a `JsonObject` argument to your class.

Example:
```
public class SomeClass(): IJsonObject
{
    public SomeClass(){}
    
    // This constractor is necessary
    public SomeClass(JsonObject json)
    {
        /* Load fileds from json */
    }
    
    public override JsonObject ToJson()
    {
        return new JsonObject()
        {
            /* Save fileds to json */
        };
    }
}

SomeClass obj1 = new SomeClass();

// "item" is a JsonObject object.
JsonItem item = JsonItem.CreateFromValue(obj1) as JsonObject;

// If no constructor "public SomeClass(JsonObject json);", an exception will be thrown.
SomeClass obj2 = item.GetValue<SomeClass>();
IJsonObject obj3 = item.GetValue<IJsonObject>();
```

Notice that we save the type of object by key `__Type`. Do not set value to this key in `ToJson()`, otherwise it will be overwritten.

---

### JsonConfig
Class `JsonConfig` provides some configs.
#### EnsureAscii
If `EnsureAscii` is true, all non-ascii characters will be escaped by \u in `ToString` and `ToFormattedString`.
```
// public static bool EnsureAscii = false;

JsonItem item = JsonItem.CreateFromValue("\u5000");
JsonConfig.EnsureAscii = true;
string str1 = item.ToString(); // str1: @"\u5000"
JsonConfig.EnsureAscii = false;
string str2 = item.ToString(); // str2: "倀"
```

---

### Exceptions
#### JsonInvalidTypeException
A JsonInvalidTypeException will be thrown when getting value with an invalid type and the default value is not provided.
```
JsonItem item = JsonItem.CreateFromValue("This is not an integer");
int v = item.GetValue<int>(); // A JsonInvalidTypeException will be thrown.
```
A JsonInvalidTypeException will be thrown when creating JsonItem by a value with an unsupported type.
```
class SomeClass{}
JsonIten item = JsonItem.CreateFromValue(new SomeClass()); // A JsonInvalidTypeException will be thrown.
```
A JsonInvalidTypeException will be thrown when creating JsonObject with a non-empty dictionary whose keys are not `string` or `JsonString`.
```
Dictionary<int, int> dict = new Dictionary<int, int>();
dict.Add(1, 1);
new JsonObject(dict);
```
A JsonInvalidTypeException will be thrown when converting a JsonObject to a dictionary or converting a JsonArray to a list or an array, while not all the elements/values are in the specified type.
```
JsonArray arr = new JsonArray() {0, 1, "This is not an integer", 3, 4};
arr.ToList<int>(); // A JsonInvalidTypeException will be thrown.
```

A JsonInvalidType Exception will be thrown when getting a IJsonObject value from a JsonObject, but the JsonObject does not have "__Type" value to indicate the target class, or the target class is not found in the calling assembly, or the target class dose not have a public constructor with a JsonObject argument.
```
class SomeClass: IJsonObject{
    // No constructor: public SomeClass(JsonObject json);
}
JsonObject json = new JsonObject();
// No __Type value: json["__Type"] = "SomeNameSpace.SomeClass";
SomeClass someclass = json.GetValue<SomeClass>(); // A JsonInvalidTypeException will be thrown.
```

#### JsonFormatException
A JsonFormatException will be thrown when parsing an invalid string.
```
string str = "This string cannot be parsed to JsonObject";
JsonObject obj = JsonObject.Parse(str); // A JsonFormatException will be thrown.
```

#### JsonInvalidPathException
A JsonInvalidPathException will be thrown when calling `GetByPath` with an invalid path and the default value is not provided.
```
JsonObject obj = new JsonObject();
int value = obj.GetByPath("invalid path"); // A JsonInvalidPathException will be thrown.
```

### Notice
1. In JsonObject, we use `Dictionary<JsonString, JsonItem>` to store items. If you create JsonObject objects with the same `Dictionary<JsonString, JsonItem>`, their items are stored in the same reference:
```
Dictionary<JsonString, JsonItem> dict = new Dictionary<JsonString, JsonItem>() 
{ 
    { new JsonString("key") , JsonItem.CreateFromValue(0)} 
};
JsonObject json1 = new JsonObject(dict);
JsonObject json2 = new JsonObject(dict);
Console.WriteLine(json1); // {"key": 0}
json2["key"] = 1;
Console.WriteLine(json1); // {"key": 1}
```
In JsonArray, we use `List<JsonItem>` to store items. If you create JsonArray objects with the same `List<JsonItem>`, their items are stored in the same reference:
```
List<JsonItem> list = new List<JsonItem>() { JsonItem.CreateFromValue(0)};
JsonArray arr1 = new JsonArray(list);
JsonArray arr2 = new JsonArray(list);
Console.WriteLine(arr1); // [0]
arr2[0] = 1;
Console.WriteLine(arr1); // [1]
```
2. When you parse a number to JsonItem by `JsonItem.Parse`, a number who equals to an integer will be seemed as an integer:
```
JsonItem item1 = JsonItem.Parse("1.0"); // 1.0 equals to an integer 1
int value1 = item1.GetValue<int>(); // value1: 1

JsonItem item2 = JsonItem.Parse("1.1"); // 1.1 is not an integer
int value2 = item2.GetValue<int>(); // A JsonInvalidTypeException will be thrown
```
However, if you use `JsonItem.CreateFromValue` with a float value to create an item, or use `new JsonFloat` or `JsonFloat.Parse` to create an item, the item will be seemed as a non-integer item, even if its value equals to an integer:
```
JsonItem item1 = JsonItem.CreateFromValue(1.0);
int value1 = item1.GetValue<int>(); // A JsonInvalidTypeException will be thrown

JsonItem item2 = new JsonFloat(1);
int value2 = item2.GetValue<int>(); // A JsonInvalidTypeException will be thrown

JsonItem item3 = JsonFloat.Parse("1");
int value3 = item2.GetValue<int>(); // A JsonInvalidTypeException will be thrown
```


## Author
[FSPolarBear](https://github.com/FSPolarBear)