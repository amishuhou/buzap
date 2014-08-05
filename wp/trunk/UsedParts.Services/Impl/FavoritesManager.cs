using System;
using System.Collections.Generic;
using System.Linq;
using UsedParts.Common;

namespace UsedParts.Services.Impl
{
    public class FavoritesManager<T> : IFavoritesManager<T> where T : class
    {
        private readonly ISettings _settings;
        private static string SettingsKey
        {
            get { return string.Format("favorites_{0}", typeof(T).Name); }
        }

        public event EventHandler<FavoritesEventArgs<T>> ItemChanged;

        public FavoritesManager(ISettings settings)
        {
            _settings = settings;
        }

        public void Add(T item)
        {
            if (item == null)
                return;
            var list = GetAll();
            if (list.Contains(item))
                return;
            list.Add(item);
            Save(list);
            OnItemChanged(item, true);
        }

        public void Remove(T item)
        {
            if (item == null)
                return;
            var list = GetAll();
            if (!list.Contains(item))
                return;
            list.Remove(item);
            Save(list);
            OnItemChanged(item, false);
        }

        public bool IsFavorite(T item)
        {
            return item != null && GetAll().Contains(item);
        }

        public IList<T> GetAll()
        {
            var list = _settings.GetValue<IList<T>>(SettingsKey);
            return list ?? new List<T>();
        }

        private void OnItemChanged (T item, bool isFavorite)
        {
            if (ItemChanged != null)
                ItemChanged(this, new FavoritesEventArgs<T> {Item = item, IsFavorite = isFavorite});
        }

        private void Save(IEnumerable<T> list)
        {
            _settings.SetValue(SettingsKey, list.ToList());
        }


    }
}
