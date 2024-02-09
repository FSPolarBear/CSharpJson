# CSharpJson
This is a json library for C#. 
## Version
1.0.1
## Support
.Net 6.0
## Usage
To use this library, import json.dll or copy the code of this project to your project, and then
``` 
using Json
```
We provide class ```JsonObject``` as the object in json and class ```JsonArray``` as the array in json. We use class ```JsonItem``` as the item in json.
### Supported type of item
These types are supported as an item of json:

| Json type | C# type                                  |
|:----------|:-----------------------------------------|
| object    | JsonObject                               |
| array     | JsonArray                                |
| string    | string                                   |
| number (integer)    | long, int, short, decimal, double, float |
| number    | decimal, double, float |
| true      | bool                                     |
| false     | bool                                     |
| null      | null                                     |

### JsonItem
#### Create a JsonItem
You can create a JsonItem from a value by ```CreateFromValue``` or parse a string to a JsonItem by ```Parse```.
```
//public static JsonItem CreateFromValue(object? value);
JsonItem item1 = JsonItem.CreateFromValue("value1");
JsonItem item2 = JsonItem.CreateFromValue(1.5);
JsonItem item3 = JsonItem.CreateFromValue(true);

//public static JsonItem Parse(string str);
JsonItem item4 = JsonItem.CreateFromValue("\"value1\"");
JsonItem item5 = JsonItem.CreateFromValue("1.5");
JsonItem item6 = JsonItem.CreateFromValue("true");
```

#### Get value from a JsonItem
You can get a value in the specified type by ```GetValue```. You can provide a default value, and the default value will be return when the specified type is not valid for the item.
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
You can also parse a string to a JsonObject by ```Parse```.
```
string str = "{\"key1\": \"value1\", \"key2\": 0, \"key3\": true, \"key4\": null}";

// public static JsonObject Parse(string str);
JsonObject obj3 = JsonObject.Parse(str);
```

#### Set value to a JsonObject
You can add an item by ```Add```.
```
JsonObject obj = new JsonObject();

// public void Add(string key, object? value);
obj.Add("key1", "value1");
obj.Add("key2", 0);
obj.Add("key3", true);
obj.Add("key4", null);
```
You can also add or modify an item by index.
```
JsonObject obj = new JsonObject();
obj["key1"] = "value1";
obj["key1"] = "modified_value";
obj["key2"] = 0;
obj["key3"] = true;
```

#### Get value from a JsonObject
You can get a value in the specified type by a key by ```Get```. You can provide a default value, and the default value will be return when the key is not found or the specified type is not valid for the item.
```
JsonObject obj = new JsonObject() { { "key", 1.5 } };

// public T Get<T>(string key);
double v1 = obj.Get<double>("key"); // v1: 1.5

// public T Get<T>(string key, T defaultValue);
double v2 = obj.Get<double>("key", 0.0); // v2: 1.5
string v3 = obj.Get<string>("key", "default"); // v3: "default"
```
You can also get the JsonItem by index and then get value by ```JsonItem.GetValue```.
```
JsonObject obj = new JsonObject() { { "key", 1.5 } };

double v1 = ((JsonItem)obj["key"]).GetValue<double>(); // v1: 1.5
double v2 = JsonItem.CreateFromValue(obj["key"]).GetValue<double>(); // v2: 1.5
```

#### Convert to string
You can convert a JsonObject to string by ```ToString``` or ```ToFormattedString```.
```
JsonObject obj = new JsonObject()
{
    {"key1", "value1"},
    {"key2", 0 },
    {"key3", true},
    {"key4", null}
};

// public string ToString();
string str1 = obj.ToString();
/* 
str1 is a string:
 { "key1": "value1", "key2": 0, "key3": true, "key4": null}
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
You can create a JsonArray in a similar way to create a List.
```
JsonArray arr1 = new JsonArray();
JsonArray arr2 = new JsonArray() { "value1", 0, true, null};
```
You can also parse a string to a JsonArray by ```Parse```.
```
string str = "[\"value1\", 1.5, true, null]";

// public static JsonArray Parse(string str);
JsonArray arr3 = JsonArray.Parse(str);
```

#### Set Value to a JsonArray
You can add an item by ```Add```.
```
JsonArray arr = new JsonArray();
arr.Add("value1");
arr.Add(1.5);
arr.Add(true);
```
You can modify an item by index.
```
JsonArray arr = new JsonArray() { "value1", 1.5, true, null };
arr[1] = "modified_value";
```

#### Get value from a JsonArray
You can get a value in the specified type by a index by ```Get```. You can provide a default value, and the default value will be return when the index is out of range or the specified type is not valid for the item.
```
JsonArray arr = new JsonArray() { "value1", 1.5, true, null };

// public T Get<T>(int index);
double v1 = arr.Get<double>(0); // v1: 1.5

// public T Get<T>(int index, T defaultValue);
double v2 = obj.Get<double>(0, 0.0); // v2: 1.5
string v3 = obj.Get<string>(0, "default"); // v3: "default"
```

You can also get the JsonItem by index and then get value by ```JsonItem.GetValue```.
```
JsonArray arr = new JsonArray() { 1.5 };

double v1 = ((JsonItem)arr[0]).GetValue<double>(); // v1: 1.5
double v2 = JsonItem.CreateFromValue(arr[0]).GetValue<double>(); // v2: 1.5
```

#### Convert to string
You can convert a JsonObject to string by ```ToString```.
```
JsonObject obj = new JsonArray() { "value1", 1.5, true, null};

// public string ToString();
string str = obj.ToString();
/*
str is a string:
["value1", 1.5, true, null]
*/
```

---

### Exceptions
#### JsonInvalidTypeException
A JsonInvalidTypeException will be thrown when getting value with an invalid type.
```
JsonItem item = JsonItem.CreateFromValue("This is not an integer");
int v = item.GetValue<int>(); // A JsonInvalidTypeException will be thrown.
```
A JsonInvalidTypeException will be thrown when creating JsonItem by a value with unsupported type.
```
class SomeClass{}
JsonIten item = JsonItem.CreateFromValue(new SomeClass()); // A JsonInvalidTypeException will be thrown.
```
#### JsonFormatException
A JsonFormatException will be thrown when parsing an invalid string.
```
string str = "This string cannot be parsed to JsonObject";
JsonObject obj = JsonObject.Parse(str); // A JsonFormatException will be thrown.
```

## Author
[FSPolarBear](https://github.com/FSPolarBear)