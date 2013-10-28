using metricstdd_lib;
using System;
using System.Collections.Generic;

namespace metricstdd
{
    public class Widget
    {
        public DateTime UpdatedAt { get; private set; }
        public int Number { get; private set; }

        public Widget(IWidgetService service)
        {
            _widgetService = service;
        }

        public void Init()
        {
            _fields = new Dictionary<string, string[]>();
            try
            {
                var cachedNumber = _widgetService.GetNumberFromCache();
                Number = cachedNumber.Value;
                UpdatedAt = cachedNumber.UpdatedAt;
            }
            catch (Exception)
            {
                Number = 0;
                UpdatedAt = DateTime.MinValue;
            }
        }

        public void Update()
        {
            CachedNumber cachedNumber;
            try
            {
                cachedNumber = _widgetService.GetNumberFromCache();
            }
            catch (Exception)
            {
                cachedNumber = new CachedNumber();   
            }

            Number = _widgetService.GetNumber();
            UpdatedAt = DateTime.Now;
            cachedNumber.Value = Number;
            cachedNumber.UpdatedAt = DateTime.Now;
            _widgetService.UpdateNumberInCache(cachedNumber);
        }

        public void AddField(string key, params string[] values)
        {
            _fields.Add(key, values);
        }

        public Dictionary<string, string[]> GetFields()
        {
            return _fields;
        }

        private IWidgetService _widgetService;

        private Dictionary<string, string[]> _fields;

    }
}
