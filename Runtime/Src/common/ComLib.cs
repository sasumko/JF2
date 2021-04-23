using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using LitJson;
using System.Collections;
using System;
using System.IO;
using System.IO.Compression;


namespace JFrame
{
    public class ComLib
    {
        public static void SetActiveByName(Transform _tr, bool _bActive, string _name = "")
        {
            if (_tr == null)
            {
                return;
            }

            Transform _target = null;

            if (string.IsNullOrEmpty(_name) == true)
            {
                _target = _tr;
            }
            else
            {
                _target = _tr.Find(_name);
            }
            
            if (_target == null)
            {
                return;
            }

            _target.gameObject.SetActive(_bActive);
        }

        public static bool HasKeyInJson(JsonData _data, string _key)
        {
            if (_data == null || string.IsNullOrEmpty(_key) == true)
            {
                return false;
            }

            if (_data.IsObject == false)
            {
                return false;
            }

            IDictionary tdictionary = _data as IDictionary;

            bool _ret = tdictionary != null && tdictionary.Contains(_key) == true;

            return _ret;
        }

        public static JsonData GetJsonDataByKey(JsonData _data, string _key)
        {
            if (HasKeyInJson(_data, _key) == false)
            {
                return null;
            }

            return _data[_key];
        }

        public static T GetUIByPath<T> (Transform _tr, string _path) where T : MonoBehaviour        {
            if (_tr == null)
            {
                return default(T);
            }

            if (string.IsNullOrEmpty(_path) == false)
            {
                _tr = _tr.Find(_path);
                if (_tr == null)
                {
                    return default(T);
                }
            }

            return _tr.GetComponent<T>();

        }

        public static void SetLabel (Text _txt, string _str)
        {
            if (_txt == null)
            {
                return;
            }

            _txt.text = _str;
        }

        //########################
        //####  TIME UTILS

        public static long GetTimestamp ()
        {
            return GetTimestamp(DateTime.UtcNow);
        }

        public static long GetTimestamp (DateTime _dateTime)
        {
            DateTime _orgin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            TimeSpan _timespan = _dateTime - _orgin;

            return (long) _timespan.TotalSeconds;
        }


        public static long GetTimePassedSeconds (DateTime _startDate, DateTime _endDate)
        {
            TimeSpan _diff = _endDate - _startDate;

            return (long)_diff.TotalSeconds;
        }

        public static bool IsTimePassedByDay (DateTime _startDate, int _dayCount)
        {
            return IsTimePassedBySec(_startDate, 60 * 60 * 24 * _dayCount);
        }

        public static bool IsTimePassedByHour (DateTime _startDate, int _minCount)
        {
            return IsTimePassedBySec(_startDate, 60 * 60 * _minCount);
        }

        public static bool IsTimePassedByMin (DateTime _startDate, int _minCount)
        {
            return IsTimePassedBySec(_startDate, 60 * _minCount);
        }

        public static bool IsTimePassedBySec (DateTime _startDate, int _secCount)
        {
            long _passedSec = GetTimePassedSeconds(_startDate, DateTime.UtcNow);

            return _passedSec >= _secCount;
        }

        public static Byte[] Decompress(Byte[] buffer)
        {
            MemoryStream resultStream = new MemoryStream();

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                using (GZipStream ds = new GZipStream(ms, CompressionMode.Decompress))
                {
                    ds.CopyTo(resultStream);
                    ds.Close();
                }
            }
            Byte[] decompressedByte = resultStream.ToArray();
            resultStream.Dispose();
            return decompressedByte;
        }
    }
}
